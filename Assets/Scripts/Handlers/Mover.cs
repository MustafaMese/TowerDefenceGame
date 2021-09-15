using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Mover : MonoBehaviour
{
    private List<Vector3> path = new List<Vector3>();

    private void Start()
    {
        DOTween.Init();
    }

    private Vector3[] CreatePath(Tile tile)
    {
        path.Clear();

        path.Add(tile.PathPoint);
        while (tile.Next != null)
        {
            path.Add(tile.Next.PathPoint);
            tile = tile.next;
        }
        return path.ToArray();
    }

    public void Move(Tile tile, float movementDuration)
    {
        var pathArray = CreatePath(tile);
        transform.DOPath(
                pathArray,
                movementDuration,
                PathType.Linear,
                PathMode.Full3D,
                10,
                Color.red).
            SetLookAt(0.01f, Vector3.forward).
            SetEase(Ease.Linear);
    }
}
