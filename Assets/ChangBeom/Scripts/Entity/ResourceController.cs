using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    private StatHandler statHandler;

    public float CurrenHealth
    {
        get;
        private set;
    }

    public float MaxHealth => statHandler.Health;
    private void Awake()
    {
        statHandler = GetComponent<StatHandler>();
    }

    private void Start()
    {
        CurrenHealth = statHandler.Health;
    }

    //  �Ű������� �Է¹��� �� ��ŭ ü�� ����
    public bool ChangeHealth(float change)
    {
        if(change == 0)
        {
            return false;
        }

        CurrenHealth += change;

        if(CurrenHealth >= MaxHealth)
        {
            CurrenHealth = MaxHealth;
        }

        if(CurrenHealth <= 0)
        {
            CurrenHealth = 0;
        }

        return true;
    }
}
