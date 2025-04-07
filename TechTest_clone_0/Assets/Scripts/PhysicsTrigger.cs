using UnityEngine;
using UnityEngine.Events;

public class PhysicsTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask _layerToDetect;
    [SerializeField] private bool _useOnCollition;
    public UnityEvent OnTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (_useOnCollition) return;
        if (((1 << other.gameObject.layer) & _layerToDetect) == 0) return;
        Debug.Log("Button Triggered");
        OnTrigger?.Invoke();

    }

    private void OnCollisionEnter(Collision other)
    {
        if (!_useOnCollition) return;
        if (((1 << other.gameObject.layer) & _layerToDetect) == 0) return;
        Debug.Log("Button Touched");
        OnTrigger?.Invoke();
    }
}
