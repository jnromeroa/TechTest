using UnityEngine;
using DG.Tweening;

/// <summary>
/// Handles the visual animation of a button being pressed or released.
/// </summary>
public class ButtonAnimator : MonoBehaviour
{
    [SerializeField] private Transform _presserTransform;
    [SerializeField] private float _pressedYPosition;

    private float _originalYPosition;

    private void Awake()
    {
        _originalYPosition = _presserTransform.localPosition.y;
    }

    /// <summary>
    /// Animates the button to the pressed position.
    /// </summary>
    public void Press()
    {
        _presserTransform.DOLocalMoveY(_pressedYPosition, 1f).SetEase(Ease.OutElastic);
    }

    /// <summary>
    /// Animates the button back to its original position.
    /// </summary>
    public void Release()
    {
        _presserTransform.DOLocalMoveY(_originalYPosition, 1f).SetEase(Ease.InElastic);
    }

}
