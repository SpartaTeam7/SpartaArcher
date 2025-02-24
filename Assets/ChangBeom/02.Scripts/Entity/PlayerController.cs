using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class PlayerController : BaseController
{
    [SerializeField] private SpriteRenderer chracterRenderer;
    [SerializeField] private GameObject[] monsterList;
    [SerializeField] private GameObject target;
    [SerializeField] private Transform attackPivot;

    [SerializeField ]private float fireDelay = 0.5f;

    private Vector3 direction;
    public Vector3 Direction
    {
        get => direction;
    }

    private bool isFire = false;

    private void Start()
    {
        monsterList = GameObject.FindGameObjectsWithTag("Enemy");
        attackPivot = transform.GetChild(1).transform.GetChild(0);
    }
    private void Update()
    {
        //  �������� ���� ���� �ٶ�
        if (movementDirection.magnitude < 0.7f)
        {
            OnLook();
            if (!isFire)
            {
                isFire = true;
                StartCoroutine(OnFire());
            }
        }
        else
        {
            Rotate(lookDirection);
        }
    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isRight = Mathf.Abs(rotZ) < 90f;

        chracterRenderer.flipX = isRight;
    }

    //  Input System���� Ű�Է��� �޾� �÷��̾ �����̴� �Լ�
    private void OnMove(InputValue value)
    {
        movementDirection = value.Get<Vector2>();
        movementDirection = movementDirection.normalized;
    }

    //  Player�� ���� ����� ���� �ٶ󺸴� �Լ�
    void OnLook()
    {
        //  ���������� ���Ͱ� ������ ���� ����� ���� �ٶ󺸰�, ������ �������� ������ �ٶ�
        if (monsterList.Length <= 0)
        {
            float angle = Mathf.Atan2(lookDirection.x, -lookDirection.y) * Mathf.Rad2Deg - 90f;
            attackPivot.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            float minDistance = 999999f;

            foreach (var monster in monsterList)
            {
                direction = monster.transform.position - attackPivot.position;
                float distance = Vector3.Distance(monster.transform.position, attackPivot.position);

                RaycastHit2D hit = Physics2D.Raycast(attackPivot.position, direction.normalized, distance, LayerMask.GetMask("Enemy"));
                Debug.DrawRay(attackPivot.position, direction.normalized * distance, Color.red);

                if (hit.collider != null)
                {
                    if (minDistance > distance)
                    {
                        minDistance = distance;
                        target = hit.collider.gameObject;
                    }
                }
            }

            Vector3 targetDirection = target.transform.position - attackPivot.position;
            float angle = Mathf.Atan2(targetDirection.x, -targetDirection.y) * Mathf.Rad2Deg - 90f;
            attackPivot.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            if(attackPivot.transform.right.x > 0)
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
        Attack();
        yield return new WaitForSeconds(fireDelay);
        isFire = false;
    }
}
