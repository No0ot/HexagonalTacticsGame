using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum MapType
{
    HEXAGONAL,
    PARRALLELOGRAM,
    RECTANGLE,
    TRIANGLE
}

public class HexGrid : MonoBehaviour
{
    public List<HexTile> hexPrefabs;
    public int mapRadius;

    public List<HexTile> highlightedTiles = new List<HexTile>();
    public List<HexTile> hexList = new List<HexTile>();

    static List<Vector3Int> directions = new List<Vector3Int>() { new Vector3Int(1,0,-1), new Vector3Int(1,-1,0), new Vector3Int(0,-1,1), new Vector3Int(-1,0,1), new Vector3Int(-1,1,0), new Vector3Int(0,1,-1) };
    public MapType mapType;

    public HexTile GetSpawnTile(bool opposite)
    {
        if (!opposite)
            return GetHex(new Vector3Int(-mapRadius + 1, 0, mapRadius - 1));
        else
            return GetHex(new Vector3Int(mapRadius - 1, 0, -mapRadius + 1));
    }
    public void BuildGrid()
    {
        switch(mapType)
        {
            case MapType.HEXAGONAL:
                BuildHexagonalMap();
                break;
            case MapType.PARRALLELOGRAM:
                break;
        }

        SetGridNeighbours();
    }

    void BuildHexagonalMap()
    {
        for (int q = -mapRadius; q <= mapRadius; q++)
        {
            int r1 = Mathf.Max(-mapRadius, -q - mapRadius);

            int r2 = Mathf.Min(mapRadius, -q + mapRadius);

            for (int r = r1; r <= r2; r++)
            {
                CreateTile(q, r);
            }
        }
    }

    void BuildParrallelogramMap()
    {
        for(int q = -mapRadius; q <= mapRadius; q++)
        {
            
        }
    }

    void CreateTile(int q, int r)
    {
        int rand = Random.Range(0, 10);
        int type;

        if (rand < 2)
            type = 1;
        else if (rand < 5)
            type = 2;
        else if (rand < 6)
            type = 3;
        else
            type = 0;

        HexTile temp = Instantiate(hexPrefabs[type], this.transform);
        temp.coordinates = new Vector3Int(q, r, -q - r);
        temp.SetSprites();
        hexList.Add(temp);
        Vector2 position = temp.hex_to_pixel(temp.coordinates);
        temp.gameObject.transform.position = new Vector3(position.x, position.y, 0.0f);
    }

    HexTile GetHex(Vector3Int coord)
    {
        foreach(HexTile tile in hexList)
        {
            if(tile.coordinates == coord)
            {
                return tile;
            }
        }
        return null;
    }

    public void RecomputeGlobalValues(Vector3Int goal)
    {
        foreach(HexTile tile in hexList)
        {
            tile.ComputeGlobalValue(goal);
        }
    }

    void SetGridNeighbours()
    {
        foreach(HexTile tile in hexList)
        {
            SetTileNeighbours(tile);
        }
    }

    void SetTileNeighbours(HexTile tile)
    {
        Vector3Int temp = tile.coordinates;

        for(int i = 0; i < 6; i++)
        {
            tile.neighbours.Add(GetHex(temp + directions[i]));
        }
    }
    public List<HexTile> GetThreatenedTiles(HexTile startinghex, int range)
    {
        foreach (HexTile t in hexList)
        {
            if (t.globalValue <= range && HexUtil.CheckLineOfSight(HexLineDraw(startinghex, t)))
            {
                highlightedTiles.Add(t);
                t.ActivateHighlight(HighlightColor.THREATEN);
            }
        }
        return highlightedTiles;
    }

    public List<HexTile> HexLineDraw(HexTile a, HexTile b)
    {
        int distance = HexUtil.DistancebetweenHexs(a, b);
        List<HexTile> results = new List<HexTile>();
        float num;
        if (distance > 1)
            num = distance;
        else
            num = 1;

        float step = 1.0f / num;

        for (int i = 0; i < distance; i++)
        {
            //std::cout << thing->getCubeCoordinate().x << " " << thing->getCubeCoordinate().y << " " << thing->getCubeCoordinate().z << std::endl;
            results.Add(GetHex(HexUtil.HexRound(HexUtil.HexLerp(a, b, step * i))));
        }

        return results;
    }

    public void ResetTiles()
    {
        foreach(HexTile tile in hexList)
        {
            tile.pathfindingVisited = false;
            tile.localValue = 100;
            tile.ActivateHighlight(HighlightColor.NONE);
        }
        highlightedTiles.Clear();
    }

    public void ResetHexPathfindingValues()
    {
        foreach (HexTile tile in hexList)
        {
            tile.pathfindingVisited = false;
            tile.localValue = 100;
        }
    }
}
