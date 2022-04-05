using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class GridController : MonoBehaviour
{
    private Grid grid;
    [SerializeField] private Tilemap interactiveMap = null;
    [SerializeField] private Tilemap pathMap = null;
    //[SerializeField] private Tile hoverTile = null;

    [SerializeField] private Tile defaultTile = null;
    [SerializeField] private Tile upTile = null;
    [SerializeField] private Tile downTile = null;
    [SerializeField] private Tile leftTile = null;
    [SerializeField] private Tile rightTile = null;

    private Vector3Int previousMousePos = new Vector3Int();

    TileManager tileManager;

    //
    bool isDrawingPath = false;
    List<ConveyerTilePath> paths = new List<ConveyerTilePath>();

    ConveyerTilePath currentPath;
    ConveyerTilePath CurrentPath
    {
        get
        {
            return this.currentPath;
        }
        set
        {
            this.currentPath = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        grid = gameObject.GetComponent<Grid>();
        tileManager = FindObjectOfType<TileManager>();

        interactiveMap.CompressBounds();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int mousePos = GetMousePosition();

        

        // Mouse over -> highlight tile
        if (!mousePos.Equals(previousMousePos) && interactiveMap.cellBounds.Contains(mousePos))
        {
            SetHoverTile(mousePos);
        }
        else if (!interactiveMap.cellBounds.Contains(mousePos))
        {
            interactiveMap.SetTile(previousMousePos, null); // Remove old hoverTile
        }

        // Left mouse click -> add path tile (if within bounds and on a valid path) && pathMap.GetTile<Tile>(mousePos) == defaultTile
        if (Input.GetMouseButton(0) && interactiveMap.cellBounds.Contains(mousePos) )
        {
            // if starting to draw a path, save the starting path details and set bool
            if (isDrawingPath == false && CurrentPath == null)
            {
                isDrawingPath = true;

                // get closest neighbour to mouse position
                Vector3Int? closestNeighbourPos = GetStartingNeighbour(mousePos);

                if (closestNeighbourPos != null)
                {
                    Vector3Int startTilePosition = (Vector3Int)closestNeighbourPos;
                    Tile startOriginalTile = pathMap.GetTile<Tile>(startTilePosition);

                    CurrentPath = new ConveyerTilePath(startOriginalTile);
                    ConveyerTile startTile = new ConveyerTile(startTilePosition, startOriginalTile);
                    CurrentPath.AddTileToPath(startTile);
                }
                else
                {
                    CurrentPath = new ConveyerTilePath(null);
                }
            }
            else if (isDrawingPath && CurrentPath != null)
            {
                SetConveyorTiles(mousePos);
            }
        }
        else
        {
            if (isDrawingPath)
            {
                paths.Add(CurrentPath);
                CurrentPath = null;
                isDrawingPath = false;
            }
        }

        // Right mouse click -> remove path tile (if within bounds)
        if (Input.GetMouseButton(1) && interactiveMap.cellBounds.Contains(mousePos))
        {
            RemoveConveyorTile(mousePos);
        }
    }

    #region Setting/Removing Tiles 
    void SetConveyorTiles(Vector3Int mousePos)
    {
        Vector3Int? previousTilePosition = null;

        ConveyerTile lastConveyerTile = CurrentPath.GetLastTile();
        if (lastConveyerTile != null)
        {
            previousTilePosition = lastConveyerTile.Position;
        }

        if (mousePos != previousTilePosition)
        {
            Tile setTile = null;

            // set tile based on direction
            if (mousePos - Vector3Int.up == previousTilePosition) setTile = upTile;
            else if (mousePos - Vector3Int.down == previousTilePosition) setTile = downTile;
            else if (mousePos - Vector3Int.left == previousTilePosition) setTile = leftTile;
            else if (mousePos - Vector3Int.right == previousTilePosition) setTile = rightTile;
            else setTile = upTile;

            ConveyerTile newConveyerTile = new ConveyerTile(mousePos, setTile);

            // update last placed tile
            if (lastConveyerTile != null)
                SetConveyorTile(lastConveyerTile.Position, newConveyerTile.Tile);

            // add new tile to path
            CurrentPath.AddTileToPath(newConveyerTile);

            // set tile for new path
            SetConveyorTile(newConveyerTile.Position, newConveyerTile.Tile);

            // Play sound
            AudioPlayer.PlaySoundEffect(EnumSoundEffects.TileSet);
        }
    }

    void SetConveyorTile(Vector3Int gridPos, Tile conveyorTile)
    {
        // Set converyor tile
        pathMap.SetTile(gridPos, conveyorTile);
    }

    void SetHoverTile(Vector3Int mousePos)
    {
        // Get hover tile
        Vector3Int? neighbourPos = GetStartingNeighbour(mousePos);
        Tile hoverTile = GetConveyorTile(mousePos, neighbourPos);

        // Set tile
        interactiveMap.SetTile(previousMousePos, null); // Remove old hoverTile
        interactiveMap.SetTile(mousePos, hoverTile);
        previousMousePos = mousePos;
    }

    void SetDefaultTile(Vector3Int gridPos)
    {
        pathMap.SetTile(gridPos, defaultTile);
    }

    Tile GetConveyorTile(Vector3Int mousePos, Vector3Int? neighbourPos)
    {
        Tile setTile = null;

        // set tile based on direction
        if(neighbourPos == null) setTile = upTile;
        else if (mousePos - Vector3Int.up == neighbourPos) setTile = upTile;
        else if (mousePos - Vector3Int.down == neighbourPos) setTile = downTile;
        else if (mousePos - Vector3Int.left == neighbourPos) setTile = leftTile;
        else if (mousePos - Vector3Int.right == neighbourPos) setTile = rightTile;

        return setTile;
    }

    private void RemoveConveyorTile(Vector3Int mousePosition)
    {
        // loop through each path created & search for mousePosition
        foreach (var path in paths)
        {
            ConveyerTile removeTile = path.GetTileAtPosition(mousePosition);

            // if position exists
            if (removeTile != null)
            {
                // if tile is second in the path, & start position hasn't been reset
                if(path.ConveyerTiles.IndexOf(removeTile) == 1 && path.HasStartingTileBeenReset == false)
                {
                    // reset start tile to default state & remove from path
                    Tile startPositionTile = path.StartingTileOriginal;
                    pathMap.SetTile(path.ConveyerTiles[0].Position, startPositionTile);

                    path.RemoveStartingTile();
                }

                SetDefaultTile(mousePosition);
                path.RemoveTileFromPath(removeTile);
            }
        }
    }

    #endregion

    Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }

    Vector3Int? GetStartingNeighbour(Vector3Int gridPosition)
    {
        Dictionary<EnumNeighbour, Tuple<EnumTileDirection, Vector3Int>> neighbourTiles = tileManager.GetAllNeighborTiles(gridPosition);

        foreach (KeyValuePair<EnumNeighbour, Tuple<EnumTileDirection, Vector3Int>> neighbourTile in neighbourTiles)
        {
            EnumTileDirection neighbourTileDirection = neighbourTile.Value.Item1;
            Vector3Int neighbourPosition = neighbourTile.Value.Item2;

            if (neighbourTileDirection != EnumTileDirection.None)
            {
                return neighbourPosition;
            }
        }

        return null;
    }

    

}