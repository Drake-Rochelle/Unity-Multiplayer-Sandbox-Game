using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField createInput;
    public void CreateRoom()
    {
        if (!createInput.text.Contains("_"))
        {
            PhotonNetwork.CreateRoom($"{createInput.text}_{Random.Range(0,999999999)}");
        }
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel("Game");
        sceneTransitionManager.Instance.Scene("Game", true);
    }
}
