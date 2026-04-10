using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemWheelController : MonoBehaviour
{
    public Animator anim; //add animations later
    private bool weaponWheelSelected = false;

     public GameObject itemWheel;
    public Image selectedItem;
    public Sprite noImage;

    public static int weaponID;
    public TextMeshProUGUI itemText;
    public TextMeshProUGUI quantityText;

    void Start()
    {
        itemWheel.SetActive(false);
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
        }
        else
        {
            Time.timeScale = 1f; // resume game
            anim.SetBool("OpenWeaponWheel", false);
            itemText.gameObject.SetActive(false);
        }

        switch (weaponID)
        {
            case 0: // nothing selected
                selectedItem.sprite = noImage;
                break;

            case 1: // Sword
                Debug.Log("Sword"); // call the function like animation or item equip         
                break;

            case 2: // Blast
                Debug.Log("Potion");
                break;

            case 3: // Axe
                Debug.Log("Torch");
                break;

            case 4: // Potion
                Debug.Log("Shard");
                break;

            case 5: // Meat
                Debug.Log("Blast");
                break;

            case 6: // Torch
                Debug.Log("Empty");
                break;

            case 7: // Placeholder
                Debug.Log("Axe");
                break;

            case 8: // Placeholder
                Debug.Log("Meat");
                break;
        }
    }

     public void SetSelectedItemInfo(string newItemName, Sprite newIcon, int amount)
    {
        if (selectedItem != null)
            selectedItem.sprite = newIcon != null ? newIcon : noImage;

        if (itemText != null)
            itemText.text = newItemName;

        if (quantityText != null)
        {
            if (amount > 0)
                quantityText.text = "x" + amount;
            else
                quantityText.text = "";
        }
    }

    public void ClearSelectedItemInfo()
    {
        if (selectedItem != null)
            selectedItem.sprite = noImage;

        if (itemText != null)
            itemText.text = "";

        if (quantityText != null)
            quantityText.text = "";
    }
}

