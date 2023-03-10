using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    SINGLE,
    AOE,

}

public abstract class Skill
{
    public SkillData data;


    public abstract void UseSkill(List<Unit> target);
}
