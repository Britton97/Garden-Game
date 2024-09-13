using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionEvent_Float : MonoBehaviour
{
    [SerializeField] private GameAction_Float gameAction;
    [SerializeField] private UnityEvent<float> unityEvent;

    private void OnEnable()
    {
        gameAction.action += EventInvoker;
    }

    private void OnDisable()
    {
        gameAction.action -= EventInvoker;
    }

    public void EventInvoker(float value)
    {
        unityEvent.Invoke(value);
    }
}
