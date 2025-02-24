using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class enemyMeLeeFSM : EnemyBase //FSM - 유한 상태 기계
{
    public enum state
    {
        Idle,   // 대기
        Move,   // 이동
        Attack, // 공격
    };
    public state currentstate = state.Idle; //기본적으로 대기 상태
    WaitForSeconds Delay500 = new WaitForSeconds(0.5f); // 0.5초 대기
    WaitForSeconds Delay250 = new WaitForSeconds(0.25f);// 0.25초 대기

    protected void Start()

    {
        base.Start(); // 부모 클래스의 start() 호출
        parentRoom = transform.parent.transform.parent.gameObject; // 부모 방 객체 참조
        StartCoroutine(FSM()); // FSM 코루틴 실행
    }
    protected virtual void InitMonster() { } // 몬스터 초기화 메서드

   
    protected virtual IEnumerator FSM() // FSM 코루틴: 적의 상태를 관리하는 코루틴
    {
        yield return null;

       // while (!parentRoom.GetComponent<RoomCondition>().playerInThisRoom) // 플레이어가 방에 들어올 때까지 기다림
        {
            yield return Delay500; // 0.5초 대기
        }
        InitMonster(); // 몬스터 초기화

        while (true) // 상태에 따라 계속 반복
        {
            yield return StartCoroutine(currentstate.ToString()); // 현재 상태에 맞는 코루틴 실행
        }
    }
    private Animator Anim;
    protected virtual IEnumerator Idle() // 대기 상태
    {
        yield return null;

        // 애니메이션 상태가 "Idle"이 아니면 "Idle" 애니메이션 트리거
        if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            Anim.SetTrigger("Idle");
        }

        if (CanAtkstatefun()) // 공격할 수 있다면
        {
            if (canAtk)
            {
                currentstate = state.Idle; // 공격할 수 있으면 대기 상태 유지
            }
            else
            {
                currentstate = state.Idle;// 공격할 수 없으면 대기 상태로 전환
             //   transform.LookAt(PlayerPrefs.transform.position); // 플레이어를 바라봄
            }
        }
        else
        {
            currentstate = state.Move; // 공격할 수 없다면 이동 상태
        }
    }

    protected virtual void AtkEffect() // 공격 효과 처리
    {

    }

    protected virtual IEnumerator Attack()  // 공격 상태
    {
        yield return null;
        //Atk

        nvAgent.stoppingDistance = 0f; // 정지 거리 0
        nvAgent.isStopped = true; // 이동 중지
        //nvAgent.SetDestination(PlayerPrefs.transform.position); // 플레이어 위치로 이동
        yield return Delay500; // 0.5초 대기

        nvAgent.isStopped = false; // 이동 재개
        nvAgent.speed = 30f;  // 이동 속도 증가
        canAtk = false; // 공격 후 공격 불가 상태로 설정

       // // 애니메이션 상태가 "Stun"이 아니면 공격 애니메이션 실행
       // if (!Animation.GetCurrentAnimatorSateInfo(0).IsName("Stun"))
       // {
       //     Animation.setTrigger("Attack");
       // }
        AtkEffect(); // 공격 효과 실행
        yield return Delay500;  // 0.5초 대기

        nvAgent.speed = moveSpeed; // 원래 속도로 복구
        nvAgent.stoppingDistance = attackRange;  // 공격 범위로 복구
       // currentState = state.Idle; // 상태를 대기 상태로 변경

    }

    protected virtual IEnumerator Move() // 이동 상태
    {
        yield return null;

        // 애니메이션 상태가 "Walk"가 아니면 "Walk" 애니메이션 트리거
       // if (!Animation.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            Anim.SetTrigger("Walk");
        }
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
           // nvAgent.SetDestination(PlayerPrefs.transform.position); // 플레이어를 향해 이동
        }
    }

}
