using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Tween_Vector3
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
    [ReadOnly]
    [SerializeField]
    private bool currentlyTweening = false;
    public bool CurrentlyTweening { get { return currentlyTweening; } }

    [BoxGroup("Tween Settings")]
    [SerializeField]
    private Vector3 from, to;
    public Vector3 From { get { return from; } }
    public Vector3 To { get { return to; } }

    [BoxGroup("Tween Settings")]
    [ReadOnly]
    [SerializeField]
    private Vector3 initialFrom, initialTo;
    public Vector3 InitialFrom { get { return initialFrom; } }
    public Vector3 InitialTo { get { return initialTo; } }

    [SerializeField]
    private UnityEvent<Vector3> receiveValue;
    public UnityEvent<Vector3> ReceiveValue { get { return receiveValue; } }

    [SerializeField]
    private UnityEvent onComplete;
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

    public void Tween()
    {
        if (currentlyTweening && !ignoreIsPlaying) { return; }
        if (currentlyTweening && ignoreIsPlaying)
        {
            LeanTween.cancel(self);
            if (switchDirectionsOnComplete)
            {
                SwitchDirections();
            }
            currentlyTweening = false;
        }
        if (tweenOnce && tweenedOnce) { return; }

        currentlyTweening = true;
        LeanTween.value(self, from, to, duration).setEase(easeType).setDelay(delay).setOnUpdate((Vector3 val) =>
        {
            receiveValue?.Invoke(val);
        }).setOnComplete(() =>
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
        });
    }

    //Tween that receives a two vector3s to tween between
    public void Tween(Vector3 _from, Vector3 _to)
    {
        from = _from;
        to = _to;
        Tween();
    }

    public void TweenBetweenObjects(GameObject startObject, GameObject endObject)
    {
        if (currentlyTweening && !ignoreIsPlaying) { return; }
        if (currentlyTweening && ignoreIsPlaying)
        {
            LeanTween.cancel(self);
            if (switchDirectionsOnComplete)
            {
                SwitchDirections();
            }
            currentlyTweening = false;
        }
        if (tweenOnce && tweenedOnce) { return; }

        currentlyTweening = true;
        LeanTween.move(self, endObject.transform.position, duration).setEase(easeType).setDelay(delay).setOnUpdate((Vector3 val) =>
        {
            receiveValue?.Invoke(val);
        }).setOnComplete(() =>
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
        });
    }

    public void SwitchDirections()
    {
        Vector3 temp = from;
        from = to;
        to = temp;
    }
}