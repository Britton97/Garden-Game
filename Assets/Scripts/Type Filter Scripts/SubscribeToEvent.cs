using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class SubscribeToEvent : SerializedMonoBehaviour
{
    [SerializeField] GameObject test;

    //[Title("Event Subscriber")]
    //[InlineProperty]
    //[HideLabel]
    //[BoxGroup]
    [OdinSerialize] public EventSubscriberBase eventSubscriber;

    private void Update()
    {
        //on spacebar press, invoke the event
        if (Input.GetKeyDown(KeyCode.Space))
        {
            eventSubscriber.InvokeEvent();
        }
    }

    public void Announce(int value)
    {
        Debug.Log("Announcing: " + value);
    }

    public void Announce(float value)
    {
        Debug.Log("Announcing: " + value);
    }

    public void Announce(GameObject value)
    {
        Debug.Log("Announcing: " + value.name);
    }
}

[HideLabel]
public abstract class EventSubscriberBase
{
    public abstract void InvokeEvent();
}

public class EventSubscriber_Empty : EventSubscriberBase
{
    #region EnumList
    //[InfoBox("EventSubscriber_Empty requires implementing class to have a UnityEvent and UnityAction", InfoMessageType.Warning)]
    //[TypeFilter("EnumList")]
    //public SubscribeToEvent_Empty subscribeTo;

    [BoxGroup("Empty Event")]
    [InlineProperty] [HideLabel] public UnityEvent emptyEvent = new UnityEvent();
    /*
    public IEnumerable<Type> EnumList()
    {
        var q = typeof(SubscribeToEvent_Empty).Assembly.GetTypes()
            .Where(x => !x.IsAbstract)
            .Where(x => !x.IsGenericTypeDefinition)
            .Where(x => typeof(SubscribeToEvent_Empty).IsAssignableFrom(x));
        return q;
    }
    */
    #endregion

    public override void InvokeEvent()
    {
        //subscribeTo.InvokeEvent();
        emptyEvent.Invoke();
    }
}

public class EventSubscriber_Float : EventSubscriberBase
{
    #region EnumList
    [BoxGroup("Float Subscriber")]
    public bool useScriptableObject = false;
    [BoxGroup("Float Subscriber")]
    [InlineProperty]
    [HideIf("useScriptableObject")]
    [TypeFilter("EnumList")]
    public SubscribeToEvent_Float subscribeTo;

    [BoxGroup("Float Subscriber")]
    [ShowIf("useScriptableObject")]
    public DataFloat_SO dataFloat_SO;

    [BoxGroup("Float Subscriber")]
    [HideReferenceObjectPicker] 
    [InlineProperty] 
    public UnityEvent<float> floatEvent = new UnityEvent<float>();
    public IEnumerable<Type> EnumList()
    {
        var q = typeof(SubscribeToEvent_Float).Assembly.GetTypes()
            .Where(x => !x.IsAbstract)
            .Where(x => !x.IsGenericTypeDefinition)
            .Where(x => typeof(SubscribeToEvent_Float).IsAssignableFrom(x));
        return q;
    }
    #endregion

    public override void InvokeEvent()
    {
        if (useScriptableObject)
        {
            floatEvent.Invoke(dataFloat_SO.GetData());
        }
        else
        {
            floatEvent.Invoke(subscribeTo.ReturnValue());
        }
    }
}

public class EventSubscriber_Int : EventSubscriberBase
{
    #region EnumList
    [BoxGroup("Int Subscriber")]
    public bool useScriptableObject = false;
    [BoxGroup("Int Subscriber")]
    [InlineProperty]
    [HideIf("useScriptableObject")]
    [TypeFilter("EnumList")]
    public SubscribeToEvent_Int subscribeTo;

    [BoxGroup("Int Subscriber")]
    [ShowIf("useScriptableObject")]
    public DataInt_SO dataInt_SO;

    [BoxGroup("Int Subscriber")]
    [HideReferenceObjectPicker] 
    [InlineProperty] 
    public UnityEvent<int> intEvent = new UnityEvent<int>();
    public IEnumerable<Type> EnumList()
    {
        var q = typeof(SubscribeToEvent_Int).Assembly.GetTypes()
            .Where(x => !x.IsAbstract)
            .Where(x => !x.IsGenericTypeDefinition)
            .Where(x => typeof(SubscribeToEvent_Int).IsAssignableFrom(x));
        return q;
    }
    #endregion

    public override void InvokeEvent()
    {
        if (useScriptableObject)
        {
            intEvent.Invoke(dataInt_SO.GetData());
        }
        else
        {
            intEvent.Invoke(subscribeTo.ReturnValue());
        }
    }
}

public class EventSubscriber_GameObject : EventSubscriberBase
{
    #region EnumList
    [BoxGroup("GameObject Subscriber")]
    public bool useScriptableObject = false;
    [BoxGroup("GameObject Subscriber")]
    [InlineProperty]
    [HideIf("useScriptableObject")]
    [TypeFilter("EnumList")]
    public SubscribeToEvent_GameObject subscribeTo;

    [BoxGroup("GameObject Subscriber")]
    [ShowIf("useScriptableObject")]
    public DataGameObject_SO dataGameObject_SO;

    [BoxGroup("GameObject Subscriber")]
    [HideReferenceObjectPicker] 
    [InlineProperty] 
    public UnityEvent<GameObject> gameObjectEvent = new UnityEvent<GameObject>();
    public IEnumerable<Type> EnumList()
    {
        var q = typeof(SubscribeToEvent_GameObject).Assembly.GetTypes()
            .Where(x => !x.IsAbstract)
            .Where(x => !x.IsGenericTypeDefinition)
            .Where(x => typeof(SubscribeToEvent_GameObject).IsAssignableFrom(x));
        return q;
    }
    #endregion

    public override void InvokeEvent()
    {
        if (useScriptableObject)
        {
            gameObjectEvent.Invoke(dataGameObject_SO.GetData());
        }
        else
        {
            gameObjectEvent.Invoke(subscribeTo.ReturnValue());
        }
    }
}