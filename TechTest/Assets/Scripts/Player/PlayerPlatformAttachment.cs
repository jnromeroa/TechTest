using UnityEngine;

public class PlayerPlatformAttachment : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private Vector3 _localRayOrigin;
    [SerializeField] private float _rayDistance = 1.5f;
    [SerializeField] private LayerMask _floorLayer;
    private Transform _t;
    private Transform currentPlatform;

    private void Awake()
    {
        _t = transform;
    }
    void Update()
    {
        CheckForPlatformBelow();
    }

    private void CheckForPlatformBelow()
    {
        if (TryGetPlatformBelow(out Transform platform))
        {
            if (platform != currentPlatform)
            {
                AttachToPlatform(platform);
            }
        }
        else if (currentPlatform != null)
        {
            DetachFromPlatform();
        }
    }

    private bool TryGetPlatformBelow(out Transform platform)
    {
        RaycastHit hit;
        Vector3 rayOrigin = _t.position + _localRayOrigin;
        bool hitSomething = Physics.Raycast(rayOrigin, Vector3.down, out hit, _rayDistance, _floorLayer);

        if (hitSomething)
        {
            platform = hit.collider.transform;
            Debug.DrawRay(rayOrigin, Vector3.down * _rayDistance, Color.green);
            return true;
        }

        platform = null;
        Debug.DrawRay(rayOrigin, Vector3.down * _rayDistance, Color.red);
        return false;
    }

    private void AttachToPlatform(Transform platform)
    {
        currentPlatform = platform;
        transform.SetParent(currentPlatform);
        Debug.Log($"Attached to platform: {currentPlatform.name}");
    }

    private void DetachFromPlatform()
    {
        transform.SetParent(null);
        Debug.Log($"Detached from platform: {currentPlatform.name}");
        currentPlatform = null;
    }
}
