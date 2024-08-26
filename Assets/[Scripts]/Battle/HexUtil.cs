using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexUtil
{
    public static List<HexTile> GetReachableHexes(HexTile startinghex, int range)
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
                if (hex.type != HexType.IMPASSABLE && templocal < hex.localValue && !hex.occupant)
                    hex.localValue = templocal;

                if (hex.type != HexType.IMPASSABLE && !hex.pathfindingVisited && hex.localValue <= range && !hex.occupant)
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

     public static bool CheckLineOfSight(List<HexTile> line)
    {
        foreach (HexTile h in line)
        {
            if (h.type == HexType.FOREST || h.type == HexType.IMPASSABLE)
            {
                return false;

            }
        }
        return true;
    }

    public static int DistancebetweenHexs(HexTile a, HexTile b)
    {
        return (Mathf.Abs(a.coordinates.x - b.coordinates.x) +
                  Mathf.Abs(a.coordinates.y - b.coordinates.y) +
                  Mathf.Abs(a.coordinates.z - b.coordinates.z)) / 2;
    }

    public static Vector3 HexLerp(HexTile a, HexTile b, float t)
    {
        Vector3 temp = new Vector3(Mathf.Lerp(a.coordinates.x, b.coordinates.x, t),
                                   Mathf.Lerp(a.coordinates.y, b.coordinates.y, t),
                                   Mathf.Lerp(a.coordinates.z, b.coordinates.z, t));

        return temp;
    }


    public static Vector3Int HexRound(Vector3 cube)
    {
        float rx = Mathf.Round(cube.x);
        float ry = Mathf.Round(cube.y);
        float rz = Mathf.Round(cube.z);

        float x_diff = Mathf.Abs(rx - cube.x);
        float y_diff = Mathf.Abs(ry - cube.y);
        float z_diff = Mathf.Abs(rz - cube.z);

        if (x_diff > y_diff && x_diff > z_diff)
            rx = -ry - rz;
        else if (y_diff > z_diff)
            ry = -rx - rz;
        else
            rz = -rx - ry;

        return new Vector3Int((int)rx, (int)ry, (int)rz);
    }
}
