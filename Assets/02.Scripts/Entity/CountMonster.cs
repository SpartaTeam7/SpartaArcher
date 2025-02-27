using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountMonster : MonoBehaviour
{
    [SerializeField] private int Level = 0;

    private EnemyManager enemyManager;
    private GameManager gameManager;

    private void Start()
    {
        enemyManager = EnemyManager.Instance;
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if(Level == gameManager.currentLevel)
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyManager.StartStage();
        }
    }
}
