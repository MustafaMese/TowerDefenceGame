using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    [SerializeField] private LevelPropHandler levelPropHandler;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private int height = 10;
    [SerializeField] private int width = 10;
    [SerializeField] private float innerRadius = 10;
    [SerializeField] private float outerRadius = 10;
    [SerializeField] private int unblockedPointCountAtPath = 2;
    [SerializeField] private float randomMaxCostValue = 1f;
    [SerializeField] private int maxTryCount;
    [SerializeField] private int maxPathCount = 5;
    
    private readonly Dictionary<Vector2Int, Tile> _tileCoordinatePairs = new();
    private List<Tile> _startTiles = new();
    private List<Tile> _endTiles = new();
    private List<Tile> _pathTiles = new();

    private PathHandler _pathHandler;

    private int Height => height + 1;

    /// <summary>
    /// Initializes by setting up level properties, creating tiles, setting tile neighbors,
    /// drawing a new path and creating level props.
    /// </summary>
    public void Initialize()
    {
        levelPropHandler.Initialize();
        
        CreateTiles();
        SetTileNeighbors();

        var loadData = GameManager.Instance.AutoSaveHandler.IsLoadSelected;

        if (!loadData)
        {
            DrawPath();
            CreateLevelProps();
        }
        else
        {
            DrawPathFromLoadedData(GameManager.Instance.AutoSaveHandler.GetGameState());
            LoadLevelProps(GameManager.Instance.AutoSaveHandler.GetGameState());
        }
        
        GameManager.Instance.CommandManager.AddCommandListener<AutoSaveCommand>(AutoSaveCommand);
    }

    private void LoadLevelProps(GameState gameState)
    {
        levelPropHandler.LoadProps(gameState.GetPropList, GetTileByCoordinate);
    }

    private void CreateLevelProps()
    {
        foreach (var tile in _tileCoordinatePairs.Values)
        {
            if (tile.PathNeighbors.Count < 1 && !tile.IsEndTile)
                levelPropHandler.AddProp(tile);
        }
    }

    private void AutoSaveCommand(AutoSaveCommand command)
    {
        GameManager.Instance.AutoSaveHandler.SaveTiles(_pathTiles, _startTiles, _endTiles);
        GameManager.Instance.AutoSaveHandler.SavePaths(_pathHandler.GetPaths);
    }
    
    private void DrawPathFromLoadedData(GameState gameState)
    {
        _pathTiles = ConvertCoordinatesToTiles(gameState.GetPathTileCoordinates());
        _startTiles = ConvertCoordinatesToTiles(gameState.GetStartTile());
        _endTiles = ConvertCoordinatesToTiles(gameState.GetEndTiles());
        
        MarkTilesAsPath(_pathTiles);
        SetTilePathLists();

        _pathHandler = new PathHandler(ConvertCoordinateListsToTileLists(gameState.GetPaths()));
    }
    
    /// <summary>
    /// Draws two paths in a grid-based system using the A* algorithm.
    /// </summary>
    private void DrawPath()
    {
        for (int i = 0; i < width; i++)
        {
            _startTiles.Add(_tileCoordinatePairs[new Vector2Int(i, 0)]);
            _endTiles.Add(_tileCoordinatePairs[new Vector2Int(i, Height - 1)]);
        }
        
        // Select random start and end tiles for two separate paths.
        var startTile1 = GetRandomTile(true, _startTiles);
        var endTile1 = GetRandomTile(true, _endTiles);
        var startTile2 = GetRandomTile(false, _startTiles);
        var endTile2 = GetRandomTile(false, _endTiles);
        
        var aStar = new AStar(randomMaxCostValue);
        
        DrawPath(aStar, startTile1, endTile1, true);
        var pathFound =  DrawPath(aStar, startTile2, endTile2, false);
        
        SetTilePathLists();
        
        _startTiles.Clear();
        _startTiles.Add(startTile1);
        if(pathFound)
            _startTiles.Add(startTile2);
        
        _endTiles.Clear();
        _endTiles.Add(endTile1);
        if(pathFound)
            _endTiles.Add(endTile2);
        
        CreatePathHandler();
    }

    private Tile GetRandomTile(bool removeUnusable, List<Tile> list)
    {
        var tile = list[Random.Range(0, list.Count)];
        tile.SetAsStartTile();
        if(removeUnusable)
            RemoveUnusableTiles(tile, list);

        return tile;
    }

    private void RemoveUnusableTiles(Tile tile, List<Tile> list)
    {
        list.Remove(tile);
        if (tile.Grid.Right != null)
            list.Remove(tile.Grid.Right.Tile);
        if (tile.Grid.Left != null)
            list.Remove(tile.Grid.Left.Tile);
    }

    /// <summary>
    /// Draws a path between the specified start and end tiles using the A* algorithm.
    /// Optionally resets some tiles along the path to be unblocked.
    /// </summary>
    private bool DrawPath(AStar aStar, Tile start, Tile end, bool reset)
    {
        var path = aStar.FindPath(start, end);

        if (path == null)
        {
            print("Can't find way.");
            return false;
        }
        
        MarkTilesAsPath(path);

        if(!reset) return true;
        
        int count = 0;
        while (count < unblockedPointCountAtPath && path.Count > 0)
        {
            var tile = path[Random.Range(0, path.Count)];
            tile.IsBlocked = false;
            
            path.Remove(tile);
            
            // Ensure the tile is not a border tile before incrementing count.
            if(!tile.IsBorderTile)
                count++;
        }

        foreach (var tile in _tileCoordinatePairs.Values)
        {
            tile.Reset();
        }

        return true;
    }

    private void MarkTilesAsPath(List<Tile> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            var tile = path[i];

            tile.SetAsPath();
            tile.IsBlocked = true;

            if (!_pathTiles.Contains(tile))
                _pathTiles.Add(tile);

            GameManager.Instance.TileEnemyMatchHandler.Subscribe(tile);
        }
    }
    
    private void CreateTiles()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                InstantiateTile(x, y);
            }
        }
    }
    
    private void InstantiateTile (int x, int y) {
        
        Vector3 localPosition;
        localPosition.x = (x + y * 0.5f - y / 2) * (innerRadius * 2f);
        localPosition.z = 1f;
        localPosition.y = y * (outerRadius * 1.5f);

        var tile = Instantiate(tilePrefab).GetComponent<Tile>();
        var coordinate = new Vector2Int(x, y);
        tile.Initialize(transform, localPosition, coordinate, y == height, x == 0 || x == width - 1);
        
        _tileCoordinatePairs.Add(coordinate, tile);
    }
    
    private void CreatePathHandler()
    {
        _pathHandler = new PathHandler(_startTiles, _endTiles, maxTryCount, maxPathCount);
    }

    public PathHandler GetPathHandler()
    {
        return _pathHandler;
    }

    public List<Tile> GetAllTiles()
    {
        return _tileCoordinatePairs.Values.ToList();
    }

    public Tile GetTileByCoordinate(Vector2Int vector2Int)
    {
        return _tileCoordinatePairs[vector2Int];
    }
    
    private List<List<Tile>> ConvertCoordinateListsToTileLists(List<List<Vector2Int>> coordinateLists)
    {
        List<List<Tile>> tileLists = new();

        for (int i = 0; i < coordinateLists.Count; i++)
        {
            var coordinateList = coordinateLists[i];
            List<Tile> tileList = new();
            tileLists.Add(tileList);
            for (int j = 0; j < coordinateList.Count; j++)
                tileList.Add(_tileCoordinatePairs[coordinateList[j]]);
        }

        return tileLists;
    }
    
    private List<Tile> ConvertCoordinatesToTiles(List<Vector2Int> coordinates)
    {
        List<Tile> list = new();
        for (int i = 0; i < coordinates.Count; i++)
            list.Add(_tileCoordinatePairs[coordinates[i]]);

        return list;
    }
    
    private void SetTileNeighbors()
    {
        foreach (var tile in _tileCoordinatePairs.Values)
        {
            Tile left = null, right = null, upLeft = null, upRight = null, downRight = null, downLeft = null;

            left = SetTileNeighbor(tile, Direction.Left);
            right = SetTileNeighbor(tile, Direction.Right);
            upLeft = SetTileNeighbor(tile, Direction.UpLeft);
            upRight = SetTileNeighbor(tile, Direction.UpRight);
            downLeft = SetTileNeighbor(tile, Direction.DownLeft);
            downRight = SetTileNeighbor(tile, Direction.DownRight);
            
            tile.SetNeighbors(left, right, upLeft, upRight, downLeft, downRight);
        }
    }

    private Tile SetTileNeighbor(Tile tile, Direction direction)
    {
        Tile neighbor = null;
        var coordinate = tile.GetDesiredNeighborCoordinate(direction);
        if (coordinate.x >= 0 && coordinate.x < width && coordinate.y >= 0 && coordinate.y < Height)
            neighbor = _tileCoordinatePairs[coordinate];
        return neighbor;
    }

    private void SetTilePathLists()
    {
        foreach (var tile in _tileCoordinatePairs.Values)
        {
            tile.SetPathList();
        }
    }
}
