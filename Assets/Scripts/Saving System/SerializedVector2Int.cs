using System;
using UnityEngine;

[Serializable]
public class SerializedVector2Int
{
    public int x;
    public int y;

    public SerializedVector2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public SerializedVector2Int(Vector2Int vector2Int)
    {
        x = vector2Int.x;
        y = vector2Int.y;
    }
}