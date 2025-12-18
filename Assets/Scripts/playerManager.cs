using UnityEngine;
using Photon.Pun;
using System.Text;
using System;
public class playerManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject player;
    public Vector3 GetSpawnPosition(Terrain[] terrain, int targetLayer)
    {
        while (true)
        {
            int index = UnityEngine.Random.Range(0, terrain.Length);
            TerrainData terrainData = terrain[index].terrainData;
            Vector3 terrainPos = terrain[index].transform.position;
            float randomX = UnityEngine.Random.Range(0f, terrainData.size.x);
            float randomZ = UnityEngine.Random.Range(0f, terrainData.size.z);
            int mapX = Mathf.FloorToInt(randomX / terrainData.size.x * terrainData.alphamapWidth);
            int mapZ = Mathf.FloorToInt(randomZ / terrainData.size.z * terrainData.alphamapHeight);
            mapX = Mathf.Clamp(mapX, 0, terrainData.alphamapWidth - 1);
            mapZ = Mathf.Clamp(mapZ, 0, terrainData.alphamapHeight - 1);
            float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);
            int dominantLayer = 0;
            float maxWeight = 0f;
            for (int i = 0; i < terrainData.alphamapLayers; i++)
            {
                float weight = splatmapData[0, 0, i];
                if (weight > maxWeight)
                {
                    maxWeight = weight;
                    dominantLayer = i;
                }
            }
            if (dominantLayer == targetLayer)
            {
                float worldX = terrainPos.x + randomX;
                float worldZ = terrainPos.z + randomZ;
                float worldY = terrain[index].SampleHeight(new Vector3(worldX, 0f, worldZ)) + terrainPos.y;

                return new Vector3(worldX, worldY+2, worldZ);
            }
        }
    }
    public int StringToInt(string input)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i<input.Length; i++)
        {
            sb.Append((((int)input[i]+127)%256).ToString());
        }
        return int.Parse(sb.ToString().Substring(0, Mathf.Min(9,sb.ToString().Length)));
    }
    void Awake()
    {
        UnityEngine.Random.InitState(StringToInt(PhotonNetwork.CurrentRoom.Name));
        transform.position = GetSpawnPosition(Terrain.activeTerrains, 0);
        PhotonNetwork.Instantiate(player.name, transform.position, Quaternion.identity);
    }
}
