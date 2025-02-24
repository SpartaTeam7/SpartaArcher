using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected Rigidbody2D rb;
    
    //  플레이어가 바라보고 있는 방향
    protected Vector2 lookDirection = Vector2.down;
    public Vector2 LookDirection { get => lookDirection; }

    //  플레이어의 진행 방향
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

    //  입력받은 방향에 따라 플레이어를 이동시키는 함수
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
