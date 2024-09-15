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
        //update the destination of the nav mesh agent to the target position
        if(gardenObject_ctx.interest != null) gardenObject_ctx.navMeshAgent.SetDestination(gardenObject_ctx.interest.transform.position);
        
        if (Vector2.Distance(gardenObject_ctx.transform.position, gardenObject_ctx.navMeshAgent.destination) < distanceToTarget)
        {
            //Debug.Log($"Distance to target is less than {hungerThreshold}");
            if(gardenObject_ctx.interest != null && Vector2.Distance(gardenObject_ctx.navMeshAgent.destination, gardenObject_ctx.interest.transform.position) < distanceToTarget)
            {
                //if the interest is food, eat the food
                //if the interest is a the same type of animal, mate (debug for now)
                //if the interest is a different type of animal, fight (debug for now)
                if(gardenObject_ctx.interest.TryGetComponent<GardenObject_MonoBehavior>(out GardenObject_MonoBehavior interestObject))
                {
                    if(interestObject is iMatable && gardenObject_ctx is iMatable &&gardenObject_ctx.GetName() == interestObject.GetName()) //and the interest is the same animal as gardenObject_ctx
                    {
                        bool canMate1 = gardenObject_ctx.IsMatable();
                        Animal_MonoBehavior interestAnimal = interestObject.GetComponent<Animal_MonoBehavior>();
                        bool canMate2 = interestAnimal.IsMatable();
                        if(canMate1 && canMate2)
                        {
                            gardenObject_ctx.Mate(gardenObject_ctx, interestAnimal);
                        }
                    }
                    else if(interestObject is iEdible)
                    {
                        gardenObject_ctx.animalObject.EatFood(gardenObject_ctx, gardenObject_ctx.interest);
                    }
                }
                //gardenObject_ctx.animalObject.EatFood(gardenObject_ctx, gardenObject_ctx.interest);
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