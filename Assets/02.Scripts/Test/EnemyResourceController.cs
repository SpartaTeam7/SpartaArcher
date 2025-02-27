using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyResourceController : ResourceController
{
    public HealthText healthText;
    private bool isHeadShot;
    public bool firstHitted;
    private bool isExploded;

    protected override void Start()
    {
        base.Start();
        isHeadShot = Random.Range(0, 100) < 15 ? true : false;
        firstHitted = false;
        isExploded = false;
    }

    protected override void Awake()
    {
        base.Awake();
        healthText = GetComponent<HealthText>();
    }

    protected override bool CalculateCalChangeHealth(float change)
    {
        if (change == 0 || timeSinceLastChange < healthChangeDelay)
        {
            return false;
        }

        if (SkillManager.Instance.HeadShotAvailable)
        {
            if (firstHitted == false)
            {
                if (isHeadShot == true)
                {
                    Death();
                    return true;
                }
            }
        }

        timeSinceLastChange = 0f;

        CurrentHealth += change;
        CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth;
        CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;

        if (CurrentHealth <= 0f)
        {
            Death();
        }

        return true;
    }

    protected override void Death()
    {
        if (SkillManager.Instance.BoomOnDeath && isExploded == false)
        {
            Explode();
        }
        base.Death();
    }

    private float explosionRadius = 3f;

    private void Explode()
    {
        if (isExploded == false)
        {
            isExploded = true;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (Collider2D collider in colliders)
            {
                ResourceController resourceController = collider.GetComponent<EnemyResourceController>();
                if (resourceController != null && collider.gameObject != gameObject) // 자신 제외
                {
                    resourceController.ChangeHealth(-SkillManager.Instance.BoomOnDeathDamage); // 적들에게 데미지
                }
            }
        }
    }
}
