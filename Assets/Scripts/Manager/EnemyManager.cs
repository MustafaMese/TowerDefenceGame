using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Variables")]
    [SerializeField] private List<EnemyProperties> enemyPropertyList = new();
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private int enemyPoolElementCount = 30;
    
    [Header("Spawn Rate Variables")]
    [SerializeField] private float baseSpawnRate = 1f;
    [SerializeField] private float spawnRateIncrease = 0.1f;
    [SerializeField] private float spawnRateIncreaseTimeInterval = 2f;

    [Header("Enemy Health Variables")] 
    [SerializeField] private float healthIncreaseTimeInterval;
    [SerializeField] private float healthIncrease;

    private readonly Dictionary<EnemyType, EnemyProperties> _enemyTypePropertyPairs = new();
    
    private EnemySpawnRateHandler _enemySpawnRateHandler;
    private EnemyHealthHandler _enemyHealthHandler;
    private LevelCreator _levelCreator;
    
    public float SpawnRate { get; set; }
    public float HealthPercentage { get; set; }
    public bool Terminate { get; private set; }

    private ObjectPool<Enemy> enemyPool;

    /// <summary>
    /// Initializes the EnemyManager with the provided LevelCreator and sets up various parameters and event listeners.
    /// </summary>
    public void Initialize(LevelCreator levelCreator)
    {
        _levelCreator = levelCreator;
        var load = GameManager.Instance.AutoSaveHandler.IsLoadSelected;
        
        SpawnRate = load ? GameManager.Instance.AutoSaveHandler.GetGameState().GetSpawnRate() : baseSpawnRate;
        HealthPercentage = load ? GameManager.Instance.AutoSaveHandler.GetGameState().GetHealthPercentage() : 0f;
        
        // Initialize handlers for dynamic spawn rate and health adjustments.
        _enemySpawnRateHandler = new EnemySpawnRateHandler(this, spawnRateIncreaseTimeInterval, spawnRateIncrease);
        _enemyHealthHandler = new EnemyHealthHandler(this, healthIncreaseTimeInterval, healthIncrease);
       
        GameManager.Instance.CommandManager.AddCommandListener<DefeatCommand>(defeatCommand => Terminate = true);
        GameManager.Instance.CommandManager.AddCommandListener<AutoSaveCommand>(AutoSaveCommand);

        SetEnemyPropertyDictionary();

        enemyPool = new ObjectPool<Enemy>(enemyPrefab, enemyPoolElementCount);
        
        if (load)
            LoadEnemies();
        
        StartCoroutine(StartSpawning());
    }

    private void SetEnemyPropertyDictionary()
    {
        for (int i = 0; i < enemyPropertyList.Count; i++)
        {
            EnemyProperties enemyProperty = enemyPropertyList[i];
            _enemyTypePropertyPairs[enemyProperty.enemyType] = enemyProperty;
        }
    }

    private void LoadEnemies()
    {
        List<Enemy> enemies = InstantiateSavedEnemies(GameManager.Instance.AutoSaveHandler.GetGameState().GetEnemyList());
        
        for (int i = 0; i < enemies.Count; i++)
            enemies[i].Move(false);
    }

    private List<Enemy> InstantiateSavedEnemies(SerializedEnemy[] arr)
    {
        List<Enemy> enemyList = new();
        for (int i = 0; i < arr.Length; i++)
        {
            var enemy = enemyPool.Pop();
            enemy.gameObject.SetActive(true);
            
            var serializedEnemy = arr[i];
            var path = ConvertTileList(serializedEnemy.path);
            var currentTile = _levelCreator.GetTileByCoordinate(serializedEnemy.coordinate.ToVector2Int());
            var position = serializedEnemy.position.ToVector3();
            var property = _enemyTypePropertyPairs[(EnemyType)serializedEnemy.enemyType];
            
            enemy.Initialize(property, position, currentTile, path, serializedEnemy.maxHealth, serializedEnemy.health, enemyPool.Push);
            
            enemyList.Add(enemy);
        }

        return enemyList;
    }

    private List<Tile> ConvertTileList(SerializedVector2Int[] path)
    {
        List<Tile> list = new();
        for (int i = 0; i < path.Length; i++)
            list.Add(_levelCreator.GetTileByCoordinate(path[i].ToVector2Int()));
        return list;
    }

    private void AutoSaveCommand(AutoSaveCommand command)
    {
        GameManager.Instance.AutoSaveHandler.SaveSpawnRate(SpawnRate);
        GameManager.Instance.AutoSaveHandler.SaveHealthPercentage(HealthPercentage);
    }

    private IEnumerator StartSpawning()
    {
        StartCoroutine(_enemySpawnRateHandler.ManipulateSpawnRate());
        StartCoroutine(_enemyHealthHandler.ManipulateHealthIncreasePercentage());
        
        while (!Terminate)
        {
            yield return new WaitForSeconds(SpawnRate); 
            
            InstantiateEnemy();
        }
    }

    private void InstantiateEnemy()
    {
        if(Terminate) return;

        var enemy = enemyPool.Pop();
        enemy.gameObject.SetActive(true);
        
        var property = enemyPropertyList[Random.Range(0, enemyPropertyList.Count)];
        enemy.Initialize(property, _levelCreator.GetPathHandler().GetRandomPath(), HealthPercentage, enemyPool.Push);
        enemy.Move(true);
    }
}