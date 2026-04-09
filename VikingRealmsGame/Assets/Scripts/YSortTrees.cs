using UnityEngine;

public class YSortTrees : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        // Set sorting order based on Y position
        // Objects higher on screen (higher Y) appear in front
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * 100);
    }
}