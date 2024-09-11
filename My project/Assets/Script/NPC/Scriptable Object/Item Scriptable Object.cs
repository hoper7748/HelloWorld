using UnityEngine;


namespace SlimeProject
{
    public enum WeaponType
    {
        None = 0, // 기타 특수효과 추가할 때,
        Shop,
        Sword,
        Bow,
        Saw,
    }

    [CreateAssetMenu(fileName = "Items", menuName = "ScriptableObject/Items", order = 0)]
    public class ItemScriptableObject : ScriptableObject
    {
        public Sprite Sprite;
        
        public WeaponType type;

        [Header("데미지 증가")]
        public float Damage;

        [Header("공격 빈도 증가")]
        public float Speed;

        [Header("체력 증가")]
        public float Heal;
    }
}