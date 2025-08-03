using System;
using UnityEngine;

/// <summary>
/// Optional utility to delay a callback invocation by a specified amount of time.
/// </summary>
public class AutoreleaseTimer : MonoBehaviour
{
    #region Private Fields

    private float _delay;
    private Action _callback;
    private bool _enabled;

    #endregion

    #region Public Methods

    /// <summary>
    /// Starts a timer that will invoke the callback after a delay.
    /// </summary>
    /// <param name="delay">Time in seconds before the callback is triggered.</param>
    /// <param name="callback">The action to invoke after the delay.</param>
    public void StartTimer(float delay, Action callback)
    {
        _delay = delay;
        _callback = callback;
        _enabled = true;
        CancelInvoke(nameof(Trigger));
        Invoke(nameof(Trigger), _delay);
    }

    /// <summary>
    /// Cancels the timer if it is running.
    /// </summary>
    public void Cancel()
    {
        CancelInvoke(nameof(Trigger));
        _enabled = false;
    }

    #endregion

    #region Private Methods

    private void Trigger()
    {
        if (_enabled)
        {
            _callback?.Invoke();
            _enabled = false;
        }
    }

    #endregion
}
