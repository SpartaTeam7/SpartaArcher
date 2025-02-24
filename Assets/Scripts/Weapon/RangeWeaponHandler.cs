using UnityEngine;
using Random = UnityEngine.Random;

public class RangeWeaponHandler : WeaponHandler
{

    [Header("Ranged Attack Data")]
    [SerializeField] private Transform projectileSpawnPosition;

    [SerializeField] private int bulletIndex;
    public int BulletIndex { get { return bulletIndex; } }

    [SerializeField] private float bulletSize = 1;
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
    protected override void Start()
    {
        base.Start();
        projectileManager = ProjectileManager.Instance;
    }

    public override void Attack()
    {
        base.Attack();

        for (int i = 0; i < numberOfForwardProjectiles; i++)
        {
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
            float angle = 180;
            CreateProjectile(Controller.LookDirection, angle, i);
        }
    }

    private void CreateProjectile(Vector2 _lookDirection, float angle, int index)
    {
        float offset = (index % 2 == 0 ? -1f : 1f) * 0.2f * ((index + 1) / 2);
        Debug.Log(index);
        Debug.Log(offset);
        Vector3 indexOffset = new Vector3(0, offset, 0);
        Vector3 projectileSpawnPositionIndex = projectileSpawnPosition.position + indexOffset;
        projectileManager.ShootBullet(
            this,
            projectileSpawnPositionIndex,
            RotateVector2(_lookDirection, angle));
    }

    private static Vector2 RotateVector2(Vector2 v, float degree)
    {
        return Quaternion.Euler(0, 0, degree) * v;
    }
}
