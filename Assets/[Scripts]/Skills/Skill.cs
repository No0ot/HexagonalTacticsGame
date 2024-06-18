using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType
{
    ALLY,
    SELF,
    ENEMY

}

public abstract class Skill : ScriptableObject
{
    public TargetType type;

    public int range;
    public int radius;
    public int cooldown;
    //public float duration;
    public float damage;
    //public Effects[];
    public abstract void UseSkill(List<Unit> target);

    public void GetTargets()
    {

    }
}
