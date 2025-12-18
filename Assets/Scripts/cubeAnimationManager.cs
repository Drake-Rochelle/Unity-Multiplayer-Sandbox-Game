using UnityEngine;

public class cubeAnimationManager : MonoBehaviour
{
    private Vector3 rot;
    [SerializeField] private Transform rectTransform;
    [SerializeField] private float speed;
    [SerializeField] private float rotSpeed;
    [SerializeField] private float intensity;
    private float i;
    private float a;
    private void Start()
    {
        i = rectTransform.position.y;
        a = rectTransform.position.x;
    }
    void Update()
    {
        rectTransform.position = new Vector3(a + Mathf.Cos(Time.time * speed) * intensity, i+Mathf.Sin(Time.time*speed)*intensity, rectTransform.position.z);
        rectTransform.rotation = Quaternion.Euler(0,Time.time*rotSpeed, 0);
    }
}
