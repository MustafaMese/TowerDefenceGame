using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite pathSprite;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite propSprite;
    
    private TileType _tileType;
    
    private TileType TileType
    {
        get => _tileType;
        set
        {
            _tileType = value;

            ChangeSprite(_tileType);
        }
    }
    
    public void Initialize(TileType type, int sortingOrder, bool isInLastRow)
    {
        TileType = type;
        spriteRenderer.sortingOrder = sortingOrder;
        spriteRenderer.enabled = !isInLastRow;
    }

    private void ChangeSprite(TileType tileType)
    {
        switch (tileType)
        {
            case TileType.Path:
                spriteRenderer.sprite = pathSprite;
                break;
            case TileType.Normal:
                spriteRenderer.sprite = normalSprite;
                break;
            case TileType.Prop:
                spriteRenderer.sprite = propSprite;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null);
        }
    }

    public void SetAsPath()
    {
        TileType = TileType.Path;
    }

    public void SetAsProp()
    {
        TileType = TileType.Prop;
    }
}

public enum TileType
{
    Path,
    Normal,
    Prop
}