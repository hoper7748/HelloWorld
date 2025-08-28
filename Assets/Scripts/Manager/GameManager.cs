using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Instance
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                return null;
            return instance;
        }
    }
    private static GameManager instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public enum PlayType
    {
        Ready,
        Play,
        End,
    }
    public float MaxTimer = 90f;

    public int curTime = 90;
    public int FeverTime = 0;
    public float CurTimer = 0f;

    public PlayType playType = PlayType.Ready;
    [Header("Info")]
    public Entity P1;
    public Entity P2;


    [Header("UI")]
    public TextMeshProUGUI TimerText;

    public Slider P1Hp;
    private Vector3 p1HpOrigin;
    public Slider P2Hp;
    private Vector3 p2HpOrigin;

    public List<SpriteRenderer> MyPlace;
    public List<SpriteRenderer> EnemyPlace;
    [Header("Summon Object")]
    public List<GameObject> P1Objects;
    public List<GameObject> P2Objects;

    public GameObject TrapPrefab;

    public GameObject EndGamePanel;
    public GameObject WinPanel;
    public GameObject LosePanel;

    public Color placeColor;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var place in MyPlace)
        {
            place.color = new Color(1, 1, 1, 0);
        }
        foreach (var place in EnemyPlace)
        {
            place.color = new Color(1, 1, 1, 0);
        }
        p1HpOrigin = P1Hp.GetComponent<RectTransform>().localPosition;
        p2HpOrigin = P2Hp.GetComponent<RectTransform>().localPosition;
        EndGamePanel.SetActive(false);
        WinPanel.SetActive(false);
        LosePanel.SetActive(false);
    }

    public void Init()
    {
        //CurTimer = MaxTimer;
    }

    public void UpdateHp(Entity entity)
    {
        if(entity.PlayerType == EPlayerType.P1)
        {
            P1Hp.value = entity.Hp;
            // 흔들림 효과 실행
            StopAllCoroutines();
            StartCoroutine(ShakeHpBar(EPlayerType.P1));
        }
        else if(entity.PlayerType == EPlayerType.P2)
        {
            P2Hp.value = entity.Hp;
            // 흔들림 효과 실행
            StopAllCoroutines();
            StartCoroutine(ShakeHpBar(EPlayerType.P2));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(playType == PlayType.Play)
        {
            CurTimer += Time.unscaledDeltaTime;
            if (curTime == FeverTime)
            {
                Time.timeScale = 1.5f;
            }
            if (CurTimer > 1 && curTime > 0)
            {
                curTime -= 1;
                if (curTime <= 15)
                {
                    TimerText.color = Color.red;
                }
                TimerText.text = $"{curTime}";
                CurTimer = 0;
            }
            else if (curTime == 0)
            {
                playType = PlayType.End;
            }
        }
    }

    internal void Summon(Entity entity, SummonType type)
    {
        Entity temp = entity.GetComponent<Entity>();
        if(type == SummonType.None)
        {
            CustomDebug.Log("SummonType None");
        }
        switch (temp.PlayerType)
        {
            case EPlayerType.P1:
                // 일단 레인저부터 소환
                temp.summonObject = Instantiate(P1Objects[(int)type - 1]);
                temp.summonObject.transform.position = temp.transform.position + Vector3.up * 6f;
                break;
            case EPlayerType.P2:
                temp.summonObject = Instantiate(P2Objects[(int)type - 1]);
                temp.summonObject.transform.position = temp.transform.position + Vector3.up * 6f;
                break;
            default:
                break;
        }

    }

    public Entity GetTarget(Entity entity)
    {
        if(entity.PlayerType == EPlayerType.P1)
        {
            return P2;  
        }
        else if(entity.PlayerType == EPlayerType.P2)
        {
            return P1;
        }

        return null;
    }

    public List<SpriteRenderer> GetPlace(EPlayerType type)
    {
        if( type == EPlayerType.P1)
        {
            return EnemyPlace;
        }
        else
        {
            return MyPlace;
        }
    }

    public IEnumerator ShakeHpBar(EPlayerType type)
    {
        float elapsed = 0f;

        while (elapsed < 0.3f)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * 10f;
            if(type == EPlayerType.P1)
            {
                P1Hp.GetComponent<RectTransform>().localPosition = p1HpOrigin + new Vector3(x, 0f, 0f);
            }
            else if(type == EPlayerType.P2)
            {
                P2Hp.GetComponent<RectTransform>().localPosition = p2HpOrigin + new Vector3(x, 0f, 0f);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (type == EPlayerType.P1)
        {
            P1Hp.GetComponent<RectTransform>().localPosition = p1HpOrigin; // 원위치 복구
        }
        else if (type == EPlayerType.P2)
        {
            P2Hp.GetComponent<RectTransform>().localPosition = p2HpOrigin; // 원위치 복구
        }
    }

    public void GameSet(EPlayerType playerType)
    {
        // UI 띄우기
        if (playerType == EPlayerType.P1)
        {
            CustomDebug.Log("유저 패배");
            EndGamePanel.SetActive(true);
            WinPanel.SetActive(true);
            //LosePanel.SetActive(false);
        }
        else if (playerType == EPlayerType.P2)
        {
            CustomDebug.Log("AI 패배");
            EndGamePanel.SetActive(true);
            //WinPanel.SetActive(false);
            LosePanel.SetActive(true);

        }

        playType = PlayType.End;
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
