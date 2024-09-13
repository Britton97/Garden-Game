using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tile", menuName = "Tile SO")]
public class Tile_SO : SerializedScriptableObject
{
    public string tileName;
    public TileBase tileBase;
    public int cost;
    public Sprite tileSprite;
}
