using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectA : MonoBehaviour
{
    public GameObject obstaclePrefab; //장애물 프리팹 설정
    public int minObstacles = 1;  //생성될때 최소 장애물 개수
    public int maxObstacles = 2; //생성될때 최대 장애물 개수
    public Vector2 spawnAreaMin = new Vector2(-8f, -4f); //생성 영역 최소 좌표
    public Vector2 spawnAreaMax = new Vector2(8f, 4f);  //스폰 영역 최대 좌표

    private GameObject[] spawnedObstacles;  //생성된 장애물들을 담을 배열

    void Start()
    {
        SpawnObstacles();
    }

    void SpawnObstacles()
    {
        int obstacleCount = Random.Range(minObstacles, maxObstacles + 1); //랜덤한 생성 개수
        spawnedObstacles = new GameObject[obstacleCount];

        for (int i = 0; i < obstacleCount; i++) //랜덤한 생성 좌표
        {
            Vector2 spawnPosition = new Vector2(   
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            spawnedObstacles[i] = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        }
    }
    
    

}
