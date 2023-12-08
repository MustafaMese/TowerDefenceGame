using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private TileVisual tileVisual;

    public Grid Grid;
    
    private bool _isStartTile = false;
    private bool _isEndTile = false;
    
    private List<Tile> _pathNeighbors;
    private List<Tile> _allNeighbors;

    public bool IsPath { get; set; }

    private Tile Right => Grid.Right?.Tile;
    private Tile Left => Grid.Left?.Tile;
    private Tile UpRight => Grid.UpRight?.Tile;
    private Tile DownRight => Grid.DownRight?.Tile;
    private Tile UpLeft => Grid.UpLeft?.Tile;
    private Tile DownLeft => Grid.DownLeft?.Tile;

    public bool IsBlocked
    {
        get => Grid.IsBlocked;
        set => Grid.IsBlocked = value;
    }

    public bool IsBorderTile { get; set; }
    
    public List<Tile> PathNeighbors => _pathNeighbors;

    public List<Tile> AllNeighbors => _allNeighbors;
    
    public Vector2Int Coordinate => Grid.Coordinate;
    public bool IsEndTile => _isEndTile;

    public void Initialize(Transform parent, Vector3 localPosition, Vector2Int coordinate, bool isInLastRow, bool isBorderTile)
    {
        transform.SetParent(parent);
        transform.localPosition = localPosition;
        
        if(isInLastRow)
            _isEndTile = true;

        IsBorderTile = isBorderTile;
        
        tileVisual.Initialize(TileType.Normal, -coordinate.y, isInLastRow);

        var grid = new Grid(this, coordinate);
        Grid = grid;
    }

    public Vector2Int GetDesiredNeighborCoordinate(Direction direction)
    {
        Vector2Int coordinate;
        
        switch (direction)
        {
            case Direction.Right:
                coordinate = Grid.Coordinate + new Vector2Int(1, 0);
                break;
            case Direction.Left:
                coordinate = Grid.Coordinate + new Vector2Int(-1, 0);
                break;
            case Direction.UpRight:
                coordinate = Grid.Coordinate.y % 2 == 0
                    ? Grid.Coordinate + new Vector2Int(0, 1)
                    : Grid.Coordinate + new Vector2Int(1, 1);
                break;
            case Direction.UpLeft:
                coordinate = Grid.Coordinate.y % 2 == 0
                    ? Grid.Coordinate + new Vector2Int(-1, 1)
                    : Grid.Coordinate + new Vector2Int(0, 1);
                break;
            case Direction.DownRight:
                coordinate = Grid.Coordinate.y % 2 == 0
                    ? Grid.Coordinate + new Vector2Int(0, -1)
                    : Grid.Coordinate + new Vector2Int(1, -1);
                break;
            case Direction.DownLeft:
                coordinate = Grid.Coordinate.y % 2 == 0
                    ? Grid.Coordinate + new Vector2Int(-1, -1)
                    : Grid.Coordinate + new Vector2Int(0, -1);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }

        return coordinate;
    }

    public void SetNeighbors(Tile left, Tile right, Tile upLeft, Tile upRight, Tile downLeft, Tile downRight)
    {
        Grid.SetNeighbors(left, right, upLeft, upRight, downLeft, downRight);

        _allNeighbors = new();
        
        if (Right != null)
            _allNeighbors.Add(Right);
        
        if (Left != null)
            _allNeighbors.Add(Left);
        
        if (DownLeft != null)
            _allNeighbors.Add(DownLeft);
        
        if (DownRight != null)
            _allNeighbors.Add(DownRight);
        
        if (UpLeft != null)
            _allNeighbors.Add(UpLeft);
        
        if (UpRight != null)
            _allNeighbors.Add(UpRight);
    }

    public void SetAsPath()
    {
        tileVisual.SetAsPath();
        IsPath = true;
    }

    public void SetAsPropTile()
    {
        tileVisual.SetAsProp();
    }

    public void Reset()
    {
        Grid.Reset();
    }

    public void SetAsStartTile()
    {
        _isStartTile = true;
    }

    public void SetPathList()
    {
        _pathNeighbors = new();

        if (Right != null && Right.IsPath)
            _pathNeighbors.Add(Right);
        
        if (Left != null && Left.IsPath)
            _pathNeighbors.Add(Left);
        
        if (UpLeft != null && UpLeft.IsPath)
            _pathNeighbors.Add(UpLeft);
        
        if (UpRight != null && UpRight.IsPath)
            _pathNeighbors.Add(UpRight);
        
        if (DownLeft != null && DownLeft.IsPath)
            _pathNeighbors.Add(DownLeft);
        
        if (DownRight != null && DownRight.IsPath)
            _pathNeighbors.Add(DownRight);
    }
}