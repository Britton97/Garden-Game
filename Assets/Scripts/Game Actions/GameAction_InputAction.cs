using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu]
[InlineEditor]
public class GameAction_InputAction : ScriptableObject
{
    public InputContext inputContext;
    public UnityAction<InputAction.CallbackContext> action;

    public void InvokeAction(InputAction.CallbackContext context)
    {
        if (inputContext == InputContext.Started && context.started)
        {
            action?.Invoke(context);
        }
        else if (inputContext == InputContext.Performed && context.performed)
        {
            action?.Invoke(context);
        }
        else if (inputContext == InputContext.Canceled && context.canceled)
        {
            action?.Invoke(context);
        }
    }
}

public enum InputContext
{
    Started,
    Performed,
    Canceled
}
