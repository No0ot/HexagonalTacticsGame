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
    public Unit user;

    public TargetType type;

    public int range;
    public int radius;
    public int cooldown;
    //public float duration;
    public float damage;
    //public Effects[];
    public abstract void UseSkill(List<Unit> target);

    public void GetTargets(HexTile targetTile, out List<Unit> units)
    {
        units = new List<Unit>();
        List<HexTile> open = new List<HexTile>();
        open.Add(targetTile);

        for (int i = 0; i < radius; i++)
        {
            List<HexTile> open2 = new List<HexTile>();

            for (int j = 0; j < open.Count; j++)
            {

                foreach (HexTile n in open[j].neighbours)
                {
                    if (!open.Contains(n))
                        open2.Add(n);
                }
            }
            open.AddRange(open2);
        }

        foreach (HexTile t in open)
        {
            if (t.occupant)
                units.Add(t.occupant);
        }
    }
}
