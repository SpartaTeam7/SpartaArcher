using System.ComponentModel;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer;

    private RangeWeaponHandler rangeWeaponHandler;

    private float currentDuration;
    private Vector2 direction;
    private bool isReady;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer spriteRenderer;

    public bool fxOnDestory = true;

    [SerializeField] private int reflectionCount;

    ProjectileManager projectileManager;
    private RaycastHit2D reflectionInfo;
    private Vector2 preCalculatedNormal;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        reflectionCount = 0;
    }

    void Start()
    {
        reflectionInfo = FindReflectionInfo(transform, 30f, levelCollisionLayer);

        if (reflectionInfo.collider != null)
        {
            preCalculatedNormal = reflectionInfo.normal;
        }
    }

    private void Update()
    {
        if (!isReady)
        {
            return;
        }

        currentDuration += Time.deltaTime;

        if (currentDuration > rangeWeaponHandler.Duration)
        {
            DestroyProjectile(transform.position, false);
        }

        _rigidbody.velocity = transform.right * rangeWeaponHandler.Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isCritical = Random.Range(0f, 100f) < rangeWeaponHandler.CriticalChance;
        bool isBerserker = SkillManager.Instance.BerserkerMode;
        float damage = isCritical ? -rangeWeaponHandler.Power * rangeWeaponHandler.CriticalDamage : -rangeWeaponHandler.Power;
        damage = isBerserker ? damage * SkillManager.Instance.PlayerHealthPersent : damage;

        if (levelCollisionLayer.value == (levelCollisionLayer.value | (1 << collision.gameObject.layer)))
        {
            if (reflectionCount >= rangeWeaponHandler.ReflectionCount)
            {
                DestroyProjectile(collision.ClosestPoint(transform.position) - direction * .2f, fxOnDestory);
            }
            else
            {
                if (_rigidbody != null)
                {
                    Vector2 newVelocity = Vector2.Reflect(_rigidbody.velocity, preCalculatedNormal);
                    _rigidbody.velocity = newVelocity;
                    direction = newVelocity.normalized;
                    transform.right = direction;
                }

                reflectionCount++;
                reflectionInfo = FindReflectionInfo(transform, 30f, levelCollisionLayer);
                if (reflectionInfo.collider != null)
                {
                    preCalculatedNormal = reflectionInfo.normal;
                }
            }
        }
        else if (rangeWeaponHandler.target.value == (rangeWeaponHandler.target.value | (1 << collision.gameObject.layer)))
        {
            ResourceController resourceController = collision.GetComponent<ResourceController>();
            if (resourceController != null)
            {
                resourceController.ChangeHealth(damage);
                if (rangeWeaponHandler.IsOnKnockback)
                {
                    BaseController controller = collision.GetComponent<BaseController>();
                    if (controller != null)
                    {
                        controller.ApplyKnockback(transform, rangeWeaponHandler.KnockbackPower, rangeWeaponHandler.KnockbackTime);
                    }
                }
            }

            if (rangeWeaponHandler.IsPenetration == false)
            {
                DestroyProjectile(collision.ClosestPoint(transform.position), fxOnDestory);
            }
        }
    }

    public void Init(Quaternion rotation, RangeWeaponHandler weaponHandler, ProjectileManager projectileManager)
    {
        // Debug.Log(" z " + rotation.eulerAngles.z);
        this.projectileManager = projectileManager;
        rangeWeaponHandler = weaponHandler;
        transform.rotation = rotation;
        currentDuration = 0;
        transform.localScale = Vector3.one * weaponHandler.BulletSize;
        spriteRenderer.color = weaponHandler.ProjectileColor;

        isReady = true;
    }

    private void DestroyProjectile(Vector3 position, bool createFx)
    {
        if (createFx)
        {
            projectileManager.CreateImpactParticlesAtPosition(position, rangeWeaponHandler);
        }

        Destroy(this.gameObject);
    }

    //충돌 디버그용 함수
    private void DrawCircle(Vector2 position, float radius, Color color)
    {
        int segments = 16; // 원을 그릴 때 사용하는 점 개수
        float angleStep = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angleA = Mathf.Deg2Rad * (i * angleStep);
            float angleB = Mathf.Deg2Rad * ((i + 1) * angleStep);

            Vector2 pointA = position + new Vector2(Mathf.Cos(angleA), Mathf.Sin(angleA)) * radius;
            Vector2 pointB = position + new Vector2(Mathf.Cos(angleB), Mathf.Sin(angleB)) * radius;

            Debug.DrawLine(pointA, pointB, color, 0.1f);
        }
    }

    //raycast 미리 실행
    // private RaycastHit2D FindReflectionInfo(Transform objTransform, float maxDistance, LayerMask collisionLayer, float radius = 0.2f)
    // {
    //     // 🔹 현재 오브젝트의 위치 및 이동 방향 설정
    //     Vector2 origin = objTransform.position;
    //     Vector2 direction = objTransform.right; // `right` 방향으로 CircleCast 실행

    //     // 🔹 CircleCast 실행
    //     RaycastHit2D hit = Physics2D.CircleCast(origin, radius, direction, maxDistance, collisionLayer);

    //     if (hit.collider != null)
    //     {
    //         Debug.Log($"미리 감지된 충돌: {hit.collider.gameObject.name} at {hit.point} | Normal: {hit.normal}");

    //         // 🔹 시각적 디버깅 (검색 경로 및 충돌 지점)
    //         Debug.DrawRay(origin, direction * (hit.collider != null ? hit.distance : maxDistance), Color.green, 0.2f); // 이동 방향
    //         DrawCircle(origin, radius, Color.blue); // 시작 위치에서 원을 그림
    //         DrawCircle(origin + direction.normalized * maxDistance, radius, Color.cyan); // 검색 끝 위치에서 원을 그림
    //         Debug.DrawRay(hit.point, hit.normal * 0.3f, Color.red, 0.2f); // 충돌 지점에서 법선 벡터 표시
    //     }
    //     else
    //     {
    //         Debug.Log("🔍 미리 감지된 충돌 없음");
    //     }

    //     return hit;
    // }

    private RaycastHit2D FindReflectionInfo(Transform objTransform, float maxDistance, LayerMask collisionLayer, float thickness = 0.2f)
    {
        // 🔹 오브젝트의 `right` 방향을 가져옴
        Vector2 origin = objTransform.position;
        Vector2 direction = objTransform.right; // right 방향으로 Raycast 실행

        // 🔹 Ray를 두 개 쏠 위치 계산 (좌우로 thickness/2 만큼 이동)
        Vector2 offset = Vector2.Perpendicular(direction).normalized * (thickness / 2);
        Vector2 leftOrigin = origin - offset;
        Vector2 rightOrigin = origin + offset;

        // 🔹 두 개의 Raycast 실행
        RaycastHit2D hitLeft = Physics2D.Raycast(leftOrigin, direction, maxDistance, collisionLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(rightOrigin, direction, maxDistance, collisionLayer);

        // 🔹 더 가까운 충돌을 선택
        RaycastHit2D bestHit = hitLeft.collider != null && (hitRight.collider == null || hitLeft.distance < hitRight.distance)
            ? hitLeft
            : hitRight;

        if (bestHit.collider != null)
        {
            Debug.DrawRay(leftOrigin, direction * (hitLeft.collider != null ? hitLeft.distance : maxDistance), Color.yellow, 0.2f); // 왼쪽 Ray 시각화
            Debug.DrawRay(rightOrigin, direction * (hitRight.collider != null ? hitRight.distance : maxDistance), Color.yellow, 0.2f); // 오른쪽 Ray 시각화
            Debug.DrawRay(bestHit.point, bestHit.normal * 0.3f, Color.red, 0.2f); // 충돌 지점과 법선 벡터 표시
        }

        return bestHit;
    }
}

