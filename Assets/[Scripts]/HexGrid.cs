using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HexGrid : MonoBehaviour
{
    public List<HexTile> hexPrefabs;
    public int mapRadius;

    public List<HexTile> highlightedTiles = new List<HexTile>();
    public List<HexTile> hexList = new List<HexTile>();

    static List<Vector3Int> directions = new List<Vector3Int>() { new Vector3Int(1,0,-1), new Vector3Int(1,-1,0), new Vector3Int(0,-1,1), new Vector3Int(-1,0,1), new Vector3Int(-1,1,0), new Vector3Int(0,1,-1) };

    public void BuildGrid()
    {
        for (int q = -mapRadius; q <= mapRadius; q++)
        {
            int r1 = Mathf.Max(-mapRadius, -q - mapRadius);

            int r2 = Mathf.Min(mapRadius, -q + mapRadius);

            for (int r = r1; r <= r2; r++)
            {
                int rand = Random.Range(0,5);
                int type;
                if (rand == 4)
                    type = 2;
                else if (rand == 3)
                    type = 1;
                else
                    type = 0;
                HexTile temp = Instantiate(hexPrefabs[type], this.transform);
                temp.coordinates = new Vector3Int(q, r, -q - r);
                temp.SetSprites();
                hexList.Add(temp);
                Vector2 position = temp.hex_to_pixel(temp.coordinates);
                temp.gameObject.transform.position = new Vector3(position.x, position.y, 0.0f);
            }
        }

        SetGridNeighbours();
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

    //public List<HexTile> GetReachableHexes(HexTile startinghex, int range)
    //{
    //    LinkedList<HexTile> frontier = new LinkedList<HexTile>();
    //    frontier.AddLast(startinghex);
    //    HexTile current = frontier.First.Value;
    //    current.localValue = 0.0f;
    //
    //    List<HexTile> reached = new List<HexTile>();
    //    while(frontier.Count > 0 && current.localValue < range)
    //    {
    //        //frontier.OrderBy((p1, p2) => p1.localValue.CompareTo(p2.localValue));
    //        frontier.OrderBy(p1 => p1.localValue);
    //
    //        while(frontier.Count > 0 && frontier.First.Value.pathfindingVisited)
    //        {
    //            frontier.RemoveFirst();
    //        }
    //
    //        if (frontier.Count == 0)
    //            break;
    //
    //        current = frontier.First.Value;
    //        current.pathfindingVisited = true;
    //        foreach(HexTile hex in current.neighbours)
    //        {
    //            if (hex == null)
    //                continue;
    //            float templocal = current.localValue + hex.pathfindingCost;
    //            if (hex.type != HexType.FOREST && templocal < hex.localValue && !hex.occupant)
    //                hex.localValue = templocal;
    //
    //            if (hex.type != HexType.FOREST && !hex.pathfindingVisited && hex.localValue <= range && !hex.occupant)
    //            {
    //                frontier.AddLast(hex);
    //                reached.Add(hex);
    //            }
    //            else
    //                continue;
    //        }
    //    }
    //    return reached;
    //}

    public List<HexTile> GetReachableHexes(HexTile startinghex, int range)
    {
        List<HexTile> frontier = new List<HexTile>();
        frontier.Add(startinghex);
        HexTile current = frontier[0];
        current.localValue = 0.0f;

        List<HexTile> reached = new List<HexTile>();
        while (frontier.Count > 0 && current.localValue < range)
        {
            //frontier.OrderBy((p1, p2) => p1.localValue.CompareTo(p2.localValue));
            LocalValueComparison value = new LocalValueComparison();
            frontier.Sort(new LocalValueComparison());

            while (frontier.Count > 0 && frontier[0].pathfindingVisited)
            {
                frontier.RemoveAt(0);
            }

            if (frontier.Count == 0)
                break;

            current = frontier[0];
            current.pathfindingVisited = true;
            foreach (HexTile hex in current.neighbours)
            {
                if (hex == null)
                    continue;
                float templocal = current.localValue + hex.pathfindingCost;
                if (hex.type != HexType.FOREST && templocal < hex.localValue && !hex.occupant)
                    hex.localValue = templocal;

                if (hex.type != HexType.FOREST && !hex.pathfindingVisited && hex.localValue <= range && !hex.occupant)
                {
                    frontier.Add(hex);
                    reached.Add(hex);
                }
                else
                    continue;
            }
        }
        return reached;
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
