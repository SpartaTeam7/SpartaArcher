using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentLevel;

    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject clearUI;


    private void Awake()
    {
        Instance = this;

        Init();
    }

    public void Init()
    {
        currentLevel = 1;
    }

    public void EndGame()
    {
        OnClearUI();
    }

    private void OnClearUI()
    {
        clearUI.SetActive(true);
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
