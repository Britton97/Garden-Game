using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenRotate : MonoBehaviour
{
    [SerializeField] Tween_Float tweener;

    public Axis axis;

    void Start()
    {
        tweener.SetInitialValues(gameObject);
    }

    private Vector3 rot;
    public void Rotate(float val)
    {
        switch (axis)
        {
            case Axis.X:
                transform.eulerAngles = new Vector3(val, 0, 0);
                break;
            case Axis.Y:
                transform.eulerAngles = new Vector3(0, val, 0);
                break;
            case Axis.Z:
                transform.eulerAngles = new Vector3(0, 0, val);
                break;
        }
    }

    public void _TweenRotate()
    {
        tweener.Tween();
    }

    public void StopTween()
    {
        tweener.StopTween();
    }
}
