using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class enemyMeLeeFSM : EnemyBase //FSM - ���� ���� ���
{
    public GameObject Player;
    public enum state
    {
        Idle,        // ���
        Move,        // �̵�
        Attack,      // ���� ����
        RangedAttack // ���Ÿ� ���� �߰�
    };

    public state currentstate = state.Idle; //�⺻������ ��� ����
    WaitForSeconds Delay500 = new WaitForSeconds(0.5f); // 0.5�� ���
    WaitForSeconds Delay250 = new WaitForSeconds(0.25f);// 0.25�� ���

    public GameObject projectilePrefab; // ���Ÿ� ���ݿ� ����� �߻�ü ������
    public float projectileSpeed = 10f; // �߻�ü �ӵ�
    public float rangedAttackCooldown = 2f; // ���Ÿ� ���� ��Ÿ��
    private float rangedAttackCooldownTimer = 0f; // ���Ÿ� ���� ��Ÿ�� Ÿ�̸�

    private Animator Anim;

    protected virtual void Start()
    {
        base.Start(); // �θ� Ŭ������ start() ȣ��
        Player = GameObject.Find("Player");
        parentRoom = transform.parent.transform.parent.gameObject; // �θ� �� ��ü ����
        StartCoroutine(FSM()); // FSM �ڷ�ƾ ����
    }

    protected virtual void InitMonster() { } // ���� �ʱ�ȭ �޼���

    // FSM �ڷ�ƾ: ���� ���¸� �����ϴ� �ڷ�ƾ
    protected virtual IEnumerator FSM()
    {
        yield return null;
        while (true) // ���¿� ���� ��� �ݺ�
        {
            yield return StartCoroutine(currentstate.ToString()); // ���� ���¿� �´� �ڷ�ƾ ����
        }
    }

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
                currentstate = state.Idle; // ������ �� ������ ��� ���·� ��ȯ
            }
        }
        else
        {
            currentstate = state.Move; // ������ �� ���ٸ� �̵� ����
        }
    }

    protected virtual IEnumerator Attack()  // ���� ���� ����
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

    // ���Ÿ� ���� ����
    protected virtual IEnumerator RangedAttack()
    {
        if (rangedAttackCooldownTimer <= 0) // ��Ÿ���� ������ ���� ����
        {
            nvAgent.isStopped = true; // �̵� ����
            Anim.SetTrigger("RangedAttack"); // ���Ÿ� ���� �ִϸ��̼� Ʈ����

            // �߻�ü ����
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            Vector3 direction = (Player.transform.position - transform.position).normalized;
            rb.velocity = direction * projectileSpeed; // �߻�ü �ӵ� ����

            rangedAttackCooldownTimer = rangedAttackCooldown; // ��Ÿ�� ����

            yield return Delay500; // ���� �� 0.5�� ���
            nvAgent.isStopped = false; // �̵� �簳
        }
        else
        {
            rangedAttackCooldownTimer -= Time.deltaTime; // ��Ÿ�� ����
        }
    }

    protected virtual IEnumerator Move() // �̵� ����
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

    // ���Ÿ� ���� ��Ÿ�� ����
    private void Update()
    {
        if (rangedAttackCooldownTimer > 0)
        {
            rangedAttackCooldownTimer -= Time.deltaTime; // ��Ÿ�� ����
        }
    }

    protected virtual void AtkEffect() // ���� ���� ȿ�� ó��
    {
        // ���Ÿ� ���� ȿ���� RangedAttack���� ó��
    }
}
