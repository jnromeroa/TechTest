using UnityEngine;
using Mirror;
using Mirror.Discovery;
using System.Collections;

/// <summary>
/// Custom NetworkManager that handles automatic server discovery and connection.
/// Can start as a client or become host based on discovery results.
/// </summary>
[RequireComponent(typeof(NetworkDiscovery))]
public class MyNetworkManager : NetworkManager
{
    [Header("Discovery Settings")]
    [Tooltip("Maximum wait time for server discovery in seconds.")]
    [SerializeField] private int _serverDiscoveryWaitTime = 5;

    [Tooltip("Use random time within wait time range before becoming host.")]
    [SerializeField] private bool _useRandomSeconds = false;

    [Tooltip("Automatically attempt to join or host on start.")]
    [SerializeField] private bool _autoStart = true;

    private NetworkDiscovery _networkDiscovery;
    private ServerResponse? _response = null;

    #region Unity Lifecycle

    /// <summary>
    /// Unity Awake callback. Initializes NetworkDiscovery reference.
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        _networkDiscovery = GetComponent<NetworkDiscovery>();
    }

    /// <summary>
    /// Unity OnEnable callback. Subscribes to server found event.
    /// </summary>
    private void OnEnable()
    {
        if (_networkDiscovery != null)
            _networkDiscovery.OnServerFound.AddListener(OnServerFound);
    }

    /// <summary>
    /// Unity OnDisable callback. Unsubscribes from server found event.
    /// </summary>
    private void OnDisable()
    {
        if (_networkDiscovery != null)
            _networkDiscovery.OnServerFound.RemoveListener(OnServerFound);
    }

    /// <summary>
    /// Unity Start callback. Begins auto start process if enabled.
    /// </summary>
    public override void Start()
    {
        base.Start();
        if (_autoStart)
        {
            StartCoroutine(TryConnectAsClientOrHost());
        }
    }

    #endregion

    #region Client Code

    /// <summary>
    /// Attempts to discover a server and connect as client. Becomes host if no server found.
    /// </summary>
    /// <returns>Coroutine for discovery wait loop.</returns>
    public IEnumerator TryConnectAsClientOrHost()
    {
        int waitTime = _useRandomSeconds ? Random.Range(0, _serverDiscoveryWaitTime) : _serverDiscoveryWaitTime;
        WaitForSeconds wait = new WaitForSeconds(1f);

        _networkDiscovery.StartDiscovery();

        for (int i = 0; i < waitTime; i++)
        {
            if (_response != null)
            {
                _networkDiscovery.StopDiscovery();
                StartClient(_response.Value.uri);
                yield break;
            }
            yield return wait;
        }

        _networkDiscovery.StopDiscovery();
        StartAsHost();
    }

    /// <summary>
    /// Callback when a server is found. Stores its response.
    /// </summary>
    /// <param name="serverResponse">Details of the discovered server.</param>
    private void OnServerFound(ServerResponse serverResponse)
    {
        _response = serverResponse;
        Debug.Log($"Server Found at {serverResponse.uri}");
    }

    #endregion

    #region Server Code

    /// <summary>
    /// Starts the game as host and advertises over LAN.
    /// </summary>
    public void StartAsHost()
    {
        StartHost();
        Debug.Log("Host Started");
        _networkDiscovery.AdvertiseServer();
    }

    #endregion
}
