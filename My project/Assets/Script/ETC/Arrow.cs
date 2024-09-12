using SlimeProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Vector3 MoveDirection;

    private float damage = 0;
    private float speed = 0;
    private LayerMask targetLayer;
    private Collider2D[] colliders;

    // Update is called once per frame
    void Update()
    {
        float nowSpeed = FieldManager.Instance.Scrolling ? speed : speed - FieldManager.Instance.ScrollSpeed;
        if(!FieldManager.Instance.OpenUI)
        {
            transform.position += MoveDirection * nowSpeed * Time.deltaTime;
            CheckHit();
            if (transform.position.y > 10f || transform.position.y < -10f)
            {
                gameObject.SetActive(false);
            }
        }
    }
    
    private void CheckHit()
    {
        Vector2 boxCenter = transform.position ;

        // 박스 영역 내의 충돌체 탐색 (colliders 배열에 충돌한 콜라이더 저장)
        colliders = Physics2D.OverlapBoxAll(boxCenter, Vector3.one * 0.5f, 0f, targetLayer);

        if(colliders.Length > 0 )
        {
            foreach (var col in colliders)
            {
                col.GetComponent<Character>()?.Damaged(damage);
            }
            gameObject.SetActive(false);
        }

    }

    public void SetArrow(float damage, float speed, LayerMask targetLayer)
    {
        this.damage = damage;
        this.speed = speed;
        this.targetLayer = targetLayer;
    }
}
