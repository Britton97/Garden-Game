using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tester : SerializedMonoBehaviour
{
    [OdinSerialize] public EventSubscriberBase eventSubscriberBase;

    public GameObject testObject;

    void Start()
    {

    }
}
