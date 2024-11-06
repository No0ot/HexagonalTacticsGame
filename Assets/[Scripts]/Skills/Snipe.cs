using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Snipe", menuName = "Unit/Skill/Snipe")]
public class Snipe : Skill
{
    public override void UseSkill(List<UnitObject> target)
    {
        foreach(UnitObject u in target)
        {
            u.TakeDamage(damage);
        }
    }
}
