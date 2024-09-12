using UnityEngine;
using UnityEngine.SceneManagement;

namespace SlimeProject
{
    public class FieldManager : MonoBehaviour
    {
        private static FieldManager instance;
        public static FieldManager Instance
        {
            get { return instance;  }
        }

        public Vector2 gridSize;

        public float NodeRadius = 0;
        public bool displayGridGizmos;

        public float YOffset = 0.5f;

        public Transform[] ScrollingField;

        [ReadOnly]
        public float Meters = 0;

        [HideInInspector]
        public bool Scrolling = false;
        [HideInInspector]
        public bool OpenUI = false;
        [HideInInspector, ReadOnly] // 약 100 미터 마다 강화 
        public int Level = 0;

        public float ScrollSpeed = 0;
        public float resetOffset = 1;
        [SerializeField, ReadOnly]
        private float resetPositionY = 0;
        [SerializeField, ReadOnly]
        private float startPositionY = 0;
        [SerializeField, ReadOnly]
        public bool BossSpawn = false;

        int gridSizeX, gridSizeY;
        float nodeDiameter;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            Scrolling = true;
        }

        private void Start()
        {
            resetPositionY = ScrollingField[0].position.y;
            
            startPositionY = ScrollingField[ScrollingField.Length - 1].position.y;
        }

        private void FixedUpdate()
        {
            if (Scrolling)
                ScrollUpdate();
        }

        private void ScrollUpdate()
        {
            for(int i = 0; i < ScrollingField.Length; i++)
            {
                ScrollingField[i].position += Vector3.down * ScrollSpeed * Time.deltaTime;
                Meters += ScrollSpeed * 0.1f * Time.deltaTime ;
                // 특정 위치에 도달하면 필드를 처음 위치로 되돌림
                if (ScrollingField[i].position.y <= resetPositionY)
                {
                    ScrollingField[i].position = ScrollingField[i - 1 < 0 ? ScrollingField.Length -1: i - 1].position + Vector3.up * 4.75f ;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector2(gridSize.x, gridSize.y));

            nodeDiameter = NodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridSize.y / nodeDiameter);

            resetPositionY = -(gridSize.y / 2);

            if (displayGridGizmos)
            {
                for (int x = 0; x < gridSizeX; x++)
                {
                    for (int y = 0; y < gridSizeY; y++)
                    {
                        Gizmos.color = (x + y) % 2 == 1 ? Color.white : Color.black;
                        Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.25f);

                        Gizmos.DrawCube(new Vector2(x - (gridSize.x *0.5f ) + 0.5f, y - (gridSize.y * 0.5f) + YOffset), Vector3.one * nodeDiameter);
                    }
                }
            }
        }

        public void LevelUp()
        {
            Level++;
        }

        public void StopGame()
        {
            Scrolling = false;
            OpenUI = true;
        }

        public void StartGame()
        {
            Scrolling = true;
            OpenUI = false;
        }

        public void Replay()
        {
            ManagerReset();
            
            SceneManager.LoadScene("Game Scene");
        }

        public void EndGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        public void ManagerReset()
        {
            Level = 0;
            Meters = 0;
            UIManager.Instance.ManagerReset();
        }

    }
}