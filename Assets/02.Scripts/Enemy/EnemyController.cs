using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseController
{
    // ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Ï´ï¿½ ï¿½Å´ï¿½ï¿½ï¿½
    private EnemyManager enemyManager;

    private PlayerController playerController;

    // ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ Å¸ï¿½ï¿½ (ï¿½Ã·ï¿½ï¿½Ì¾ï¿½ ï¿½ï¿½)
    private Transform target;

    [SerializeField] private Transform attackPivot;

    // ÀûÀÌ Å¸°ÙÀ» ÃßÀûÇÏ´Â ¹üÀ§ (°Å¸®)
    [SerializeField] private float followRange = 15f;

    // °ø°Ý ¹üÀ§
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
        {
            Rotate(lookDirection);
        }

    }

    // ÃÊ±âÈ­ ¸Þ¼­µå: EnemyManager¿Í Å¸°ÙÀ» ¼³Á¤
    public void Init()
    {
        enemyManager = EnemyManager.Instance;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        target = GameObject.Find("Player").transform;
    }

    // Å¸ï¿½Ù°ï¿½ï¿½ï¿½ ï¿½Å¸ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½Ï´ï¿?ï¿½Þ¼ï¿½ï¿½ï¿½
    protected float DistanceToTarget()
    {
        // ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ ï¿½ï¿½Ä¡ï¿½ï¿½ Å¸ï¿½ï¿½ ï¿½ï¿½Ä¡ ï¿½ï¿½ï¿½ï¿½ ï¿½Å¸ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½Ï¿ï¿?ï¿½ï¿½È¯
        return Vector3.Distance(transform.position, target.position);
    }

    // Å¸ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½Ï´ï¿?ï¿½Þ¼ï¿½ï¿½ï¿½
    protected Vector2 DirectionToTarget()
    {
        // Å¸ï¿½Ù°ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½Ä¡ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½Ì¸ï¿½ ï¿½ï¿½ï¿½ï¿½Ï¿ï¿?ï¿½ï¿½ï¿½ï¿½È­ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½È¯
        return (target.position - transform.position).normalized;
    }

    // ï¿½àµ¿ï¿½ï¿½ Ã³ï¿½ï¿½ï¿½Ï´ï¿½ ï¿½Þ¼ï¿½ï¿½ï¿½: ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½Â¿ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½Ìµï¿½ ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
    protected override void HandleAction()
    {
        base.HandleAction(); // ï¿½Î¸ï¿½ Å¬ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ HandleAction() È£ï¿½ï¿½

        // ï¿½ï¿½ï¿½ï¿½ ï¿½Úµé·¯ï¿½ï¿½ Å¸ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½Ìµï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ß°ï¿½ ï¿½ï¿½È¯
        if (target == null)
        {
            movementDirection = Vector2.zero;
            return;
        }

        // Å¸ï¿½Ù°ï¿½ï¿½ï¿½ ï¿½Å¸ï¿½ ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿?
        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        // ï¿½âº»ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½Â¸ï¿½ falseï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
        isAttacking = false;

        // Å¸ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
        if (distance <= followRange)
        {
            lookDirection = direction; // Å¸ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½Ù¶óº¸±ï¿½

            // ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
            if (distance <= attackRange)
            {
                // Å¸ï¿½Ù°ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½Ä³ï¿½ï¿½Æ®ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½Ï¿ï¿?ï¿½ï¿½ï¿½ï¿½ Å¸ï¿½ï¿½ï¿½ï¿½ ï¿½Â´ï¿½ï¿½ï¿½ Ã¼Å©
                int layerMaskTarget = LayerMask.GetMask("Player");
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, attackRange,
                    layerMaskTarget);

                Debug.DrawRay(transform.position, direction * attackRange, Color.red);

                // Å¸ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½Ä³ï¿½ï¿½Æ®ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½Â·ï¿½ ï¿½ï¿½ï¿½ï¿½
                if (hit.collider != null)
                {
                    isAttacking = true;
                }

                movementDirection = Vector2.zero; // ÀÌµ¿À» ¸ØÃã

                if(WeaponPrefab != null)
                {
                    // Å¸°Ù ¹æÇâÀ¸·Î ¹«±â È¸Àü
                    Vector3 targetDirection = target.transform.position - attackPivot.position;
                    lookDirection = targetDirection;
                    float angle = Mathf.Atan2(targetDirection.x, -targetDirection.y) * Mathf.Rad2Deg - 90f;
                    attackPivot.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                }

                return; // °ø°Ý ÁØºñ°¡ µÇ¾úÀ¸¹Ç·Î ¹ÝÈ¯
            }

            // ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½Ü¿ï¿½ï¿½ï¿½ Å¸ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½Ìµï¿½
            movementDirection = direction;
        }

    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f;

        characterRenderer.flipX = isLeft;
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

    public override void Death()
    {
        if (SkillManager.Instance.HealOnDeath)
        {
            SkillManager.Instance.HealPlayer(10f);
        }
        enemyManager.monsterList.Remove(this.gameObject);
        base.Death(); // ï¿½Î¸ï¿½ Å¬ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ Death() È£ï¿½ï¿½
        playerController.target = null;
        enemyManager.CheckStageClear();
    }
}
