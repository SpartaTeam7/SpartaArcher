using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseController
{
    // 적을 관리하는 매니저
    private EnemyManager enemyManager;

    private PlayerController playerController;

    // 적이 추적할 타겟 (플레이어 등)
    private Transform target;

    // 적이 타겟을 추적하는 범위 (거리)
    [SerializeField] private float followRange = 15f;

    public void Start()
    {
        Init();
    }

    // 초기화 메서드: EnemyManager와 타겟을 설정
    public void Init()
    {
        enemyManager = EnemyManager.Instance;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        target = GameObject.Find("Player").transform;
    }

    // 타겟과의 거리를 계산하는 메서드
    protected float DistanceToTarget()
    {
        // 현재 적 위치와 타겟 위치 간의 거리를 계산하여 반환
        return Vector3.Distance(transform.position, target.position);
    }

    // 행동을 처리하는 메서드: 적의 상태에 맞춰 이동 및 공격을 제어
    protected override void HandleAction()
    {
        base.HandleAction(); // 부모 클래스의 HandleAction() 호출

        // 무기 핸들러나 타겟이 없으면 이동을 멈추고 반환
        if (weaponHandler == null || target == null)
        {
            if (!movementDirection.Equals(Vector2.zero)) movementDirection = Vector2.zero;
            return;
        }

        // 타겟과의 거리 및 방향 계산
        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        // 기본적으로 공격 상태를 false로 설정
        isAttacking = false;

        // 타겟이 추적 범위 내에 있으면
        if (distance <= followRange)
        {
            lookDirection = direction; // 타겟을 향한 방향으로 바라보기

            // 공격 범위 내에 들어오면 공격 시작
            if (distance <= weaponHandler.AttackRange)
            {
                // 타겟과 공격 범위 내에서 레이캐스트를 사용하여 실제 타겟이 맞는지 체크
                int layerMaskTarget = weaponHandler.target;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, weaponHandler.AttackRange * 1.5f,
                    (1 << LayerMask.NameToLayer("Level")) | layerMaskTarget);

                // 타겟이 레이캐스트에 맞으면 공격 상태로 설정
                if (hit.collider != null && layerMaskTarget == (layerMaskTarget | (1 << hit.collider.gameObject.layer)))
                {
                    isAttacking = true;
                }

                movementDirection = Vector2.zero; // 이동을 멈춤
                return; // 공격 준비가 되었으므로 반환
            }

            // 공격 범위 외에는 타겟을 향해 이동
            movementDirection = direction;
        }

    }

    // 타겟 방향을 계산하는 메서드
    protected Vector2 DirectionToTarget()
    {
        // 타겟과 현재 위치의 벡터 차이를 계산하여 정규화된 방향을 반환
        return (target.position - transform.position).normalized;
    }

    // 적이 사망했을 때 호출되는 메서드
    public override void Death()
    {
        enemyManager.monsterList.Remove(this.gameObject);
        base.Death(); // 부모 클래스의 Death() 호출
        playerController.target = null;
    }
}
