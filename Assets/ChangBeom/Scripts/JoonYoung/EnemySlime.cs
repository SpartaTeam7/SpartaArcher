using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : enemyMeLeeFSM
{
    public GameObject enemyCanvasGo;
    public GameObject meleeAtkArea;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerRealizeRange);

    }
}
