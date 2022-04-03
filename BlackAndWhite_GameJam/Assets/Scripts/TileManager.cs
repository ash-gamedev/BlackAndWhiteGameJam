using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap tileConveyerMap;

    [SerializeField]
    private List<TileConveyer> tileConveyers;

    [SerializeField]
    public float ConveyerSpeed = 2;

    private Dictionary<TileBase, TileConveyer> tileConveyerFromTileBase;
    private Dictionary<EnumTileDirection, Vector3> vectorByDirection;
    private Dictionary<Vector3Int, TileBase> defaultBorderTileConveyers;

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

        // Direction dictionary
        vectorByDirection = new Dictionary<EnumTileDirection, Vector3>()
        {
            { EnumTileDirection.Up, Vector3.up },
            { EnumTileDirection.Down, Vector3.down },
            { EnumTileDirection.Left, Vector3.left },
            { EnumTileDirection.Right, Vector3.right },
            { EnumTileDirection.None, Vector3.zero }
        };

        defaultBorderTileConveyers = new Dictionary<Vector3Int, TileBase>();
        foreach (Vector3Int cellPos in tileConveyerMap.cellBounds.allPositionsWithin)
        {
            TileBase tile = tileConveyerMap.GetTile(cellPos);
            if (tile != null && tileConveyerFromTileBase.ContainsKey(tile))
            {
                defaultBorderTileConveyers[cellPos] = tile;
            }
        }
    }

    public Vector3 GetTileDirection(Vector2 worldPosition)
    {
        Vector3 direction = Vector3.zero;

        Vector3Int gridPosition = tileConveyerMap.WorldToCell(worldPosition);

        TileBase tile = tileConveyerMap.GetTile(gridPosition);

        try
        {
            if (tile != null && tileConveyerFromTileBase.ContainsKey(tile))
            {
                TileConveyer tileConveyer = tileConveyerFromTileBase[tile];
                if (tileConveyer != null)
                {
                    EnumTileDirection tileDirection = tileConveyerFromTileBase[tile].TileDirection;
                    direction = vectorByDirection[tileDirection];
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log(worldPosition + " " + gridPosition);
        }
                
        return direction;
    }

    public Vector3Int GetTileGridPosition(Vector2 worldPosition)
    {
        return tileConveyerMap.WorldToCell(worldPosition);
    }

    public Dictionary<EnumNeighbour, Tuple<EnumTileDirection, Vector3Int>> GetAllNeighborTiles(Vector3Int gridPosition)
    {
        Dictionary<EnumNeighbour, Tuple<EnumTileDirection, Vector3Int>> neighbourTiles = new Dictionary<EnumNeighbour, Tuple<EnumTileDirection, Vector3Int>>();

        neighbourTiles[EnumNeighbour.Right] = GetRightNeighbourTileDirection(gridPosition);
        neighbourTiles[EnumNeighbour.Left] = GetLeftNeighbourTileDirection(gridPosition);
        neighbourTiles[EnumNeighbour.Top] = GetUpNeighbourTileDirection(gridPosition);
        neighbourTiles[EnumNeighbour.Bottom] = GetDownNeighbourTileDirection(gridPosition);

        return neighbourTiles;
    }

    public Tuple<EnumTileDirection, Vector3Int> GetLeftNeighbourTileDirection(Vector3Int gridPosition)
    {
        return GetNeighbourTileDirection(gridPosition, Vector3Int.left);
    }

    public Tuple<EnumTileDirection, Vector3Int> GetRightNeighbourTileDirection(Vector3Int gridPosition)
    {
        return GetNeighbourTileDirection(gridPosition, Vector3Int.right);
    }

    public Tuple<EnumTileDirection, Vector3Int> GetUpNeighbourTileDirection(Vector3Int gridPosition)
    {
        return GetNeighbourTileDirection(gridPosition, Vector3Int.up);
    }

    public Tuple<EnumTileDirection, Vector3Int> GetDownNeighbourTileDirection(Vector3Int gridPosition)
    {
        return GetNeighbourTileDirection(gridPosition, Vector3Int.down);
    }

    private Tuple<EnumTileDirection, Vector3Int> GetNeighbourTileDirection(Vector3Int gridPosition, Vector3Int neighbourDirection)
    {
        EnumTileDirection tileDirection = EnumTileDirection.None;

        Vector3Int neighborPos = gridPosition + neighbourDirection;

        TileBase neighbourTile = tileConveyerMap.GetTile(neighborPos);

        if(neighbourTile != null && tileConveyerFromTileBase.ContainsKey(neighbourTile))
        {
            tileDirection = tileConveyerFromTileBase[neighbourTile].TileDirection;
        }

        return new Tuple<EnumTileDirection, Vector3Int>(tileDirection, neighborPos);
    }

    public EnumNeighbour GetTileNeighbour(Vector3Int gridposition, Vector3Int previousPosition)
    {
        if (gridposition + Vector3Int.up == previousPosition) return EnumNeighbour.Top;
        else if (gridposition + Vector3Int.down == previousPosition) return EnumNeighbour.Bottom;
        else if (gridposition + Vector3Int.right == previousPosition) return EnumNeighbour.Right;
        else return EnumNeighbour.Left;
    }

    public void SetTileOnConveyer(Vector3Int gridPosition, EnumTileDirection tileDirection)
    {
        if (defaultBorderTileConveyers.ContainsKey(gridPosition))
        {
            TileConveyer tileConveyer = tileConveyers.FirstOrDefault(x => x.TileDirection == tileDirection);
            TileBase tile = tileConveyerFromTileBase.FirstOrDefault(x => x.Value == tileConveyer).Key;
            tileConveyerMap.SetTile(gridPosition, tile);
        }
    }

    public void ResetTileOnConveyer(Vector3Int gridPosition)
    {
        if (defaultBorderTileConveyers.ContainsKey(gridPosition))
        {
            TileBase tile = defaultBorderTileConveyers[gridPosition];
            tileConveyerMap.SetTile(gridPosition, tile);
        }
    }

    public bool IsOnConveyerTile(Vector2 worldPosition)
    {
        Vector3Int gridPosition = tileConveyerMap.WorldToCell(worldPosition);

        TileBase tile = tileConveyerMap.GetTile(gridPosition);

        if (tile != null && tileConveyerFromTileBase.ContainsKey(tile))
        {
            return true;
        }

        return false;
    }
}
