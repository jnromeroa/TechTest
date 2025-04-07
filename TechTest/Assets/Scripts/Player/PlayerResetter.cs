using UnityEngine;

public class PlayerResetter : MonoBehaviour
{
    [SerializeField] private Transform spawnPosition;

    public void ResetPosition()
    {
        transform.SetPositionAndRotation(spawnPosition.position, spawnPosition.rotation);
    }
}
