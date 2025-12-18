using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Use this base SO for any event that 
/// requires string data being passed.
/// </summary>


[CreateAssetMenu(menuName = "Events/Game Event")]
public class GameEventSO : ScriptableObject
{   

    [SerializeField] private List<GameEventListener> listeners = new List<GameEventListener>();
    [SerializeField] private bool consoleReport;
    public void RaiseEvent(Component sender, object data)
    {
        if (consoleReport)
            Debug.Log($"{sender} raising {this}");

        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i].OnEventRaised(sender, data); 
        }

    }

    public void RegisterListener(GameEventListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }
    public void UnRegisterListener(GameEventListener listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }
}