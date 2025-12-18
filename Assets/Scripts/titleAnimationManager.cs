using UnityEngine;

public class titleAnimationManager : MonoBehaviour
{
    private Vector3 rot;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float intensityInput;
    [SerializeField] private float speed;
    private float intensity;

    private float Value(float x, float offset)
    {
        return Mathf.Sin((x * 2 * Mathf.PI) + offset) * intensity;
    }
    private Vector3 GetRotation(float time)
    {
        return new Vector3(Value(time * speed, 1f / 3f), Value(time * speed, 2f / 3f), Value(time * speed, 3f / 3f));
    }
    void Update()
    {
        intensity = intensityInput;
        rectTransform.rotation = Quaternion.Euler(GetRotation(Time.time));
    }
}
