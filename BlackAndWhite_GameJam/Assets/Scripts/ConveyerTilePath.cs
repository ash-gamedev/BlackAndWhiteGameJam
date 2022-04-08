using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ConveyerTilePath 
{
    public Vector3Int? StartingGridPosition { get; private set; }
    public List<ConveyerTile> ConveyerTiles { get; private set; }
    public ConveyerTilePath(Vector3Int? startingGridPosition)
    {
        StartingGridPosition = startingGridPosition;
        ConveyerTiles = new List<ConveyerTile>();
    }

    public ConveyerTile GetFirstTile()
    {
        ConveyerTile neighbourTile = ConveyerTiles.FirstOrDefault();

        return neighbourTile;
    }
    public ConveyerTile GetLastTile()
    {
        ConveyerTile neighbourTile = ConveyerTiles.LastOrDefault();

        return neighbourTile;
    }

    public ConveyerTile GetTileAtPosition(Vector3Int position)
    {
        return ConveyerTiles.FirstOrDefault(x => x.Position == position);
    }

    public void RemoveStartingTile()
    {
        if (StartingGridPosition != null)
        {
            ConveyerTile startingTile = GetTileAtPosition((Vector3Int)StartingGridPosition);

            if(startingTile != null)
            {
                ConveyerTiles.Remove(startingTile);
            }
        }
    }

    public void AddTileToPath(ConveyerTile conveyerTile)
    {
        ConveyerTiles.Add(conveyerTile);
    }

    public void RemoveTileFromPath(ConveyerTile conveyerTile)
    {
        ConveyerTiles.Remove(conveyerTile);
    }

    public void RemoveTileFromPath(Vector3Int pathPosition)
    {
        ConveyerTile conveyerTile = GetTileAtPosition(pathPosition);

        if(conveyerTile != null && ConveyerTiles.Contains(conveyerTile))
            ConveyerTiles.Remove(conveyerTile);
    }

    public bool DoesPathHaveStartingTile()
    {
        if (StartingGridPosition == null) return false;

        ConveyerTile conveyerTile = GetTileAtPosition((Vector3Int)StartingGridPosition);

        if (conveyerTile != null)
            return true;
        else
            return false;
    }
}