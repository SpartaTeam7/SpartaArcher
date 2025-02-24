using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private bool isReady;
    private float currentDuration;

    private Rigidbody2D rb;

    private RangeWeaponHandler rangeWeaponHandler;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isReady)
        {
            return;
        }

        currentDuration += Time.deltaTime;

        if (currentDuration > rangeWeaponHandler.Duration)
        {
            Destroy(this.gameObject);
        }

        rb.velocity = transform.right * rangeWeaponHandler.Speed;
    }

    public void Init(RangeWeaponHandler rangeWeaponHandler, Quaternion rotation)
    {
        this.rangeWeaponHandler = rangeWeaponHandler;
        currentDuration = 0;
        transform.rotation = rotation;
        transform.localScale = Vector3.one * rangeWeaponHandler.BulletSize;

        isReady = true;
    }
}
