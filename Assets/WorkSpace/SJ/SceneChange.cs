using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public string[] stageScenes; // 이동할 스테이지 씬 목록

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어가 충돌했고, 포탈 오브젝트와 충돌한 경우만 씬 이동
        if (other.CompareTag("Player") && gameObject.CompareTag("Portal"))
        {
            string randomStage = stageScenes[Random.Range(0, stageScenes.Length)];
            SceneManager.LoadScene(randomStage);
        }
    }
}
