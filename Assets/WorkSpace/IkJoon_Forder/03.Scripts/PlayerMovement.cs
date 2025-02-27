using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private StatHandler _stHandler;
    public float moveSpeed; // 이동 속도
    public float playerHp;

    private Boss _boss;

    void Start()
    {
        _stHandler = GetComponent<StatHandler>();
        
        moveSpeed = _stHandler.Speed;
        playerHp = _stHandler.Health;

        // 보스를 찾아서 가져오기
        GameObject bossObj = GameObject.FindGameObjectWithTag("Enemy");
        if (bossObj != null)
        {
            _boss = bossObj.GetComponent<Boss>();
        }
        else
        {
            Debug.LogError("[PlayerMovement] Boss not found! Check if the Boss has the correct tag.");
        }

    }
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // A, D 또는 왼쪽/오른쪽 화살표 키
        float moveY = Input.GetAxis("Vertical");   // W, S 또는 위/아래 화살표 키

        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;
        transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;
        
        if(Input.GetMouseButtonDown(0))
        {
            _boss.currBossHp -= 10f;
        }
    }
    public void TakeDamage(float damage)
    {
        playerHp -= damage;
        Debug.Log("플레이어가 " + damage + "의 피해를 입음! 현재 체력: " + playerHp);
        
        if (playerHp <= 0)
        {
            Debug.Log("플레이어가 사망했습니다!");
            // 사망 처리 로직 추가 가능
        }
    }
}
