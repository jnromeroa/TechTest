using UnityEngine;
/// <summary>
///Handles the reset of the position to a spawn position
/// </summary>
public class PlayerResetter : MonoBehaviour
{
    [SerializeField] private Transform _spawnPosition;

    public void ResetPosition()
    {
        transform.SetPositionAndRotation(_spawnPosition.position, _spawnPosition.rotation);
    }
}
