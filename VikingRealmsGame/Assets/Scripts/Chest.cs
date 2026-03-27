using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public bool IsOpened { get; private set; }
    [SerializeField] private string chestID; // Manually set in Inspector
    public string ChestID => chestID;
    public GameObject itemPrefab;
    public Sprite openedSprite;

    void Start()
    {
        // Only generate if not manually set
        if (string.IsNullOrEmpty(chestID))
        {
            chestID = GlobalHelper.GenerateUniqueID(gameObject);
            Debug.Log($"Generated Chest ID: {chestID}");
        }
    }

    public bool CanInteract()
    {
        return !IsOpened;
    }

    public void Interact()
    {
        if (!CanInteract()) return;
        OpenChest();
    }

    private void OpenChest()
    {
        SetOpened(true);

        if (itemPrefab)
        {
            GameObject item = Instantiate(itemPrefab, transform.position + Vector3.down, itemPrefab.transform.rotation);
        }
    }

    public void SetOpened(bool opened)
    {
        IsOpened = opened;
        if (IsOpened && openedSprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
        }
    }
}