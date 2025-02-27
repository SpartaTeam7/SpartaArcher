using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthSystem : MonoBehaviour
{
    public static HealthSystem Instance;
    public Image currentHealthGlobe;
    public Text healthText;

    private PlayerResourceController playerResourceController;

    [SerializeField] private float MaxHealth;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        playerResourceController = GetComponentInParent<PlayerResourceController>();

        if (playerResourceController == null)
        {
            Debug.LogError("❌ StatHandler is missing on " + gameObject.name);
            return;
        }

        // StatHandler의 Health 값이 제대로 설정된 후 MaxHealth를 설정
        MaxHealth = playerResourceController.MaxHealth;
        Debug.Log($"Max Health Set: {MaxHealth}");

        UpdateGraphics();
    }


    void Update()
    {
        UpdateGraphics();
    }


    private void UpdateHealthGlobe()
    {
        if (playerResourceController == null || currentHealthGlobe == null) return;

        float ratio = MaxHealth > 0 ? playerResourceController.CurrentHealth / MaxHealth : 0; //  0 나누기 방지
        float height = currentHealthGlobe.rectTransform.rect.height;
        float newY = Mathf.Clamp(height * ratio - height, -height, 0); //  값 범위 제한

        currentHealthGlobe.rectTransform.localPosition = new Vector3(0, newY, 0);
    }

    private void UpdateHealthText()
    {
        if (playerResourceController == null || healthText == null) return;
        healthText.text = $"{playerResourceController.CurrentHealth} / {MaxHealth}"; // 최대 체력 반영
    }

    private void UpdateGraphics()
    {
        UpdateHealthGlobe();
        UpdateHealthText();
    }
}
