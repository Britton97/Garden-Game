using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "WalkToPoint", menuName = "State System/Bool Conditions/WalkToPoint")]
public class AnimalState_WalkToPoint : AnimalStateBoolCondition_Abstract
{
    //public Vector3 targetPosition;
    public float distanceToTarget = 2f;
    public override bool OnEnterBehavior(Animal_MonoBehavior gardenObject_ctx)
    {
        //Debug.Log("Entered WalkToPoint");
        gardenObject_ctx.animator.SetTrigger("Walk");
        //gardenObject_ctx.navMeshAgent.SetDestination(targetPosition);
        return false;
    }

    public override bool OnUpdateBehavior(Animal_MonoBehavior gardenObject_ctx)
    {
        if (Vector2.Distance(gardenObject_ctx.transform.position, gardenObject_ctx.navMeshAgent.destination) < distanceToTarget)
        {
            //Debug.Log($"Distance to target is less than {hungerThreshold}");
            if(gardenObject_ctx.interest != null && Vector3.Distance(gardenObject_ctx.navMeshAgent.destination, gardenObject_ctx.interest.transform.position) < distanceToTarget)
            {
                gardenObject_ctx.animalObject.EatFood(gardenObject_ctx, gardenObject_ctx.interest);
            }
            return false;
        }
        return true;
    }

    public override bool CheckCondition(Animal_MonoBehavior gardenObject_ctx)
    {
        return true;
    }
}