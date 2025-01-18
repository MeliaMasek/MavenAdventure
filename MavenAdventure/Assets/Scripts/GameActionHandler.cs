using UnityEngine;
using UnityEngine.Events;

public class GameActionHandler : MonoBehaviour
{
    public GameAction gameActionobj;
    public UnityEvent onRaiseEvent;
    private void Start()
    {
        gameActionobj.raise += Raise;
    }

    private void Raise()
    {
        onRaiseEvent.Invoke();
    }
}