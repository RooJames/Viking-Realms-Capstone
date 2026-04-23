using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Viking RPG/Quest")]
public class Quest : ScriptableObject
{
    [Header("Identity")]
    public string questID;          // unique string, e.g. "q_hunt_wolves"
    public string title;
    [TextArea(2, 4)] public string description;

    [Header("Dialogue Lines")]
    [TextArea(1, 3)] public string[] offerLines;       // shown when quest is accepted
    [TextArea(1, 3)] public string[] activeLines;      // shown while quest is in progress
    [TextArea(1, 3)] public string[] completionLines;  // shown when turned in

    [Header("Objectives & Rewards")]
    public QuestObjective[] objectives;
    public QuestReward[] rewards;
}
