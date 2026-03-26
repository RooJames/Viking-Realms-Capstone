using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID;
    private Vector3 inventoryScale = new Vector3(0.55f, 0.6707f, 0.6707f);
    private Vector3 originalWorldScale;

    void OnTransformParentChanged()
    {
        // Only center if parent is a Slot
        if (GetComponentInParent<Slot>() != null)
        {
            CenterItem();
        }
    }

    void Start()
    {
        // Save the world scale that was set in the prefab
        originalWorldScale = transform.localScale;

        // Check if this item is in a slot or in the world
        if (GetComponentInParent<Slot>() != null)
        {
            CenterItem();
        }
    }

    private void CenterItem()
    {
        RectTransform rect = GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchoredPosition = Vector2.zero;
            rect.localScale = inventoryScale;
            rect.localRotation = Quaternion.identity;
        }
    }
}