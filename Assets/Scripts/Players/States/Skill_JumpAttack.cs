using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 낙하 지점으로 이동했을 때, 떨어져서 죽는 기믹.
/// </summary>
public class Skill_JumpAttack : State
{
    private enum SkillSequence
    {
        None,
        Jump,
        Shoot,
        Down,
        End
    }

    private SkillSequence seq = SkillSequence.Jump;

    public Skill_JumpAttack(Entity entity) : base(entity)
    {

    }

    private float startY;
    private float endY;

    public override void Enter()
    {
        seq = SkillSequence.Jump;
        startY = entity.transform.position.y;
        endY = entity.transform.position.y + Vector3.up.y * 2f;
        shootCount = 3;
        jumpTimer = 0;

        if (entity != null)
        {
            entity.state = EState.Skill;
            entity.animator.SetTrigger("Jump");
        }
        // 3개의 스폰 지점 생성
        RandomPlace();
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

    public override void Exit()
    {
        spawner.Clear();
        spawnCount = 0;
    }

    // 시퀸스 필요
    public override void Update()
    {
        PlaySequence();
    }

    private float timer = 0;

    private float jumpTimer = 0;
    private float shootDelay = 0.25f;
    private int shootCount = 3;

    private void PlaySequence()
    {
        //timer += Time.deltaTime;

        // 시간당 행동
        switch (seq)
        {
            case SkillSequence.Jump:
                // 일정 좌표까지 빠르게 점프
                if (jumpTimer < 1f)
                {
                    jumpTimer += Time.deltaTime / .15f; // 0 → 1까지 0.5초
                    Vector3 pos = entity.transform.position;
                    pos.y = Mathf.Lerp(startY, endY, jumpTimer);
                    entity.transform.position = pos;
                }
                else
                {
                    timer += Time.deltaTime;
                    if (timer > 0.125f)
                    {
                        seq = SkillSequence.Shoot;
                        entity.animator.SetTrigger("Down");
                        timer = 0;
                    }
                }
                break;
            case SkillSequence.Shoot:
                // 아래로 3발 쏘기
                timer += Time.deltaTime;
                if(timer > shootDelay && shootCount > 0)
                {
                    // 발사
                    CustomDebug.Log("아래로 쏨");
                    ShootArrow();
                    // 시간 초기화
                    timer = 0;
                    //shootCount--;
                }
                else if(shootCount ==0 )
                {
                    if(timer > 0.25f)
                    {
                        seq = SkillSequence.Down;
                        timer = 0;
                    }
                }
                break;
            case SkillSequence.Down:
                // 쏘면 내려오기.

                //CustomDebug.Log("떨어지는 중");
                if (jumpTimer > 0f)
                {
                    jumpTimer -= Time.deltaTime / .25f; // 0 → 1까지 0.5초
                    Vector3 pos = entity.transform.position;
                    pos.y = Mathf.Lerp(startY, endY, jumpTimer);
                    entity.transform.position = pos;
                }
                else
                {
                    timer += Time.deltaTime;
                    if (timer > 0.25f)
                    {
                        entity.animator.SetTrigger("Stop");
                        seq = SkillSequence.End;
                        timer = 0;
                    }
                }
                break;
            case SkillSequence.End:
                // State 교체 
                CustomDebug.Log("끝");
                entity.CallChangeState(EState.Attack);
                break;
            default:
                break;
        }
    }

    // 이 때 화살은 하늘로 향하는 화살이여야함.
    private void ShootArrow()
    {
        // 현재 위치 바로 위에서 생성.
        GameObject obj = null;
        obj = GameObject.Instantiate(entity.BlinkArrowPrefab);
        obj.transform.position = entity.transform.position + Vector3.down * 0.5f;
        
        obj.GetComponent<BlinkArrow>()?.Init(entity.Target, BlinkArrow.Way.Down, BlinkArrow.ArrowType.Arrow, SummonType.None, spawnCount < spawner.Count ? spawner[spawnCount++] : null);
        //obj.GetComponent<ParabolaArrow>()?.Init();
        shootCount -= 1;
    }

}
