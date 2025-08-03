using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Represents an interactable environment button with optional auto-release and animation.
/// </summary>
public class InteractableButton : MonoBehaviour, IInteractable
{
   


    [Header("Config")]
    [Tooltip("Should the button auto-release after a delay?")]
    [SerializeField] private bool _autoRelease = true;

    [Tooltip("Delay time (in seconds) before auto-releasing.")]
    [SerializeField] private float _autoReleaseTime = 10f;

    [Header("References")]
    [Tooltip("Handles the button's animation.")]
    [SerializeField] private ButtonAnimator _buttonAnimator;

    [Tooltip("Handles auto-release timing logic.")]
    [SerializeField] private AutoreleaseTimer _autoReleaseTimer;

    [Header("Events")]
    [Tooltip("Called when the button is pressed.")]
    public UnityEvent OnInteract;

    [Tooltip("Called when the button is released.")]
    public UnityEvent OnRelease;

    private bool _isInteractable = true;


    /// <summary>
    /// Called by external systems to interact with this button.
    /// </summary>
    public void Interact()
    {
        if (!_isInteractable) return;

        Press();

        if (_autoRelease)
            _autoReleaseTimer.StartTimer(_autoReleaseTime, Release);
    }


    /// <summary>
    /// Handles internal logic when the button is pressed.
    /// </summary>
    private void Press()
    {
        _isInteractable = false;
        _buttonAnimator.Press();
        OnInteract?.Invoke();
    }

    /// <summary>
    /// Handles internal logic when the button is released.
    /// </summary>
    private void Release()
    {
        _isInteractable = true;
        _buttonAnimator.Release();
        OnRelease?.Invoke();
    }

}
