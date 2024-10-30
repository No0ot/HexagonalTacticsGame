using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRace", menuName = "Unit/Race")]
public class Race : ScriptableObject
{
    public NameList nameList;

    [Header("Information")]
    public string raceName;

    public Stats stats;

    //[Header("Base Stats")]
    //public float baseStrength;
    //public float baseFinesse;
    //public float baseConcentration;
    //public float baseResolve;
    //
    //[Header("Stat Growths")]
    //public float growthStrength;
    //public float growthFinesse;
    //public float growthConcentration;
    //public float growthResolve;

    public Color color;

    public int bonusMovement;
    public int bonusDash;


    public string GetRandomName()
    {
        string newName = "";

        int randName = Random.Range(0, nameList.names.Length);
        newName = nameList.names[randName];


        return newName;
    }
    //passive abilities
}
