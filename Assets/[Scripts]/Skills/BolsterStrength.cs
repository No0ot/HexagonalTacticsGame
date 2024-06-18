using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BolsterStrength", menuName = "Unit/Skill/Bolster Strength")]
public class BolsterStrength : Skill
{
    public float value;
    public int duration;

    public override void UseSkill(List<Unit> target)
    {
        foreach(Unit u in target)
        {
            u.localStats.GetStat(Stat.STRENGTH).statModifiers.Add(new StatModifier(value, StatModifierType.FLAT, this));
            u.RecalculateHealth();
        }
    }
}
