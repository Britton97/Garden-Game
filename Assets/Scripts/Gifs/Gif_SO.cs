using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
[InlineProperty]
[CreateAssetMenu(fileName = "Gif", menuName = "ScriptableObjects/Gif", order = 1)]
public class Gif_SO : SerializedScriptableObject
{
    //list of sprites
    public string gifName;
    public List<Sprite> gifSprites;
    public float frameRate = 0.1f;
    public bool loop = true;
    public bool playOnAwake = true;
    public bool playOnEnable = false;
}
