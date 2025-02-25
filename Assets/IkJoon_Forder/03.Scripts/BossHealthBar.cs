using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private Image fillHealthBar;
    [SerializeField] private Text healthText;
    [SerializeField] private bool isShowHpNum = true;
    [SerializeField] private bool isHealthAnim = true;

    private Boss _boss;
    private float currentFill;

    public float fullHealth = 200f;

    // Start is called before the first frame update
    void Start()
    {
        _boss = FindAnyObjectByType<Boss>();

        currentFill = 1f;
        UpdateHealthBar();

    }

    // Update is called once per frame
    void Update()
    {
        if(_boss == null) return;
        UpdateHealthBar();
    }
    private void UpdateHealthBar()
    {
        float healthPer = _boss.bossHp / fullHealth;
        if(isHealthAnim)
        {
            currentFill = Mathf.Lerp(currentFill, healthPer, Time.deltaTime * 5);
        }else
        {
            currentFill = healthPer;
        }

        fillHealthBar.fillAmount = currentFill;
        healthText.text = $"{(int)_boss.bossHp} / {(int)fullHealth}";
        healthText.enabled = isShowHpNum;
    }
}
