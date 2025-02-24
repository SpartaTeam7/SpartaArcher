using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{

    //  Input System으로 키입력을 받아 플레이어를 움직이는 함수
    private void OnMove(InputValue value)
    {
        movementDirection = value.Get<Vector2>();
        movementDirection = movementDirection.normalized;
    }
}
