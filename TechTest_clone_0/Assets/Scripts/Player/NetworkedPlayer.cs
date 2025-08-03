using UnityEngine;
using Mirror;

/// <summary>
/// Handles player-specific behavior in a networked multiplayer environment.
/// Manages head visibility and local player tracking.
/// </summary>
public class NetworkedPlayer : NetworkBehaviour
{
    /// <summary>
    /// Reference to the player's head GameObject.
    /// This will be hidden for the local player.
    /// </summary>
    [SerializeField] private GameObject _head;

    /// <summary>
    /// Singleton reference to the local player.
    /// </summary>
    private static NetworkedPlayer _local;

    /// <summary>
    /// Gets the local player instance.
    /// </summary>
    public static NetworkedPlayer Local => _local;

    /// <summary>
    /// Whether the player is currently playing.
    /// </summary>
    private bool _isPlaying;

    /// <summary>
    /// Gets whether the player is actively playing the game.
    /// </summary>
    public bool IsPlaying => _isPlaying;

    /// <summary>
    /// Called when the object is started on the client.
    /// Handles visibility of the head object and sets the local player reference.
    /// </summary>
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (isOwned)
        {
            _head.SetActive(false);
            _local = this;
        }
        else
        {
            _head.SetActive(true);
        }
    }
}
