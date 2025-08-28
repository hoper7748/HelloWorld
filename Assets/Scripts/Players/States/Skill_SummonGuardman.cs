using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_SummonGuardman : State
{
    private float AttackDelay = 0.15f;
    private float AttackTimer = 0f;
    private SummonType summonType = SummonType.Guarder;

    public Skill_SummonGuardman(Entity entity) : base(entity)
    {

    }

    public override void Enter()
    {
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
        if (AttackTimer > AttackDelay)
        {
            ShootArrow();
            AttackTimer = 0;
            entity.CallChangeState(EState.Attack);
        }
        //if (AttackTimer > AttackDelay)
        //{
        //}
        //CustomDebug.Log("Attack Update");
    }

    // 이 때 화살은 하늘로 향하는 화살이여야함.
    private void ShootArrow()
    {
        // 현재 위치 바로 위에서 생성.
        GameObject obj = null;
        obj = GameObject.Instantiate(entity.BlinkArrowPrefab);
        obj.transform.position = entity.transform.position + Vector3.up * 0.5f;
        obj.GetComponent<BlinkArrow>()?.Init(entity, BlinkArrow.Way.Up, BlinkArrow.ArrowType.Summon, summonType);
        //obj.GetComponent<ParabolaArrow>()?.Init();
        //Count -= 1;
    }
}
