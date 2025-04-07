using Mirror;
using System;
using UnityEngine;

public class FloatUntilCollision : NetworkBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        if (!isServer) return;
        rb.useGravity = false;
    }
    public void DisableFloat()
    {
        if (!isServer) return;
        rb.useGravity = true;
    }
}
