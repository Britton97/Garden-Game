using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class Scale_UI : MonoBehaviour
{
    /*
    [SerializeField] RectTransform rectTransform;
    public Tween_Float tweenFloat;

    void Start()
    {
        if (rectTransform == null)
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
        }
        tweenFloat.SetInitialValues(gameObject);
    }

    public void TweenScale()
    {
        tweenFloat.TweenA();
    }

    public void Scale(float scale)
    {
        rectTransform.localScale = new Vector3(scale, scale, scale);
    }
    */
    void Start()
    {
        Debug.LogWarning("Disabled this script since it's not being used.");
    }
}
