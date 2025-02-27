using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponHandler : WeaponHandler
{
    public Vector2 colliderBoxSize = Vector2.one;

    [SerializeField] private ParticleSystem attackEff;

    private void Start()
    {
        colliderBoxSize = colliderBoxSize * WeaponSize;
    }

    public override void Attack()
    {
        base.Attack();

        // BoxCast�� ������ ���
        Vector2 boxCastOrigin = transform.position + (Vector3)Controller.LookDirection * colliderBoxSize.x;

        // BoxCast�� ����
        RaycastHit2D hit = Physics2D.BoxCast(boxCastOrigin, colliderBoxSize, 0, Vector2.zero, 0, target);

        // BoxCast�� �������� ������ ����Ͽ� Ray�� �׸���
        Debug.DrawRay(boxCastOrigin, Vector2.right * colliderBoxSize.x / 2, Color.red, 0.1f); // ������ �������� Ray
        Debug.DrawRay(boxCastOrigin, Vector2.left * colliderBoxSize.x / 2, Color.red, 0.1f); // ���� �������� Ray
        Debug.DrawRay(boxCastOrigin, Vector2.up * colliderBoxSize.y / 2, Color.green, 0.1f); // ���� �������� Ray
        Debug.DrawRay(boxCastOrigin, Vector2.down * colliderBoxSize.y / 2, Color.green, 0.1f);

        if (hit.collider != null)
        {
            ResourceController resourceController = hit.collider.GetComponent<ResourceController>();
            if (resourceController != null)
            {
                resourceController.ChangeHealth(-Power);
                attackEff.Play();

                if (IsOnKnockback)
                {
                    BaseController controller = hit.collider.GetComponent<BaseController>();
                    if (controller != null)
                    {
                        controller.ApplyKnockback(transform, KnockbackPower, KnockbackTime);
                    }
                }
            }
        }
    }
}
