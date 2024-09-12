using SlimeProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeProject
{
    public class CryStal : Interact
    {
        //public ItemScriptableObject So;
        public override ItemScriptableObject Activated()
        {
            return ItemSO;
        }

        public override void SetItems(ItemScriptableObject itemSO)
        {
            ItemSO = itemSO;
        }

        private void Update()
        {
            // 이동
            if (FieldManager.Instance.Scrolling)
            {
                transform.position += Vector3.down * FieldManager.Instance.ScrollSpeed * Time.deltaTime;
                if (transform.position.y < -5)
                {
                    gameObject.SetActive(false);
                }
            }

        }
    }
}
