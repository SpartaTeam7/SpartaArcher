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

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        reflectionCount = 0;
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
        bool isCritical = Random.Range(0f, 100f) < rangeWeaponHandler.CriticalChance; // 크리티컬 계산
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
                RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.2f, direction, 0.5f, levelCollisionLayer);

                if (hit.collider != null)
                {
                    Vector2 normal = hit.normal;
                    Rigidbody2D rb = GetComponent<Rigidbody2D>();

                    if (rb != null)
                    {
                        Vector2 newVelocity = Vector2.Reflect(rb.velocity, normal);
                        rb.velocity = newVelocity;
                        direction = newVelocity.normalized;
                        transform.right = direction;
                    }
                }

                reflectionCount++;
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


    public void Init(Quaternion rotation, RangeWeaponHandler weaponHandler)
    {
        Debug.Log(" z " + rotation.eulerAngles.z);
        rangeWeaponHandler = weaponHandler;
        transform.rotation = rotation;
        currentDuration = 0;
        transform.localScale = Vector3.one * weaponHandler.BulletSize;
        spriteRenderer.color = weaponHandler.ProjectileColor;

        isReady = true;
    }

    private void DestroyProjectile(Vector3 position, bool createFx)
    {
        Destroy(this.gameObject);
    }
}

