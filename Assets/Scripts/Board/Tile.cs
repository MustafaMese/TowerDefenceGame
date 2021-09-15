using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] MeshRenderer grid;
    [SerializeField] Transform pathPoint;

    public Tile north, east, south, west;
    public Tile next;
    private int distance;
    private Quaternion northRotation = Quaternion.Euler(0f, 180f, 0f),
        eastRotation = Quaternion.Euler(0f, 270f, 0f),
        southRotation = Quaternion.Euler(0f, 0f, 0f),
        westRotation = Quaternion.Euler(0f, 90f, 0f);

    public TileContent content;
    public TileContent Content
    {
        get => content;
        set
        {
            if (content != null && content.Type != value.Type)
                DestroyImmediate(content.gameObject);
            content = value;
            content.transform.localPosition = transform.localPosition;
            content.transform.SetParent(transform);
        }
    }

    public Tile Next { get => next; }
    public bool IsAlternative { get; set; }
    public Vector3 PathPoint { get => pathPoint.position; }

    private Tower tower;
    public bool IsThereTower => tower != null;
    public void SetTower(Tower tower) => this.tower = tower;
    public Tower GetTower() => tower;

    public void SetContent()
    {
        if (GetComponentInChildren<TileContent>())
        {
            Content = GetComponentInChildren<TileContent>();
            Content.transform.position = transform.position;
        }

    }

    private void Awake()
    {
        SetContent();
        if (content.Type == TileContentType.EMPTY)
            next = null;

        meshRenderer.gameObject.SetActive(false);
        grid.gameObject.SetActive(false);
    }

    #region NEIGHBOR METHODS

    public void ClearPath()
    {
        distance = int.MaxValue;
        next = null;
    }

    public void BecomeDestination()
    {
        distance = 0;
        next = null;
    }

    public bool HasPath()
    {
        return distance != int.MaxValue;
    }

    private Tile GrowPath(Tile neighbor)
    {
        if (!HasPath() || neighbor == null || neighbor.HasPath())
        {
            return null;
        }
        neighbor.distance = distance + 1;
        neighbor.next = this;
        return neighbor.Content.Type != TileContentType.EMPTY ? neighbor : null;
    }

    public Tile GrowPathNorth() => GrowPath(north);

    public Tile GrowPathEast() => GrowPath(east);

    public Tile GrowPathSouth() => GrowPath(south);

    public Tile GrowPathWest() => GrowPath(west);

    public static void AddEastWestNeighbors(Tile east, Tile west)
    {
        west.east = east;
        east.west = west;
    }

    public static void AddNorthSouthNeighbors(Tile north, Tile south)
    {
        south.north = north;
        north.south = south;
    }

    #endregion

    public void ShowPath()
    {
        if (Content == null || Content.Type != TileContentType.ARROW) return;

        if (distance == 0)
        {
            Content.gameObject.SetActive(false);
            return;
        }

        Content.gameObject.SetActive(true);
        Content.transform.localRotation =
            next == north ? northRotation :
            next == east ? eastRotation :
            next == south ? southRotation :
            westRotation;
    }
}
