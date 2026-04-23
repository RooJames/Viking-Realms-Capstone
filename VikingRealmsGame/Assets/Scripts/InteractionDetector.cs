using UnityEngine;
using UnityEngine.InputSystem;

// Detects nearby interactable objects and triggers interaction when player presses F or T.
// Attach this to the Player GameObject alongside a CircleCollider2D set to Is Trigger.
// If InteractionManager is also present on this GameObject, this script defers to it
// to avoid double-firing.
public class InteractionDetector : MonoBehaviour
{
    [Tooltip("Key the player presses to interact (default: T)")]
    public KeyCode interactKey = KeyCode.T;

    [Tooltip("Optional icon shown above the player when an interactable is in range")]
    public GameObject interactionIcon;

    private IInteractable _inRange;
    private InteractionManager _manager;

    void Awake()
    {
        // If InteractionManager is on the same object, let it handle everything
        _manager = GetComponent<InteractionManager>();
    }

    void Start()
    {
        if (interactionIcon) interactionIcon.SetActive(false);
    }

    void Update()
    {
        if (_manager != null) return; // InteractionManager handles input

        if (_inRange == null) return;

        // Old Input System (KeyCode)
        if (Input.GetKeyDown(interactKey))
            _inRange.Interact();

        // New Input System (Keyboard) — only use if not already handled by KeyCode above
        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
            _inRange.Interact();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_manager != null) return; // InteractionManager handles this

        if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            _inRange = interactable;
            if (interactionIcon) interactionIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_manager != null) return; // InteractionManager handles this

        if (collision.TryGetComponent(out IInteractable interactable) && interactable == _inRange)
        {
            _inRange = null;
            if (interactionIcon) interactionIcon.SetActive(false);
        }
    }
}
