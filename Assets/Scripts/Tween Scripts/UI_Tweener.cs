using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UI_Tweener : MonoBehaviour
{
    public Tween_Float tweener;
    public tweenType tweenType;
    public RectTransform referenceRectTransform;
    public bool useParentRectTransform;
    public TweenDirection tweenDirection;
    public ScreenSpaceTweenType screenSpaceTweenType;
    private RectTransform rectTransform;

    [ShowIfGroup("screenSpaceTweenType", Value = ScreenSpaceTweenType.ScreenSpace)]
    [BoxGroup("screenSpaceTweenType/From")]
    [Range(0, 1)]
    public float fromScreenPercentage;
    [BoxGroup("screenSpaceTweenType/From")]
    public float fromBuffer;
    [BoxGroup("screenSpaceTweenType/From")]
    [LabelText("Account for RectTransform Size")]
    public bool accoutForRectSizeFrom;


    [BoxGroup("screenSpaceTweenType/To")]
    [Range(0, 1)]
    public float toScreenPercentage;
    [BoxGroup("screenSpaceTweenType/To")]
    public float toBuffer;
    [BoxGroup("screenSpaceTweenType/To")]
    [LabelText("Account for RectTransform Size")]
    public bool accoutForRectSizeTo;


    private float multiplier;

    public void DebugMessage()
    {
        Debug.Log("Tweening");
    }

    private void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();

        if (useParentRectTransform)
        {
            referenceRectTransform = gameObject.transform.parent.GetComponent<RectTransform>();
        }

        switch (tweenType)
        {
            case tweenType.Move:
                SetUpTweenPosition();
                break;
            case tweenType.Scale:
                SetUpTweenScale();
                break;
        }

        tweener.SetInitialValues(gameObject);
    }

    public void SetUpTweenPosition()
    {
        if (screenSpaceTweenType == ScreenSpaceTweenType.TypedValue)
        {
            if (tweenDirection == TweenDirection.Width)
            {
                tweener.SetFrom(-gameObject.GetComponent<RectTransform>().anchoredPosition.x);
            }
            else
            {
                tweener.SetFrom(gameObject.GetComponent<RectTransform>().anchoredPosition.y);
            }
        }
        else
        {
            if (tweenDirection == TweenDirection.Width)
            {
                float val;
                if(referenceRectTransform != null)
                {
                    val = referenceRectTransform.sizeDelta.x;
                }
                else
                {
                    val = Screen.width;
                }
                SetAnchorMultiplier();

                if (accoutForRectSizeFrom)
                {
                    float tempVal = ((val * fromScreenPercentage) * multiplier) + (gameObject.GetComponent<RectTransform>().rect.width * -multiplier) + (fromBuffer * multiplier);
                    tweener.SetFrom(tempVal);
                }
                else
                {
                    float tempVal = (val * fromScreenPercentage) * multiplier + (fromBuffer * multiplier);
                    tweener.SetFrom(tempVal);
                }
                if (accoutForRectSizeTo)
                {
                    float tempVal = ((val * toScreenPercentage) * multiplier) + (gameObject.GetComponent<RectTransform>().rect.width * -multiplier) + (toBuffer * multiplier);
                    tweener.SetTo(tempVal);
                }
                else
                {
                    float tempVal = (val * toScreenPercentage) * multiplier + (toBuffer * multiplier);
                    tweener.SetTo(tempVal);
                }
            }
            else
            {
                float val;
                if (referenceRectTransform != null)
                {
                    val = referenceRectTransform.sizeDelta.y;
                }
                else
                {
                    val = Screen.height;
                }
                SetAnchorMultiplier();

                if (accoutForRectSizeFrom)
                {
                    float tempVal = ((val * fromScreenPercentage) * multiplier) + (gameObject.GetComponent<RectTransform>().rect.height * -multiplier) + (fromBuffer * multiplier);
                    tweener.SetFrom(tempVal);
                }
                else
                {
                    float tempVal = (val * fromScreenPercentage) * multiplier + (fromBuffer * multiplier);
                    tweener.SetFrom(tempVal);
                }
                if (accoutForRectSizeTo)
                {
                    float tempVal = ((val * toScreenPercentage) * multiplier) + (gameObject.GetComponent<RectTransform>().rect.height * -multiplier) + (toBuffer * multiplier);
                    tweener.SetTo(tempVal);
                }
                else
                {
                    float tempVal = (val * toScreenPercentage) * multiplier + (toBuffer * multiplier);
                    tweener.SetTo(tempVal);
                }
            }
        }
    }

    private void TweenPosition(float value)
    {
        if (screenSpaceTweenType == ScreenSpaceTweenType.ScreenSpace)
        {
            //if screen space then move the object 50% of the screen width
            if (tweenDirection == TweenDirection.Width)
            {
                rectTransform.anchoredPosition = new Vector2(value, rectTransform.anchoredPosition.y);
            }
            else
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, value);
            }
        }
        else
        {
            if (tweenDirection == TweenDirection.Height)
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, value);
            }
            else
            {
                //rectTransform.sizeDelta = new Vector2(value, rectTransform.sizeDelta.y);
                rectTransform.anchoredPosition = new Vector2(value, rectTransform.anchoredPosition.y);
            }
        }
    }

    public void SetUpTweenScale()
    {
        if (screenSpaceTweenType == ScreenSpaceTweenType.TypedValue)
        {
            if (tweenDirection == TweenDirection.Width)
            {
                tweener.SetFrom(gameObject.GetComponent<RectTransform>().sizeDelta.x);
            }
            else
            {
                tweener.SetFrom(gameObject.GetComponent<RectTransform>().sizeDelta.y);
            }
        }
        else
        {
            if (tweenDirection == TweenDirection.Width)
            {
                float val;
                if(referenceRectTransform != null)
                {
                    val = referenceRectTransform.sizeDelta.x;
                }
                else
                {
                    val = Screen.width;
                }
                Debug.Log(val);

                if (accoutForRectSizeFrom)
                {
                    float tempVal = (val * fromScreenPercentage) + gameObject.GetComponent<RectTransform>().rect.width + fromBuffer;
                    tweener.SetFrom(tempVal);
                }
                else
                {
                    float tempVal = (val * fromScreenPercentage) + fromBuffer;
                    tweener.SetFrom(tempVal);
                }
                if (accoutForRectSizeTo)
                {
                    float tempVal = (val * toScreenPercentage)+ gameObject.GetComponent<RectTransform>().rect.width + toBuffer;
                    tweener.SetTo(tempVal);
                }
                else
                {
                    float tempVal = (val * toScreenPercentage) + toBuffer;
                    tweener.SetTo(tempVal);
                }
            }
            else
            {
                float val;
                if (referenceRectTransform != null)
                {
                    val = referenceRectTransform.sizeDelta.y;
                }
                else
                {
                    val = Screen.height;
                }
                SetAnchorMultiplier();

                if (accoutForRectSizeFrom)
                {
                    float tempVal = (val * fromScreenPercentage) + gameObject.GetComponent<RectTransform>().rect.height + fromBuffer;
                    tweener.SetFrom(tempVal);
                }
                else
                {
                    float tempVal = (val * fromScreenPercentage) + fromBuffer;
                    tweener.SetFrom(tempVal);
                }
                if (accoutForRectSizeTo)
                {
                    float tempVal = (val * toScreenPercentage) + gameObject.GetComponent<RectTransform>().rect.height + toBuffer;
                    tweener.SetTo(tempVal);
                }
                else
                {
                    float tempVal = (val * toScreenPercentage) + toBuffer;
                    tweener.SetTo(tempVal);
                }
            }
        }
    }

    private void TweenScale(float value)
    {
        if (tweenDirection == TweenDirection.Height)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, value);
        }
        else
        {
            rectTransform.sizeDelta = new Vector2(value, rectTransform.sizeDelta.y);
        }
    }
    
    public void ReceiveValue(float value)
    {
        switch (tweenType)
        {
            case tweenType.Move:
                TweenPosition(value);
                break;
            case tweenType.Scale:
                TweenScale(value);
                break;
        }
    }

    public void Tween()
    {
        tweener.Tween();
    }

    public void TweenA()
    {
        tweener.TweenA();
    }

    public void TweenB()
    {
        tweener.TweenB();
    }

    public void SetAnchorMultiplier()
    {
        if (tweenDirection == TweenDirection.Width)
        {
            if (rectTransform.anchorMin.x == 0 && rectTransform.anchorMin.x == 0)
            {
                multiplier = 1;
            }
            else if (rectTransform.anchorMin.x == 1 && rectTransform.anchorMax.x == 1)
            {
                multiplier = -1;
            }
        }
        else
        {
            if (rectTransform.anchorMin.y == 1 && rectTransform.anchorMax.y == 1)
            {
                multiplier = -1;
            }
            else if (rectTransform.anchorMin.y == 0 && rectTransform.anchorMax.y == 0)
            {
                multiplier = 1;
            }
        }
    }
}

public enum TweenDirection
{
    Height,
    Width
}

public enum tweenType
{
    Move,
    Scale
}

public enum ScreenSpaceTweenType
{
    ScreenSpace,
    TypedValue
}