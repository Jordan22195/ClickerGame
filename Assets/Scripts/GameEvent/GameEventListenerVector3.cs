using UnityEngine;
using UnityEngine.Events;
public class GameEventListenerVector3 : MonoBehaviour
{
    public GameEventVector3 gameEvent;
    public UnityEvent<Vector3> onEventTriggered;
    void OnEnable()
    {
        gameEvent.AddListener(this);
    }
    void OnDisable()
    {
        gameEvent.RemoveListener(this);
    }
    public void OnEventTriggered(Vector3 arg)
    {
        onEventTriggered.Invoke(arg);
    }
}
