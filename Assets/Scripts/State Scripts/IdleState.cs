using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

//[InlineEditor]
[CreateAssetMenu(fileName = "IdleState", menuName = "State System/Idle State")]
public class IdleState : BaseState_Animal
{
    /*
    public void EnterState(GardenObject_MonoBehavior gardenObject_ctx)
    {
        //throw new System.NotImplementedException();
        Debug.Log("Entering Idle State");
        //gardenObject_ctx.animator.SetTrigger("Attack");
    }
    */
    /*
    public override BaseState_Animal GetNextState()
    {
        throw new System.NotImplementedException();
    }
    */

    public override void OnTriggerExit(Collider other)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnTriggerStay(Collider other)
    {
        //throw new System.NotImplementedException();
    }
}
