using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[ExecuteInEditMode]
public class TerrainBatchEditor : MonoBehaviour
{
    [SerializeField] private Terrain[] settingsDestinations;
    [SerializeField] private Terrain settingsSource;
    [SerializeField] private bool addAllTerrains;
    [SerializeField] private bool applySettings;
    private void Update()
    {
        if (applySettings)
        {
            applySettings = false;
            ModifyTerrains(settingsDestinations);
        }
        if (addAllTerrains)
        {
            addAllTerrains = false;
            settingsDestinations = Terrain.activeTerrains;
        }
    }
    public void ModifyTerrains(Terrain[] terrains)
    {
        if (terrains.Length == 0)
        {
            Debug.LogWarning("No terrains set.");
            return;
        }

        foreach (Terrain terrain in terrains)
        {
            terrain.heightmapPixelError = settingsSource.heightmapPixelError;
            terrain.allowAutoConnect = settingsSource.allowAutoConnect;
            terrain.basemapDistance = settingsSource.basemapDistance;
            terrain.shadowCastingMode = settingsSource.shadowCastingMode;
            terrain.treeBillboardDistance = settingsSource.treeBillboardDistance;
            terrain.treeCrossFadeLength = settingsSource.treeCrossFadeLength;
            terrain.treeDistance = settingsSource.treeDistance;
            terrain.treeLODBiasMultiplier = settingsSource.treeLODBiasMultiplier;
            terrain.treeMaximumFullLODCount = settingsSource.treeMaximumFullLODCount;
            terrain.treeMotionVectorModeOverride = settingsSource.treeMotionVectorModeOverride;
            terrain.detailObjectDensity = settingsSource.detailObjectDensity;
            terrain.detailObjectDistance = settingsSource.detailObjectDistance;
            terrain.drawHeightmap = settingsSource.drawHeightmap;
            terrain.drawInstanced = settingsSource.drawInstanced;
            terrain.drawTreesAndFoliage = settingsSource.drawTreesAndFoliage;
            terrain.editorRenderFlags = settingsSource.editorRenderFlags;
            terrain.enabled = settingsSource.enabled;
            terrain.enableHeightmapRayTracing = settingsSource.enableHeightmapRayTracing;
            terrain.heightmapMaximumLOD = settingsSource.heightmapMaximumLOD;
            terrain.heightmapMinimumLODSimplification = settingsSource.heightmapMinimumLODSimplification;
            terrain.heightmapMaximumLOD = settingsSource.heightmapMaximumLOD;
            terrain.heightmapPixelError = settingsSource.heightmapPixelError;
            terrain.ignoreQualitySettings = settingsSource.ignoreQualitySettings;
            terrain.preserveTreePrototypeLayers = settingsSource.preserveTreePrototypeLayers;
            terrain.reflectionProbeUsage = settingsSource.reflectionProbeUsage;
        }

        Debug.Log($"Modified {terrains.Length} terrains.");
    }
}
