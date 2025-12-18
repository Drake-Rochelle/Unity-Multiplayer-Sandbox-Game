using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class settingsManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolution;
    [SerializeField] private TMP_Dropdown windowMode;
    [SerializeField] private Slider sfxVolume;
    [SerializeField] private Slider ambienceVolume;
    private FullScreenMode fullScreenMode;
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("SETTINGS")) { return; }
        string[] settings = PlayerPrefs.GetString("SETTINGS").Split(" ");
        resolution.value = int.Parse(settings[0]);
        windowMode.value = int.Parse(settings[1]);
        sfxVolume.value = float.Parse(settings[2]);
        ambienceVolume.value = float.Parse(settings[3]);
    }

    private void UpdateSaveData()
    {
        string settings = resolution.value + " " + windowMode.value + " " + sfxVolume.value + " " + ambienceVolume.value;
        PlayerPrefs.SetString("SETTINGS", settings);
    }
    public void OnVolumeChange()
    {
        UpdateSaveData();
    }
    public void OnWindowModeChange()
    {
        UpdateSaveData();
        switch (windowMode.value)
        {
            case 0:
                fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 1:
                fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 2:
                fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
            case 3:
                fullScreenMode = FullScreenMode.Windowed;
                break;
        }
        Screen.SetResolution(Screen.width, Screen.height,fullScreenMode);
    }
    public void OnResolutionChange()
    {
        UpdateSaveData();
        switch (windowMode.value)
        {
            case 0:
                fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 1:
                fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 2:
                fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
            case 3:
                fullScreenMode = FullScreenMode.Windowed;
                break;
        }
        if (resolution.value == 0)
        {
            Screen.SetResolution(Screen.resolutions[0].width, Screen.resolutions[0].height,fullScreenMode);
            return;
        }
        Screen.SetResolution(int.Parse(resolution.options[resolution.value].text.Split("x")[0]), int.Parse(resolution.options[resolution.value].text.Split("x")[1]), fullScreenMode);
    }
}
