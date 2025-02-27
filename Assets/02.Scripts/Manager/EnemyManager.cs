using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    [SerializeField]
    private List<GameObject> enemyPrefabs; // ������ �� ������ ����Ʈ
    public List<GameObject> monsterList; // ���� Level�� �����ϴ� ���� ���

    //  ���Ͱ� ������ ��
    public int minMonsters = 3;
    public int maxMonsters = 5;

    public bool isClear = false;
    public GameObject skillUpSlot;

    //  ���� ���� ��ġ
    public Vector2 spawnAreaMin = new Vector2(0f, 0f);
    public Vector2 spawnAreaMax = new Vector2(8f, 7f);
    private List<Vector2> monsterPositions;

    public float minPadding = 2f;

    private GameManager gameManager;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void StartStage()
    {
        isClear = false;
        if(gameManager.currentLevel < 4)
        {
            SpawnMonster();
        }
    }

    public void SpawnMonster()
    {
        Vector2 mapPosition = new Vector2(gameManager.currentLevel * 25f - 25f, 0);
        monsterPositions = new List<Vector2>();
        int monsterCount = Random.Range(minMonsters, maxMonsters + 1);

        for (int i = 0; i < monsterCount; i++)
        {
            Vector2 spawnPosition = Vector2.zero;
            bool positionCheck = false;

            while (!positionCheck)
            {
                spawnPosition = new Vector2(
                    Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                    Random.Range(spawnAreaMin.y, spawnAreaMax.y)
                );

                spawnPosition += mapPosition;

                positionCheck = true;

                foreach (Vector2 pos in monsterPositions)
                {
                    if (Vector2.Distance(spawnPosition, pos) < minPadding)
                    {
                        positionCheck = false;
                        break;
                    }
                }
            }
            int monsterIndex = Random.Range(0, enemyPrefabs.Count);
            Instantiate(enemyPrefabs[monsterIndex], spawnPosition, Quaternion.identity);
            monsterPositions.Add(spawnPosition);
        }
    }

    public void RemoveEnemyOnDeath(GameObject enemy)
    {
        monsterList.Remove(enemy);
    }
    public void CheckStageClear()
    {
        if (monsterList.Count == 0) 
        {
            // gameManager.StageClear();
            Debug.Log("good");
            skillUpSlot.SetActive(true);
        }
    }
}
