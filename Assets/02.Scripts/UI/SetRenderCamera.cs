using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRenderCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();

        Camera mainCamera = Camera.main;

        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = mainCamera;
    }
}
