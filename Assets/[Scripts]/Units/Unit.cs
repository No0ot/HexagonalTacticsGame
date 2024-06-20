using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

public class Unit : MonoBehaviour
{
    public string name;
    public Race race;
    public Job job;
    public int playerNum;
    public Color playerColor;

    public SpriteRenderer ringSprite;
    public SpriteRenderer raceSprite;
    public SpriteRenderer jobSprite;

    public int level;
    public HexTile tile;
    public HexTile attackDirection;

    public Stats localStats;
    public Stats statTemplate;

    //[Header("Stats")]
    //public float strength;
    //public float finesse;
    //public float concentration;
    //public float resolve;
    //public float maxHealth;
    //public float currentHealth;
    //public int movementRange;
    //public int dashRange;
    //public float minDamage;
    //public float maxDamage;
    //
    //[Header("Battle Stats")]
    //public int initiative;
    //public float threat;
    HexDirection facing;

    public List<Skill> skills;
    public Skill activeSkill;

    [SerializeReference]
    public List<Effect> effects = new List<Effect>();

    private void Awake()
    {
        ringSprite = GetComponent<SpriteRenderer>();
    }

    public void InitializeUnit()
    {
        raceSprite.color = race.color;
        raceSprite.sprite = job.sprite;
        jobSprite.color = playerColor;
        ringSprite.color = playerColor;

        int rand = Random.Range(1, 7);
        RotateTowards((HexDirection)rand);
        foreach(Skill s in job.skills)
        {
            skills.Add(s);
        }
        SetupStats();
    }

    public void SetupStats()
    {
        if(localStats == null)
            localStats = new Stats(statTemplate);

        var tempStrength = race.stats.GetStat(Stat.STRENGTH).CalculateFinalValue() + (race.stats.GetStat(Stat.STRENGTH_GROWTH).CalculateFinalValue() * level) + (job.baseStats.GetStat(Stat.STRENGTH_GROWTH).CalculateFinalValue() * level);
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

        float attackModifier = 0;
        switch(job.mainAttribute)
        {
            case UnitAttributes.STRENGTH:
                attackModifier = tempStrength;
                break;
            case UnitAttributes.FINESSE:
                attackModifier = tempFinesse;
                break;
            case UnitAttributes.CONCENTRATION:
                attackModifier = tempConcentration;
                break;
            case UnitAttributes.RESOLVE:
                attackModifier = tempResolve;
                break;
        }
        localStats.SetStat(Stat.MIN_DAMAGE, attackModifier);
        var tempMaxDamage = job.baseStats.GetStat(Stat.DAMAGE_VARIANCE).CalculateFinalValue() + attackModifier;
        localStats.SetStat(Stat.MAX_DAMAGE, tempMaxDamage);

        localStats.SetStat(Stat.RANGE, job.baseStats.GetStat(Stat.RANGE).CalculateFinalValue());
    }

    public void RecalculateHealth()
    {
        var previousMaxHealth = localStats.GetStat(Stat.MAX_HEALTH).baseValue;
        var tempMaxHealth = Mathf.Round(localStats.GetStat(Stat.STRENGTH).CalculateFinalValue() * 9.6f) + 50f;

        var difference = tempMaxHealth - previousMaxHealth;

        localStats.SetStat(Stat.MAX_HEALTH, tempMaxHealth);
        localStats.SetStat(Stat.CURRENT_HEALTH, localStats.GetStat(Stat.CURRENT_HEALTH).baseValue + difference);
    }

    public void PlaceUnit(HexTile hex)
    {
        if (tile)
            tile.occupant = null;

        hex.occupant = this;
        tile = hex;
        transform.position = hex.gameObject.transform.position;
    }

    public void RollInitiative()
    {
        var tempInitiative = Random.Range(1, 21) + (int)job.baseStats.GetStat(Stat.INITIATIVE).CalculateFinalValue();
        localStats.SetStat(Stat.INITIATIVE, tempInitiative);
    }

    public void Activate()
    {
        //Debug.Log("activatijng");
        ringSprite.color = Color.yellow;
    }

