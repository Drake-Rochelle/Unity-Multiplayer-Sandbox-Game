using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EventListenerComponent : MonoBehaviour
{
    public List<GameEventListener> listeners = new List<GameEventListener>();

    private void OnEnable()
    {
        foreach (GameEventListener listener in listeners)
        {
            listener.Register();
        }
    }

    private void OnDisable()
    {
        foreach (GameEventListener listener in listeners)
        {
            listener.UnRegister();
        }
    }

}
