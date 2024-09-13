using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[InlineEditor]
[CreateAssetMenu(fileName = "Grown State", menuName = "ScriptableObjects/Plant States/Grown State", order = 1)]
public class GrownState : BaseState_Plant
{
    [SerializeField] private UnlockableTree unlockableTree;
    public override void EnterState(Plant_MonoBehavior gardenObject_ctx)
    {
        gardenObject_ctx.SetPlantState(PlantState.Mature);
        //gardenObject_ctx.particleEvent.Invoke();
        gardenObject_ctx.finishedGrowingEvent.Invoke();
        gardenObject_ctx.JumpTween();
        gardenObject_ctx._spriteRenderer.sprite = gardenObject_ctx.plantObject.fullyGrownSprite;
        gardenObject_ctx.shadowRenderer.sprite = gardenObject_ctx.plantObject.fullyGrownSprite;
        if(gardenObject_ctx.plantObject.alreadyGrownOnce == false)
        {
            gardenObject_ctx.plantObject.alreadyGrownOnce = true;
            unlockableTree.AddExperience((int)gardenObject_ctx.plantObject.FirstTimeTameExperience);
        }
        //gardenObject_ctx.lastProgressUpdate = 1;
        //gardenObject_ctx.growthProgress = 1;
    }

    public override void ExitState(Plant_MonoBehavior gardenObject_ctx)
    {
        gardenObject_ctx.DestroyPlant();
    }

    public override void OnTriggerExit(Collider other)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnTriggerStay(Collider other)
    {
        //throw new System.NotImplementedException();
    }
}