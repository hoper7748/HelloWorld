using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    enum TrapState
    {
        Down,
        Setting,
        Ready,
    }
    // 역할
    // 떨어질 땐 반응 없음
    // 떨어진 이후 n1초 뒤 작동
    // 이후 n2초 동안 작동하지 않으면 Delete

    private TrapState state = TrapState.Down;

    public int Atk = 0;

    // Start is called before the first frame update
    void Start()
    {
        state = TrapState.Down;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case TrapState.Down:
                DownAction();
                break;
            case TrapState.Setting:
                WaitSettingAction();
                break;
            case TrapState.Ready:
                WaitAction();
                break;
            default:
                break;
        }
    }

    private float curTimer = 0;
    private float SetTimer = 2f;

    private float HideTimer = 3f;

    private void WaitAction()
    {
        curTimer += Time.deltaTime;
        if(curTimer > HideTimer)
        {
            Destroy(gameObject);
        }
    }


    private void WaitSettingAction()
    {
        curTimer += Time.deltaTime;
        if(curTimer > SetTimer)
        {
            state = TrapState.Ready;
            curTimer = 0;
        }
    }

    private void DownAction()
    {
        if(transform.position.y >= -1.4f)
        {
            // 내려와라
            transform.Translate(Vector3.down * 8 * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, -1.4f, transform.position.z);
            state = TrapState.Setting;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 3 && TrapState.Ready == state)
        {
            CustomDebug.Log("Player가 밟음");
            Entity temp = collision.GetComponent<Entity>();
            temp.rootedTime = 3;
            temp.GetDamaged(Atk);
            temp.state = EState.Rooted;
            temp.CallChangeState(EState.Attack);
            temp.SilentIcon.SetActive(true);
            Destroy(gameObject);
        }
    }
}
