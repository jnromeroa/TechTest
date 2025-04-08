using Mirror;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;

public enum VRBodyPart
    {
        HEAD,
        RIGHT,
        LEFT,
        NONE
    }
public class PhysicallyVRFollower : NetworkBehaviour
{
    [SerializeField] private VRBodyPart _bodyPart = VRBodyPart.NONE;
    [SerializeField] private float _distanceToShowNonPhysicalHand = 0.05f;
    private Renderer _nonPhysicalHand;
    private Transform _target;
    private Rigidbody _rb;
    private Transform _t;
    private bool _showingNonPhysicalHand = false;
    private bool _isMoving = false;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _t = transform;
        var origin = FindFirstObjectByType<XROrigin>();
        var moveProvider = origin.GetComponentInChildren<ContinuousMoveProvider>();
        moveProvider.locomotionStarted += (provider) => _isMoving = true;
        moveProvider.locomotionEnded += (provider) => _isMoving = false;
        switch (_bodyPart)
        {
            case VRBodyPart.HEAD:
                _target = VRRigReferences.Instance.head;
                break;
            case VRBodyPart.RIGHT:
                _target = VRRigReferences.Instance.rightHand;
                _nonPhysicalHand = _target.GetComponentInChildren<Renderer>();
                _nonPhysicalHand.enabled = false;
                break;
            case VRBodyPart.LEFT:
                _target = VRRigReferences.Instance.leftHand;
                _nonPhysicalHand = _target.GetComponentInChildren<Renderer>();
                _nonPhysicalHand.enabled = false;
                break;
            default:
                break;
        }
    }
    private void Update()
    {
        if (!isOwned) return;
        if (_bodyPart == VRBodyPart.NONE || _bodyPart == VRBodyPart.HEAD) return;
        CheckNonPhysicalHandDistance();
    }

    private void CheckNonPhysicalHandDistance()
    {
        float distanceBetweenHands = Vector3.Distance(_t.position, _target.position);
        if (distanceBetweenHands >= _distanceToShowNonPhysicalHand && !_showingNonPhysicalHand)
        {
            _showingNonPhysicalHand = true;
            _nonPhysicalHand.enabled = true;
            return;
        }
        if (distanceBetweenHands < _distanceToShowNonPhysicalHand && _showingNonPhysicalHand)
        {
            _showingNonPhysicalHand = false;
            _nonPhysicalHand.enabled = false;
        }
    }

    private void LateUpdate()
    {
        if (!isOwned) return;
        if (!_isMoving) return;
        if (_bodyPart == VRBodyPart.NONE) return;
        _t.SetPositionAndRotation(_target.position, _target.rotation);
    }
    private void FixedUpdate()
    {
        if (!isOwned) return;
        if (_isMoving) return;
        if (_bodyPart == VRBodyPart.NONE) return;
        SyncPosition();
        SyncRotation();
    }
    void SyncPosition()
    {
        _rb.linearVelocity = (_target.position - _t.position) / Time.fixedDeltaTime;

    }
    void SyncRotation()
    {
        Quaternion rotationDifference = _target.rotation * Quaternion.Inverse(transform.rotation);
        rotationDifference.ToAngleAxis(out float angleInDegree, out Vector3 rotationAxis);
        Vector3 rotationDifferenceInDegree = angleInDegree * rotationAxis;
        _rb.angularVelocity = rotationDifferenceInDegree * Mathf.Deg2Rad / Time.fixedDeltaTime;
    }
}
