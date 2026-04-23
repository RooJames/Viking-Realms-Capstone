using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    private InteractionManager interactionManager;
    private ItemWheelController itemWheelController;

    void Start()
    {
        menuCanvas.SetActive(false);
        interactionManager = FindObjectOfType<InteractionManager>();
        itemWheelController = FindAnyObjectByType<ItemWheelController>();
    }

    void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            bool isActive = !menuCanvas.activeSelf;
            menuCanvas.SetActive(isActive);

            // Close the weapon wheel whenever the inventory opens or closes
            if (itemWheelController != null)
            {
                itemWheelController.CloseWheel();
                itemWheelController.SetSelectedItemVisible(!isActive);
            }

            if (interactionManager != null)
                interactionManager.SetPaused(isActive);
        }
    }

    /// <summary>
    /// Closes the inventory (called externally, e.g. when weapon wheel opens).
    /// </summary>
    public void CloseInventory()
    {
        menuCanvas.SetActive(false);
        if (interactionManager != null)
            interactionManager.SetPaused(false);
        if (itemWheelController != null)
            itemWheelController.SetSelectedItemVisible(true);
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
