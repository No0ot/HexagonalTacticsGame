using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    SINGLE,
    AOE,

}

public abstract class Skill : ScriptableObject
{
    public SkillType type;

    public int range;
    public int radius;
    public int cooldown;
    //public float duration;
    public float damage;
    //public Effects[];

    public virtual void GetHexChoices() { }

    public virtual void UseSkill(List<Unit> target) { }

    public static void GetSingleTarget()
    {

    }
}
