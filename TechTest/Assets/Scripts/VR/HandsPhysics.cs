using UnityEngine;

public class HandsPhysics : MonoBehaviour
{
    [SerializeField] Transform _target;
    private Rigidbody _rb;
    private Transform _t;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _t = transform;
    }
    private void FixedUpdate()
    {
        _rb.linearVelocity = (_target.position - _t.position) / Time.fixedDeltaTime;
        Quaternion rotationDifference = _target.rotation * Quaternion.Inverse(transform.rotation);
        rotationDifference.ToAngleAxis(out float angleInDegree, out Vector3 rotationAxis);
        Vector3 rotationDifferenceInDegree = angleInDegree * rotationAxis;
        _rb.angularVelocity = rotationDifferenceInDegree * Mathf.Deg2Rad / Time.fixedDeltaTime;
    }
}
