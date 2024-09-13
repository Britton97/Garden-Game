using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
//[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
//where T is the garden object scriptable object
public abstract class GardenObject_MonoBehavior : SerializedMonoBehaviour, iSelectable, iSellable, iInteractable, iEdible
{
    protected bool alreadyRemoved = false;
    [HideInInspector] public Animator animator;
    /*
    public Interactable_Abs _interactionBehaviour;
    public Interactable_Abs interactable
    {
        get { return _interactionBehaviour; }
        set { _interactionBehaviour = value; }
    }
    */

    public InteractionManager _interactionManager;
    public InteractionManager interactionManager
    {
        get { return _interactionManager; }
        set { _interactionManager = value; }
    }


    public void Select()
    {
        //SelectionManager.Instance.NewSelection(this, gameObject);
        Debug.LogError("Select function on garden object mono behavior is not implemented");
    }
    //on start of the game object, add the garden item to the garden manager
    public virtual void Start()
    {
        alreadyRemoved = false;
        GardenManager.Instance.AddGardenItem(this);
        animator = GetComponent<Animator>();
    }

    void OnDestroy()
    {
        //Debug.Log($"alreadyRemoved: {alreadyRemoved}");
        if (!alreadyRemoved)
        {
            GardenManager.Instance.RemoveGardenItem(this);
            SelectionManager.Instance.IsSelectionManagerUsingObject(gameObject);
            alreadyRemoved = true;
        }
    }

    public abstract string GetName();
    public abstract Sprite GetSprite();
    public void OnSell()
    {
        Destroy(gameObject);
    }
    public abstract int GetSellPrice();
    public abstract bool IsSellable();
    public abstract bool IsEdible();

    public virtual void OnEnable()
    {
        if (alreadyRemoved)
        {
            GardenManager.Instance.AddGardenItem(this);
            alreadyRemoved = false;
        }
    }

    void OnDisable()
    {
        if (!alreadyRemoved)
        {
            GardenManager.Instance.RemoveGardenItem(this);
            SelectionManager.Instance.IsSelectionManagerUsingObject(gameObject);
            alreadyRemoved = true;
        }
    }
}
[Serializable]
public class InteractionManager
{
    public Interactable_Abs AButton;
    public Interactable_Abs BButton;
    public Interactable_Abs XButton;
    public Interactable_Abs YButton;

    #region OnHoverEnter & OnHoverExit
    public bool OnHoverEnter(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        List<bool> hoverEnterResults = new List<bool>();
        if (AButton != null)
        {
            hoverEnterResults.Add(AButton.OnHoverEnter(context, selectedObject));
        }
        if (BButton != null)
        {
            hoverEnterResults.Add(BButton.OnHoverEnter(context, selectedObject));
        }
        if (XButton != null)
        {
            hoverEnterResults.Add(XButton.OnHoverEnter(context, selectedObject));
        }
        if (YButton != null)
        {
            hoverEnterResults.Add(YButton.OnHoverEnter(context, selectedObject));
        }

        //check each result to see if any of them are true
        //if so, return true
        bool result = false;
        foreach (bool hoverEnterResult in hoverEnterResults)
        {
            if (hoverEnterResult)
            {
                result = true;
            }
        }
        return result;
    }

    public void OnHoverExit(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        if (AButton != null)
        {
            AButton.OnHoverExit(context, selectedObject);
        }
        if (BButton != null)
        {
            BButton.OnHoverExit(context, selectedObject);
        }
        if (XButton != null)
        {
            XButton.OnHoverExit(context, selectedObject);
        }
        if (YButton != null)
        {
            YButton.OnHoverExit(context, selectedObject);
        }
    }
    #endregion

    public void Deselect(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        if (AButton != null)
        {
            AButton.OnDeselect(context, selectedObject);
        }
        if (BButton != null)
        {
            BButton.OnDeselect(context, selectedObject);
        }
        if (XButton != null)
        {
            XButton.OnDeselect(context, selectedObject);
        }
        if (YButton != null)
        {
            YButton.OnDeselect(context, selectedObject);
        }
    }

    public void SoftDeSelect(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        if (AButton != null)
        {
            AButton.SoftDeSelect(context, selectedObject);
        }
        if (BButton != null)
        {
            BButton.SoftDeSelect(context, selectedObject);
        }
        if (XButton != null)
        {
            XButton.SoftDeSelect(context, selectedObject);
        }
        if (YButton != null)
        {
            YButton.SoftDeSelect(context, selectedObject);
        }
    }

    public void UpdateManagerSelectionsDetails(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        if (AButton != null)
        {
            AButton.UpdateSelectionDetails(context, selectedObject);
        }
        if (BButton != null)
        {
            BButton.UpdateSelectionDetails(context, selectedObject);
        }
        if (XButton != null)
        {
            XButton.UpdateSelectionDetails(context, selectedObject);
        }
        if (YButton != null)
        {
            YButton.UpdateSelectionDetails(context, selectedObject);
        }
    }

    public void HoverEnterWhileSelected(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        if (AButton != null && AButton.hasHoverText)
        {
            AButton.HoverEnterWhileSelected(context, selectedObject);
        }
        if (BButton != null && BButton.hasHoverText)
        {
            BButton.HoverEnterWhileSelected(context, selectedObject);
        }
        if (XButton != null && XButton.hasHoverText)
        {
            XButton.HoverEnterWhileSelected(context, selectedObject);
        }
        if (YButton != null && YButton.hasHoverText)
        {
            YButton.HoverEnterWhileSelected(context, selectedObject);
        }
    }

    //hover exit while selected
    public void HoverExitWhileSelected(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        if (AButton != null && AButton.hasHoverText)
        {
            AButton.HoverExitWhileSelected(context, selectedObject);
        }
        if (BButton != null && BButton.hasHoverText)
        {
            BButton.HoverExitWhileSelected(context, selectedObject);
        }
        if (XButton != null && XButton.hasHoverText)
        {
            XButton.HoverExitWhileSelected(context, selectedObject);
        }
        if (YButton != null && YButton.hasHoverText)
        {
            YButton.HoverExitWhileSelected(context, selectedObject);
        }
    }

    #region Select and Perform Actions
    public void AButtonSelect(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        if (AButton != null)
        {
            if (AButton.OnSelect(context, selectedObject))
            {
                context.currentSelection = context.hoveringOver;
            }
        }
    }
    public void AButtonPerformAction(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        if (AButton != null)
        {
            AButton.PerformAction(context, selectedObject);
        }
    }

    public void BButtonSelect(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        if (BButton != null)
        {
            if (BButton.OnSelect(context, selectedObject))
            {
                context.currentSelection = context.hoveringOver;
            }
        }
    }
    public void BButtonPerformAction(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        if (BButton != null)
        {
            BButton.PerformAction(context, selectedObject);
        }
    }

    public void XButtonSelect(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        if (XButton != null)
        {
            if (XButton.OnSelect(context, selectedObject))
            {
                context.currentSelection = context.hoveringOver;
            }
        }
    }
    public void XButtonPerformAction(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        if (XButton != null)
        {
            XButton.PerformAction(context, selectedObject);
        }
    }

    public void YButtonSelect(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        if (YButton != null)
        {
            if (YButton.OnSelect(context, selectedObject))
            {
                context.currentSelection = context.hoveringOver;
            }
        }
    }
    public void YButtonPerformAction(SelectionManager context, GardenObject_MonoBehavior selectedObject)
    {
        if (YButton != null)
        {
            YButton.PerformAction(context, selectedObject);
        }
    }
    #endregion
}