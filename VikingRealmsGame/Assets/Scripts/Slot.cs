using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public GameObject currentItem;
    
    [Header("Wheel Assignment")]
    [SerializeField] public Color normalColor = Color.white;
    [SerializeField] public Color selectedColor = Color.yellow;
    [SerializeField] public bool isAssignedToWheel = false;
    
    private Image slotImage;
    private ItemWheelController wheelController;
    private GameObject previousItem; // Track previous item to detect changes
    
    void Start()
    {
        slotImage = GetComponent<Image>();
        wheelController = FindAnyObjectByType<ItemWheelController>();
        previousItem = currentItem;
    }
    
    void Update()
    {
        // Check if item was removed or changed
        if (previousItem != currentItem)
        {
            // Item was removed or swapped
            if (isAssignedToWheel && wheelController != null)
            {
                // Unassign from wheel
                wheelController.UnassignItemFromWheel(this);
                SetAssignedToWheel(false);
            }
            
            previousItem = currentItem;
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        // Only allow assignment/unassignment if there's an item in this slot
        if (currentItem != null && wheelController != null)
        {
            Item item = currentItem.GetComponent<Item>();
            if (item != null)
            {
                // Toggle: if already assigned, unassign it
                if (isAssignedToWheel)
                {
                    wheelController.UnassignItemFromWheel(this);
                    SetAssignedToWheel(false);
                }
                else
                {
                    // Try to assign to wheel
                    bool wasAssigned = wheelController.AssignItemToWheel(item, this);
                    
                    if (wasAssigned)
                    {
                        SetAssignedToWheel(true);
                    }
                }
            }
        }
    }
    
    public void SetAssignedToWheel(bool assigned)
    {
        isAssignedToWheel = assigned;
        
        if (slotImage != null)
        {
            slotImage.color = assigned ? selectedColor : normalColor;
        }
    }
}
