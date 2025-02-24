using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Attack = Animator.StringToHash("Attack");

    private Animator _animator;
    private Transform _playerTf;
    private StatHandler _playerStatHandler;

    public float followSpeed = 0.7f;
    private Vector2 _movement;
    public float attackRange = 3f;
    private bool _isAttacking = false; 
    private bool _playerInRange = false; // 플레이어가 범위 안에 있는지 확인

    private float _speedBoostTimer;
    private float _speedBoostDuration = 2f;
    private float _boostInterval = 10f;
    private float _timeSinceLastBoost;

    public GameObject attackEffect;
    public GameObject buffEffect;
    

    void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            return;
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null)
        {
            return;
        }

        _playerTf = playerObj.transform;
        _playerStatHandler = playerObj.GetComponent<StatHandler>();

        if (_playerStatHandler == null)
        {
            Debug.LogError("[Boss] Player does not have a StatHandler component.");
        }
        attackEffect.SetActive(false);
        buffEffect.SetActive(false);
    }

    private void Update()
{
    if (_playerTf == null)
        return;

    // 10초마다 이동속도 증가
    _timeSinceLastBoost += Time.deltaTime;
    if (_timeSinceLastBoost >= _boostInterval)
    {
        _speedBoostTimer = _speedBoostDuration;
        _timeSinceLastBoost = 0f;
    }

    // 이동속도 조절
    bool isBoosted = _speedBoostTimer > 0;
    followSpeed = isBoosted ? 2f : 0.7f;
    _speedBoostTimer -= Time.deltaTime;

    // 이동 속도가 빠르면 버프 이펙트 활성화, 느려지면 비활성화
    if (buffEffect != null)
    {
        buffEffect.SetActive(isBoosted);
    }

    // 플레이어가 공격 범위 안에 있는지 확인
    bool isPlayerInRange = Vector2.Distance(transform.position, _playerTf.position) <= attackRange;
    _animator.SetBool(Attack, isPlayerInRange);

    if (isPlayerInRange && !_playerInRange)
    {
        _playerInRange = true;
        StartCoroutine(DelayedAttackStart()); // 0.4초 후 공격 시작
    }
    else if (!isPlayerInRange && _playerInRange)
    {
        _playerInRange = false;
        _isAttacking = false;
        if (attackEffect != null) attackEffect.SetActive(false);
    }

    FollowPlayer();
    UpdateAnimation();
}


    private void FollowPlayer()
    {
        Vector2 direction = (_playerTf.position - transform.position).normalized;
        _movement = direction;
        transform.position = Vector2.Lerp(transform.position, _playerTf.position, followSpeed * Time.deltaTime);
    }

    private void UpdateAnimation()
    {
        _animator.SetFloat(Horizontal, _movement.x);
        _animator.SetFloat(Vertical, _movement.y);
        _animator.SetBool(Walk, _movement.magnitude > 0.1f);
    }

    private IEnumerator DelayedAttackStart()
    {
        yield return new WaitForSeconds(0.45f); // 0.4초 대기 후 공격 시작
        if (_playerInRange)
        {
            _isAttacking = true;
            attackEffect.SetActive(true);
            StartCoroutine(AttackPlayerRepeatedly());
        }
    }

    private IEnumerator AttackPlayerRepeatedly()
    {
        while (_isAttacking)
        {
            if (_playerStatHandler != null)
            {
                _playerStatHandler.Health -= 10;
                Debug.Log($"[Boss] Player hit! Remaining HP: {_playerStatHandler.Health}");
            }
            yield return new WaitForSeconds(0.45f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
