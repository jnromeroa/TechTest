using Mirror;
using UnityEngine;

public enum BodyPartType
{
    Head,
    LeftHand,
    RightHand,
    None
}
/// <summary>
/// Synchronizes the position and rotation of a VR body part transform across the network.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class VRBodyPartFollower : NetworkBehaviour
{
    [SerializeField] private BodyPartType bodyPart;

    private INonPhysicalHandVisualizer _handVisualizer;
    private ILocomotionStateProvider _locomotionProvider;

    private Transform _target;
    private Transform _localTransform;
    private Rigidbody _rb;

    /// <summary>
    /// Unity Awake callback. Initializes references and casts injected components.
    /// </summary>
    private void Awake()
    {
        _target = GetTargetTransform();
        _rb = GetComponent<Rigidbody>();
        _localTransform = transform;
        _handVisualizer = GetComponent<INonPhysicalHandVisualizer>();
        _handVisualizer.Initialize(_target.GetComponentInChildren<Renderer>());
        _locomotionProvider = GetComponent<ILocomotionStateProvider>();

    }
    /// <summary>
    /// Fetch the singleton for the VR References.
    /// </summary>
    private Transform GetTargetTransform()
    {
        VRRigReferences rig = VRRigReferences.Instance;
        return bodyPart switch
        {
            BodyPartType.Head => rig.head,
            BodyPartType.LeftHand => rig.leftHand,
            BodyPartType.RightHand => rig.rightHand,
            _ => null,
        };
    }

    /// <summary>
    /// Handles hand visual logic on Update.
    /// </summary>
    private void Update()
    {
        if (!isOwned || _handVisualizer == null || !_handVisualizer.ShouldVisualize()) return;
        _handVisualizer.UpdateVisibility(_localTransform.position, _target.position);
    }

    /// <summary>
    /// Handles direct position/rotation sync while locomotion is active.
    /// </summary>
    private void LateUpdate()
    {
        if (!isOwned || _locomotionProvider == null || !_locomotionProvider.IsMoving() || _target == null) return;
        _localTransform.SetPositionAndRotation(_target.position, _target.rotation);
    }

    /// <summary>
    /// Handles Rigidbody-based movement synchronization while not moving.
    /// </summary>
    private void FixedUpdate()
    {
        if (!isOwned || _locomotionProvider == null || _locomotionProvider.IsMoving() || _target == null) return;
        SyncPosition();
        SyncRotation();
    }

    /// <summary>
    /// Sync Rigidbody linear velocity to follow the target position.
    /// </summary>
    private void SyncPosition()
    {
        _rb.linearVelocity = (_target.position - _localTransform.position) / Time.fixedDeltaTime;
    }

    /// <summary>
    /// Sync Rigidbody angular velocity to follow the target rotation.
    /// </summary>
    private void SyncRotation()
    {
        Quaternion delta = _target.rotation * Quaternion.Inverse(_localTransform.rotation);
        delta.ToAngleAxis(out float angle, out Vector3 axis);
        Vector3 angularVelocity = angle * axis * Mathf.Deg2Rad / Time.fixedDeltaTime;
        _rb.angularVelocity = angularVelocity;
    }
}





