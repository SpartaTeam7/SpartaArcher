using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;

    public void OnEnable()
    {
        restartButton.onClick.AddListener(OnClickRestartButton);
        menuButton.onClick.AddListener(OnClickMenuButton);
        Time.timeScale = 0;
    }

    public void OnClickRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void OnClickMenuButton()
    {
        SceneManager.LoadScene("StartScene");
        Time.timeScale = 1;
    }
}
