using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy")]
public class EnemyProperties : ScriptableObject
{
    public EnemyType enemyType;
    public int maxHealth;
    public Sprite[] animationSprites;
}

public enum EnemyType
{
    Pink,
    Yellow,
    Blue,
    Green
}
