using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BolsterStrength", menuName = "Unit/Skill/Bolster Strength")]
public class BolsterStrength : Skill
{
    public float value;
    public int duration;

    public override void UseSkill(List<UnitObject> target)
    {
        foreach(UnitObject u in target)
        {
            Effect newEffect = new BuffStrength(value);
            newEffect.duration = duration;
            newEffect.ApplyEffect(u);
            u.effects.Add(newEffect);
        }
    }
}
