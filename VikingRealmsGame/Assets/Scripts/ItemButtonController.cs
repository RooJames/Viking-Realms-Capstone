using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemButtonController : MonoBehaviour
{
    public int ID;
    private Animator anim;
     public int itemAmount = 1; 
    public TextMeshProUGUI quantityText; 
    public string itemName;
    public TextMeshProUGUI itemText;
    public Image selectedItem;
    private bool selected = false;
    public Sprite icon;

    private ItemWheelController wheelController;
    void Start()
    {
        
        anim = GetComponent<Animator>();
        wheelController = FindAnyObjectByType<ItemWheelController>();
    }

    void Update()
    {
       if (selected)
        {
            selectedItem.sprite = icon;
            itemText.text = itemName;

            if (quantityText != null)
            {
                if (itemAmount > 0)
                    quantityText.text = "x" + itemAmount;
                else
                    quantityText.text = "";
            }

            if (wheelController != null)
                wheelController.SetSelectedItemInfo(itemName, icon, itemAmount);
        }
    }

    public void Selected()
    {
        selected = true;
        ItemWheelController.weaponID = ID;
    }

    public void Deselected()
    {
        selected = true;
        ItemWheelController.weaponID = ID;

        if (wheelController != null)
            wheelController.SetSelectedItemInfo(itemName, icon, itemAmount);
    }

    public void HoverEnter()
    {
        anim.SetBool("Hover", true);
        itemText.text = itemName;

        if (quantityText != null)
        {
            if (itemAmount > 0)
                quantityText.text = "x" + itemAmount;
            else
                quantityText.text = "";
        }
    }

    public void HoverExit()
    {
        anim.SetBool("Hover", false);
        itemText.text = "";

        if (quantityText != null)
            quantityText.text = "";
    }
}
