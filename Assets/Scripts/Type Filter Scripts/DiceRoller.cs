using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class DiceRoller : MonoBehaviour
{
    public int diceSides = 6;
    public int result;
}

[SerializeField]
[InlineProperty]
public class RandomInt : SubscribeToEvent_Int
{
    public int min = 1;
    public int max = 6;
    public override int ReturnValue()
    {
        return Random.Range(min, max + 1);
    }
}

[SerializeField]
[InlineProperty]
public class RandomFloat : SubscribeToEvent_Float
{
    public float min = 0f;
    public float max = 1f;
    public override float ReturnValue()
    {
        return Random.Range(min, max);
    }
}

[SerializeField]
[InlineProperty]
public class ReturnGameObject : SubscribeToEvent_GameObject
{
    public GameObject gameObject;
    public override GameObject ReturnValue()
    {
        return gameObject;
    }
}