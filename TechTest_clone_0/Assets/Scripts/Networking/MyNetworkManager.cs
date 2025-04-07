using UnityEngine;
using Mirror;
using Mirror.Discovery;
using TMPro;
using System.Collections;

public class MyNetworkManager : NetworkManager
{
    [SerializeField] private int _serverDiscoveryWaitTime = 5;
    [SerializeField] private bool _useRandomSeconds;
    [SerializeField] private bool _autoStart = true;
    NetworkDiscovery _networkDiscovery;
    ServerResponse? _response = null;
    public override void Awake()
    {
        base.Awake();
        _networkDiscovery = GetComponent<NetworkDiscovery>();
    }
    private void OnEnable()
    {
        if (_networkDiscovery == null) return;
        _networkDiscovery.OnServerFound.AddListener(ServerFound);
    }
    private void OnDisable()
    {
        if (_networkDiscovery == null) return;
        _networkDiscovery.OnServerFound.RemoveListener(ServerFound);
    }

    public override void Start()
    {
        if (!_autoStart) return;
        StartCoroutine(ActiveClient());
    }
    public void ActiveHost()
    {
        StartHost();
        _networkDiscovery.AdvertiseServer();

    }
    public IEnumerator ActiveClient()
    {
        int randomSeconds = Random.Range(0, _serverDiscoveryWaitTime);
        WaitForSeconds wfs = new WaitForSeconds(1f);
        _networkDiscovery.StartDiscovery();
        for (int i = 0; 
            i < (_useRandomSeconds ? randomSeconds : _serverDiscoveryWaitTime); 
            i++)
        {
            if (_response != null)
            {
                _networkDiscovery.StopDiscovery();
                StartClient(_response.Value.uri);
                yield break;
            }
            yield return wfs;
        }
        _networkDiscovery.StopDiscovery();
        ActiveHost();
    }

    void ServerFound(ServerResponse serverResponse)
    {
        _response = serverResponse;
        Debug.Log($"Server Found at {serverResponse.uri}");
    }




}
