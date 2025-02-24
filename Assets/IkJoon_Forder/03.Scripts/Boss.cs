using System;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Attack = Animator.StringToHash("Attack");

    private Animator _animator;
    private Transform _player;
    public float followSpeed = 0.7f; // 기본 이동 속도
    private Vector2 _movement;
    private float attackRange = 3f;

    private float _speedBoostTimer; // 이동속도 증가 타이머
    private float _speedBoostDuration = 1f; // 이동속도 증가 지속 시간 (1초)
    private float _boostInterval = 10f; // 속도 증가 간격 (10초)
    private float _timeSinceLastBoost; // 마지막 속도 증가 이후 경과 시간

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("[DemoPlayer.cs] Can't find 'Animator' component on this GameObject.");
            return;
        }

        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (_player == null)
        {
            Debug.LogError("[DemoPlayer.cs] Can't find GameObject with tag 'Player'");
            return;
        }
    }

    private void Update()
    {
        if (_player == null)
            return;

        // 10초 간격으로 이동속도 증가
        _timeSinceLastBoost += Time.deltaTime;
        if (_timeSinceLastBoost >= _boostInterval)
        {
            _speedBoostTimer = _speedBoostDuration; // 1초 동안 속도 증가
            _timeSinceLastBoost = 0f; // 타이머 리셋
        }

        // 이동속도 증가 타이머가 0보다 크면 이동속도 증가
        if (_speedBoostTimer > 0)
        {
            followSpeed = 2f;
            _speedBoostTimer -= Time.deltaTime; // 타이머 감소
        }
        else
        {
            followSpeed = 0.7f; // 원래 속도로 돌아감
        }

        bool isPlayerInRange = Vector2.Distance(transform.position, _player.position) <= attackRange;
        _animator.SetBool(Attack, isPlayerInRange);

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);  // 공격 범위 표시
    }
}
