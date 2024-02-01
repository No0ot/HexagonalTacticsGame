using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

[Serializable]
public struct PawnStats
{
    [Header("Stats")]
    public float strength;
    public float finesse;
    public float concentration;
    public float resolve;
    public float maxHealth;
    public float currentHealth;
    public float minDamage;
    public float maxDamage;
}

[Serializable]
public class Pawn
{
    public string name;
    public Race race;
    public Job job;
    public int level;

    public PawnStats stats;

    [Header("Battle Stats")]
    public int initiative;
    public float threat;
    public Unit target;
    public float attackSpeed = 2.0f;
    public float attackTimer;

    public void InitializePawn()
    {
        SetupStats();
    }

    void SetupStats()
    {
        stats.strength = race.baseStrength + (race.growthStrength * level) + (job.growthStrength * level);
        stats.finesse = race.baseFinesse + (race.growthFinesse * level) + (job.growthFinesse * level);
        stats.concentration = race.baseConcentration + (race.growthConcentration * level) + (job.growthConcentration * level);
        stats.resolve = race.baseResolve + (race.growthResolve * level) + (job.growthStrength * level);

        stats.currentHealth = stats.maxHealth = Mathf.Round(stats.strength * 9.6f) + 50f;
        threat += job.baseThreat;

        float attackModifier = 0;
        switch (job.mainAttribute)
        {
            case UnitAttributes.STRENGTH:
                attackModifier = stats.strength;
                break;
            case UnitAttributes.FINESSE:
                attackModifier = stats.finesse;
                break;
            case UnitAttributes.CONCENTRATION:
                attackModifier = stats.concentration;
                break;
            case UnitAttributes.RESOLVE:
                attackModifier = stats.resolve;
                break;
        }
        stats.minDamage = attackModifier;
        stats.maxDamage = job.damageVariance + attackModifier;

    }

    public Unit GetTarget(List<Unit> enemyUnits)
    {
        if(enemyUnits.Count <= 0)
        {
            Debug.Log("Can't get target. No units in list.");
            return null;
        }
        List<Unit> enemies = new List<Unit>();
        enemies = enemyUnits;


        enemies.Sort(new ThreatComparison());
        Unit newTarget = enemies[0];
        target = newTarget;

        return newTarget;
    }

    public void Attack()
    {
        float damage = UnityEngine.Random.Range(stats.minDamage, stats.maxDamage);
        damage = Mathf.RoundToInt(damage);
        threat += damage;
        target.TakeDamage(damage);
        //CombatTextGenerator.Instance.NewCombatText(other, damage);
        Debug.Log("And hit! Dealing " + damage);
        attackTimer = 0f;
    }

    public void TakeDamage(float damage)
    {
        stats.currentHealth -= damage;
        
    }
}
