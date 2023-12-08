using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameState
{
    private SerializedVector2Int[] _pathTileCoordinates;
    private SerializedVector2Int[] _startTileCoordinates;
    private SerializedVector2Int[] _endTileCoordinates;
    private SerializedVector2Int[][] _paths;
    private SerializedEnemy[] _enemyList;
    private SerializedTower[] _towerList;
    private SerializedLevelProp[] _propList;
    
    private float _spawnRate;
    private float _healthPercentage;
    private int _score;

    public List<Vector2Int> GetPathTileCoordinates()
    {
        return ConvertToCoordinates(_pathTileCoordinates);
    }

    public void SetPathTileCoordinates(List<Tile> pathTiles)
    {
        _pathTileCoordinates = ConvertToSerializedVector2Ints(pathTiles);
    }

    public List<Vector2Int> GetStartTile()
    {
        return ConvertToCoordinates(_startTileCoordinates);
    }

    public void SetStartTiles(List<Tile> startTiles)
    {
        _startTileCoordinates = ConvertToSerializedVector2Ints(startTiles);
    }
    
    public List<Vector2Int> GetEndTiles()
    {
        return ConvertToCoordinates(_endTileCoordinates);
    }

    public void SetEndTiles(List<Tile> endTiles)
    {
        _endTileCoordinates = ConvertToSerializedVector2Ints(endTiles);
    }
    
    public void SetPaths(List<List<Tile>> pathLists)
    {
        _paths = new SerializedVector2Int[pathLists.Count][];
        for (int i = 0; i < pathLists.Count; i++)
        {
            var path = pathLists[i];
            _paths[i] = new SerializedVector2Int[path.Count];
            for (int j = 0; j < path.Count; j++)
                _paths[i][j] = path[j].Coordinate.FromVector2Int();
        }
    }

    public List<List<Vector2Int>> GetPaths()
    {
        List<List<Vector2Int>> lists = new();

        for (int i = 0; i < _paths.Length; i++)
        {
            var path = _paths[i];
            List<Vector2Int> list = new();
            lists.Add(list);
            for (int j = 0; j < path.Length; j++)
                list.Add(path[j].ToVector2Int());
        }

        return lists;
    }
    
    public void SetSpawnRate(float spawnRate)
    {
        _spawnRate = spawnRate;
    }
    
    public float GetSpawnRate()
    {
        return _spawnRate;
    }

    public void SetHealthPercentage(float healthPercentage)
    {
        _healthPercentage = healthPercentage;
    }
    
    public float GetHealthPercentage()
    {
        return _healthPercentage;
    }
    
    public void SetEnemyList(List<Enemy> enemyList)
    {
        _enemyList = new SerializedEnemy[enemyList.Count];
        for (int i = 0; i < enemyList.Count; i++)
            _enemyList[i] = new SerializedEnemy(enemyList[i]);
    }

    public SerializedEnemy[] GetEnemyList() => _enemyList;

    public void SetTowerList(List<Tower> towerList)
    {
        _towerList = new SerializedTower[towerList.Count];
        for (int i = 0; i < towerList.Count; i++)
            _towerList[i] = new SerializedTower(towerList[i]);
    }

    public SerializedTower[] GetTowerList => _towerList;
    
    public void SetPropList(List<LevelProp> propList)
    {
        _propList = new SerializedLevelProp[propList.Count];
        for (int i = 0; i < propList.Count; i++)
            _propList[i] = new SerializedLevelProp(propList[i]);
    }

    public SerializedLevelProp[] GetPropList => _propList;
    
    private SerializedVector2Int[] ConvertToSerializedVector2Ints(List<Tile> tiles)
    {
        SerializedVector2Int[] serializedCoordinates = new SerializedVector2Int[tiles.Count];
        for (int i = 0; i < tiles.Count; i++)
            serializedCoordinates[i] = tiles[i].Coordinate.FromVector2Int();

        return serializedCoordinates;
    }
    
    private List<Vector2Int> ConvertToCoordinates(SerializedVector2Int[] array)
    {
        List<Vector2Int> coordinates = new();
        
        for (int i = 0; i < array.Length; i++)
            coordinates.Add(array[i].ToVector2Int());

        return coordinates;
    }

    public void SetScore(int score)
    {
        _score = score;
    }

    public int GetScore()
    {
        return _score;
    }
}