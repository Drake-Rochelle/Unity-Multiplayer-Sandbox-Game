using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomGameEvent : UnityEvent<Component, object>{ };

[System.Serializable]
public class GameEventListener
{
    [SerializeField] private GameEventSO gameEvent;
    [SerializeField] private CustomGameEvent response;
    public void Register()
    {
        gameEvent.RegisterListener(this);
    }

    public void UnRegister()
    {
        gameEvent.UnRegisterListener(this);
    }

    public void OnEventRaised(Component sender, object data)
    {
        response.Invoke(sender,data);
    }
}
