using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ArrowRain : State
{
    public Skill_ArrowRain(Entity entity) : base(entity)
    {

    }

    public override void Enter()
    {
        if(entity != null)
        {
            entity.state = EState.Skill;
        }
    }

    public override void Exit()
    {

    }

    float curTimer = 0;
    float shootDelay = 0.15f;

    int curSpawer = 0;

    public override void Update()
    {
        curTimer += Time.deltaTime;
        if(curTimer >= shootDelay)
        {
            ShootArrow();
            curTimer = 0;
        }
        if (curSpawer >= GameManager.Instance.GetPlace(entity.PlayerType).Count)
        {
            curSpawer = 0;
            entity.CallChangeState(EState.Attack);
        }
    }

    private void ShootArrow()
    {
        GameObject obj = null;
        obj = GameObject.Instantiate(entity.ArrowPrefab);
        obj.transform.position = entity.transform.position;
        obj.GetComponent<ParabolaArrow>()?.SetAToB(entity.transform.position, GameManager.Instance.GetPlace(entity.PlayerType)[curSpawer].transform.position, entity, true);
        curSpawer++;
    }

}
