using System.Linq;
using UnityEngine;

namespace SlimeProject
{
    public class Player : Character
    {
        public float startPositionY = 0;
        public Vector3 CheckOffset = Vector3.zero;
        public LayerMask PanelLayer;

        [Space(20f)]
        public Color AttackColor = Color.white; 

        // Start is called before the first frame update
        void Start()
        {
            Hp = CharacterSO.Hp;
            Maxhp = CharacterSO.Hp;
            Damage = CharacterSO.Damage;
            AttackInterval = CharacterSO.AtkInterval;
        }

        // Update is called once per frame
        void Update()
        {
            curAtkTime += Time.deltaTime;
            curAnimTimer += Time.deltaTime;
            PlayAnim();
            if(!FieldManager.Instance.Shopping)
                HandleDrag();
            else
            {
                // 정면 충돌이 일어날 경우
                if (UpCheck())
                {
                    Attack();
                    FieldManager.Instance.Scrolling = false;
                }
                else if (SelectorCheck())
                {
                    Debug.Log($"오");
                }
                else if(!FieldManager.Instance.Shopping)
                {
                    FieldManager.Instance.Scrolling = true;
                }
            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            Gizmos.DrawWireCube(transform.position + transform.rotation * CheckOffset + Vector3.up * 0.5f, Vector3.one * 0.5f);
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

            RaycastHit2D luHit = Physics2D.Raycast(transform.position + Vector3.up * 0.5f, -transform.right, 0.5f, targetLayer);
            RaycastHit2D ldHit = Physics2D.Raycast(transform.position - Vector3.up * 0.5f, -transform.right, 0.5f, targetLayer);
            RaycastHit2D lcHit = Physics2D.Raycast(transform.position, -transform.right, 0.5f, targetLayer);

            return luHit || ldHit || lcHit;
        }

        private bool RightCheck()
        {

            Debug.DrawRay(transform.position + Vector3.up * 0.25f, new Vector3(1, 0, 0) * 0.5f, new Color(1, 0, 0));
            Debug.DrawRay(transform.position - Vector3.up * 0.25f, new Vector3(1, 0, 0) * 0.5f, new Color(1, 0, 0));
            Debug.DrawRay(transform.position, new Vector3(1, 0, 0) * 0.5f, new Color(1, 0, 0));

            RaycastHit2D ruHit = Physics2D.Raycast(transform.position + Vector3.up * 0.5f, transform.right, 0.5f, targetLayer);
            RaycastHit2D rdHit = Physics2D.Raycast(transform.position - Vector3.up * 0.5f, transform.right, 0.5f, targetLayer);
            RaycastHit2D rcHit = Physics2D.Raycast(transform.position, transform.right, 0.5f, targetLayer);

            return ruHit || rdHit || rcHit;
        }

        private void PlayAnim()
        {
            if (curAnimTimer > AnimPlayTime)
            {
                PlaySpriteAnimation();
                curAnimTimer = 0;
            }
        }

        private Vector3 offset;
        private float zCoord;

        void HandleDrag()
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

        void HandleTouch(Touch touch)
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

        void HandleMouse()
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

        Vector3 GetTouchWorldPosition(Vector2 touchPosition)
        {
            // 터치 위치를 월드 좌표로 변환
            Vector3 touchWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, zCoord));
            return touchWorldPosition;
        }

        Vector3 GetMouseWorldPosition()
        {
            // 마우스 위치를 월드 좌표로 변환
            Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zCoord);
            return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        }

    }

}