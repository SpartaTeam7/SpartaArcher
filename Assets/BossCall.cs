using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCall : MonoBehaviour
{
    public GameObject boss;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            boss.SetActive(true);
        }
    }
}
