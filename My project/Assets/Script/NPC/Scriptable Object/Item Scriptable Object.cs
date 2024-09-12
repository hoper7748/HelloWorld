using UnityEngine;


namespace SlimeProject
{
    public enum ItemType
    {
        None = 0, // 기타 특수효과 추가할 때,
        Shop,
        Sword,
        Bow,
        Saw,
        Crystal,
    }

    [CreateAssetMenu(fileName = "Items", menuName = "Scriptable Object/Items", order = int.MaxValue)]
    public class ItemScriptableObject : ScriptableObject
    {
        public Sprite Sprite;
        
        public ItemType type;

        [Header("데미지 증가")]
        public float Damage;

        [Header("공격 빈도")]
        public float Interval;

        [Header("투사체 속도")]
        public float Speed;

        [Header("체력 증가")]
        public float Heal;

        [Header("크리스탈 증가")]
        public int AddCryStal;

        [Header("가격")]
        public int Price;

        [Header("설명")]
        public string Dialogue;

        [Header("랜덤 여부")]
        public bool IsRandom;

    }
}