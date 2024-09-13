using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenScale : MonoBehaviour
{
    [SerializeField] Tween_Float tweener;

    public Axis axis;

    void Start()
    {
        tweener.SetInitialValues(gameObject);
    }

    private Vector3 scale;
    public void Scale(float val)
    {
        switch (axis)
        {
            case Axis.X:
                transform.localScale = new Vector3(val, 1, 1);
                break;
            case Axis.Y:
                transform.localScale = new Vector3(1, val, 1);
                break;
            case Axis.Z:
                transform.localScale = new Vector3(1, 1, val);
                break;
        }
    }

    public void _TweenScale()
    {
        tweener.Tween();
    }

    public void StopTween()
    {
        tweener.StopTween();
    }

    public void TweenA()
    {
        tweener.TweenA();
    }

    public void TweenB()
    {
        tweener.TweenB();
    }
}
