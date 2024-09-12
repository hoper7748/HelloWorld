using Unity.VisualScripting;
using UnityEngine;

namespace SlimeProject
{
    public class Panel : Interact
    {
        public SpriteRenderer Icon;

        public override ItemScriptableObject Activated()
        {
            Use = true;
            return ItemSO;
        }

        public override void SetItems(ItemScriptableObject itemSO)
        {
            ItemSO = itemSO;
            Icon.sprite = ItemSO.Sprite;
        }

    }

}