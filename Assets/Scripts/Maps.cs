using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maps : MonoBehaviour
{
   public int obstacleCount = 10;
   public Vector2 spawnRangeX = new Vector2( -10f, 10f);
   public Vector2 spawnRangeZ = new Vector2( -10f, 10f);

   void Start()
   {
      spawnObstacles();
   }

   void spawnObstacles()
   {
      for (int i = 0; i < obstacleCount; i++)
      {
         //랜덤 위치 설정
         float randomX = Random.Range(-spawnRangeX.x, spawnRangeX.y);
         float randomZ = Random.Range(-spawnRangeZ.x, spawnRangeZ.y);
         Vector3 randomPos = new Vector3(randomX, 0, randomZ);
      }
   }
}

