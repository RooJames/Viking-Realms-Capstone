using UnityEngine;
using UnityEngine.UI;

public class ItemWheelController : MonoBehaviour
{
    //public Animator anim;
    private bool weaponWheelSelected = false;

     public GameObject itemWheel;
    public Image selectedItem;
    public Sprite noImage;

    public static int weaponID;

    void Update()
    {
         if (Input.GetKeyDown(KeyCode.I))
        {
            weaponWheelSelected = !weaponWheelSelected;
            itemWheel.SetActive(weaponWheelSelected);
        }

        if (weaponWheelSelected)
        {

        }
        else
        {

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
}
