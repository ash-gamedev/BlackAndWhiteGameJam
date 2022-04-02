using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ConveyerTilePath 
{
    public ConveyerTilePath(Tile defaultTile, Tile startingTile, Tilemap tilemap)
    {
        DefaultTile = defaultTile;
        StartingTileOriginal = startingTile;
        TileMap = tilemap;
        ConveyerTiles = new List<ConveyerTile>();
    }
    Tilemap TileMap { get; set; }
    Tile DefaultTile { get; set; }
    Tile StartingTileOriginal { get; set; }
    public List<ConveyerTile> ConveyerTiles { get; set; }

    public ConveyerTile GetLastTile()
    {
        ConveyerTile neighbourTile = ConveyerTiles.Last();

        return neighbourTile;
    }

    public void AddTileToPath(ConveyerTile conveyerTile)
    {
        // if tiles already exist, update last neighbour tile
        if (ConveyerTiles?.Count != 0)
        {
            ConveyerTile neighbourTile = ConveyerTiles.Last();
            TileMap.SetTile(neighbourTile.Position, conveyerTile.Tile);
        }

        ConveyerTiles.Add(conveyerTile);
        TileMap.SetTile(conveyerTile.Position, conveyerTile.Tile);
    }

    public void ResetTilesToDefault()
    {
        if (ConveyerTiles?.Count > 0)
        {
            ConveyerTile startingTileConveyer = ConveyerTiles.First();
            TileMap.SetTile(startingTileConveyer.Position, StartingTileOriginal);
            ConveyerTiles.Remove(startingTileConveyer);
        }

        foreach (ConveyerTile conveyerTile in ConveyerTiles)
        {
            TileMap.SetTile(conveyerTile.Position, DefaultTile);
        }

        ConveyerTiles.Clear();
    }
}