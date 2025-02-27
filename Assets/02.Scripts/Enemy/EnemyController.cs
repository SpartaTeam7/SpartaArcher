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

    [SerializeField] private Transform attackPivot;

    // ���� Ÿ���� �����ϴ� ���� (�Ÿ�)
    [SerializeField] private float followRange = 15f;

    // ���� ����
    [SerializeField] private float attackRange = 10f;

    [SerializeField] private float fireDelay = 1f;

    private bool isFire = false;

    public void Start()
    {
        Init();
    }

    private void Update()
    {
        if (!isFire && isAttacking)
        {
            isFire = true;
            StartCoroutine(OnFire());
        }

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
                }

                movementDirection = Vector2.zero; // �̵��� ����

                if(WeaponPrefab != null)
                {
                    // Ÿ�� �������� ���� ȸ��
                    Vector3 targetDirection = target.transform.position - attackPivot.position;
                    lookDirection = targetDirection;
                    float angle = Mathf.Atan2(targetDirection.x, -targetDirection.y) * Mathf.Rad2Deg - 90f;
                    attackPivot.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                }

                return; // ���� �غ� �Ǿ����Ƿ� ��ȯ
            }

            // ���� ���� �ܿ��� Ÿ���� ���� �̵�
            movementDirection = direction;
        }

    }

    protected override void Attack()
    {
        base.Attack();
    }

    private IEnumerator OnFire()
    {
        Attack();
        yield return new WaitForSeconds(fireDelay);
        isFire = false;
    }


    // ���� ������� �� ȣ��Ǵ� �޼���
    public override void Death()
    {
        enemyManager.monsterList.Remove(this.gameObject);
        base.Death(); // �θ� Ŭ������ Death() ȣ��
        playerController.target = null;
        enemyManager.CheckStageClear();
    }

}
