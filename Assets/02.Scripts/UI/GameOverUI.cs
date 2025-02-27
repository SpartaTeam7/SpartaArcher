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
    }

    public void OnClickRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickMenuButton()
    {
        SceneManager.LoadScene("StartScene");
    }
}
