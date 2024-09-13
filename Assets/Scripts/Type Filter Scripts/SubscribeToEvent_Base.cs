using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class SubscribeToEvent_Base { }
public abstract class SubscribeToEvent_Empty : SubscribeToEvent_Base
{
    public abstract void ReturnValue();
}

public abstract class SubscribeToEvent_Float : SubscribeToEvent_Base
{
    public abstract float ReturnValue();
}

public abstract class SubscribeToEvent_Int : SubscribeToEvent_Base
{
    public abstract int ReturnValue();
}

public abstract class SubscribeToEvent_String : SubscribeToEvent_Base
{
    public abstract string ReturnValue();
}

public abstract class SubscribeToEvent_Bool : SubscribeToEvent_Base
{
    public abstract bool ReturnValue();
}

public abstract class SubscribeToEvent_Vector2 : SubscribeToEvent_Base
{
    public abstract Vector2 ReturnValue();
}

public abstract class SubscribeToEvent_Vector3 : SubscribeToEvent_Base
{
    public abstract Vector3 ReturnValue();
}

public abstract class SubscribeToEvent_GameObject : SubscribeToEvent_Base
{
    public abstract GameObject ReturnValue();
}