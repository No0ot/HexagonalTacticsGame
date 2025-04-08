using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public bool scaleWithMainStat = false;
    public float mainStatScaleValue = 1.0f;
}
[Serializable]
[CreateAssetMenu(fileName = "New Effect", menuName = "Unit/Effect")]
public class Effect : ScriptableObject
{
    public string name;
    public string description;
    public int duration;
    public EffectType type;
    //public TargetType target;
    public Sprite sprite;
    public List<EffectMod> mods = new List<EffectMod>();
    [HideInInspector]
    public UnitObject source;
    

     public float threatGenerated = 0f;

    public Effect(Effect template, UnitObject newSource)
    {
        duration = template.duration;
        type = template.type;
        //target = template.target;
        sprite = template.sprite;
        mods = template.mods;
        source = newSource;
        name = template.name;
        description = template.description;
    }
    public void ApplyEffect(UnitObject target)
    {
        target.effects.Add(this);
        foreach(EffectMod mod in mods)
        {
            if (mod.scaleWithMainStat)
            {
                mod.modifier.value += source.unitInfo.localStats.GetStat(source.unitInfo.job.mainAttribute).CalculateFinalValue() * mod.mainStatScaleValue;
            }

            if(mod.moddedStat == Stat.CURRENT_HEALTH)
            {
                //target.unitInfo.localStats.EditStat(Stat.CURRENT_HEALTH, -mod.modifier.value);
                target.TakeDamage(mod.modifier.value);
            }
            else
                target.unitInfo.localStats.GetStat(mod.moddedStat).statModifiers.Add(mod.modifier);
        }

        target.unitInfo.RecalculateHealth();

        source.unitInfo.localStats.EditStat(Stat.THREAT, threatGenerated);
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
