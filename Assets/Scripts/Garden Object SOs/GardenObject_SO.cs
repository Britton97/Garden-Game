using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
public class GardenObject_SO : SerializedScriptableObject
{
    [SerializeField] private Sprite objectSprite;
    public Sprite gardenObjectSprite { get { return objectSprite; } }
    [SerializeField] private string gardenObjectName;
    public string GardenObjectName { get { return GetName(); } }
    public int maxQuantity = 10;
    public GameObject objectPrefab;

    protected virtual string GetName()
    {
        return gardenObjectName;
    }

    public virtual void ResetProgess(){}
}

[Serializable]
//[InlineEditor]
public abstract class ItemRequirement_Abs
{
    public abstract string ItemName { get;}
    [MinValue(1)]public int requiredQuantity;
    public bool requirementDiscovered = false;
    public abstract int GetCount();
    public abstract Sprite GetSprite();
    public abstract string GetName();
}

[Serializable]
[InlineEditor]
public class GardenItemRequirement : ItemRequirement_Abs
{
    [SerializeField] public GardenObject_SO _gardenObject;
    [HideInInspector] public override string ItemName { get { return _gardenObject.GardenObjectName; } }
    public RequirementType requirementType;

    public override int GetCount()
    {
        return GardenManager.Instance.GetQuantityOfItem(_gardenObject.GardenObjectName);
    }

    public override Sprite GetSprite()
    {
        return _gardenObject.gardenObjectSprite;
    }

    public override string GetName()
    {
        return _gardenObject.GardenObjectName;
    }
    //make a enum for the type of requirement
}
[Serializable]
[InlineEditor]
public class TileRequirement : ItemRequirement_Abs
{
    [SerializeField] public Tile_SO _tile;
    [HideInInspector] public override string ItemName { get { return _tile.tileName; } }

    public override int GetCount()
    {
        //Debug.Log($"Getting the tile count for {_tile.tileName}");
        return TileMap_Manager.Instance.GetTileAmount(_tile.tileName);
    }

    public override Sprite GetSprite()
    {
        return _tile.tileSprite;
    }

    public override string GetName()
    {
        return _tile.tileName;
    }
}

public enum RequirementType
{
    EatItem,
    ItemInGarden
}
