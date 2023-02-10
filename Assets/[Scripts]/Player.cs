using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Player
{
    public int number;
    public Color color;

    public Player(int playnum)
    {
        number = playnum;
    }
}
