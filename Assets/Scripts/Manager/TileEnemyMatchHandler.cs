using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the association between tiles and lists of enemies, main purpose is blocking of physics-related interactions.
/// </summary>
public class TileEnemyMatchHandler
{
    private readonly Dictionary<Tile, List<Enemy>> _tileEnemyListPairs;

    public TileEnemyMatchHandler()
    {
        _tileEnemyListPairs = new();
    }
    
    public void Subscribe(Tile tile)
    {
        if(!_tileEnemyListPairs.ContainsKey(tile))
            _tileEnemyListPairs[tile] = new();
    }

    public void AddEnemyToTile(Enemy enemy, Tile tile)
    {
        _tileEnemyListPairs[tile].Add(enemy);

        enemy.Tile = tile;
    }

    public void ChangeEnemyTile(Enemy enemy, Tile oldTile, Tile newTile)
    {
        _tileEnemyListPairs[oldTile].Remove(enemy);
        _tileEnemyListPairs[newTile].Add(enemy);
        
        enemy.Tile = newTile;
    }

    public void RemoveEnemy(Enemy enemy, Tile tile)
    {
        _tileEnemyListPairs[tile].Remove(enemy);
    }

    public bool IsTileContains(Tile tile, out Enemy enemy)
    {
        enemy = null;
        var list = _tileEnemyListPairs[tile];

        if (list.Count <= 0) return false;
        
        enemy = list[Random.Range(0, list.Count)];
        return true;

    }
}
