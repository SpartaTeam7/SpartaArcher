using System;
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

    private bool boomOnDeath;
    public bool BoomOnDeath { get => boomOnDeath; }

    private bool healOnDeath;
    public bool HealOnDeath { get => healOnDeath; }

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
        rangeWeaponHandler.CriticalChance = 0; //치명타율(최대 100)
        rangeWeaponHandler.CriticalDamage = 1.5f; //치명타 데미지
        // 근처 적 튕김 - 미구현
        rangeWeaponHandler.ReflectionCount = 0; //벽 튕김 횟수
        rangeWeaponHandler.IsPenetration = false; //적 관통
        rangeWeaponHandler.ExtraAttack = 0; //추가 공격 횟수
        rangeWeaponHandler.NumberOfForwardProjectiles = 1; //전방 공격 화살 갯수
        rangeWeaponHandler.NumberOfDiagonalProjectiles = 0; //사선 공격 화살 갯수
        rangeWeaponHandler.NumberOfSideProjectiles = 0; //옆방향 공격 화살 갯수
        rangeWeaponHandler.NumberOfBackwardProjectiles = 0; //후방 공격 화살 갯수
        statHandler.Health = 100; //최대 체력
        berserkerMode = false; //체력이 적으면 공격력 증가
        boomOnDeath = false; //적이 죽으면 터짐
        resourceController.HealthBoost = 1; //회복 강화
        healOnDeath = false; //적이 죽으면 회복
        resourceController.InvincibleTime = 0; //무적 시간 (10초마다 n초)
        // 쉴드 가드 - 미구현
        // 헤드샷 - 미구현
        resourceController.EvasionChance = 0; //회피
        resourceController.IsFlying = false; //지형 무시
        resourceController.ExtraLife = 0; //추가 생명력
    }

    public void ChangePower(float power)
    {
        float currnetPower = rangeWeaponHandler.Power;
        if (power > 0)
        {
            rangeWeaponHandler.Power = currnetPower + power;
        }
        else
        {
            rangeWeaponHandler.Power = currnetPower + power < 0 ? 0 : currnetPower + power;
        }
    }

    public void ChangeDelay(float delay)
    {
        float currnetdelay = rangeWeaponHandler.Delay;
        if (delay > 0)
        {
            rangeWeaponHandler.Delay = currnetdelay + delay;
        }
        else
        {
            rangeWeaponHandler.Delay = currnetdelay + delay < 0.2f ? 0.2f : currnetdelay + delay;
        }
    }

    public void ChangeCriticalChance(float CriticalChance)
    {
        float currnetCriticalChance = rangeWeaponHandler.CriticalChance;

        if (CriticalChance > 0)
        {
            rangeWeaponHandler.CriticalChance = currnetCriticalChance + CriticalChance;
        }
        else
        {
            rangeWeaponHandler.CriticalChance = currnetCriticalChance + CriticalChance < 0 ? 0 : currnetCriticalChance + CriticalChance;
        }
    }

    public void ChangeCriticalDamage(float CriticalDamage)
    {
        float currnetCriticalDamage = rangeWeaponHandler.CriticalDamage;

        if (CriticalDamage > 0)
        {
            rangeWeaponHandler.CriticalDamage = currnetCriticalDamage + CriticalDamage;
        }
        else
        {
            rangeWeaponHandler.CriticalDamage = currnetCriticalDamage + CriticalDamage < 1.5f ? 1.5f : currnetCriticalDamage + CriticalDamage;
        }
    }

    public void ChangeReflectionCount(int ReflectionCount)
    {
        int currnetReflectionCount = rangeWeaponHandler.ReflectionCount;

        if (ReflectionCount > 0)
        {
            rangeWeaponHandler.ReflectionCount = currnetReflectionCount + ReflectionCount;
        }
        else
        {
            rangeWeaponHandler.ReflectionCount = currnetReflectionCount + ReflectionCount < 0 ? 0 : currnetReflectionCount + ReflectionCount;
        }
    }

    public void PenetrationOn()
    {
        rangeWeaponHandler.IsPenetration = true;
    }

    public void PenetrationOff()
    {
        rangeWeaponHandler.IsPenetration = false;
    }

    public void ChangeExtraAttackCount(int ExtraAttackCount)
    {
        int currnetExtraAttackCount = rangeWeaponHandler.ExtraAttack;

        if (ExtraAttackCount > 0)
        {
            rangeWeaponHandler.ExtraAttack = currnetExtraAttackCount + ExtraAttackCount;
        }
        else
        {
            rangeWeaponHandler.ExtraAttack = currnetExtraAttackCount + ExtraAttackCount < 0 ? 0 : currnetExtraAttackCount + ExtraAttackCount;
        }
    }

    public void ChangeNumberOfForwardProjectiles(int NumberOfForwardProjectiles)
    {
        int currnetNumberOfForwardProjectiles = rangeWeaponHandler.NumberOfBackwardProjectiles;

        if (NumberOfForwardProjectiles > 0)
        {
            rangeWeaponHandler.NumberOfBackwardProjectiles = currnetNumberOfForwardProjectiles + NumberOfForwardProjectiles;
        }
        else
        {
            rangeWeaponHandler.NumberOfBackwardProjectiles = currnetNumberOfForwardProjectiles + NumberOfForwardProjectiles < 1 ? 1 : currnetNumberOfForwardProjectiles + NumberOfForwardProjectiles;
        }
    }

    public void ChangeNumberOfDiagonalProjectiles(int NumberOfDiagonalProjectiles)
    {
        int currnetNumberOfDiagonalProjectiles = rangeWeaponHandler.NumberOfDiagonalProjectiles;

        if (NumberOfDiagonalProjectiles > 0)
        {
            rangeWeaponHandler.NumberOfDiagonalProjectiles = currnetNumberOfDiagonalProjectiles + NumberOfDiagonalProjectiles;
        }
        else
        {
            rangeWeaponHandler.NumberOfDiagonalProjectiles = currnetNumberOfDiagonalProjectiles + NumberOfDiagonalProjectiles < 0 ? 0 : currnetNumberOfDiagonalProjectiles + NumberOfDiagonalProjectiles;
        }
    }

    public void ChangeNumberOfSideProjectiles(int NumberOfSideProjectiles)
    {
        int currnetNumberOfSideProjectiles = rangeWeaponHandler.NumberOfSideProjectiles;

        if (NumberOfSideProjectiles > 0)
        {
            rangeWeaponHandler.NumberOfSideProjectiles = currnetNumberOfSideProjectiles + NumberOfSideProjectiles;
        }
        else
        {
            rangeWeaponHandler.NumberOfSideProjectiles = currnetNumberOfSideProjectiles + NumberOfSideProjectiles < 0 ? 0 : currnetNumberOfSideProjectiles + NumberOfSideProjectiles;
        }
    }

    public void ChangeNumberOfBackwardProjectiles(int NumberOfBackwardProjectiles)
    {
        int currnetNumberOfBackwardProjectiles = rangeWeaponHandler.NumberOfBackwardProjectiles;

        if (NumberOfBackwardProjectiles > 0)
        {
            rangeWeaponHandler.NumberOfBackwardProjectiles = currnetNumberOfBackwardProjectiles + NumberOfBackwardProjectiles;
        }
        else
        {
            rangeWeaponHandler.NumberOfBackwardProjectiles = currnetNumberOfBackwardProjectiles + NumberOfBackwardProjectiles < 0 ? 0 : currnetNumberOfBackwardProjectiles + NumberOfBackwardProjectiles;
        }
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

    public void ChangeMaxHealth(int MaxHealthChange)
    {
        statHandler.Health = Mathf.Max(1, statHandler.Health + MaxHealthChange);
    }

    public void BerserkerModeOn() //구현 해야됨
    {
        berserkerMode = true;
    }

    public void BerserkerModeOff()
    {
        berserkerMode = false;
    }

    public void BoomOnDeathOn() //구현 해야됨 - enemy death 트리거에서 죽을때 주번에 데미지 주는걸로
    {
        boomOnDeath = true;
    }

    public void BoomOnDeathOff()
    {
        boomOnDeath = false;
    }

    public void ChangeHealthBoost(float ChangeBoost)
    {
        resourceController.HealthBoost = Mathf.Max(1, resourceController.HealthBoost + ChangeBoost);
    }

    public void HealOnDeathOn() //구현 해야됨 - enemy death 트리거에서 죽을때 회복하는 걸로
    {
        healOnDeath = true;
    }

    public void HealOnDeathOff()
    {
        healOnDeath = false;
    }

    public void ChangeInvincibleTime(float changeInvincibleTime)
    {
        resourceController.InvincibleTime = Mathf.Max(1, resourceController.InvincibleTime + changeInvincibleTime);
    }

    public void ChangeEvasionChance(float changeEvasionChance)
    {
        resourceController.EvasionChance = Mathf.Max(0, resourceController.EvasionChance + changeEvasionChance);
    }

    public void FlyingOn()
    {
        resourceController.IsFlying = true;
    }

    public void FlyingOff()
    {
        resourceController.IsFlying = false;
    }

    public void ChangeExtraLife(int changeExtraLife)
    {
        resourceController.ExtraLife = Mathf.Max(0, resourceController.ExtraLife + changeExtraLife);
    }
}
