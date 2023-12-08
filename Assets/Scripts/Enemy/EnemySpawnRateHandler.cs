using System.Collections;
using UnityEngine;

public class EnemySpawnRateHandler
{
    private readonly EnemyManager _enemyManager;
    private readonly float _spawnRateIncrease;
    private readonly WaitForSeconds _manipulateSpawnRate;
    
    private float _spawnRate;

    public EnemySpawnRateHandler(EnemyManager enemyManager, float timeInterval, float spawnRateIncrease)
    {
        _enemyManager = enemyManager;
        _spawnRateIncrease = spawnRateIncrease;
        
        _manipulateSpawnRate = new WaitForSeconds(timeInterval);
        _spawnRate = _enemyManager.SpawnRate;
    }

    public IEnumerator ManipulateSpawnRate()
    {
        while (!_enemyManager.Terminate)
        {
            yield return _manipulateSpawnRate;

            _spawnRate = Mathf.Clamp(_spawnRate - _spawnRateIncrease, 0.15f, 999f);
            _enemyManager.SpawnRate = _spawnRate;
        }
    }
    
}