using UnityEngine;
using TMPro;

public class HealthText : MonoBehaviour
{
    private ResourceController resourceController;
    public TextMeshPro healthText;

    void Start()
    {
        resourceController = GetComponent<ResourceController>();
        UpdateHealthText();
    }

    void Update()
    {
        UpdateHealthText();
    }

    public void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = $"{resourceController.CurrentHealth}";
        }
    }
}