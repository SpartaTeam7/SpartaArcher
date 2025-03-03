using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [Header("Attack Info")]
    [SerializeField] private float delay = 0.5f;
    public float Delay { get => delay; set => delay = value; }

    [SerializeField] private float weaponSize = 1f;
    public float WeaponSize { get => weaponSize; set => weaponSize = value; }

    [SerializeField] private float power = 1f;
    public float Power { get => power; set => power = value; }

    [SerializeField] private float speed = 1f;
    public float Speed { get => speed; set => speed = value; }

    [SerializeField] private float attackRange = 10f;
    public float AttackRange { get => attackRange; set => attackRange = value; }

    [SerializeField] private float criticalChance = 30f;
    public float CriticalChance { get => criticalChance; set => criticalChance = value; }

    [SerializeField] private float criticalDamage = 2.0f;
    public float CriticalDamage { get => criticalDamage; set => criticalDamage = value; }

    [SerializeField] private int extraAttack = 1;
    public int ExtraAttack { get => extraAttack; set => extraAttack = value; }

    public LayerMask target;

    [Header("Knock Back Info")]
    [SerializeField] private bool isOnKnockback = false;
    public bool IsOnKnockback { get => isOnKnockback; set => isOnKnockback = value; }

    [SerializeField] private float knockbackPower = 0.1f;
    public float KnockbackPower { get => knockbackPower; set => knockbackPower = value; }

    [SerializeField] private float knockbackTime = 0.5f;
    public float KnockbackTime { get => knockbackTime; set => knockbackTime = value; }

    public BaseController Controller { get; private set; }
    private SpriteRenderer weaponRenderer;

    protected virtual void Awake()
    {
        Controller = GetComponentInParent<BaseController>();
        weaponRenderer = GetComponentInChildren<SpriteRenderer>();

        transform.localScale = Vector3.one * weaponSize;
    }

    public virtual void Attack()
    {

    }
}
