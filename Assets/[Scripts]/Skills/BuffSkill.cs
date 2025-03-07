using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffSkill", menuName = "Unit/Skill/BuffSkill")]
public class BuffSkill : Skill
{
    public override List<HexTile> GetHexesInRange(Dictionary<Vector2Int, HexTile> hexTiles)
    {
        List<HexTile> tiles = new List<HexTile>();

        tiles = HexPathfinding.GetTilesWithinAttackRange(user.tile, hexTiles, range, false);

        return tiles;
    }

    public override void UseSkill(HexTile targetedHex, Dictionary<Vector2Int, HexTile> hexTiles)
    {
        base.UseSkill(targetedHex, hexTiles);
    }
}
