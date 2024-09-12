using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using Random = UnityEngine.Random;

namespace SlimeProject
{
    public struct Weapon
    {
        public Sprite Sprite;
        public ItemType Type;
        public int Level;
        public float Damage;
        public float Interval;
        public float Speed;

        public float CurrentTime;
        public Weapon(ItemScriptableObject so)
        {
            Sprite = so.Sprite;
            switch (so.type)
            {
                case ItemType.Bow:
                    Damage = 10f;
                    Interval = 2f;
                    Type = ItemType.Bow;
                    Speed = 5f;
                    break;
                case ItemType.Saw:
                    Damage = 10f;
                    Interval = 0.1f;
                    Type = ItemType.Saw;
                    Speed = 5f;
                    break;
                default:
                    Damage = 0;
                    Interval = 0;
                    Speed = 0;
                    Type = ItemType.None;
                    break;
            }
            Level = 0;
            Damage += Damage * so.Damage;
            Speed += Speed * so.Speed;
            CurrentTime = 0;
        }
        public void Update(ItemScriptableObject so)
        {
            Damage += so.Damage;
            switch (so.type)
            {
                case ItemType.Bow:
                    Interval -= so.Interval;
                    if (Interval < 0.05f)
                        Interval = 0.1f;
                    break;
                case ItemType.Saw:
                    break;
                default:
                    break;
            }
            if(Level < 5)
                Level += 1;
            
            Speed += so.Speed;
        }

        public void TimerUpdate()
        {
            CurrentTime += Time.deltaTime;
        }

    }
    public class Player : Character
    {
        public float startPositionY = 0;
        public Vector3 CheckOffset = Vector3.zero;
        public LayerMask PanelLayer;

        public Arrow ArrowPrefab;
        private Arrow[] arrowPools = new Arrow[50];
        private int arrowCurrent = 0;


        private Dictionary<ItemType, Weapon> weapons = new Dictionary<ItemType, Weapon>();
        
        [Space(20f)]
        public Color AttackColor = Color.white; 

        // Start is called before the first frame update
        void Start()
        {
            Hp = CharacterSO.Hp;
            Maxhp = CharacterSO.Hp;
            Damage = CharacterSO.Damage;
            AttackInterval = CharacterSO.AtkInterval;
            MainColor = MainSprite.color;
            UIManager.Instance.UpdateHpBar(Maxhp, Hp);
        }

