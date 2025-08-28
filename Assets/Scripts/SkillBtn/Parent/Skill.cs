using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skill : MonoBehaviour
{
    public float CoolDown = 0;
    protected float curCooldown = 0;
    public Image CoolDownImage;
    public TextMeshProUGUI CooldownText;
    public bool isCooldown = false;

    public Entity Target;

    public abstract void UsingSkill();

    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
