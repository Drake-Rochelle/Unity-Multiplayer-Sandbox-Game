using UnityEngine;
using UnityEngine.SceneManagement;

public class menuAudioManager : MonoBehaviour
{
    [SerializeField] private int checkRate;
    private AudioSource audioSource;
    private int f;
    public static menuAudioManager Instance
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
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(this.gameObject);
            if (PlayerPrefs.HasKey("SETTINGS"))
            {
                audioSource.volume *= float.Parse(PlayerPrefs.GetString("SETTINGS").Split(" ")[3]);
            }
            else
            {
                audioSource.volume *= 0.5f;
            }
        }
    }
    private void Update()
    {
        f++;
        if (f > checkRate)
        {
            f = 0;
            if (SceneManager.GetActiveScene().name == "Game")
            {
                Destroy(this.gameObject);
            }
        }
    }
}
