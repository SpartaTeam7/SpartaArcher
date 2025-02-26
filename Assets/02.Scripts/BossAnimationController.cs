using UnityEngine;

public class BossAnimatorController : MonoBehaviour
{
    private Animator animator;

    public bool isWalking;
    public bool isAttacking;
    public bool isCasting;
    public bool isSpelling;
    public bool isDead;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Animator 파라미터 업데이트
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isCasting", isCasting);
        animator.SetBool("isSpelling", isSpelling);
        animator.SetBool("isDead", isDead);
    }
}
