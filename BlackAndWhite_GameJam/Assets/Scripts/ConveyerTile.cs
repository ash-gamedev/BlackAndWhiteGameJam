using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ConveyerTile 
{
    public ConveyerTile(Vector3Int position, Tile tile)
    {
        Position = position;
        Tile = tile;
    }

    public Vector3Int Position { get; set; }
    public Tile Tile { get; set; }
}