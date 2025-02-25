using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private static SkillManager instance;
    public static SkillManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private RangeWeaponHandler rangeWeaponHandler;
    private ResourceController resourceController;
    private StatHandler statHandler;

    private bool berserkerMode;
    public bool BerserkerMode { get => berserkerMode; }

    void Start()
    {
        rangeWeaponHandler = GetComponentInChildren<RangeWeaponHandler>();
        resourceController = GetComponent<ResourceController>();
        statHandler = GetComponent<StatHandler>();

        if (rangeWeaponHandler == null)
        {
            Debug.Log("무기 못 찾음");
        }
        if (resourceController == null)
        {
            Debug.Log("스탯 컨트롤러 못 찾음");
        }
        if (statHandler == null)
        {
            Debug.Log("스탯 관리 못 찾음");
        }

        //스킬 초기화
        rangeWeaponHandler.Power = 5; //공격력
        rangeWeaponHandler.Delay = 1; //공격 속도
        rangeWeaponHandler.CriticalChance = 0; //치명타율
        rangeWeaponHandler.CriticalDamage = 1.5f; //치명타 데미지
        // 근처 적 튕김 - 미구현
        rangeWeaponHandler.ReflectionCount = 0; //벽 튕김 횟수
        rangeWeaponHandler.IsPenetration = false; //적 관통
        rangeWeaponHandler.ExtraAttack = 0; //추가 공격 횟수
        rangeWeaponHandler.NumberOfForwardProjectiles = 0; //전방 공격 화살 갯수
        rangeWeaponHandler.NumberOfDiagonalProjectiles = 0; //사선 공격 화살 갯수
        rangeWeaponHandler.NumberOfSideProjectiles = 0; //옆방향 공격 화살 갯수
        rangeWeaponHandler.NumberOfBackwardProjectiles = 0; //후방 공격 화살 갯수
        statHandler.Health = 100; //최대 체력
        berserkerMode = false; //체력이 적으면 공격력 증가

    }

    public void ChangePower()
    {

    }

    public void HealPlayer(float HealAmount)
    {
        if (HealAmount < 0)
        {
            Debug.LogWarning("체력 회복용 메소드입니다");
        }
        else
        {
            resourceController.ChangeHealth(HealAmount);
        }
    }

    public void BerserkerModeOn() //구현 해야됨
    {
        berserkerMode = true;
    }

    public void BerserkerModeOff()
    {
        berserkerMode = false;
    }
}
