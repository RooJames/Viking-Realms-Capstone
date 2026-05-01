using UnityEngine;

public class HouseExit : MonoBehaviour
{
    [SerializeField] private Transform outsideSpawnPoint;
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        other.transform.position = outsideSpawnPoint.position;
    }
}