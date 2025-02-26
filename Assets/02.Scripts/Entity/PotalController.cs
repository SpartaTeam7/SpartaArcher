using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PotalController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private CameraController playerCamera;

    private GameManager gameManager;
    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어가 포탈에 들어왔을 경우
        if (other.CompareTag("Player"))
        {
            player.transform.position = new Vector2(-1 + (gameManager.currentLevel * 25), -7);
            playerCamera.center = new Vector2(gameManager.currentLevel * 25, 0);
            gameManager.currentLevel++;
        }
    }
}
