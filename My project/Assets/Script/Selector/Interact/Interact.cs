using SlimeProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeProject
{
    public abstract class Interact : MonoBehaviour
    {
        public bool Use = false;

        public ItemScriptableObject ItemSO;
        public abstract ItemScriptableObject Activated();

        public abstract void SetItems(ItemScriptableObject itemSO);
    }

}