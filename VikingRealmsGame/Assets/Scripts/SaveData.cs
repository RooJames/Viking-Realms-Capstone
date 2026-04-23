using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition;
    public List<string> openedChestIDs = new List<string>();
    public List<string> usedStatueIDs = new List<string>();
    public List<string> collectedItemIDs = new List<string>();
    public string mapBoundary;
    public List<InventorySaveData> inventorySaveData;

    // ── Quest state ──────────────────────────────────────────────────────────
    public List<string> activeQuestIDs    = new List<string>();
    public List<string> completedQuestIDs = new List<string>();
    public List<QuestProgressEntry> questProgress = new List<QuestProgressEntry>();
}

[System.Serializable]
public class QuestProgressEntry
{
    public string questID;
    public int[]  objectiveCounts;
}

// Lightweight wrapper passed between SaveController and QuestManager
[System.Serializable]
public class QuestSaveBlock
{
    public List<string>             activeQuestIDs    = new List<string>();
    public List<string>             completedQuestIDs = new List<string>();
    public List<QuestProgressEntry> questProgress     = new List<QuestProgressEntry>();
}