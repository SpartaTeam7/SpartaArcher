using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private static ProjectileManager instance;
    public static ProjectileManager Instance
    {
        get => instance;
    }

    [SerializeField] private GameObject[] projectileList;

    private void Awake()
    {
        instance = this;
    }

    public void ShootBullet(RangeWeaponHandler rangeWeaponHandler, Vector2 spawnPosition, Quaternion rotation)
    {
        GameObject origin = projectileList[rangeWeaponHandler.BulletIndex];
        GameObject obj = Instantiate(origin, spawnPosition, Quaternion.identity);

        ProjectileController projectileController = obj.GetComponent<ProjectileController>();
        projectileController.Init(rangeWeaponHandler,rotation);
    }
}
