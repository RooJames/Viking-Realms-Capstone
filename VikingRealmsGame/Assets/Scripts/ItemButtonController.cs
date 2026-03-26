using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemButtonController : MonoBehaviour
{
    public int ID;
    private Animator anim;
    public string itemName;
    public TextMeshProUGUI itemText;
    public Image selectedItem;
    private bool selected = false;
    public Sprite icon;

    void Start()
    {
        
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        if (selected)
        {
            selectedItem.sprite = icon;
            itemText.text = itemName;
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
        anim.SetBool("Hover", true);
        itemText.text = itemName;
    }

    public void HoverExit()
    {
        anim.SetBool("Hover", false);
        itemText.text = "";
    }
}
