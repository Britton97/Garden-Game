using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameAction_Float : ScriptableObject
{
    public System.Action<float> action;

    public void InvokeAction(float value)
    {
        action?.Invoke(value);
    }
}