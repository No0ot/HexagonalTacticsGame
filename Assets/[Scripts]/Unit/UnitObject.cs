using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class InitComparison : IComparer<Unit>
{
    public int Compare(Unit x, Unit y)
    {
        if (x.localStats.GetStat(Stat.INITIATIVE).CalculateFinalValue() == 0 || y.localStats.GetStat(Stat.INITIATIVE).CalculateFinalValue() == 0)
            return 0;

        return y.localStats.GetStat(Stat.INITIATIVE).CalculateFinalValue().CompareTo(x.localStats.GetStat(Stat.INITIATIVE).CalculateFinalValue());
    }
}

public class ThreatComparison : IComparer<Unit>
{
    public int Compare(Unit x, Unit y)
    {
        return y.localStats.GetStat(Stat.THREAT).CalculateFinalValue().CompareTo(x.localStats.GetStat(Stat.THREAT).CalculateFinalValue());
    }
}


[Serializable]
public class Unit
{
    public string name;
    public Race race;
    public Job job;

    [NonSerialized]
    Player player;

    public int level;

    public Stats localStats;
    public Stats statTemplate;


    public List<Skill> skills;
    public void SetPlayer(Player newplayer)
    {
        player = newplayer;
    }

    public Player GetPlayer()
    {
        return player;
    }

    public void InitializeUnit()
    {
        SetupStats();

    }

    public void SetupStats()
    {
        if (localStats == null)
            localStats = new Stats(statTemplate);

        var tempStrength = race.stats.GetStat(Stat.STRENGTH).CalculateFinalValue() + (
                           race.stats.GetStat(Stat.STRENGTH_GROWTH).CalculateFinalValue() *
        level) + (job.baseStats.GetStat(Stat.STRENGTH_GROWTH).CalculateFinalValue() * level);
        localStats.SetStat(Stat.STRENGTH, tempStrength);
        var tempFinesse = race.stats.GetStat(Stat.FINESSE).CalculateFinalValue() + (race.stats.GetStat(Stat.FINESSE_GROWTH).CalculateFinalValue() * level) + (job.baseStats.GetStat(Stat.FINESSE_GROWTH).CalculateFinalValue() * level);
        localStats.SetStat(Stat.FINESSE, tempFinesse);
        var tempConcentration = race.stats.GetStat(Stat.CONCENTRATION).CalculateFinalValue() + (race.stats.GetStat(Stat.CONCENTRATION_GROWTH).CalculateFinalValue() * level) + (job.baseStats.GetStat(Stat.CONCENTRATION_GROWTH).CalculateFinalValue() * level);
        localStats.SetStat(Stat.CONCENTRATION, tempConcentration);
        var tempResolve = race.stats.GetStat(Stat.RESOLVE).CalculateFinalValue() + (race.stats.GetStat(Stat.RESOLVE_GROWTH).CalculateFinalValue() * level) + (job.baseStats.GetStat(Stat.RESOLVE_GROWTH).CalculateFinalValue() * level);
        localStats.SetStat(Stat.RESOLVE, tempResolve);
        var tempMaxHealth = Mathf.Round(localStats.GetStat(Stat.STRENGTH).CalculateFinalValue() * 9.6f) + 50f;
        localStats.SetStat(Stat.MAX_HEALTH, tempMaxHealth);
        localStats.SetStat(Stat.CURRENT_HEALTH, tempMaxHealth);
        var tempMovementRange = (int)job.baseStats.GetStat(Stat.MOVEMENT_RANGE).CalculateFinalValue() + race.bonusMovement;
        localStats.SetStat(Stat.MOVEMENT_RANGE, tempMovementRange);
        var tempDashRange = (int)job.baseStats.GetStat(Stat.DASH_RANGE).CalculateFinalValue() + race.bonusDash;
        localStats.SetStat(Stat.DASH_RANGE, tempDashRange);
        var tempThreat = job.baseStats.GetStat(Stat.THREAT).CalculateFinalValue();
        localStats.SetStat(Stat.THREAT, tempThreat);
        localStats.SetStat(Stat.MAIN_STAT_MULTIPLIER, job.baseStats.GetStat(Stat.MAIN_STAT_MULTIPLIER).CalculateFinalValue());

        float attackModifier = 0;
        switch (job.mainAttribute)
        {
            case Stat.STRENGTH:
                attackModifier = tempStrength;
                break;
            case Stat.FINESSE:
                attackModifier = tempFinesse;
                break;
            case Stat.CONCENTRATION:
                attackModifier = tempConcentration;
                break;
            case Stat.RESOLVE:
                attackModifier = tempResolve;
                break;
            default:
                attackModifier = tempStrength;
                break;
        }
        attackModifier *= localStats.GetStat(Stat.MAIN_STAT_MULTIPLIER).CalculateFinalValue();

        localStats.SetStat(Stat.MIN_DAMAGE, attackModifier);
        var tempMaxDamage = job.baseStats.GetStat(Stat.DAMAGE_VARIANCE).CalculateFinalValue() + attackModifier;
        localStats.SetStat(Stat.MAX_DAMAGE, tempMaxDamage);
        localStats.SetStat(Stat.RANGE, job.baseStats.GetStat(Stat.RANGE).CalculateFinalValue());
        localStats.SetStat(Stat.RADIUS, job.baseStats.GetStat(Stat.RADIUS).CalculateFinalValue());
        localStats.SetStat(Stat.ARMOR, job.baseStats.GetStat(Stat.ARMOR).CalculateFinalValue());
        localStats.SetStat(Stat.DAMAGE_MULTIPLIER, 1.0f);
        localStats.SetStat(Stat.ACCURACY, 0.0f);
        localStats.SetStat(Stat.NUM_OF_ATTACKS, 1.0f);
        localStats.SetStat(Stat.CRIT_MULTIPLIER, job.baseStats.GetStat(Stat.ARMOR).CalculateFinalValue());
    }

