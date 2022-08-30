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

    public Unit currentTurnUnit = null;
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

        RoundStart();
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
            if (!hex.occupant)
            {
                selectedUnit.PlaceUnit(hex);
                selectedUnit = null;
            }
        }
    }

    public void Unselect()
    {
        selectedUnit = null;
        selectedTile = null;
    }

    public void RoundStart()
    {
        RollInitiative();

        unitList.Sort(SortByInitiative);
        foreach(Unit unit in unitList)
        {
            turnOrder.Enqueue(unit);
        }

        TurnStart();
    }
    static int SortByInitiative(Unit p1, Unit p2)
    {
        return -p1.initiative.CompareTo(-p2.initiative);
    }

    void RollInitiative()
    {
        foreach(Unit unit in unitList)
        {
            unit.RollInitiative();
        }
    }


    public void TurnStart()
    {
        currentTurnUnit = turnOrder.Dequeue();

        currentTurnUnit.Activate();
        //Pass currenturnUnit into currenturnUnit UI(Bottom left)

        //while(turnOrder.Count > 0)
        //{
        //    Unit temp = turnOrder.Dequeue();
        //    Debug.Log(temp.gameObject.name);
        //}
    }
}
