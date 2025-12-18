using UnityEditor;
using UnityEngine;

public class exitGame : MonoBehaviour
{
    public void OnClick()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }
}
