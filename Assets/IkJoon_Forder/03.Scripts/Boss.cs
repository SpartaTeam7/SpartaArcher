using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Attack = Animator.StringToHash("Attack");

    private Animator _animator;
    private Transform _player;
    private StatHandler _playerStatHandler;

    public float followSpeed = 0.7f;
    private Vector2 _movement;
    public float attackRange = 3f;
    private bool _isAttacking = false; 
    private bool _playerInRange = false; // 플레이어가 범위 안에 있는지 확인

    private float _speedBoostTimer;
    private float _speedBoostDuration = 1f;
    private float _boostInterval = 10f;
    private float _timeSinceLastBoost;

    void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("[Boss] Can't find 'Animator' component on this GameObject.");
            return;
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogError("[Boss] Can't find GameObject with tag 'Player'");
            return;
        }

        _player = playerObj.transform;
        _playerStatHandler = playerObj.GetComponent<StatHandler>();

        if (_playerStatHandler == null)
        {
            Debug.LogError("[Boss] Player does not have a StatHandler component.");
        }
    }

    private void Update()
    {
        if (_player == null)
            return;

        // 10초마다 이동속도 증가
        _timeSinceLastBoost += Time.deltaTime;
        if (_timeSinceLastBoost >= _boostInterval)
        {
            _speedBoostTimer = _speedBoostDuration;
            _timeSinceLastBoost = 0f;
        }

        // 이동속도 조절
        followSpeed = (_speedBoostTimer > 0) ? 2f : 0.7f;
        _speedBoostTimer -= Time.deltaTime;

        // 플레이어가 공격 범위 안에 있는지 확인
        bool isPlayerInRange = Vector2.Distance(transform.position, _player.position) <= attackRange;
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
        }

        FollowPlayer();
        UpdateAnimation();
    }

    private void FollowPlayer()
    {
        Vector2 direction = (_player.position - transform.position).normalized;
        _movement = direction;
        transform.position = Vector2.Lerp(transform.position, _player.position, followSpeed * Time.deltaTime);
    }

    private void UpdateAnimation()
    {
        _animator.SetFloat(Horizontal, _movement.x);
        _animator.SetFloat(Vertical, _movement.y);
        _animator.SetBool(Walk, _movement.magnitude > 0.1f);
    }

    private IEnumerator DelayedAttackStart()
    {
        yield return new WaitForSeconds(0.4f); // 0.4초 대기 후 공격 시작
        if (_playerInRange)
        {
            _isAttacking = true;
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
            yield return new WaitForSeconds(0.4f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
