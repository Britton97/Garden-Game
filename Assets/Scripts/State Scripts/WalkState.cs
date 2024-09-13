using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[InlineEditor]
[CreateAssetMenu(fileName = "WalkState", menuName = "State System/Walk State")]
public class WalkState : BaseState_Animal
{
    /*
    public override void EnterState(GardenObject_MonoBehavior gardenObject_ctx)
    {
        //throw new System.NotImplementedException();
        Debug.Log("Entering Walk State");
        gardenObject_ctx.animator.SetTrigger("Walk");
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

    // Implement the other abstract methods...
}