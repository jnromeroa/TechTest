using Mirror;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// A networked platform that moves along the Z-axis when activated.
/// It uses Mirror for synchronization and DOTween for smooth movement.
/// </summary>
public class NetworkPlatform : NetworkBehaviour
{

    /// <summary>
    /// The target Z-axis position the platform should move to.
    /// </summary>
    [SerializeField] private float _targetZPosition;

    /// <summary>
    /// Time it takes to reach the target Z position.
    /// </summary>
    [SerializeField] private float _moveDurationSeconds = 4f;

    /// <summary>
    /// Wait time before returning to the original position.
    /// </summary>
    [SerializeField] private float _waitDurationSeconds = 2f;


    /// <summary>
    /// Current Z-axis position of the platform, synced across the network.
    /// </summary>
    [SyncVar(hook = nameof(OnZPosChanged))]
    private float _zPosition;

    /// <summary>
    /// Indicates if the platform is currently moving.
    /// </summary>
    [SyncVar]
    private bool _isMoving;


    /// <summary>
    /// The original Z position the platform started at.
    /// </summary>
    private float _originalZPosition;

    /// <summary>
    /// Cached transform reference.
    /// </summary>
    private Transform _t;



    /// <summary>
    /// Caches the transform and stores the original position.
    /// </summary>
    private void Awake()
    {
        _t = transform;
        _originalZPosition = _t.position.z;
    }


    #region Server

    /// <summary>
    /// Initializes Z position state on the server.
    /// </summary>
    public override void OnStartServer()
    {
        base.OnStartServer();
        _zPosition = _t.position.z;
    }

    /// <summary>
    /// Command to activate the platform movement to the target Z position.
    /// </summary>
    [Command(requiresAuthority = false)]
    public void ActivateCmd()
    {
        if (_isMoving) return;

        _isMoving = true;

        _t.DOMoveZ(_targetZPosition, _moveDurationSeconds)
            .SetEase(Ease.InOutCubic)
            .OnComplete(() =>
            {
                _isMoving = false;
                Invoke(nameof(Deactivate), _waitDurationSeconds);
            });
    }

    /// <summary>
    /// Updates the Z position while the platform is moving (server only).
    /// </summary>
    [ServerCallback]
    private void Update()
    {
        if (!_isMoving) return;
        _zPosition = _t.position.z;
    }
    #endregion

    #region Client

    /// <summary>
    /// Context menu helper for manually activating the platform in the editor.
    /// </summary>
    [ContextMenu("Activate")]
    public void Activate()
    {
        ActivateCmd();
    }

    /// <summary>
    /// Resets the platform to its original Z position.
    /// Called after a delay when the movement to target finishes.
    /// </summary>
    [ClientRpc]
    public void Deactivate()
    {
        if (_isMoving) return;

        _isMoving = true;

        _t.DOMoveZ(_originalZPosition, _moveDurationSeconds)
            .SetEase(Ease.InOutCubic)
            .OnComplete(() => _isMoving = false);
    }

    #endregion

    #region SyncVar Callbacks

    /// <summary>
    /// Updates the position of the platform on clients when Z position changes.
    /// </summary>
    /// <param name="oldValue">Previous Z value.</param>
    /// <param name="newValue">New Z value.</param>
    private void OnZPosChanged(float oldValue, float newValue)
    {
        _t.position = new Vector3(_t.position.x, _t.position.y, newValue);
    }

    #endregion
}