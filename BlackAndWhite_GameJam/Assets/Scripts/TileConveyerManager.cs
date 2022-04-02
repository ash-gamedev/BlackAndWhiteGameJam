using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TileConveyerManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap map;

    [SerializeField]
    private List<TileConveyer> tileConveyers;

    [SerializeField]
    public float ConveyerSpeed = 2;

    private Dictionary<TileBase, TileConveyer> tileConveyerFromTileBase;

    private void Awake()
    {
        tileConveyerFromTileBase = new Dictionary<TileBase, TileConveyer>();

        foreach(var tileConveyer in tileConveyers)
        {
            foreach(var tile in tileConveyer.tiles)
            {
                tileConveyerFromTileBase.Add(tile, tileConveyer);
            }
        }
    }

    public Vector2 GetTileDirection(Vector2 worldPosition)
    {
        Vector2 direction = Vector2.zero;

        Vector3Int gridPosition = map.WorldToCell(worldPosition);

        TileBase tile = map.GetTile(gridPosition);

        if (tile != null)
        {
            TileConveyer tileConveyer = tileConveyerFromTileBase[tile];
            if (tileConveyer != null)
                direction = tileConveyerFromTileBase[tile].direction;
        }
        
        return direction;
    }
}
