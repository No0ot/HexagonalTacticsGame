using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Stat
{
    //Main Stats
    STRENGTH,
    FINESSE,
    CONCENTRATION,
    RESOLVE,
    STRENGTH_GROWTH,
    FINESSE_GROWTH,
    CONCENTRATION_GROWTH,
    RESOLVE_GROWTH,
    //Combat Stats
    MAX_HEALTH,
    CURRENT_HEALTH,
    MOVEMENT_RANGE,
    DASH_RANGE,
    MIN_DAMAGE,
    MAX_DAMAGE,
    DAMAGE_VARIANCE,
    INITIATIVE,
    THREAT,
    // Skill Stats
    RANGE,
    RADIUS,
    COOLDOWN,
    ARMOR, 
}

public enum StatModifierType
{
    FLAT,
    PERCENT
}

[Serializable]
public class StatInfo
{
    public Stat statType;
    public float baseValue;

    public List<StatModifier> statModifiers =  new List<StatModifier>();

    public float CalculateFinalValue()
    {
        float finalValue = baseValue;

        if(statModifiers.Count < 1)
        {
            return finalValue;
        }

        for(int i = 0; i < statModifiers.Count; i++)
        {
            StatModifier mod = statModifiers[i];

            if (mod.type == StatModifierType.FLAT)
            {
                finalValue += mod.value;
            }
            else if(mod.type == StatModifierType.PERCENT)
            {
                finalValue = baseValue * (mod.value);
            }
        }

        return (float)Math.Round(finalValue, 4);
    }
}

[Serializable]
public class StatModifier
{
    public float value;
    public StatModifierType type;
    public int order;
    public object source;
    public StatModifier(float stat, StatModifierType stattype, int newOrder, object newSource)
    {
        value = stat;
        type = stattype;
        order = newOrder;
        source = newSource;
    }

    public StatModifier(float value, StatModifierType type, object source) : this(value, type, (int)type, source) { }
}

[CreateAssetMenu(fileName = "NewStats", menuName = "Stats")]
public class Stats : ScriptableObject
{
    public List<StatInfo> statInfo = new List<StatInfo>();

    public Stats(Stats template)
    {
        foreach(StatInfo info in template.statInfo)
        {
            StatInfo tempInfo = new StatInfo();
            tempInfo.statType = info.statType;
            tempInfo.baseValue = info.baseValue;

            statInfo.Add(tempInfo);
        }
    }

    public StatInfo GetStat(Stat stat)
    {
        foreach(var s in statInfo)
        {
            if (s.statType == stat)
                return s;
        }

        Debug.LogError($"No Stat value found for {stat} on {this.name}");
        return null;
    }

    public void SetStat(Stat stat, float newValue)
    {
        foreach (var s in statInfo)
        {
            if (s.statType == stat)
            {
                s.baseValue = newValue;
                return;
            }
        }

        Debug.LogError($"No Stat value found for {stat} on {this.name}");
    }

    public void EditStat(Stat stat, float newValue)
    {
        foreach (var s in statInfo)
        {
            if (s.statType == stat)
            {
                s.baseValue += newValue;
                return;
            }

        }

        Debug.LogError($"No Stat value found for {stat} on {this.name}");
    }

    public void AddStatModifier(Stat stat, StatModifier modifier)
    {
        foreach (var s in statInfo)
        {
            if (s.statType == stat)
            {
                s.statModifiers.Add(modifier);
                return;
            }

        }

        Debug.LogError($"No Stat value found for {stat} on {this.name}");
    }

    public void RemoveStatModifer()
    {

    }
}
