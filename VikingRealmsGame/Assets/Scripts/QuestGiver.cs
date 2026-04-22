using UnityEngine;

// Add this alongside an NPC component to make that NPC offer/complete quests.
// NPC.Interact() will call TryHandleInteraction() first; if it returns true
// the quest dialogue is shown and the normal NPC dialogue is skipped.
[RequireComponent(typeof(NPC))]
public class QuestGiver : MonoBehaviour
{
    [Header("Quest Assignments")]
    [Tooltip("Quest this NPC starts. Leave empty if this NPC only turns in.")]
    public Quest questToOffer;

    [Tooltip("Quest this NPC completes. Can be the same as questToOffer.")]
    public Quest questToComplete;

    [Header("Quest Context (optional)")]
    [Tooltip("A quest this NPC is involved in but doesn't give or complete — e.g. a visit target.")]
    public Quest questContext;

    [Tooltip("Lines shown while questContext is active. Falls back to normal NPC dialogue if empty.")]
    [TextArea(1, 3)] public string[] questContextLines;

    [Tooltip("Lines shown after questContext has been completed.")]
    [TextArea(1, 3)] public string[] questContextCompletedLines;

    private NPC _npc;

    void Awake()
    {
        _npc = GetComponent<NPC>();
    }

    // Returns true if quest dialogue was shown (skipping the NPC's default lines).
    public bool TryHandleInteraction()
    {
        if (QuestManager.Instance == null) return false;

        string npcName = _npc.npcName;

        // ── 1. Turn-in ────────────────────────────────────────────────────────
        if (questToComplete != null
            && QuestManager.Instance.IsActive(questToComplete)
            && QuestManager.Instance.AreAllObjectivesComplete(questToComplete))
        {
            QuestManager.Instance.TurnInQuest(questToComplete);
            ShowLines(npcName, questToComplete.completionLines, "Quest complete!");
            return true;
        }

        // ── 2. Offer ──────────────────────────────────────────────────────────
        if (questToOffer != null && QuestManager.Instance.IsAvailable(questToOffer))
        {
            QuestManager.Instance.AcceptQuest(questToOffer);
            ShowLines(npcName, questToOffer.offerLines, "...");
            return true;
        }

        // ── 3. In progress ────────────────────────────────────────────────────
        if (questToOffer != null && QuestManager.Instance.IsActive(questToOffer))
        {
            ShowLines(npcName, questToOffer.activeLines, "...");
            return true;
        }

        // ── 4. Quest context: special lines when a related quest is active/done ─
        if (questContext != null)
        {
            if (QuestManager.Instance.IsActive(questContext) &&
                questContextLines != null && questContextLines.Length > 0)
            {
                ShowLines(npcName, questContextLines, "...");
                return true;
            }

            if (QuestManager.Instance.IsCompleted(questContext.questID) &&
                questContextCompletedLines != null && questContextCompletedLines.Length > 0)
            {
                ShowLines(npcName, questContextCompletedLines, "...");
                return true;
            }
        }

        // ── 5. Fall through to normal NPC dialogue ────────────────────────────
        return false;
    }

    private void ShowLines(string npcName, string[] lines, string fallback)
    {
        if (DialogueUI.Instance == null) return;

        if (lines != null && lines.Length > 0)
            DialogueUI.Instance.ShowDialogue(npcName, lines);
        else
            DialogueUI.Instance.ShowDialogue(npcName, new[] { fallback });
    }
}
