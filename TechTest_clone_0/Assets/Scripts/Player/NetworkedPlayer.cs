using UnityEngine;
using Mirror;
public class NetworkedPlayer : NetworkBehaviour
{
    [SerializeField] private GameObject _head;
    private static NetworkedPlayer _local;
    public static NetworkedPlayer Local => _local;
    private bool _isPlaying;
    public bool IsPlaying => _isPlaying;

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
