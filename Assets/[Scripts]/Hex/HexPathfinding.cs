using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Node
{
    public HexTile HexTile; // The tile associated with this node
    public float GCost; // Cost from the start node
    public float HCost; // Heuristic cost to the end node
    public Node Parent; // Parent node for path reconstruction

    public float FCost => GCost + HCost; // Total cost
}

public static class HexPathfinding
{
    public static List<HexTile> FindPath(HexTile startTile, HexTile targetTile, Dictionary<Vector2Int, HexTile> hexTiles)
    {
        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        Node startNode = new Node { HexTile = startTile, GCost = 0 };
        Node targetNode = new Node { HexTile = targetTile };

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            // Get node with the lowest FCost
            Node currentNode = openList.OrderBy(n => n.FCost).First();

            if (currentNode.HexTile == targetNode.HexTile)
            {
                return RetracePath(startNode, currentNode); // Implement path retracing
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // Check neighbors
            foreach (var neighborHexTile in GetNeighbors(currentNode.HexTile, hexTiles))
            {
                // Skip if neighbor blocks movement
                if (neighborHexTile.BlocksMovement)
                    continue;

                // Create neighbor node
                Node neighborNode = new Node { HexTile = neighborHexTile };

                // Calculate the new GCost (current node's GCost + neighbor's movement cost)
                float newCostToNeighbor = currentNode.GCost + neighborHexTile.MovementCost + GetDistance(currentNode.HexTile, neighborHexTile);

                // Only proceed if the new GCost is lower, or the node hasn't been evaluated yet
                if (newCostToNeighbor < neighborNode.GCost || !openList.Any(n => n.HexTile == neighborHexTile))
                {
                    neighborNode.GCost = newCostToNeighbor;
                    neighborNode.HCost = GetDistance(neighborNode.HexTile, targetNode.HexTile);
                    neighborNode.Parent = currentNode;

                    // If not already in the open list, add it
                    if (!openList.Any(n => n.HexTile == neighborNode.HexTile))
                        openList.Add(neighborNode);
                }
            }
        }

        return null; // No path found
    }

    private static List<HexTile> RetracePath(Node startNode, Node endNode)
    {
        List<HexTile> path = new List<HexTile>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.HexTile);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }

    public static List<HexTile> GetNeighbors(HexTile hexTile, Dictionary<Vector2Int, HexTile> hexTiles)
    {
        List<HexTile> neighbors = new List<HexTile>();

        Vector2Int[] hexDirections = {
        new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0, -1),
        new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0, 1)
    };

        Vector2Int currentAxialCoords = hexTile.AxialCoordinates;

        foreach (Vector2Int direction in hexDirections)
        {
            Vector2Int neighborCoords = currentAxialCoords + direction;

            if (hexTiles.ContainsKey(neighborCoords)) // Ensure the neighbor exists in your grid
            {
                neighbors.Add(hexTiles[neighborCoords]);
            }
        }

        return neighbors;
    }

    public static float GetDistance(HexTile a, HexTile b)
    {
        Vector2Int aCoords = a.AxialCoordinates;
        Vector2Int bCoords = b.AxialCoordinates;

        return (Mathf.Abs(aCoords.x - bCoords.x)
                + Mathf.Abs(aCoords.x + aCoords.y - (bCoords.x + bCoords.y))
                + Mathf.Abs(aCoords.y - bCoords.y)) / 2;
    }

    public static List<HexTile> GetTilesWithinRange(HexTile startTile, Dictionary<Vector2Int, HexTile> hexTiles, int maxMovementRange)
    {
        // Create a list to store reachable tiles within the movement range
        List<HexTile> tilesInRange = new List<HexTile>();

        // A dictionary to store the movement cost to reach each tile
        Dictionary<HexTile, int> movementCostSoFar = new Dictionary<HexTile, int>();
        Queue<HexTile> frontier = new Queue<HexTile>();

        // Initialize the starting tile
        frontier.Enqueue(startTile);
        movementCostSoFar[startTile] = 0;

        // Perform BFS with movement cost tracking
        while (frontier.Count > 0)
        {
            HexTile currentTile = frontier.Dequeue();
            int currentCost = movementCostSoFar[currentTile];

            // Add the current tile to the list if within range
            if (currentCost <= maxMovementRange)
            {
                tilesInRange.Add(currentTile);

                // Check neighbors for further expansion
                foreach (HexTile neighbor in GetNeighbors(currentTile, hexTiles))
                {
                    // Skip if the tile blocks movement or if it has already been processed
                    if (!neighbor.IsWalkable() || movementCostSoFar.ContainsKey(neighbor) || Mathf.Abs(neighbor.Height - currentTile.Height) >= 4)
                        continue;

                    // Calculate the movement cost to reach this neighbor
                    int newCost = currentCost + neighbor.MovementCost;

                    // If the new cost is within the movement range, add it to the frontier
                    if (newCost <= maxMovementRange)
                    {
                        frontier.Enqueue(neighbor);
                        movementCostSoFar[neighbor] = newCost;
                    }
                }
            }
        }

        return tilesInRange;
    }
}
