using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SlimeProject
{
    public class ItemPanel : MonoBehaviour
    {
        public Image Icon;

        public TextMeshProUGUI Text;

        public ItemScriptableObject ItemSO;

        public GameObject SoldOutImage;

        private bool use;

        private Button btn;

        private void Awake()
        {
            btn = GetComponent<Button>();
            SoldOutImage.SetActive(false);
        }


        public void ItemSet(ItemScriptableObject itemSO)
        {
            ItemSO = itemSO;
            Icon.sprite = ItemSO.Sprite;
            if(itemSO.type == ItemType.Crystal )
            {
                Text.text = ItemSO.Dialogue + $"가격)" + ItemSO.Price.ToString();
            }
            else
            {
                Text.text = ItemSO.Dialogue;
            }
            SoldOutImage.SetActive(false);
            btn.enabled = true;
            use = false;
        }

        public void Activated()
        {
            if(UIManager.Instance.HaveCryStal >= ItemSO.Price)
            {
                btn.enabled = false;
                use = true;
                UIManager.Instance.UseCryStal(ItemSO.Price);
                GameObject.Find("Player").GetComponent<Player>().ActivatedItem(ItemSO);
                SoldOutImage.SetActive(true);  
            }
        }
    }

}