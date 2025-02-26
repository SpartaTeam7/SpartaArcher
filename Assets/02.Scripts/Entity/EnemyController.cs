using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseController
{
    private EnemyManager enemyManager; // ���� �����ϴ� �Ŵ���
    private Transform target; // ���� ������ Ÿ��(�ַ� �÷��̾�)

    [SerializeField] private float followRange = 15f; // Ÿ���� ������ �Ÿ� ����

    // ���� �ʱ�ȭ �޼���, enemyManager�� target�� ����
    public void Init(EnemyManager enemyManager, Transform target)
    {
        this.enemyManager = enemyManager; // ���� �����ϴ� �Ŵ��� ����
        this.target = target; // ������ Ÿ�� ����
    }

    // Ÿ�ٰ��� �Ÿ� ���
    protected float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, target.position); // ���� Ÿ�� ���� �Ÿ� ���
    }

    // ���� �ൿ�� ó���ϴ� �޼���
    protected override void HandleAction()
    {
        base.HandleAction(); // �θ� Ŭ������ HandleAction ȣ��

        // weaponHandler�� ���ų� target�� ���ٸ�, ���� �ƹ� ���۵� ���� ����
        if (weaponHandler == null || target == null)
        {
            if (!movementDirection.Equals(Vector2.zero)) movementDirection = Vector2.zero; // �̵� ����
            return; // �� �̻� ������ �������� ����
        }

        // Ÿ�ٰ��� �Ÿ� ���
        float distance = DistanceToTarget();
        // Ÿ���� ���� ���� ���
        Vector2 direction = DirectionToTarget();

        isAttacking = false; // �⺻������ ���� ���´� �ƴ϶�� ����
        if (distance <= followRange) // Ÿ���� ���� ���� ���� ������
        {
            lookDirection = direction; // ���� Ÿ���� ���� ����

            // Ÿ���� ���� ���� ���� ������ ������ ����
            if (distance <= weaponHandler.AttackRange)
            {
                int layerMaskTarget = weaponHandler.target; // ���� ������ ���̾� ����
                // Raycast�� Ÿ���� ���� ���� ���� �ִ��� Ȯ��
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, weaponHandler.AttackRange * 1.5f,
                    (1 << LayerMask.NameToLayer("Level")) | layerMaskTarget);

                // Ÿ���� ���� ������ ���� ���� ������ ���� ���·� ����
                if (hit.collider != null && layerMaskTarget == (layerMaskTarget | (1 << hit.collider.gameObject.layer)))
                {
                    isAttacking = true;
                }

                movementDirection = Vector2.zero; // ���� ���� ���� ������ �̵� ����
                return;
            }

            movementDirection = direction; // ���� ���� �ۿ� ������ Ÿ���� ���� �̵�
        }

    }

    // Ÿ���� ���� ������ ����ϴ� �޼���
    protected Vector2 DirectionToTarget()
    {
        return (target.position - transform.position).normalized; // Ÿ�� �������� ����ȭ�� ���� ��ȯ
    }

    // ���� �׾��� �� ȣ��Ǵ� �޼���
    public override void Death()
    {
        base.Death(); // �θ� Ŭ������ Death ȣ��
        enemyManager.RemoveEnemyOnDeath(this); // ���� ������ �� ���� �Ŵ������� ����
    }
}
