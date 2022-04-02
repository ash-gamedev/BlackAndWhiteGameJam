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
    [SerializeField] private RuleTile pathTile = null;

    [SerializeField] private Tile defaultTile = null;
    [SerializeField] private Tile upTile = null;
    [SerializeField] private Tile downTile = null;
    [SerializeField] private Tile leftTile = null;
    [SerializeField] private Tile rightTile = null;

    private Vector3Int previousMousePos = new Vector3Int();

    // Start is called before the first frame update
    void Start()
    {
        grid = gameObject.GetComponent<Grid>();
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
            pathMap.SetTile(mousePos, upTile);
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

    void PlaceTile()
    {
        //Vector3 
    }
}