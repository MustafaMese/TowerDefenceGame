using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Board : MonoBehaviour
{
    public Vector2 boardSize = default;
    public Tile tilePrefab;
    public List<Tile> tiles = new List<Tile>();
    public TileContentFactory contentFactory;

    private Queue<Tile> searchFrontier = new Queue<Tile>();

    private void Awake()
    {
        tiles = new List<Tile>(FindObjectsOfType<Tile>());

        FindPaths();
    }

    private void InstantiateTiles()
    {
        Vector2Int size = Vector2Int.FloorToInt(boardSize);

        if (size == default) return;

        Vector2 offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);
        for (int i = 0, y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, i++)
            {
                Tile tile = tiles[i];

                if (x > 0)
                    Tile.AddEastWestNeighbors(tile, tiles[i - 1]);
                if (y > 0)
                    Tile.AddNorthSouthNeighbors(tile, tiles[i - size.x]);

                tile.IsAlternative = x % 2 == 0;

                if ((y & 1) == 0)
                {
                    tile.IsAlternative = !tile.IsAlternative;
                }
            }
        }
    }

    public void InitializeTile(Tile tile)
    {
        tile.Content = contentFactory.Get(TileContentType.EMPTY);
    }

    public void AddTile(Tile tile)
    {
        tiles.Add(tile);
    }

    public bool FindPaths()
    {
        foreach (Tile tile in tiles)
        {
            if (tile.Content.Type == TileContentType.DESTINATION)
            {
                tile.BecomeDestination();
                searchFrontier.Enqueue(tile);
            }
            else
            {
                tile.ClearPath();
            }
        }
        if (searchFrontier.Count == 0)
        {
            return false;
        }

        while (searchFrontier.Count > 0)
        {
            Tile tile = searchFrontier.Dequeue();
            if (tile != null)
            {
                if (tile.IsAlternative)
                {
                    searchFrontier.Enqueue(tile.GrowPathNorth());
                    searchFrontier.Enqueue(tile.GrowPathSouth());
                    searchFrontier.Enqueue(tile.GrowPathEast());
                    searchFrontier.Enqueue(tile.GrowPathWest());
                }
                else
                {
                    searchFrontier.Enqueue(tile.GrowPathWest());
                    searchFrontier.Enqueue(tile.GrowPathEast());
                    searchFrontier.Enqueue(tile.GrowPathSouth());
                    searchFrontier.Enqueue(tile.GrowPathNorth());
                }
            }
        }

        foreach (Tile tile in tiles)
        {
            tile.ShowPath();
        }

        return true;
    }

    public void ToggleDestination(Tile tile)
    {
        switch (tile.Content.Type)
        {
            case TileContentType.DESTINATION:
                tile.Content = contentFactory.Get(TileContentType.BEGIN);
                if (!FindPaths())
                {
                    print(11);
                    tile.Content = contentFactory.Get(TileContentType.DESTINATION);
                    FindPaths();
                }
                break;
            case TileContentType.EMPTY:
                tile.Content = contentFactory.Get(TileContentType.ARROW);
                FindPaths();
                break;
            case TileContentType.BEGIN:
                tile.Content = contentFactory.Get(TileContentType.EMPTY);
                FindPaths();
                break;
            case TileContentType.ARROW:
                tile.Content = contentFactory.Get(TileContentType.DESTINATION);
                FindPaths();
                break;
        }
    }

}
