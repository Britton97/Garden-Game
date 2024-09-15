using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DirectAnimal", menuName = "Interactable/DirectAnimal")]
public class Interactable_DirectAnimal : Interactable_Abs
{
    public override bool OnHoverEnter(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        iDirectable directable = selectedObject.gameObject.GetComponent<iDirectable>();
        if (directable != null && directable.IsDirectable())
        {
            UIButtonState.InvokeAction(true);
            UIButtonText.InvokeAction(hoverText);
            return true;
        }
        return false;
    }

    public override void OnHoverExit(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        //Debug.Log("Hover Exit");
        UIButtonState.InvokeAction(false);
        UIButtonText.InvokeAction("Null");
    }

    public override void UpdateSelectionDetails(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        return;
    }

    public override bool OnSelect(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        Debug.Log($"Parenting {context.gameObject.name} to {selectedObject.gameObject.name}");
        if(selectedObject is iDirectable)
        {
            iDirectable directable = selectedObject.gameObject.GetComponent<iDirectable>();
            bool _isDirectable = directable.IsDirectable();
            if (_isDirectable == false) { return false; }
        }

        context.transform.parent = selectedObject.transform;
        context.transform.localPosition = Vector3.zero;
        context.UpdateManagerSelectionDetails();
        UIButtonText.InvokeAction(selectText);
        return true;
    }

    public override void SoftDeSelect(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        Debug.Log("Soft Deselected Direct Animal");
    }

    public override void OnDeselect(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        context.transform.parent = null;
        UIButtonState.InvokeAction(false);
        UIButtonText.InvokeAction("Null");
    }

    public override void PerformAction(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        //Debug.Log("Performing Action");
        selectedObject.transform.parent = null;
        context.transform.parent = null;

        //if statement for if the selected object is a animal mono behavior
        if (selectedObject is Animal_MonoBehavior)
        {
            Vector3 mousePos = context.ReturnMousePosition();
            RaycastHit2D hit = context.RaycastForCollider(mousePos, context.raycastLayer);
            if (context.MoveAnimalToPoint(mousePos, hit))
            {
                UIButtonText.InvokeAction("Sent");
            }
        }
    }

    public override void HoverEnterWhileSelected(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        Vector3 mousePos = context.ReturnMousePosition();
        RaycastHit2D hit = context.RaycastForCollider(mousePos, context.raycastLayer);
        //if hit object is a animal mono behavior or plant mono behavior
        //then set the button text to "Eat"
        //else set the button text to "Null"
        if (hit.collider != null)
        {
            //out a garden object mono behavior
            if (hit.collider.gameObject.TryGetComponent(out GardenObject_MonoBehavior gardenObject))
            {
                //if the garden object has the same name as the selected object
                if(gardenObject.GetName() == selectedObject.GetName())
                {
                    UIButtonText.InvokeAction($"Mate {gardenObject.GetName()} with {selectedObject.GetName()}");
                }
                else
                {
                    UIButtonText.InvokeAction($"Eat ({gardenObject.GetName()})");
                }
                //UIButtonText.InvokeAction($"Eat ({gardenObject.GetName()})");
            }
        }
        //UIButtonText.InvokeAction(selectAndHoverText);
    }
    public override void HoverExitWhileSelected(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        UIButtonText.InvokeAction(selectText);
        //Debug.Log("Hover Exit While Selected");
    }
}
