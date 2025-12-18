using UnityEngine;
using UnityEngine.Events;

public class inputManager : MonoBehaviour
{
    [System.Serializable] public class data
    {
        public KeyCode key;
        public UnityEvent function;
    }
    [SerializeField] data[] inputs;

    void Update()
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            if (Input.GetKeyDown(inputs[i].key))
            {
                inputs[i].function.Invoke();
            }
        }
    }
}
