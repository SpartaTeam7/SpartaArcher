using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class enemyMeLeeFSM : EnemyBase //FSM - ���� ���� ���
{
    public enum state
    {
        Idle,   // ���
        Move,   // �̵�
        Attack, // ����
    };
    public state currentstate = state.Idle; //�⺻������ ��� ����
    WaitForSeconds Delay500 = new WaitForSeconds(0.5f); // 0.5�� ���
    WaitForSeconds Delay250 = new WaitForSeconds(0.25f);// 0.25�� ���

    protected void Start()

    {
        base.Start(); // �θ� Ŭ������ start() ȣ��
        parentRoom = transform.parent.transform.parent.gameObject; // �θ� �� ��ü ����
        StartCoroutine(FSM()); // FSM �ڷ�ƾ ����
    }
    protected virtual void InitMonster() { } // ���� �ʱ�ȭ �޼���

   
    protected virtual IEnumerator FSM() // FSM �ڷ�ƾ: ���� ���¸� �����ϴ� �ڷ�ƾ
    {
        yield return null;

       // while (!parentRoom.GetComponent<RoomCondition>().playerInThisRoom) // �÷��̾ �濡 ���� ������ ��ٸ�
        {
            yield return Delay500; // 0.5�� ���
        }
        InitMonster(); // ���� �ʱ�ȭ

        while (true) // ���¿� ���� ��� �ݺ�
        {
            yield return StartCoroutine(currentstate.ToString()); // ���� ���¿� �´� �ڷ�ƾ ����
        }
    }
    private Animator Anim;
    protected virtual IEnumerator Idle() // ��� ����
    {
        yield return null;

        // �ִϸ��̼� ���°� "Idle"�� �ƴϸ� "Idle" �ִϸ��̼� Ʈ����
        if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            Anim.SetTrigger("Idle");
        }

        if (CanAtkstatefun()) // ������ �� �ִٸ�
        {
            if (canAtk)
            {
                currentstate = state.Idle; // ������ �� ������ ��� ���� ����
            }
            else
            {
                currentstate = state.Idle;// ������ �� ������ ��� ���·� ��ȯ
             //   transform.LookAt(PlayerPrefs.transform.position); // �÷��̾ �ٶ�
            }
        }
        else
        {
            currentstate = state.Move; // ������ �� ���ٸ� �̵� ����
        }
    }

    protected virtual void AtkEffect() // ���� ȿ�� ó��
    {

    }

    protected virtual IEnumerator Attack()  // ���� ����
    {
        yield return null;
        //Atk

        nvAgent.stoppingDistance = 0f; // ���� �Ÿ� 0
        nvAgent.isStopped = true; // �̵� ����
        //nvAgent.SetDestination(PlayerPrefs.transform.position); // �÷��̾� ��ġ�� �̵�
        yield return Delay500; // 0.5�� ���

        nvAgent.isStopped = false; // �̵� �簳
        nvAgent.speed = 30f;  // �̵� �ӵ� ����
        canAtk = false; // ���� �� ���� �Ұ� ���·� ����

       // // �ִϸ��̼� ���°� "Stun"�� �ƴϸ� ���� �ִϸ��̼� ����
       // if (!Animation.GetCurrentAnimatorSateInfo(0).IsName("Stun"))
       // {
       //     Animation.setTrigger("Attack");
       // }
        AtkEffect(); // ���� ȿ�� ����
        yield return Delay500;  // 0.5�� ���

        nvAgent.speed = moveSpeed; // ���� �ӵ��� ����
        nvAgent.stoppingDistance = attackRange;  // ���� ������ ����
       // currentState = state.Idle; // ���¸� ��� ���·� ����

    }

    protected virtual IEnumerator Move() // �̵� ����
    {
        yield return null;

        // �ִϸ��̼� ���°� "Walk"�� �ƴϸ� "Walk" �ִϸ��̼� Ʈ����
       // if (!Animation.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            Anim.SetTrigger("Walk");
        }
        // ������ �� ������ ���� ���·� ��ȯ
        if (CanAtkstatefun() && canAtk)
        {
            currentstate = state.Attack;
        }
        else if (distance > playerRealizeRange) // �÷��̾� �ν� ������ �����
        {
            nvAgent.SetDestination(transform.parent.parent.position - Vector3.forward * 5f); // ���� �Ÿ� �ڷ� �̵�

        }
        else
        {
           // nvAgent.SetDestination(PlayerPrefs.transform.position); // �÷��̾ ���� �̵�
        }
    }

}
