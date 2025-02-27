using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerResourceController : ResourceController
{
    [SerializeField] private bool isFlying;
    public bool IsFlying
    {
        get => isFlying;
        set
        {
            if (isFlying == value) return; // 값이 변하지 않으면 실행할 필요 없음

            isFlying = value;
            if (isFlying)
                StartFlying();
            else
                StopFlying();
        }
    }

    [SerializeField] private float healthBoost;
    public float HealthBoost { get => healthBoost; set => healthBoost = value; }

    [SerializeField] private float invincibleTime;
    public float InvincibleTime { get => invincibleTime; set => invincibleTime = value; }
    private bool isInvincible = false;

    [SerializeField] private float evasionChance;
    public float EvasionChance { get => evasionChance; set => evasionChance = value; }

    [SerializeField] private float extraLife;
    public float ExtraLife { get => extraLife; set => extraLife = value; }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(InvincibilityRoutine());
    }

    protected override bool CalculateCalChangeHealth(float change)
    {
        if (change == 0 || timeSinceLastChange < healthChangeDelay)
        {
            return false;
        }

        timeSinceLastChange = 0f;

        if (change > 0)
        {
            CurrentHealth += healthBoost * change;
        }
        else
        {
            if (!isInvincible)
            {
                CurrentHealth += change;
                // healthText.UpdateHealthText();
            }
        }

        CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth;
        CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;

        if (CurrentHealth <= 0f)
        {
            Death();
        }

        return true;
    }

    private IEnumerator InvincibilityRoutine()
    {
        while (true) // 무한 반복 (게임이 꺼질 때까지)
        {
            yield return new WaitForSeconds(10f); // 10초 대기
            isInvincible = true;
            // Debug.Log("🔹 무적 상태 ON");

            yield return new WaitForSeconds(invincibleTime); // 무적 유지 시간 대기
            isInvincible = false;
            // Debug.Log("⚡ 무적 상태 OFF");
        }
    }

    public void StartFlying()
    {
        gameObject.layer = LayerMask.NameToLayer("FlyingPlayer");
    }

    public void StopFlying()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    protected override void Death()
    {
        if (extraLife > 0)
        {
            extraLife--;
            CurrentHealth = MaxHealth;
        }
        else { baseController.Death(); }
    }
}
