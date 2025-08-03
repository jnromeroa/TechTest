using UnityEngine;

/// <summary>
/// Handles automatic attachment and detachment of the player to platforms below them using raycasting.
/// Useful for moving platforms or dynamic terrain.
/// </summary>
public class PlayerPlatformAttachment : MonoBehaviour
{

    [Header("Raycast Settings")]

    /// <summary>
    /// Local offset from the player's position to start the raycast.
    /// </summary>
    [SerializeField] private Vector3 _localRayOrigin;

    /// <summary>
    /// Maximum distance for the raycast to detect platforms.
    /// </summary>
    [SerializeField] private float _rayDistance = 1.5f;

    /// <summary>
    /// LayerMask to specify which layers count as valid platforms.
    /// </summary>
    [SerializeField] private LayerMask _floorLayer;

    /// <summary>
    /// Cached transform of the player.
    /// </summary>
    private Transform _t;

    /// <summary>
    /// Reference to the currently attached platform.
    /// </summary>
    private Transform currentPlatform;


    /// <summary>
    /// Caches the transform on Awake.
    /// </summary>
    private void Awake()
    {
        _t = transform;
    }

    /// <summary>
    /// Continuously checks for a platform below and updates parent transform accordingly.
    /// </summary>
    private void Update()
    {
        CheckForPlatformBelow();
    }


    /// <summary>
    /// Checks whether there is a platform below and handles attachment or detachment.
    /// </summary>
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

    /// <summary>
    /// Performs a raycast downward to detect a valid platform.
    /// </summary>
    /// <param name="platform">Out parameter that receives the detected platform's transform.</param>
    /// <returns>True if a platform was found below the player.</returns>
    private bool TryGetPlatformBelow(out Transform platform)
    {
        Vector3 rayOrigin = _t.position + _localRayOrigin;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, _rayDistance, _floorLayer))
        {
            platform = hit.collider.transform;
            Debug.DrawRay(rayOrigin, Vector3.down * _rayDistance, Color.green);
            return true;
        }

        platform = null;
        Debug.DrawRay(rayOrigin, Vector3.down * _rayDistance, Color.red);
        return false;
    }

    /// <summary>
    /// Parents the player to the detected platform to follow its movement.
    /// </summary>
    /// <param name="platform">The platform transform to attach to.</param>
    private void AttachToPlatform(Transform platform)
    {
        currentPlatform = platform;
        _t.SetParent(currentPlatform);
    }

    /// <summary>
    /// Detaches the player from the current platform.
    /// </summary>
    private void DetachFromPlatform()
    {
        _t.SetParent(null);
        currentPlatform = null;
    }

}