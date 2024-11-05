using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TileType
{
    Grass,
    Sand,
    Water,
    Rocky,
    Forest
}

public class HexTile : MonoBehaviour
{
    public Vector2Int AxialCoordinates { get; private set; }
    [SerializeField]
    public float Height { get; private set; }
    public TileType Type { get; private set; }
    public int MovementCost;
    public bool BlocksLOS = false;
    public bool BlocksMovement = false;

    public GameObject HexVisual;
    public GameObject HexObjectsVisual;
    public GameObject UnitPosition;
    public UnitObject Occupant;

    public void SetHexTile(Vector2Int axialCoordinates, float height)
    {
        AxialCoordinates = axialCoordinates;
        Height = height;
        
        HexVisual.transform.localScale = new Vector3(HexVisual.transform.localScale.x, Height + 1f, HexVisual.transform.localScale.z);

        if(HexObjectsVisual)
            HexObjectsVisual.transform.localPosition = new Vector3(transform.GetChild(1).localPosition.x, Height * 0.1f, transform.GetChild(1).localPosition.z);

        if(UnitPosition)
            UnitPosition.transform.localPosition = new Vector3(transform.GetChild(1).localPosition.x, Height * 0.1f + 0.3f, transform.GetChild(1).localPosition.z);
    }

    // Utility functions
    public bool IsWalkable()
    {
        return !BlocksMovement;
    }

    public bool BlocksLineOfSight()
    {
        return BlocksLOS;
    }
}
