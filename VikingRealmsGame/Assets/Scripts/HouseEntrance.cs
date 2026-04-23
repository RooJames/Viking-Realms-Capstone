using UnityEngine;

public class HouseEntrance : MonoBehaviour
{
    [SerializeField] private Transform interiorSpawnPoint;
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        other.transform.position = interiorSpawnPoint.position;
    }
}