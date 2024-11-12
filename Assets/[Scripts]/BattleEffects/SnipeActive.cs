using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipeActive //: Effect
{
    //public List<StatModifier> effectStatModifiers = new List<StatModifier>();
    //
    //public SnipeActive(float value, StatModifierType modtype)
    //{
    //    effectStatModifiers.Add(new StatModifier(value, modtype, this));
    //    type = EffectType.STATIC;
    //
    //    effectStatModifiers.Add(new StatModifier(1, StatModifierType.FLAT, this));
    //}
    //
    //public override void ApplyEffect(UnitObject target)
    //{
    //    target.unitInfo.localStats.GetStat(Stat.MIN_DAMAGE).statModifiers.Add(effectStatModifiers[0]);
    //    target.unitInfo.localStats.GetStat(Stat.MAX_DAMAGE).statModifiers.Add(effectStatModifiers[0]);
    //
    //    target.unitInfo.localStats.GetStat(Stat.RANGE).statModifiers.Add(effectStatModifiers[1]);
    //}
    //
    //public override void RemoveEffect(UnitObject unit)
    //{
    //    if (effectStatModifiers.Count > 0)
    //    {
    //        foreach (StatInfo stat in unit.unitInfo.localStats.statInfo)
    //        {
    //            List<StatModifier> modsToRemove = new List<StatModifier>();
    //            foreach (StatModifier mod in stat.statModifiers)
    //            {
    //                if (mod.source == (object)this)
    //                {
    //                    modsToRemove.Add(mod);
    //                }
    //            }
    //
    //            foreach (StatModifier mod in modsToRemove)
    //            {
    //                stat.statModifiers.Remove(mod);
    //            }
    //        }
    //    }
    //    unit.effects.Remove(this);
    //}
    //
}
