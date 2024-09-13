using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Axis axis;
    public SelforOther selfOrOther;
    [ShowIf("selfOrOther", SelforOther.Other)]
    public GameObject target;
    public float rotateSpeed;

    private void Start()
    {
        if (selfOrOther == SelforOther.Other)
        {
            if (target == null)
            {
                Debug.LogError("Target is null");
            }
        }
        else
        {
            target = gameObject;
        
        }
    }
    void FixedUpdate()
    {
        switch (axis)
        {
            case Axis.X:
                target.transform.Rotate(rotateSpeed * Time.deltaTime, 0, 0);
                break;
            case Axis.Y:
                target.transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
                break;
            case Axis.Z:
                target.transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
                break;
        }
    }
}

public enum Axis
{
    X,
    Y,
    Z
}

public enum SelforOther
{
    Self,
    Other
}