using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public enum EffectType
{
    STATIC,
    CONTINUOUS,
}

[Serializable]
public class EffectMod
{
    public Stat moddedStat;
    public StatModifier modifier;
}
[Serializable]
[CreateAssetMenu(fileName = "New Effect", menuName = "Unit/Effect")]
public class Effect : ScriptableObject
{
    public int duration;
    public EffectType type;
    //public TargetType target;
    public Sprite sprite;
    public List<EffectMod> mods = new List<EffectMod>();

    public Effect(Effect template)
    {
        duration = template.duration;
        type = template.type;
        //target = template.target;
        sprite = template.sprite;
        mods = template.mods;
    }
    public void ApplyEffect(UnitObject target)
    {
        target.effects.Add(this);
        foreach(EffectMod mod in mods)
        {
            target.unitInfo.localStats.GetStat(mod.moddedStat).statModifiers.Add(mod.modifier);
        }

        target.unitInfo.RecalculateHealth();
    }

    public void RemoveEffect(UnitObject unit)
    {
        foreach (EffectMod Emod in mods)
        {
            foreach (StatInfo stat in unit.unitInfo.localStats.statInfo)
            {
                List<StatModifier> modsToRemove = new List<StatModifier>();
                foreach (StatModifier mod in stat.statModifiers)
                {
                    if (mod.source == (object)this)
                    {
                        modsToRemove.Add(mod);
                    }
                }

                foreach (StatModifier mod in modsToRemove)
                {
                    stat.statModifiers.Remove(mod);
                }
            }
        }

        unit.unitInfo.RecalculateHealth();
        unit.effects.Remove(this);
    }

}
