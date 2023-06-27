using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Game Event Vector3")]
public class GameEventVector3 : ScriptableObject
{
    private List<GameEventListenerVector3> listeners = new List<GameEventListenerVector3>();
    public void TriggerEvent(Vector3 arg)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventTriggered(arg);
        }
    }
    public void AddListener(GameEventListenerVector3 listener)
    {
        listeners.Add(listener);
    }
    public void RemoveListener(GameEventListenerVector3 listener)
    {
        listeners.Remove(listener);
    }
}