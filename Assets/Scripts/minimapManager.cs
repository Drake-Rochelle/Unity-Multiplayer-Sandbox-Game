using Photon.Pun;
using UnityEngine;

public class minimapManager : MonoBehaviour
{
    [SerializeField] private PhotonView view;
    private Transform mapCam;
    private void Update()
    {
        GameObject mapCamGO = GameObject.FindGameObjectWithTag("mapCam");
        if (mapCamGO != null)
        {
            mapCam = mapCamGO.transform;
        }
        if (view.IsMine)
        {
            mapCam.transform.position = new Vector3(transform.position.x, mapCam.transform.position.y, transform.position.z);
            mapCam.transform.rotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, 0);
        }
    }
}