    public void RecalculateHealth()
    {
        var previousMaxHealth = localStats.GetStat(Stat.MAX_HEALTH).baseValue;
        var tempMaxHealth = Mathf.Round(localStats.GetStat(Stat.STRENGTH).CalculateFinalValue() * 9.6f) + 50f;

        var difference = tempMaxHealth - previousMaxHealth;

        localStats.SetStat(Stat.MAX_HEALTH, tempMaxHealth);
        localStats.SetStat(Stat.CURRENT_HEALTH, localStats.GetStat(Stat.CURRENT_HEALTH).baseValue + difference);
    }

    public void RollInitiative()
    {
        var tempInitiative = UnityEngine.Random.Range(1, 21) + (int)job.baseStats.GetStat(Stat.INITIATIVE).CalculateFinalValue();
        localStats.SetStat(Stat.INITIATIVE, tempInitiative);
    }
}

[Serializable]
public class UnitObject : MonoBehaviour
{
    public Unit unitInfo;


    public MeshRenderer head;
    public MeshRenderer body;
    public MeshRenderer facingIndicator;

    public HexTile tile;
    public HexTile attackDirection;

    public List<Skill> skills;
    public List<int> skillCooldowns;
    public Skill activeSkill;

    public bool hasMoved = false;
    public bool hasDashed = false;
    public bool hasAttacked = false;

    public int actionPoint = 2;

    //Below stuff should be moved to UNIT

    [SerializeReference]
    public List<Effect> effects = new List<Effect>();

    [SerializeReference]
    public List<Effect> attackAppliedEffects = new List<Effect>();

    public Skill specialAttackSkill;

    public bool attackCrit = false;

    private void Awake()
    {

    }

    public void InitializeUnitObject()
    {
        head.material.color = unitInfo.race.color;
        body.material.color = unitInfo.GetPlayer().color;

        int rand = UnityEngine.Random.Range(1, 7);
        //RotateTowards((HexDirection)rand);
        foreach(Skill s in unitInfo.skills)
        {
            skills.Add(s);
            
        }
        skillCooldowns.Add(0);
        skillCooldowns.Add(0);
        skillCooldowns.Add(0);
    }

