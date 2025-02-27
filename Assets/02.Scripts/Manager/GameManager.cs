using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentLevel;

    private EnemyManager enemyManager;
    
    private void Awake()
    {
        Instance = this;

        Init();
        enemyManager = GetComponentInChildren<EnemyManager>();

    }

    public void Init()
    {
        currentLevel = 1;
    }

    public void StartGame()
    {
        //  ȣ��Ǹ� ������ ���۵�
    }

    public void EndGame()
    {
        //  ������ ������ ������ ����ǰ� ����ȭ������ ���ư�
    }

    public void GameOver()
    {
        //  �÷��̾��� HP�� 0�̵Ǹ� GameOver���� UI�� �����
    }
}
