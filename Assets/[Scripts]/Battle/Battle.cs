using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Battle
{
    List<Unit> teamOne;
    List<Unit> teamTwo;

    List<Unit> unitList = new List<Unit>();

    public Battle(List<Unit> teamOneUnits, List<Unit> teamTwoUnits)
    {
        teamOne = teamOneUnits;
        teamTwo = teamTwoUnits;
    }

    public void StartBattle()
    {
        foreach (Unit unit in teamOne)
        {
            unit.Initialize();
            unitList.Add(unit);
        }

        foreach (Unit unit in teamTwo)
        {
            unit.Initialize();
            unit.pawn.GetTarget(teamOne);
            unitList.Add(unit);
        }

        foreach (Unit unit in teamOne)
        {
            unit.pawn.GetTarget(teamTwo);
        }

        foreach(Unit unit in unitList)
        {
            unit.isFighting = true;
            BattleManager.Instance.PlaceUnitInBattleLocation(unit, Random.Range(0,8));
        }

    }
}