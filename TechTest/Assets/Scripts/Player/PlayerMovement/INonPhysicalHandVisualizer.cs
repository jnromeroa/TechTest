using UnityEngine;
/// <summary>
/// Manages visibility behavior of a non-physical VR hand.
/// </summary>
public interface INonPhysicalHandVisualizer
{
    bool ShouldVisualize();
    void Initialize(Renderer renderer);
    void UpdateVisibility(Vector3 currentPos, Vector3 targetPos);
}