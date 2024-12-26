using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType
{
    ALLY,
    SELF,
    ENEMY,
    BOTH,
    ATTACK,
}

[Serializable]
public class TargetedEffect
{
    public TargetType target;
    public int duration;
    public Effect effect;
}
public abstract class Skill : ScriptableObject
{
    public UnitObject user;

    public int range;
    public int radius;
    public int cooldown;
    public List<TargetedEffect> effects = new List<TargetedEffect>();
    public Sprite sprite;
    public virtual void UseSkill(HexTile targetedHex, Dictionary<Vector2Int, HexTile> hexTiles)
    {
        List<UnitObject> targetedUnits = new List<UnitObject>();

        List<HexTile> affectedHexes = HexPathfinding.GetTilesWithinAttackRange(targetedHex, hexTiles, radius);

        foreach (TargetedEffect tEffect in effects)
        {
            Effect newEffect = new Effect(tEffect.effect, user);
            newEffect.duration = tEffect.duration;

            switch(tEffect.target)
            {
                case TargetType.SELF:
                    newEffect.ApplyEffect(user);
                    break;

                case TargetType.ENEMY:
                    foreach (HexTile hex in affectedHexes)
                    {
                        if (hex.Occupant)
                        {
                            if (hex.Occupant.unitInfo.GetPlayer() != user.unitInfo.GetPlayer())
                                newEffect.ApplyEffect(hex.Occupant);
                        }
                    }
                    break;

                case TargetType.BOTH:
                    foreach (HexTile hex in affectedHexes)
                    {
                        if (hex.Occupant)
                        {
                            newEffect.ApplyEffect(hex.Occupant);
                        }
                    }
                    break;

                case TargetType.ALLY:
                    foreach (HexTile hex in affectedHexes)
                    {
                        if (hex.Occupant)
                        {
                            if(hex.Occupant.unitInfo.GetPlayer() == user.unitInfo.GetPlayer())
                                newEffect.ApplyEffect(hex.Occupant);
                        }
                    }
                    break;

                case TargetType.ATTACK:
                    user.attackAppliedEffects.Add(newEffect);
                    break;
            }
        }
    }

    public abstract List<HexTile> GetHexesInRange(Dictionary<Vector2Int, HexTile> hexTiles);

}
