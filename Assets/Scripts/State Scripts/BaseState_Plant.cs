using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class BaseState_Plant : SerializedScriptableObject
{
    public PlantStateBoolCondition_Abstract boolCondition;
    public PlantStateCollisionCondition_Abs collisionCondition;
    //public BaseState_Animal nextState;

    public abstract void EnterState(Plant_MonoBehavior gardenObject_ctx);
    public abstract void ExitState(Plant_MonoBehavior gardenObject_ctx);
    public void UpdateState(Plant_MonoBehavior gardenObject_ctx)
    {
        if (boolCondition.CheckCondition(gardenObject_ctx))
        {
            ExitState(gardenObject_ctx);

            try
            {
                gardenObject_ctx.currentState = boolCondition.nextState;
                gardenObject_ctx.currentState.EnterState(gardenObject_ctx);
            }
            catch
            {
                return;
            }
        }
    }

    public void StateOnTriggerEnter2D(Plant_MonoBehavior gardenObject_ctx, Collider2D other)
    {
        if (collisionCondition.CheckCondition(gardenObject_ctx, other))
        {
            ExitState(gardenObject_ctx);
            gardenObject_ctx.currentState = collisionCondition.nextState;
            gardenObject_ctx.currentState.EnterState(gardenObject_ctx);
            //nextState.EnterState(gardenObject_ctx);
        }
    }
    public abstract void OnTriggerStay(Collider other);
    public abstract void OnTriggerExit(Collider other);
}
