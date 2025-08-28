using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_TrapShot : State
{
    private float AttackDelay = 0.15f;
    private float AttackTimer = 0f;
    private int Count = 2;

    public Skill_TrapShot(Entity entity) : base(entity)
    {
    }

    public override void Enter()
    {   
        CustomDebug.Log("TrapShot Enter");
        if (entity != null)
        {
            entity.state = EState.Skill;
            Count = entity.ScheduledArrow;
        }
        RandomPlace();
        spawnCount = 0;
    }

    public override void Exit()
    {
        CustomDebug.Log("TrapShot Exit");
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
        if (AttackTimer > AttackDelay && Count == 0)
        {
            entity.CallChangeState(EState.Attack);
        }
    }

    private List<SpriteRenderer> spawner = null;
    private int spawnCount = 0;

    private void RandomPlace()
    {
        List<SpriteRenderer> tempList = new List<SpriteRenderer>(GameManager.Instance.GetPlace(entity.PlayerType));

        int n = tempList.Count;
        System.Random rng = new System.Random();
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            SpriteRenderer value = tempList[k];
            tempList[k] = tempList[n];
            tempList[n] = value;
        }

        spawner = tempList.GetRange(0, 3);
    }


    // 이 때 화살은 하늘로 향하는 화살이여야함.
    private void ShootArrow()
    {
        // 현재 위치 바로 위에서 생성.
        GameObject obj = null;
        obj = GameObject.Instantiate(entity.BlinkArrowPrefab);
        obj.transform.position = entity.transform.position + Vector3.up * 0.5f;
        obj.GetComponent<BlinkArrow>()?.Init(entity, BlinkArrow.Way.Up, BlinkArrow.ArrowType.Trap,SummonType.None, spawnCount < spawner.Count ? spawner[spawnCount++] : null);
        //obj.GetComponent<ParabolaArrow>()?.Init();
        Count -= 1;
    }
}
