using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    private Sequence _sequence;
    
    public void Initialize(Vector3 position, Tile current)
    {
        transform.position = position;
        enemy.OnAdding.Invoke(enemy, current);
    }
    
    public void Move(List<Tile> path, bool startFromBeginning)
    {
        if(path.Count < 1)
            return;
        
        var arr = new Vector3[path.Count - 1];
        for (int i = 1; i < path.Count; i++)
            arr[i - 1] = path[i].transform.position;

        if(startFromBeginning)
            enemy.transform.position = path[0].transform.position;

        _sequence = DOTween.Sequence();
        
        _sequence.Append(enemy.transform.DOPath(arr, arr.Length)
            .SetEase(Ease.Linear)
            .OnWaypointChange(value =>
            {
                if(value != 0)
                    enemy.OnTileChanged.Invoke(enemy, path[value - 1], path[value]);
            }))
            .OnComplete(() =>
            {
                GameManager.Instance.CommandManager.InvokeCommand(new DefeatCommand());
                SaveManager.SetIsLastGameDefeat(true);
            });
    }

    public void KillMovement()
    {
        _sequence.Kill();
    }
}