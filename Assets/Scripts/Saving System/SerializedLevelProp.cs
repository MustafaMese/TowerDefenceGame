using System;

[Serializable]
public class SerializedLevelProp
{
    public SerializedVector2Int coordinate;
    public int levelPropType;

    public SerializedLevelProp(LevelProp levelProp)
    {
        coordinate = levelProp.Tile.Coordinate.FromVector2Int();
        levelPropType = (int)levelProp.LevelPropType;
    }

}