using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
[InlineProperty]
[CreateAssetMenu(fileName = "Plant Info", menuName = "ScriptableObjects/Plant Info", order = 1)]
public class Plant_SO : GardenObject_SO, iFirstTimeTame
{
    [Unit(Units.Minute)] public float growthTime = 10;
    //float for how long it takes to rot
    [Unit(Units.Minute)]public float rotTime = 10;

    [Unit(Units.Minute)] public float waterDrainRate = 1;
    [SerializeField]
    [ReadOnly]
    //private float maxWaterFillValue = 1;
    public float cost;
    public float sellPrice;
    public float hungerFillValue;
    public GrowthType growthType;
    [ShowIf("growthType", GrowthType.SpriteSheet)]
    public Dictionary<float, Sprite> growthSpriteDict = new Dictionary<float, Sprite>();
    [ShowIf("growthType", GrowthType.Scale)]
    public Vector3 startScale;
    [ShowIf("growthType", GrowthType.Scale)]
    public Vector3 endScale;
    public Sprite fullyGrownSprite;

    public bool alreadyGrownOnce = false;

    public bool FirstTimeTame { get => alreadyGrownOnce; set => alreadyGrownOnce = value;}
    public float firstTimeGrowingExperience = 10;
    public float FirstTimeTameExperience { get => firstTimeGrowingExperience; }

    public Sprite GetSprite(float growthProgress)
    {
        float closestKey = 0;
        foreach (float key in growthSpriteDict.Keys)
        {
            if (key <= growthProgress && key > closestKey)
            {
                closestKey = key;
            }
        }
        return growthSpriteDict[closestKey];
    }

    public bool CheckForGrowthChange(float currentProgress, float lastProgress)
    {
        if(GetNextHighestKey(lastProgress) <= currentProgress)
        {
            return true;
        }
        return false;
    }

    private float GetNextHighestKey(float currentProgress)
    {
        //find the value of the next highest key
        float nextHighestKey = 1;
        foreach (float key in growthSpriteDict.Keys)
        {
            if (key > currentProgress && key < nextHighestKey)
            {
                nextHighestKey = key;
            }
        }
        return nextHighestKey;
    }

    public override void ResetProgess()
    {
        alreadyGrownOnce = false;
    }
}

public enum GrowthType
{
    Scale,
    SpriteSheet
} 
