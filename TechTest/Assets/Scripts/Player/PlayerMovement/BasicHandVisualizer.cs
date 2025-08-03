using UnityEngine;

public class BasicHandVisualizer : MonoBehaviour, INonPhysicalHandVisualizer
{
    [SerializeField] private float showDistance = 0.05f;
    private Renderer _handRenderer;
    private bool _isVisible = false;

    public void Initialize(Renderer renderer)
    {
        _handRenderer = renderer;
        _handRenderer.enabled = false;
    }

    public bool ShouldVisualize() => _handRenderer != null;

    public void UpdateVisibility(Vector3 current, Vector3 target)
    {
        float dist = Vector3.Distance(current, target);
        bool shouldShow = dist > showDistance;

        if (shouldShow != _isVisible)
        {
            _handRenderer.enabled = shouldShow;
            _isVisible = shouldShow;
        }
    }
}