        // Update is called once per frame
        void Update()
        {
            curAtkTimer += Time.deltaTime;
            curAnimTimer += Time.deltaTime;
            PlayAnim();
            if(!FieldManager.Instance.OpenUI)
                HandleDrag();
            // 정면 충돌이 일어날 경우
            if (UpCheck())
            {
                NormalAttack();
                FieldManager.Instance.Scrolling = false;
            }
            else if (SelectorCheck())
            {
                ActivatedPanel();
            }
            else if (!FieldManager.Instance.OpenUI)
            {
                FieldManager.Instance.Scrolling = true;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            Gizmos.DrawWireCube(transform.position + transform.rotation * CheckOffset + Vector3.up * 0.5f, Vector3.one * 0.5f);
        }
        public void WeaponAttack()
        {
            foreach(var weapon in weapons)
            {
                switch (weapon.Value.Type)
                {
                    case ItemType.Bow:
                        weapon.Value.TimerUpdate();
                        break;
                    case ItemType.Saw:
                        break;
                    default:
                        break;
                }
            }
        }

        public void ActivatedItem(ItemScriptableObject itemSO)
        {
            switch (itemSO.type)
            {
                case ItemType.None:
                    // 회복을 포함한 다양한 기능
                    break;
                case ItemType.Shop:
                    // 상점일 경우 게임을 잠시 멈추고 쇼핑 시작.
                    UIManager.Instance.OpenShop();
                    break;
                case ItemType.Sword:
                    // 기본 공격력 증가
                    Damage += Damage * itemSO.Damage;
                    AttackInterval -= AttackInterval * itemSO.Interval;
                    UIManager.Instance.GetItem(itemSO);
                    break;
                case ItemType.Bow:
                    WeaponUpdate(itemSO, ItemType.Bow);
                    UIManager.Instance.GetItem(itemSO);
                    // 활 오브젝트 발사, 적중 시 피해
                    break;
                case ItemType.Saw:
                    WeaponUpdate(itemSO, ItemType.Bow);
                    UIManager.Instance.GetItem(itemSO);
                    // 톱니 오브젝트 생성 및 적중 시 피해.
                    break;
                case ItemType.Crystal:
                    // 돈 추가
                    int rand = Random.Range(0, 10);
                    if (rand < 5 || !itemSO.IsRandom)
                        UIManager.Instance.AddCryStal(itemSO.AddCryStal);

                    break;
                default:
                    break;
            }
        }

        private void ActivatedPanel()
        {
            ItemScriptableObject so = Colliders[0].GetComponent<Interact>().Activated();
            Colliders[0].gameObject.SetActive(false);
            ActivatedItem(so);
        }

        private async UniTaskVoid BowAttack(Weapon weaponInfo)
        {
            try
            {
                // 게임이 끝나면 종료
                while (true)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(weaponInfo.Interval));
                    if (!FieldManager.Instance.OpenUI)
                    {
                        arrowPools[arrowCurrent].gameObject.SetActive(true);
                        arrowPools[arrowCurrent].SetArrow(weaponInfo.Damage, weaponInfo.Speed, targetLayer);
                        arrowPools[arrowCurrent].transform.position = transform.position;
                        arrowCurrent++;
                        if (arrowCurrent >= arrowPools.Length)
                        {
                            arrowCurrent = 0;
                        }
                    }
                }
            }
            catch
            {
                Debug.Log("UniTask 강제 종료");
            }
        }
        
        private void CreateArrowPools()
        {
            for(int i =0; i < arrowPools.Length; i++)
            {
                arrowPools[i] = Instantiate(ArrowPrefab.gameObject).GetComponent<Arrow>();
                arrowPools[i].gameObject.SetActive(false);
            }
        }
        
        private void WeaponUpdate(ItemScriptableObject so, ItemType type)
        {
            if (!weapons.ContainsKey(type))
            {
                weapons.Add(type, new Weapon(so));
                switch (type)
                {
                    case ItemType.Bow:
                        CreateArrowPools();
                        BowAttack(weapons[ItemType.Bow]).Forget();
                        break;
                    case ItemType.Saw:
                        break;
                    default:
                        break;
                }

            }
            else
            {
                weapons[type].Update(so);
            }
        }

        private bool SelectorCheck()
        {
            Vector2 boxCenter = transform.position + transform.rotation * CheckOffset;

            Colliders = Physics2D.OverlapBoxAll(boxCenter, Vector3.one * 0.5f, 0f, PanelLayer);

            // 충돌한 콜라이더가 있는 경우 true 반환
            return Colliders.Length > 0 ? true : false;

        }
        private bool UpCheck()
        {
            // 박스의 중심 위치 설정
            Vector2 boxCenter = transform.position + transform.rotation * CheckOffset + Vector3.up * 0.5f;

            // 박스 영역 내의 충돌체 탐색 (colliders 배열에 충돌한 콜라이더 저장)
            Colliders = Physics2D.OverlapBoxAll(boxCenter, Vector3.one * 0.5f, 0f, targetLayer);

            // 충돌한 콜라이더가 있는 경우 true 반환
            return Colliders.Length > 0 ? true : false;
        }


