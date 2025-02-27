using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 스킬 업그레이드 사용법

==== 투사체 관련 ====

공격력 - skillManager.ChangePower(파워 증감 양)
공격 속도 - skillManager.ChangeDelay(파워 증감 양)
크리티컬 확률 - skillManager.ChangeCriticalChance(치명타율 증감 양 - 100이 100퍼센트 - 수치는 100이상 가능)
크리티컬 데미지 - skillManager.ChangeCriticalDamage(치명타 데미지 증감 (1 더하면 100퍼센트 증가) - 최소 1.5배)
근처 적에게 튕김 - 미구현
벽 반사 - skillManager.ChangeReflectionCount - 버그(벽 반사각 가끔 오류)
관통 - skillManager.PenetrationOn, Off
공격 한번 더 발사 - skillManager.ChangeExtraAttackCount(추가 공격 횟수 - 최소 0)
전방 화살 추가 - skillManager.ChangeNumberOfForwardProjectiles(투사체 갯수 증감 - 최소 1) - 생성 위치 다시 확인 필요
사선 화살 추가 - skillManager.ChangeNumberOfDiagonalProjectiles(투사체 갯수 증감 - 최소 0)
측면 화살 추가 - skillManager.ChangeNumberOfSideProjectiles(투사체 갯수 증감 - 최소 0)
후방 화살 추가 - skillManager.ChangeNumberOfBackwardProjectiles(투사체 갯수 증감 - 최소 0)

==== 의존적인 스킬 ====

회복 - skillManager.HealPlayer(힐 양)
최대 체력 증가 - skillManager.ChangeMaxHealth(변화량 - 최소1 최대1만)
체력이 낮을수록 공격력 증가 - skillManager.BerserkerModeOn, Off
적이 죽을때 터짐 - skillManager.BoomOnDeathOn, Off
회복 강화 - skillManager.ChangeHealthBoost (최소 1)
적이 죽을때 체력 회복 - skillManager.HealOnDeathOn, Off
무적 - skillManager.InvincibleTime (10초후에 n초 무적)
쉴드 가드 - 미구현
헤드샷 - skillManager.HeadShotOn, Off (15퍼 확률로 적이 첫 사격에 즉사)
회피 마스터 - skillManager.ChangeEvasionChance (퍼센트 회피)
장애물 통과 - skillManager.FlyingOn, Off
추가 목숨 - skillManager.ChangeExtraLife
*/

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
    private PlayerResourceController resourceController;
    private StatHandler statHandler;

    public float PlayerHealthPersent { get; private set; }

    private bool berserkerMode;
    public bool BerserkerMode { get => berserkerMode; }

    private bool boomOnDeath;
    public bool BoomOnDeath { get => boomOnDeath; }

    private float boomOnDeathDamage;
    private float boomOnDeathDamagemultiple = 5;
    public float BoomOnDeathDamage { get => boomOnDeathDamage; }

    private bool healOnDeath;
    public bool HealOnDeath { get => healOnDeath; }

    private bool headShotAvailable;
    public bool HeadShotAvailable { get => headShotAvailable; }

    void Start()
    {
        rangeWeaponHandler = GetComponentInChildren<RangeWeaponHandler>();
        resourceController = GetComponent<PlayerResourceController>();
        statHandler = GetComponent<StatHandler>();
        PlayerHealthPersent = 1 + (statHandler.Health - resourceController.CurrentHealth) / statHandler.Health;

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
        rangeWeaponHandler.Delay = 0.3f; //공격 속도
        rangeWeaponHandler.CriticalChance = 0; //치명타율(최대 100)
        rangeWeaponHandler.CriticalDamage = 1.5f; //치명타 데미지
        // 근처 적 튕김 - 미구현
        rangeWeaponHandler.ReflectionCount = 2; //벽 튕김 횟수
        rangeWeaponHandler.IsPenetration = true; //적 관통
        rangeWeaponHandler.ExtraAttack = 0; //추가 공격 횟수
        rangeWeaponHandler.NumberOfForwardProjectiles = 1; //전방 공격 화살 갯수
        rangeWeaponHandler.NumberOfDiagonalProjectiles = 0; //사선 공격 화살 갯수
        rangeWeaponHandler.NumberOfSideProjectiles = 0; //옆방향 공격 화살 갯수
        rangeWeaponHandler.NumberOfBackwardProjectiles = 0; //후방 공격 화살 갯수
        statHandler.Health = 100; //최대 체력
        berserkerMode = false; //체력이 적으면 공격력 증가
        boomOnDeath = false; //적이 죽으면 터짐
        boomOnDeathDamage = rangeWeaponHandler.Power * boomOnDeathDamagemultiple; //터지는 데미지
        resourceController.HealthBoost = 1; //회복 강화
        healOnDeath = false; //적이 죽으면 회복
        resourceController.InvincibleTime = 0; //무적 시간 (10초마다 n초)
        // 쉴드 가드 - 미구현
        headShotAvailable = false; //헤드샷 (15퍼 확률로 적이 첫 사격에 즉사)
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
            boomOnDeathDamage = rangeWeaponHandler.Power * boomOnDeathDamagemultiple;
        }
        else
        {
            rangeWeaponHandler.Power = currnetPower + power < 0 ? 0 : currnetPower + power;
            boomOnDeathDamage = rangeWeaponHandler.Power * boomOnDeathDamagemultiple;
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

    public void BerserkerModeOn()
    {
        berserkerMode = true;
    }

    public void BerserkerModeOff()
    {
        berserkerMode = false;
    }

    public void BoomOnDeathOn()
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

    public void HealOnDeathOn()
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

    public void HeadShotOn()
    {
        headShotAvailable = true;
    }

    public void HeadShotOff()
    {
        headShotAvailable = false;
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