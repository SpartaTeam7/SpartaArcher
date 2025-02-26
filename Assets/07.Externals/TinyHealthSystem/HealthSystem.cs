using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthSystem : MonoBehaviour
{
    public static HealthSystem Instance;
    public Image currentHealthGlobe;
    public Text healthText;

    private StatHandler statHandler;

    private int MaxHealth;

    // Regeneration Variables
    public bool Regenerate = true;
    public float regen = 0.1f;
    private float timeleft = 0.0f;
    public float regenUpdateInterval = 1f;

    void Awake()
    {
        Instance = this;
    }

   void Start()
{
    statHandler = GetComponentInParent<StatHandler>();
    if (statHandler == null)
    {
        Debug.LogError("❌ StatHandler is missing on " + gameObject.name);
        return;
    }

    // StatHandler의 Health 값이 제대로 설정된 후 MaxHealth를 설정
    MaxHealth = statHandler.Health;
    Debug.Log($"Max Health Set: {MaxHealth}");

    timeleft = regenUpdateInterval;
    UpdateGraphics();
}


    void Update()
    {
        if (Regenerate)
        {
            Regen();
        }
    }

    private void Regen()
    {
        timeleft -= Time.deltaTime;
        if (timeleft <= 0.0f)
        {
            statHandler.Health += (int)regen;  // 체력 회복
            UpdateGraphics();
            timeleft = regenUpdateInterval;
        }
    }

    private void UpdateHealthGlobe()
{
    if (statHandler == null || currentHealthGlobe == null) return;

    float ratio = MaxHealth > 0 ? (float)statHandler.Health / MaxHealth : 0; //  0 나누기 방지
    float height = currentHealthGlobe.rectTransform.rect.height;
    float newY = Mathf.Clamp(height * ratio - height, -height, 0); //  값 범위 제한

    currentHealthGlobe.rectTransform.localPosition = new Vector3(0, newY, 0);
}

    private void UpdateHealthText()
    {
        if (statHandler == null || healthText == null) return;
        healthText.text = $"{statHandler.Health} / {MaxHealth}"; // 최대 체력 반영
    }

    public void TakeDamage(float damage)
    {
        if (statHandler == null) return;

        statHandler.TakeDamage((int)damage);
        UpdateGraphics();
        StartCoroutine(PlayerHurts());
    }

    public void HealDamage(float heal)
    {
        if (statHandler == null) return;

        statHandler.Health += (int)heal;
        UpdateGraphics();
    }

    private void UpdateGraphics()
    {
        UpdateHealthGlobe();
        UpdateHealthText();
    }

    IEnumerator PlayerHurts()
    {
        PopupText.Instance.Popup("Ouch!", 1f, 1f);
        if (statHandler.Health <= 0)
        {
            yield return StartCoroutine(PlayerDied());
        }
        else
        {
            yield return null;
        }
    }

    IEnumerator PlayerDied()
    {
        PopupText.Instance.Popup("You have died!", 1f, 1f);
        yield return null;
    }
}
