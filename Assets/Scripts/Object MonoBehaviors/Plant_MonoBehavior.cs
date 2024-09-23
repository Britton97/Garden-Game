using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public enum PlantState
{
    Seed,
    Sprout,
    Mature,
    Dead
}
//[RequireComponent(typeof(SpriteRenderer))]
public class Plant_MonoBehavior : GardenObject_MonoBehavior, iPickupable, iWaterable, iEmoji
{
    public Plant_SO plantObject;
    public PlantState plantState;
    public DataFloat_SO timeScale;
    [SerializeField][ReadOnly] private string currentPlantName;
    [ShowIf("IsSeedOrSprout")]
    [ReadOnly] public float growthProgress = 0;
    [ShowIf("IsSeedOrSprout")]
    [ReadOnly] public float lastProgressUpdate = 0;
    public bool IsSeedOrSprout
    {
        get { return plantState == PlantState.Seed || plantState == PlantState.Sprout; }
    }


    //show if plantstate is mature
    [ShowIf("plantState", PlantState.Mature)]
    [ReadOnly] public float rotProgress = 0;
    [ReadOnly] public float waterProgress = 1;
    [BoxGroup("Plant States")]
    public List<BaseState_Plant> plantStates;
    [BoxGroup("Plant States")]
    public BaseState_Plant currentState;
    public SpriteRenderer _spriteRenderer;
    public SpriteRenderer shadowRenderer;
    public UnityEvent particleEvent;
    public UnityEvent finishedGrowingEvent;
    public Tween_Float squashStretchTween;
    public Tween_Float jumpTween;

    [SerializeField] private GifPlayer gifPlayer;
    public GifPlayer _GifPlayer => gifPlayer;


    public override void Start()
    {
        alreadyRemoved = false;
        SetName();
        squashStretchTween.SetInitialValues(_spriteRenderer.gameObject);
        jumpTween.SetInitialValues(_spriteRenderer.gameObject);
        GardenManager.Instance.AddGardenItem(this);
        if (currentState == null)
        {
            currentState = plantStates[0];
        }
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    #region Get Info Functions
    public override string GetName()
    {
        return currentPlantName;
    }
    public void SetName()
    {
        switch (plantState)
        {
            case PlantState.Seed:
                currentPlantName = plantObject.GardenObjectName + " (Seed)";
                break;
            case PlantState.Sprout:
                currentPlantName = plantObject.GardenObjectName + " (Sprout)";
                break;
            case PlantState.Mature:
                currentPlantName = plantObject.GardenObjectName;
                break;
            case PlantState.Dead:
                currentPlantName = plantObject.GardenObjectName + " (Dead)";
                break;
            default:
                currentPlantName = plantObject.GardenObjectName;
                break;
        }
    }
    public override Sprite GetSprite()
    {
        return plantObject.gardenObjectSprite;
    }

    public override int GetSellPrice()
    {
        return (int)plantObject.sellPrice;
    }
    #endregion
    #region Interface Functions
    public override bool IsSellable()
    {
        return plantState == PlantState.Mature;
    }

    public bool IsPickupable()
    {
        //if plant state is mature, return true
        return plantState == PlantState.Mature;
    }

    public override bool IsEdible()
    {
        if (plantState == PlantState.Mature)
        {
            return true;
        }
        return false;
    }

    public bool IsWaterable()
    {
        if (plantState == PlantState.Seed || plantState == PlantState.Sprout)
        {
            return true;
        }
        return false;
    }

    public void Water(float waterAmount)
    {
        if (plantState == PlantState.Seed || plantState == PlantState.Sprout)
        {
            waterProgress += waterAmount;
            if (waterProgress > 1)
            {
                waterProgress = 1;
            }
        }
    }
    #endregion

    public void DestroyPlant()
    {
        Destroy(gameObject);
    }

    public void SetPlantState(PlantState newState)
    {
        if (plantState == newState) return;
        //Debug.Log($"Setting Plant State to {newState} from {plantState}");
        GardenManager.Instance.RemoveGardenItem(this);
        plantState = newState;
        SetName();
        GardenManager.Instance.AddGardenItem(this);
    }

    #region Plant Tween Functions
    public void SquashStretch()
    {
        squashStretchTween.Tween();
    }
    public void SquashStretchReceiveValue(float value)
    {
        _spriteRenderer.gameObject.transform.localScale = new Vector3(value, value, _spriteRenderer.gameObject.transform.localScale.z);
    }

    public void JumpTween()
    {
        startJumpValue = _spriteRenderer.gameObject.transform.localPosition.y;
        jumpTween.Tween();
        finishedGrowingEvent.Invoke();
    }
    private float startJumpValue;

    public void JumpTweenReceiveValue(float value)
    {
        _spriteRenderer.gameObject.transform.localPosition = new Vector3(_spriteRenderer.gameObject.transform.localPosition.x, startJumpValue + value, _spriteRenderer.gameObject.transform.localPosition.z);
    }
    #endregion
}
