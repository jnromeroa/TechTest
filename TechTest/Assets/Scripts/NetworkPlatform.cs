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
    private bool isMoving;
    private float _originalZPosition;
    private Transform _t;

    private void Awake()
    {
        _t = transform;
        _originalZPosition = _t.position.z;
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
        _zPosition = _t.position.z;

    }

    [ContextMenu("Activate")]
    public void Activate()
    {
        ActivateCmd();
    }
    [Command(requiresAuthority = false)]
    public void ActivateCmd()
    {
        if (isMoving) return;
        isMoving = true;
        _t.DOMoveZ(_targetZPosition, _moveDurationSeconds).SetEase(Ease.InOutCubic).OnComplete(() =>
        {
            isMoving = false;
            Invoke(nameof(Deactivate), _waitDurationSeconds);
            }
        );
    }

    public void Deactivate()
    {
        isMoving = true;
        _t.DOMoveZ(_originalZPosition, _moveDurationSeconds).SetEase(Ease.InOutCubic).OnComplete(()=>isMoving = false);
    }
    private void Update()
    {
        if (!isMoving) return;
        _zPosition = _t.position.z;
    }


    private void OnZPosChanged(float oldValue, float newValue)
    {
        _t.position = new Vector3(_t.position.x, _t.position.y, newValue);
    }

    
}
