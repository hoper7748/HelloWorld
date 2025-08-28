using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : Entity
{
    protected override void ChangeState(State state)
    {
        if (curState == attackState && this.state == EState.Rooted)
            return;
        curState.Exit();
        curState = state;
        curState.Enter();
    }

    float changeTimer = 0;


    // Start is called before the first frame update
    void Start()
    {
        TargetTimer = Random.Range(MinAttackTimer, MaxAttackTimer);
    }

    [Header("Timer Parameters")]
    public float MinAttackTimer;
    public float MaxAttackTimer;
    public float MinMoveTimer;
    public float MaxMoveTimer;
    public float TargetTimer;
    public float curTimer;

    // AI 전용 스킬 쿨타임도 정해야할 듯?
    [Header("Skill Cool down")]
    public SkillType S1 = SkillType.Sky_Shot;
    public float S1CoolTime = 0;
    public float S1MaxCool;

    [Space(10)]
    public SkillType S2 = SkillType.Trap_Arrow;
    public float S2CoolTime = 0;
    public float S2MaxCool;

    [Space(10)]
    public SkillType S3 = SkillType.Summon_Ranger;
    public float S3CoolTime = 0;
    public float S3MaxCool;


    float rootedTimer = 0;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.playType == GameManager.PlayType.Play)
        {
            curTimer += Time.deltaTime;
            curState.Update();
            if (this.state == EState.Rooted)
            {
                rootedTimer += Time.deltaTime;
                if (rootedTime < rootedTimer)
                {
                    rootedTimer = 0;
                    this.state = EState.Attack;
                    SilentIcon.SetActive(false);
                }
            }
            else
            {
                SkillUpdate();
                switch (state)
                {
                    case EState.Attack:
                        if (TargetTimer < curTimer && state != EState.Rooted)
                        {
                            CustomDebug.Log("Attack");
                            ChangeState(moveState);

                            // 타이머 초기화
                            TargetTimer = Random.Range(MinMoveTimer, MaxMoveTimer);
                            isRight = Random.Range(0, 1) == 0 ? false : true;
                            curTimer = 0;
                        }
                        break;
                    case EState.Move:
                        // 움직임 시작
                        if (!CheckGround())
                        {
                            isRight = !isRight;
                        }
                        if (TargetTimer < curTimer)
                        {
                            // 스킬을 사용할 수 있음 스킬부터 사용 
                            // 스킬을 사용할 수 없는 상태면 공격 전환
                            //일단 현재는 공격으로 전환
                            CustomDebug.Log("Move");
                            ChangeState(attackState);
                            TargetTimer = Random.Range(MinAttackTimer, MaxAttackTimer);
                            curTimer = 0;
                        }
                        break;
                    case EState.Skill:

                        break;
                    case EState.Rooted:
                        break;
                    default:
                        break;
                }
            }
            
        }
    }


    private void SkillUpdate()
    {
        if (curState == attackState && this.state == EState.Rooted)
            return;
        S1CoolTime += Time.deltaTime;
        if(S1CoolTime >= S1MaxCool && (state == EState.Attack || state == EState.Move))
        {
            S1CoolTime = 0;
            CustomDebug.Log("스킬 1 사용");
            ChangeSkillState(S1);
        }

        S2CoolTime += Time.deltaTime;
        if (S2CoolTime >= S2MaxCool && (state == EState.Attack || state == EState.Move))
        {
            S2CoolTime = 0;
            CustomDebug.Log("스킬 2 사용");
            ChangeSkillState(S2);
        }

        S3CoolTime += Time.deltaTime;
        if (S3CoolTime >= S3MaxCool && (state == EState.Attack || state == EState.Move))
        {
            S3CoolTime = 0;
            CustomDebug.Log("스킬 3 사용");
            ChangeSkillState(S3);
        }
    }

    private void ChangeSkillState(SkillType stype)
    {
        switch (stype)
        {
            case SkillType.None:
                break;
            case SkillType.Sky_Shot:
                ChangeState(skyAttack);
                break;
            case SkillType.Mole_Shot:
                ChangeState(jumpAttack);
                break;
            case SkillType.MuDaMuDa_Arrow:
                //ChangeState(skyAttack);
                break;
            case SkillType.The_World:
                //ChangeState(skyAttack);
                break;
            case SkillType.Trap_Arrow:
                ChangeState(trapShot);
                break;
            case SkillType.Summon_Guard:
                ChangeState(summonGuardman);
                break;
            case SkillType.Summon_Ranger:
                ChangeState(summonRanger);
                break;
            case SkillType.ArrowRain:
                ChangeState(arrowRain);
                break;
            default:
                break;
        }
    }
}
