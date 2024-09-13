using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Tween_Float
{
    [BoxGroup("Tween Settings")]
    [SerializeField]
    private float duration;
    public float Duration { get { return duration; } }

    [BoxGroup("Tween Settings")]
    [SerializeField]
    private float delay;
    public float Delay { get { return delay; } }

    [BoxGroup("Tween Settings")]
    [SerializeField]
    private LeanTweenType easeType;
    public LeanTweenType EaseType { get { return easeType; } }

    [BoxGroup("Tween Settings")]
    [SerializeField]
    [ShowIf("easeType", LeanTweenType.animationCurve)]
    [HideReferenceObjectPicker]
    public AnimationCurve animationCurve;

    [BoxGroup("Tween Settings")]
    [SerializeField]
    private bool switchDirectionsOnComplete;
    public bool SwitchDirectionsOnComplete { get { return switchDirectionsOnComplete; } }

    [BoxGroup("Tween Settings")]
    [SerializeField]
    private bool tweenOnce;
    public bool TweenOnce { get { return tweenOnce; } }

    private bool tweenedOnce;

    [BoxGroup("Tween Settings")]
    [SerializeField]
    private bool ignoreIsPlaying;
    public bool IgnoreIsPlaying { get { return ignoreIsPlaying; } }

    [BoxGroup("Tween Settings")]
    [SerializeField]
    private bool setOnStart;
    public bool SetOnStart { get { return setOnStart; } }
    [BoxGroup("Tween Settings")]
    [SerializeField]
    private bool setValueOnInterupt;

    [BoxGroup("Tween Settings")]
    [ReadOnly]
    [SerializeField]
    private bool currentlyTweening = false;
    public bool CurrentlyTweening { get { return currentlyTweening; } }

    [BoxGroup("Tween Settings")]
    [SerializeField]
    private float from, to;
    public float From { get { return from; } }
    public float To { get { return to; } }

    [BoxGroup("Tween Settings")]
    [ReadOnly]
    [ShowIf("setOnStart")]
    [SerializeField]
    private float initialFrom, initialTo;
    public float InitialFrom { get { return initialFrom; } }
    public float InitialTo { get { return initialTo; } }

    [BoxGroup("Tween Settings")]
    [SerializeField]
    private UnityEvent<float> receiveValue;
    public UnityEvent<float> ReceiveValue { get { return receiveValue; } }

    [BoxGroup("Tween Settings")]
    [SerializeField]
    private UnityEvent onComplete;

    [BoxGroup("Tween Settings")]
    [SerializeField]
    private UnityEvent onTweenAComplete;
    [BoxGroup("Tween Settings")]
    [SerializeField]
    private UnityEvent onTweenBComplete;
    public UnityEvent OnComplete { get { return onComplete; } }

    private GameObject self;

    public void SetInitialValues(GameObject _self)
    {
        initialFrom = from;
        initialTo = to;
        self = _self;
        if (setOnStart)
        {
            receiveValue?.Invoke(from);
        }
    }

    public void SetFrom(float _from)
    {
        from = _from;
    }

    public void SetTo(float _to)
    {
        to = _to;
    }

    private float timeTweenStarted;
    private float expectedTimeOfTweenEnd;
    private float lastValue;
    public void Tween()
    {
        if (currentlyTweening && !ignoreIsPlaying)
        {
            if (setValueOnInterupt)
            {
                receiveValue?.Invoke(from);
            }
            return;
        }
        float tempFrom = from;
        float tempDuration = duration;
        if(currentlyTweening && ignoreIsPlaying)
        {
            //cancel self and thats it
            LeanTween.cancel(self);
        }
        if (currentlyTweening && ignoreIsPlaying && setValueOnInterupt)
        {
            //just cancel self
            LeanTween.cancel(self);
            //get the percentage of the tween that has been completed
            float percentageComplete = (Time.time - timeTweenStarted) / (expectedTimeOfTweenEnd - timeTweenStarted);
            tempFrom = lastValue;
            tempDuration = duration - (duration * percentageComplete);

            if (switchDirectionsOnComplete)
            {
                SwitchDirections();
            }
            currentlyTweening = false;
        }
        if (tweenOnce && tweenedOnce) { return; }

        currentlyTweening = true;

        timeTweenStarted = Time.time;
        expectedTimeOfTweenEnd = timeTweenStarted + duration;

        if (easeType == LeanTweenType.animationCurve)
        {
            LeanTween.value(self, tempFrom, to, tempDuration)
                .setEase(animationCurve)
                .setDelay(delay)
                .setOnUpdate((float val) =>
                {
                    receiveValue?.Invoke(val);
                    lastValue = val;
                })
                .setOnComplete(() =>
                {
                    if (switchDirectionsOnComplete)
                    {
                        SwitchDirections();
                    }
                    if (tweenOnce)
                    {
                        tweenedOnce = true;
                    }
                    currentlyTweening = false;
                    onComplete?.Invoke();
                    CheckIfAOrBTween();
                });
        }
        else
        {
            LeanTween.value(self, tempFrom, to, tempDuration)
                .setEase(easeType)
                .setDelay(delay)
                .setOnUpdate((float val) =>
                {
                    receiveValue?.Invoke(val);
                    lastValue = val;
                })
                .setOnComplete(() =>
                {
                    if (switchDirectionsOnComplete)
                    {
                        SwitchDirections();
                    }

                    if (tweenOnce)
                    {
                        tweenedOnce = true;
                    }
                    currentlyTweening = false;
                    onComplete?.Invoke();
                    CheckIfAOrBTween();
                });
        }
    }

    public void SwitchDirections()
    {
        if (to == initialTo)
        {
            from = initialTo;
            to = initialFrom;
        }
        else
        {
            from = initialFrom;
            to = initialTo;
        }
    }

    private void CheckIfAOrBTween()
    {
        if (from == initialFrom && to == initialTo)
        {
            onTweenAComplete?.Invoke();
        }
        else if (from == initialTo && to == initialFrom)
        {
            onTweenBComplete?.Invoke();
        }
    }

    public void TweenA()
    {
        if (currentlyTweening && !ignoreIsPlaying) { return; }
        if (from != initialFrom) { return; }
        if (to != initialTo) { return; }
        from = initialFrom;
        to = initialTo;
        if (currentlyTweening && ignoreIsPlaying && setValueOnInterupt) { receiveValue?.Invoke(from); return; }
        Tween();
    }

    public void TweenB()
    {
        if (currentlyTweening && !ignoreIsPlaying) { return; }
        if (from != initialTo) { return; }
        if (to != initialFrom) { return; }
        from = initialTo;
        to = initialFrom;
        Tween();
    }

    //function to stop tweening
    public void StopTween()
    {
        LeanTween.cancel(self);
        currentlyTweening = false;
    }
}