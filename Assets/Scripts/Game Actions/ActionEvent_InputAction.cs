using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ActionEvent_InputAction : MonoBehaviour
{
    [SerializeField] private GameAction_InputAction gameAction;
    [SerializeField] private UnityEvent<InputAction.CallbackContext> unityEvent;

    private void OnEnable()
    {
        gameAction.action += EventInvoker;
    }

    private void OnDisable()
    {
        gameAction.action -= EventInvoker;
    }

    public void EventInvoker(InputAction.CallbackContext context)
    {
        unityEvent.Invoke(context);
    }

    public void DebugMessage(string message)
    {
        Debug.Log(message);
    }
}
