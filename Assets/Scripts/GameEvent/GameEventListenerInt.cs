using UnityEngine;
using UnityEngine.Events;
public class GameEventListenerInt : MonoBehaviour
{
    public GameEventInt gameEvent;
    public UnityEvent<int> onEventTriggered;
    void OnEnable()
    {
        gameEvent.AddListener(this);
    }
    void OnDisable()
    {
        gameEvent.RemoveListener(this);
    }
    public void OnEventTriggered(int arg)
    {
        onEventTriggered.Invoke(arg);
    }
}
