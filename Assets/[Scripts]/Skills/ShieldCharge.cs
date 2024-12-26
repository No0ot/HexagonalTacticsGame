using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shield Charge", menuName = "Unit/Skill/Shield Charge")]
public class ShieldCharge : Skill
{
    public override List<HexTile> GetHexesInRange(Dictionary<Vector2Int, HexTile> hexTiles)
    {
        List<HexTile> tiles = new List<HexTile>();
        HexTile startTile = user.tile;

        Vector2Int[] hexDirections = {
        new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0, -1),
        new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0, 1)
        };

        foreach(Vector2Int direction in hexDirections)
        {
            tiles.AddRange(HexPathfinding.GetHexsInDirection(startTile, range, direction, hexTiles));
        }
        

        return tiles;
    }

    public override void UseSkill(HexTile targetedHex, Dictionary<Vector2Int, HexTile> hexTiles)
    {
        List<HexTile> line = HexPathfinding.HexLineDraw(user.tile, targetedHex, hexTiles);
        List<HexTile> neighbours = HexPathfinding.GetNeighbors(targetedHex, hexTiles);

        HexTile chargeTile = null;

        foreach (HexTile lineTile in line)
        {
            for(int i = 0; i < neighbours.Count; i++)
            {
                HexTile neighbour = neighbours[i];
        
                if (lineTile == neighbour)
                {
                    chargeTile = neighbour;
                    while(chargeTile.Occupant)
                    {
                        i++;
                        if(i > neighbours.Count)
                        {
                            Debug.Log("No tile for Charge");
                            return;
                        }
                        chargeTile = neighbours[i];
        
                    }
        
                }
            }
        }

        if(chargeTile != null)
        {
            user.PlaceUnit(chargeTile);
        }

        base.UseSkill(targetedHex, hexTiles);
    }
}
