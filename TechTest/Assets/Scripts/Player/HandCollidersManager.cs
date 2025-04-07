using Mirror;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class HandCollidersManager : NetworkBehaviour
{
    [SerializeField] private VRBodyPart _bodyPart = VRBodyPart.NONE;
    [SerializeField] private float _delaySeconds = 0.5f;
    [SyncVar(hook = nameof(OnEnableCollidersChanged))]
    bool areCollidersEnabled = true;
    Collider[] handColliders;

    private void Awake()
    {
        handColliders = GetComponentsInChildren<Collider>();
    }
    public override void OnStartAuthority()
    {
        base.OnStartServer();
        switch (_bodyPart)
        {
            case VRBodyPart.RIGHT:
                XRDirectInteractor rightInteractor = VRRigReferences.Instance.rightHand.parent.GetComponent<XRDirectInteractor>();
                rightInteractor.selectEntered.AddListener((args) => Disable());
                rightInteractor.selectExited.AddListener((args) => EnableDelay(_delaySeconds));
                break;
            case VRBodyPart.LEFT:
                XRDirectInteractor leftInteractor = VRRigReferences.Instance.leftHand.parent.GetComponent<XRDirectInteractor>();
                leftInteractor.selectEntered.AddListener((args) => Disable());
                leftInteractor.selectExited.AddListener((args) => EnableDelay(_delaySeconds));
                break;
            default:
                break;
        }
    }

    private void OnEnableCollidersChanged(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            EnableColliders();
            return;
        }
        DisableColliders();
    }

    public void Enable()
    {
        EnableCmd();
        EnableColliders();
    }
    [Command]
    private void EnableCmd()
    {
        areCollidersEnabled = true;
        EnableColliders();
    }
    
    private void EnableColliders()
    {
        foreach (Collider collider in handColliders)
        {
            collider.enabled = true;
        }
    }

    public void EnableDelay(float time)
    {
        Invoke(nameof(Enable), time);
    }

    public void Disable()
    {
        DisableCmd();
        DisableColliders();
    }
    [Command]
    void DisableCmd()
    {
        areCollidersEnabled = false;
        DisableColliders();
    }
    void DisableColliders()
    {
        foreach (Collider collider in handColliders)
        {
            collider.enabled = false;
        }
    }
}

