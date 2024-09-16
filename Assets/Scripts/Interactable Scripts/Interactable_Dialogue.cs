using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Interactable/Dialogue")]
public class Interactable_Dialogue : Interactable_Abs
{
    [SerializeField] private DataStringList_SO dialogue;
    [SerializeField] GameAction startDialogueAction;
    public override void HoverEnterWhileSelected(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        return;
    }

    public override void HoverExitWhileSelected(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        return;
    }

    public override void OnDeselect(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        return;
    }

    public override bool OnHoverEnter(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        UIButtonState.InvokeAction(true);
        UIButtonText.InvokeAction(hoverText);
        return true;
    }

    public override void OnHoverExit(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        UIButtonState.InvokeAction(false);
        UIButtonText.InvokeAction("Null");
    }

    public override bool OnSelect(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        startDialogueAction.InvokeAction();
        //selection manager deselect
        SelectionManager.Instance.Deselect();
        return true;
    }

    public override void PerformAction(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        startDialogueAction.InvokeAction();
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
