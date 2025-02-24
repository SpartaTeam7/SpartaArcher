using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

public class PlayerController : BaseController
{
    [SerializeField] private GameObject[] monsterList;
    [SerializeField] private GameObject target;

    private void Start()
    {
        monsterList = GameObject.FindGameObjectsWithTag("Enemy");
    }
    private void Update()
    {
        //  �������� ���� ���� �ٶ�
        if (movementDirection.magnitude < 0.7f)
        {
            OnLook();
        }
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
        if (monsterList.Length <= 0)
        {
            float angle = Mathf.Atan2(lookDirection.x, -lookDirection.y) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            float minDistance = 999999f;

            foreach (var monster in monsterList)
            {
                Vector3 direction = monster.transform.position - transform.position;
                float distance = Vector3.Distance(monster.transform.position, transform.position);

                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, distance, LayerMask.GetMask("Enemy"));
                Debug.DrawRay(transform.position, direction.normalized * distance, Color.red);

                if (hit.collider != null)
                {
                    if(minDistance > distance)
                    {
                        minDistance = distance;
                        target = hit.collider.gameObject;
                    }
                }
            }

            Vector3 targetDirection = target.transform.position - transform.position;
            float angle = Mathf.Atan2(targetDirection.x, -targetDirection.y) * Mathf.Rad2Deg -90f;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        }
    }
}
