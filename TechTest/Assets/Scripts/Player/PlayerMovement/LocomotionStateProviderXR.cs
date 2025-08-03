using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;

public class LocomotionStateProviderXR : MonoBehaviour, ILocomotionStateProvider
{
    private bool _isMoving = false;

    private void Awake()
    {
        var origin = FindFirstObjectByType<XROrigin>();
        var moveProvider = origin.GetComponentInChildren<ContinuousMoveProvider>();
        moveProvider.locomotionStarted += (_) => _isMoving = true;
        moveProvider.locomotionEnded += (_) => _isMoving = false;
    }

    public bool IsMoving() => _isMoving;
}
