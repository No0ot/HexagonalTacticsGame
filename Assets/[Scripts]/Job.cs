using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRace", menuName = "Unit/Job")]
public class Job : ScriptableObject
{
    [Header("Information")]
    public string jobName;
    UnitAttributes mainAttribute;

    [Header("Stat Growths")]
    public float growthStrength;
    public float growthFinesse;
    public float growthConcentration;
    public float growthResolve;

    public Sprite sprite;


    //abilities
}
