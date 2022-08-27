using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }
    public delegate void SelectHexDelegate(HexTile hex);
    public SelectHexDelegate selectHex;

    public List<Unit> unitList = new List<Unit>();
    Queue<Unit> turnOrder = new Queue<Unit>();
    HexGrid grid;

    public HexTile selectedTile { get; set; }
    public Unit selectedUnit{ get; set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        selectHex += SelectHex;
        grid = FindObjectOfType<HexGrid>();
    }
    // Start is called before the first frame update
    void Start()
    {
        grid.BuildGrid();
        foreach(Unit unit in unitList)
        {
            int temp = Random.Range(0, grid.hexList.Count);
            while (grid.hexList[temp].occupant)
            {
                temp = Random.Range(0, grid.hexList.Count);
            }
            unit.PlaceUnit(grid.hexList[temp]);
        }

    }

    void SelectHex(HexTile hex)
    {
        if (!selectedUnit)
        {
            selectedTile = hex;
            if (selectedTile.occupant)
            {
                selectedUnit = selectedTile.occupant;
            }
        }
        else
        {
            selectedUnit.PlaceUnit(hex);
            selectedUnit = null;
        }
    }

    public void Unselect()
    {
        selectedUnit = null;
        selectedTile = null;
    }
}
