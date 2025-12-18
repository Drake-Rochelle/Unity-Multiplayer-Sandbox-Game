using UnityEngine;

public class DayNightManager : MonoBehaviour
{
    [SerializeField] private float timeSpeed = 1f;
    [HideInInspector] public float playerLight;
    [HideInInspector] public float lightAngle;
    public float time;
    [SerializeField] private bool setTime;

    public void SetTime(float time)
    {
        lightAngle = time * 360;
    }
    private void Update()
    {
        if (setTime)
        {
            SetTime(-0.25f);
        }
        lightAngle += timeSpeed * Time.deltaTime;
        time = (lightAngle % 360)/360;
        playerLight = Mathf.Max(-Mathf.Sin(time * 2 * Mathf.PI)*0.75f+0.25f,0);
    }
}
