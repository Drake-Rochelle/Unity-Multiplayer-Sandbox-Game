using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[ExecuteInEditMode]
public class TerrainSplitter : MonoBehaviour
{
    [Header("Terrain Splitter")]
    [SerializeField] private Terrain[] terrains;
    [SerializeField] private bool chunkTrees;
    [Space]
    [Space]
    [Space]
    [SerializeField] private bool allActiveTerrains;
    [SerializeField] private bool splitTerrains;
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Header("Undo Terrain Splitter")]
    [SerializeField] private Terrain[] undoTerrains;
    [Space]
    [Space]
    [Space]
    [SerializeField] private bool addAllActiveTerrains;
    [SerializeField] private bool addAllFromTerrainSplitterList;
    [SerializeField] private bool undoTerrainSplitter;

    private void Update()
    {
        if (addAllActiveTerrains)
        {
            addAllActiveTerrains = false;
            undoTerrains = Terrain.activeTerrains;
        }
        if (undoTerrainSplitter)
        {
            undoTerrainSplitter = false;
            DeleteInactiveTerrainChildren();
        }
        if (addAllFromTerrainSplitterList)
        {
            addAllFromTerrainSplitterList = false;
            undoTerrains = terrains;
        }
        if (allActiveTerrains)
        {
            allActiveTerrains = false;
            terrains = Terrain.activeTerrains;
        }
        if (splitTerrains)
        {
            splitTerrains = false;
            SplitAllTerrains();
        }
    }
    public void SplitAllTerrains()
    {
        if (terrains == null || terrains.Length == 0){return;}
        for (int i = 0; i < terrains.Length; i++)
        {
            SplitTerrainIntoGrid(terrains[i]);
            terrains[i].drawHeightmap = false;
            terrains[i].drawInstanced = true;
        }
    }
    void SplitTerrainIntoGrid(Terrain sourceTerrain)
    {
        if (sourceTerrain.drawHeightmap)
        {
            TerrainData sourceData = sourceTerrain.terrainData;
            int heightRes = PadResolution(sourceData.heightmapResolution - 1) + 1;
            int detailRes = PadResolution(sourceData.detailResolution);
            int alphaRes = PadResolution(sourceData.alphamapResolution);
            int heightStep = (heightRes - 1) / 2;
            int detailStep = detailRes / 2;
            int alphaStep = alphaRes / 2;
            Vector3 terrainSize = sourceData.size;
            Vector3 chunkSize = new Vector3(terrainSize.x / 2, terrainSize.y, terrainSize.z / 2);
            Terrain[,] terrainGrid = new Terrain[2, 2];
            for (int x = 0; x < 2; x++)
            {
                for (int z = 0; z < 2; z++)
                {
                    TerrainData newData = new TerrainData();
                    newData.heightmapResolution = heightStep + 1;
                    newData.size = chunkSize;
                    newData.baseMapResolution = sourceData.baseMapResolution;
                    newData.alphamapResolution = alphaStep;
                    newData.SetDetailResolution(detailStep, sourceData.detailResolutionPerPatch);
                    newData.wavingGrassStrength = sourceData.wavingGrassStrength;
                    newData.wavingGrassSpeed = sourceData.wavingGrassSpeed;
                    newData.wavingGrassAmount = sourceData.wavingGrassAmount;
                    newData.wavingGrassTint = sourceData.wavingGrassTint;
                    newData.terrainLayers = sourceData.terrainLayers;
                    newData.detailPrototypes = sourceData.detailPrototypes;
                    newData.treePrototypes = sourceData.treePrototypes;
                    float[,] heights = sourceData.GetHeights(x * heightStep, z * heightStep, heightStep + 1, heightStep + 1);
                    newData.SetHeights(0, 0, heights);
                    float[,,] splats = sourceData.GetAlphamaps(x * alphaStep, z * alphaStep, alphaStep, alphaStep);
                    newData.SetAlphamaps(0, 0, splats);
                    for (int layer = 0; layer < sourceData.detailPrototypes.Length; layer++)
                    {
                        int[,] details = sourceData.GetDetailLayer(x * detailStep, z * detailStep, detailStep, detailStep, layer);
                        newData.SetDetailLayer(0, 0, layer, details);
                    }
                    List<TreeInstance> treeList = new List<TreeInstance>();
                    foreach (TreeInstance tree in sourceData.treeInstances)
                    {
                        Vector3 worldPos = new Vector3(tree.position.x * terrainSize.x, tree.position.y, tree.position.z * terrainSize.z);
                        if (worldPos.x >= x * chunkSize.x && worldPos.x < (x + 1) * chunkSize.x &&
                            worldPos.z >= z * chunkSize.z && worldPos.z < (z + 1) * chunkSize.z)
                        {
                            TreeInstance newTree = tree;
                            newTree.position = new Vector3(
                                (worldPos.x - x * chunkSize.x) / chunkSize.x,
                                tree.position.y,
                                (worldPos.z - z * chunkSize.z) / chunkSize.z);
                            treeList.Add(newTree);
                        }
                    }
                    if (chunkTrees)
                    {
                        newData.treeInstances = treeList.ToArray();
                    }
                    GameObject terrainGO = Terrain.CreateTerrainGameObject(newData);
                    terrainGO.transform.parent = sourceTerrain.gameObject.transform;
                    terrainGO.transform.position = sourceTerrain.transform.position + new Vector3(x * chunkSize.x, 0, z * chunkSize.z);
                    terrainGO.name = $"{sourceTerrain.name}_Chunk_{x}_{z}";
                    Terrain newTerrain = terrainGO.GetComponent<Terrain>();
                    newTerrain.heightmapPixelError = sourceTerrain.heightmapPixelError;
                    newTerrain.allowAutoConnect = sourceTerrain.allowAutoConnect;
                    newTerrain.basemapDistance = sourceTerrain.basemapDistance;
                    newTerrain.shadowCastingMode = sourceTerrain.shadowCastingMode;
                    newTerrain.treeBillboardDistance = sourceTerrain.treeBillboardDistance;
                    newTerrain.treeCrossFadeLength = sourceTerrain.treeCrossFadeLength;
                    newTerrain.treeDistance = sourceTerrain.treeDistance;
                    newTerrain.treeLODBiasMultiplier = sourceTerrain.treeLODBiasMultiplier;
                    newTerrain.treeMaximumFullLODCount = sourceTerrain.treeMaximumFullLODCount;
                    newTerrain.treeMotionVectorModeOverride = sourceTerrain.treeMotionVectorModeOverride;
                    newTerrain.detailObjectDensity = sourceTerrain.detailObjectDensity;
                    newTerrain.detailObjectDistance = sourceTerrain.detailObjectDistance;
                    newTerrain.drawHeightmap = sourceTerrain.drawHeightmap;
                    newTerrain.drawInstanced = sourceTerrain.drawInstanced;
                    newTerrain.drawTreesAndFoliage = sourceTerrain.drawTreesAndFoliage;
                    newTerrain.editorRenderFlags = sourceTerrain.editorRenderFlags;
                    newTerrain.enabled = sourceTerrain.enabled;
                    newTerrain.enableHeightmapRayTracing = sourceTerrain.enableHeightmapRayTracing;
                    newTerrain.heightmapMaximumLOD = sourceTerrain.heightmapMaximumLOD;
                    newTerrain.heightmapMinimumLODSimplification = sourceTerrain.heightmapMinimumLODSimplification;
                    newTerrain.heightmapMaximumLOD = sourceTerrain.heightmapMaximumLOD;
                    newTerrain.heightmapPixelError = sourceTerrain.heightmapPixelError;
                    newTerrain.ignoreQualitySettings = sourceTerrain.ignoreQualitySettings;
                    newTerrain.preserveTreePrototypeLayers = sourceTerrain.preserveTreePrototypeLayers;
                    newTerrain.reflectionProbeUsage = sourceTerrain.reflectionProbeUsage;
                    terrainGrid[x, z] = terrainGO.GetComponent<Terrain>();
                    #if UNITY_EDITOR
                    Undo.RegisterCreatedObjectUndo(terrainGO, "Split Terrain");
                    #endif
                }
            }
            for (int x = 0; x < 2; x++)
            {
                for (int z = 0; z < 2; z++)
                {
                    Terrain t = terrainGrid[x, z];
                    Terrain left = x > 0 ? terrainGrid[x - 1, z] : null;
                    Terrain right = x < 2 - 1 ? terrainGrid[x + 1, z] : null;
                    Terrain top = z < 2 - 1 ? terrainGrid[x, z + 1] : null;
                    Terrain bottom = z > 0 ? terrainGrid[x, z - 1] : null;
                    t.SetNeighbors(left, top, right, bottom);
                }
            }
            if (chunkTrees)
            {
                sourceTerrain.drawTreesAndFoliage = false;
            }
        }
    }
    int PadResolution(int res)
    {
        int remainder = res % 2;
        return remainder == 0 ? res : res + (2 - remainder);
    }
    public void DeleteInactiveTerrainChildren()
    {
        Terrain[] activeTerrains = undoTerrains;
        foreach (Terrain terrain in activeTerrains)
        {
            if (terrain == null) continue;
            terrain.drawTreesAndFoliage = true;
            Transform terrainTransform = terrain.transform;
            for (int i = terrainTransform.childCount - 1; i >= 0; i--)
            {
                Transform child = terrainTransform.GetChild(i);
                if (child.name.StartsWith(terrain.name))
                {
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
