using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Player
{
    public int number;
    public Color color;


    public List<UnitScriptable> unitScriptables;
    public List<Unit> units { get; private set; }

    public Player(int playnum, Color playcolor, List<Unit> playunits)
    {
        number = playnum;
        color = playcolor;
        units = playunits;
    }

    public Player(int playnum, Color playcolor, List<UnitScriptable> playunits)
    {
        number = playnum;
        color = playcolor;
        foreach(UnitScriptable script in playunits)
        {
            units.Add(script.unit);
        }
    }

    public void InitializePlayer()
    {
        units.Clear();
        foreach (UnitScriptable script in unitScriptables)
        {
            units.Add(script.unit);
        }
    }
}
