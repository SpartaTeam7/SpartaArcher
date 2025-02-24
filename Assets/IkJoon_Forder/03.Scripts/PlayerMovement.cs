using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private StatHandler _stHandler;
    public float moveSpeed; // 이동 속도
    public float playerHp;

    void Start()
    {
        _stHandler = GetComponent<StatHandler>();
        moveSpeed = _stHandler.Speed;
        playerHp = _stHandler.Health;

    }
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // A, D 또는 왼쪽/오른쪽 화살표 키
        float moveY = Input.GetAxis("Vertical");   // W, S 또는 위/아래 화살표 키

        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;
        transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;
    }
}
