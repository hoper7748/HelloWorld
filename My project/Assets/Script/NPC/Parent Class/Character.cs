using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace SlimeProject
{
    public abstract class Character : MonoBehaviour
    {
        public CharacterScriptableObject CharacterSO;
        public SpriteRenderer MainSprite;
        public LayerMask targetLayer;
        public Sprite[] AnimSprites;
        [HideInInspector , ReadOnly]
        public Collider2D[] Colliders = new Collider2D[10];

        //[Space(10f), Header("Character Info")]
        [ReadOnly]
        public float Hp;

        public Color MainColor;
        public Color DamageColor;

        [HideInInspector] public float Maxhp;

        [HideInInspector] public float Damage;
        [HideInInspector] public float AttackInterval;
        protected float curAtkTimer = 0;
        
        protected int CurrentSprite = 0;

        public float AnimPlayTime = 0;
        [HideInInspector] protected float curAnimTimer = 0;
        public virtual void PlaySpriteAnimation()
        {
            MainSprite.sprite = AnimSprites[CurrentSprite++];
            if (CurrentSprite >= AnimSprites.Length)
            {
                CurrentSprite = 0;
            }
        }

        public virtual void NormalAttack()
        {
            if(curAtkTimer > AttackInterval)
            {
                for (int i = 0; i < Colliders.Length; i++)
                {
                    Colliders[i].GetComponent<Character>()?.Damaged(Damage);
                }
                curAtkTimer = 0;
            }
        }


        public virtual void Damaged(float damage)
        {
            Hp -= damage;
            if (Hp <= 0)
            {
                transform.position = Vector3.down * 10f;
                if (!(this as Player))
                {
                    gameObject.SetActive(false);
                    return;
                }
                else
                {
                    FieldManager.Instance.StopGame();
                    UIManager.Instance.GameOver();
                    return;
                }
            }
            ColorChange().Forget();
        }

        async UniTaskVoid ColorChange()
        {
            MainSprite.material.color = DamageColor;
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f));
            MainSprite.material.color = Color.white;

        }

        public virtual void ScrollingCharacter()
        {

        }

        public virtual void PlayAnim()
        {

        }
    }

}