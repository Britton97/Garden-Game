using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "IdleUntilHungry", menuName = "State System/Bool Conditions/IdleUntilHungry")]
public class AnimalState_IdleUntilHungry : AnimalStateBoolCondition_Abstract
{
    [SerializeField] private float hungerThreshold = 50f;
    public override bool OnEnterBehavior(Animal_MonoBehavior gardenObject_ctx)
    {
        Debug.Log("Entered IdleUntilHungry");
        gardenObject_ctx.animator.SetTrigger("Idle");
        if (gardenObject_ctx.hunger <= 0)
        {
            return true;
        }
        return false;
    }

    public override bool OnUpdateBehavior(Animal_MonoBehavior gardenObject_ctx)
    {
        if (gardenObject_ctx.hunger <= hungerThreshold)
        {
            return false;
        }
        return true;
    }

    public override bool CheckCondition(Animal_MonoBehavior gardenObject_ctx)
    {
        return true;
    }
}
