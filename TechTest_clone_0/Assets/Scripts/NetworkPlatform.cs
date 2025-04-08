using Mirror;
using UnityEngine;
using DG.Tweening;

public class NetworkPlatform : NetworkBehaviour
{

    [SerializeField] private float _targetZPosition;
    [SerializeField] private float _moveDurationSeconds = 4f;
    [SerializeField] private float _waitDurationSeconds = 2;
    [SyncVar(hook = nameof(OnZPosChanged))]
    private float _zPosition;
    [SyncVar]
    private bool _isMoving;
    private float _originalZPosition;
    private Transform _t;

    private void Awake()
    {
        _t = transform;
        _originalZPosition = _t.position.z;
    }

    #region Server
    public override void OnStartServer()
    {
        base.OnStartServer();
        _zPosition = _t.position.z;

    }
    [Command(requiresAuthority = false)]
    public void ActivateCmd()
    {
        if (_isMoving) return;
        _isMoving = true;
        _t.DOMoveZ(_targetZPosition, _moveDurationSeconds).SetEase(Ease.InOutCubic).OnComplete(() =>
        {
            _isMoving = false;
            Invoke(nameof(Deactivate), _waitDurationSeconds);
        }
        );
    }

    [ServerCallback]
    private void Update()
    {
        if (!_isMoving) return;
        _zPosition = _t.position.z;
    } 
    #endregion

    #region Client
    [ContextMenu("Activate")]
    public void Activate()
    {
        ActivateCmd();
    }
    public void Deactivate()
    {
        _isMoving = true;
        _t.DOMoveZ(_originalZPosition, _moveDurationSeconds).SetEase(Ease.InOutCubic).OnComplete(() => _isMoving = false);
    } 
    #endregion


    #region SyncVar Callbacks
    private void OnZPosChanged(float oldValue, float newValue)
    {
        _t.position = new Vector3(_t.position.x, _t.position.y, newValue);
    } 
    #endregion


}
