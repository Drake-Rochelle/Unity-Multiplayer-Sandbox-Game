using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class sceneTransitionManager : MonoBehaviour
{
    [SerializeField] private Vector3 fadeInHoldFadeOut;
    [SerializeField] private Image fade;
    public static sceneTransitionManager Instance
    {
        get; private set;
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
        FadeIn();
    }
    public bool Scene(string name, bool global)
    {
        if (Application.CanStreamedLevelBeLoaded(name))
        {
            StartCoroutine(LoadScene(name, global));
            return true;
        }
        return false;
    }
    private IEnumerator FadeIn()
    {
        float timer = 0;
        while (timer < fadeInHoldFadeOut.z)
        {
            timer += Time.unscaledDeltaTime;
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1 - (timer / fadeInHoldFadeOut.z));
            yield return null;
        }
    }
    private IEnumerator LoadScene(string name, bool global)
    {
        float timer = 0;
        while (timer < fadeInHoldFadeOut.x)
        {
            timer += Time.unscaledDeltaTime;
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b,timer/fadeInHoldFadeOut.x);
            yield return null;
        }
        timer = 0;
        if (global)
        {
            PhotonNetwork.LoadLevel(name);
        }
        else
        {
            SceneManager.LoadScene(name);
        }
        while (SceneManager.GetActiveScene().name != name)
        {
            yield return null;
        }
        while (timer < fadeInHoldFadeOut.y)
        {
            timer += Time.unscaledDeltaTime;
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1);
            yield return null;
        }
        timer = 0;
        while (timer<fadeInHoldFadeOut.z)
        {
            timer += Time.unscaledDeltaTime;
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1-(timer / fadeInHoldFadeOut.z));
            yield return null;
        }
    }
}
