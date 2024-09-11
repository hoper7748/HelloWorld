using UnityEngine;

namespace SlimeProject
{
    public class Monster : Character
    {

        private void Awake()
        {

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // 충돌 체크는 여기서
            PlayAnim();
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
        
        private void PlayAnim()
        {
            curAnimTimer += Time.deltaTime;
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
        }


    }

}