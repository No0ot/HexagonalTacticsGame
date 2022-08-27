using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Race race;
    public Job job;
    public Color playerColor;

    SpriteRenderer raceSprite;
    public SpriteRenderer jobSprite;

    int level;
    HexTile tile;

    [Header("Stats")]
    public float strength;
    public float finesse;
    public float concentration;
    public float resolve;

    private void Awake()
    {
        raceSprite = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupStats();
        raceSprite.color = race.color;
        jobSprite.sprite = job.sprite;
        jobSprite.color = playerColor;
    }

    void SetupStats()
    {
        strength = race.baseStrength + (race.growthStrength * level) + (job.growthStrength * level);
        finesse = race.baseFinesse + (race.growthFinesse * level) + (job.growthFinesse * level);
        concentration = race.baseConcentration + (race.growthConcentration * level) + (job.growthConcentration * level);
        resolve = race.baseResolve + (race.growthResolve * level) + (job.growthStrength * level);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaceUnit(HexTile hex)
    {
        if (tile)
            tile.occupant = null;

        hex.occupant = this;
        tile = hex;
        transform.position = hex.gameObject.transform.position;
    }
}
