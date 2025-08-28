using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EState
{
    Attack,
    Move,
    Skill,
    Rooted,
}

public enum EPlayerType
{
    P1,
    P2,
}

public abstract class Entity : MonoBehaviour
{
    public int MaxHp = 1400;
    public int Hp = 1400;
    public int ATK = 75;

    public EPlayerType PlayerType = EPlayerType.P1;

    public GameObject summonObject;

    public EState state;
    protected State curState;
    protected Attack attackState;
    protected Move moveState;
    protected Skill_SkyAttack skyAttack;
    protected Skill_JumpAttack jumpAttack;
    protected Skill_TrapShot trapShot;
    protected Skill_SummonRanger summonRanger;
    protected Skill_SummonGuardman summonGuardman;
    protected Skill_ArrowRain arrowRain;
    //protected




    protected SpriteRenderer spriteRenderer;

    protected bool isRight = false;

    public Entity Target;

    [SerializeField]
    public Animator animator;

    public GameObject ArrowPrefab;
    public GameObject BlinkArrowPrefab;

    public List<ParabolaArrow> ArrowContainer = new List<ParabolaArrow>();

    public List<SpriteRenderer> TargetSprite;

    public int ScheduledArrow = 0;
    public int rootedTime = 0;
    public GameObject SilentIcon;

    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        attackState = new Attack(this);
        moveState = new Move(this);
        skyAttack = new Skill_SkyAttack(this);
        jumpAttack = new Skill_JumpAttack(this);
        summonRanger = new Skill_SummonRanger(this);
        summonGuardman = new Skill_SummonGuardman(this);
        trapShot = new Skill_TrapShot(this);
        arrowRain = new Skill_ArrowRain(this);
        curState = attackState;
        // 시작 State 
        curState.Enter();
        state = EState.Attack;
        if(SilentIcon != null)
            SilentIcon.SetActive(false);
        animator = GetComponent<Animator>();

    }

    public bool Direction
    {
        get
        {
            return isRight;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + (transform.right * 0.5f) + (Vector3.down * 0.5f), transform.position + (transform.right* 0.5f) + Vector3.down);
    }

    protected abstract void ChangeState(State state);

    public void CallChangeState(EState state)
    {
        switch (state)
        {
            case EState.Attack:
                ChangeState(attackState);
                break;
            case EState.Move:
                ChangeState(moveState);
                break;
            //case EState.Skill:
            //    break;
            default:
                break;
        }
    }

    // 이동 방향이 절벽 또는 벽인지 체크하기.
    public bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + ((isRight ? transform.right : -transform.right) * 0.25f), Vector2.down, 1, 1 << 6);
        return hit.collider != null;
    }

    public void UseSkill(SkillType num)
    {
        if(curState == attackState || curState == moveState)
        {
            switch (num)
            {
                case SkillType.None:
                    break;
                case SkillType.Sky_Shot:
                    // 하늘을 향해 샷
                    ScheduledArrow = 3;
                    ChangeState(skyAttack);
                    break;
                case SkillType.Trap_Arrow:
                    // 하늘을 향해 샷
                    ScheduledArrow = 2;
                    ChangeState(trapShot);
                    break;
                case SkillType.Mole_Shot:
                    // 점프 샷
                    ChangeState(jumpAttack);
                    break;
                case SkillType.MuDaMuDa_Arrow:
                    // 일반 공격으로 대체 
                    break;
                case SkillType.The_World:
                    // 어떤 조작이든 가능.
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
                //break;
                default:
                    break;
            }
        }
    }

    public void GetDamaged(int damage)
    {
        Hp -= damage;
        GameManager.Instance.UpdateHp(this);

        if (Hp <= 0)
        {
            // 금지
            GameManager.Instance.GameSet(PlayerType);
        }
    }
}
