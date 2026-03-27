using UnityEngine;

public class WorldItem : MonoBehaviour, IInteractable
{
    [SerializeField] private string worldItemID;
    public string WorldItemID => worldItemID;
    private Item item;

    void Start()
    {
        if (string.IsNullOrEmpty(worldItemID))
        {
            worldItemID = GlobalHelper.GenerateUniqueID(gameObject);
        }

        item = GetComponent<Item>();
    }

    public bool CanInteract()
    {
        return true; // Can always pick up items
    }

    public void Interact()
    {
        PickUpItem();
    }

    private void PickUpItem()
    {
        if (item != null)
        {
            InventoryController inventoryController = FindObjectOfType<InventoryController>();
            ItemDictionary itemDictionary = FindObjectOfType<ItemDictionary>();

            if (inventoryController != null && itemDictionary != null)
            {
                GameObject itemPrefab = itemDictionary.GetItemPrefab(item.ID);
                if (itemPrefab != null && inventoryController.AddItem(itemPrefab))
                {
                    // Successfully added to inventory
                    SaveController saveController = FindObjectOfType<SaveController>();
                    if (saveController != null)
                    {
                        saveController.RegisterCollectedItem(worldItemID);
                    }
                    Destroy(gameObject);
                }
            }
        }
    }

    public void MarkAsCollected()
    {
        Destroy(gameObject);
    }
}