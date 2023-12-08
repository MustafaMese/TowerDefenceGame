using System.Collections;
using UnityEngine;

public class EnemyHealthHandler
{
    private readonly EnemyManager _enemyManager;
    private readonly float _healthPercentageIncrease;
    private readonly WaitForSeconds _manipulateHealthIncreasePercentage;

    private float _healthPercentage;
    
    public EnemyHealthHandler(EnemyManager enemyManager, float timeInterval, float healthPercentageIncrease)
    {
        _enemyManager = enemyManager;
        _healthPercentageIncrease = healthPercentageIncrease;

        _healthPercentage = _enemyManager.HealthPercentage;
        _manipulateHealthIncreasePercentage = new WaitForSeconds(timeInterval);
    }

    public IEnumerator ManipulateHealthIncreasePercentage()
    {
        while (!_enemyManager.Terminate)
        {
            
            yield return _manipulateHealthIncreasePercentage;

            _healthPercentage += _healthPercentageIncrease;
            _enemyManager.HealthPercentage = _healthPercentage;
        }
    }
}