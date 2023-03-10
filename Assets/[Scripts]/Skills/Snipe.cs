using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snipe : Skill
{
    public override void UseSkill(List<Unit> target)
    {
        foreach(Unit u in target)
        {
            u.TakeDamage(data.damage);
        }
    }
}
