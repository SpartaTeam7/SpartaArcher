using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected Rigidbody2D rb;
    
    //  �÷��̾ �ٶ󺸰� �ִ� ����
    protected Vector2 lookDirection = Vector2.down;
    public Vector2 LookDirection { get => lookDirection; }

    //  �÷��̾��� ���� ����
    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection { get => movementDirection; }

    protected StatHandler statHandler;

    [SerializeField] protected WeaponHandler weaponHandler;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        statHandler = GetComponent<StatHandler>();
    }

    protected virtual void FixedUpdate()
    {
        Movement(movementDirection);
    }

    //  �Է¹��� ���⿡ ���� �÷��̾ �̵���Ű�� �Լ�
    private void Movement(Vector2 direction)
    {
        direction *= statHandler.Speed;

        rb.velocity = direction;

        if(direction.magnitude > 0)
        {
            lookDirection = direction / statHandler.Speed;
        }
    }

    protected virtual void Attack()
    {
        if (lookDirection != Vector2.zero)
        {
            weaponHandler.Attack();
        }
    }
}
