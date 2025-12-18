using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class fogManager : MonoBehaviour
{
    private HDAdditionalReflectionData rp;
    [SerializeField] private Material material;
    private int f;
    void Start()
    {
        rp = GetComponent<HDAdditionalReflectionData>();
        material.SetTexture("_Sky", rp.realtimeTexture);
    }
    private void Update()
    {
        if (f > 50)
        {
            f = 0;
            material.SetTexture("_Sky", rp.realtimeTexture);
        }
        f++;
    }
}
