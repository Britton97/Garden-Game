using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[InlineEditor]
[CreateAssetMenu(fileName = "Growing State", menuName = "ScriptableObjects/Plant States/Growing State", order = 1)]
public class GrowingState : BaseState_Plant
{
    public override void EnterState(Plant_MonoBehavior gardenObject_ctx)
    {
        gardenObject_ctx.SetPlantState(PlantState.Seed);

        if(gardenObject_ctx.plantObject.growthType == GrowthType.SpriteSheet)
        {
            gardenObject_ctx._spriteRenderer.sprite = gardenObject_ctx.plantObject.GetSprite(0);
            gardenObject_ctx.shadowRenderer.sprite = gardenObject_ctx.plantObject.GetSprite(0);
        }
        else
        {
            gardenObject_ctx._spriteRenderer.transform.localScale = gardenObject_ctx.plantObject.startScale;
            //gardenObject_ctx.shadowRenderer.transform.localScale = gardenObject_ctx.plantObject.startScale;
        }
        //gardenObject_ctx._spriteRenderer.sprite = gardenObject_ctx.plantObject.GetSprite(0);
        //gardenObject_ctx.shadowRenderer.sprite = gardenObject_ctx.plantObject.GetSprite(0);
        gardenObject_ctx.lastProgressUpdate = 0;
    }

    public override void ExitState(Plant_MonoBehavior gardenObject_ctx)
    {
        if(gardenObject_ctx.plantObject.growthType == GrowthType.SpriteSheet)
        {
            gardenObject_ctx._spriteRenderer.sprite = gardenObject_ctx.plantObject.fullyGrownSprite;
            gardenObject_ctx.shadowRenderer.sprite = gardenObject_ctx.plantObject.fullyGrownSprite;
        }
        else
        {
            gardenObject_ctx._spriteRenderer.transform.localScale = gardenObject_ctx.plantObject.endScale;
            //gardenObject_ctx.shadowRenderer.transform.localScale = gardenObject_ctx.plantObject.endScale;
        }
        //gardenObject_ctx._spriteRenderer.sprite = gardenObject_ctx.plantObject.fullyGrownSprite;
        //gardenObject_ctx.shadowRenderer.sprite = gardenObject_ctx.plantObject.fullyGrownSprite;
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
