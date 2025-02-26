using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{
    [SerializeField] private SpriteRenderer chracterRenderer;
    [SerializeField] private GameObject[] monsterList;
    [SerializeField] private GameObject target;
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
        monsterList = GameObject.FindGameObjectsWithTag("Enemy");
        attackPivot = transform.GetChild(1).transform.GetChild(0);
        rangeWeaponHandler = GetComponentInChildren<RangeWeaponHandler>();
    }
    private void Update()
    {
        fireDelay = rangeWeaponHandler.Delay;
        extraAttacks = rangeWeaponHandler.ExtraAttack;

        if (movementDirection.magnitude < 0.7f)
        {
            OnLook();

            if (!isFire && monsterList.Length > 0)
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

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isRight = Mathf.Abs(rotZ) < 90f;

        chracterRenderer.flipX = isRight;
    }

    private void OnMove(InputValue value)
    {
        movementDirection = value.Get<Vector2>();
        movementDirection = movementDirection.normalized;
    }

    void OnLook()
    {
        if (monsterList.Length <= 0)
        {
            float angle = Mathf.Atan2(lookDirection.x, -lookDirection.y) * Mathf.Rad2Deg - 90f;
            attackPivot.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            float minDistance = 999999f;

            int layerMask = LayerMask.GetMask("Enemy") | LayerMask.GetMask("Level");

            foreach (var monster in monsterList)
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

            Vector3 targetDirection = target.transform.position - attackPivot.position;
            lookDirection = targetDirection;
            float angle = Mathf.Atan2(targetDirection.x, -targetDirection.y) * Mathf.Rad2Deg - 90f;
            attackPivot.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            if (attackPivot.transform.right.x > 0)
            {
                chracterRenderer.flipX = true;
            }
            else
            {
                chracterRenderer.flipX = false;
            }
        }
    }

    private IEnumerator OnFire()
    {
        StartCoroutine(PerformExtraAttacks(extraAttacks));
        yield return new WaitForSeconds(fireDelay);
        isFire = false;
    }

    private IEnumerator PerformExtraAttacks(int extraAttacks)
    {
        for (int i = 0; i < extraAttacks + 1; i++)
        {
            Debug.Log("Attack " + i);
            Attack();
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void TargetAim()
    {
        if (monsterList.Length <= 0)
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
        // gameManager.GameOver();
    }
}