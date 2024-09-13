using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimalStateBoolCondition_Abstract : SerializedScriptableObject
{
    [Range(0,10)]
    public int priority = 0;
    //public BaseState_Animal nextState;
    public abstract bool OnEnterBehavior(Animal_MonoBehavior gardenObject_ctx);
    public abstract bool CheckCondition(Animal_MonoBehavior gardenObject_ctx);
    public abstract bool OnUpdateBehavior(Animal_MonoBehavior gardenObject_ctx);
}