using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimalStateCollisionCondition_Abs
{
    public BaseState_Animal nextState;
    public abstract bool CheckCondition(Animal_MonoBehavior gardenObject_ctx, Collider2D other);
}
/*
[Serializable]
public class AnimalStateCollisionCondition_CollidedWithInterest : AnimalStateCollisionCondition_Abs
{
    public override bool CheckCondition(Animal_MonoBehavior gardenObject_ctx, Collider2D other)
    {
        if (other.gameObject == gardenObject_ctx.interest && gardenObject_ctx.interest != null)
        {
            //Debug.Log("Collided with interest");
            //if other gameobject is of type plant monobeheavior
            if(other.gameObject.TryGetComponent<Plant_MonoBehavior>(out Plant_MonoBehavior plant))
            {
                gardenObject_ctx.hunger = gardenObject_ctx.maxHunger;
                gardenObject_ctx.interest = null;
                plant.DestroyPlant();
                return true;
            }
            return true;
        }
        return false;
    }
}

[Serializable]
//null
public class AnimalStateCollisionCondition_CollidedWithNull : AnimalStateCollisionCondition_Abs
{
    public override bool CheckCondition(Animal_MonoBehavior gardenObject_ctx, Collider2D other)
    {
        return false;
    }
}
*/