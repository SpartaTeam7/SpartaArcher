using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    public float damage = 10f; // 데미지 값
    public float checkRadius = 2f; // 검사할 반경 (씬에서 확인 가능)

    void OnEnable()
    {
        Debug.Log("DamageArea 활성화됨!");

        // 현재 DamageArea의 위치를 기준으로 반경 안에 있는 모든 콜라이더 검색
        Collider[] colliders = Physics.OverlapSphere(transform.position, checkRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                Debug.Log("DamageArea 활성화 시 플레이어가 영역 안에 있음!");

                PlayerMovement pm = collider.GetComponent<PlayerMovement>();
                if (pm != null)
                {
                    Debug.Log("플레이어의 체력 감소 전: " + pm.playerHp);
                    pm.TakeDamage(damage);
                    Debug.Log("플레이어의 체력 감소 후: " + pm.playerHp);
                }
                else
                {
                    Debug.LogError("PlayerMovement 컴포넌트를 찾을 수 없음!");
                }
                return; // 플레이어를 찾으면 더 이상 반복할 필요 없음
            }
        }

        Debug.Log("DamageArea 활성화 시 플레이어가 없음.");
    }

    // ✅ 기즈모(Gizmos)로 반경을 표시 (씬에서 시각적으로 확인 가능)
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // 색상 설정 (빨간색)
        Gizmos.DrawWireSphere(transform.position, checkRadius); // 반경을 원으로 그리기
    }
}
