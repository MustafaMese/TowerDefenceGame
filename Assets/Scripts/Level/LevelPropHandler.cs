using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelPropHandler : MonoBehaviour
{
    [SerializeField] private List<LevelPropProperties> propertiesList = new();
    [SerializeField] private LevelProp levelPropPrefab;

    private Dictionary<LevelPropType, LevelPropProperties> _levelPropTypePairs = new();
    
    public void Initialize()
    {
        for (int i = 0; i < propertiesList.Count; i++)
            _levelPropTypePairs[propertiesList[i].levelPropType] = propertiesList[i];
    }
    
    private void AddProp(LevelPropProperties levelPropProperties, Tile tile)
    {
        CreateProp(levelPropProperties, tile);
    }

    public void AddProp(Tile tile)
    {
        CreateProp(propertiesList[Random.Range(0, propertiesList.Count)], tile);
    }

    private void CreateProp(LevelPropProperties levelPropProperties, Tile tile)
    {
        var prop = Instantiate(levelPropPrefab);

        prop.Initialize(levelPropProperties, tile);

        tile.SetAsPropTile();
    }

    public void LoadProps(SerializedLevelProp[] serializedLevelProps, Func<Vector2Int, Tile> GetTile)
    {
        for (int i = 0; i < serializedLevelProps.Length; i++)
        {
            var serializedLevelProp = serializedLevelProps[i];
            var properties = _levelPropTypePairs[(LevelPropType)serializedLevelProp.levelPropType];
            var tile = GetTile.Invoke(serializedLevelProp.coordinate.ToVector2Int());
            
            AddProp(properties, tile);
        }
    }

    
}
