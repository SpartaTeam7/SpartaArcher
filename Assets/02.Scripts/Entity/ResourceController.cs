using UnityEngine;
using System.Collections;

public class ResourceController : MonoBehaviour
{
    [SerializeField] private float healthChangeDelay = 0f;

    private BaseController baseController;
    private StatHandler statHandler;

    private float timeSinceLastChange = float.MaxValue;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => statHandler.Health;

    public HealthText healthText;

    [SerializeField] private float healthBoost;
    public float HealthBoost { get => healthBoost; set => healthBoost = value; }

    [SerializeField] private float invincibleTime;
    public float InvincibleTime { get => invincibleTime; set => invincibleTime = value; }
    private bool isInvincible = false;

    [SerializeField] private float evasionChance;
    public float EvasionChance { get => evasionChance; set => evasionChance = value; }

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

    [SerializeField] private float extraLife;
    public float ExtraLife { get => extraLife; set => extraLife = value; }

    private void Awake()
    {
        statHandler = GetComponent<StatHandler>();
        // animationHandler = GetComponent<AnimationHandler>();
        baseController = GetComponent<BaseController>();
        healthText = GetComponent<HealthText>();
    }

    private void Start()
    {
        CurrentHealth = statHandler.Health;
        StartCoroutine(InvincibilityRoutine());
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

    private void Update()
    {
        if (timeSinceLastChange < healthChangeDelay)
        {
            timeSinceLastChange += Time.deltaTime;
            if (timeSinceLastChange >= healthChangeDelay)
            {
                // animationHandler.InvincibilityEnd();
            }
        }
    }

    public bool ChangeHealth(float change)
    {
        if (change == 0 || timeSinceLastChange < healthChangeDelay)
        {
            return false;
        }

        timeSinceLastChange = 0f;
        if (change > 0)
        {
            CurrentHealth += healthBoost * change; // 이러면 몬스터도 힐 부스트 받긴 함 - 혹시 모르니 체크
        }
        else
        {
            if (!isInvincible)
            {
                CurrentHealth += change;
                // animationHandler.Damage();
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

    public void StartFlying()
    {
        gameObject.layer = LayerMask.NameToLayer("FlyingPlayer");
    }

    public void StopFlying()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void Death()
    {
        if (extraLife > 0)
        {
            extraLife--;
            CurrentHealth = MaxHealth;
        }
        else
        {
            baseController.Death();
        }
    }
}