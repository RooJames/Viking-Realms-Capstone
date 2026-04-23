using System.Collections.Generic;

[System.Serializable]
public class QuestSaveBlock
{
    public List<string> completedQuestIDs;
    public List<string> activeQuestIDs;
    public List<QuestProgressEntry> questProgress;
}