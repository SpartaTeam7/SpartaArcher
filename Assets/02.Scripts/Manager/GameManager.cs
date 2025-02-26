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
    private void Start()
    {
        
    }

    public void Init()
    {
        currentLevel = 1;
    }

    public void StartGame()
    {
        //  호출되면 게임이 시작됨
    }

    public void EndGame()
    {
        //  보스를 잡으면 게임이 종료되고 메인화면으로 돌아감
    }

    public void GameOver()
    {
        //  플레이어의 HP가 0이되면 GameOver관련 UI를 출력함
    }
}
