using Mirror;
using UnityEngine;


/// <summary>
/// Manages the enabling and disabling of hand colliders on the local player,
/// syncing the state across the network when objects are grabbed or released.
/// </summary>
public class HandCollidersManager : NetworkBehaviour
{
    #region Serialized Fields

    /// <summary>
    /// The hand this collider manager is associated with.
    /// </summary>
    [SerializeField] private BodyPartType _bodyPart = BodyPartType.None;

    /// <summary>
    /// Time in seconds to wait before re-enabling colliders after release.
    /// </summary>
    [SerializeField] private float _delaySeconds = 0.5f;

    #endregion

    #region SyncVars

    /// <summary>
    /// Indicates whether the hand colliders are currently enabled.
    /// Synced across the network.
    /// </summary>
    [SyncVar(hook = nameof(OnEnableCollidersChanged))]
    private bool areCollidersEnabled = true;

    #endregion

    #region Private Fields

    private Collider[] handColliders;

    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Caches references to the child colliders of the hand.
    /// </summary>
    private void Awake()
    {
        handColliders = GetComponentsInChildren<Collider>();
    }

    #endregion

    #region Client

    /// <summary>
    /// Called when this client becomes the authority over this object.
    /// Sets up interaction listeners based on which hand this script is attached to.
    /// </summary>
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor interactor = null;

        switch (_bodyPart)
        {
            case BodyPartType.RightHand:
                interactor = VRRigReferences.Instance.rightHand.parent.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor>();
                break;

            case BodyPartType.LeftHand:
                interactor = VRRigReferences.Instance.leftHand.parent.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor>();
                break;
        }

        if (interactor != null)
        {
            interactor.selectEntered.AddListener(_ => Disable());
            interactor.selectExited.AddListener(_ => EnableWithDelay(_delaySeconds));
        }
    }

    /// <summary>
    /// Disables the colliders on this hand and notifies the server.
    /// </summary>
    public void Disable()
    {
        DisableCmd();
        DisableColliders();
    }

    /// <summary>
    /// Enables the colliders on this hand and notifies the server.
    /// </summary>
    public void Enable()
    {
        EnableCmd();
        EnableColliders();
    }

    /// <summary>
    /// Enables the colliders after a delay.
    /// </summary>
    /// <param name="time">Delay in seconds before enabling colliders.</param>
    public void EnableWithDelay(float time)
    {
        Invoke(nameof(Enable), time);
    }

    #endregion

    #region Server

    /// <summary>
    /// Server-side command to enable colliders across the network.
    /// </summary>
    [Command]
    private void EnableCmd()
    {
        areCollidersEnabled = true;
        EnableColliders();
    }

    /// <summary>
    /// Server-side command to disable colliders across the network.
    /// </summary>
    [Command]
    private void DisableCmd()
    {
        areCollidersEnabled = false;
        DisableColliders();
    }

    #endregion

    #region Collider Control

    /// <summary>
    /// Enables all colliders on the hand.
    /// </summary>
    private void EnableColliders()
    {
        foreach (Collider collider in handColliders)
        {
            collider.enabled = true;
        }
    }

    /// <summary>
    /// Disables all colliders on the hand.
    /// </summary>
    private void DisableColliders()
    {
        foreach (Collider collider in handColliders)
        {
            collider.enabled = false;
        }
    }

    #endregion

    #region SyncVar Callback

    /// <summary>
    /// Called when <see cref="areCollidersEnabled"/> changes.
    /// Updates the collider state locally.
    /// </summary>
    /// <param name="oldValue">Previous value of the collider state.</param>
    /// <param name="newValue">New value of the collider state.</param>
    private void OnEnableCollidersChanged(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            EnableColliders();
        }
        else
        {
            DisableColliders();
        }
    }

    #endregion
}
