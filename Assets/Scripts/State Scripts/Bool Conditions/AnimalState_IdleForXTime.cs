using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IdleForXTime", menuName = "State System/Bool Conditions/IdleForXTime")]
public class AnimalState_IdleForXTime : AnimalStateBoolCondition_Abstract
{
    [SerializeField] [Min(0)] private float minIdleTime = 5;
    [SerializeField] [Min(0)] private float maxIdleTime = 10;
    public override bool OnEnterBehavior(Animal_MonoBehavior gardenObject_ctx)
    {
        ///Debug.Log("Entered IdleForXTime");
        gardenObject_ctx.animator.SetTrigger("Idle");
        gardenObject_ctx.IdleOnTimer(Random.Range(minIdleTime, maxIdleTime));
        return false;
    }

    public override bool OnUpdateBehavior(Animal_MonoBehavior gardenObject_ctx)
    {
        if (gardenObject_ctx.idleTime >= 0)
        {
            /*
            if(gardenObject_ctx.idleTimerCoroutine != null)
            {
                gardenObject_ctx.StopCoroutine(gardenObject_ctx.idleTimerCoroutine);
                gardenObject_ctx.idleTimerCoroutine = null;
            }
            */
            return true;
        }
        return false;
    }

    public override bool CheckCondition(Animal_MonoBehavior gardenObject_ctx)
    {
        if(gardenObject_ctx.idleTime <= 0)
        {
            return true;
        }
        return false;
    }
}