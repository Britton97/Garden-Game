using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionEvent_Bool : MonoBehaviour
{
    [SerializeField] private GameAction_Bool gameAction;
    [SerializeField] private UnityEvent<bool> unityEvent;

    private void OnEnable()
    {
        gameAction.action += EventInvoker;
    }

    private void OnDisable()
    {
        gameAction.action -= EventInvoker;
    }

    public void EventInvoker(bool value)
    {
        unityEvent.Invoke(value);
    }
}
