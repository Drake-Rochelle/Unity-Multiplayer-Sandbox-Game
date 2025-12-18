using UnityEngine;

public class windManager : MonoBehaviour
{
    [SerializeField] private AudioSource windSource;
    [SerializeField] private AudioClip wind;
    private float windTime;
    public static windManager Instance
    {
        get; private set;
    }
    private void Awake()
    {

        if (Instance != null)
        {
            Debug.Log("More than one " + this.name + ", ya chump");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            windSource.PlayOneShot(wind);
            windSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(this.gameObject);
            if (PlayerPrefs.HasKey("SETTINGS"))
            {
                windSource.volume = float.Parse(PlayerPrefs.GetString("SETTINGS").Split(" ")[3]);
            }
            else
            {
                windSource.volume = 0.5f;
            }
        }
    }
    void Update()
    {
        windTime += Time.unscaledDeltaTime;
        if (windTime > wind.length - 12)
        {
            windTime = 0;
            windSource.PlayOneShot(wind);
        }
    }
}
