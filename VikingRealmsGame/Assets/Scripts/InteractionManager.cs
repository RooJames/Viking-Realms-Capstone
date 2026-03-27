using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private List<IInteractable> nearbyInteractables = new List<IInteractable>();
    private int currentIndex = 0;

    public KeyCode interactKey = KeyCode.F;
    public KeyCode cycleKey = KeyCode.C;

    public GameObject interactUI;
    public GameObject itemUI;

    private bool isPaused = false;

    void Update()
    {
        if (isPaused) return; // Don't process interactions when paused

        nearbyInteractables.RemoveAll(x => x == null || (x as MonoBehaviour) == null || !x.CanInteract());
        
        // Clamp index after cleanup in case items were removed
        if (nearbyInteractables.Count > 0)
        {
            currentIndex = Mathf.Clamp(currentIndex, 0, nearbyInteractables.Count - 1);
        }
        else
        {
            currentIndex = 0;
        }

        if (nearbyInteractables.Count > 0)
        {
            if (Input.GetKeyDown(cycleKey))
            {
                currentIndex = (currentIndex + 1) % nearbyInteractables.Count;
                UpdateUI();
            }

            if (Input.GetKeyDown(interactKey))
            {
                // Verify index is still valid before accessing
                if (currentIndex < nearbyInteractables.Count)
                {
                    IInteractable current = nearbyInteractables[currentIndex];
                    if (current != null && current.CanInteract())
                    {
                        current.Interact();
                        HideUI();

                        // Item will be removed by RemoveAll on next frame
                        // Just clamp the index
                        if (currentIndex >= nearbyInteractables.Count - 1)
                        {
                            currentIndex = Mathf.Max(0, nearbyInteractables.Count - 2);
                        }

                        UpdateUI();
                    }
                }
            }

            UpdateUI();
        }
        else
        {
            HideUI();
        }
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;
        if (isPaused)
        {
            HideUI();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();
        if (interactable != null && interactable.CanInteract() && !nearbyInteractables.Contains(interactable))
        {
            nearbyInteractables.Add(interactable);
            currentIndex = 0;
            UpdateUI();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();
        if (interactable != null && nearbyInteractables.Contains(interactable))
        {
            nearbyInteractables.Remove(interactable);
            currentIndex = Mathf.Clamp(currentIndex, 0, nearbyInteractables.Count - 1);
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (isPaused || nearbyInteractables.Count == 0)
        {
            HideUI();
            return;
        }

        IInteractable current = nearbyInteractables[currentIndex];

        if (current is WorldItem)
        {
            ShowItemUI();
        }
        else
        {
            ShowInteractUI();
        }
    }

    private void ShowItemUI()
    {
        if (itemUI != null)
        {
            itemUI.SetActive(true);
        }
        if (interactUI != null)
        {
            interactUI.SetActive(false);
        }
    }

    private void ShowInteractUI()
    {
        if (interactUI != null)
        {
            interactUI.SetActive(true);
        }
        if (itemUI != null)
        {
            itemUI.SetActive(false);
        }
    }

    private void HideUI()
    {
        if (interactUI != null)
        {
            interactUI.SetActive(false);
        }
        if (itemUI != null)
        {
            itemUI.SetActive(false);
        }
    }
}