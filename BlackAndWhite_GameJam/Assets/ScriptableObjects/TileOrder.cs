using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileOrder : ScriptableObject
{
    public TileBase[] tiles;

    public EnumOrder order;

}
