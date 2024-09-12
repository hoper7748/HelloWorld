using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
//using System;

namespace SlimeProject
{
    public class AttackState : BaseState
    {
        private Arrow[] arrowPools;
        private List<int> counting = new List<int> { 0, 1, 2, 3, 4 };

        private float shootTime = 1f;
        private float currentTiemr = 0;
        private int arrowCurrent = 0;
        private System.Random random = new System.Random();
        private float speed = 8;

        public AttackState(FSMController fSMController) :base(fSMController)
        {
            currentTiemr = 0;
            arrowCurrent = 0;
            arrowPools = new Arrow[4];
            for (int i = 0; i < 4; i++)
            {
                arrowPools[i] = GameObject.Instantiate(FSMController.arrowPrefab);
                arrowPools[i].gameObject.SetActive(false);;
            }
        }

        public override void Enter()
        {
            base.Enter();
            currentTiemr = 0;
            counting = counting.OrderBy(_ => random.Next()).ToList();
            arrowCurrent = 0;
            
        }
        public override void Update()
        {
            //base.Update();
            currentTiemr += Time.deltaTime;
            if(arrowCurrent < 3)
            {
                ShootArrow();
            }
            else
            {
                FSMController.ChangeState(FSMController.IdleState);
            }
        }

        private void ShootArrow()
        {
            float nowSpeed = FieldManager.Instance.Scrolling ? speed : speed - FieldManager.Instance.ScrollSpeed;

            if (currentTiemr > shootTime)
            {
                arrowPools[arrowCurrent].gameObject.SetActive(true);
                arrowPools[arrowCurrent].SetArrow(FSMController.CharacterSO.Damage, nowSpeed, FSMController.targetLayer);
                arrowPools[arrowCurrent].transform.position = new Vector2(counting[arrowCurrent] - (5 * 0.5f) + 0.5f, Vector3.up.y * 7f); 
                arrowCurrent++;

                if (arrowCurrent >= arrowPools.Length)
                {
                    arrowCurrent = 0;
                }
                currentTiemr = 0;

            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}