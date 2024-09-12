using UnityEngine;

namespace SlimeProject
{
    public class Selector : MonoBehaviour
    {
        public Panel LeftSelector;
        public Panel RightSelector;

        public ItemScriptableObject[] itemScriptableObjects;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(LeftSelector.Use || RightSelector.Use)
            {
                LeftSelector.Use = true;
                RightSelector.Use = true;
            }

            if (FieldManager.Instance.Scrolling)
            {
                transform.position += Vector3.down * FieldManager.Instance.ScrollSpeed * Time.deltaTime;
                if (transform.position.y < -5)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        public void OnEnable()
        {
            InitSelector();
        }

        public void InitSelector()
        {
            // 랜덤한 Sprite를 넣어야
            LeftSelector.gameObject.SetActive(true);
            LeftSelector.SetItems(itemScriptableObjects[Random.Range(0, itemScriptableObjects.Length)]);

            RightSelector.gameObject.SetActive(true);
            RightSelector.SetItems(itemScriptableObjects[Random.Range(0, itemScriptableObjects.Length)]);
        }
    }

}