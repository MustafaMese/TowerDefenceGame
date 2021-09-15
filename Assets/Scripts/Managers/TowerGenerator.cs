using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGenerator : MonoBehaviour
{
    [SerializeField] List<Tower> towerPrefabs = new List<Tower>();
    [SerializeField] ProjectileObjectPoolHandler projectileHandler;

    private List<Tile> emptyTiles = new List<Tile>();
    private void Start()
    {
        FindEmptyTiles();
    }

    private void FindEmptyTiles()
    {
        var tiles = FindObjectsOfType<Tile>();
        foreach (var tile in tiles)
        {
            if (tile.Content.Type == TileContentType.EMPTY)
                emptyTiles.Add(tile);
        }
    }

    public void Create()
    {
        if (emptyTiles.Count < 1)
        {
            Debug.Log("Cant find empty tile!");
            return;
        }

        var number = Random.Range(0, emptyTiles.Count);
        Tile tile = emptyTiles[number];

        if (tile.IsThereTower)
            Destroy(tile.GetTower().gameObject);

        number = Random.Range(0, towerPrefabs.Count);
        var tower = Instantiate(towerPrefabs[number], tile.transform);
        tile.SetTower(tower);
        tower.SetProjectilePoolHandler(projectileHandler);
    }
}
