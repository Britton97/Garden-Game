using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "WalkToFood", menuName = "State System/Bool Conditions/WalkToFood")]
public class AnimalState_WalkToFood : AnimalStateBoolCondition_Abstract
{
    public float distanceToTarget = 2f;
    [SerializeField] private float hungerThreshold = 50f;
    public override bool OnEnterBehavior(Animal_MonoBehavior gardenObject_ctx)
    {
        Debug.Log("Entered WalkToFood");
        gardenObject_ctx.animator.SetTrigger("Walk");
        //GardenObject_MonoBehavior food = GardenManager.Instance.GetGardenItem(gardenObject_ctx.animalObject.GetRandomRequirement());
        GardenObject_MonoBehavior food = GardenManager.Instance.GetFoodRequirementFromInterests(gardenObject_ctx);
        /*
        if (food == null)
        {
            food = GardenManager.Instance.GetRandomMaturePlant();
        }
        */
        if (food == null)
        {
            Debug.Log("No food found");
        }
        gardenObject_ctx.interest = food.gameObject;
        if (gardenObject_ctx is iEmoji && gardenObject_ctx._GifPlayer.GetGifName() != "Hungry")
        {
            gardenObject_ctx._GifPlayer.PlayGif("Hungry", 3f);
        }
        gardenObject_ctx.navMeshAgent.SetDestination(food.transform.position);
        return false;
    }

    public override bool OnUpdateBehavior(Animal_MonoBehavior gardenObject_ctx)
    {
        //interest is missing
        if (gardenObject_ctx.interest == null)
        {
            Debug.Log("Interest is missing");
            if (gardenObject_ctx is iEmoji && gardenObject_ctx._GifPlayer.GetGifName() != "Frustrated")
            {
                gardenObject_ctx._GifPlayer.PlayGif("Frustrated", 3f);
            }
            Destroy(gardenObject_ctx.gameObject,4);
            return false;
        }
        gardenObject_ctx.navMeshAgent.SetDestination(gardenObject_ctx.interest.transform.position);
        if (Vector2.Distance(gardenObject_ctx.transform.position, gardenObject_ctx.navMeshAgent.destination) < distanceToTarget && gardenObject_ctx.interest != null)
        {
            gardenObject_ctx.animalObject.EatFood(gardenObject_ctx, gardenObject_ctx.interest);
            return false;
        }
        return true;
    }

    public override bool CheckCondition(Animal_MonoBehavior gardenObject_ctx)
    {
        if (gardenObject_ctx.isTamed == true) return false;
        var requirement = GardenManager.Instance.GetFoodRequirementFromInterests(gardenObject_ctx);
        if (requirement == null && gardenObject_ctx.hunger <= 0)
        {
            //Destroy(gardenObject_ctx.gameObject);
            Debug.Log("No food requirement found and at 0 hunger");
            return false;
        }
        if (gardenObject_ctx.hunger < hungerThreshold && GardenManager.Instance.GetFoodRequirementFromInterests(gardenObject_ctx) != null)
        {
            return true;
        }
        return false;
    }
}