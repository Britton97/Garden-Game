using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class PlantStateCollisionCondition_Abs
{
    public BaseState_Plant nextState;
    public abstract bool CheckCondition(Plant_MonoBehavior gardenObject_ctx, Collider2D other);
}

//null
[Serializable]
public class PlantStateCollisionCondition_CollidedWithNull : PlantStateCollisionCondition_Abs
{
    public override bool CheckCondition(Plant_MonoBehavior gardenObject_ctx, Collider2D other)
    {
        return false;
    }
}