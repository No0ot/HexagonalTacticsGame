using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(fileName = "Snipe", menuName = "Unit/Skill/Snipe")]
public class Snipe : Skill
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
        //Effect newEffect = new DamageBuff(damage, StatModifierType.PERCENT);
        //newEffect.sprite = sprite;
        //newEffect.duration = 1;
        //user.effects.Add(newEffect);
        //newEffect.ApplyEffect(user);
        //
        //Effect effect = new BuffStrength(0.9f, StatModifierType.PERCENT);
        //newEffect.sprite = sprite;
        //newEffect.duration = 2;
        //user.attackAppliedEffects.Add(effect);
    }
}
