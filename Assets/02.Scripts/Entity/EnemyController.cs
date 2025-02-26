using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseController
{
    private EnemyManager enemyManager; // 적을 관리하는 매니저
    private Transform target; // 적이 추적할 타겟(주로 플레이어)

    [SerializeField] private float followRange = 15f; // 타겟을 추적할 거리 범위

    // 적의 초기화 메서드, enemyManager와 target을 설정
    public void Init(EnemyManager enemyManager, Transform target)
    {
        this.enemyManager = enemyManager; // 적을 관리하는 매니저 설정
        this.target = target; // 추적할 타겟 설정
    }

    // 타겟과의 거리 계산
    protected float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, target.position); // 적과 타겟 간의 거리 계산
    }

    // 적의 행동을 처리하는 메서드
    protected override void HandleAction()
    {
        base.HandleAction(); // 부모 클래스의 HandleAction 호출

        // weaponHandler가 없거나 target이 없다면, 적은 아무 동작도 하지 않음
        if (weaponHandler == null || target == null)
        {
            if (!movementDirection.Equals(Vector2.zero)) movementDirection = Vector2.zero; // 이동 중지
            return; // 더 이상 동작을 수행하지 않음
        }

        // 타겟과의 거리 계산
        float distance = DistanceToTarget();
        // 타겟을 향한 방향 계산
        Vector2 direction = DirectionToTarget();

        isAttacking = false; // 기본적으로 공격 상태는 아니라고 설정
        if (distance <= followRange) // 타겟이 추적 범위 내에 들어오면
        {
            lookDirection = direction; // 적은 타겟을 향해 본다

            // 타겟이 공격 범위 내에 있으면 공격을 시작
            if (distance <= weaponHandler.AttackRange)
            {
                int layerMaskTarget = weaponHandler.target; // 공격 가능한 레이어 설정
                // Raycast로 타겟이 공격 범위 내에 있는지 확인
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, weaponHandler.AttackRange * 1.5f,
                    (1 << LayerMask.NameToLayer("Level")) | layerMaskTarget);

                // 타겟이 공격 가능한 범위 내에 있으면 공격 상태로 변경
                if (hit.collider != null && layerMaskTarget == (layerMaskTarget | (1 << hit.collider.gameObject.layer)))
                {
                    isAttacking = true;
                }

                movementDirection = Vector2.zero; // 공격 범위 내에 있으면 이동 멈춤
                return;
            }

            movementDirection = direction; // 공격 범위 밖에 있으면 타겟을 향해 이동
        }

    }

    // 타겟을 향한 방향을 계산하는 메서드
    protected Vector2 DirectionToTarget()
    {
        return (target.position - transform.position).normalized; // 타겟 방향으로 정규화된 벡터 반환
    }

    // 적이 죽었을 때 호출되는 메서드
    public override void Death()
    {
        base.Death(); // 부모 클래스의 Death 호출
        enemyManager.RemoveEnemyOnDeath(this); // 적이 죽으면 적 관리 매니저에서 제거
    }
}
