using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeProject
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;

        public static UIManager Instance
        {
            get
            {
                return instance;
            }
        }

        [Header("크리스탈 UI 관련")]
        public TextMeshProUGUI CryStalText;
        [ReadOnly]
        public int HaveCryStal = 0;

        [Header("Item Panel")]
        public Image[] GetItemList;
        private int currentItemCount = 0;
        public Dictionary<ItemScriptableObject, Image> ItemDictionary = new Dictionary<ItemScriptableObject, Image>();

        public Shop ShopPanel;

        public GameObject SettingPanel;

        public GameObject GameOverPanel;

        [Space(10)]
        public Image PlayerHpBar;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            SettingPanel.SetActive(false);
            ShopPanel.gameObject.SetActive(false);
            GameOverPanel.SetActive(false);
        }

        public void OpenShop()
        {
            FieldManager.Instance.StopGame();
            ShopPanel.gameObject.SetActive(true);
            ShopPanel.SetItems();
        }
        public void CloseShop()
        {
            FieldManager.Instance.StartGame();
            ShopPanel.gameObject.SetActive(false);
        }

        public void OpenSetting()
        {
            SettingPanel.SetActive(true);
            FieldManager.Instance.StopGame();
        }

        public void CloseSetting()
        {
            SettingPanel.SetActive(false);
            FieldManager.Instance.StartGame();
        }

        public void GameOver()
        {
            GameOverPanel.SetActive(true);
        }

        public void AddCryStal(int crystal)
        {
            HaveCryStal += crystal;
            CryStalText.text = HaveCryStal.ToString();
        }

        public void UseCryStal(int cryStal)
        {
            HaveCryStal -= cryStal;
            CryStalText.text = HaveCryStal.ToString();
        }

        public void GetItem(ItemScriptableObject item)
        {
            if (!ItemDictionary.ContainsKey(item))
            {
                ItemDictionary.Add(item, GetItemList[currentItemCount]);
                GetItemList[currentItemCount].sprite = item.Sprite;
            }
        }

        public void UpdateHpBar(float maxHp, float curHp)
        {
            PlayerHpBar.fillAmount = curHp / maxHp;
        }

        public void ManagerReset()
        {
            HaveCryStal = 0;
            CryStalText.text = HaveCryStal.ToString();
        }
    }

}