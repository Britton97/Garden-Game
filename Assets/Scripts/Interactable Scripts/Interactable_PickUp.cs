using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PickUp", menuName = "Interactable/PickUp")]
public class Interactable_PickUp : Interactable_Abs
{
    public override bool OnHoverEnter(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        //Debug.Log("Hover Enter");
        iPickupable pickupable = IsPickupable(selectedObject.gameObject);
        if (pickupable != null && pickupable.IsPickupable())
        {
            UIButtonState.InvokeAction(true);
            UIButtonText.InvokeAction(hoverText);
            return true;
        }
        else if (pickupable != null && !pickupable.IsPickupable())
        {
            return false;
        }
        else
        {
            UIButtonState.InvokeAction(true);
            UIButtonText.InvokeAction(hoverText);
            return true;
        }
    }

    public override void OnHoverExit(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        //Debug.Log("Hover Exit");
        UIButtonState.InvokeAction(false);
        UIButtonText.InvokeAction("Null");
    }

    public override void UpdateSelectionDetails(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        //Debug.Log("Testing Select PickUp");
    }

    public override bool OnSelect(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        //Debug.Log($"Parenting {selectedObject.gameObject.name} to {context.gameObject.name}");
        iPickupable pickupable = IsPickupable(selectedObject.gameObject);
        bool _isPickupable = pickupable.IsPickupable();

        if (_isPickupable == false) {return false;}

        context.currentSelection = selectedObject.gameObject;
        selectedObject.transform.parent = context.transform;
        selectedObject.transform.localPosition = Vector3.zero;
        selectedObject.GetComponent<CircleCollider2D>().enabled = false;
        context.UpdateManagerSelectionDetails();
        UIButtonText.InvokeAction(selectText);
        
        return true;
    }

    public override void OnDeselect(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        //Debug.Log("Deselected");
        selectedObject.transform.parent = null;
        UIButtonState.InvokeAction(false);
        UIButtonText.InvokeAction("Null");
    }

    public override void SoftDeSelect(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        //Debug.Log("Soft Deselected PickUp");
    }

    public override void PerformAction(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        //Debug.Log("Performing Action");
        Vector3 mousePos = context.ReturnMousePosition();
        RaycastHit2D hit = context.RaycastForCollider(mousePos, context.raycastLayer);

        if (hit.collider == null)
        {
            selectedObject.transform.parent = null;
            selectedObject.GetComponent<CircleCollider2D>().enabled = true;

            //disableButtonUI.InvokeAction();
            //UIButtonState.InvokeAction(false);
            context.SoftDeSelect();
            UIButtonText.InvokeAction(performText);
            context.currentSelection = null;
        }
        else
        {
            Debug.Log("Hit collider was not null: " + hit.collider.name);
        }

    }

    public iPickupable IsPickupable(GameObject item)
    {
        if (item.TryGetComponent(out iPickupable pickupable))
        {
            return pickupable;
        }
        return null;
    }

    public override void HoverEnterWhileSelected(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        Debug.Log("Hovering While Selected");
        UIButtonText.InvokeAction(selectAndHoverText);
    }

    public override void HoverExitWhileSelected(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        UIButtonText.InvokeAction(selectText);
    }
}
