using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    STATIC,
    CONTINUOUS
}

[Serializable]
public abstract class Effect
{
    public int duration;
    public EffectType type;

    public abstract void ApplyEffect(UnitObject target);

    public abstract void RemoveEffect(UnitObject unit);

}
