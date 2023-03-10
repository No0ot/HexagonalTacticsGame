using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Unit/Skill")]
public class SkillData : ScriptableObject
{
    public SkillType type;

    public int range;
    public int radius;
    public int cooldown;
    //public float duration;
    public float damage;
    //public Effects[];
}
