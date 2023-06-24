using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Game Event Int")]
public class GameEventInt : ScriptableObject
{
    private List<GameEventListenerInt> listeners = new List<GameEventListenerInt>();
    public void TriggerEvent(int arg)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventTriggered(arg);
        }
    }
    public void AddListener(GameEventListenerInt listener)
    {
        listeners.Add(listener);
    }
    public void RemoveListener(GameEventListenerInt listener)
    {
        listeners.Remove(listener);
    }
}