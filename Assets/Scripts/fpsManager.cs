using TMPro;
using UnityEngine;

public class fpsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;
    private float f;
    void Update()
    {
        f+=Time.deltaTime;
        if (f > 0.5f)
        {
            f = 0;
            tmp.text = $"FPS: {Mathf.Min(Mathf.RoundToInt(1 / Time.smoothDeltaTime), 999999999)}";
        }
    }
}
