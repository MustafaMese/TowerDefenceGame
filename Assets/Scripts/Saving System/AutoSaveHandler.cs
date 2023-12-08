using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSaveHandler
{
    private readonly WaitForSeconds _waitForSeconds;

    private GameState _gameState;

    private bool Terminate { get; set; }
    public bool IsLoadSelected { get; set; }

    private readonly List<Enemy> _tempSavedEnemyList = new();
    private readonly List<Tower> _tempSavedTowerList = new();
    private readonly List<LevelProp> _tempSavedPropList = new();

    public AutoSaveHandler(float savingInterval)
    {
        _gameState = SaveManager.LoadGameState();
        
        _waitForSeconds = new WaitForSeconds(savingInterval);
        
        GameManager.Instance.CommandManager.AddCommandListener<DefeatCommand>
        (command => 
        {
            Terminate = true;
            Save();
        });
    }
    
    public IEnumerator StartRecording()
    {
        yield return new WaitForSeconds(5f);
        
        while (!Terminate)
        {
            Save();
            
            yield return _waitForSeconds;
        }
    }
    
    private void Save()
    {
        Debug.Log("Start Saving.");
        
        _tempSavedEnemyList.Clear();
        _tempSavedTowerList.Clear();
        _tempSavedPropList.Clear();
        
        GameManager.Instance.CommandManager.InvokeCommand(new AutoSaveCommand());

        _gameState.SetEnemyList(_tempSavedEnemyList);
        _gameState.SetTowerList(_tempSavedTowerList);
        _gameState.SetPropList(_tempSavedPropList);
        
        SaveManager.SaveGameState(_gameState);
        
        Debug.Log("Saving Finished.");
    }
    
    public GameState GetGameState()
    {
        return _gameState;
    }
    
    public void SaveTiles(List<Tile> pathTiles, List<Tile> startTiles, List<Tile> endTiles)
    {
        _gameState.SetPathTileCoordinates(pathTiles);
        _gameState.SetStartTiles(startTiles);
        _gameState.SetEndTiles(endTiles);
    }

    public void SavePaths(List<List<Tile>> paths)
    {
        _gameState.SetPaths(paths);
    }

    public void SaveSpawnRate(float spawnRate)
    {
        _gameState.SetSpawnRate(spawnRate);
    }

    public void SaveHealthPercentage(float healthPercentage)
    {
        _gameState.SetHealthPercentage(healthPercentage);
    }

    public void SaveEnemy(Enemy enemy)
    {
        _tempSavedEnemyList.Add(enemy);
    }

    public void SaveTower(Tower tower)
    {
        _tempSavedTowerList.Add(tower);
    }

    public void SaveLevelProp(LevelProp levelProp)
    {
        _tempSavedPropList.Add(levelProp);
    }
}