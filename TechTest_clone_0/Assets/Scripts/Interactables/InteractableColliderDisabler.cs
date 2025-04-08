using Mirror;
using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class InteractableColliderDisabler : NetworkBehaviour
{
    [SerializeField] bool _includeServer = false;
    private XRGrabInteractable _interactable;
    private Collider[] _colliders;
    private void Awake()
    {
        _colliders = GetComponentsInChildren<Collider>();
        if (isServer) return;
        _interactable = GetComponent<XRGrabInteractable>();
        _interactable.selectEntered.AddListener((args) =>
        {
            AssignOwnershipCmd(NetworkedPlayer.Local.connectionToClient);
        });
        _interactable.selectExited.AddListener((args) =>
        {
            RemoveOwnershipCmd();
        });

    }

    private void EnableColliders()
    {
        foreach (var collider in _colliders)
        {
            collider.enabled = true;
        }
    }
    private void DisableColliders()
    {
        foreach (var collider in _colliders)
        {
            collider.enabled = false;
        }
    }

    #region Server
    [Command(requiresAuthority = false)]
    private void RemoveOwnershipCmd()
    {
        netIdentity.RemoveClientAuthority();
        EnableCollidersRpc();
        if (!_includeServer) return;
        EnableColliders();

    }

    [Command(requiresAuthority = false)]
    void AssignOwnershipCmd(NetworkConnectionToClient conn)
    {
        netIdentity.AssignClientAuthority(conn);
        DisableCollidersRpc();
        if (!_includeServer) return;
        DisableColliders();
    }
    #endregion

    #region Client
    [ClientRpc(includeOwner = false)]
    void EnableCollidersRpc()
    {
        EnableColliders();
    }
    [ClientRpc(includeOwner = false)]
    void DisableCollidersRpc()
    {
        DisableColliders();
    }
    #endregion
}
