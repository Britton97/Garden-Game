using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PickUp", menuName = "Interactable/Turn on and off")]
public class Interactable_TurnOnAndOff : Interactable_Abs
{
    public override void HoverEnterWhileSelected(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {

    }

    public override void HoverExitWhileSelected(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        throw new System.NotImplementedException();
    }

    public override void OnDeselect(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        throw new System.NotImplementedException();
    }

    public override bool OnHoverEnter(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        if(selectedObject is iTurnOnAndOffAble)
        {
            UIButtonState.InvokeAction(true);
            bool isOn = (selectedObject as iTurnOnAndOffAble).IsOn;
            if(isOn)
            {
                UIButtonText.InvokeAction("Turn Off");
            }
            else
            {
                UIButtonText.InvokeAction("Turn On");
            }
            //UIButtonText.InvokeAction(hoverText);
            return true;
        }
        return false;
    }

    public override void OnHoverExit(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        if(selectedObject is iTurnOnAndOffAble)
        {
            UIButtonState.InvokeAction(false);
            UIButtonText.InvokeAction("Null");
        }
    }

    public override bool OnSelect(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        if(selectedObject is iTurnOnAndOffAble)
        {
            if((selectedObject as iTurnOnAndOffAble).IsOn)
            {
                (selectedObject as iTurnOnAndOffAble).TurnOff();
            }
            else
            {
                (selectedObject as iTurnOnAndOffAble).TurnOn();
            }
            return false;
        }
        return false;
    }

    public override void PerformAction(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        if(selectedObject is iTurnOnAndOffAble)
        {
            if((selectedObject as iTurnOnAndOffAble).IsOn)
            {
                (selectedObject as iTurnOnAndOffAble).TurnOff();
            }
            else
            {
                (selectedObject as iTurnOnAndOffAble).TurnOn();
            }
        }
    }

    public override void SoftDeSelect(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateSelectionDetails(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        throw new System.NotImplementedException();
    }
}
