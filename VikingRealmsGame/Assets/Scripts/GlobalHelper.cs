using UnityEngine;
//generates a unique ID for GameObjects according to scene name and position for tracking persistent state across sessions
public static class GlobalHelper
{
    public static string GenerateUniqueID(GameObject obj)
    {
        return $"{obj.scene.name}_{obj.transform.position.x}_{obj.transform.position.y}"; //Chest_3_4
    }
}