using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Info", menuName = "Interactable/Info")]
public class Interactable_Info : Interactable_Abs
{
    public override bool OnHoverEnter(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        if (selectedObject is iSelectable && context.selectionMenuUI.isSelected == false)
        {
            UIButtonState.InvokeAction(true);
            UIButtonText.InvokeAction(hoverText);
            return true;
        }
        return false;
    }

    public override void OnHoverExit(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        if (context.selectionMenuUI.isSelected == false)
        {
            //Debug.Log("Hover Exit");
            UIButtonState.InvokeAction(false);
            UIButtonText.InvokeAction("Null");
            context.selectionMenuUI.Deselect();
        }
    }

    public override void UpdateSelectionDetails(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        //Debug.Log("Testing Select Info");
    }

    public override bool OnSelect(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        //context.currentSelection = selectedObject.gameObject;
        SendSelection(context, selectedObject);
        return false;
    }

    public override void SoftDeSelect(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        //Debug.Log("Soft Deselected Info");
    }

    public override void OnDeselect(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        Debug.Log("Info should be deselected");
        UIButtonState.InvokeAction(false);
        UIButtonText.InvokeAction("Null");
        //context.selectionMenuUI.Deselect();
    }

    public override void PerformAction(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        SendSelection(context, selectedObject);
    }

    public void SendSelection(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        Debug.Log("Performing Action");
        if (selectedObject is iSelectable)
        {
            if (context.selectionMenuUI.isSelected == false)
            {
                //context.transform.parent = selectedObject.transform;
                //context.transform.localPosition = Vector3.zero;
                context.selectionMenuUI.NewUISelection(selectedObject, selectedObject.gameObject);
                UIButtonText.InvokeAction(selectText);
            }
            else if (context.selectionMenuUI.isSelected == true)
            {
                UIButtonText.InvokeAction(hoverText);
                context.selectionMenuUI.Deselect();
            }
        }
    }

    public override void HoverEnterWhileSelected(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        UIButtonText.InvokeAction(selectAndHoverText);
    }

    public override void HoverExitWhileSelected(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        UIButtonText.InvokeAction(selectText);
        Debug.Log("Hover Exit While Selected");
    }
}
