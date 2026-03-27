using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemButtonController : MonoBehaviour
{
    public int ID;
    private Animator anim;
    public TextMeshProUGUI itemText;
    public Image selectedItem;
    private bool selected = false;
    public Sprite icon;
    
    [Header("Auto-Sync from Inventory")]
    public Sprite emptySprite; // Sprite to show when no item in this slot
    
    private GameObject currentItemObject; // The instantiated item GameObject (child of this button)
    private Item currentItem; // Reference to the Item component

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (selected)
        {
            selectedItem.sprite = icon;
            selectedItem.preserveAspect = true; // Maintain proper aspect ratio
            
            // Match inventory scale exactly
            RectTransform selectedRect = selectedItem.GetComponent<RectTransform>();
            if (selectedRect != null)
            {
                selectedRect.localScale = new Vector3(0.55f, 0.6707f, 0.6707f);
            }
            
            if (itemText != null && currentItem != null)
                itemText.text = currentItem.itemName;
        }
    }

    public void Selected()
    {
        selected = true;
        ItemWheelController.weaponID = ID;
    }

    public void Deselected()
    {
        selected = false;
        ItemWheelController.weaponID = 0;
    }

    public void HoverEnter()
    {
        if (anim != null)
            anim.SetBool("Hover", true);
        if (itemText != null)
        {
            if (currentItem != null)
                itemText.text = currentItem.itemName;
            else
                itemText.text = "Empty";
        }
    }

    public void HoverExit()
    {
        if (anim != null)
            anim.SetBool("Hover", false);
        if (itemText != null)
            itemText.text = "";
    }
    
    /// <summary>
    /// Update this button's display with an item prefab
    /// </summary>
    public void SetItemPrefab(GameObject itemPrefab)
    {
        // Clear any existing item
        if (currentItemObject != null)
        {
            Destroy(currentItemObject);
            currentItemObject = null;
            currentItem = null;
        }
        
        if (itemPrefab != null)
        {
            // Instantiate the item prefab as a child of this button (same as inventory slots)
            currentItemObject = Instantiate(itemPrefab, transform);
            currentItem = currentItemObject.GetComponent<Item>();
            
            // Set up the RectTransform to display properly
            RectTransform itemRect = currentItemObject.GetComponent<RectTransform>();
            if (itemRect != null)
            {
                itemRect.anchoredPosition = Vector2.zero;
                // Use exact same scale as inventory slots
                itemRect.localScale = new Vector3(0.55f, 0.6707f, 0.6707f);
                
                // Force world rotation to 0 (upright) regardless of parent rotation
                itemRect.rotation = Quaternion.identity;
            }
            
            // Store the sprite for selected item display
            Image itemImage = currentItemObject.GetComponent<Image>();
            if (itemImage != null && itemImage.sprite != null)
            {
                icon = itemImage.sprite;
            }
        }
        else
        {
            // Empty slot - no item to display
            currentItem = null;
            icon = emptySprite;
        }
    }
}
