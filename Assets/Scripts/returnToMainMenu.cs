using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
public class returnToMainMenu : MonoBehaviourPunCallbacks
{
    private const byte LEAVE_ROOM = 34;
    public void ReturnToMainMenu()
    {
        if (PhotonNetwork.CurrentRoom == null && PhotonNetwork.CurrentLobby == null)
        {
//            SceneManager.LoadScene("main menu");
            sceneTransitionManager.Instance.Scene("Main Menu", false);
        }
        Time.timeScale = 1.0f;
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.RaiseEvent(LEAVE_ROOM,(object)null, new RaiseEventOptions { Receivers = ReceiverGroup.Others},SendOptions.SendReliable);
        }
        if (PhotonNetwork.NetworkClientState != ClientState.Leaving)
        {
            PhotonNetwork.Disconnect();
        }
    }
    public void OnLeaveRoom()
    {
        PhotonNetwork.Disconnect();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
  //      SceneManager.LoadScene("Main Menu");
        sceneTransitionManager.Instance.Scene("Main Menu", false);
    }
}
