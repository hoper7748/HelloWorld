using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeProject
{
    public class Shop : MonoBehaviour
    {
        public ItemPanel[] ItemPanels;

        public ItemScriptableObject[] ItemDatas;

        public void SetItems()
        {
            int randItemDataNum = 0;
            for(int i = 0; i < ItemPanels.Length;i++)
            {
                randItemDataNum = Random.Range(0, ItemDatas.Length);
                ItemPanels[i].ItemSet(ItemDatas[randItemDataNum]);
            }
        }
    }
}