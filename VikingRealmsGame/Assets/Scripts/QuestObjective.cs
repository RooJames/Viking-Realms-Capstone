using System;
using UnityEngine;

[Serializable]
public class QuestObjective
{
    public enum ObjectiveType { KillEnemy, CollectItem, TalkToNPC }

    public ObjectiveType type;

    [Tooltip("Enemy tag (e.g. 'Orc'), WorldItemID, or NPC name — must match exactly")]
    public string targetID;

    [TextArea(1, 2)]
    public string description;      // shown in the quest log

    public int requiredCount = 1;

    // Runtime-only counter — restored from QuestManager's dictionary on load.
    // Not written to the .asset file.
    [NonSerialized] public int currentCount;

    public bool IsComplete => currentCount >= requiredCount;
}
