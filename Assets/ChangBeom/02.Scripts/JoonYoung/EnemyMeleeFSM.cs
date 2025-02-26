using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class enemyMeLeeFSM : EnemyBase // FSM - 유한 상태 기계
{
    public GameObject Player; // 플레이어 객체
    public enum state
    {
        Idle,        // 대기 상태
        Move,        // 이동 상태
        Attack,      // 근접 공격 상태
    };

    public state currentstate = state.Idle; // 기본적으로 대기 상태
    WaitForSeconds Delay500 = new WaitForSeconds(0.5f); // 0.5초 대기

    private Animator Anim; // 애니메이터 컴포넌트

    // Start() 메소드
    protected virtual void Start()
    {
        base.Start(); // 부모 클래스의 Start() 호출
        Player = GameObject.Find("Player"); // 플레이어 객체 찾기
        parentRoom = transform.parent.transform.parent.gameObject; // 부모 방 객체 참조
        StartCoroutine(FSM()); // FSM 코루틴 실행
    }

    // 몬스터 초기화 메서드 (현재는 사용되지 않음)
    protected virtual void InitMonster() { }

    // FSM (Finite State Machine) 코루틴: 적의 상태를 관리하는 코루틴
    protected virtual IEnumerator FSM()
    {
        yield return null;
        while (true) // 상태에 따라 계속 반복
        {
            yield return StartCoroutine(currentstate.ToString()); // 현재 상태에 맞는 코루틴 실행
        }
    }

    // 대기 상태
    protected virtual IEnumerator Idle()
    {
        yield return null;

        // 애니메이션 상태가 "Idle"이 아니면 "Idle" 애니메이션 트리거
        if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            Anim.SetTrigger("Idle");
        }

        // 공격 가능한 상태인지 체크
        if (CanAtkstatefun())
        {
            if (canAtk)
            {
                currentstate = state.Idle; // 공격할 수 있으면 대기 상태 유지
            }
            else
            {
                currentstate = state.Idle; // 공격할 수 없으면 대기 상태로 전환
            }
        }
        else
        {
            currentstate = state.Move; // 공격할 수 없다면 이동 상태로 전환
        }
    }

    // 근접 공격 상태
    protected virtual IEnumerator Attack()
    {
        yield return null;

        nvAgent.stoppingDistance = 0f; // 정지 거리 0
        nvAgent.isStopped = true; // 이동 중지
        yield return Delay500; // 0.5초 대기

        nvAgent.isStopped = false; // 이동 재개
        nvAgent.speed = 30f;  // 이동 속도 증가
        canAtk = false; // 공격 후 공격 불가 상태로 설정

        AtkEffect(); // 근접 공격 효과 실행
        yield return Delay500;  // 0.5초 대기

        nvAgent.speed = moveSpeed; // 원래 속도로 복구
        nvAgent.stoppingDistance = attackRange;  // 공격 범위로 복구
    }

    // 이동 상태
    protected virtual IEnumerator Move()
    {
        yield return null;

        // 애니메이션 상태가 "Walk"가 아니면 "Walk" 애니메이션 트리거
        Anim.SetTrigger("Walk");

        // 공격할 수 있으면 공격 상태로 전환
        if (CanAtkstatefun() && canAtk)
        {
            currentstate = state.Attack;
        }
        else if (distance > playerRealizeRange) // 플레이어 인식 범위를 벗어나면
        {
            nvAgent.SetDestination(transform.parent.parent.position - Vector3.forward * 5f); // 일정 거리 뒤로 이동
        }
        else
        {
            nvAgent.SetDestination(Player.transform.position); // 플레이어를 향해 이동
        }
    }
}
