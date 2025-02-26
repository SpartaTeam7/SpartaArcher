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

    // �ൿ�� ó���ϴ� �޼���: ���� ���¿� ���� �̵� �� ������ ����
    protected override void HandleAction()
    {
        base.HandleAction(); // �θ� Ŭ������ HandleAction() ȣ��

        // ���� �ڵ鷯�� Ÿ���� ������ �̵��� ���߰� ��ȯ
        if (weaponHandler == null || target == null)
        {
            if (!movementDirection.Equals(Vector2.zero)) movementDirection = Vector2.zero;
            return;
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
            if (distance <= weaponHandler.AttackRange)
            {
                // Ÿ�ٰ� ���� ���� ������ ����ĳ��Ʈ�� ����Ͽ� ���� Ÿ���� �´��� üũ
                int layerMaskTarget = weaponHandler.target;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, weaponHandler.AttackRange * 1.5f,
                    (1 << LayerMask.NameToLayer("Level")) | layerMaskTarget);

                // Ÿ���� ����ĳ��Ʈ�� ������ ���� ���·� ����
                if (hit.collider != null && layerMaskTarget == (layerMaskTarget | (1 << hit.collider.gameObject.layer)))
                {
                    isAttacking = true;
                }

                movementDirection = Vector2.zero; // �̵��� ����
                return; // ���� �غ� �Ǿ����Ƿ� ��ȯ
            }

            // ���� ���� �ܿ��� Ÿ���� ���� �̵�
            movementDirection = direction;
        }

    }

    // Ÿ�� ������ ����ϴ� �޼���
    protected Vector2 DirectionToTarget()
    {
        // Ÿ�ٰ� ���� ��ġ�� ���� ���̸� ����Ͽ� ����ȭ�� ������ ��ȯ
        return (target.position - transform.position).normalized;
    }

    // ���� ������� �� ȣ��Ǵ� �޼���
    public override void Death()
    {
        enemyManager.monsterList.Remove(this.gameObject);
        base.Death(); // �θ� Ŭ������ Death() ȣ��
        playerController.target = null;
    }
}
