using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

public class PhotonEventListenerComponent : MonoBehaviour, IOnEventCallback
{
    [System.Serializable]
    public class Lstnr
    {
        [SerializeField] public int eventCode;
        [SerializeField] public UnityEvent<object, int> response;
        [SerializeField] public bool consoleReport;
        public void Invoke(object payload, int sender)
        {
            response.Invoke(payload, sender);
        }
    }
    [SerializeField] private Lstnr[] listeners;
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    public void OnEvent(EventData photonEvent)
    {
        int eventCode = photonEvent.Code;
        object data = photonEvent.CustomData;
        int sender = photonEvent.Sender;
        for (int l = 0; l < listeners.Length; l++)
        {
            if (listeners[l].consoleReport && eventCode < 200 && listeners[l].eventCode==eventCode)
            {
                Debug.Log($"Photon Event received by listener {l} in " + transform.gameObject.name + $": Code={eventCode}, Sender={sender}");
            }
            if (listeners[l].eventCode == eventCode)
            {
                listeners[l].Invoke(data, sender);
            }
        }
    }
}

