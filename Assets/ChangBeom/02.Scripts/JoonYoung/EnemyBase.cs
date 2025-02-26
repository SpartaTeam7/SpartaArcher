using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    public float maxHp = 1000f;
    public float currentHp = 1000f;

    public float damage = 100f;

    protected float playerRealizeRange = 10f;
    protected float attackRange = 5f;
    protected float attackCoolTime = 5f;
    protected float attackCoolTimeCacl = 5f;
    protected bool canAtk = true;

    protected float moveSpeed = 2f;

    protected GameObject player;
    protected NavMeshAgent nvAgent;
    protected float distance;

    protected GameObject parentRoom;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected LayerMask LayerMask;

    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        nvAgent = player.GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        parentRoom = transform.parent.transform.parent.gameObject;

        StartCoroutine(CalcCoolTime());
    }

    protected bool CanAtkstatefun()
    {
        Vector3 targetDir = new Vector3(player.transform.position.x
            - transform.position.x, 0f, player.transform.position.z - transform.position.z);

        Physics.Raycast(new Vector3(transform.position.x, 0.5f, transform.position.z),
            targetDir, out RaycastHit hit, 30f, LayerMask);

        if (hit.transform == null)
        {
            Debug.Log("hit.transform == null");
            return false;
        }

        if (hit.transform.CompareTag("Player") && distance <= attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected virtual IEnumerator CalcCoolTime()
    {
        while (true)
        {
            yield return null;
            if (!canAtk)
            {
                attackCoolTimeCacl -= Time.deltaTime;
                if (attackCoolTimeCacl <= 0)
                {
                    attackCoolTimeCacl = attackCoolTime;
                    canAtk = true;
                }
            }
        }
    }
    public class Projectile : MonoBehaviour
    {
        public float damage = 20f; // 발사체의 데미지

        // 충돌 시 처리되는 로직
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("Player")) // 플레이어와 충돌한 경우
            {
                // 플레이어에 피해를 입히는 로직
                // collision.transform.GetComponent<Player>().TakeDamage(damage); // 플레이어의 TakeDamage 함수 호출
                Destroy(gameObject);  // 발사체 제거
            }
            else if (collision.transform.CompareTag("Wall")) // 벽과 충돌한 경우
            {
                Destroy(gameObject); // 발사체 제거
            }
        }
    }
}
