using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StatHandler : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    [SerializeField] private int health = 100;

    public int Health
    {
        get => health;
        set => health = value;
    }
}
