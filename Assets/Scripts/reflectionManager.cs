using UnityEngine;
public class reflectionManager : MonoBehaviour
{
    [SerializeField] private ReflectionProbe reflectionProbe;
    [SerializeField] private int refreshInterval;
    private int f;
    void Update()
    {
        f++;
        if (f >= refreshInterval)
        {
            f = 0;
            reflectionProbe.RenderProbe();
        }
    }
}
