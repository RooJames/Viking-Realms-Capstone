using UnityEngine;

public class YSortOffset : MonoBehaviour
{
    [SerializeField] private int sortingOffset = -10; // Negative = behind, Positive = in front
    [SerializeField] private Transform sortingReference; // Reference object for Y position (e.g., campfire)
    private ParticleSystemRenderer particleRenderer;
    
    void Awake()
    {
        particleRenderer = GetComponent<ParticleSystemRenderer>();
        
        // If no reference set, use parent or self
        if (sortingReference == null)
        {
            sortingReference = transform.parent != null ? transform.parent : transform;
        }
    }
    
    void LateUpdate()
    {
        // Set sorting order based on reference Y position with offset
        float yPos = sortingReference != null ? sortingReference.position.y : transform.position.y;
        int baseSortingOrder = Mathf.RoundToInt(-yPos * 100);
        particleRenderer.sortingOrder = baseSortingOrder + sortingOffset;
    }
}
