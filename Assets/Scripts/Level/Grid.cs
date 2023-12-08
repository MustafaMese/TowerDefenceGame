using UnityEngine;

/// <summary>
/// Represents a grid cell in a tile-based environment, storing information about its position, neighbors,
/// and A* pathfinding values.
/// </summary>
public class Grid
{
    public readonly Tile Tile;
    public Vector2Int Coordinate;
    
    // -- A* stuff  
    public float F => G + H;
    public float H;
    public float G;
    public Grid Parent { get; set; }
    // --   
    
    public Grid Right { get; private set; }
    public Grid Left{ get; private set; }
    public Grid UpRight{ get; private set; }
    public Grid DownRight{ get; private set; }
    public Grid UpLeft{ get; private set; }
    public Grid DownLeft{ get; private set; }
    public bool IsBlocked { get; set; }

    public Grid(Tile tile, Vector2Int coordinate)
    {
        Tile = tile;
        Coordinate = coordinate;
    }

    public void SetNeighbors(Tile left, Tile right, Tile upLeft, Tile upRight, Tile downLeft, Tile downRight)
    {
        Left = left != null ? left.Grid : null;
        Right = right != null ? right.Grid : null;
        UpLeft = upLeft != null ? upLeft.Grid : null;
        UpRight = upRight != null ? upRight.Grid : null;
        DownLeft = downLeft != null ? downLeft.Grid : null;
        DownRight = downRight != null ? downRight.Grid : null;
    }

    public void Reset()
    {
        G = 0;
        H = 0;
        Parent = null;
    }
}

public enum Direction
{
    Right,
    Left,
    UpRight,
    UpLeft,
    DownRight,
    DownLeft
}