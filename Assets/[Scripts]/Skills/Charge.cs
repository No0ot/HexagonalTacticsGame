using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Charge", menuName = "Unit/Skill/Charge")]
public class Charge : Skill
{
    public override void UseSkill(List<UnitObject> targets)
    {
       // Unit target = null;
       // if (targets.Count == 1)
       //     target = targets[0];
       // else
       //     return;

        //List<HexTile> direction = BattleManager.Instance.grid.HexLineDraw(user.tile, target.tile);
        //HexTile chargeTile = null;
        //
        //foreach (HexTile lineTile in direction)
        //{
        //    for(int i = 0; i < target.tile.neighbours.Count; i++)
        //    {
        //        HexTile neighbour = target.tile.neighbours[i];
        //
        //        if (lineTile == neighbour)
        //        {
        //            chargeTile = neighbour;
        //            while(chargeTile.occupant)
        //            {
        //                i++;
        //                if(i > target.tile.neighbours.Count)
        //                {
        //                    Debug.Log("No tile for Charge");
        //                    return;
        //                }
        //                chargeTile = target.tile.neighbours[i];
        //
        //            }
        //
        //        }
        //    }
        //}

        //if(chargeTile != null)
        //{
        //    user.PlaceUnit(chargeTile);
        //    target.TakeDamage(damage);
        //}
    }
}
