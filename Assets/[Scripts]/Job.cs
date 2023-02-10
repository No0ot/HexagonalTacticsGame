using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRace", menuName = "Unit/Job")]
public class Job : ScriptableObject
{
    [Header("Information")]
    public string jobName;
    public UnitAttributes mainAttribute;

    [Header("Stat Growths")]
    public float growthStrength;
    public float growthFinesse;
    public float growthConcentration;
    public float growthResolve;

    [Header("Combat Stats")]
    public int initiativeBonus;
    public int movementRange;
    public int dashRange;
    public int attackRange;
    public float baseThreat;
    public float baseMinDamage;
    public float baseMaxDamage;

    public Sprite sprite;


    //abilities
}
