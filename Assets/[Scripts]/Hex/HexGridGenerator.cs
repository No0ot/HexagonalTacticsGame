using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum MapType
{
    Rectangular,
    Hexagonal,
    Parralellogram
}
public class HexGridGenerator : MonoBehaviour
{
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float hexSize = 1.0f;

    // Perlin Noise settings
    public float noiseScale = 0.1f; // Scale of Perlin noise (controls smoothness)
    public int maxHeight = 5; // Maximum height for the tiles

    // References to different tile prefabs
    public GameObject grassTilePrefab;
    public GameObject sandTilePrefab;
    public GameObject waterTilePrefab;
    public GameObject rockyTilePrefab;
    public GameObject forestTilePrefab;

    // Optionally use an array to reference all prefabs
    public GameObject[] tilePrefabs;

    public Dictionary<Vector2Int, HexTile> hexTiles { get; private set; }

    // Axial directions used for hexagonal grid
    private readonly Vector2Int[] hexDirections = {
        new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0, -1),
        new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0, 1)
    };

    public void InitializeHexGrid()
    {
        hexTiles = new Dictionary<Vector2Int, HexTile>();
        GenerateHexagonalGrid(gridWidth);
    }

    public void GenerateRectangularGrid(int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int axialCoordinates = new Vector2Int(x, y - (x / 2)); // Axial coordinate offset
                CreateHexTile(axialCoordinates);
            }
        }
    }

    public void GenerateHexagonalGrid(int radius)
    {
        for (int q = -radius; q <= radius; q++)
        {
            int r1 = Mathf.Max(-radius, -q - radius);
            int r2 = Mathf.Min(radius, -q + radius);
            for (int r = r1; r <= r2; r++)
            {
                Vector2Int axialCoordinates = new Vector2Int(q, r);
                CreateHexTile(axialCoordinates);
            }
        }
    }

    public void GenerateParallelogramGrid(int width, int height)
    {
        for (int q = 0; q < width; q++)
        {
            for (int r = 0; r < height; r++)
            {
                Vector2Int axialCoordinates = new Vector2Int(q, r);
                CreateHexTile(axialCoordinates);
            }
        }
    }

    private void CreateHexTile(Vector2Int axialCoordinates)
    {
        Vector3 worldPosition = AxialToWorld(axialCoordinates);

        TileType tileType = GetRandomTileType(axialCoordinates);

        // Get the correct prefab based on the tile type
        GameObject prefabToSpawn = GetPrefabForTileType(tileType);

        GameObject hexTileGO = Instantiate(prefabToSpawn, worldPosition, Quaternion.identity);
        hexTileGO.name = $"HexTile {axialCoordinates}";

        int height = CalculateHeight(axialCoordinates); // You can randomly assign heights or predefine them.

        HexTile hexTile = hexTileGO.GetComponent<HexTile>();
        hexTile.SetHexTile(axialCoordinates, height);
        hexTiles.Add(axialCoordinates, hexTile);
    }

    private Vector3 AxialToWorld(Vector2Int axial)
    {
        float x = hexSize * (3f / 2f * axial.x);
        float z = hexSize * (Mathf.Sqrt(3f) * (axial.y + axial.x / 2f));
        return new Vector3(x, 0, z);
    }

    // Calculate tile height using Perlin noise
    private int CalculateHeight(Vector2Int axialCoordinates)
    {
        // Normalize axial coordinates for consistent noise values
        float x = (float)axialCoordinates.x / gridWidth * noiseScale;
        float y = (float)axialCoordinates.y / gridHeight * noiseScale;

        // Generate Perlin noise value and scale it to the maximum height, then round to an integer
        float noiseValue = Mathf.PerlinNoise(x, y);
        return Mathf.RoundToInt(noiseValue * maxHeight);
    }

    // Logic to get tile type (could be random or from a predefined map)
    private TileType GetRandomTileType(Vector2Int axialCoordinates)
    {
        // Normalize axial coordinates for consistent noise values
        float x = (float)axialCoordinates.x / gridWidth * noiseScale + 100f;
        float y = (float)axialCoordinates.y / gridHeight * noiseScale + 100f;

        // Generate Perlin noise value and scale it to the maximum height, then round to an integer
        float noiseValue = Mathf.PerlinNoise(x, y);

        if (noiseValue < 0.2f) return TileType.Water; // Low noise value for water
        if (noiseValue < 0.4f) return TileType.Sand; // Medium low noise value for sand
        if (noiseValue < 0.6f) return TileType.Grass; // Medium noise value for grass
        if (noiseValue < 0.8f) return TileType.Forest; // Medium high noise value for forest
        return TileType.Rocky; // High noise value for rocky terrain
    }

    // Get the correct prefab based on the tile type
    GameObject GetPrefabForTileType(TileType tileType)
    {
        switch (tileType)
        {
            case TileType.Grass:
                return grassTilePrefab;
            case TileType.Sand:
                return sandTilePrefab;
            case TileType.Water:
                return waterTilePrefab;
            case TileType.Rocky:
                return rockyTilePrefab;
            case TileType.Forest:
                return forestTilePrefab;
            default:
                return grassTilePrefab; // Fallback in case something goes wrong
        }
    }

    // Example of how you might calculate the position of hex tiles in the grid
    Vector3 CalculateHexPosition(int x, int y)
    {
        float offset = (y % 2 == 0) ? 0 : hexSize * 0.866f; // Offset for hex grid layout
        float xPos = x * hexSize * 1.5f + offset;
        float yPos = y * hexSize * 0.866f;
        return new Vector3(xPos, 0, yPos);
    }

    public HexTile GetRandomWalkableTile()
    {
        // Create a list of walkable tiles
        List<HexTile> walkableTiles = new List<HexTile>();

        // Loop through the hexTiles dictionary
        foreach (var hexTile in hexTiles.Values)
        {
            if (hexTile.IsWalkable())
            {
                walkableTiles.Add(hexTile);
            }
        }

        // If there are no walkable tiles, return null
        if (walkableTiles.Count == 0)
            return null;

        // Select a random walkable tile
        int randomIndex = Random.Range(0, walkableTiles.Count);
        return walkableTiles[randomIndex];
    }

    public HexTile GetRandomNeighbour(HexTile Tile)
    {
        HexTile newTile = null;
        List<HexTile> neighbours = HexPathfinding.GetNeighbors(Tile, hexTiles);

        newTile = neighbours[Random.Range(0, neighbours.Count() - 1)];
        return newTile;
    }
}