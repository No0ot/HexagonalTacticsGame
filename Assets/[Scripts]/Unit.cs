using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InitComparison : IComparer<Unit>
{
    public int Compare(Unit x, Unit y)
    {
        if (x.initiative == 0 || y.initiative == 0)
            return 0;

        return y.initiative.CompareTo(x.initiative);
    }
}

public class Unit : MonoBehaviour
{
    public string name;
    public Race race;
    public Job job;
    public Color playerColor;

    public SpriteRenderer raceSprite;
    public SpriteRenderer jobSprite;

    int level;
    public HexTile tile;

    [Header("Stats")]
    public float strength;
    public float finesse;
    public float concentration;
    public float resolve;

    [Header("Battle Stats")]
    public int initiative;
    public float maxHealth;
    public float currentHealth;

    private void Awake()
    {
        raceSprite = GetComponent<SpriteRenderer>();
        SetupStats();
        raceSprite.color = playerColor;
        jobSprite.sprite = job.sprite;
        jobSprite.color = playerColor;

        int rand = Random.Range(1, 7);
        Vector3 zRotation = new Vector3(0f, 0f,30f + rand * 60f);
        transform.rotation = Quaternion.Euler(zRotation);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void SetupStats()
    {
        strength = race.baseStrength + (race.growthStrength * level) + (job.growthStrength * level);
        finesse = race.baseFinesse + (race.growthFinesse * level) + (job.growthFinesse * level);
        concentration = race.baseConcentration + (race.growthConcentration * level) + (job.growthConcentration * level);
        resolve = race.baseResolve + (race.growthResolve * level) + (job.growthStrength * level);

        currentHealth = maxHealth = Mathf.Round(strength * 9.6f);
        
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
        initiative = Random.Range(1, 21) + job.initiativeBonus;
    }

    public void Activate()
    {
        Debug.Log("activatijng");
        raceSprite.color = Color.yellow;
    }

    public void Deactivate()
    {
        raceSprite.color = playerColor;
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
        Vector3 zRotation = new Vector3(0f, 0f, angle);
        transform.rotation = Quaternion.Euler(zRotation);
    }
}
