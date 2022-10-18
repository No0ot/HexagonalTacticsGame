using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string name;
    public Race race;
    public Job job;
    public Color playerColor;

    public SpriteRenderer raceSprite;
    public SpriteRenderer jobSprite;

    int level;
    HexTile tile;

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
}
