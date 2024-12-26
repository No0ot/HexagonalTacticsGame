using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(fileName = "New Special Attack", menuName = "Unit/Skill/Special Attack")]
public class SpecialAttack : Skill
{
    public override List<HexTile> GetHexesInRange(Dictionary<Vector2Int, HexTile> hexTiles)
    {
        List<HexTile> hex = new List<HexTile>();
        hex.Add(user.tile);

        return hex;
    }

    public override void UseSkill(HexTile targetedHex, Dictionary<Vector2Int, HexTile> hexTiles)
    {
        base.UseSkill(targetedHex, hexTiles);
    }
}
