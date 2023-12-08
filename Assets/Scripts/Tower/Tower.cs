using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private TowerVisual towerVisual;
    [SerializeField] private TowerProperties towerProperties;
    [SerializeField] private Transform muzzlePoint;
    
    private List<Tile> _range;
    public Tile Tile { get; private set; }

    private Func<Projectile> PopProjectile;
    private Action<Projectile> PushProjectile;

    private Enemy Target { get; set; }

    private bool Terminate { get; set; }

    public void Initialize(Tile tile, Func<Projectile> popProjectile, Action<Projectile> pushProjectile)
    {
        Tile = tile;
        towerVisual.Initialize(-tile.Coordinate.y);
        
        PopProjectile = popProjectile;
        PushProjectile = pushProjectile;
        
        GameManager.Instance.CommandManager.AddCommandListener<DefeatCommand>(command => Stop());
        GameManager.Instance.CommandManager.AddCommandListener<AutoSaveCommand>(AutoSaveCommand);
        
        ConfigureRange(towerProperties.towerRangeType);
        SetPosition();
        StartCoroutine(Attack());
    }

    private void AutoSaveCommand(AutoSaveCommand e)
    {
        GameManager.Instance.AutoSaveHandler.SaveTower(this);
    }

    private void Stop()
    {
        Terminate = true;
    }

    private IEnumerator Attack()
    {
        while (!Terminate)
        {
            if (Target == null || Target.IsDead || !IsInRange(Target))
                Target = FindTarget();

            if (Target != null)
                SendProjectile();
            
            yield return new WaitForSeconds(towerProperties.attackRate);
        }
    }

    private void SendProjectile()
    {
        var projectile = PopProjectile.Invoke();
        StartCoroutine(projectile.Move(towerProperties.Damage, muzzlePoint.position, Target, PushProjectile));
    }

    private Enemy FindTarget()
    {
        for (int i = 0; i < _range.Count; i++)
        {
            var tile = _range[i];
            if (GameManager.Instance.TileEnemyMatchHandler.IsTileContains(tile, out Enemy enemy))
                return enemy;
        }

        return null;
    }

    private bool IsInRange(Enemy target)
    {
        for (int i = 0; i < _range.Count; i++)
        {
            var tile = _range[i];
            if (tile == target.Tile)
                return true;
        }

        return false;
    }

    private void SetPosition()
    {
        transform.SetParent(Tile.transform);
        transform.localPosition = Vector3.zero;
    }

    private void ConfigureRange(TowerRangeType rangeType)
    {
        _range = new(Tile.AllNeighbors);
        List<Tile> seconderNeighbors = new();
        
        for (int i = 0; i < _range.Count; i++)
        {
            var neighbor = _range[i];
            if (rangeType == TowerRangeType.LongRange)
            {
                var allNeighbors = neighbor.AllNeighbors;
                for (int j = 0; j < allNeighbors.Count; j++)
                {
                    var seconderNeighbor = allNeighbors[j];
                    if(seconderNeighbor != Tile && !_range.Contains(seconderNeighbor) && !seconderNeighbors.Contains(seconderNeighbor))
                        seconderNeighbors.Add(seconderNeighbor);
                }
            }
        }

        for (int i = 0; i < seconderNeighbors.Count; i++)
            _range.Add(seconderNeighbors[i]);

        _range.RemoveAll(tile => !tile.IsPath);
    }
}
