using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseState_Animal : SerializedScriptableObject
{
    public AnimalStateBoolCondition_Abstract selfBoolCondition;
    public List<BaseState_Animal> possibleExitStates;
    //public AnimalStateCollisionCondition_Abs collisionCondition;

    public void EnterState(Animal_MonoBehavior gardenObject_ctx)
    {
        selfBoolCondition.OnEnterBehavior(gardenObject_ctx);
    }
    public void ExitState(){}
    public void UpdateState(Animal_MonoBehavior gardenObject_ctx)
    {
        if (!selfBoolCondition.OnUpdateBehavior(gardenObject_ctx))
        {
            BaseState_Animal nextState = null;
            int highestPriority = -1;

            foreach (var condition in possibleExitStates)
            {
                if (condition.selfBoolCondition.CheckCondition(gardenObject_ctx))
                {
                    if (condition.selfBoolCondition.priority > highestPriority)
                    {
                        highestPriority = condition.selfBoolCondition.priority;
                        nextState = condition;
                    }//else if they are the same priotity randomly choose one
                    else if (condition.selfBoolCondition.priority == highestPriority)
                    {
                        if (UnityEngine.Random.Range(0, 2) == 0)
                        {
                            nextState = condition;
                        }
                    }
                    
                }
            }
            if (nextState != null)
            {
                ExitState();
                nextState.EnterState(gardenObject_ctx);
                gardenObject_ctx.currentState = nextState;
            }
            else
            {
                Debug.Log("No valid state found maybe just enter the same state again???");
                //call enter state again
                EnterState(gardenObject_ctx);
            }
        }
    }

    public void StateOnTriggerEnter2D(Animal_MonoBehavior gardenObject_ctx, Collider2D other)
    {
        /*
        if (collisionCondition.CheckCondition(gardenObject_ctx, other))
        {
            ExitState();
            gardenObject_ctx.currentState = collisionCondition.nextState;
            gardenObject_ctx.currentState.EnterState(gardenObject_ctx);
            //nextState.EnterState(gardenObject_ctx);
        }
        */
    }
    public abstract void OnTriggerStay(Collider other);
    public abstract void OnTriggerExit(Collider other);
}