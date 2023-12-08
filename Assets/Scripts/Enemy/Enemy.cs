using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemyVisual enemyVisual;
    
    public Action<Enemy, Tile> OnAdding;
    public Action<Enemy, Tile, Tile> OnTileChanged;
    public Action<int> OnHealthChanged;
    public Action OnDeath;

    private Action<Enemy> OnEnd;
    
    public EnemyType EnemyType { get; private set; }
    public bool IsDead { get; private set; }
    public Tile Tile { get; set; }
    public List<Tile> CurrentPath { get; private set; }
    
    public int CurrentHealth => enemyHealth.CurrentHealth;
    public int MaxHealth => enemyHealth.MaxHealth;
    
    public void Initialize(EnemyProperties enemyProperties, List<Tile> path, float healthPercentage, Action<Enemy> onEnd)
    {
        Configure(enemyProperties, path, onEnd);

        enemyHealth.Initialize(enemyProperties, healthPercentage);
        enemyMovement.Initialize(path[0].transform.position, path[0]);
    }
    
    public void Initialize(EnemyProperties enemyProperties, Vector3 position, Tile current, List<Tile> path, int maxHealth, int currentHealth, Action<Enemy> onEnd)
    {
        Configure(enemyProperties, path, onEnd);
        
        ConfigurePath(CurrentPath, current);
        enemyHealth.Initialize(maxHealth, currentHealth);
        enemyMovement.Initialize(position, current);
    }
    
    private void Configure(EnemyProperties enemyProperties, List<Tile> path, Action<Enemy> onEnd)
    {
        enemyVisual.Initialize(enemyProperties);
        EnemyType = enemyProperties.enemyType;
        IsDead = false;
        
        OnTileChanged += GameManager.Instance.TileEnemyMatchHandler.ChangeEnemyTile;
        OnAdding += GameManager.Instance.TileEnemyMatchHandler.AddEnemyToTile;
        OnDeath += Death;
        OnEnd = onEnd;
        
        GameManager.Instance.CommandManager.AddCommandListener<DefeatCommand>((command => Stop()));
        GameManager.Instance.CommandManager.AddCommandListener<AutoSaveCommand>(AutoSaveCommand);
        
        CurrentPath = path;
    }
    
    private void Death()
    {
        if(IsDead) return;
        
        GameManager.Instance.CommandManager.RemoveCommandListener<AutoSaveCommand>(AutoSaveCommand);
        
        Stop();
        
        IsDead = true;
        OnTileChanged = null;
        OnAdding = null;
        OnDeath = null;
        gameObject.SetActive(false);
        
        GameManager.Instance.TileEnemyMatchHandler.RemoveEnemy(this, Tile);
        ScoreManager.Instance.IncreaseScore();
        
        OnEnd.Invoke(this);
    }
    
    public void Move(bool startFromBeginning)
    {
        enemyMovement.Move(CurrentPath, startFromBeginning);
        enemyVisual.Stop();
    }

    private void ConfigurePath(List<Tile> path, Tile currentTile)
    {
        var index = path.IndexOf(currentTile);
        path.RemoveRange(0, index);
    }
    
    private void AutoSaveCommand(AutoSaveCommand command)
    {
        GameManager.Instance.AutoSaveHandler.SaveEnemy(this);
    }
    
    private void Stop()
    {
        enemyMovement.KillMovement(); 
    }

    public void TakeDamage(int delta)
    {
        OnHealthChanged.Invoke(delta);
        
        FloatingTextController.Instance.GetText(Mathf.Abs(delta).ConvertToCharArray(), transform.position + Vector3.up / 2);
    }
}
