using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingDamage : MonoBehaviour
{

    public TextMeshPro MainText;
    public TextMeshPro OutlineText;

    public float hideTime = 0;
    public float curTimer = 0;
    public float moveUpSpeed = 1;
    // 스폰되면 위로 올라가면서 alpha값이 옅어지도록 설계
    // alpha가 0이 되면 삭제

    // Start is called before the first frame update
    void Start()
    {
        curTimer = hideTime;
    }
    public void SetText(int damageText)
    {
        MainText.text = damageText.ToString();
        OutlineText.text = damageText.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        curTimer -= Time.deltaTime;

        MainText.color = new Color(1, 1, 1, curTimer / hideTime);
        OutlineText.color = new Color(1, 1, 1, curTimer / hideTime);

        transform.Translate(Vector3.up * moveUpSpeed * Time.deltaTime);


        if (curTimer / hideTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
