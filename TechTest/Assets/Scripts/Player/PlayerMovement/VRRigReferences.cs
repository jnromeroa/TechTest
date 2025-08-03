using UnityEngine;

public class VRRigReferences : MonoBehaviour
{
    public static VRRigReferences Instance { get; private set; }
    public Transform root;
    public Transform head;
    public Transform rightHand;
    public Transform leftHand;
    private void Awake()
    {
        Instance = this;
    }
}
