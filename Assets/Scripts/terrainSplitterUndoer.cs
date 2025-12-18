#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteInEditMode]
public class TerrainSplitterUndoer : MonoBehaviour
{
    [Header("Undo")]
    [SerializeField] private bool undoTerrainSplitter;
    [SerializeField] private Terrain[] parentTerrains;
    [SerializeField] private bool addAllActiveTerrains;
    private void Update()
    {
        if (addAllActiveTerrains)
        {
            addAllActiveTerrains = false;
            parentTerrains = Terrain.activeTerrains;
        }
        if (undoTerrainSplitter)
        {
            undoTerrainSplitter = false;
            Debug.Log("uerg");
            DeleteInactiveTerrainChildren();
        }
    }
    public void DeleteInactiveTerrainChildren()
    {
        Terrain[] activeTerrains = parentTerrains;

        foreach (Terrain terrain in activeTerrains)
        {
            Transform terrainTransform = terrain.transform;

            for (int i = terrainTransform.childCount - 1; i >= 0; i--)
            {
                Transform child = terrainTransform.GetChild(i);

                if (child.name.StartsWith(terrain.name))
                {
                    Debug.Log($"Deleted inactive child '{child.name}' from terrain '{terrain.name}'");
#if UNITY_EDITOR
                    Undo.DestroyObjectImmediate(child.gameObject);
#else
                    GameObject.DestroyImmediate(child.gameObject);
#endif
                }
            }
            terrain.drawHeightmap = true;
        }
    }
}
