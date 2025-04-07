using Mirror;
using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ObjectColliderDisabler : NetworkBehaviour
{
    [SerializeField] bool includeServer = false;
    private XRGrabInteractable interactable;
    private Collider[] colliders;
    

    private void Awake()
    {
        colliders = GetComponentsInChildren<Collider>();
        if (isServer) return;
        interactable = GetComponent<XRGrabInteractable>();
        interactable.selectEntered.AddListener((args) =>
        {
            AssignOwnershipCmd(NetworkedPlayer.Local.connectionToClient);
        });
        interactable.selectExited.AddListener((args) =>
        {
            RemoveOwnershipCmd();
        });
        
    }
    [Command(requiresAuthority = false)]
    private void RemoveOwnershipCmd()
    {
        netIdentity.RemoveClientAuthority();
        EnableCollidersRpc();
        if (!includeServer) return;
        EnableColliders();

    }

    [Command(requiresAuthority = false)]
    void AssignOwnershipCmd(NetworkConnectionToClient conn)
    {
        netIdentity.AssignClientAuthority(conn);
        DisableCollidersRpc();
        if (!includeServer) return;
        DisableColliders();
    }
    [ClientRpc(includeOwner = false)]
    void EnableCollidersRpc()
    {
        EnableColliders();
    }
    void EnableColliders()
    {
        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }
    }
    [ClientRpc(includeOwner = false)]
    void DisableCollidersRpc()
    {
        DisableColliders();
    }
    void DisableColliders()
    {
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
    }
}
