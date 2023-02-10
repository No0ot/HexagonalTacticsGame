using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGenerator : MonoBehaviour
{
    public Unit prefab;

    public List<Race> races = new List<Race>();
    public List<Job> jobs = new List<Job>();

    public Unit CreateUnit(Player player, int level)
    {
        Unit newUnit = Instantiate(prefab);
        int randRace = Random.Range(0, races.Count);
        int randJob = Random.Range(0, jobs.Count);

        newUnit.race = races[randRace];
        newUnit.job = jobs[randJob];

        newUnit.name = newUnit.race.GetRandomName();
        newUnit.playerNum = player.number;
        newUnit.playerColor = player.color;
        newUnit.level = level;

        newUnit.InitializeUnit();
        return newUnit;
    }
}
