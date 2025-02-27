using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseController
{
    // ���� �����ϴ� �Ŵ���
    private EnemyManager enemyManager;

    private PlayerController playerController;

    // ���� ������ Ÿ�� (�÷��̾� ��)
    private Transform target;

    // ���� Ÿ���� �����ϴ� ���� (�Ÿ�)
    [SerializeField] private float followRange = 15f;

    // ���� ����
    [SerializeField] private float attackRange = 10f;

    [SerializeField] private RangeWeaponHandler rangeWeaponHandler;
    private float lastRangedAttackTime;

    private ProjectileController projectileController;

    public void Start()
    {
        Init();
    }

    // �ʱ�ȭ �޼���: EnemyManager�� Ÿ���� ����
    public void Init()
    {

        enemyManager = EnemyManager.Instance;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        target = GameObject.Find("Player").transform;
    }

    // Ÿ�ٰ��� �Ÿ��� ����ϴ� �޼���
    protected float DistanceToTarget()
    {
        // ���� �� ��ġ�� Ÿ�� ��ġ ���� �Ÿ��� ����Ͽ� ��ȯ
        return Vector3.Distance(transform.position, target.position);
    }

    // Ÿ�� ������ ����ϴ� �޼���
    protected Vector2 DirectionToTarget()
    {
        // Ÿ�ٰ� ���� ��ġ�� ���� ���̸� ����Ͽ� ����ȭ�� ������ ��ȯ
        return (target.position - transform.position).normalized;
    }

    // �ൿ�� ó���ϴ� �޼���: ���� ���¿� ���� �̵� �� ������ ����
    protected override void HandleAction()
    {
        base.HandleAction(); // �θ� Ŭ������ HandleAction() ȣ��

        // ���� �ڵ鷯�� Ÿ���� ������ �̵��� ���߰� ��ȯ
        if (target == null)
        {
            movementDirection = Vector2.zero;
        }

        // Ÿ�ٰ��� �Ÿ� �� ���� ���
        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        // �⺻������ ���� ���¸� false�� ����
        isAttacking = false;

        // Ÿ���� ���� ���� ���� ������
        if (distance <= followRange)
        {
            lookDirection = direction; // Ÿ���� ���� �������� �ٶ󺸱�

            // ���� ���� ���� ������ ���� ����
            if (distance <= attackRange)
            {
                // Ÿ�ٰ� ���� ���� ������ ����ĳ��Ʈ�� ����Ͽ� ���� Ÿ���� �´��� üũ
                int layerMaskTarget = LayerMask.GetMask("Player");
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, attackRange,
                    layerMaskTarget);

                Debug.DrawRay(transform.position, direction * attackRange, Color.red);

                // Ÿ���� ����ĳ��Ʈ�� ������ ���� ���·� ����
                if (hit.collider != null)
                {
                    isAttacking = true;

                    return;
                }

            }


            movementDirection = Vector2.zero; // �̵��� ����        

            //  Ÿ���� ���� �̵�
            movementDirection = direction;
        }

    }
    private void PerformRangedAttack()
    {
        // ���Ÿ� ������ RangeWeaponHandler���� ó���ϵ��� ȣ��
        if (rangeWeaponHandler != null)
        {
            rangeWeaponHandler.Attack(); // ���Ÿ� ������ ����
        }
    }
    public override void Death()
    {
        if (SkillManager.Instance.HealOnDeath)
        {
            SkillManager.Instance.HealPlayer(10f);
        }
        enemyManager.monsterList.Remove(this.gameObject);
        base.Death();
        playerController.target = null;
    }
}
