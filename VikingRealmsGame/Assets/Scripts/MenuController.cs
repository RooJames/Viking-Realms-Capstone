using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    private InteractionManager interactionManager;

    void Start()
    {
        menuCanvas.SetActive(false);
        interactionManager = FindObjectOfType<InteractionManager>();
    }

    void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            bool isActive = !menuCanvas.activeSelf;
            menuCanvas.SetActive(isActive);

            if (interactionManager != null)
            {
                interactionManager.SetPaused(isActive);
            }
        }
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}