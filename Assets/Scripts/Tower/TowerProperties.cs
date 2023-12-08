using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ScriptableObjects/Tower")]
public class TowerProperties : ScriptableObject
{
    public TowerRangeType towerRangeType;
    
    [Header("Unit/Sec"), Range(10, 50)]
    public float damagePower;

    [Header("Seconds"), Range(0.15f, 3f)]
    public float attackRate;
    
    public float Damage => attackRate * damagePower;
}

public enum TowerRangeType
{
    CloseRange = 1,
    LongRange
}
