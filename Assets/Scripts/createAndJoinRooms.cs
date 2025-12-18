using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using NUnit.Framework;
using Unity.VisualScripting;
using System.Collections.Generic;
public class createAndJoinRooms : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField createInput;
    [SerializeField] private TMP_InputField joinInput;
    [SerializeField] private GameObject button;
    [SerializeField] private Transform buttonParent;
    private string[] worlds;
    private List<GameObject> worldButtons;
    private GameObject worldButton;
    private string worldsString;
    private string text;

    private void Start()
    {
        worldButtons = new List<GameObject>();
        if (PlayerPrefs.HasKey("WORLDS"))
        {
            worldsString = PlayerPrefs.GetString("WORLDS");
            print(worldsString);
            worlds = worldsString.Split("\n");
            for (int i = 0; i < worlds.Length-1; i++)
            {
                worldButton = Instantiate(button, buttonParent);
                worldButton.GetComponentInChildren<TextMeshProUGUI>().text = worlds[i];
                worldButton.gameObject.name = worlds[i];
                worldButtons.Add(worldButton);
            }
        }
    }

    public void WorldClicked(Object sender, object data)
    {
        text = ((GameObject)data).name;
        CreateRoom();
    }

    public void OnCreateInputSubmit()
    {
        print("ogijerg");
        worldsString += createInput.text;
        worldsString += "\n";
        PlayerPrefs.SetString("WORLDS", worldsString);
        PlayerPrefs.Save();
        if (!string.IsNullOrEmpty(createInput.text))
        {
            text = createInput.text;
            CreateRoom();
        }
    }

    public void OnJoinInputSubmit()
    {
        if (!string.IsNullOrEmpty(joinInput.text))
        {
            text = joinInput.text;
            JoinRoom();
        }
    }   
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(text);
    }
    public void JoinRoom()
    {
        print("oeirgoinergerrgergegsergt");
        if (string.IsNullOrEmpty(text))
        {
            text = joinInput.text;
        }
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else if (PhotonNetwork.CurrentLobby == null)
        {
            PhotonNetwork.JoinLobby();
        }
        else
        {
            PhotonNetwork.JoinRoom(text);
        }
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        sceneTransitionManager.Instance.Scene("Game", true);

    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        base.OnConnectedToMaster();
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        PhotonNetwork.JoinRoom(text);
    }
}
