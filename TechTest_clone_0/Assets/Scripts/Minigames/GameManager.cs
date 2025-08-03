using Mirror;
using System.Collections;
using UnityEngine;

/// <summary>
/// Coordinates gameplay flow, including player scoring and ball respawning.
/// </summary>
public class GameManager : NetworkBehaviour
{
    /// <summary>
    /// Identifies a specific player.
    /// </summary>
    public enum PlayerNumber { NONE, PLAYER1, PLAYER2 }

    [Header("Spawn Points")]
    [SerializeField] private Transform _ballSpawnPointP1;

    [SerializeField] private Transform _ballSpawnPointP2;

    [Header("Dependencies")]
    [Tooltip("Handles all ball-related actions.")]
    [SerializeField] private BallController _ballController;

    [Tooltip("Handles all scoring logic.")]
    [SerializeField] private ScoreManager _scoreManager;

    /// <summary>
    /// Adds a point to the selected player and respawns the ball at the opposing player's side.
    /// </summary>
    /// <param name="player">The player who scored.</param>
    public void AddPointToPlayer(int player)
    {
        if (!isServer) return;

        switch (player)
        {
            case (int)PlayerNumber.PLAYER1:
                _scoreManager.AddPointToPlayer(PlayerNumber.PLAYER1);
                StartCoroutine(RespawnBallCoroutine(_ballSpawnPointP2.position));
                break;

            case (int)PlayerNumber.PLAYER2:
                _scoreManager.AddPointToPlayer(PlayerNumber.PLAYER2);
                StartCoroutine(RespawnBallCoroutine(_ballSpawnPointP1.position));
                break;
        }
    }

    /// <summary>
    /// Temporarily disables the ball and respawns it after a delay.
    /// </summary>
    /// <param name="position">The position where the ball should respawn.</param>
    private IEnumerator RespawnBallCoroutine(Vector3 position)
    {
        _ballController.SetBallActive(false);
        yield return new WaitForSeconds(2f);
        _ballController.Respawn(position);
    }

}
