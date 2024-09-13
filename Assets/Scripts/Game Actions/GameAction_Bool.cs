using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameAction_Bool : ScriptableObject
{
    public System.Action<bool> action;

    public void InvokeAction(bool value)
    {
        action?.Invoke(value);
    }
}
