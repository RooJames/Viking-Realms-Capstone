using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController;
    private List<string> collectedItemIDs = new List<string>();


    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryController = FindAnyObjectByType<InventoryController>();
        LoadGame();
    }

    public void RegisterCollectedItem(string itemID)
    {
        if (!collectedItemIDs.Contains(itemID))
        {
            collectedItemIDs.Add(itemID);
        }
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData();
        saveData.playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        saveData.inventorySaveData = inventoryController.GetInventoryItems() ?? new List<InventorySaveData>();
        saveData.collectedItemIDs = new List<string>(collectedItemIDs);


        // Save all opened chests
        Chest[] chests = FindObjectsByType<Chest>(FindObjectsSortMode.None);
        foreach (Chest chest in chests)
        {
            if (chest.IsOpened)
            {
                saveData.openedChestIDs.Add(chest.ChestID);
            }
        }

        // Save all used statues
        SaveStatue[] statues = FindObjectsByType<SaveStatue>(FindObjectsSortMode.None);
        foreach (SaveStatue statue in statues)
        {
            if (statue.WasUsed)
            {
                saveData.usedStatueIDs.Add(statue.StatueID);
            }
        }

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
        Debug.Log("Game saved!");
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));


            // Restore player position
            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;

            // Restore collected items list
            collectedItemIDs = saveData.collectedItemIDs ?? new List<string>();

            // Remove collected world items
            WorldItem[] worldItems = FindObjectsByType<WorldItem>(FindObjectsSortMode.None);
            foreach (WorldItem worldItem in worldItems)
            {
                if (collectedItemIDs.Contains(worldItem.WorldItemID))
                {
                    Destroy(worldItem.gameObject);
                }
            }

            // Restore inventory
            if (saveData.inventorySaveData != null)
            {
                inventoryController.SetInventoryItems(saveData.inventorySaveData);
            }
            else
            {
                inventoryController.SetInventoryItems(new List<InventorySaveData>());
            }

            // Restore chest states
            Chest[] chests = FindObjectsByType<Chest>(FindObjectsSortMode.None);
            foreach (Chest chest in chests)
            {
                if (saveData.openedChestIDs.Contains(chest.ChestID))
                {
                    chest.SetOpened(true);
                }
            }

            // Restore statue states
            SaveStatue[] statues = FindObjectsByType<SaveStatue>(FindObjectsSortMode.None);
            foreach (SaveStatue statue in statues)
            {
                if (saveData.usedStatueIDs.Contains(statue.StatueID))
                {
                    statue.SetUsed(true);
                }
            }

            Debug.Log("Game loaded!");
        }
        else
        {
            // No save file exists, initialize with empty inventory
            inventoryController.SetInventoryItems(new List<InventorySaveData>());
        }
    }

    public void NewGame()
    {
        if (File.Exists(saveLocation))
        {
            File.Delete(saveLocation);
            Debug.Log("Save deleted! Restart the game to start fresh.");
        }
    }
}
