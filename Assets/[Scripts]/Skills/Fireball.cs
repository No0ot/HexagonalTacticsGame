using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fireball", menuName = "Unit/Skill/Fireball")]
public class Fireball : Skill
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
