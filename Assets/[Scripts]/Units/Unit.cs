using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


public class InitComparison : IComparer<Unit>
{
    public int Compare(Unit x, Unit y)
    {
        if (x.pawn.initiative == 0 || y.pawn.initiative == 0)
            return 0;

        return y.pawn.initiative.CompareTo(x.pawn.initiative);
    }
}

public class ThreatComparison : IComparer<Unit>
{
    public int Compare(Unit x, Unit y)
    {
        return y.pawn.threat.CompareTo(x.pawn.threat);
    }
}

public class Unit : MonoBehaviour
{
    public Pawn pawn;
    public bool isFighting = false;

    public SpriteRenderer raceSprite;
    public SpriteRenderer jobSprite;
    public void Initialize()
    {
        raceSprite.color = pawn.race.color;
        jobSprite.sprite = pawn.job.sprite;
        //jobSprite.color = playerColor;

        pawn.InitializePawn();
    }

    public void TakeDamage(float damage)
    {
        CombatTextGenerator.Instance.NewCombatText(this, damage);
        pawn.TakeDamage(damage);

        if (pawn.stats.currentHealth <= 0)
        {
            pawn.stats.currentHealth = 0;
            Invoke("Die", 1f);
        }
    }

    private void Update()
    {
        if (!isFighting || pawn.target == null)
            return;

        if(pawn.target.pawn.stats.currentHealth <= 0 || pawn.target == null)
        {
            return;
            //get new target
        }

        Vector3 direction = new Vector3(transform.position.x - pawn.target.transform.position.x,
                                            transform.position.y - pawn.target.transform.position.y,
                                            0.0f);
        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
        }

