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
    bool mousePressedDown = false;
    bool isDrawingPath = false;
    Tile startOriginalTile = null;
    Vector3Int startTilePosition;
    Vector3Int previousTilePosition;
    Tile lastTile = null;

    // Start is called before the first frame update
    void Start()
    {
        grid = gameObject.GetComponent<Grid>();
        tileManager = FindObjectOfType<TileManager>();
    }

    // Update is called once per frame
    void Update()
    {
        interactiveMap.CompressBounds();

        // Mouse over -> highlight tile
        Vector3Int mousePos = GetMousePosition();
        if (!mousePos.Equals(previousMousePos) && interactiveMap.cellBounds.Contains(mousePos))
        {
            interactiveMap.SetTile(previousMousePos, null); // Remove old hoverTile
            interactiveMap.SetTile(mousePos, hoverTile);
            previousMousePos = mousePos;
        }

        // Left mouse click -> add path tile (if within bounds)
        if (Input.GetMouseButton(0) && interactiveMap.cellBounds.Contains(mousePos))
        {
            // if starting to draw a path, save the starting path details and set bool
            if (isDrawingPath == false)
            {
                ResetTilesToDefault();

                isDrawingPath = true;
                Vector3Int? closestNeighbourPos = GetStartingNeighbour(mousePos);
                if (closestNeighbourPos != null)
                {
                    
                    startTilePosition = (Vector3Int)closestNeighbourPos;
                    startOriginalTile = pathMap.GetTile<Tile>((Vector3Int)closestNeighbourPos);
                    previousTilePosition = startTilePosition;
                    
                }
                else
                {
                    previousTilePosition = mousePos;
                    lastTile = upTile;
                }
            }

            SetTiles(mousePos);
        }
        else
        {
            isDrawingPath = false;
        }

        // Right mouse click -> remove path tile (if within bounds)
        if (Input.GetMouseButton(1) && interactiveMap.cellBounds.Contains(mousePos))
        {
            pathMap.SetTile(mousePos, defaultTile);
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
        if(gridPosition != previousTilePosition)
        {
            Tile setTile = null;

            // set tile based on direction
            if (gridPosition - Vector3Int.up == previousTilePosition) setTile = upTile;
            else if (gridPosition - Vector3Int.down == previousTilePosition) setTile = downTile;
            else if (gridPosition - Vector3Int.left == previousTilePosition) setTile = leftTile;
            else setTile = rightTile;

            // update tiles
            Debug.Log("Setting tiles: " + gridPosition + " & " + previousTilePosition + " to " + setTile.name);
            pathMap.SetTile(gridPosition, setTile);
            pathMap.SetTile(previousTilePosition, setTile);

            // update previous tile to current tile
            previousTilePosition = gridPosition;
            lastTile = setTile;
        }
        else
        {
            pathMap.SetTile(gridPosition, lastTile);
        }
    }

    private void ResetTilesToDefault()
    {
        // Reset start tile
        if(startOriginalTile != null)
        {
            pathMap.SetTile(startTilePosition, startOriginalTile);
            startOriginalTile = null;
        }
        foreach(Vector3Int cellPos in interactiveMap.cellBounds.allPositionsWithin)
        {
            pathMap.SetTile(cellPos, defaultTile);
        }
    }
}