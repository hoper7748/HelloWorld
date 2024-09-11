//using JetBrains.Annotations;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;

//namespace SlimeProject
//{
//    public class CollisionManager : MonoBehaviour
//    {
//        private static CollisionManager instance = null;
//        public static CollisionManager Instance
//        {
//            get
//            {
//                return instance;
//            }
//        }

//        [SerializeField]
//        private List<CustomCollision> collisions = new List<CustomCollision>();

//        private void Awake()
//        {
//            if (instance == null)
//            {
//                instance = this;
//            }
//            else
//                Destroy(gameObject);
//        }

//        // Start is called before the first frame update
//        void Start()
//        {
//            // 게임이 진행되면 일정 구간마다 생성을 해줘야함.
//            // 시간대 별로 일정 구간마다 생성되게 하고 싶은데..
            
//        }

//        public void AddCollision(CustomCollision collision)
//        {
//            collisions.Add(collision);
//        }

//        public void SubCollision(CustomCollision collision)
//        {
//            collisions.Remove(collision);
//        }

//        List<CustomCollision> hitContainers = new List<CustomCollision>();
//        public List<CustomCollision> CheckCollision(Transform t, string tag) 
//        {
//            hitContainers.Clear();
//            foreach (var col in collisions)
//            {
//                if (col.CollisionEnter(t, tag))
//                {
//                    hitContainers.Add(col);
//                }
//            }
//            return hitContainers;
//        }

//        public List<CustomCollision> CheckCollision(Vector3 v, string tag)
//        {
//            hitContainers.Clear();
//            foreach (var col in collisions)
//            {
//                if (col.CollisionEnter(v, tag))
//                {
//                    hitContainers.Add(col);
//                }
//            }
//            return hitContainers;
//        }
//    }
//}