    public void SetActiveSkill(int skillnum)
    {
        if(skillnum == -1)
        {
            activeSkill = null;
            BattleManager.Instance.activeSkillNum = -1;
            return;
        }
        BattleManager.Instance.activeSkillNum = skillnum;
        Skill skill = skills[skillnum];
        activeSkill = skill;
        skillCooldowns[skillnum] = activeSkill.cooldown;
        if (activeSkill as SpecialAttack)
        {
            Debug.Log("Set Active skill as Special Attack");
            specialAttackSkill = skill as SpecialAttack;
            specialAttackSkill.user = this;
        }
    }

    public void PlaceUnit(HexTile hex)
    {
        if (tile)
            tile.Occupant = null;
        
        hex.Occupant = this;
        tile = hex;
        transform.position = hex.UnitPosition.transform.position;
    }

    

    public void Activate()
    {
        //Debug.Log("activatijng");
        //ringSprite.color = Color.yellow;
    }

    public void Threatened()
    {
        //ringSprite.color = Color.red;
    }

    public void Deactivate()
    {
        //ringSprite.color = playerColor;
    }

    public void RotateTowards(HexTile direction)
    {
        Vector3 noYDirectionPosition = direction.transform.position;
        noYDirectionPosition.y = 0;

        Vector3 noYPosition = transform.position;
        noYPosition.y = 0;

        var lookPos = noYDirectionPosition - noYPosition;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;
    }


    public void Attack(UnitObject other)
    {
        if (specialAttackSkill != null)
        {
           specialAttackSkill.UseSkill(other.tile, BattleManager.Instance.GetHexTiles());
           Debug.Log("Special Skill used");
            
            specialAttackSkill = null;
        }
        else
        {
            for (int i = 0; i < unitInfo.localStats.GetStat(Stat.NUM_OF_ATTACKS).CalculateFinalValue(); i++)
            {
                if (CheckIfHit(other))
                {
                    if (!attackCrit)
                    {
                        float damage = UnityEngine.Random.Range(unitInfo.localStats.GetStat(Stat.MIN_DAMAGE).CalculateFinalValue(), unitInfo.localStats.GetStat(Stat.MAX_DAMAGE).CalculateFinalValue());
                        damage *= unitInfo.localStats.GetStat(Stat.DAMAGE_MULTIPLIER).CalculateFinalValue();
                        damage = Mathf.RoundToInt(damage);
                        unitInfo.localStats.EditStat(Stat.THREAT, damage);
                        other.TakeDamage(damage);
                        CombatTextGenerator.Instance.NewCombatText(other, damage, false);

                        Debug.Log("And hit! Dealing " + damage);
                    }
                    else
                    {
                        float damage = UnityEngine.Random.Range(unitInfo.localStats.GetStat(Stat.MIN_DAMAGE).CalculateFinalValue(), unitInfo.localStats.GetStat(Stat.MAX_DAMAGE).CalculateFinalValue());
                        damage *= unitInfo.localStats.GetStat(Stat.DAMAGE_MULTIPLIER).CalculateFinalValue();
                        damage *= unitInfo.localStats.GetStat(Stat.CRIT_MULTIPLIER).CalculateFinalValue();
                        damage = Mathf.RoundToInt(damage);
                        unitInfo.localStats.EditStat(Stat.THREAT, damage);
                        other.TakeDamage(damage);
                        CombatTextGenerator.Instance.NewCombatText(other, damage, false);
                        attackCrit = false;

                        Debug.Log("And CRIT! Dealing " + damage);
                    }
                }
                else
                {
                    CombatTextGenerator.Instance.NewCombatText(other, 0f,false);
                    //CombatTextGenerator.Instance.NewCombatText(other, 0f);
                    Debug.Log(" And missed!");
                }

                //attackAppliedEffects.Clear();
            }
        }
        other.Deactivate();
        other.attackDirection = null;
    }

