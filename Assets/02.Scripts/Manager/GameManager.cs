using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentLevel;

    [SerializeField] GameObject gameOverUI;
    
    private void Awake()
    {
        Instance = this;

        Init();
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
        Invoke("OnGameOverUI", 2f);
    }

    private void OnGameOverUI()
    {
        gameOverUI.SetActive(true);
    }
}
