using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Interactable_Abs : SerializedScriptableObject
{
    [SerializeField] protected GameAction_Bool UIButtonState;
    [SerializeField] protected GameAction_String UIButtonText;
    [SerializeField] protected GameAction disableButtonUI;
    [SerializeField] protected string hoverText, selectText, performText, selectAndHoverText;
    [SerializeField] public bool hasHoverText;
    public abstract void OnHoverExit(SelectionManager context, GardenObject_MonoBehavior selectedObject);
    public abstract bool OnHoverEnter(SelectionManager context, GardenObject_MonoBehavior selectedObject);
    public abstract void UpdateSelectionDetails(SelectionManager context, GardenObject_MonoBehavior selectedObject);
    public abstract void SoftDeSelect(SelectionManager context, GardenObject_MonoBehavior selectedObject);
    public abstract bool OnSelect(SelectionManager context, GardenObject_MonoBehavior selectedObject);
    public abstract void OnDeselect(SelectionManager context, GardenObject_MonoBehavior selectedObject);
    public abstract void PerformAction(SelectionManager context, GardenObject_MonoBehavior selectedObject);

    public abstract void HoverEnterWhileSelected(SelectionManager context, GardenObject_MonoBehavior selectedObject);
    public abstract void HoverExitWhileSelected(SelectionManager context, GardenObject_MonoBehavior selectedObject);
}

public interface iInteractable
{
    InteractionManager interactionManager { get; set; }
}