        private bool LeftCheck()
        {
            Debug.DrawRay(transform.position + Vector3.up * 0.25f, new Vector3(-1, 0, 0) * 0.5f, new Color(0, 1, 0));
            Debug.DrawRay(transform.position - Vector3.up * 0.25f, new Vector3(-1, 0, 0) * 0.5f, new Color(0, 1, 0));
            Debug.DrawRay(transform.position, new Vector3(-1, 0, 0) * 0.5f, new Color(1, 0, 0));

            RaycastHit2D luHit = Physics2D.Raycast(transform.position + Vector3.up * 0.3f, -transform.right, 0.5f, targetLayer);
            RaycastHit2D ldHit = Physics2D.Raycast(transform.position - Vector3.up * 0.3f, -transform.right, 0.5f, targetLayer);
            RaycastHit2D lcHit = Physics2D.Raycast(transform.position, -transform.right, 0.5f, targetLayer);

            return luHit || ldHit || lcHit;
        }

        private bool RightCheck()
        {

            Debug.DrawRay(transform.position + Vector3.up * 0.25f, new Vector3(1, 0, 0) * 0.5f, new Color(1, 0, 0));
            Debug.DrawRay(transform.position - Vector3.up * 0.25f, new Vector3(1, 0, 0) * 0.5f, new Color(1, 0, 0));
            Debug.DrawRay(transform.position, new Vector3(1, 0, 0) * 0.5f, new Color(1, 0, 0));

            RaycastHit2D ruHit = Physics2D.Raycast(transform.position + Vector3.up * 0.3f, transform.right, 0.5f, targetLayer);
            RaycastHit2D rdHit = Physics2D.Raycast(transform.position - Vector3.up * 0.3f, transform.right, 0.5f, targetLayer);
            RaycastHit2D rcHit = Physics2D.Raycast(transform.position, transform.right, 0.5f, targetLayer);

            return ruHit || rdHit || rcHit;
        }

        public override void PlayAnim()
        {
            if (curAnimTimer > AnimPlayTime)
            {
                PlaySpriteAnimation();
                curAnimTimer = 0;
            }
        }

        public override void Damaged(float damage)
        {
            base.Damaged(damage);

            UIManager.Instance.UpdateHpBar(Maxhp, Hp );
        }

        private Vector3 offset;
        private float zCoord;

        private void HandleDrag()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                HandleTouch(touch);
            }

            // Editor에서 테스트할 때 마우스 입력 처리
            if (Input.GetMouseButton(0))
            {
                HandleMouse();
            }
        }

        private void HandleTouch(Touch touch)
        {
            if (touch.phase == TouchPhase.Began)
            {
                // 터치 시작 위치
                zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
                offset = gameObject.transform.position - GetTouchWorldPosition(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                // 터치 이동 시 캐릭터 위치 업데이트
                transform.position = Vector3.MoveTowards(transform.position, (Vector3.right * (GetMouseWorldPosition() + offset).x + Vector3.up * startPositionY), 5 * Time.deltaTime);
                //transform.position = Vector3.right * (GetTouchWorldPosition(touch.position) + offset).x + Vector3.up * startPositionY;
            }
        }

        private void HandleMouse()
        {
            Vector3 touchPoint = (Vector3.right * (GetMouseWorldPosition() + offset).x + Vector3.up * startPositionY);
            Vector3 direction = touchPoint - transform.position;
            direction.Normalize();
            if (direction.x < 0 && LeftCheck())
            {
                return;
            }
            else if (direction.x > 0 && RightCheck())
            {
                return;
            }

            transform.position = Vector3.MoveTowards(transform.position, touchPoint, 5 * Time.deltaTime);
            //transform.position = Vector3.right *( GetMouseWorldPosition() + offset).x + Vector3.up * startPositionY;
        }

        private Vector3 GetTouchWorldPosition(Vector2 touchPosition)
        {
            // 터치 위치를 월드 좌표로 변환
            Vector3 touchWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, zCoord));
            return touchWorldPosition;
        }

        private Vector3 GetMouseWorldPosition()
        {
            // 마우스 위치를 월드 좌표로 변환
            Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zCoord);
            return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        }

    }

}