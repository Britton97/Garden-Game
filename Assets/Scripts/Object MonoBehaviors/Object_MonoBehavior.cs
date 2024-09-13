using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Object_MonoBehavior : GardenObject_MonoBehavior, iTurnOnAndOffAble
{
    public Object_SO object_SO;
    private bool isOn = true;
    bool iTurnOnAndOffAble.IsOn => isOn;

    public UnityEvent turnOnEvent;
    public UnityEvent turnOffEvent;

    public override string GetName()
    {
        return object_SO.GardenObjectName;
    }

    public override int GetSellPrice()
    {
        throw new System.NotImplementedException();
    }

    public override Sprite GetSprite()
    {
        throw new System.NotImplementedException();
    }

    public override bool IsEdible()
    {
        return false;
    }

    public override bool IsSellable()
    {
        throw new System.NotImplementedException();
    }

    void iTurnOnAndOffAble.TurnOn()
    {
        isOn = true;
        turnOnEvent.Invoke();
    }

    void iTurnOnAndOffAble.TurnOff()
    {
        isOn = false;
        turnOffEvent.Invoke();
    }
}
