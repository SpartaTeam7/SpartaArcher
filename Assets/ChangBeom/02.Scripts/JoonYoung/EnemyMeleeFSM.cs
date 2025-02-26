using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class enemyMeLeeFSM : EnemyBase // FSM - ���� ���� ���
{
    public GameObject Player; // �÷��̾� ��ü
    public enum state
    {
        Idle,        // ��� ����
        Move,        // �̵� ����
        Attack,      // ���� ���� ����
    };

    public state currentstate = state.Idle; // �⺻������ ��� ����
    WaitForSeconds Delay500 = new WaitForSeconds(0.5f); // 0.5�� ���

    private Animator Anim; // �ִϸ����� ������Ʈ

    // Start() �޼ҵ�
    protected virtual void Start()
    {
        base.Start(); // �θ� Ŭ������ Start() ȣ��
        Player = GameObject.Find("Player"); // �÷��̾� ��ü ã��
        parentRoom = transform.parent.transform.parent.gameObject; // �θ� �� ��ü ����
        StartCoroutine(FSM()); // FSM �ڷ�ƾ ����
    }

    // ���� �ʱ�ȭ �޼��� (����� ������ ����)
    protected virtual void InitMonster() { }

    // FSM (Finite State Machine) �ڷ�ƾ: ���� ���¸� �����ϴ� �ڷ�ƾ
    protected virtual IEnumerator FSM()
    {
        yield return null;
        while (true) // ���¿� ���� ��� �ݺ�
        {
            yield return StartCoroutine(currentstate.ToString()); // ���� ���¿� �´� �ڷ�ƾ ����
        }
    }

    // ��� ����
    protected virtual IEnumerator Idle()
    {
        yield return null;

        // �ִϸ��̼� ���°� "Idle"�� �ƴϸ� "Idle" �ִϸ��̼� Ʈ����
        if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            Anim.SetTrigger("Idle");
        }

        // ���� ������ �������� üũ
        if (CanAtkstatefun())
        {
            if (canAtk)
            {
                currentstate = state.Idle; // ������ �� ������ ��� ���� ����
            }
            else
            {
                currentstate = state.Idle; // ������ �� ������ ��� ���·� ��ȯ
            }
        }
        else
        {
            currentstate = state.Move; // ������ �� ���ٸ� �̵� ���·� ��ȯ
        }
    }

    // ���� ���� ����
    protected virtual IEnumerator Attack()
    {
        yield return null;

        nvAgent.stoppingDistance = 0f; // ���� �Ÿ� 0
        nvAgent.isStopped = true; // �̵� ����
        yield return Delay500; // 0.5�� ���

        nvAgent.isStopped = false; // �̵� �簳
        nvAgent.speed = 30f;  // �̵� �ӵ� ����
        canAtk = false; // ���� �� ���� �Ұ� ���·� ����

        AtkEffect(); // ���� ���� ȿ�� ����
        yield return Delay500;  // 0.5�� ���

        nvAgent.speed = moveSpeed; // ���� �ӵ��� ����
        nvAgent.stoppingDistance = attackRange;  // ���� ������ ����
    }

    // �̵� ����
    protected virtual IEnumerator Move()
    {
        yield return null;

        // �ִϸ��̼� ���°� "Walk"�� �ƴϸ� "Walk" �ִϸ��̼� Ʈ����
        Anim.SetTrigger("Walk");

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
            nvAgent.SetDestination(Player.transform.position); // �÷��̾ ���� �̵�
        }
    }
}
