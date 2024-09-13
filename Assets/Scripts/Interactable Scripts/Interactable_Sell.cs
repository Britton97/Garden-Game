using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sell", menuName = "Interactable/Sell")]
public class Interactable_Sell : Interactable_Abs
{
    [SerializeField] private DataInt_SO playerMoney;
    [SerializeField] private GameAction moneyAction;
    [SerializeField] private GameAction soldAction;

    public override bool OnHoverEnter(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        //return false because we don't want to show the button when hovering over the object
        //we only want to show the button when an object is selected
        return false;
    }

    public override void OnHoverExit(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        UIButtonState.InvokeAction(false);
        UIButtonText.InvokeAction("Null");
    }
    public override void UpdateSelectionDetails(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        //Debug.Log("Testing Select Sell");
        iSellable sellable = IsSellable(selectedObject.gameObject);
        bool _isSellable = sellable.IsSellable();
        int _sellPrice = sellable.GetSellPrice();
        if (sellable != null && _isSellable)
        {
            UIButtonState.InvokeAction(true);
            UIButtonText.InvokeAction($"{selectText} ({_sellPrice})");
        }
        else
        {
            UIButtonState.InvokeAction(true);
            UIButtonText.InvokeAction($"{selectText} ({_sellPrice})");
        }
    }
    public override bool OnSelect(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        return false;
        //this technically shouldn't ever be called since
        //OnHoverEnter returns false which means the button should never be shown
        //until an object is selected
        //Debug.Log("Somehow OnSelect was called on Interactable_Sell");
        /*
        if (context.currentSelection != null)
        {
            SellItem(context, selectedObject);
        }
        */
    }

    public override void OnDeselect(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        //Debug.Log("Deselected");
        context.transform.parent = null;
        UIButtonState.InvokeAction(false);
        UIButtonText.InvokeAction("Null");
    }

    public override void SoftDeSelect(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        UIButtonState.InvokeAction(false);
        UIButtonText.InvokeAction("Null");
    }

    public override void PerformAction(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        if (context.currentSelection != null)
        {
            SellItem(context, selectedObject);
        }
    }

    private void SellItem(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        iSellable sellable = IsSellable(selectedObject.gameObject);
        if (sellable != null && sellable.IsSellable())
        {
            context.transform.parent = null;
            playerMoney.data += sellable.GetSellPrice();
            moneyAction.InvokeAction();
            soldAction.InvokeAction();
            sellable.OnSell();
            context.selectionMenuUI.Deselect();
            context.currentSelection = null;
            context.hoveringOver = null;
            context.currentInteractable = null;
            UIButtonState.InvokeAction(false);
            UIButtonText.InvokeAction("Null");
            disableButtonUI.InvokeAction();
        }
    }

    private iSellable IsSellable(GameObject obj)
    {
        iSellable sellable = obj.GetComponent<iSellable>();
        if (sellable != null)
        {
            return sellable;
        }
        else
        {
            return null;
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
