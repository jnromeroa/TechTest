using Mirror;
using UnityEngine;

/// <summary>
/// Controls the visibility, activation, and position of the ball object.
/// </summary>
public class BallController : NetworkBehaviour
{
    [SerializeField] private Transform _ball;

    [SyncVar(hook = nameof(OnBallActiveChanged))]
    private bool _active;

    /// <summary>
    /// Activates or deactivates the ball.
    /// </summary>
    /// <param name="active">Whether the ball should be active.</param>
    public void SetBallActive(bool active)
    {
        if (!isServer) return;

        _ball.gameObject.SetActive(active);
        _ball.GetComponent<Rigidbody>().Sleep();
        _active = active;
    }

    /// <summary>
    /// Moves the ball to a new position and reactivates it.
    /// </summary>
    /// <param name="position">The position to respawn the ball at.</param>
    public void Respawn(Vector3 position)
    {
        _ball.position = position;
        SetBallActive(true);
    }

    /// <summary>
    /// Mirror SyncVar hook to update ball visibility on clients.
    /// </summary>
    private void OnBallActiveChanged(bool oldVal, bool newVal)
    {
        _ball.gameObject.SetActive(newVal);
    }

}