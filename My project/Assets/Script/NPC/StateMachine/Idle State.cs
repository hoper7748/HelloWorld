using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.XR;

namespace SlimeProject
{
    public class IdleState : BaseState
    {
        private float skillTime = 0;
        private float currentTimer = 0;
        public IdleState(FSMController fSMController) : base(fSMController)
        {
            // 플레이어와 거리가 가까워지면 공격 시작
            // 기본적으로 공격은 BossMonster Script에서 사용 예정이고
            // 특수 공격의 경우 일정 시간이 경과하면 사용
            skillTime = 3;
            currentTimer = 3;
        }
        
        public override void Enter()
        {
            base.Enter();
            currentTimer = 0;
        }

        public override void Update()
        {
            currentTimer += Time.deltaTime;
            if(currentTimer > skillTime)
            {
                FSMController.ChangeState(FSMController.AttackState);
            }
        }

        public override void Exit() 
        { 
        
        }
    }
}
