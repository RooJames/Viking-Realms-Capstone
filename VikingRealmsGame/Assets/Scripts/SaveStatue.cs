using UnityEngine;
using System.IO;

public class SaveStatue : MonoBehaviour, IInteractable
{
    private SaveController saveController;
    public Sprite activatedSprite; // Optional: change sprite when saved
    private bool canInteract = true;

    void Start()
    {
        saveController = FindFirstObjectByType<SaveController>();
    }

    public bool CanInteract()
    {
        return canInteract;
    }

    public void Interact()
    {
        if (!CanInteract()) return;

        SaveGame();
    }

    private void SaveGame()
    {
        saveController.SaveGame();
        Debug.Log("Game Saved at Statue!");

        // Optional: Change sprite when saved
        if (activatedSprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = activatedSprite;
        }

        // Optional: Add cooldown so player can't spam save
        // canInteract = false;
        // Invoke("ResetInteract", 2f);
    }

    // private void ResetInteract()
    // {
    //     canInteract = true;
    // }
}