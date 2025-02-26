using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected Rigidbody2D _rigidbody;

    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private Transform weaponPivot;

    //  �÷��̾��� ���� ����
    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection { get => movementDirection; }

    //  �÷��̾ �ٶ󺸰� �ִ� ����
    protected Vector2 lookDirection = Vector2.down;
    public Vector2 LookDirection { get => lookDirection; }

    private Vector2 knockback = Vector2.zero;
    private float knockbackDuration = 0.0f;

    protected StatHandler statHandler;

    [SerializeField] public WeaponHandler WeaponPrefab;

    protected bool isAttacking;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        statHandler = GetComponent<StatHandler>();
    }

    protected virtual void FixedUpdate()
    {
        Movment(movementDirection);
        if (knockbackDuration > 0.0f)
        {
            knockbackDuration -= Time.fixedDeltaTime;
        }
    }

    private void Movment(Vector2 direction)
    {
        direction = direction * statHandler.Speed;
        if (knockbackDuration > 0.0f)
        {
            direction *= 0.2f;
            direction += knockback;
        }

        _rigidbody.velocity = direction;

        if (direction.magnitude > 0)
        {
            lookDirection = direction / statHandler.Speed;
        }
    }

    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        knockback = -(other.position - transform.position).normalized * power;
    }

    protected virtual void Attack()
    {
        if (lookDirection != Vector2.zero)
            WeaponPrefab.Attack();
    }

    public virtual void Death()
    {
        _rigidbody.velocity = Vector3.zero;

        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }

        Destroy(gameObject, 2f);
    }
}
