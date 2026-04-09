using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Singleton dialogue panel. Wire up via the Unity Inspector.
// Requires a Canvas with:
//   - dialoguePanel  (the root panel GameObject, toggled on/off)
//   - nameText       (TMP_Text)  -- shows the NPC's name
//   - bodyText       (TMP_Text)  -- shows the current dialogue line
//   - continueButton (Button)    -- advances / closes dialogue
public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance { get; private set; }

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text bodyText;
    public Button continueButton;

    public bool IsOpen => dialoguePanel != null && dialoguePanel.activeSelf;

    private string[] _lines;
    private int _lineIndex;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (continueButton != null) continueButton.onClick.AddListener(OnContinue);
    }

    // Call this from NPC.Interact()
    public void ShowDialogue(string npcName, string[] lines)
    {
        if (lines == null || lines.Length == 0) return;

        _lines = lines;
        _lineIndex = 0;

        if (nameText != null) nameText.text = npcName;
        dialoguePanel.SetActive(true);
        DisplayLine();
    }

    private void DisplayLine()
    {
        if (bodyText != null) bodyText.text = _lines[_lineIndex];
    }

    // Bound to the Continue button
    private void OnContinue()
    {
        _lineIndex++;
        if (_lineIndex < _lines.Length)
        {
            DisplayLine();
        }
        else
        {
            CloseDialogue();
        }
    }

    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        _lines = null;
    }
}
