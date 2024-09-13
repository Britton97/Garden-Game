using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class PlantStateBoolCondition_Abstract
{
    public BaseState_Plant nextState;
    public abstract bool CheckCondition(Plant_MonoBehavior gardenObject_ctx);
}

[Serializable]
public class PlantState_GrowthComplete : PlantStateBoolCondition_Abstract
{
    public override bool CheckCondition(Plant_MonoBehavior gardenObject_ctx)
    {
        gardenObject_ctx.growthProgress += Time.deltaTime / gardenObject_ctx.plantObject.growthTime;
        gardenObject_ctx.waterProgress -= Time.deltaTime / gardenObject_ctx.plantObject.waterDrainRate;
        if (gardenObject_ctx.waterProgress <= 0)
        {
            //Debug.Log("Plant Died from low water");
            gardenObject_ctx.DestroyPlant();
            return false;
        }

        //testing emoji system here !!!!!!!!!!!!!!-----------!!!!!!!!!
        if (gardenObject_ctx.waterProgress <= .25f && gardenObject_ctx._GifPlayer.IsGifAlreadyPlayingOrQueued("Need Water"))
        {
            //Debug.Log("Need Water");
            gardenObject_ctx._GifPlayer.PlayGif("Need Water");
        }
        /*
        if(gardenObject_ctx.waterProgress >= .95 && gardenObject_ctx._GifPlayer.GetGifName() != "Happy")
        {
            Debug.Log("Happy");
            gardenObject_ctx._GifPlayer.PlayGif("Happy", 2f);

        }
        */

        if (gardenObject_ctx.growthProgress >= 1f)
        {
            gardenObject_ctx._GifPlayer.StopGif();
            return true;
        }

        if (gardenObject_ctx.plantObject.CheckForGrowthChange(gardenObject_ctx.growthProgress, gardenObject_ctx.lastProgressUpdate))
        {
            //Debug.Log("Growth Change");
            gardenObject_ctx.lastProgressUpdate = gardenObject_ctx.growthProgress;
            gardenObject_ctx.particleEvent.Invoke();
            //if the plant state is not already sprout then set to sprout
            if (gardenObject_ctx.plantState == PlantState.Seed)
            {
                //Debug.Log("Setting to Sprout");
                gardenObject_ctx.SetPlantState(PlantState.Sprout);
            }
            gardenObject_ctx._spriteRenderer.sprite = gardenObject_ctx.plantObject.GetSprite(gardenObject_ctx.growthProgress);
            gardenObject_ctx.shadowRenderer.sprite = gardenObject_ctx.plantObject.GetSprite(gardenObject_ctx.growthProgress);
        }
        return false;
    }
}

//plantstate_rotcomplete
[Serializable]
public class PlantState_RotComplete : PlantStateBoolCondition_Abstract
{
    public override bool CheckCondition(Plant_MonoBehavior gardenObject_ctx)
    {
        gardenObject_ctx.rotProgress += Time.deltaTime / gardenObject_ctx.plantObject.rotTime;
        if (gardenObject_ctx.rotProgress >= 1f) return true;
        return false;
    }
}


[Serializable]
public class PlantState_Null : PlantStateBoolCondition_Abstract
{
    public override bool CheckCondition(Plant_MonoBehavior gardenObject_ctx)
    {
        return false;
    }
}
