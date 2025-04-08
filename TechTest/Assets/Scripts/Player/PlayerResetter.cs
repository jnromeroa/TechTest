using UnityEngine;

public class PlayerResetter : MonoBehaviour
{
    [SerializeField] private Transform _spawnPosition;

    public void ResetPosition()
    {
        transform.SetPositionAndRotation(_spawnPosition.position, _spawnPosition.rotation);
    }
}
