using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerController : Entity
{
    private bool buttonHold = false;

    // 방향 지정해주면 될 듯?


    // Start is called before the first frame update
    void Start()
    {
        
    }

    float rootedTimer = 0;
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.playType == GameManager.PlayType.Play)
        {
            if (this.state == EState.Rooted)
            {
                rootedTimer += Time.deltaTime;
                if(rootedTime < rootedTimer)
                {
                    rootedTimer = 0;
                    this.state = EState.Attack;
                    SilentIcon.SetActive(false);    
                }
            }
            curState.Update();

        }
    }

    // 특수 상태일 땐 이동할 수 없게 해야함.
    public void MovementStart(int direction)
    {
        if (state == EState.Skill || state == EState.Rooted)
            return;
        buttonHold = true;
        if (direction == 0)
        {
            CustomDebug.Log("좌측 이동");
            spriteRenderer.flipX = true;
            isRight = false;
        }
        else if (direction == 1)
        {
            CustomDebug.Log("우측 이동");
            spriteRenderer.flipX = false;
            isRight = true;
        }
        ChangeState(moveState);
    }

    public void MovementStop()
    {
        if (state == EState.Skill || state == EState.Rooted)
            return;
        buttonHold = false;
        ChangeState(attackState);
    }

    // rooted 상태일 땐 조작이 불가능하게 해야함.
    protected override void ChangeState(State state)
    {
        if (curState == attackState && this.state == EState.Rooted)
            return;
        curState.Exit();
        curState = state;   
        curState.Enter();
    }
}
