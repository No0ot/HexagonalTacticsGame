using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(fileName = "New Special Attack", menuName = "Unit/Skill/Special Attack")]
public class SpecialAttack : Skill
{
    public Stat mainAttribute;
    public float attributeMultiplier;
    public float damageVariance;

    public override List<HexTile> GetHexesInRange(Dictionary<Vector2Int, HexTile> hexTiles)
    {
        List<HexTile> hex = new List<HexTile>();
        hex.Add(user.tile);
         
        return hex;
    }

    public void SetSpecialAttack()
    {
        if (user.specialAttackSkill == null)
        {
            user.specialAttackSkill = this;
            return;
        }
    }

    public List<HexTile> GetThreatenedHexs(Dictionary<Vector2Int, HexTile> hexTiles)
    {
        List<HexTile> threatenedHexes = new List<HexTile>();

        if (range == 0 && radius > 0)
            threatenedHexes = HexPathfinding.GetTilesWithinAttackRange(user.tile, hexTiles, radius);
        else
            threatenedHexes = HexPathfinding.GetTilesWithinAttackRange(user.tile, hexTiles, (int)user.unitInfo.localStats.GetStat(Stat.RANGE).CalculateFinalValue());

        return threatenedHexes;

    }

    public List<UnitObject> GetThreatenedUnits(List<HexTile> threatenedHexs)
    {
        List<UnitObject> threatenedUnits = new List<UnitObject>();

        foreach(HexTile tile in threatenedHexs)
        {
            if(tile.Occupant)
            {
                // Ignore the user of the skill but still affects allies.
                if (tile.Occupant != user)
                    threatenedUnits.Add(tile.Occupant);
            }
        }

        return threatenedUnits;
    }

    public override void UseSkill(HexTile targetedHex, Dictionary<Vector2Int, HexTile> hexTiles)
    {
        List<UnitObject> targetedUnits = new List<UnitObject>();
        List<HexTile> affectedHexes;

        if (range == 0 && radius > 0)
            affectedHexes = HexPathfinding.GetTilesWithinAttackRange(user.tile, hexTiles, radius);
        else
            affectedHexes = HexPathfinding.GetTilesWithinAttackRange(targetedHex, hexTiles, radius);

        foreach (HexTile hex in affectedHexes)
        {
            if (hex.Occupant)
            {
                if (hex.Occupant != user)
                {
                    if (user.CheckIfHit(hex.Occupant))
                    {
                        if (!user.attackCrit)
                        {
                            float attributeDamage = user.unitInfo.localStats.GetStat(mainAttribute).CalculateFinalValue() * attributeMultiplier;

                            float damage = UnityEngine.Random.Range(attributeDamage, attributeDamage + (damageVariance * user.unitInfo.level));
                            damage *= user.unitInfo.localStats.GetStat(Stat.DAMAGE_MULTIPLIER).CalculateFinalValue();
                            damage = Mathf.RoundToInt(damage);
                            user.unitInfo.localStats.EditStat(Stat.THREAT, damage);
                            hex.Occupant.TakeDamage(damage);
                            CombatTextGenerator.Instance.NewCombatText(hex.Occupant, damage,false);
                        }
                        else
                        {
                            float attributeDamage = user.unitInfo.localStats.GetStat(mainAttribute).CalculateFinalValue() * attributeMultiplier;

                            float damage = UnityEngine.Random.Range(attributeDamage, attributeDamage + (damageVariance * user.unitInfo.level));
                            damage *= user.unitInfo.localStats.GetStat(Stat.DAMAGE_MULTIPLIER).CalculateFinalValue();
                            damage *= user.unitInfo.localStats.GetStat(Stat.CRIT_MULTIPLIER).CalculateFinalValue();
                            damage = Mathf.RoundToInt(damage);
                            user.unitInfo.localStats.EditStat(Stat.THREAT, damage);
                            hex.Occupant.TakeDamage(damage);
                            CombatTextGenerator.Instance.NewCombatText(hex.Occupant, damage, true);
                            user.attackCrit = false;
                        }
                    }
                    else
                    {
                        CombatTextGenerator.Instance.NewCombatText(hex.Occupant, 0f, false);
                        //CombatTextGenerator.Instance.NewCombatText(other, 0f);
                        Debug.Log(" And missed!");
                    }
                }
            }
        }
       
        base.UseSkill(targetedHex, hexTiles);
    }
}
