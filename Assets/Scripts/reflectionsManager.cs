

using UnityEngine;

public class reflectionsManager : MonoBehaviour
{
    [SerializeField] private GameObject[] probes;
    [SerializeField] private Transform cam;
    [SerializeField] private float radius;
    [SerializeField] private int refreshInterval;
    private int f;
    private void Update()
    {
        f++;
        if (f >= refreshInterval)
        {
            f = 0;
            for (int i = 0; i < probes.Length; i++) 
            {
                if ((probes[i].transform.position - cam.position).sqrMagnitude > radius * radius)
                {
                    probes[i].transform.position = new Vector3(
                        cam.position.x + Random.Range(-radius, radius),
                        cam.position.y,
                        cam.position.z + Random.Range(-radius, radius));
                }
            }
        }
    }
}
