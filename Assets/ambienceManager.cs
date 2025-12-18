using Simplex;
using UnityEngine;

public class ambienceManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private int spacing;
    [SerializeField] private float minDelay;
    private int prevClip;
    private float f;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("SETTINGS"))
        {
            audioSource.volume *= float.Parse(PlayerPrefs.GetString("SETTINGS").Split(" ")[3]);
        }
        else
        {
            audioSource.volume *= 0.5f;
        }
        prevClip = Random.Range(0, clips.Length);
        audioSource.PlayOneShot(clips[prevClip]);
    }
    private void Update()
    {
        f += Time.unscaledDeltaTime;
        if (f > minDelay)
        {
            if (audioSource.isPlaying) { return; }
            if (clips.Length == 1)
            {
                audioSource.PlayOneShot(clips[0]);
                return;
            }
            if (Random.Range(0, spacing) == 0)
            {
                int clip = prevClip;
                while (clip == prevClip)
                {
                    clip = Random.Range(0, clips.Length);
                }
                audioSource.PlayOneShot(clips[clip]);
                prevClip = clip;
                f = 0;
            }
        }
    }
}
