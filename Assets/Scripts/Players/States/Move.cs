using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 이동 기믹 실시간으로 바로 앞 칸을 체크하여 바닥(BoxCollider)이 존재하면 이동하는 것을 지향함.
/// </summary>
public class Move :State
{
    private float speed = 5;

    public Move(Entity entity) : base(entity)
    {
        
    }

    public override void Enter()
    {
        CustomDebug.Log("Move Enter");
        if (entity != null)
        {
            entity.state = EState.Move;
            entity.animator.SetTrigger("Move");
        }
    }

    public override void Exit()
    {
        CustomDebug.Log("Move Exit");
        entity.animator.SetTrigger("Stop");
        //if()
    }

    public override void Update()
    {
        if (!entity.CheckGround())
        {
            CustomDebug.Log("바닥이 없음");
            return;
        }

        if(entity.Direction)
        {
            entity.transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else
        {
            entity.transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
    }

}
