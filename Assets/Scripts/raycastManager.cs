using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;


public class RaycastManager : MonoBehaviour
{
    [SerializeField] private int currBlock;
    [SerializeField] private Transform cam;
    [SerializeField] private LayerMask raycastLayers;
    [SerializeField] private GameObject[] blocks, previews;
    [SerializeField] private Vector3[] vectorCorrection;
    [SerializeField] private float offset = 0.5f, distance = 10;
    [SerializeField] private int chunkSize = 104857;
    [SerializeField] private bool deleteAllSaveData;
    private DayNightManager dayNightManager;
    private string world;
    private RaycastHit hit;
    private bool place, delete, outgoingRequest;
    private List<Vector3Int> blockPositions;
    private List<int> blockIDs;
    private List<Vector3Int> blockDirections;
    private List<int> inventoryIndexes;
    private List<int> inventoryCounts;
    private List<int> inventoryPositions;
    private string save;
    private List<string> saveArr;
    private string[] savePartitions;
    private PhotonView view;
    public const byte REQUEST_LEVEL_DATA = 30, SEND_LEVEL_DATA = 31, PLACE_BLOCK = 32, DELETE_BLOCK = 33, SEND_INVENTORY = 34;
    [SerializeField] private Transform player;
    private Vector2 camRot;
    [SerializeField] private Transform camXComponent;
    [SerializeField] private Transform camYComponent;
    [SerializeField] private FirstPersonLook lookScript;
    [SerializeField] private int inventorySize;
    private int f;
    private string UNIQUE_ACCOUNT_ID_DO_NOT_DELETE;
    private StringBuilder sb;
    private string ufw;


