using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGenerator : MonoBehaviour
{
    public Unit prefab;

    public List<Race> races = new List<Race>();
    public List<Job> jobs = new List<Job>();

    public Unit CreateUnit(int level)
    {
        Unit newUnit = Instantiate(prefab);
        int randRace = Random.Range(0, races.Count);
        int randJob = Random.Range(0, jobs.Count);

        newUnit.pawn.race = races[randRace];
        newUnit.pawn.job = jobs[randJob];

        newUnit.name = newUnit.pawn.race.GetRandomName();
        newUnit.pawn.name = newUnit.name;
       // newUnit.pawn.playerNum = player.number;
       // newUnit.pawn.playerColor = player.color;
       // newUnit.pawn.level = level;
       //
       // newUnit.pawn.InitializeUnit();
        return newUnit;
    }
}
