using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class RangeWeaponHandler : WeaponHandler
{
    [SerializeField] private Transform projectileSpawnPosition;
    [SerializeField] private Transform weaponPivot;
    [SerializeField] private int bulletIndex;
    public int BulletIndex
    {
        get => bulletIndex;
    }

    [SerializeField] private float bulletSize = 1f;
    public float BulletSize
    {
        get => bulletSize;
    }

    [SerializeField] private float duration;
    public float Duration
    {
        get => duration;
    }

    private ProjectileManager projectileManager;

    private void Start()
    {
        projectileManager = ProjectileManager.Instance;
    }

    public override void Attack()
    {
        CreateProjectile();
    }

    private void CreateProjectile()
    {
        projectileManager.ShootBullet(this, projectileSpawnPosition.position, weaponPivot.rotation);
    }
}
