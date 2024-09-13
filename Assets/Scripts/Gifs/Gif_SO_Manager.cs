using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Gif_SO_Manager", menuName = "ScriptableObjects/Gif_SO_Manager", order = 1)]
public class Gif_SO_Manager : SerializedScriptableObject
{
    public List<Gif_SO> gif_SO_List;
    public Gif_SO GetGif_SO(string gifName)
    {
        foreach (Gif_SO gif_SO in gif_SO_List)
        {
            if (gif_SO.gifName == gifName)
            {
                return gif_SO;
            }
        }
        return null;
    }
}
