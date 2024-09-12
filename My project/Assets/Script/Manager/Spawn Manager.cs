using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Cysharp.Threading.Tasks;

namespace SlimeProject
{
    public class SpawnManager : MonoBehaviour
    {

        // 무한 생성은 에바니 약 100마리 정도 생성해 놓자. 
        public Vector3 SpawnOffset = Vector3.zero;
        public float SpawnRadius = 0;


        [Header("생성 오브젝트") ,Space(10f)]
        public GameObject[] SpawnTargetPrefab;
        public GameObject SpawnBossPrefab;

        public Selector SelectorObject;

        public CryStal SpawnCrystal;

        [HideInInspector]
        public List<GameObject[]> MonsterList = new List<GameObject[]>();
        [HideInInspector]
        public GameObject[] Monsters;
        [HideInInspector] 
        public GameObject BossMonster;

        [HideInInspector]
        public GameObject[] CryStals;


        public int MaxSpawnSize = 5;

        public float SpawnTimer = 1;


        private int curMonsterCount = 0;
        private int curCrystalCount = 0;
        private float currentTimer = 0;
        private List<int> SpawnNum = new List<int> { 0, 1, 2, 3, 4 };

        private void SetObjectPools()
        {
            Monsters = new GameObject[30];
            for (int i = 0; i < 30; i++)
            {
                Monsters[i] = Instantiate(SpawnTargetPrefab[FieldManager.Instance.Level]);
                Monsters[i].transform.parent = transform;
                Monsters[i].transform.position = Vector3.down * 10f;
                Monsters[i].SetActive(false);
                Monsters[i].name += $" {i}";
            }
            MonsterList.Add(Monsters);
            FieldManager.Instance.LevelUp();


        }

        private void Awake()
        {

            SetObjectPools();

            CreateBossMonster();
            CryStals = new GameObject[10];
            for(int i =0; i < 10; i++)
            {
                CryStals[i] = Instantiate(SpawnCrystal.gameObject);
                CryStals[i].transform.parent = transform;
                CryStals[i].SetActive(false);
                CryStals[i].name += $" {i}";
            }
            curCrystalCount = 0;
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
                    SpawnTimer = (int)Random.Range(1, 4) * 0.5f;
                    currentTimer = 0;
                }
            }
        }
        private bool SpawnSelector()
        {
            // 5미터 마다 선택지 생성
            if ((int)FieldManager.Instance.Meters % 10 < 1)
            {
                if(FieldManager.Instance.Level < SpawnTargetPrefab.Length)
                    SetObjectPools();
                SelectorObject.gameObject.SetActive(true);
                SelectorObject.InitSelector();
                SelectorObject.transform.position = new Vector2(0, transform.position.y + SpawnOffset.y);
                return true;
            }
            return false;
        }

        private void SpawnMonster()
        {
            var random = new System.Random();
            // 최대 5마리
            int spawnCount = Random.Range(1, MaxSpawnSize);
            int randomLevel = 0;
            SpawnNum = SpawnNum.OrderBy(_ => random.Next()).ToList();
            
            for (int i = 0; i < spawnCount; i++)
            {
                randomLevel = Random.Range(0, FieldManager.Instance.Level);
                ShowObject(MonsterList[randomLevel][curMonsterCount++], i);
                if (curMonsterCount >= Monsters.Length)
                    curMonsterCount = 0;
            }
        }

        private void SpawnCryStal()
        {
            var random = new System.Random();
            // 최대 5마리
            int spawnCount = Random.Range(1, 3);
            SpawnNum = SpawnNum.OrderBy(_ => random.Next()).ToList();

            for (int i = 0; i < spawnCount; i++)
            {
                ShowObject(CryStals[curCrystalCount++], i);
                if (curCrystalCount >= CryStals.Length)
                    curCrystalCount = 0;
            }
        }
        private void CreateBossMonster()
        {
            BossMonster = Instantiate(SpawnBossPrefab);
            BossMonster.SetActive(false);
            BossMonster.transform.position = new Vector2(0, transform.position.y + SpawnOffset.y);
        }

        private async UniTaskVoid SpawnBossMonster()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            BossMonster.SetActive(true);
            BossMonster.transform.position = new Vector2(2 - (SpawnRadius * 0.5f) + .5f, transform.position.y + SpawnOffset.y);

        }

        // 생성은 약 1초 간격 
        private void Spawn()
        {
            if(!FieldManager.Instance.BossSpawn)
            {
                if ((int)FieldManager.Instance.Meters % 25 < 1)
                {
                    FieldManager.Instance.BossSpawn = true;
                    SpawnBossMonster().Forget();
                    return;
                }

                if (SpawnSelector())
                {
                    return;
                }
                int spawnPercent = Random.Range(0, 100);


                if (spawnPercent < 25)
                {
                    SpawnCryStal();
                }
                else
                {
                    SpawnMonster();
                }
            }

        }

        private void ShowObject(GameObject spawnObject, int num)
        {
            spawnObject.SetActive(true);
            spawnObject.transform.position = new Vector2(SpawnNum[num] - (SpawnRadius * 0.5f) + 0.5f, transform.position.y + SpawnOffset.y);
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