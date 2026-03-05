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
        Vector2 newPos = player.transform.position;

        switch (direction)
        {
            case Direction.Up:
                newPos.y += 5;
                break;
            case Direction.Down:
                newPos.y -= 5;
                break;
            case Direction.Left:
                newPos.x -= 5;
                break;
            case Direction.Right:
                newPos.x += 5;
                break;
        }

        player.transform.position = newPos;
    }
}