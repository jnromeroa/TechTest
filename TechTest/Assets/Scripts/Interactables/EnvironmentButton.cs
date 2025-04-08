using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class EnvironmentButton : MonoBehaviour
{
    public UnityEvent OnInteract;
    public UnityEvent OnRelease;
    [SerializeField] private Transform _buttonPresserTransform;
    [SerializeField] private float _buttonPressedLocalYPosition;
    [SerializeField] private bool _autorelease = true;
    [SerializeField] private float _autoreleaseTimeSeconds = 10f;
    private float _originalLocalYPosition;
    private bool _isInteractable = true;

    private void Awake()
    {
        _originalLocalYPosition = _buttonPresserTransform.localPosition.y;
    }
    [ContextMenu("Interact")]
    public void Interact()
    {
        if (!_isInteractable) return;
        Press();
        if (!_autorelease) return;
        Invoke(nameof(Release), _autoreleaseTimeSeconds);
    }
    private void Press()
    {
        OnInteract?.Invoke();
        _isInteractable = false;
        AnimatePress();
    }
    private void Release()
    {
        OnRelease?.Invoke();
        _isInteractable = true;
        AnimateRelease();
    }
    private void AnimatePress()
    {
        _buttonPresserTransform.DOLocalMoveY(_buttonPressedLocalYPosition, 1f).SetEase(Ease.OutElastic);
    }

    private void AnimateRelease()
    {
        _buttonPresserTransform.DOLocalMoveY(_originalLocalYPosition, 1f).SetEase(Ease.InElastic);
    }
}
