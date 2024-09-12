using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeProject
{
    public class UpgradeMonster : Monster
    {
        [Header("슬라임 무기")]
        public Arrow FirePrefab;
        public float FireArrowSpeed = 0;
        [ReadOnly]
        public Arrow[] FireArrowPools;
        
        private int arrowCurrent;

        private void Awake()
        {
            FireArrowPools = new Arrow[5];
            for(int i =0; i < 5; i++)
            {
                FireArrowPools[i] = Instantiate(FirePrefab);
                FireArrowPools[i].gameObject.SetActive(false);
            }
        }

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
            curAnimTimer += Time.deltaTime;
            curAtkTimer += Time.deltaTime;
            PlayAnim();
            ScrollingCharacter();
            ShootFire();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            Gizmos.DrawWireCube(transform.position + transform.rotation * CheckOffset + Vector3.down * 0.5f, Vector3.one * 0.5f);
        }

        public override void ScrollingCharacter()
        {
            base.ScrollingCharacter();
        }

        public override void PlayAnim()
        {
            if (curAnimTimer > AnimPlayTime)
            {
                PlaySpriteAnimation();
                curAnimTimer = 0;
            }
        }

        public void ShootFire()
        {
            // 불 발싸!!!
            if (curAtkTimer > AttackInterval)
            {
                FireArrowPools[arrowCurrent].gameObject.SetActive(true);
                FireArrowPools[arrowCurrent].SetArrow(Damage, FireArrowSpeed, targetLayer);
                FireArrowPools[arrowCurrent].transform.position = transform.position;
                curAtkTimer = 0;
            }

        }

        private void OnEnable()
        {

        }

        public void Attack()
        {

        }
    }

}