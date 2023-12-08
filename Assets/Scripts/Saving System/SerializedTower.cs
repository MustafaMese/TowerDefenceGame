using System;

[Serializable]
public class SerializedTower
{
    public SerializedVector2Int coordinate;

    public SerializedTower(Tower tower)
    {
        coordinate = tower.Tile.Coordinate.FromVector2Int();
    }

}