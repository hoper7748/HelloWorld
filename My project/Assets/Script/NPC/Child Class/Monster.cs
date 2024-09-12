using UnityEngine;

namespace SlimeProject
{
    public class Monster : Character
    {

        public Vector3 CheckOffset = Vector3.zero;
        private void Awake()
        {

        }

        // Start is called before the first frame update
        void Start()
        {
            Hp = CharacterSO.Hp;
            Maxhp = CharacterSO.Hp;
            Damage = CharacterSO.Damage;
            AttackInterval = CharacterSO.AtkInterval;
            MainColor = MainSprite.color;
        }

        // Update is called once per frame
        void Update()
        {
            curAnimTimer += Time.deltaTime;
            curAtkTimer += Time.deltaTime;
            // 충돌 체크는 여기서
            PlayAnim();
            ScrollingCharacter();
            if(DownCheck())
            {
                NormalAttack();
            }
        }

        public override void ScrollingCharacter()
        {
            // 이동
            if (FieldManager.Instance.Scrolling)
            {
                transform.position += Vector3.down * FieldManager.Instance.ScrollSpeed * Time.deltaTime;
                if (transform.position.y < -5)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        public override void PlayAnim()
        {
            if (curAnimTimer > AnimPlayTime)
            {
                PlaySpriteAnimation();
                curAnimTimer = 0;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider2D>().size* 1.5f);

            Gizmos.color = Color.green;

            Gizmos.DrawWireCube(transform.position + transform.rotation * CheckOffset + Vector3.down * 0.5f, Vector3.one * 0.5f);

        }


        public bool DownCheck()
        {

            // 박스의 중심 위치 설정
            Vector2 boxCenter = transform.position + transform.rotation * CheckOffset + Vector3.down * 0.5f;

            // 박스 영역 내의 충돌체 탐색 (colliders 배열에 충돌한 콜라이더 저장)
            Colliders = Physics2D.OverlapBoxAll(boxCenter, Vector3.one * 0.5f, 0f, targetLayer);

            // 충돌한 콜라이더가 있는 경우 true 반환
            return Colliders.Length > 0 ? true : false;
        }

    }

}