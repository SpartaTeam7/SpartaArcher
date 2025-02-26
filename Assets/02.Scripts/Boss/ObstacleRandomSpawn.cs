using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRandomSpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefabs; //장애물 프리팹 설정
    public int minObstacles = 5;  //생성될때 최소 장애물 개수
    public int maxObstacles = 10; //생성될때 최대 장애물 개수

    public Vector2 spawnAreaMin = new Vector2(-8f, -8f); //생성 영역 최소 좌표
    public Vector2 spawnAreaMax = new Vector2(8f, 7f);  //스폰 영역 최대 좌표

    private List<Vector2> obstaclePositions;  //생성된 장애물들을의 위치를 저장하는 리스트

    public float minPadding = 2f;

    private void Start()
    {
        SpawnObstacles();
    }

    public void SpawnObstacles()
    {
        Vector2 mapPosition = transform.position;
        obstaclePositions = new List<Vector2>();
        int obstacleCount = Random.Range(minObstacles, maxObstacles + 1); //랜덤한 생성 개수

        for (int i = 0; i < obstacleCount; i++) //랜덤한 생성 좌표
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

                foreach (Vector2 pos in obstaclePositions)
                {
                    if (Vector2.Distance(spawnPosition, pos) < minPadding)
                    {
                        positionCheck = false;
                        break;
                    }
                }
            }

            int obstacleIndex = Random.Range(0, obstaclePrefabs.Length);
            Instantiate(obstaclePrefabs[obstacleIndex], spawnPosition, Quaternion.identity);
            obstaclePositions.Add(spawnPosition);
        }
    }



}
