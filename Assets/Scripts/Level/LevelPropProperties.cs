using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Level Prop")]
public class LevelPropProperties : ScriptableObject
{
    public LevelPropType levelPropType;
    public Sprite sprite;
}

public enum LevelPropType
{
    Autumn,
    Blue,
    Green
}
