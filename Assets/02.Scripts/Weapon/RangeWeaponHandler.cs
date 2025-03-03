using UnityEngine;
using Random = UnityEngine.Random;

public class RangeWeaponHandler : WeaponHandler
{

    [Header("Ranged Attack Data")]
    [SerializeField] private Transform projectileSpawnPosition;
    [SerializeField] private Transform weaponPivot;
    [SerializeField] private int bulletIndex;
    public int BulletIndex { get { return bulletIndex; } }

    [SerializeField] private float bulletSize = 1f;
    public float BulletSize { get { return bulletSize; } }

    [SerializeField] private float duration;
    public float Duration { get { return duration; } }

    [SerializeField] private bool isPenetration;
    public bool IsPenetration { get => isPenetration; set => isPenetration = value; }

    [SerializeField] private int reflectionCount;
    public int ReflectionCount { get => reflectionCount; set => reflectionCount = value; }

    [SerializeField] private int numberOfForwardProjectiles;
    public int NumberOfForwardProjectiles { get => numberOfForwardProjectiles; set => numberOfForwardProjectiles = value; }

    [SerializeField] private int numberOfDiagonalProjectiles;
    public int NumberOfDiagonalProjectiles { get => numberOfDiagonalProjectiles; set => numberOfDiagonalProjectiles = value; }

    [SerializeField] private int numberOfSideProjectiles;
    public int NumberOfSideProjectiles { get => numberOfSideProjectiles; set => numberOfSideProjectiles = value; }

    [SerializeField] private int numberOfBackwardProjectiles;
    public int NumberOfBackwardProjectiles { get => numberOfBackwardProjectiles; set => numberOfBackwardProjectiles = value; }

    [SerializeField] private Color projectileColor;
    public Color ProjectileColor { get { return projectileColor; } }

    private ProjectileManager projectileManager;
    private void Start()
    {
        projectileManager = ProjectileManager.Instance;
    }

    public override void Attack()
    {
        base.Attack();
        Debug.Log("전방 " + numberOfForwardProjectiles + " 후방 " + numberOfBackwardProjectiles);
        for (int i = 0; i < numberOfForwardProjectiles; i++)
        {
            Debug.Log("전방");
            float angle = 0;
            CreateProjectile(Controller.LookDirection, angle, i);
        }
        for (int i = 0; i < numberOfDiagonalProjectiles; i++)
        {
            float angle = 30;
            CreateProjectile(Controller.LookDirection, angle, i);
            CreateProjectile(Controller.LookDirection, -angle, i);
        }
        for (int i = 0; i < numberOfSideProjectiles; i++)
        {
            float angle = 90;
            CreateProjectile(Controller.LookDirection, angle, i);
            CreateProjectile(Controller.LookDirection, -angle, i);
        }
        for (int i = 0; i < numberOfBackwardProjectiles; i++)
        {
            Debug.Log("후방");
            float angle = 180;
            CreateProjectile(Controller.LookDirection, angle, i);
        }
    }

    private void CreateProjectile(Vector2 _lookDirection, float angle, int index)
    {
        float offset = (index % 2 == 0 ? -1f : 1f) * 0.2f * ((index + 1) / 2);
        Vector3 indexOffset = weaponPivot.up * offset;
        if (angle == 90 || angle == -90)
        {
            indexOffset = weaponPivot.right * offset;
        }
        Vector3 projectileSpawnPositionIndex = projectileSpawnPosition.position + indexOffset;
        Quaternion rotate = weaponPivot.rotation *= Quaternion.Euler(0, 0, angle);

        projectileManager.ShootBullet(
            this,
            projectileSpawnPositionIndex,
            rotate
        );

        weaponPivot.rotation *= Quaternion.Euler(0, 0, -angle);
    }
}
