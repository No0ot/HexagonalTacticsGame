using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewJob", menuName = "Unit/Job")]
public class Job : ScriptableObject
{
    [Header("Information")]
    public string jobName;
    public UnitAttributes mainAttribute;

    public Stats baseStats;


    public List<Skill> skills = new List<Skill>();
    
    public Sprite sprite;

    //abilities
}
