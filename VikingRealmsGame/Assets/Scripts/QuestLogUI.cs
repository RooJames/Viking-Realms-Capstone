using System.Text;
using UnityEngine;
using TMPro;

// Attach to a Canvas object.
// Wire up logPanel (the root panel to show/hide) and questLogText (a TMP_Text inside it).
// Press J (or whatever toggleKey is set to) to open/close the log.
public class QuestLogUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject logPanel;
    public TMP_Text questLogText;

    [Header("Settings")]
    public KeyCode toggleKey = KeyCode.J;

    void Start()
    {
        if (logPanel != null) logPanel.SetActive(false);

        // Subscribe to QuestManager events so the log refreshes automatically
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.OnQuestAccepted    += HandleQuestChanged;
            QuestManager.Instance.OnQuestCompleted   += HandleQuestChanged;
            QuestManager.Instance.OnObjectiveUpdated += HandleObjectiveUpdated;
        }
    }

    void OnDestroy()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.OnQuestAccepted    -= HandleQuestChanged;
            QuestManager.Instance.OnQuestCompleted   -= HandleQuestChanged;
            QuestManager.Instance.OnObjectiveUpdated -= HandleObjectiveUpdated;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            Toggle();
    }

    public void Toggle()
    {
        if (logPanel == null) return;
        bool nowOpen = !logPanel.activeSelf;
        logPanel.SetActive(nowOpen);
        if (nowOpen) Refresh();
    }

    public void Refresh()
    {
        if (QuestManager.Instance == null || questLogText == null) return;

        var active = QuestManager.Instance.ActiveQuests;
        var sb = new StringBuilder();

        if (active.Count == 0)
        {
            sb.AppendLine("No active quests.");
        }
        else
        {
            foreach (Quest q in active)
            {
                sb.AppendLine($"<b>{q.title}</b>");

                if (q.objectives != null)
                {
                    for (int i = 0; i < q.objectives.Length; i++)
                    {
                        QuestObjective obj = q.objectives[i];
                        int current = QuestManager.Instance.GetObjectiveCount(q, i);
                        bool done   = current >= obj.requiredCount;
                        string mark = done ? "<color=#00ff88>✓</color>" : "•";
                        sb.AppendLine($"  {mark} {obj.description}: {current}/{obj.requiredCount}");
                    }
                }

                sb.AppendLine();
            }
        }

        questLogText.text = sb.ToString().TrimEnd();
    }

    // Named handlers so they can be properly unsubscribed
    private void HandleQuestChanged(Quest _)            => Refresh();
    private void HandleObjectiveUpdated(Quest _, int __) => Refresh();
}
