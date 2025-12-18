using Unity.VisualScripting;
using UnityEngine;

public class playerLightManager : MonoBehaviour
{
    [SerializeField] private Light lightComponent;
    private DayNightManager dayNightManager;
    private float lightIntensity;
    private void Start()
    {
        while (dayNightManager.IsUnityNull())
        {
            dayNightManager = GameObject.FindAnyObjectByType<DayNightManager>();
        }
        print(dayNightManager);
        lightIntensity = lightComponent.intensity;
    }
    void Update()
    {
        lightComponent.intensity = dayNightManager.playerLight*lightIntensity;
    }
}
