using UnityEngine;

namespace SlimeProject
{
    public class Selector : MonoBehaviour
    {
        public Panel LeftSelector;
        public Panel RightSelector;



        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (FieldManager.Instance.Scrolling)
            {
                transform.position += Vector3.down * FieldManager.Instance.ScrollSpeed * Time.deltaTime;

            }
        }


        public void InitSelector()
        {
            // 랜덤한 Sprite를 넣어야
        }
    }

}