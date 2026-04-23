using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    [Header("Quest Registry — drag every Quest asset here so save/load can find them")]
    public List<Quest> questRegistry = new List<Quest>();

    [Header("Quests that start automatically when the game loads")]
    public List<Quest> startingQuests = new List<Quest>();

    // ── Runtime state ────────────────────────────────────────────────────────
    private readonly List<Quest>  _activeQuests      = new List<Quest>();
    private readonly List<string> _completedQuestIDs = new List<string>();

    // questID → per-objective counts (source of truth for progress)
    private readonly Dictionary<string, int[]> _objectiveCounts = new Dictionary<string, int[]>();

    public IReadOnlyList<Quest>  ActiveQuests       => _activeQuests;
    public IReadOnlyList<string> CompletedQuestIDs  => _completedQuestIDs;

    // ── Events ───────────────────────────────────────────────────────────────
    public event Action<Quest>      OnQuestAccepted;
    public event Action<Quest>      OnQuestCompleted;
    public event Action<Quest, int> OnObjectiveUpdated; // (quest, objective index)

    // ── Lifecycle ────────────────────────────────────────────────────────────

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        foreach (Quest q in startingQuests)
            AcceptQuest(q);
    }

    // ── Query ────────────────────────────────────────────────────────────────

    public bool IsAvailable(Quest q) => q != null && !IsActive(q) && !IsCompleted(q.questID);
    public bool IsActive(Quest q)    => q != null && _activeQuests.Contains(q);
    public bool IsCompleted(string questID) => _completedQuestIDs.Contains(questID);

    public bool AreAllObjectivesComplete(Quest q)
    {
        if (q.objectives == null || q.objectives.Length == 0) return true;
        if (!_objectiveCounts.TryGetValue(q.questID, out int[] counts)) return false;
        for (int i = 0; i < q.objectives.Length; i++)
        {
            if (counts[i] < q.objectives[i].requiredCount) return false;
        }
        return true;
    }

    public int GetObjectiveCount(Quest q, int objectiveIndex)
    {
        if (_objectiveCounts.TryGetValue(q.questID, out int[] counts) &&
            objectiveIndex >= 0 && objectiveIndex < counts.Length)
            return counts[objectiveIndex];
        return 0;
    }

    // ── Accept / TurnIn ──────────────────────────────────────────────────────

    public void AcceptQuest(Quest q)
    {
        if (!IsAvailable(q)) return;

        _activeQuests.Add(q);
        _objectiveCounts[q.questID] = new int[q.objectives != null ? q.objectives.Length : 0];
        SyncObjectiveCounts(q);
        OnQuestAccepted?.Invoke(q);
    }

    public void TurnInQuest(Quest q)
    {
        if (!IsActive(q) || !AreAllObjectivesComplete(q)) return;

        // Give rewards
        if (q.rewards != null)
        {
            InventoryController inventory = FindAnyObjectByType<InventoryController>();
            if (inventory != null)
            {
                foreach (QuestReward reward in q.rewards)
                {
                    if (reward.itemPrefab == null) continue;
                    for (int i = 0; i < reward.quantity; i++)
                        inventory.AddItem(reward.itemPrefab);
                }
            }
        }

        _activeQuests.Remove(q);
        _objectiveCounts.Remove(q.questID);
        if (!_completedQuestIDs.Contains(q.questID))
            _completedQuestIDs.Add(q.questID);

        OnQuestCompleted?.Invoke(q);
    }

    // ── Progress tracking ────────────────────────────────────────────────────

    public void TrackKill(string entityTag)  => TrackProgress(QuestObjective.ObjectiveType.KillEnemy,  entityTag);
    public void TrackCollect(string itemID)  => TrackProgress(QuestObjective.ObjectiveType.CollectItem, itemID);
    public void TrackTalk(string npcName)    => TrackProgress(QuestObjective.ObjectiveType.TalkToNPC,  npcName);

    private void TrackProgress(QuestObjective.ObjectiveType type, string targetID)
    {
        foreach (Quest q in _activeQuests)
        {
            if (!_objectiveCounts.TryGetValue(q.questID, out int[] counts)) continue;
            if (q.objectives == null) continue;

            for (int i = 0; i < q.objectives.Length; i++)
            {
                QuestObjective obj = q.objectives[i];
                if (obj.type == type &&
                    obj.targetID == targetID &&
                    counts[i] < obj.requiredCount)
                {
                    counts[i]++;
                    obj.currentCount = counts[i];
                    OnObjectiveUpdated?.Invoke(q, i);
                    break; // one increment per event call
                }
            }
        }
    }

    // Pushes dictionary counts back onto the ScriptableObject's NonSerialized fields
    private void SyncObjectiveCounts(Quest q)
    {
        if (!_objectiveCounts.TryGetValue(q.questID, out int[] counts) ||
            q.objectives == null) return;

        for (int i = 0; i < q.objectives.Length && i < counts.Length; i++)
            q.objectives[i].currentCount = counts[i];
    }

    // ── Save / Load ───────────────────────────────────────────────────────────

    public QuestSaveBlock GetSaveData()
    {
        QuestSaveBlock block = new QuestSaveBlock
        {
            completedQuestIDs = new List<string>(_completedQuestIDs),
            activeQuestIDs    = new List<string>(),
            questProgress     = new List<QuestProgressEntry>()
        };

        foreach (Quest q in _activeQuests)
        {
            block.activeQuestIDs.Add(q.questID);

            if (_objectiveCounts.TryGetValue(q.questID, out int[] counts))
            {
                block.questProgress.Add(new QuestProgressEntry
                {
                    questID          = q.questID,
                    objectiveCounts  = counts
                });
            }
        }
        return block;
    }

    public void LoadSaveData(QuestSaveBlock block)
    {
        if (block == null) return;

        _completedQuestIDs.Clear();
        if (block.completedQuestIDs != null)
            _completedQuestIDs.AddRange(block.completedQuestIDs);

        _activeQuests.Clear();
        _objectiveCounts.Clear();

        if (block.activeQuestIDs == null) return;

        foreach (string id in block.activeQuestIDs)
        {
            Quest q = FindQuestByID(id);
            if (q == null)
            {
                Debug.LogWarning($"QuestManager: could not find quest '{id}' in registry.");
                continue;
            }

            _activeQuests.Add(q);

            QuestProgressEntry entry = block.questProgress?.Find(p => p.questID == id);
            int[] counts = (entry?.objectiveCounts != null)
                ? entry.objectiveCounts
                : new int[q.objectives != null ? q.objectives.Length : 0];

            _objectiveCounts[id] = counts;
            SyncObjectiveCounts(q);
        }
    }

    private Quest FindQuestByID(string id)
    {
        return questRegistry.Find(q => q != null && q.questID == id);
    }
}