    public bool CheckIfHit(UnitObject other)
    {
        Vector3 noYpos = tile.transform.position;
        noYpos.y = 0;
        Vector3 otherNoYpos = other.transform.position;
        otherNoYpos.y = 0;

        Vector3 attackDirection = noYpos - otherNoYpos;
        attackDirection.Normalize();

        float angle = Vector3.Angle(attackDirection, other.transform.forward);
        Debug.Log(angle);
        float chanceToHit = 0;// + (int)other.unitInfo.localStats.GetStat(Stat.FINESSE).CalculateFinalValue() / unitInfo.level;
        if (angle > 170)
        {
            chanceToHit = 70;
            Debug.Log(unitInfo.name + " Attacked " + other.unitInfo.name + " from Behind");
        }
        else if (angle > 120)
        {
            chanceToHit = 60;
            Debug.Log(unitInfo.name + " Attacked " + other.unitInfo.name + " from Slightly Behind");
        }
        else if (angle > 50)
        {
            chanceToHit = 40;
            Debug.Log(unitInfo.name + " Attacked " + other.unitInfo.name + " from Slightly Front");
        }
        else
        {
            chanceToHit = 30;
            Debug.Log(unitInfo.name + " Attacked " + other.unitInfo.name + " from Front");
        }
        chanceToHit -= (int)other.unitInfo.localStats.GetStat(Stat.FINESSE).CalculateFinalValue() / unitInfo.level;

        int attackRollMax = 101 + (int)unitInfo.localStats.GetStat(Stat.CONCENTRATION).CalculateFinalValue() / unitInfo.level;
        int attackroll = UnityEngine.Random.Range(1, attackRollMax);

        Debug.Log(name + " has a " + (attackRollMax - (100 - chanceToHit)) + "% chance to hit)");

        Debug.Log("Rolled a " + attackroll + " against" + (100 - chanceToHit));

        if (attackroll > 100 - chanceToHit)
        {
            //check if crit.
            if (attackroll > 100)
                attackCrit = true;
            return true;
        }
        else
            return false;
    }

    public void TakeDamage(float damage)
    {
        float newDamage = Mathf.Clamp(damage - unitInfo.localStats.GetStat(Stat.ARMOR).CalculateFinalValue(),0f, 9999f);
        //CombatTextGenerator.Instance.NewCombatText(this, damage);
        unitInfo.localStats.EditStat(Stat.CURRENT_HEALTH, -newDamage);
        if(unitInfo.localStats.GetStat(Stat.CURRENT_HEALTH).CalculateFinalValue() <= 0)
        {
            unitInfo.localStats.SetStat(Stat.CURRENT_HEALTH, 0);
            Die();
        }
    }

    //public void UseActiveSkill(HexTile hex, List<UnitObject> targetedUnits)
    //{
    //    activeSkill.user = this;
    //    //activeSkill.GetTargets(hex, out targetedUnits);
    //    activeSkill.UseSkill(targetedUnits);
    //}

    public void GetAttackThreatenedHexes()
    {

    }

    public void Die()
    {
        //BattleManager.Instance.KillUnit(this);
    }

    public void StartTurn()
    {
        foreach (Effect effect in effects)
        {
            if (effect.type == EffectType.CONTINUOUS)
                effect.ApplyEffect(this);
        }

        hasMoved = false;
        hasDashed = false;
        hasAttacked = false;

        actionPoint = 2;
    }

    public void EndTurn()
    {
        ResolveHealing();

        List<Effect> effectsToRemove = new List<Effect>();
        foreach(Effect effect in effects)
        {
            effect.duration -= 1;

            if(effect.duration <= 0)
            {
                effectsToRemove.Add(effect);
            }
        }

        foreach(Effect effect in effectsToRemove)
        {
            effect.RemoveEffect(this);
        }

        attackAppliedEffects.Clear();

        for(int i = 0; i < skillCooldowns.Count; i++)
        {
            if(skillCooldowns[i] > 0)
                skillCooldowns[i] -= 1;
        }
    }

    public void UseActionPoint()
    {
        actionPoint -= 1;

        if(actionPoint <= 0)
        {
            if(hasDashed)
                UIManager.Instance.DisableAction(0);

            UIManager.Instance.DisableAction(1);
            UIManager.Instance.DisableAction(2);
        }
    }

    public void ResolveHealing()
    {
        float healing = -unitInfo.localStats.GetStat(Stat.RESOLVE).CalculateFinalValue();

        TakeDamage(healing);
        CombatTextGenerator.Instance.NewCombatText(this, healing, false);
    }
}
