using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SummonType
{
    None,
    Ranger,
    Guarder,
}

public class SummonSkill : Skill
{
    // 공격 오브젝트일 수도 있고 어그로용 일 수도 있고.
    public GameObject SummonObject;
    public SkillType SkillType;
    public override void UsingSkill()
    {
        //if (curCooldown > CoolDown)
        //{
        //    Target.UseSkill(SkillType);
        //    curCooldown = 0;
        //}
        if (!isCooldown && Target.state != EState.Skill)
        {
            Target.UseSkill(SkillType);
            curCooldown = CoolDown;
            isCooldown = !isCooldown;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        curCooldown = CoolDown;
    }
    // Update is called once per frame
    void Update()
    {
        curCooldown -= Time.deltaTime;
        if (isCooldown)
        {
            curCooldown -= Time.unscaledDeltaTime; // timeScale 무시
            CoolDownImage.fillAmount = curCooldown / CoolDown;

            if (curCooldown <= 0f)
            {
                isCooldown = false;
                CoolDownImage.fillAmount = 0f;
            }
        }
    }
}
