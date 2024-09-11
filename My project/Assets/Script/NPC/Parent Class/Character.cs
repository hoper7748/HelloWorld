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

        [HideInInspector] public float Hp;

        [HideInInspector] public float Maxhp;

        [HideInInspector] public float Damage;
        [HideInInspector] public float AttackInterval;
        protected float curAtkTime = 0;
        
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

        public virtual void Attack()
        {
            if(curAtkTime > AttackInterval)
            {
                for (int i = 0; i < Colliders.Length; i++)
                {
                    Colliders[i].GetComponent<Character>()?.Damaged(Damage);
                }
                curAtkTime = 0;
            }
        }

        public virtual void Damaged(float damage)
        {
            Hp -= damage;
            if (Hp <= 0)
            {
                gameObject.SetActive(false);
            }

        }
    }

}