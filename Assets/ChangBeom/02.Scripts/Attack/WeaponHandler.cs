using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private float delay = 0.5f;
    public float Delay
    {
        get => delay;
        set => delay = value;
    }

    [SerializeField] private float power = 1f;
    public float Power
    {
        get => power;
        set => power = value;
    }

    [SerializeField] private float speed = 1f;
    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    [SerializeField] private float attackRange;
    public float AttackRange
    {
        get => attackRange;
        set => attackRange = value;
    }

    public virtual void Attack()
    {

    }
}