    private int GetOpenSlot()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            if (!inventoryPositions.Contains(i))
            {
                return i;
            }
        }
        return -1;
    }

    private Vector2 GetCamRot()
    {
        return new Vector2(camXComponent.transform.rotation.eulerAngles.x, camXComponent.transform.rotation.eulerAngles.y);
    }
    private void ApplyCamRot()
    {
        lookScript.SetCam(camRot);
    }
    private string Enc(string str)
    {
        sb.Clear();
        for (int i  = 0; i < str.Length; i++)
        {
            sb.Append(((int)((byte)str[i])).ToString());
        }
        return sb.ToString();
    }
    private string Dec(string str)
    {
        StringBuilder strOut = new StringBuilder();
        int index = 0;
        while (index < str.Length)
        {
            sb.Clear();
            sb.Append(str[index]);
            sb.Append(str[index+1]);
            if (str[index] == '1')
            {
                sb.Append(str[index+2]);
                strOut.Append((char)int.Parse(sb.ToString()));
                index += 3;
            }
            else
            {
                strOut.Append((char)int.Parse(sb.ToString()));
                index += 2;
            }
        }
        return strOut.ToString();
    }
    private void DoSomethingSpecial()
    {
        Debug.Log("Reward");
    }

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }
    private void Start()
    {
        inventoryIndexes = new List<int>();
        inventoryCounts = new List<int>();
        inventoryPositions = new List<int>();
        for (int i = 0; i < 10; i++)
        {
            inventoryIndexes.Add(i);
            inventoryCounts.Add(9999);
            inventoryPositions.Add(i);
        }
        sb = new StringBuilder();
        if (!PlayerPrefs.HasKey("WHAT DOES IT MEAN?!?!?!?!"))
        {
            PlayerPrefs.SetString("WHAT DOES IT MEAN?!?!?!?!","7210110810811132" +Enc(UnityEngine.SystemInfo.deviceName)+ "333273391093211610410132651149810511610111446328011711632116104105115321051103211610410132114101103105115116114121321021111143297321141011199711410033");
        }
        else
        {
            if (PlayerPrefs.GetString("WHAT DOES IT MEAN?!?!?!?!")==Dec("7210110810811132" + Enc(UnityEngine.SystemInfo.deviceName) + "333273391093211610410132651149810511610111446328011711632116104105115321051103211610410132114101103105115116114121321021111143297321141011199711410033"))
            {
                DoSomethingSpecial();
            }
        }
        if (PlayerPrefs.HasKey("UNIQUE_ACCOUNT_ID_DO_NOT_DELETE"))
        {
            UNIQUE_ACCOUNT_ID_DO_NOT_DELETE = PlayerPrefs.GetString("UNIQUE_ACCOUNT_ID_DO_NOT_DELETE");
        }
        else
        {
            sb.Clear();
            System.Random rng = new System.Random((int)(Time.timeSinceLevelLoadAsDouble*100000000));
            //Is this absurd, yes. Do I care, no. Do I f*** around with my player's privacy, no.
            for (int i = 0; i < 2048; i++)
            {
                int c = rng.Next(0, 3);
                switch (c)
                {
                    case 0:
                        sb.Append((char)rng.Next(174, 256));
                        break;
                    case 1:
                        sb.Append((char)rng.Next(161, 173));
                        break;
                    case 2:
                        sb.Append(((char)rng.Next(33, 127)).ToString().Replace(":", "a")[0]);
                        break;
                }
            }
            UNIQUE_ACCOUNT_ID_DO_NOT_DELETE = sb.ToString();
            PlayerPrefs.SetString("UNIQUE_ACCOUNT_ID_DO_NOT_DELETE", UNIQUE_ACCOUNT_ID_DO_NOT_DELETE);
        }
        while (dayNightManager.IsUnityNull())
        {
            dayNightManager = GameObject.FindAnyObjectByType<DayNightManager>();
        }
        world = PhotonNetwork.CurrentRoom.Name;
        if (world.Contains("_"))
        {
            world = world.Split("_")[0];            
        }
        blockPositions = new List<Vector3Int>();
        blockIDs = new List<int>();
        blockDirections = new List<Vector3Int>();
        if (PhotonNetwork.IsMasterClient) 
        {
            if (PlayerPrefs.HasKey(world + "0"))
            {
                if (PlayerPrefs.HasKey(world + "invs0"))
                {
                    //pref world+invs is of format [owner ID] [key to inv] [owner ID] [key to inv] [owner ID] [key to inv]...
                    string invString = string.Empty;
                    int invStringIndex = 0;
                    while (PlayerPrefs.HasKey(world + "invs" + invStringIndex.ToString()))
                    {
                        invString += PlayerPrefs.GetString(world + "invs" + invStringIndex.ToString());
                        invStringIndex++;
                    }
                    int inv_index = invString.Split(' ').ToList().IndexOf(UNIQUE_ACCOUNT_ID_DO_NOT_DELETE);
                    if (inv_index != -1)
                    {
                        string[] invArr = PlayerPrefs.GetString(invString.Split(' ')[inv_index+1]).Split(' ');
                        inventoryIndexes = new List<int>();
                        inventoryCounts = new List<int>();
                        inventoryPositions = new List<int>();
                        if (invArr.Length > 2)
                        {
                            for (int i = 0; i < invArr.Length; i += 3)
                            {
                                int index1 = int.Parse(invArr[i]);
                                int count = int.Parse(invArr[i + 1]);
                                int position = int.Parse(invArr[i + 2]);
                                inventoryIndexes.Add(index1);
                                inventoryCounts.Add(count);
                                inventoryPositions.Add(position);
                            }
                        }
                    }
                }
                SendInventory();
                save = string.Empty;
                int index = 0;
                while (PlayerPrefs.GetString(world + index.ToString(), "abc") != "abc")
                {
                    save += PlayerPrefs.GetString(world + index.ToString());
                    index++;
                }
                saveArr = new List<string>(save.Split(' '));
                player.transform.position = new Vector3(float.Parse(saveArr[0]), float.Parse(saveArr[1]), float.Parse(saveArr[2]));
                camRot = new Vector2(float.Parse(saveArr[3]), float.Parse(saveArr[4]));
                ApplyCamRot();
                dayNightManager.SetTime(float.Parse(saveArr[5]));
                currBlock = int.Parse(saveArr[6]);
                for (int i = 0; i <7 ; i++)
                {
                    saveArr.RemoveAt(0);
                }
                for (int i = 0; i < saveArr.Count - 1; i += 7)
                {
                    blockPositions.Add(new Vector3Int(int.Parse(saveArr[i + 1]), int.Parse(saveArr[i + 2]), int.Parse(saveArr[i + 3])));
                    blockIDs.Add(int.Parse(saveArr[i]));
                    blockDirections.Add(new Vector3Int(int.Parse(saveArr[i + 4]), int.Parse(saveArr[i + 5]), int.Parse(saveArr[i + 6])));
                    Instantiate(blocks[int.Parse(saveArr[i])], new Vector3Int(int.Parse(saveArr[i+1]), int.Parse(saveArr[i + 2]), int.Parse(saveArr[i + 3])), blocks[int.Parse(saveArr[i])].transform.rotation * Quaternion.LookRotation(new Vector3Int(int.Parse(saveArr[i + 4]), int.Parse(saveArr[i + 5]), int.Parse(saveArr[i + 6])))).transform.SetParent(transform.parent.parent);
                }
            }
            for (int i = 0; i < previews.Length; i++)
            {
                if (previews[i].GetComponent<Renderer>() != null)
                {
                    previews[i].GetComponent<Renderer>().enabled = false;
                }
                if (previews[i].GetComponentInChildren<Renderer>() != null)
                {
                    previews[i].GetComponentInChildren<Renderer>().enabled = false;
                }
            }
            if (view.IsMine && currBlock != -1)
            {
                if (previews[currBlock].GetComponent<Renderer>() != null)
                {
                    previews[currBlock].GetComponent<Renderer>().enabled = false;
                }
                if (previews[currBlock].GetComponentInChildren<Renderer>() != null)
                {
                    previews[currBlock].GetComponentInChildren<Renderer>().enabled = false;
                }
            }
        }
        else
        {
            outgoingRequest = true;
            PhotonNetwork.RaiseEvent(REQUEST_LEVEL_DATA, (object)UNIQUE_ACCOUNT_ID_DO_NOT_DELETE, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
        }
    }
    public void OnEvent1(object data, int senderActorNumber)
    {
        inventoryIndexes = new List<int>();
        inventoryCounts = new List<int>();
        inventoryPositions = new List<int>();
        inventoryIndexes.Add(0);
        inventoryCounts.Add(5);
        inventoryPositions.Add(0);
        if (outgoingRequest)
        {
            outgoingRequest = false;
            if (!string.IsNullOrEmpty(data as string))
            {
                string[] dtArr = ((string)data).Split(":");
                string dt = dtArr[0];
                string inv;
                if (dtArr.Length > 1)
                {
                    inv = dtArr[1];
                }
                else
                {
                    inv = string.Empty;
                }
                if (inv != string.Empty)
                {
                    string[] invArr = inv.Split(' ');
                    inventoryIndexes = new List<int>();
                    inventoryCounts = new List<int>();
                    inventoryPositions = new List<int>();
                    if (invArr.Length > 2)
                    {
                        for (int i = 0; i < invArr.Length; i += 3)
                        {
                            int index = int.Parse(invArr[i]);
                            int count = int.Parse(invArr[i + 1]);
                            int position = int.Parse(invArr[i + 2]);
                            inventoryIndexes.Add(index);
                            inventoryCounts.Add(count);
                            inventoryPositions.Add(position);
                        }
                    }
                }
                SendInventory();
                saveArr = new List<string>(dt.Split(' '));
                player.transform.position = new Vector3(float.Parse(saveArr[0]), float.Parse(saveArr[1]), float.Parse(saveArr[2]));
                camRot = new Vector2(float.Parse(saveArr[3]), float.Parse(saveArr[4]));
                ApplyCamRot();
                dayNightManager.SetTime(float.Parse(saveArr[5]));
                currBlock = int.Parse(saveArr[6]);
                for (int i = 0; i < 7; i++)
                {
                    saveArr.RemoveAt(0);
                }

                for (int i = 0; i < saveArr.Count - 1; i += 7)
                {
                    blockPositions.Add(new Vector3Int(int.Parse(saveArr[i + 1]), int.Parse(saveArr[i + 2]), int.Parse(saveArr[i + 3])));
                    blockIDs.Add(int.Parse(saveArr[i]));
                    blockDirections.Add(new Vector3Int(int.Parse(saveArr[i + 4]), int.Parse(saveArr[i + 5]), int.Parse(saveArr[i + 6])));
                    Instantiate(blocks[int.Parse(saveArr[i])], new Vector3Int(int.Parse(saveArr[i+1]), int.Parse(saveArr[i + 2]), int.Parse(saveArr[i + 3])), blocks[int.Parse(saveArr[i])].transform.rotation * Quaternion.LookRotation(new Vector3Int(int.Parse(saveArr[i + 4]), int.Parse(saveArr[i + 5]), int.Parse(saveArr[i + 6])))).transform.SetParent(transform.parent.parent);
                }
            }
            for (int i = 0; i < previews.Length; i++)
            {
                if (previews[i].GetComponent<Renderer>() != null)
                {
                    previews[i].GetComponent<Renderer>().enabled = false;
                }
                if (previews[i].GetComponentInChildren<Renderer>() != null)
                {
                    previews[i].GetComponentInChildren<Renderer>().enabled = false;
                }
            }
            if (view.IsMine && currBlock != -1)
            {
                if (previews[currBlock].GetComponent<Renderer>() != null)
                {
                    previews[currBlock].GetComponent<Renderer>().enabled = false;
                }
                if (previews[currBlock].GetComponentInChildren<Renderer>() != null)
                {
                    previews[currBlock].GetComponentInChildren<Renderer>().enabled = false;
                }
            }
        }
    }
    public void OnEvent0(object data, int senderActorNumber)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            string USER_ID = (string)data;
            if (PlayerPrefs.HasKey(world + "0"))
            {
                save = string.Empty;
                int index = 0;
                while (PlayerPrefs.GetString(world + index.ToString(), "abc") != "abc")
                {
                    save += PlayerPrefs.GetString(world + index.ToString());
                    index++;
                }
                if (PlayerPrefs.HasKey(world + "invs"))
                {
                    int invsStringIndex = 0;
                    string invsString = string.Empty;
                    while (PlayerPrefs.HasKey(world + "invs" + invsStringIndex.ToString()))
                    {
                        invsString += PlayerPrefs.GetString(world + "invs" + invsStringIndex.ToString());
                        invsStringIndex++;
                    }
                    //pref world+invs is of format [owner ID] [key to inv] [owner ID] [key to inv] [owner ID] [key to inv]...
                    int inv_index = invsString.Split(' ').ToList().IndexOf(USER_ID);
                    if (inv_index != -1)
                    {
                        save += ":";
                        save += PlayerPrefs.GetString(invsString.Split(' ')[inv_index + 1]);
                    }
                }
            }
            else
            {
                save = string.Empty;
            }
            RaiseEventOptions options = new RaiseEventOptions
            {
                TargetActors = new int[] { senderActorNumber }
            };
            PhotonNetwork.RaiseEvent(SEND_LEVEL_DATA, (object)save, options, SendOptions.SendReliable);
        }
    }
    public void OnEvent2(object data, int senderActorNumber)
    {
        string str = (string)data;
        string[] strArr = str.Split(' ');
        Vector3Int newPos = new Vector3Int(int.Parse(strArr[1]), int.Parse(strArr[2]), int.Parse(strArr[3]));
        blockPositions.Add(newPos);
        blockIDs.Add(int.Parse(strArr[0]));
        blockDirections.Add(new Vector3Int(int.Parse(strArr[4]), int.Parse(strArr[5]), int.Parse(strArr[6])));
        UpdateSave();
        Instantiate(blocks[int.Parse(strArr[0])], (Vector3)newPos, blocks[int.Parse(strArr[0])].transform.rotation * Quaternion.LookRotation(new Vector3(int.Parse(strArr[4]), int.Parse(strArr[5]), int.Parse(strArr[6]))));
    }
    public void OnEvent3(object data, int senderActorNumber)
    {
        string str = (string)data;
        string[] strArr = str.Split(' ');
        Vector3Int newPos = new Vector3Int(int.Parse(strArr[1]), int.Parse(strArr[2]), int.Parse(strArr[3]));
        if (blockPositions.Contains(new Vector3Int(Mathf.RoundToInt(newPos.x), Mathf.RoundToInt(newPos.y), Mathf.RoundToInt(newPos.z))))
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("deletable");
            for (int i = 0; i < objs.Length; i++)
            {
                if (new Vector3Int(Mathf.RoundToInt(objs[i].transform.position.x), Mathf.RoundToInt(objs[i].transform.position.y), Mathf.RoundToInt(objs[i].transform.position.z)) == newPos)
                {
                    Destroy(objs[i]);
                    int index = blockPositions.IndexOf(new Vector3Int(Mathf.RoundToInt(newPos.x), Mathf.RoundToInt(newPos.y), Mathf.RoundToInt(newPos.z)));
                    if (index != -1)
                    {
                        blockPositions.RemoveAt(index);
                        blockIDs.RemoveAt(index);
                        blockDirections.RemoveAt(index);
                        UpdateSave();
                        continue;
                    }
                }
            }
        }
    }
    public void OnEvent4(object data, int senderActorNumber)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            string sender = ((string)data).Split(":")[0];
            string inv = ((string)data).Split(":")[1];
            int index = 0;
            string invsString = string.Empty;
            while (PlayerPrefs.HasKey(world + "invs"+index.ToString()))
            {
                invsString += PlayerPrefs.GetString(world + "invs"+index.ToString(),"abc");
                index++;
            }
            if (invsString == string.Empty)
            {
                invsString = "abc";
            }
            List<string> invs = invsString.Split(" ").ToList();
            string save_inv = string.Empty;
            if (invs.Contains(sender))
            {
                save_inv = invs[invs.ToList().IndexOf(sender) + 1];
            }
            else
            {
                save_inv = world+"inv_0";
                if (invs.Count > 0)
                {
                    if (invs[0] != "abc")
                    {
                        save_inv = invs[invs.Count - 1];
                        save_inv = save_inv.Split("_")[0] + "_" + (int.Parse(save_inv.Split("_")[1]) + 1).ToString();
                    }
                }
                if (invs[0] == "abc")
                {
                    invs = new List<string>();
                }
                invs.Add(sender);
                invs.Add(save_inv);
                sb.Clear();
                for (int i = 0; i < invs.Count; i++)
                {
                    sb.Append(invs[i]);
                    if (i != invs.Count - 1)
                    {
                        sb.Append(' ');
                    }
                }
                string[] invStrings = SplitByLength(sb.ToString(),chunkSize);
                for (int i = 0; i <  invStrings.Length; i++)
                {
                    PlayerPrefs.SetString(world + "invs"+i.ToString(), invStrings[i]);
                }
            }
            PlayerPrefs.SetString(save_inv, inv);
        }
    }
    private void SendInventory()
    {
        sb.Clear();
        sb.Append(UNIQUE_ACCOUNT_ID_DO_NOT_DELETE);
        sb.Append(":");
        for (int i = 0; i < inventoryIndexes.Count; i++)
        {
            sb.Append(inventoryIndexes[i]);
            sb.Append(" ");
            sb.Append(inventoryCounts[i]);
            sb.Append(" ");
            sb.Append(inventoryPositions[i]);
            if (i != inventoryIndexes.Count - 1)
            {
                sb.Append(" ");
            }
        }
        PhotonNetwork.RaiseEvent(SEND_INVENTORY, (object)sb.ToString(), new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendReliable);
    }

    private string[] SplitByLength(string input, int chunkSize)
    {
        int chunkCount = Mathf.CeilToInt((float)input.Length / chunkSize);
        string[] result = new string[chunkCount];
        for (int i = 0; i < chunkCount; i++)
        {
            int startIndex = i * chunkSize;
            int length = Mathf.Min(chunkSize, input.Length - startIndex);
            result[i] = input.Substring(startIndex, length);
        }
        return result;
    }


    private void UpdateSave()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            sb.Clear();
            sb.Append(player.transform.position.x.ToString() + " ");
            sb.Append(player.transform.position.y.ToString() + " ");
            sb.Append(player.transform.position.z.ToString() + " ");
            sb.Append(GetCamRot().x.ToString() + " ");
            sb.Append(GetCamRot().y.ToString() + " ");
            sb.Append(dayNightManager.time.ToString() + " ");
            sb.Append(currBlock.ToString() + " ");
            for (int i = 0; i < blockPositions.Count; i++)
            {
                sb.Append(blockIDs[i].ToString() + " ");
                sb.Append(blockPositions[i].x.ToString() + " ");
                sb.Append(blockPositions[i].y.ToString() + " ");
                sb.Append(blockPositions[i].z.ToString() + " ");
                sb.Append(blockDirections[i].x.ToString() + " ");
                sb.Append(blockDirections[i].y.ToString() + " ");
                sb.Append(blockDirections[i].z.ToString() + " ");
            }
            save = sb.ToString();
            savePartitions = SplitByLength(save, chunkSize);
            for (int i = 0; i < savePartitions.Length; i++)
            {
                PlayerPrefs.SetString(world + i.ToString(), savePartitions[i]);
            }
            PlayerPrefs.DeleteKey(world + savePartitions.Length);
            PlayerPrefs.Save();
        }
    }
    private void AddBlock(GameObject block)
    {
        int index = -1;
        for (int i =  0; i < blocks.Length; i++)
        {
            if (blocks[i].name == block.name.Split("(Clone)")[0])
            {
                index = i; 
                break;
            }
        }
        if (index == -1) { return; }
        int invIndex = inventoryIndexes.IndexOf(index);
        if (invIndex != -1)
        {
            inventoryCounts[invIndex]++;
        }
        else
        {
            inventoryIndexes.Add(index);
            inventoryCounts.Add(1);
            if (inventoryPositions.Count > 0)
            {
                inventoryPositions.Add(GetOpenSlot());
            }
            else
            {
                inventoryPositions.Add(0);
            }
        }
        SendInventory();
    }
    private void RemoveBlock(int index)
    {
        if (inventoryIndexes.Contains(index))
        {
            inventoryCounts[inventoryIndexes.IndexOf(index)]--;
            if (inventoryCounts[inventoryIndexes.IndexOf(index)] == 0)
            {
                inventoryCounts.RemoveAt(inventoryIndexes.IndexOf(index));
                inventoryPositions.RemoveAt(inventoryIndexes.IndexOf(index));
                inventoryIndexes.RemoveAt(inventoryIndexes.IndexOf(index));
            }
            SendInventory();
        }
    }
    public void OnPlace()
    {
        if (currBlock != -1)
        {
            place = true;
        }
    }
    public void OnDelete()
    {
        delete = true;
    }
    private void SetCurrentBlock()
    {
        if (Input.anyKeyDown)
        {
            int place = -2;
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                place = 0;
            } else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                place = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                place = 2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                place = 3;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                place = 4;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                place = 5;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                place = 6;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                place = 7;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                place = 8;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                place = 9;
            }
            if (place == currBlock)
            {
                currBlock = -1;
            }
            else if (place!=-2)
            {
                currBlock = place;
            }
        }
    }

    private Vector3 Quantize(Vector3 a)
    {
        a = a.normalized;
        if (Mathf.Abs(a.x) < Mathf.Sqrt(2) / 2)
        {
            a.x = 0;
        }
        else
        {
            a.x = vectorCorrection[currBlock].x * Mathf.Sign(a.x);
        }
        if (Mathf.Abs(a.y) < Mathf.Sqrt(2) / 2)
        {
            a.y = 0;
        }
        else
        {
            a.y = vectorCorrection[currBlock].y * Mathf.Sign(a.y);
        }
        if (Mathf.Abs(a.z) < Mathf.Sqrt(2) / 2)
        {
            a.z = 0;
        }
        else
        {
            a.z = vectorCorrection[currBlock].z * Mathf.Sign(a.z);
        }
        return a;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UpdateSave();
        }
        f++;
        if (f > 1/Time.unscaledDeltaTime)
        {
            f = 0;
            UpdateSave();
        }
        SetCurrentBlock();
        if (view.IsMine&&Time.timeScale!=0)
        {
            if (deleteAllSaveData)
            {
                deleteAllSaveData = false;
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
            }
            for (int i = 0; i < previews.Length; i++)
            {
                if (previews[i].GetComponent<Renderer>() != null)
                {
                    previews[i].GetComponent<Renderer>().enabled = false;
                }
                if (previews[i].GetComponentInChildren<Renderer>() != null)
                {
                    previews[i].GetComponentInChildren<Renderer>().enabled = false;
                }
                previews[i].SetActive(false);
            }
            if ((currBlock >= 0))
            {
                if (Physics.Raycast(cam.position, cam.forward, out hit, distance, raycastLayers))
                {
                    previews[currBlock].SetActive(true);
                    if (previews[currBlock].GetComponent<Renderer>() != null)
                    {
                        previews[currBlock].GetComponent<Renderer>().enabled = true;
                    }
                    if (previews[currBlock].GetComponentInChildren<Renderer>() != null)
                    {
                        previews[currBlock].GetComponentInChildren<Renderer>().enabled = true;
                    }
                    previews[currBlock].transform.position = new Vector3Int
                    (
                        Mathf.RoundToInt(hit.point.x + (hit.normal.x * offset)),
                        Mathf.RoundToInt(hit.point.y + (hit.normal.y * offset)),
                        Mathf.RoundToInt(hit.point.z + (hit.normal.z * offset))
                    );
                    previews[currBlock].transform.rotation = blocks[currBlock].transform.rotation * Quaternion.LookRotation(Quantize(hit.normal));
                    if (place)
                    {
                        place = false;
                        if (Time.timeScale != 0)
                        {
                            if (inventoryIndexes.Contains(currBlock))
                            {
                                Vector3Int newPos = new Vector3Int(Mathf.RoundToInt(previews[currBlock].transform.position.x), Mathf.RoundToInt(previews[currBlock].transform.position.y), Mathf.RoundToInt(previews[currBlock].transform.position.z));

                                if (!blockPositions.Contains(newPos))
                                {
                                    RemoveBlock(currBlock);
                                    PhotonNetwork.RaiseEvent(PLACE_BLOCK, (object)$"{currBlock} {newPos.x} {newPos.y} {newPos.z} {Quantize(hit.normal).x} {Quantize(hit.normal).y} {Quantize(hit.normal).z}", new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
                                }
                            }
                        }
                    }
                    if (delete)
                    {
                        delete = false;
                        if (Time.timeScale != 0)
                        {
                            if (hit.collider.gameObject.CompareTag("deletable"))
                            {
                                AddBlock(hit.collider.gameObject);
                                Vector3Int newPos = new Vector3Int(Mathf.RoundToInt(hit.collider.gameObject.transform.position.x), Mathf.RoundToInt(hit.collider.gameObject.transform.position.y), Mathf.RoundToInt(hit.collider.gameObject.transform.position.z));
                                PhotonNetwork.RaiseEvent(DELETE_BLOCK, (object)$"{currBlock} {newPos.x} {newPos.y} {newPos.z}", new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
                            }
                        }
                    }
                }
            }
        }
        place = false;
        delete = false;
    }
}
