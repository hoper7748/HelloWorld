using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : State
{
    // 게임 준비 단계에선 공격하지 않기.
    // GameManager 기준으로 Start가 활성화 되어야 함.
    private float AttackDelay = 0.75f;
    private float AttackTimer = 0f;


    public Attack(Entity entity) : base(entity)
    {

    }

    public override void Enter()
    {
        //CustomDebug.Log("Attack Enter");
        //AttackTimer = 0;
        if (entity != null&& entity.state != EState.Rooted)
        {
            entity.state = EState.Attack;
        }
    }

    public override void Exit()
    {

        //CustomDebug.Log("Attack Exit");
    }

    public override void Update()
    {
        // 일정 시간 후 작동
        AttackTimer += Time.deltaTime;
        if( AttackTimer > AttackDelay)
        {
            ShootArrow();
            AttackTimer = 0;
        }
        //CustomDebug.Log("Attack Update");
    }

    private void ShootArrow()
    {
        GameObject obj = null;
        obj = GameObject.Instantiate(entity.ArrowPrefab);
        obj.transform.position = entity.transform.position;
        obj.GetComponent<ParabolaArrow>()?.SetAToB(entity.transform.position, entity.Target.transform.position, entity);
        //obj.GetComponent<ParabolaArrow>()?.Init();
    }
}
