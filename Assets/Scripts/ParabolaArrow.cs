using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ParabolaArrow : MonoBehaviour
{
    Entity entity;

    public Vector3 pointA;  // 시작 위치
    public Vector3 pointB;  // 도착 위치
    public Vector3 control; // 최고점 위치 (중간 포인트)

    [Range(0f, 1f)]
    public float t; // 0에서 1까지 진행 정도 (속도는 Update에서 조절)

    public float speed = 1f; // 진행 속도

    public float minHeight;
    public float maxHeight;

    public void SetAToB(Vector2 A, Vector2 B, Entity entity, bool areaAttack = false)
    {
        this.entity = entity;

        pointA = A + Vector2.up * 0.5f;
        if(entity.Target.summonObject != null && !areaAttack)
        {
            pointB = entity.Target.summonObject.transform.position;
        }
        else
        {
            pointB = B;
        }
        pointB.y = -1.5f;

        // 시작점과 목표점 사이 거리
        float distance = Vector3.Distance(pointA, pointB);

        // 거리 기반으로 높이 계산 (0일 때 minHeight, 멀수록 maxHeight)
        float height = Mathf.Lerp(minHeight, maxHeight, distance / 10);
        height = Mathf.Clamp(height, minHeight, maxHeight);

        speed = Mathf.Lerp(0.25f, 1f, distance / 12);

        control = (pointA + pointB) * 0.5f;
        control.y = height;
    }

    //public void SetAToB(Vector2 A, Vector3 B, Entity entity)
    //{

    //    pointA = A + Vector2.up * 0.5f;
    //    pointB = B;

    //    pointB.y = -1.5f;

    //    // 시작점과 목표점 사이 거리
    //    float distance = Vector3.Distance(pointA, pointB);

    //    // 거리 기반으로 높이 계산 (0일 때 minHeight, 멀수록 maxHeight)
    //    float height = Mathf.Lerp(minHeight, maxHeight, distance / 10);
    //    height = Mathf.Clamp(height, minHeight, maxHeight);

    //    speed = Mathf.Lerp(0.25f, 1f, distance / 12);

    //    control = (pointA + pointB) * 0.5f;
    //    control.y = height;
    //}

    private void Update()
    {
        // 시간에 따라 t 증가
        t += Time.deltaTime / speed;
        if (t > 1f) t = 1f;

        // 베지어 곡선 위치 계산
        Vector3 pos =
            Mathf.Pow(1 - t, 2) * pointA +
            2 * (1 - t) * t * control +
            Mathf.Pow(t, 2) * pointB;

        transform.position = pos;

        Vector3 tangent = 2 * (1 - t) * (control - pointA) + 2 * t * (pointB - control);

        if (tangent != Vector3.zero)
        {
            // 2D Sprite가 오른쪽 기준이라면
            transform.right = tangent.normalized;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(GameManager.Instance.playType != GameManager.PlayType.Play)
        {
            return;
        }

        // 이거 나랑 부딧히면 사라진다 그걸 방지하자
        if (collision.GetComponent<Entity>() != null && collision.GetComponent<Entity>()?.PlayerType != entity.PlayerType)
        {
            CustomDebug.Log($"{collision.gameObject.name}");
            collision.GetComponent<Entity>()?.GetDamaged(entity.ATK);
            // 데미지 주고 사라짐
            Destroy(gameObject);
        }


        if(collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
    }
}
