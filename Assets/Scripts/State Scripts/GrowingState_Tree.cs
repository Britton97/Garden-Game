using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingState_Tree : BaseState_Plant
{
    public override void EnterState(Plant_MonoBehavior gardenObject_ctx)
    {
        gardenObject_ctx.SetPlantState(PlantState.Seed);
        gardenObject_ctx._spriteRenderer.sprite = gardenObject_ctx.plantObject.GetSprite(0);
        gardenObject_ctx.shadowRenderer.sprite = gardenObject_ctx.plantObject.GetSprite(0);
        gardenObject_ctx.lastProgressUpdate = 0;
    }

    public override void ExitState(Plant_MonoBehavior gardenObject_ctx)
    {
        gardenObject_ctx._spriteRenderer.sprite = gardenObject_ctx.plantObject.fullyGrownSprite;
        gardenObject_ctx.shadowRenderer.sprite = gardenObject_ctx.plantObject.fullyGrownSprite;
        gardenObject_ctx.lastProgressUpdate = 1;
        gardenObject_ctx.growthProgress = 1;
    }

    public override void OnTriggerExit(Collider other)
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerStay(Collider other)
    {
        throw new System.NotImplementedException();
    }
}
