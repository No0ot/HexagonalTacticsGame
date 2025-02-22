using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTextGenerator : MonoBehaviour
{
    public FloatingCombatText textPrefab;
    public Camera camera;
    public static CombatTextGenerator Instance { get; private set; }

    private void Awake()
    {
        camera = Camera.main;
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    public void NewCombatText(UnitObject unit, float damage, bool crit)
    {
        FloatingCombatText temp =  Instantiate(textPrefab, transform.GetChild(0));
        temp.UpdateText(damage, unit, crit);
    }
}
