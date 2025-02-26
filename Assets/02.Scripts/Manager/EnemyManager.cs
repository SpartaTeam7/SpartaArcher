using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Coroutine waveRoutine;

    [SerializeField]
    private List<GameObject> enemyPrefabs; // ������ �� ������ ����Ʈ

    [SerializeField]
    private List<Rect> spawnAreas; // ���� ������ ���� ����Ʈ

    [SerializeField]
    private Color gizmoColor = new Color(1, 0, 0, 0.3f); // ����� ����

    private List<EnemyController> activeEnemies = new List<EnemyController>(); // ���� Ȱ��ȭ�� ����

    private bool enemySpawnComplite;

    [SerializeField] private float timeBetweenSpawns = 0.2f;
    [SerializeField] private float timeBetweenWaves = 1f;

    GameManager gameManager;
    //  playercontroller�� �ִ� monsterList�� ����� �Űܾ���

    public void StartStage()
    {
        //  ���������� ���۵ǰ� ������ ��ġ�� ���Ͱ� ������
    }

    public void SpawnMonster()
    {
        //  ������ ��ġ�� ���������� �´� ���� ���͸� ����
        //  ������ ���ʹ� monsterList�� ����
    }
    public void RemoveEnemyOnDeath(EnemyController enemy)
    {
        activeEnemies.Remove(enemy);
        //��� �� óġ�� ���� ������ �̵� ����
    }


}
