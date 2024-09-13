using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class GameAction_String : ScriptableObject
{
    public System.Action<string> action;

    public void InvokeAction(string value)
    {
        action?.Invoke(value);
    }
}
