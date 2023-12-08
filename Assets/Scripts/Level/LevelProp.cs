using UnityEngine;

public class LevelProp : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    public LevelPropType LevelPropType { get; private set; }
    public Tile Tile { get; private set; }

    public void Initialize(LevelPropProperties levelPropProperties, Tile tile)
    {
        LevelPropType = levelPropProperties.levelPropType;

        Tile = tile;
        spriteRenderer.sprite = levelPropProperties.sprite;
        spriteRenderer.sortingOrder = -tile.Coordinate.y;
        
        transform.SetParent(tile.transform);
        transform.localPosition = Vector3.zero;
        
        GameManager.Instance.CommandManager.AddCommandListener<AutoSaveCommand>(AutoSaveCommand);
    }

    private void AutoSaveCommand(AutoSaveCommand e)
    {
        GameManager.Instance.AutoSaveHandler.SaveLevelProp(this);
    }
}
