using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image fillHealthBar;
    [SerializeField] private bool isHealthAnim = true;

    [SerializeField] private float animTime = 5f;

    private float currentFill;

    private EnemyResourceController _enemyResourceController;

    private void Awake()
    {
        // 부모 객체에서 EnemyResourceController를 가져옴
        _enemyResourceController = GetComponentInParent<EnemyResourceController>();
        if (_enemyResourceController == null)
        {
            Debug.LogError("[EnemyHealthBar] EnemyResourceController가 없");
        }
    }

    private void Update()
    {
        if (_enemyResourceController != null)
        {
            // 적의 현재 체력을 바탕으로 체력 바 업데이트
            UpdateHealthBar(_enemyResourceController.CurrentHealth, _enemyResourceController.MaxHealth);
        }
    }

    public void UpdateHealthBar(float currEnemyHp, float maxHealth)
    {
        float healthPer = currEnemyHp / maxHealth;
        if (isHealthAnim)
        {
            currentFill = Mathf.Lerp(currentFill, healthPer, Time.deltaTime * animTime);
        }
        else
        {
            currentFill = healthPer;
        }

        fillHealthBar.fillAmount = currentFill;
    }
}
