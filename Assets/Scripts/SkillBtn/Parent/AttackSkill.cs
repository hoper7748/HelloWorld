using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SkillType
{
    None = 0,
    Sky_Shot,
    Mole_Shot,
    MuDaMuDa_Arrow,
    The_World,
    Trap_Arrow,
    Summon_Guard,
    Summon_Ranger,
    ArrowRain,

}

public class AttackSkill : Skill
{
    // 다양한 공격을 시전 예정.
    // ex)
    // 하늘로 쏴서 적 위치에 n번의 화살을 쏜다.
    // 상대 진영에 랜덤으로 3곳에 느낌표 표시가 발생하고 n초 뒤 화살이 올라옴.
    // 3초간 자신이 발사한 화살의 속도가 느려짐 3초 경과시 화살의 속도가 정상으로 돌아옴.
    // 2초간 빠른 속도로 화살을 발사함. 시전 중에는 이동할 수 없음.
    // 하늘로 트랩을 쏘고 n초 뒤 상대방 진영에 2개의 트랩을 설치함. 상대방이 밟으면 상대방은 이동할 수 없는 상태가 됨.
    public SkillType SkillType;

    private void Start()
    {
        curCooldown = CoolDown;
    }

    public override void UsingSkill()
    {
        if(!isCooldown && Target.state != EState.Skill)
        {
            Target.UseSkill(SkillType);
            curCooldown = CoolDown;
            isCooldown = !isCooldown;
        }
    }

    private void Update()
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
