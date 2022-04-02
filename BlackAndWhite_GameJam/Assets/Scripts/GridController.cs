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
    [SerializeField] private Tile hoverTile = null;

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
        // Mouse over -> highlight tile
        Vector3Int mousePos = GetMousePosition();
        if (!mousePos.Equals(previousMousePos) && interactiveMap.cellBounds.Contains(mousePos))
        {
            interactiveMap.SetTile(previousMousePos, defaultTile); // Remove old hoverTile
            interactiveMap.SetTile(mousePos, hoverTile);
            previousMousePos = mousePos;
        }

        // Left mouse click -> add path tile (if within bounds and on a valid path) && pathMap.GetTile<Tile>(mousePos) == defaultTile
        if (Input.GetMouseButton(0) && interactiveMap.cellBounds.Contains(mousePos) )
        {
            // if starting to draw a path, save the starting path details and set bool
            if (isDrawingPath == false && CurrentPath == null)
            {
                
                Vector3Int? closestNeighbourPos = GetStartingNeighbour(mousePos);
                if (closestNeighbourPos != null)
                {
                    isDrawingPath = true;

                    Vector3Int startTilePosition = (Vector3Int)closestNeighbourPos;
                    Tile startOriginalTile = pathMap.GetTile<Tile>(startTilePosition);

                    CurrentPath = new ConveyerTilePath(defaultTile, startOriginalTile, pathMap);
                    ConveyerTile startTile = new ConveyerTile(startTilePosition, startOriginalTile);
                    CurrentPath.AddTileToPath(startTile);
                }
            }
            else if (isDrawingPath && CurrentPath != null)
            {
                SetTiles(mousePos);
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
            RemoveTiles(mousePos);
        }
    }

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
    private void SetTiles(Vector3Int gridPosition)
    {
        ConveyerTile lastConveyerTile = CurrentPath.GetLastTile();
        Vector3Int previousTilePosition = lastConveyerTile.Position;

        if(gridPosition != previousTilePosition)
        {
            Tile setTile = null;

            // set tile based on direction
            if (gridPosition - Vector3Int.up == previousTilePosition) setTile = upTile;
            else if (gridPosition - Vector3Int.down == previousTilePosition) setTile = downTile;
            else if (gridPosition - Vector3Int.left == previousTilePosition) setTile = leftTile;
            else setTile = rightTile;

            // Add new tile to path

            ConveyerTile conveyerTile = new ConveyerTile(gridPosition, setTile);
            CurrentPath.AddTileToPath(conveyerTile);
        }
    }

    private void RemoveTiles(Vector3Int gridPosition)
    {
        foreach(var path in paths)
        {
            path.RemoveTileFromPath(gridPosition);
        }
    }

}