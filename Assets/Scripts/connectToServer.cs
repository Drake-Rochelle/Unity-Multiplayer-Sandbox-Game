using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class connectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] private int maxTries = 5;
    private int tries;
    private void Awake()
    {
        if (!(Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) || PhotonNetwork.OfflineMode)
        {
            PhotonNetwork.OfflineMode = true;
            sceneTransitionManager.Instance.Scene("Failed To Connect", false);
            return;
        }
        if (PhotonNetwork.IsConnected)
        {
            OnConnectedToMaster();
        }
        else
        {
            tries++;
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        sceneTransitionManager.Instance.Scene("Lobby", false);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        if (tries < maxTries)
        {
            tries++;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PhotonNetwork.OfflineMode = true;
            sceneTransitionManager.Instance.Scene("Failed To Connect", false);
        }
    }
}
