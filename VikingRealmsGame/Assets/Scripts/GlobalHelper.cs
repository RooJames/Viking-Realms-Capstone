using UnityEngine;

public static class GlobalHelper
{
    public static string GenerateUniqueID(GameObject obj)
    {
        // Use rounded position values to avoid floating point issues
        int x = Mathf.RoundToInt(obj.transform.position.x * 100);
        int y = Mathf.RoundToInt(obj.transform.position.y * 100);

        // Use scene build index instead of name (more reliable)
        int sceneIndex = obj.scene.buildIndex;

        return $"{sceneIndex}_{obj.name}_{x}_{y}";
    }
}