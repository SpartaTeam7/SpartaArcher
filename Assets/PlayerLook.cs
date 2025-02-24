using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private Transform _player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            _player = playerObj.transform;
        }
        else
        {
            Debug.LogError("[PlayerLook] Can't find GameObject with tag 'Player'");
        }
    }

    void Update()
    {
        if (_player == null)
            return;

        // 플레이어 방향을 바라보도록 회전
        Vector3 direction = _player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
