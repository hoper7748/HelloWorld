using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeProject
{
    public class BossMonster : Monster
    {
        private FSMController controller;

        public Arrow ArrowPrefab;

        private void Awake()
        {
            controller = new FSMController(this);

            Hp = CharacterSO.Hp;
            Maxhp = CharacterSO.Hp;
            Damage = CharacterSO.Damage;
            AttackInterval = CharacterSO.AtkInterval;
            MainColor = MainSprite.color;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(Hp < 0)
            {
                FieldManager.Instance.BossSpawn = false;
            }
            curAnimTimer += Time.deltaTime;
            curAtkTimer += Time.deltaTime;
            PlayAnim();
            ScrollingCharacter();

            if (DownCheck())
            {
                NormalAttack();
            }

            controller.StateUpdate();
        }

        private void OnEnable()
        {
            // 생성
        }

        private void OnDisable()
        {
            // 삭제
            FieldManager.Instance.BossSpawn = false;
        }


        public override void PlayAnim()
        {
            if (curAnimTimer > AnimPlayTime)
            {
                PlaySpriteAnimation();
                curAnimTimer = 0;
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

    }
}
