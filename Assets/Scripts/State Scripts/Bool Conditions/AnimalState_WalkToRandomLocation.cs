using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "WalkToRandomLocation", menuName = "State System/Bool Conditions/WalkToRandomLocation")]
public class AnimalState_WalkToRandomLocation : AnimalStateBoolCondition_Abstract
{
    public float radius = 5f;
    public float distanceToTarget = 2f;
    public override bool OnEnterBehavior(Animal_MonoBehavior gardenObject_ctx)
    {
        if(gardenObject_ctx == null)
        {
            Debug.Log("Garden Object is null");
            return false;
        }
        //Debug.Log("Entered WalkToRandomLocation");
        gardenObject_ctx.animator.SetTrigger("Walk");
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
        randomDirection += gardenObject_ctx.transform.position;
        UnityEngine.AI.NavMeshHit hit;
        //Debug.Log($"Found Random Position: {randomDirection}");
        UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, radius, 1);
        Vector3 finalPosition = hit.position;
        gardenObject_ctx.navMeshAgent.SetDestination(finalPosition);
        return false;
    }

    public override bool OnUpdateBehavior(Animal_MonoBehavior gardenObject_ctx)
    {
        if (Vector2.Distance(gardenObject_ctx.transform.position, gardenObject_ctx.navMeshAgent.destination) < distanceToTarget)
        {
            //Debug.Log($"Distance to target is less than {hungerThreshold}");
            return false;
        }
        return true;
    }

    public override bool CheckCondition(Animal_MonoBehavior gardenObject_ctx)
    {
        return true;
    }
}