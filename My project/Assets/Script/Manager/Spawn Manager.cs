using UnityEngine;

namespace SlimeProject
{
    public class SpawnManager : MonoBehaviour
    {
        // 무한 생성은 에바니 약 100마리 정도 생성해 놓자. 
        public Vector3 SpawnOffset = Vector3.zero;
        public float SpawnRadius = 0;

        [Header("생성 오브젝트") ,Space(10f)]
        public GameObject SpawnTargetPrefab;

        public GameObject SpawnSelector;
        [HideInInspector]
        public GameObject[] Monsters;

        private int curMonsterCount = 0;

        public int MaxSpawnSize = 5;

        public float SpawnTimer = 1;
        private float currentTimer = 0;

        private void Awake()
        {
            Monsters = new GameObject[100];
            for (int i=0; i < 100; i++)
            {
                Monsters[i] = Instantiate(SpawnTargetPrefab);
                Monsters[i].transform.parent = transform;
                Monsters[i].SetActive(false);
                Monsters[i].name += $" {i}"; 
            }
            curMonsterCount = 0;
        }

        // 스폰될 위치 
        private void Update()
        {
            if (FieldManager.Instance.Scrolling)
            {
                currentTimer += Time.deltaTime;
                if (SpawnTimer < currentTimer)
                {
                    Spawn();
                    currentTimer = 0;
                }
            }
        }

        // 생성은 약 1초 간격 
        private void Spawn()
        {
            // 최대 5마리
            int spawnCount = Random.Range(1, MaxSpawnSize);

            for (int i = 0; i < spawnCount; i++)
            {
                ShowMonster(Monsters[curMonsterCount++], i);
                if (curMonsterCount >= Monsters.Length)
                    curMonsterCount = 0;
            }
        }

        private void ShowMonster(GameObject monster, int num)
        {
            monster.SetActive(true);
            monster.transform.position = new Vector2(num - (SpawnRadius * 0.5f) + 0.5f, transform.position.y + SpawnOffset.y);

        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + SpawnOffset, new Vector2(SpawnRadius, 1));

            Gizmos.color = Color.blue;
            for (int i = 0; i < SpawnRadius; i++)
            {
                Gizmos.color = i % 2 == 1 ? Color.blue : Color.green;
                Gizmos.DrawCube(new Vector2(i - (SpawnRadius * 0.5f) + 0.5f, transform.position.y + SpawnOffset.y), Vector3.one * SpawnRadius / 5f);
            }

        }
    }
}