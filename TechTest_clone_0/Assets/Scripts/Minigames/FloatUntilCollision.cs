using Mirror;
using System;
using UnityEngine;

public class FloatUntilCollision : NetworkBehaviour
{
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        if (!isServer) return;
        _rb.useGravity = false;
    }
    public void DisableFloat()
    {
        if (!isServer) return;
        _rb.useGravity = true;
    }
}
