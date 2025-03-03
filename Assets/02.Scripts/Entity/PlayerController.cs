using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{
    private EnemyManager enemyManager;

    [SerializeField] public GameObject target;
    [SerializeField] private GameObject aim;
    [SerializeField] private Transform attackPivot;
    private Transform playerTransform;

    private RangeWeaponHandler rangeWeaponHandler;
    private float fireDelay;
    private int extraAttacks;

    private Vector3 direction;
    public Vector3 Direction
    {
        get => direction;
    }

    private bool isFire = false;

    private void Start()
    {
        playerTransform = GetComponent<Transform>();
        attackPivot = transform.GetChild(1).transform.GetChild(0);
        enemyManager = EnemyManager.Instance;
        rangeWeaponHandler = GetComponentInChildren<RangeWeaponHandler>();
    }
    private void Update()
    {
        fireDelay = rangeWeaponHandler.Delay;
        extraAttacks = rangeWeaponHandler.ExtraAttack;

        if (movementDirection.magnitude < 0.7f)
        {
            OnLook();

            if (!isFire && target != null)
            {
                isFire = true;
                StartCoroutine(OnFire());
            }
        }
        else
        {
            Rotate(lookDirection);
        }

        TargetAim();
    }

    private void OnMove(InputValue value)
    {
        movementDirection = value.Get<Vector2>();
        movementDirection = movementDirection.normalized;
    }

    void OnLook()
    {
        //  ���������� ���Ͱ� ������ ���� ����� ���� �ٶ󺸰�, ������ �������� ������ �ٶ�'
        if(enemyManager != null)
        {
            if (enemyManager.monsterList.Count <= 0)
            {
                float angle = Mathf.Atan2(lookDirection.x, -lookDirection.y) * Mathf.Rad2Deg - 90f;
                attackPivot.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
            else
            {
                float minDistance = 999999f;

                int layerMask = LayerMask.GetMask("Enemy") | LayerMask.GetMask("Obstacle");

                foreach (var monster in enemyManager.monsterList)
                {
                    direction = monster.transform.position - playerTransform.position;
                    float distance = Vector3.Distance(monster.transform.position, playerTransform.position);

                    RaycastHit2D hit = Physics2D.Raycast(playerTransform.position, direction.normalized, distance, layerMask);
                    Debug.DrawRay(playerTransform.position, direction.normalized * distance, Color.red);

                    if (hit.collider != null && hit.collider.gameObject.CompareTag("Enemy"))
                    {
                        if (minDistance > distance)
                        {
                            minDistance = distance;
                            target = hit.collider.gameObject;
                        }
                    }
                }

                if (target != null)
                {
                    Vector3 targetDirection = target.transform.position - attackPivot.position;
                    lookDirection = targetDirection;
                    float angle = Mathf.Atan2(targetDirection.x, -targetDirection.y) * Mathf.Rad2Deg - 90f;
                    attackPivot.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                    if (attackPivot.transform.right.x > 0)
                    {
                        characterRenderer.flipX = true;
                    }
                    else
                    {
                        characterRenderer.flipX = false;
                    }
                }
            }
        }
    }

    private IEnumerator OnFire()
    {
        StartCoroutine(PerformExtraAttacks(extraAttacks));
        yield return new WaitForSeconds(fireDelay);
        isFire = false;
    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isRight = Mathf.Abs(rotZ) < 90f;

        characterRenderer.flipX = isRight;
    }

    private IEnumerator PerformExtraAttacks(int extraAttacks)
    {
        for (int i = 0; i < extraAttacks + 1; i++)
        {
            // Debug.Log("Attack " + i);
            Attack();
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void TargetAim()
    {
        if (target == null)
        {
            aim.SetActive(false);
        }
        else
        {
            aim.SetActive(true);
            aim.transform.position = target.transform.position;
        }
    }

    public override void Death()
    {
        base.Death();
        GameManager.Instance.GameOver();
    }
}