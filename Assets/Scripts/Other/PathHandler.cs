using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathHandler
{
    private readonly List<Tile> _startTiles;
    private readonly List<Tile> _endTiles;

    private readonly int _maxTryCount;
    private readonly int _maxPathCount;
    private List<List<Tile>> _paths;

    public PathHandler(List<Tile> startTiles, List<Tile> endTiles, int maxPathCount, int maxTryCount)
    {
        _startTiles = startTiles;
        _endTiles = endTiles;
        _maxTryCount = maxTryCount;
        _maxPathCount = maxPathCount;

        CreatePaths();
    }

    public PathHandler(List<List<Tile>> paths)
    {
        _paths = paths;
    }

    public List<List<Tile>> GetPaths => _paths;

    public List<Tile> GetRandomPath() => _paths[Random.Range(0, _paths.Count)];
    
    /// <summary>
    /// Creates multiple distinct paths connecting random start and end tiles.
    /// The number of paths created is limited by maximum try count and maximum path count.
    /// </summary>
    private void CreatePaths()
    {
        int count = 0;

        _paths = new();

        // Continue creating paths until the maximum try count or maximum path count is reached.
        while (count < _maxTryCount && _paths.Count < _maxPathCount)
        {
            var tile = _startTiles[Random.Range(0, _startTiles.Count)];

            List<Tile> path = new();
            List<Tile> closeList = new();
            path.Add(tile);

            while (!_endTiles.Contains(tile))
            {
                var neighbor = GetNeighbor(tile, path, closeList);

                // Handle cases where no valid neighbor is found.
                if (neighbor == null)
                {
                    if(path.Count > 0)
                        tile = path.Last();
                    else
                    {
                        Debug.Log("Count is zero.");
                        tile = _startTiles[Random.Range(0, _startTiles.Count)];
                        path.Add(tile);
                    }
                }
                    
                else
                {
                    path.Add(neighbor);
                    tile = neighbor;
                }
            }

            if (_paths.Count < 1)
                _paths.Add(path);
            else
            {
                bool isEqual = false;

                for (int i = 0; i < _paths.Count; i++)
                {
                    if (_paths[i].SequenceEqual(path))
                    {
                        isEqual = true;
                        break;
                    }
                }

                if (!isEqual)
                {
                    _paths.Add(path);
                    count = 0;
                }
                else
                    count++;
            }
        }
    }

    private Tile GetNeighbor(Tile tile, List<Tile> path, List<Tile> closeList)
    {
        var list = new List<Tile>(tile.PathNeighbors);

        while (list.Count > 0)
        {
            var neighbor = list[Random.Range(0, list.Count)];

            if (!path.Contains(neighbor) && !closeList.Contains(neighbor))
                return neighbor;

            list.Remove(neighbor);
        }

        path.Remove(tile);
        closeList.Add(tile);

        return null;
    }
}