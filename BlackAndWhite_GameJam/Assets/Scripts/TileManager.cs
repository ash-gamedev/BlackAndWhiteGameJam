using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    private Grid grid;

    [Header("Tilemaps")]
    [SerializeField] private Tilemap interactiveConveyorMap = null;
    [SerializeField] private Tilemap tileConveyorMap = null;

    [Header("Tiles")]
    [SerializeField] private Tile defaultTile = null;
    [SerializeField] private Tile upTile = null;
    [SerializeField] private Tile downTile = null;
    [SerializeField] private Tile leftTile = null;
    [SerializeField] private Tile rightTile = null;

    [Header("Other")]
    [SerializeField] private List<TileConveyer> tileConveyers;
    [SerializeField] private List<TileOrder> tileOrders;
        
    // Stores the tile objects and their scriptable TileConveyer objects
    private Dictionary<TileBase, TileConveyer> tileConveyerFromTileBase;
    private Dictionary<TileBase, TileOrder> tileOrdersFromTileBase;

    // Stores the starting conveyer tile positions and tiles & boolean 
    private Dictionary<Vector3Int, TileBase> baseGridPositionsAndTiles;

    // Stores the starting conveyer tile positions and if position is locked or not
    private Dictionary<Vector3Int, bool> baseGridPositionAndLockStatus;

    // Converts EnumTileDirection to Vector
    private Dictionary<EnumTileDirection, Vector3> vectorByDirection;
    
    // Grid variables
    private Vector3Int previousMousePos = new Vector3Int();
    bool isDrawingPath = false;
    ConveyerTilePath currentPath;
    List<ConveyerTilePath> paths = null;

    //Orders
    OrderManager orderManager;

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

        tileOrdersFromTileBase = new Dictionary<TileBase, TileOrder>();
        foreach (var tileOrder in tileOrders)
        {
            foreach (var tile in tileOrder.tiles)
            {
                tileOrdersFromTileBase.Add(tile, tileOrder);
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

        baseGridPositionsAndTiles = new Dictionary<Vector3Int, TileBase>();
        baseGridPositionAndLockStatus = new Dictionary<Vector3Int, bool>();
        foreach (Vector3Int cellPos in tileConveyorMap.cellBounds.allPositionsWithin)
        {
            TileBase tile = tileConveyorMap.GetTile(cellPos);
            if (tile != null && tileConveyerFromTileBase.ContainsKey(tile))
            {
                baseGridPositionsAndTiles[cellPos] = tile;
                baseGridPositionAndLockStatus[cellPos] = false; // no locked
            }
        }
    }

    void Start()
    {
        grid = gameObject.GetComponent<Grid>();
        interactiveConveyorMap.CompressBounds();

        paths = new List<ConveyerTilePath>();

        orderManager = FindObjectOfType<OrderManager>();
    }

    void Update()
    {
        // make sure level isn't finished and game isn't paused to place tiles
        if (!LevelManager.LevelComplete && !PauseMenu.GameIsPaused)
        {
            Vector3Int mousePos = GetMousePosition();

            // Mouse over -> highlight tile (interactive tile conveyor)
            if (!mousePos.Equals(previousMousePos) && interactiveConveyorMap.cellBounds.Contains(mousePos) && !IsBaseConveryTile(mousePos))
            {
                SetHoverTile(mousePos);
            }
            else if (!interactiveConveyorMap.cellBounds.Contains(mousePos))
            {
                interactiveConveyorMap.SetTile(previousMousePos, null); // Remove old hoverTile
            }

            // Left mouse click -> add path tile (if within bounds and on a valid path) && pathMap.GetTile<Tile>(mousePos) == defaultTile
            if (Input.GetMouseButton(0) && interactiveConveyorMap.cellBounds.Contains(mousePos) && !IsBaseConveryTile(mousePos))
            {
                // if starting to draw a path, save the starting path details and set bool
                if (isDrawingPath == false && currentPath == null)
                {
                    isDrawingPath = true;

                    // get closest neighbour to mouse position
                    Vector3Int? closestNeighbourPos = GetStartingNeighbour(mousePos);

                    // set current path
                    currentPath = new ConveyerTilePath(closestNeighbourPos);

                    // add start to path if exists
                    if (closestNeighbourPos != null)
                    {
                        Vector3Int startTilePosition = (Vector3Int)closestNeighbourPos;
                        Tile startOriginalTile = tileConveyorMap.GetTile<Tile>(startTilePosition);
                        ConveyerTile startTile = new ConveyerTile(startTilePosition, startOriginalTile);
                        currentPath.AddTileToPath(startTile);
                    }
                }
                else if (isDrawingPath && currentPath != null)
                {
                    SetConveyorTiles(mousePos);
                }
            }
            else
            {
                if (isDrawingPath)
                {
                    paths.Add(currentPath);
                    currentPath = null;
                    isDrawingPath = false;
                }
            }

            // Right mouse click -> remove path tile (if within bounds)
            if (Input.GetMouseButton(1) && interactiveConveyorMap.cellBounds.Contains(mousePos))
            {
                RemoveConveyorTile(mousePos);
            }
        }
    }

    #region Tile Grid Controller
    #region Setting/Removing Tiles 
    void SetConveyorTiles(Vector3Int mousePos)
    {
        Vector3Int? previousTilePosition = null;

        ConveyerTile lastConveyerTile = currentPath.GetLastTile();
        if (lastConveyerTile != null)
        {
            previousTilePosition = lastConveyerTile.Position;
        }

        if (mousePos != previousTilePosition)
        {            
            // get new conveyor tile
            Tile setTile = GetConveyorTile(mousePos, previousTilePosition);

            if(setTile != null)
            {
                // first remove any tiles currently at the position
                RemoveConveyorTile(mousePos);

                // update last placed tile (if not locked)
                if (lastConveyerTile != null)
                    SetConveyorTile(lastConveyerTile.Position, setTile);

                // add new tile to path
                ConveyerTile newConveyerTile = new ConveyerTile(mousePos, setTile);
                currentPath.AddTileToPath(newConveyerTile);

                // set tile for new path
                SetConveyorTile(newConveyerTile.Position, newConveyerTile.Tile);

                // Play sound
                AudioPlayer.PlaySoundEffect(EnumSoundEffects.TileSet);
            }
        }
    }

    void SetConveyorTile(Vector3Int gridPos, Tile conveyorTile)
    {
        // Set converyor tile
        bool isTileOnConveyerLocked = IsTileOnConveyerLocked(gridPos);
        if(!isTileOnConveyerLocked)
            tileConveyorMap.SetTile(gridPos, conveyorTile);
    }

    void SetHoverTile(Vector3Int mousePos)
    {
        // Get hover tile
        Vector3Int ? neighbourPos = GetStartingNeighbour(mousePos);
        Tile hoverTile = GetConveyorTile(mousePos, neighbourPos);

        // Set tile
        interactiveConveyorMap.SetTile(previousMousePos, null); // Remove old hoverTile
        interactiveConveyorMap.SetTile(mousePos, hoverTile);
        previousMousePos = mousePos;
    }

    void SetDefaultTile(Vector3Int gridPos)
    {
        SetConveyorTile(gridPos, defaultTile);
    }

    Tile GetConveyorTile(Vector3Int mousePos, Vector3Int? neighbourPos)
    {
        Tile setTile = null;

        // set tile based on direction
        if (neighbourPos == null) setTile = upTile;
        else if (mousePos - Vector3Int.up == neighbourPos) setTile = upTile;
        else if (mousePos - Vector3Int.down == neighbourPos) setTile = downTile;
        else if (mousePos - Vector3Int.left == neighbourPos) setTile = leftTile;
        else if (mousePos - Vector3Int.right == neighbourPos) setTile = rightTile;

        return setTile;
    }

    private void RemoveConveyorTile(Vector3Int mousePosition)
    {
        if (!IsBaseConveryTile(mousePosition))
        {
            if (paths != null && paths?.Count > 0)
            {
                // loop through each path created & search for mousePosition to remove tile
                foreach (var path in paths)
                {
                    ConveyerTile removeTile = path.GetTileAtPosition(mousePosition);

                    // if position exists
                    if (removeTile != null)
                    {
                        // sound effect
                        AudioPlayer.PlaySoundEffect(EnumSoundEffects.TileRemove);

                        // if tile is second in the path, & start position hasn't been reset
                        if (path.ConveyerTiles.IndexOf(removeTile) == 1 && path.DoesPathHaveStartingTile())
                        {
                            // reset start tile to default state & remove from path
                            Vector3Int? startTilePosition = path.StartingGridPosition;
                            if (startTilePosition != null)
                                SetTileOnConveyerToDefault((Vector3Int)startTilePosition);

                            path.RemoveStartingTile();
                        }
                        path.RemoveTileFromPath(removeTile);
                    }
                }
                // remove empty paths
                paths.RemoveAll(x => x.ConveyerTiles?.Count == 0);
            }

            SetDefaultTile(mousePosition);
        }
    }

    #endregion

    #region private helpers
    public Vector3 GetWorldPos(Vector3Int gridPos)
    {
        return grid.CellToWorld(gridPos);
    }

    Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }

    Vector3Int? GetStartingNeighbour(Vector3Int gridPosition)
    {
        Dictionary<EnumNeighbour, Tuple<EnumTileDirection, Vector3Int>> neighbourTiles = GetAllNeighborTiles(gridPosition);

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
    #endregion
    #endregion

    #region Tile Manager
    public Vector3 GetTileDirection(Vector2 worldPosition)
    {
        Vector3 direction = Vector3.zero;

        Vector3Int gridPosition = tileConveyorMap.WorldToCell(worldPosition);

        TileBase tile = tileConveyorMap.GetTile(gridPosition);

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
        return tileConveyorMap.WorldToCell(worldPosition);
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

        TileBase neighbourTile = tileConveyorMap.GetTile(neighborPos);

        if (neighbourTile != null && tileConveyerFromTileBase.ContainsKey(neighbourTile))
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
        if (baseGridPositionsAndTiles.ContainsKey(gridPosition))
        {
            // get scriptable tileConveyer object
            TileConveyer tileConveyer = tileConveyers.FirstOrDefault(x => x.TileDirection == tileDirection);

            // get tile base
            TileBase tile = tileConveyerFromTileBase.FirstOrDefault(x => x.Value == tileConveyer).Key;

            // set tile
            tileConveyorMap.SetTile(gridPosition, tile);

            // lock tile so no conveyers can change the direction
            baseGridPositionAndLockStatus[gridPosition] = true;
        }
    }

    public void SetTileOnConveyerToDefault(Vector3Int gridPosition)
    {
        // if key exists and tile is not locked
        if (baseGridPositionsAndTiles.ContainsKey(gridPosition) && !IsTileOnConveyerLocked(gridPosition))
        {
            TileBase tile = baseGridPositionsAndTiles[gridPosition];
            tileConveyorMap.SetTile(gridPosition, tile);
        }
    }

    public void ResetTileOnConveyer(Vector3Int gridPosition)
    {
        if (baseGridPositionsAndTiles.ContainsKey(gridPosition))
        {
            // unlock tile so conveyers can change the direction
            baseGridPositionAndLockStatus[gridPosition] = false;

            SetTileOnConveyerToDefault(gridPosition);
        }
    }

    public bool IsTileOnConveyerLocked(Vector3Int gridPosition)
    {
        if (baseGridPositionAndLockStatus.ContainsKey(gridPosition))
        {
            return baseGridPositionAndLockStatus[gridPosition];
        }

        return false;
    }

    public bool IsOnConveyerTile(Vector2 worldPosition)
    {
        Vector3Int gridPosition = tileConveyorMap.WorldToCell(worldPosition);

        TileBase tile = tileConveyorMap.GetTile(gridPosition);

        if (tile != null && tileConveyerFromTileBase.ContainsKey(tile))
        {
            return true;
        }

        return false;
    }

    public bool IsBaseConveryTile(Vector3Int gridPosition)
    {
        if (baseGridPositionsAndTiles.ContainsKey(gridPosition)) return true;
        else return false;
    }
    #endregion

}
