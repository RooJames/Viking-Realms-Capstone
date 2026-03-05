using UnityEngine;
using UnityEditor;
using System.IO;

public class TilesetExporter : EditorWindow
{
    private Texture2D tileset;
    private int tileWidth = 8;
    private int tileHeight = 16;
    private string exportFolder = "Assets/ExportedTiles";

    [MenuItem("Tools/Tileset Exporter")]
    public static void ShowWindow()
    {
        GetWindow<TilesetExporter>("Tileset Exporter");
    }

    void OnGUI()
    {
        GUILayout.Label("Export Tileset to Individual PNGs", EditorStyles.boldLabel);
        
        tileset = (Texture2D)EditorGUILayout.ObjectField("Tileset", tileset, typeof(Texture2D), false);
        tileWidth = EditorGUILayout.IntField("Tile Width (pixels)", tileWidth);
        tileHeight = EditorGUILayout.IntField("Tile Height (pixels)", tileHeight);
        exportFolder = EditorGUILayout.TextField("Export Folder", exportFolder);

        if (GUILayout.Button("Export Tiles"))
        {
            if (tileset != null)
            {
                ExportTiles();
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Please assign a tileset texture first!", "OK");
            }
        }
    }

    void ExportTiles()
    {
        // Make texture readable
        string assetPath = AssetDatabase.GetAssetPath(tileset);
        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        
        bool wasReadable = importer.isReadable;
        TextureImporterCompression originalCompression = importer.textureCompression;
        
        importer.isReadable = true;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        AssetDatabase.ImportAsset(assetPath);

        // Create export folder if it doesn't exist
        if (!Directory.Exists(exportFolder))
        {
            Directory.CreateDirectory(exportFolder);
        }

        int tilesX = tileset.width / tileWidth;
        int tilesY = tileset.height / tileHeight;
        int tileCount = 0;

        for (int y = 0; y < tilesY; y++)
        {
            for (int x = 0; x < tilesX; x++)
            {
                // Create a new texture for this tile
                Texture2D tile = new Texture2D(tileWidth, tileHeight);
                
                // Get pixels from the tileset (flip Y because Unity coordinates go bottom-up)
                Color[] pixels = tileset.GetPixels(x * tileWidth, tileset.height - (y + 1) * tileHeight, tileWidth, tileHeight);
                tile.SetPixels(pixels);
                tile.Apply();

                // Encode to PNG
                byte[] bytes = tile.EncodeToPNG();
                
                // Save to file
                string fileName = $"{Path.GetFileNameWithoutExtension(assetPath)}_tile_{tileCount}.png";
                string filePath = Path.Combine(exportFolder, fileName);
                File.WriteAllBytes(filePath, bytes);
                
                tileCount++;
                
                // Clean up
                DestroyImmediate(tile);
            }
        }

        // Restore original texture settings
        importer.isReadable = wasReadable;
        importer.textureCompression = originalCompression;
        AssetDatabase.ImportAsset(assetPath);

        // Refresh asset database
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Success", $"Exported {tileCount} tiles to {exportFolder}", "OK");
    }
}
