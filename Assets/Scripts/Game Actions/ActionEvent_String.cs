using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEvent_String : MonoBehaviour
{
    [SerializeField] private GameAction_String gameAction;
    [SerializeField] private UnityEngine.Events.UnityEvent<string> unityEvent;

    private void OnEnable()
    {
        gameAction.action += EventInvoker;
    }

    private void OnDisable()
    {
        gameAction.action -= EventInvoker;
    }

    public void EventInvoker(string value)
    {
        unityEvent.Invoke(value);
    }
}
