using UnityEngine;
using Mirror;
using Mirror.Discovery;
using TMPro;
using System.Collections;

public class MyNetworkManager : NetworkManager
{
    [SerializeField] int serverDiscoveryWaitTime = 5;
    [SerializeField] bool useRandomSeconds;
    NetworkDiscovery networkDiscovery;
    ServerResponse? response = null;
    public override void Awake()
    {
        base.Awake();
        networkDiscovery = GetComponent<NetworkDiscovery>();
    }
    private void OnEnable()
    {
        if (networkDiscovery == null) return;
        networkDiscovery.OnServerFound.AddListener(ServerFound);
    }
    private void OnDisable()
    {
        if (networkDiscovery == null) return;
        networkDiscovery.OnServerFound.RemoveListener(ServerFound);
    }

    public override void Start()
    {
        StartCoroutine(ActiveClient());
    }
    public void ActiveHost()
    {
        StartHost();
        networkDiscovery.AdvertiseServer();

    }
    public IEnumerator ActiveClient()
    {
        int randomSeconds = Random.Range(0, serverDiscoveryWaitTime);
        WaitForSeconds wfs = new WaitForSeconds(1f);
        networkDiscovery.StartDiscovery();
        for (int i = 0; 
            i < (useRandomSeconds ? randomSeconds : serverDiscoveryWaitTime); 
            i++)
        {
            if (response != null)
            {
                networkDiscovery.StopDiscovery();
                StartClient(response.Value.uri);
                yield break;
            }
            yield return wfs;
        }
        networkDiscovery.StopDiscovery();
        ActiveHost();
    }

    void ServerFound(ServerResponse serverResponse)
    {
        response = serverResponse;
        Debug.Log($"Server Found at {serverResponse.uri}");
    }




}
