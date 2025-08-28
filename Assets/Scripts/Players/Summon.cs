using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 소환몹의 AI 그래서 대충 만들 예정
public class Summon : Entity
{
    enum SummonSequence
    {
        None,
        // 떨어지는 중
        DropDown,
        // 공격 개시
        Attack,
        // 죽음
        Die,
    }

    [SerializeField]
    private SummonSequence seq = SummonSequence.DropDown;
    [SerializeField]
    private SummonType summonType;

    [SerializeField]
    private float dropSpeed = 9f;
    [SerializeField]
    private float HideTimer = 10;
    private float curTimer = 0;
    private void Start()
    {
        //Hp = 150;
        //MaxHp = Hp;
        Init();
    }

    public void Init()
    {
        Target = GameManager.Instance.GetTarget(this);
    }

    private void Update()
    {
        switch (seq)
        {
            case SummonSequence.None:
                break;
            case SummonSequence.DropDown:
                DropDownAction();
                break;
            case SummonSequence.Attack:

                AttackAction();
                HideCheck();
                break;
            case SummonSequence.Die:
                CustomDebug.Log("Hide Action");
                DieSequence();
                break;
            default:
                break;
        }
    }

    private void DropDownAction()
    {
        if(transform.position.y > -1.5)
        {
            transform.Translate(Vector3.down * dropSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            seq = SummonSequence.Attack;
        }
    }

    private float timer = 0;
    private float attackDelay = 1.5f;

    private void AttackAction()
    {
        if(Hp > 0 && summonType == SummonType.Ranger)
        {
            timer += Time.deltaTime;
            if(timer > attackDelay)
            {
                //CustomDebug.Log("ATtack");
                ShootArrow();
                timer = 0;
            }
        }
    }

    private void HideCheck()
    {
        curTimer += Time.deltaTime;
        if(curTimer > HideTimer)
        {
            seq = SummonSequence.Die;
            if (animator != null)
            {
                animator.SetTrigger("Die");
            }

            curTimer = 0;
        }
    }

    private void DieSequence()
    {
        curTimer += Time.deltaTime;
        if(curTimer > 2)
        {
            Destroy(gameObject);
        }
    }

    private void ShootArrow()
    {
        GameObject obj = null;
        obj = GameObject.Instantiate(ArrowPrefab);
        obj.transform.position = transform.position;
        obj.GetComponent<ParabolaArrow>()?.SetAToB(transform.position, Target.transform.position, this);
        if(animator != null)
        {
            animator.SetTrigger("Attack");
        }
        //obj.GetComponent<ParabolaArrow>()?.Init();
    }


    protected override void ChangeState(State state)
    {
        throw new System.NotImplementedException();
    }

    // 그냥 대충 짜자
}
