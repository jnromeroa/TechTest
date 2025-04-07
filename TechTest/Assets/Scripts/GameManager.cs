using Mirror;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : NetworkBehaviour
{
    public enum PlayerNumber
    {
        NONE,
        PLAYER1,
        PLAYER2
    }
    [SerializeField] private Transform _ballSpawnPointP1;
    [SerializeField] private Transform _ballSpawnPointP2;
    [SerializeField] private Transform _ball;
    [SyncVar(hook = nameof(OnActiveBallChanged))]
    private bool activeBall;
    [SyncVar(hook =nameof(OnPlayer1ScoreChanged))]
    private int _player1Score;
    [SyncVar(hook = nameof(OnPlayer2ScoreChanged))]
    private int _player2Score;
    public UnityEvent<int> OnPlayer1ScoreChangedCallback;
    public UnityEvent<int> OnPlayer2ScoreChangedCallback;
    public int Player1Score => _player1Score;
    public int Player2Score => _player2Score;
    
    public void AddPointToPlayer(int playerNumber)
    {
        if (!isServer) return;
        switch (playerNumber)
        {
            case (int)PlayerNumber.PLAYER1:
                _player1Score++;
                StartCoroutine(RespawnBallCoroutine(_ballSpawnPointP2.position));
                break;
            case (int)PlayerNumber.PLAYER2:
                _player2Score++;
                StartCoroutine(RespawnBallCoroutine(_ballSpawnPointP1.position));
                break;
            default:
                break;
        }
    }
    private IEnumerator RespawnBallCoroutine(Vector3 position)
    {
        SetBallActive(false);
        yield return new WaitForSeconds(2f);
        RespawnBall(position);
    }
    public void SetBallActive(bool value)
    {
        if (!isServer) return;
        _ball.gameObject.SetActive(value);
        _ball.GetComponent<Rigidbody>().Sleep();
        activeBall = value;
    }
    private void RespawnBall(Vector3 position)
    {
        _ball.position = position;
        SetBallActive(true);
    }
    private void OnPlayer1ScoreChanged(int oldValue, int newValue)
    {
        OnPlayer1ScoreChangedCallback?.Invoke(newValue);
    }
    private void OnPlayer2ScoreChanged(int oldValue, int newValue)
    {
        OnPlayer2ScoreChangedCallback?.Invoke(newValue);
    }

    private void OnActiveBallChanged(bool oldValue, bool newValue)
    {
        _ball.gameObject.SetActive(newValue);
    }

}
