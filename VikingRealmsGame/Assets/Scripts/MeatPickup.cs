using UnityEngine;

// Attach this to the Meat prefab.
// The player walks over it to auto-collect (no button press needed).
// If you want a press-to-pickup version, implement IInteractable instead.
[RequireComponent(typeof(Collider2D))]
public class MeatPickup : MonoBehaviour
{
    [Header("Settings")]
    public string itemName = "Raw Meat";
    public int quantity = 1;

    void Awake()
    {
        // Ensure the collider is a trigger so it doesn't block movement
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // TODO: Add the meat to the player's inventory here.
        // Example (swap for your real inventory call):
        //   other.GetComponent<Inventory>().AddItem(itemName, quantity);
        Debug.Log($"Player picked up {quantity}x {itemName}");

        Destroy(gameObject);
    }
}
