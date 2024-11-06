using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BuffStrength : Effect
{
    StatModifier modifier;

    public BuffStrength(float value)
    {
        modifier = new StatModifier(value, StatModifierType.FLAT, this);
        type = EffectType.STATIC;
    }

    public override void ApplyEffect(UnitObject target)
    {
        target.unitInfo.localStats.GetStat(Stat.STRENGTH).statModifiers.Add(modifier);
        target.unitInfo.RecalculateHealth();
    }

    public override void RemoveEffect(UnitObject unit)
    {
        if (modifier != null)
        {
            foreach (StatInfo stat in unit.unitInfo.localStats.statInfo)
            {
                List<StatModifier> modsToRemove = new List<StatModifier>();
                foreach(StatModifier mod in stat.statModifiers)
                {
                    if(mod.source == (object)this)
                    {
                        modsToRemove.Add(mod);
                    }
                }

                foreach(StatModifier mod in modsToRemove)
                {
                    stat.statModifiers.Remove(mod);
                }
            }
        }
        unit.unitInfo.RecalculateHealth();

        unit.effects.Remove(this);
    }
}
