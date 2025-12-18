using Unity.VisualScripting;
using UnityEngine;
public class TerrainManger : MonoBehaviour
{
    [SerializeField] private Terrain[] terrains;
    [SerializeField] private float renderDistance;
    [SerializeField] private bool addAllActiveTerrains;
    [SerializeField] private bool areTreesChunked;
    void Update()
    {
        if (addAllActiveTerrains)
        {
            addAllActiveTerrains = false;
            terrains = Terrain.activeTerrains;
        }
        for (int i = 0; i < terrains.Length; i++)
        {
            if (!terrains[i].IsUnityNull())
            {
                Cull(Camera.main, terrains[i], renderDistance);
            }
        }
    }
    public float XZDistance(Vector3 a, Vector3 b)
    {
        Vector2 aXZ = new Vector2(a.x, a.z);
        Vector2 bXZ = new Vector2(b.x, b.z);
        return Vector2.Distance(aXZ, bXZ);
    }
    public void Cull(Camera cam, Terrain terrain, float renderDistance)
    {
        if (cam.IsUnityNull())
        {
            return;
        }
        Vector3 terrainCenter = terrain.transform.position + terrain.terrainData.size * 0.5f;
        float xzDist = XZDistance(cam.transform.position, terrainCenter);
        bool tooFar = xzDist > renderDistance;
        terrain.drawHeightmap = !(tooFar);
    }
}
