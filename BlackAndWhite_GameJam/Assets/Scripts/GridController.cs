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
            SetTiles(mousePos);
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

    private void SetTiles(Vector3Int gridPosition)
    {
        Dictionary<EnumNeighbour, Tuple<EnumTileDirection, Vector3Int>> neighbourTiles = tileManager.GetAllNeighborTiles(gridPosition);
        
        foreach(KeyValuePair<EnumNeighbour, Tuple<EnumTileDirection, Vector3Int>> neighbourTile in neighbourTiles)
        {
            EnumNeighbour neighbour = neighbourTile.Key;
            EnumTileDirection neighbourTileDirection = neighbourTile.Value.Item1;
            Vector3Int neighbourPosition = neighbourTile.Value.Item2;

            Debug.Log(neighbour.ToString() + " direction: " + neighbourTileDirection.ToString());
            if(neighbourTileDirection != EnumTileDirection.None)
            {
                if(neighbour == EnumNeighbour.Right)
                {
                    pathMap.SetTile(gridPosition, leftTile);
                    pathMap.SetTile(neighbourPosition, leftTile);
                    break;
                }
                else if(neighbour == EnumNeighbour.Left)
                {
                    pathMap.SetTile(gridPosition, rightTile);
                    pathMap.SetTile(neighbourPosition, rightTile);
                    break;
                }
                else if (neighbour == EnumNeighbour.Top)
                {
                    pathMap.SetTile(gridPosition, downTile);
                    pathMap.SetTile(neighbourPosition, downTile);
                    break;
                }
                else if (neighbour == EnumNeighbour.Bottom)
                {
                    pathMap.SetTile(gridPosition, upTile);
                    pathMap.SetTile(neighbourPosition, upTile);
                    break;
                }
            }
        }

    }

}