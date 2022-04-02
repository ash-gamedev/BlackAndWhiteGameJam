using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileConveyer : ScriptableObject
{
    public TileBase[] tiles;

    public Vector2 direction;
}
