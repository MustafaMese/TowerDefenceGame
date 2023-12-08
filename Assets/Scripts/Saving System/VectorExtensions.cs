using UnityEngine;

public static class VectorExtensions
{
    public static Vector2Int ToVector2Int(this SerializedVector2Int serializedVector2Int)
    {
        return new Vector2Int(serializedVector2Int.x, serializedVector2Int.y);
    }

    public static SerializedVector2Int FromVector2Int(this Vector2Int vector2Int)
    {
        return new SerializedVector2Int(vector2Int);
    }
    
    public static Vector3 ToVector3(this SerializedVector3 serializedVector3)
    {
        return new Vector3(serializedVector3.x, serializedVector3.y, serializedVector3.z);
    }

    public static SerializedVector3 FromVector3(this Vector3 vector3)
    {
        return new SerializedVector3(vector3);
    }
}