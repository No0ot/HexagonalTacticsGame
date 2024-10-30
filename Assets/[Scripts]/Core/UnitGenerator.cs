using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGenerator : MonoBehaviour
{
    public UnitObject prefab;

    public List<Race> races = new List<Race>();
    public List<Job> jobs = new List<Job>();

    public UnitObject CreateRandomUnit(Player player, int level)
    {
        UnitObject newUnit = Instantiate(prefab);
        int randRace = Random.Range(0, races.Count);
        int randJob = Random.Range(0, jobs.Count);

        newUnit.unitInfo.race = races[randRace];
        newUnit.unitInfo.job = jobs[randJob];

        newUnit.name = newUnit.unitInfo.race.GetRandomName();
        newUnit.unitInfo.GetPlayer().number = player.number;
        newUnit.unitInfo.GetPlayer().color = player.color;
        newUnit.unitInfo.level = level;

        newUnit.InitializeUnitObject();
        return newUnit;
    }

    public UnitObject CreateUnitObject(Unit unit)
    {
        UnitObject newUnit = Instantiate(prefab);
        newUnit.unitInfo = unit;
        newUnit.InitializeUnitObject();

        newUnit.gameObject.transform.SetParent(transform);
        return newUnit;
    }
}
