using Unity.Cinemachine;
using UnityEngine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] BoxCollider2D mapBoundary;
    CinemachineConfiner2D confiner;

    [SerializeField] Direction direction;
    enum Direction { Up, Down, Left, Right }

    private void Awake()

    {
        confiner = Object.FindAnyObjectByType<CinemachineConfiner2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            confiner.BoundingShape2D = mapBoundary;
            UpdatePlayerPosition(collision.gameObject);
        }
    }

    private void UpdatePlayerPosition(GameObject player)
    {
        Bounds b = mapBoundary.bounds;
        const float margin = 2f; // how far inside the boundary edge to place the player
        Vector2 newPos = player.transform.position;

        switch (direction)
        {
            case Direction.Up:    newPos.y = b.min.y + margin; break;
            case Direction.Down:  newPos.y = b.max.y - margin; break;
            case Direction.Left:  newPos.x = b.max.x - margin; break;
            case Direction.Right: newPos.x = b.min.x + margin; break;
        }

        player.transform.position = newPos;

        // Kill any carried momentum so the player doesn't overshoot after the teleport
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }
}