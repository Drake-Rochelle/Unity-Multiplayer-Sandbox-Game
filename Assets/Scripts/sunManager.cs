using UnityEngine;

public class sunManager : MonoBehaviour
{
    [SerializeField] private GameObject lightObj;
    private DayNightManager dayNightManager;
    private void Awake()
    {
        dayNightManager = GameObject.FindGameObjectWithTag("dayNightManager").GetComponent<DayNightManager>();
    }
    void Update()
    {
        lightObj.transform.rotation = Quaternion.Euler(dayNightManager.lightAngle, 0.0f, 0.0f);
    }
}
