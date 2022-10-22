using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnPhase
{
    NONE,
    MOVE,
    ATTACK,
    SKILL,
    FACE
}

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

    TurnPhase phase;

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
        switch (phase)
        {
            case TurnPhase.MOVE:
                if (!selectedUnit)
                {
                    selectedTile = hex;
                    if (selectedTile.occupant)
                    {
                        selectedUnit = selectedTile.occupant;
                        UIManager.Instance.selectedUnitProfile.UpdateProfile(selectedUnit);
                    }
                }
                else
                {
                    if (!hex.occupant)
                    {
                        //move is done here for now but later the hex will need to be passed to an object that will determine whether a move or attack or rotation needs to be done.
                        currentTurnUnit.PlaceUnit(hex);
                        selectedUnit = null;
                        UIManager.Instance.selectedUnitProfile.UpdateProfile(null);
                    }
                }
                break;
            case TurnPhase.ATTACK:
                break;
            case TurnPhase.SKILL:
                break;
            case TurnPhase.FACE:

                for(int i = 0; i < currentTurnUnit.tile.neighbours.Count; i++)
                {
                    if (currentTurnUnit.tile.neighbours[i])
                    {
                        if(currentTurnUnit.tile.neighbours[i] == hex)
                        {
                            currentTurnUnit.RotateTowards((HexDirection)i);
                            EndTurn();
                        }
                    }
                }

                break;
            default:
                break;
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

        InitComparison comp = new InitComparison();
        unitList.Sort(comp);
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
        phase = TurnPhase.NONE;
        currentTurnUnit = turnOrder.Dequeue();

        currentTurnUnit.Activate();

        //Pass currenturnUnit into currenturnUnit UI(Bottom left)
        UIManager.Instance.currentUnitProfile.UpdateProfile(currentTurnUnit);

        UIManager.Instance.actionBar.SetActive(true);

        //while(turnOrder.Count > 0)
        //{
        //    Unit temp = turnOrder.Dequeue();
        //    Debug.Log(temp.gameObject.name);
        //}
    }

    public void HighlightTiles()
    {
        List<HexTile> highlightedTiles = new List<HexTile>();

        switch(phase)
        {
            case TurnPhase.MOVE:
                break;
            case TurnPhase.ATTACK:
                break;
            case TurnPhase.SKILL:
                break;
            case TurnPhase.FACE:
                highlightedTiles = currentTurnUnit.tile.neighbours;
                foreach(HexTile tile in highlightedTiles)
                {
                    if(tile)
                        tile.ActivateHighlight(HighlightColor.FACE);
                }

                break;
            default:
                break;
        }
    }

    public void FaceUnit()
    {
        phase = TurnPhase.FACE;
        HighlightTiles();
    }

    public void EndTurn()
    {
        currentTurnUnit.Deactivate();
        if (turnOrder.Count > 0)
            TurnStart();
        else
            RoundStart();
    }
}