        pawn.attackTimer += Time.deltaTime;
        if (pawn.attackTimer >= pawn.attackSpeed)
        {
            pawn.Attack();
        }
    }

    public void Die()
    {
        Debug.Log(pawn.name + " has died!");
        Destroy(this.gameObject);
    }
    #region Old Hex Unit
    //
    //    public string name;
    //    public Race race;
    //    public Job job;
    //    public int playerNum;
    //    public Color playerColor;
    //
    //    public SpriteRenderer raceSprite;
    //    public SpriteRenderer jobSprite;
    //
    //    public int level;
    //    public HexTile tile;
    //    public HexTile attackDirection;
    //
    //    [Header("Stats")]
    //    public float strength;
    //    public float finesse;
    //    public float concentration;
    //    public float resolve;
    //    public float maxHealth;
    //    public float currentHealth;
    //    public int movementRange;
    //    public int dashRange;
    //    public float minDamage;
    //    public float maxDamage;
    //
    //    [Header("Battle Stats")]
    //    public int initiative;
    //    public float threat;
    //    HexDirection facing;
    //    public List<Skill> skills;
    //    public Skill activeSkill;
    //
    //    private void Awake()
    //    {
    //        raceSprite = GetComponent<SpriteRenderer>();
    //    }
    //
    //    public void InitializeUnit()
    //    {
    //        raceSprite.color = playerColor;
    //        jobSprite.sprite = job.sprite;
    //        jobSprite.color = playerColor;
    //
    //        int rand = Random.Range(1, 7);
    //        RotateTowards((HexDirection)rand);
    //        foreach(Skill s in job.skills)
    //        {
    //            skills.Add(s);
    //        }
    //        SetupStats();
    //    }
    //
    //    void SetupStats()
    //    {
    //        strength = race.baseStrength + (race.growthStrength * level) + (job.growthStrength * level);
    //        finesse = race.baseFinesse + (race.growthFinesse * level) + (job.growthFinesse * level);
    //        concentration = race.baseConcentration + (race.growthConcentration * level) + (job.growthConcentration * level);
    //        resolve = race.baseResolve + (race.growthResolve * level) + (job.growthStrength * level);
    //
    //        currentHealth = maxHealth = Mathf.Round(strength * 9.6f) + 50f;
    //
    //        movementRange = job.movementRange + race.bonusMovement;
    //        dashRange = job.dashRange + race.bonusDash;
    //        threat += job.baseThreat;
    //
    //        float attackModifier = 0;
    //        switch(job.mainAttribute)
    //        {
    //            case UnitAttributes.STRENGTH:
    //                attackModifier = strength;
    //                break;
    //            case UnitAttributes.FINESSE:
    //                attackModifier = finesse;
    //                break;
    //            case UnitAttributes.CONCENTRATION:
    //                attackModifier = concentration;
    //                break;
    //            case UnitAttributes.RESOLVE:
    //                attackModifier = resolve;
    //                break;
    //        }
    //        minDamage = attackModifier;
    //        maxDamage = job.damageVariance + attackModifier;
    //
    //    }
    //
    //    public void PlaceUnit(HexTile hex)
    //    {
    //        if (tile)
    //            tile.occupant = null;
    //
    //        hex.occupant = this;
    //        tile = hex;
    //        transform.position = hex.gameObject.transform.position;
    //    }
    //
    //    public void RollInitiative()
    //    {
    //        initiative = Random.Range(1, 21) + job.initiativeBonus;
    //    }
    //
    //    public void Activate()
    //    {
    //        //Debug.Log("activatijng");
    //        raceSprite.color = Color.yellow;
    //    }
    //
    //    public void Threatened()
    //    {
    //        raceSprite.color = Color.red;
    //    }
    //
    //    public void Deactivate()
    //    {
    //        raceSprite.color = playerColor;
    //    }
    //
    //    public void RotateTowards(HexDirection direction)
    //    {
    //        float angle = 30f;
    //        switch(direction)
    //        {
    //            case HexDirection.RIGHT:
    //                angle = 270f;
    //                break;
    //            case HexDirection.RIGHTDOWN:
    //                angle = 210f;
    //                break;
    //            case HexDirection.LEFTDOWN:
    //                angle = 150f;
    //                break;
    //            case HexDirection.LEFT:
    //                angle = 90f;
    //                break;
    //            case HexDirection.LEFTUP:
    //                angle = 30f;
    //                break;
    //            case HexDirection.RIGHTUP:
    //                angle = 330f;
    //                break;
    //        }
    //        facing = direction;
    //        Vector3 zRotation = new Vector3(0f, 0f, angle);
    //        transform.rotation = Quaternion.Euler(zRotation);
    //    }
    //
    //    public void Attack(Unit other, HexTile direction)
    //    {
    //        Vector3 attackDirection = direction.transform.position - other.tile.transform.position;
    //        Debug.DrawLine(other.transform.position, other.transform.position + other.transform.up * 10f, Color.red, 10f);
    //        Debug.DrawLine(other.tile.transform.position, other.tile.transform.position + attackDirection * 10f, Color.magenta, 10f);
    //        float angle = Vector3.Angle(other.transform.up, attackDirection);
    //        int missChance = 30 + (int)other.finesse / 2;
    //        if(angle > 170)
    //        {
    //            missChance -= 30;
    //            Debug.Log(name + " Attacked " + other.name + " from Behind");
    //        }
    //        else if(angle > 120)
    //        {
    //            missChance -= 15;
    //            Debug.Log(name + " Attacked " + other.name + " from Slightly Behind");
    //        }
    //        else if(angle > 50)
    //        {
    //            missChance += 10;
    //            Debug.Log(name + " Attacked " + other.name + " from Slightly Front");
    //        }
    //        else
    //        {
    //            missChance += 20;
    //            Debug.Log(name + " Attacked " + other.name + " from Front");
    //        }
    //
    //        
    //        if (CheckIfHit(missChance))
    //        {
    //            float damage = Random.Range(minDamage, maxDamage);
    //            damage = Mathf.RoundToInt(damage);
    //            threat += damage;
    //            other.TakeDamage(damage);
    //            //CombatTextGenerator.Instance.NewCombatText(other, damage);
    //            Debug.Log("And hit! Dealing " + damage);
    //        }
    //        else
    //        {
    //            //CombatTextGenerator.Instance.NewCombatText(other, 0f);
    //            Debug.Log(" And missed!");
    //        }
    //
    //        other.Deactivate();
    //        other.attackDirection = null;
    //    }
    //
    //    public bool CheckIfHit(int missChance)
    //    {
    //        int attackroll = Random.Range(1, 101 + (int)concentration / 2);
    //        Debug.Log(name + " has a " + (100 + (int)concentration / 2 - missChance) + "% chance to hit)");
    //        Debug.Log("Rolled a " + attackroll + " against " + missChance);
    //        if (attackroll > missChance)
    //            return true;
    //        else
    //            return false;
    //    }
    //
    //    public void TakeDamage(float damage)
    //    {
    //        CombatTextGenerator.Instance.NewCombatText(this, damage);
    //        currentHealth -= damage;
    //        if(currentHealth <= 0)
    //        {
    //            currentHealth = 0;
    //            Die();
    //        }
    //    }
    //
    //    public void UseAbility(Unit target)
    //    {
    //        if(activeSkill)
    //        {
    //           
    //        }
    //    }
    //
    //    public void Die()
    //    {
    //        BattleManager.Instance.KillUnit(this);
    //    }
    #endregion
}
