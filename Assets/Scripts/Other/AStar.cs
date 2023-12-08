using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    private readonly float _randomValue;
    
    public AStar(float randomValue)
    {
        _randomValue = randomValue;
    }
    
    public List<Tile> FindPath(Tile startTile, Tile goalTile, int pathLength = -1)
    {
        var start = startTile.Grid;
        var goal = goalTile.Grid;
        
        List<Grid> openList = new ();
        List<Grid> closedList = new ();

        openList.Add(start);

        while (openList.Count > 0)
        {
            Grid current = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].F < current.F || (openList[i].F == current.F && openList[i].H < current.H))
                    current = openList[i];
            }

            openList.Remove(current);
            closedList.Add(current);

            if (current == goal)
            {
                return ConstructPath(current, pathLength);
            }

            List<Grid> neighbors = GetNeighbors(current);

            foreach (Grid neighbor in neighbors)
            {
                if (closedList.Contains(neighbor))
                    continue;

                if(neighbor != start && neighbor.IsBlocked)
                    continue;

                float tentativeG = current.G + Random.Range(0f, _randomValue);

                if (!neighbor.IsBlocked && (!openList.Contains(neighbor) || tentativeG < neighbor.G))
                {
                    neighbor.G = tentativeG;
                    neighbor.H = CalculateH(neighbor, goal);
                    neighbor.Parent = current;

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                }
            }
        }

        return null; // No path found
    }

    private float CalculateH(Grid node, Grid goal)
    {
        // Example: Manhattan Distance as the heuristic
        return Mathf.Abs(node.Coordinate.x - goal.Coordinate.x) + Mathf.Abs(node.Coordinate.y - goal.Coordinate.y);
    }

    private List<Grid> GetNeighbors(Grid node)
    {
        List<Grid> neighbors = new();

        if (node.UpLeft != null)
            neighbors.Add(node.UpLeft);
        if (node.DownLeft != null)
            neighbors.Add(node.DownLeft);
        if (node.UpRight != null)
            neighbors.Add(node.UpRight);
        if (node.DownRight != null)
            neighbors.Add(node.DownRight);
        if (node.Left != null)
            neighbors.Add(node.Left);
        if (node.Right != null)
            neighbors.Add(node.Right);

        return neighbors;
    }

    private List<Tile> ConstructPath(Grid node, int pathLength)
    {
        List<Tile> path = new ();
        while (node != null)
        {
            path.Add(node.Tile);
            node = node.Parent;
        }
        
        path.Reverse();

        if (pathLength != -1 && pathLength < path.Count)
        {
            path.RemoveRange(pathLength, path.Count - pathLength);
        }
        
        return path;
    }
}