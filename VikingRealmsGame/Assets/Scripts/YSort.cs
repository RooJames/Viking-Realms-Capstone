using UnityEngine;

public class YSort : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void LateUpdate()
    {
        // Set sorting order based on Y position
        // Objects lower on screen (lower Y) appear in front
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}
