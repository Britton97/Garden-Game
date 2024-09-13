using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerOnSpacePushed : MonoBehaviour
{
    public UnityEvent onSpacePushed;
    void Update()
    {
        //if the space key is pushed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //invoke the event
            onSpacePushed.Invoke();
        }
    }
}
