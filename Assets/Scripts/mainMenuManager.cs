using UnityEngine.SceneManagement;
using Photon.Pun;
public class mainMenuManager : MonoBehaviourPunCallbacks
{
    public void OnPlayGame()
    {
        PhotonNetwork.OfflineMode = true;
        sceneTransitionManager.Instance.Scene("Play Game", false);
    }
    public void OnMultiplayer()
    {
        PhotonNetwork.OfflineMode = false;
        sceneTransitionManager.Instance.Scene("Multiplayer Loading", false);
    }
    public void OnSettings()
    {
        sceneTransitionManager.Instance.Scene("Settings",false);
    }
}
