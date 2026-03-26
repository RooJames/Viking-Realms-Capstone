using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public Image[] tabImages;
    public GameObject[] pages;

    void Start()
    {
        ActivateTab(0);
    }

    void Update()
    {
        // Check for keyboard number inputs 1-4
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateTab(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateTab(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActivateTab(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ActivateTab(3);
        }
    }

    public void ActivateTab(int tabNo)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
            tabImages[i].color = Color.grey;
        }
        pages[tabNo].SetActive(true);
        tabImages[tabNo].color = Color.white;
    }
}
