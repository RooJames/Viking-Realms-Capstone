using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemEnabler : MonoBehaviour
{
    void Awake()
    {
        // This needs to run very early
        Debug.Log("InputSystemEnabler: Enabling Input System...");
        
        // Enable the UI input module's actions
        var eventSystem = UnityEngine.EventSystems.EventSystem.current;
        if (eventSystem != null)
        {
            var inputModule = eventSystem.GetComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
            if (inputModule != null && inputModule.actionsAsset != null)
            {
                inputModule.actionsAsset.Enable();
                Debug.Log("UI Input Actions enabled in Awake");
            }
        }
    }
}
