using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TileContentFactory : ScriptableObject
{
    [SerializeField] TileContent destinationPrefab;
    [SerializeField] TileContent arrowPrefab;
    [SerializeField] TileContent emptyPrefab;
    [SerializeField] TileContent startPrefab;

    private TileContent Create(TileContent prefab)
    {
        TileContent instance = Instantiate(prefab);
        return instance;
    }

    public TileContent Get(TileContentType type)
    {
        switch (type)
        {
            case TileContentType.DESTINATION: return Create(destinationPrefab);
            case TileContentType.EMPTY: return Create(emptyPrefab);
            case TileContentType.ARROW: return Create(arrowPrefab);
            case TileContentType.BEGIN: return Create(startPrefab);
        }
        return null;

    }
}
