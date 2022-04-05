using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ConveyerTilePath 
{
    public Tile StartingTileOriginal { get; private set; }
    
    public bool HasStartingTileBeenReset { get; private set; }

    public List<ConveyerTile> ConveyerTiles { get; private set; }

    public ConveyerTilePath(Tile startingTile)
    {
        HasStartingTileBeenReset = false;
        StartingTileOriginal = startingTile;
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
        if(HasStartingTileBeenReset == false)
        {
            HasStartingTileBeenReset = true;
            ConveyerTile startingTile = ConveyerTiles[0];

            ConveyerTiles.Remove(startingTile);
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
}