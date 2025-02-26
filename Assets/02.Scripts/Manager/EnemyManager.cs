using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Coroutine waveRoutine;

    [SerializeField]
    private List<GameObject> enemyPrefabs; // 생성할 적 프리팹 리스트

    [SerializeField]
    private List<Rect> spawnAreas; // 적을 생성할 영역 리스트

    [SerializeField]
    private Color gizmoColor = new Color(1, 0, 0, 0.3f); // 기즈모 색상

    private List<EnemyController> activeEnemies = new List<EnemyController>(); // 현재 활성화된 적들

    private bool enemySpawnComplite;

    [SerializeField] private float timeBetweenSpawns = 0.2f;
    [SerializeField] private float timeBetweenWaves = 1f;

    GameManager gameManager;
    //  playercontroller에 있는 monsterList를 여기로 옮겨야함

    public void StartStage()
    {
        //  스테이지가 시작되고 랜덤한 위치에 몬스터가 스폰됨
    }

    public void SpawnMonster()
    {
        //  랜덤한 위치에 스테이지에 맞는 수의 몬스터를 스폰
        //  스폰된 몬스터는 monsterList에 저장
    }
    public void RemoveEnemyOnDeath(EnemyController enemy)
    {
        activeEnemies.Remove(enemy);
        //모든 적 처치시 다음 층으로 이동 가능
    }


}
