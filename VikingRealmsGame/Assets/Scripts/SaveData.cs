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
    
}