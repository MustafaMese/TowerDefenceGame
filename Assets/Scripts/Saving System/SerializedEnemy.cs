using System;
using System.Collections.Generic;

[Serializable]
public class SerializedEnemy
{
    public SerializedVector3 position;
    public SerializedVector2Int coordinate;
    public SerializedVector2Int[] path;
    public int health;
    public int maxHealth;
    public int enemyType;
    
    public SerializedEnemy(Enemy enemy)
    {
        position = enemy.transform.position.FromVector3();
        coordinate = enemy.Tile.Coordinate.FromVector2Int();
        SetPathArray(enemy.CurrentPath);
        health = enemy.CurrentHealth;
        maxHealth = enemy.MaxHealth;
        enemyType = (int)enemy.EnemyType;
    }

    private void SetPathArray(List<Tile> path)
    {
        this.path = new SerializedVector2Int[path.Count];
        for (int i = 0; i < path.Count; i++)
            this.path[i] = path[i].Coordinate.FromVector2Int();
    }
}