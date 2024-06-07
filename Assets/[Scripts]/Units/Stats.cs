using System;
using System.Collections;
using System.Collections.Generic;
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
    COOLDOWN
}

[Serializable]
public class StatInfo
{
    public Stat statType;
    public float value;
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
            tempInfo.value = info.value;

            statInfo.Add(tempInfo);
        }
    }

    public float GetStat(Stat stat)
    {
        foreach(var s in statInfo)
        {
            if (s.statType == stat)
                return s.value;
        }

        Debug.LogError($"No Stat value found for {stat} on {this.name}");
        return 0;
    }

    public void SetStat(Stat stat, float newValue)
    {
        foreach (var s in statInfo)
        {
            if (s.statType == stat)
            {
                s.value = newValue;
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
                s.value += newValue;
                return;
            }

        }

        Debug.LogError($"No Stat value found for {stat} on {this.name}");
    }
}