    public void Threatened()
    {
        ringSprite.color = Color.red;
    }

    public void Deactivate()
    {
        ringSprite.color = playerColor;
    }

    public void RotateTowards(HexDirection direction)
    {
        float angle = 30f;
        switch(direction)
        {
            case HexDirection.RIGHT:
                angle = 270f;
                break;
            case HexDirection.RIGHTDOWN:
                angle = 210f;
                break;
            case HexDirection.LEFTDOWN:
                angle = 150f;
                break;
            case HexDirection.LEFT:
                angle = 90f;
                break;
            case HexDirection.LEFTUP:
                angle = 30f;
                break;
            case HexDirection.RIGHTUP:
                angle = 330f;
                break;
        }
        facing = direction;
        Vector3 zRotation = new Vector3(0f, 0f, angle);
        transform.rotation = Quaternion.Euler(zRotation);
    }

    public void Attack(Unit other, HexTile direction)
    {
        Vector3 attackDirection = direction.transform.position - other.tile.transform.position;
        Debug.DrawLine(other.transform.position, other.transform.position + other.transform.up * 10f, Color.red, 10f);
        Debug.DrawLine(other.tile.transform.position, other.tile.transform.position + attackDirection * 10f, Color.magenta, 10f);
        float angle = Vector3.Angle(other.transform.up, attackDirection);
        int missChance = 30 + (int)other.localStats.GetStat(Stat.FINESSE).CalculateFinalValue() / 2;
        if(angle > 170)
        {
            missChance -= 30;
            Debug.Log(name + " Attacked " + other.name + " from Behind");
        }
        else if(angle > 120)
        {
            missChance -= 15;
            Debug.Log(name + " Attacked " + other.name + " from Slightly Behind");
        }
        else if(angle > 50)
        {
            missChance += 10;
            Debug.Log(name + " Attacked " + other.name + " from Slightly Front");
        }
        else
        {
            missChance += 20;
            Debug.Log(name + " Attacked " + other.name + " from Front");
        }

        
        if (CheckIfHit(missChance))
        {
            float damage = Random.Range(localStats.GetStat(Stat.MIN_DAMAGE).CalculateFinalValue(), localStats.GetStat(Stat.MAX_DAMAGE).CalculateFinalValue());
            damage = Mathf.RoundToInt(damage);
            localStats.EditStat(Stat.THREAT, damage);
            other.TakeDamage(damage);
            //CombatTextGenerator.Instance.NewCombatText(other, damage);
            Debug.Log("And hit! Dealing " + damage);
        }
        else
        {
            //CombatTextGenerator.Instance.NewCombatText(other, 0f);
            CombatTextGenerator.Instance.NewCombatText(other, 0f);
            Debug.Log(" And missed!");
        }

        other.Deactivate();
        other.attackDirection = null;
    }

    public bool CheckIfHit(int missChance)
    {
        int attackroll = Random.Range(1, 101 + (int)localStats.GetStat(Stat.CONCENTRATION).CalculateFinalValue() / 2);
        Debug.Log(name + " has a " + (100 + (int)localStats.GetStat(Stat.CONCENTRATION).CalculateFinalValue() / 2 - missChance) + "% chance to hit)");
        Debug.Log("Rolled a " + attackroll + " against " + missChance);
        if (attackroll > missChance)
            return true;
        else
            return false;
    }

    public void TakeDamage(float damage)
    {
        CombatTextGenerator.Instance.NewCombatText(this, damage);
        localStats.EditStat(Stat.CURRENT_HEALTH, -damage);
        if(localStats.GetStat(Stat.CURRENT_HEALTH).CalculateFinalValue() <= 0)
        {
            localStats.SetStat(Stat.CURRENT_HEALTH, 0);
            Die();
        }
    }

    public void UseAbility(Unit target)
    {

    }

    public void Die()
    {
        BattleManager.Instance.KillUnit(this);
    }

    public void StartTurn()
    {
        foreach (Effect effect in effects)
        {
            if (effect.type == EffectType.CONTINUOUS)
                effect.ApplyEffect(this);
        }
    }

    public void EndTurn()
    {
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
    }
}
