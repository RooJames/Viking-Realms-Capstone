using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemWheelController : MonoBehaviour
{
    public Animator anim;
    private bool weaponWheelSelected = false;

    public GameObject itemWheel;
    public Image selectedItem;
    public Sprite noImage;

    public static int weaponID;
    public TextMeshProUGUI itemText;
    
    [Header("Inventory Integration")]
    public InventoryController inventoryController;
    public ItemDictionary itemDictionary;
    
    [Header("Wheel Buttons")]
    [Tooltip("Parent containing the 8 ItemButtonController components")]
    public Transform wheelButtonsParent;
    
    private ItemButtonController[] wheelButtons;
    private GameObject[] assignedItemPrefabs = new GameObject[8]; // Prefabs to instantiate
    private Slot[] assignedSlots = new Slot[8]; // Track which slots are assigned
    private int nextAvailableWheelSlot = 0; // Next wheel slot to fill

    void Start()
    {
        itemWheel.SetActive(false);
        
        if (inventoryController == null)
            inventoryController = FindAnyObjectByType<InventoryController>();
        
        if (itemDictionary == null)
            itemDictionary = FindAnyObjectByType<ItemDictionary>();
        
        // Auto-discover wheel button controllers
        if (wheelButtonsParent != null)
        {
            List<ItemButtonController> buttons = new List<ItemButtonController>();
            foreach (Transform child in wheelButtonsParent)
            {
                ItemButtonController btn = child.GetComponent<ItemButtonController>();
                if (btn != null)
                    buttons.Add(btn);
            }
            wheelButtons = buttons.ToArray();
            Debug.Log($"ItemWheelController: Found {wheelButtons.Length} wheel buttons");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            weaponWheelSelected = !weaponWheelSelected;
            itemWheel.SetActive(weaponWheelSelected);
        }

        if (weaponWheelSelected)
        {
            Time.timeScale = 0f; // pause game
            anim.SetBool("OpenWeaponWheel", true);
            itemText.gameObject.SetActive(true);
            
            // Update wheel display with manually assigned items
            UpdateWheelDisplay();
        }
        else
        {
            Time.timeScale = 1f; // resume game
            anim.SetBool("OpenWeaponWheel", false);
            itemText.gameObject.SetActive(false);
        }

        // Display selected item info from inventory
        if (weaponID > 0)
        {
            Item selectedItemData = GetItemAtWheelSlot(weaponID);
            
            if (selectedItemData != null)
            {
                itemText.text = selectedItemData.itemName;
            }
            else
            {
                itemText.text = "Empty";
            }
        }
        else
        {
            selectedItem.sprite = noImage;
            itemText.text = "";
        }
    }
    
    /// <summary>
    /// Assign an item from inventory to the next available wheel slot
    /// </summary>
    public bool AssignItemToWheel(Item item, Slot sourceSlot)
    {
        if (item == null || nextAvailableWheelSlot >= 8)
            return false;
        
        // Check if this slot is already assigned
        for (int i = 0; i < assignedSlots.Length; i++)
        {
            if (assignedSlots[i] == sourceSlot)
            {
                // Already assigned, do nothing
                return false;
            }
        }
        
        // Get the item prefab from ItemDictionary using the item's ID
        GameObject itemPrefab = null;
        if (itemDictionary != null)
        {
            itemPrefab = itemDictionary.GetItemPrefab(item.ID);
        }
        
        if (itemPrefab == null)
        {
            Debug.LogWarning($"Could not find prefab for item ID {item.ID}");
            return false;
        }
        
        // Assign prefab to next available wheel slot
        assignedItemPrefabs[nextAvailableWheelSlot] = itemPrefab;
        assignedSlots[nextAvailableWheelSlot] = sourceSlot;
        
        Debug.Log($"Assigned {item.itemName} to wheel slot {nextAvailableWheelSlot + 1}");
        
        nextAvailableWheelSlot++;
        
        // Immediately update the wheel button if wheelButtons is initialized
        if (wheelButtons != null && wheelButtons.Length > 0)
        {
            int buttonIndex = nextAvailableWheelSlot - 1;
            if (buttonIndex < wheelButtons.Length)
            {
                wheelButtons[buttonIndex].SetItemPrefab(itemPrefab);
            }
        }
        else
        {
            Debug.LogError("wheelButtons is null or empty! Make sure wheelButtonsParent is assigned in Inspector.");
        }
        
        return true;
    }
    
    /// <summary>
    /// Remove an item from the wheel (called when item is removed from inventory)
    /// </summary>
    public void UnassignItemFromWheel(Slot sourceSlot)
    {
        for (int i = 0; i < assignedSlots.Length; i++)
        {
            if (assignedSlots[i] == sourceSlot)
            {
                // Shift all items after this one down
                for (int j = i; j < assignedSlots.Length - 1; j++)
                {
                    assignedItemPrefabs[j] = assignedItemPrefabs[j + 1];
                    assignedSlots[j] = assignedSlots[j + 1];
                }
                
                // Clear the last slot
                assignedItemPrefabs[7] = null;
                assignedSlots[7] = null;
                
                nextAvailableWheelSlot = Mathf.Max(0, nextAvailableWheelSlot - 1);
                
                // Update assigned status for remaining slots
                UpdateAllSlotAssignmentStatus();
                
                UpdateWheelDisplay();
                return;
            }
        }
    }
    
    /// <summary>
    /// Update the visual assignment status of all inventory slots
    /// </summary>
    private void UpdateAllSlotAssignmentStatus()
    {
        if (inventoryController == null || inventoryController.inventoryPanel == null)
            return;
        
        Transform inventoryPanel = inventoryController.inventoryPanel.transform;
        
        // First, reset all slots
        foreach (Transform child in inventoryPanel)
        {
            Slot slot = child.GetComponent<Slot>();
            if (slot != null)
            {
                slot.SetAssignedToWheel(false);
            }
        }
        
        // Then mark assigned ones
        for (int i = 0; i < assignedSlots.Length; i++)
        {
            if (assignedSlots[i] != null)
            {
                assignedSlots[i].SetAssignedToWheel(true);
            }
        }
    }
    
    /// <summary>
    /// Updates wheel buttons to show manually assigned items
    /// </summary>
    private void UpdateWheelDisplay()
    {
        if (wheelButtons == null || wheelButtons.Length == 0)
            return;
        
        for (int i = 0; i < wheelButtons.Length; i++)
        {
            if (i < assignedItemPrefabs.Length && assignedItemPrefabs[i] != null)
            {
                // Update button with assigned item prefab
                wheelButtons[i].SetItemPrefab(assignedItemPrefabs[i]);
            }
            else
            {
                // Empty slot
                wheelButtons[i].SetItemPrefab(null);
            }
        }
    }
    
    /// <summary>
    /// Get the Item component at a specific wheel slot
    /// </summary>
    private Item GetItemAtWheelSlot(int slotID)
    {
        // weaponID is 1-8, convert to array index 0-7
        int wheelIndex = slotID - 1;
        
        if (wheelIndex >= 0 && wheelIndex < assignedItemPrefabs.Length && assignedItemPrefabs[wheelIndex] != null)
        {
            return assignedItemPrefabs[wheelIndex].GetComponent<Item>();
        }
        
        return null;
    }
}
