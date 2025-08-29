using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BlinkArrow : MonoBehaviour
{
    public enum ArrowType
    {
        Arrow,
        Summon,
        Trap,
    }

    public enum Way
    {
        None,
        Up,
        Down
    }
    Entity entity;

    public Vector2 startPos;
    public Vector2 targetPos;
    public float speed = 10f;  // 발사 속도
    public float gravity = 9.8f;

    public float minAlpha = 0f;
    public float maxAlpha = 0.5f;
    public float blickSpeed = 1f; // 깜빡이는 속도


    [SerializeField]
    private float time;
    [SerializeField]
    private float wayChangeTime = 0;
    [SerializeField]
    private Way way;
    [SerializeField]
    private ArrowType arrowType;
    [SerializeField]
    private SummonType summonType;
    [SerializeField]
    private int changeCount = 0;
    [SerializeField]
    private Entity target;

    [SerializeField]
    private SpriteRenderer mySprite;

    private SpriteRenderer warningSign;

    public ParticleSystem particle;

    void Start()
    {
        
    }

    public void Init(Entity entity, Way way, ArrowType arrowType, SummonType type = SummonType.None, SpriteRenderer warningSign = null)
    {
        this.entity = entity;   
        this.target = entity.Target;
        this.way = way;
        this.warningSign = warningSign;
        this.arrowType = arrowType;
        this.summonType = type;
        if(warningSign != null) 
            Debug.Log($"{warningSign.name}");
        switch (way)
        {
            case Way.None:
                break;
            case Way.Up:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Way.Down:
                transform.rotation = Quaternion.Euler(0, 0, 180f);
                break;
            default:
                break;
        }
        //// 스폰 위치 조정
    }

    // 목표 지점 도달 후 n초 뒤 지정된 위치에서 생성되어야함.
    void Update()
    {
        //if (time < 1)
        time += Time.deltaTime * 50f;
        wayChangeTime += Time.deltaTime;

        if(ArrowType.Summon == arrowType && transform.position.y >= 8f)
        {
            Summon();
            Destroy(gameObject);
        }

        if (wayChangeTime > 3 && changeCount == 0)
        {
            changeCount++;
            ChangeWay();
            //way = Way.Down;
            //if (way == Way.Down)
            //    transform.position = target.transform.position + Vector3.up * 6f;
            wayChangeTime = 0;
        }
        else if (changeCount == 0 && !mySprite.enabled)
        {
            // 진행 방향이 바뀌지 않았으면서 자기 자신의 Sprite가 disable일 때, targetGround Sprite를 블링크 해주기
            BlinkSprite();
        }
        Movement();
        //transform.Translate((way == Way.Up ? Vector3.up * time * Time.deltaTime : Vector3.down * gravity * Time.deltaTime));
    }

    public void Summon()
    {
        GameManager.Instance.Summon(entity, summonType);
        Destroy(this.gameObject);
    }

    private void BlinkSprite()
    {
        if (warningSign == null) return;

        float t = Mathf.PingPong(Time.time * blickSpeed, 1f);

        // alpha 범위로 변환
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, t);

        Color c = warningSign.color;
        c.a = alpha;
        warningSign.color = c;
    }

    GameObject temp;

    private void ChangeWay()
    {
        switch (way)
        {
            case Way.None:
                break;
            case Way.Up:
                way = Way.Down;
                if(arrowType == ArrowType.Trap)
                {
                    // trap 생성, 떨궈주는 로직은 따로  Trap에 적용
                    temp = Instantiate(GameManager.Instance.TrapPrefab);
                    temp.transform.position = warningSign.transform.position + Vector3.up * 6f;
                    Destroy(gameObject);
                }
                else
                {
                    transform.position = target.transform.position + Vector3.up * 6f;
                    transform.rotation = Quaternion.Euler(0, 0, 180f);
                }
                time = 0;
                break;
            case Way.Down:
                way = Way.Up;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                // 예고된 좌표에서 나오게 해야함.
                // 좌표의 경우 Init으로 설정해주기.
                if (!mySprite.enabled)
                {
                    mySprite.enabled = true;
                    warningSign.color = new Color(1f, 1f, 1f, 0f);
                }
                if (warningSign != null)
                {
                    transform.position = warningSign.transform.position;
                    particle.Play();
                }
                time = 0;
                break;
            default:
                break;
        }
    }

    // 상 하 좌 우 방향에 따라 반대 방향으로 이동하게 해야함.
    private void Movement()
    {
        switch (way)
        {
            case Way.None:
                break;
            case Way.Up:
                // changeCount 가 1 이상 일 경우 등속운동 아닐 경우 가속
                //transform.Translate((changeCount < 1? Vector3.up * time * Time.deltaTime : Vector3.up * gravity * Time.deltaTime));
                transform.Translate((changeCount < 1? Vector3.up * time * Time.deltaTime : Vector3.up * time * Time.deltaTime));
                CustomDebug.Log("올라가는 중");
                break;
            case Way.Down:
                transform.Translate((changeCount < 1 ? Vector3.up * time * Time.deltaTime : Vector3.up * time * Time.deltaTime));
                //transform.Translate((changeCount < 1 ? Vector3.up * time * Time.deltaTime : Vector3.up * gravity * Time.deltaTime));
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance.playType != GameManager.PlayType.Play)
        {
            return;
        }
        // 충돌이 일어나면 위치를 띄워주고 2초 뒤 SpriteRenderer를 활성화 시켜줘야함.
        // 또한 충돌은 way가 Down일 때만 반응함
        if (way== Way.Down && collision.gameObject.layer == 6)
        {
            mySprite.enabled = false;
            particle.Stop();
            //Destroy(gameObject);
        }
        // Entity계열과 충돌시 피해를 주는 옵션 진행.
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

}
