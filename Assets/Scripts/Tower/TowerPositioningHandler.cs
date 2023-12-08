using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPositioningHandler
{
    private readonly List<Tile> _closestTilesToPath;
    
    public TowerPositioningHandler(List<Tile> allTiles)
    {
        _closestTilesToPath = new();
        
        foreach (var tile in allTiles)
        {
            if(tile.IsPath || tile.IsEndTile)
                continue;
            
            if(tile.PathNeighbors.Count > 0)
                _closestTilesToPath.Add(tile);
        }
    }

    public Tile GetRandomTile()
    {
        if (_closestTilesToPath.Count > 0)
        {
            var tile = GetTile(_closestTilesToPath);
            return tile;
        }

        return null;
    }

    private Tile GetTile(List<Tile> list)
    {
        var tile = list[Random.Range(0, list.Count)];
        list.Remove(tile);
        return tile;
    }

    public void MarkTile(Tile tile)
    {
        if (_closestTilesToPath.Contains(tile))
            _closestTilesToPath.Remove(tile);
    }
}
