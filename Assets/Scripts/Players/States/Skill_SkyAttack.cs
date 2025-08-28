using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_SkyAttack : State
{
    // 게임 준비 단계에선 공격하지 않기.
    // GameManager 기준으로 Start가 활성화 되어야 함.
    private float AttackDelay = 0.15f;
    private float AttackTimer = 0f;
    private int Count = 0; 


    public Skill_SkyAttack(Entity entity) : base(entity)
    {

    }

    public override void Enter()
    {
        Count = entity.ScheduledArrow;
        if (entity != null)
        {
            entity.state = EState.Skill;
        }
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        // 일정 시간 후 작동
        AttackTimer += Time.deltaTime;
        if (AttackTimer > AttackDelay && Count > 0)
        {
            ShootArrow();
            AttackTimer = 0;
        }
        if(AttackTimer > AttackDelay && Count == 0)
        {
            entity.CallChangeState(EState.Attack);
        }
        //CustomDebug.Log("Attack Update");
    }

    // 이 때 화살은 하늘로 향하는 화살이여야함.
    private void ShootArrow()
    {
        // 현재 위치 바로 위에서 생성.
        GameObject obj = null;
        obj = GameObject.Instantiate(entity.BlinkArrowPrefab);
        obj.transform.position = entity.transform.position + Vector3.up * 0.5f;
        obj.GetComponent<BlinkArrow>()?.Init(entity, BlinkArrow.Way.Up, BlinkArrow.ArrowType.Arrow);
        //obj.GetComponent<ParabolaArrow>()?.Init();
        Count -= 1;
    }
}
