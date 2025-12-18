using UnityEngine;

public class blockAudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] sfx;
    private AudioSource sfxSource;
    private string[] str;
    private void Awake()
    {
        sfxSource = GetComponent<AudioSource>();
    }

    public void OnPlace(object data, int senderActorNumber)
    {
        SFX(data);
    }
    public void OnDelete(object data, int senderActorNumber)
    {
        SFX(data);
    }
    private void SFX(object data)
    {
        sfxSource.panStereo = Random.Range(-0.25f, 0.25f);
        sfxSource.pitch = Random.Range(0.5f, 1.5f);
        str = ((string)data).Split(" ");
        transform.position = new Vector3Int(int.Parse(str[1]), int.Parse(str[2]), int.Parse(str[3]));
        if (int.Parse(str[0]) >= str.Length)
        {
            sfxSource.PlayOneShot(sfx[0]);
            return;
        }
        sfxSource.PlayOneShot(sfx[int.Parse(str[0])]);
    }
}
