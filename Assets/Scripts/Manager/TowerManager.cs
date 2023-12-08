using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private Tower towerPrefab;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private int projectilePoolElementCount = 50;
    
    private TowerPositioningHandler _towerPositioningHandler;
    private LevelCreator _levelCreator;
    
    private ObjectPool<Projectile> _projectilePool;
    private ObjectPool<Tower> _towerPool;
    
    /// <summary>
    /// Initializes the TowerManager with the provided LevelCreator and sets up necessary components.
    /// Creates a TowerPositioningHandler using all available tiles from the LevelCreator.
    /// </summary>
    public void Initialize(LevelCreator levelCreator)
    {
        _levelCreator = levelCreator;
        _towerPositioningHandler = new TowerPositioningHandler(levelCreator.GetAllTiles());
        _projectilePool = new ObjectPool<Projectile>(projectilePrefab, projectilePoolElementCount);
        _towerPool = new ObjectPool<Tower>(towerPrefab,10);
        
        if (GameManager.Instance.AutoSaveHandler.IsLoadSelected)
            LoadTowers();
    }

    private void LoadTowers()
    {
        var serializedTowers = SaveManager.LoadGameState().GetTowerList;
        for (int i = 0; i < serializedTowers.Length; i++)
        {
            var tower = _towerPool.Pop();
            tower.gameObject.SetActive(true);
            
            var tile = _levelCreator.GetTileByCoordinate(serializedTowers[i].coordinate.ToVector2Int());
            tower.Initialize(tile, PopProjectile, PushProjectile);
            _towerPositioningHandler.MarkTile(tile);
        }
    }

    // Button method.
    public void PutTowerNearPath()
    {
        Tile tile = _towerPositioningHandler.GetRandomTile();

        if (tile != null)
            Put(tile);
    }

    private void Put(Tile tile)
    {
        var tower = _towerPool.Pop();
        tower.gameObject.SetActive(true);
        tower.Initialize(tile, PopProjectile, PushProjectile);
    }

    private Projectile PopProjectile()
    {
        var projectile = _projectilePool.Pop();
        projectile.gameObject.SetActive(true);
        return projectile;
    }

    private void PushProjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
        _projectilePool.Push(projectile);
    }
}