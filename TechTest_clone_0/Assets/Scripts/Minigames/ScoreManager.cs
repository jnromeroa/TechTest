using Mirror;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages and synchronizes player scores across the network.
/// </summary>
public class ScoreManager : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnPlayer1ScoreChanged))]
    private int _player1Score;

    [SyncVar(hook = nameof(OnPlayer2ScoreChanged))]
    private int _player2Score;


    [Tooltip("Called when Player 1's score changes.")]
    public UnityEvent<int> OnPlayer1ScoreChangedCallback;

    [Tooltip("Called when Player 2's score changes.")]
    public UnityEvent<int> OnPlayer2ScoreChangedCallback;


    /// <summary>
    /// Current score for Player 1.
    /// </summary>
    public int Player1Score => _player1Score;

    /// <summary>
    /// Current score for Player 2.
    /// </summary>
    public int Player2Score => _player2Score;

    /// <summary>
    /// Increases the score of the specified player.
    /// </summary>
    /// <param name="player">The player to score a point for.</param>
    public void AddPointToPlayer(GameManager.PlayerNumber player)
    {
        switch (player)
        {
            case GameManager.PlayerNumber.PLAYER1:
                _player1Score++;
                break;
            case GameManager.PlayerNumber.PLAYER2:
                _player2Score++;
                break;
        }
    }

    /// <summary>
    /// Called when Player 1's score changes.
    /// </summary>
    private void OnPlayer1ScoreChanged(int oldVal, int newVal)
    {
        OnPlayer1ScoreChangedCallback?.Invoke(newVal);
    }

    /// <summary>
    /// Called when Player 2's score changes.
    /// </summary>
    private void OnPlayer2ScoreChanged(int oldVal, int newVal)
    {
        OnPlayer2ScoreChangedCallback?.Invoke(newVal);
    }

}

