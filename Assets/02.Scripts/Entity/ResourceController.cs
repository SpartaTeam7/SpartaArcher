using UnityEngine;
using System.Collections;

public class ResourceController : MonoBehaviour //다 빼버림... 이럴거면 추상 클래스로 바꿔야...
{

    private AnimationHandler animationHandler;

    [SerializeField] protected float healthChangeDelay = 0f;

    protected BaseController baseController;
    protected StatHandler statHandler;

    protected float timeSinceLastChange = float.MaxValue;

    [SerializeField] private float currentHealth;
    public float CurrentHealth
    {
        get => currentHealth;
        protected set => currentHealth = value;
    }
    public float MaxHealth => statHandler.Health;

    protected virtual void Awake()
    {
        statHandler = GetComponent<StatHandler>();
        animationHandler = GetComponent<AnimationHandler>();
        baseController = GetComponent<BaseController>();
    }

    protected virtual void Start()
    {
        currentHealth = statHandler.Health;
    }

    private void Update()
    {
        if (timeSinceLastChange < healthChangeDelay)
        {
            timeSinceLastChange += Time.deltaTime;
            if (timeSinceLastChange >= healthChangeDelay)
            {
                animationHandler.InvincibilityEnd();
            }
        }
    }

    public bool ChangeHealth(float change)
    {
        return CalculateCalChangeHealth(change);
    }

    protected virtual bool CalculateCalChangeHealth(float change)
    {
        return true;
    }

    protected virtual void Death()
    {
        baseController.Death();
    }
}