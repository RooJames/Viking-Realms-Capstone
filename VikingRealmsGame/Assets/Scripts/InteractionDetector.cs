using UnityEngine;

// Detects nearby interactable objects and triggers interaction when player presses E.
// Attach this to the Player GameObject alongside a CircleCollider2D set to Is Trigger.
public class InteractionDetector : MonoBehaviour
{
    [Tooltip("Key the player presses to interact (default: T)")]
    public KeyCode interactKey = KeyCode.T;

    [Tooltip("Optional icon shown above the player when an interactable is in range")]
    public GameObject interactionIcon;

    private IInteractable _inRange;

    void Start()
    {
        if (interactionIcon) interactionIcon.SetActive(false);
    }

    void Update()
    {
        if (_inRange != null && Input.GetKeyDown(interactKey))
            _inRange.Interact();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            _inRange = interactable;
            if (interactionIcon) interactionIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable == _inRange)
        {
            _inRange = null;
            if (interactionIcon) interactionIcon.SetActive(false);
        }
    }
}