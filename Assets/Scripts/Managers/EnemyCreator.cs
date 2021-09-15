using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    [SerializeField] List<Unit> enemyPrefabs;
    [SerializeField] float delayTime;
    [SerializeField] float firstCooldownDuration;
    [SerializeField] float cooldownDecreaseMultiplier;
    [SerializeField] float minCooldownValue;
    [SerializeField] float movementDuration;

    private float waitForNextDuration;
    private int beginningTilesCount;
    private List<Tile> beginningTiles = new List<Tile>();
    private bool minTimeReached = false;

    private void Start()
    {
        FindBeginningTiles();

        beginningTilesCount = beginningTiles.Count;
        waitForNextDuration = firstCooldownDuration;

        StartCoroutine(Create());
    }

    private void FindBeginningTiles()
    {
        var tiles = FindObjectsOfType<Tile>();
        foreach (var tile in tiles)
        {
            if (tile.Content.Type == TileContentType.BEGIN)
                beginningTiles.Add(tile);
        }
    }

    private IEnumerator Create()
    {
        yield return new WaitForSeconds(delayTime);
        while (beginningTiles.Count > 0)
        {
            InstantiateEnemy();
            var time = SetDuration();
            yield return new WaitForSeconds(time);
        }
    }

    private float SetDuration()
    {
        if (minTimeReached) return minCooldownValue;

        waitForNextDuration = waitForNextDuration - Time.deltaTime * cooldownDecreaseMultiplier;
        if (Mathf.Max(waitForNextDuration, minCooldownValue) == minCooldownValue)
            minTimeReached = true;

        return waitForNextDuration;
    }

    private void InstantiateEnemy()
    {
        var number = UnityEngine.Random.Range(0, beginningTilesCount);
        Tile startPoint = beginningTiles[number];

        number = UnityEngine.Random.Range(0, enemyPrefabs.Count);

        var enemy = Instantiate(enemyPrefabs[number], startPoint.PathPoint, Quaternion.identity);
        enemy.transform.SetParent(transform);
        enemy.MovementDuration = movementDuration;
        enemy.Move(startPoint);
    }

}
