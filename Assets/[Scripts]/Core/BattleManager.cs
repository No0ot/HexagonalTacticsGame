using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public enum BattleTurnPhase
{
    START,
    IDLE,
    MOVE_SHOW,
    MOVE,
    ATTACK_SHOW,
    ATTACK,
    SKILL_SHOW,
    USESKILL,
    FACEING_SHOW,
    END

}

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    UnitGenerator unitGenerator;
    HexGridGenerator hexGridGenerator;
    public Queue<UnitObject> turnOrder = new Queue<UnitObject>();
    List<UnitObject> totalUnits = new List<UnitObject>();

    int round = 0;
    public bool useRandomUnits = false;

    TileSelector tileSelector;

    public UnitObject currentTurnUnit = null;
    public HexTile selectedTile { get; set; }
    public UnitObject selectedUnit { get; set; }
    public UnitObject attackedUnit = null;

    BattleTurnPhase currentTurnPhase = BattleTurnPhase.START;

    public UnityEvent<BattleTurnPhase> BroadcastPhase;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        unitGenerator = GetComponent<UnitGenerator>();
        hexGridGenerator = FindObjectOfType<HexGridGenerator>();
        tileSelector = FindObjectOfType<TileSelector>();
    }

    private void Start()
    {
        UIManager.Instance.UIButtonPressed.AddListener(StartTurnPhaseCoroutine);
        tileSelector.hexTileSelected.AddListener(HexTileSelected);
    }
    public void InitializeBattle()
    {
        hexGridGenerator.InitializeHexGrid();

        foreach(Player player in GameManager.Instance.players)
        {
            Debug.Log("Player:" + player.number);
            foreach(Unit unit in player.units)
            {
                Debug.Log("Unit:" + player.number);
                unit.InitializeUnit();
                unit.SetPlayer(player);
                UnitObject newUnit = unitGenerator.CreateUnitObject(unit);
                totalUnits.Add(newUnit);

                HexTile spawnTile = hexGridGenerator.GetRandomWalkableTile();
                newUnit.PlaceUnit(spawnTile);

                HexTile facingTile = hexGridGenerator.GetRandomNeighbour(spawnTile);
                newUnit.RotateTowards(facingTile);

                //facingTile.GetComponent<HexTileHighlight>().ShowOutline();

            }
        }

        //HexTile chosenTile = null;

        //while(chosenTile == null &)

        RoundStart();
    }

    public void RoundStart()
    {
        round++;
        UIManager.Instance.UpdateRoundCounter(round);

        RollInitiative();
    
        InitComparison comp = new InitComparison();
        List<Unit> sortList = new List<Unit>();

        foreach(Player player in GameManager.Instance.players)
        {
            sortList.AddRange(player.units);
        }
    
        sortList.Sort(comp);
        foreach (Unit unit in sortList)
        {
            foreach(UnitObject uObj in totalUnits)
            {
                if (uObj.unitInfo == unit)
                {
                    turnOrder.Enqueue(uObj);
                    
                }

            }
        }

        StartTurnPhaseCoroutine(BattleTurnPhase.START, 0.0f);
        //TurnStart();
    }

    public void TurnStart()
    {
        if (turnOrder.Count < 1)
        {
            RoundStart();
            return;
        }

        UIManager.Instance.UpdateTurnOrderBar();
        currentTurnUnit = turnOrder.Dequeue();

        currentTurnUnit.Activate();

        //Pass currenturnUnit into currenturnUnit UI(Bottom left)
        UIManager.Instance.currentUnitProfile.UpdateProfile(currentTurnUnit);

        UIManager.Instance.actionBar.SetActive(true);
        UIManager.Instance.ResetActions();
        //grid.RecomputeGlobalValues(currentTurnUnit.tile.coordinates);
        currentTurnUnit.StartTurn();

        currentTurnUnit.tile.GetComponent<HexTileHighlight>().ShowRay();
        //while(turnOrder.Count > 0)
        //{
        //    Unit temp = turnOrder.Dequeue();
        //    Debug.Log(temp.gameObject.name);
        //}

        StartTurnPhaseCoroutine(BattleTurnPhase.IDLE, 1.0f);
    }
    static int SortByInitiative(UnitObject p1, UnitObject p2)
    {
        return -p1.unitInfo.localStats.GetStat(Stat.INITIATIVE).CalculateFinalValue().CompareTo(-p2.unitInfo.localStats.GetStat(Stat.INITIATIVE).CalculateFinalValue());
    }
    
    void RollInitiative()
    {
        foreach(UnitObject unit in totalUnits)
        {
            unit.unitInfo.RollInitiative();
        }
    }

    void ShowMoveTiles()
    {
        List<HexTile> DashTiles = HexPathfinding.GetTilesWithinRange(currentTurnUnit.tile, hexGridGenerator.hexTiles, (int)currentTurnUnit.unitInfo.localStats.GetStat(Stat.DASH_RANGE).CalculateFinalValue());
        List<HexTile> MoveTiles = HexPathfinding.GetTilesWithinRange(currentTurnUnit.tile, hexGridGenerator.hexTiles, (int)currentTurnUnit.unitInfo.localStats.GetStat(Stat.MOVEMENT_RANGE).CalculateFinalValue());

        foreach(HexTile hex in MoveTiles)
        {
            hex.GetComponent<HexTileHighlight>().ShowOutline(TileHighlight.MOVE);
        }

        foreach (HexTile hex in DashTiles)
        {
            hex.GetComponent<HexTileHighlight>().ShowOutline(TileHighlight.DASH);
        }
    }

    void StartTurnPhaseCoroutine(BattleTurnPhase turnPhase, float time)
    {
        IEnumerator Coroutine;
        Coroutine = UpdateTurnPhase(turnPhase, time);
        StartCoroutine(Coroutine);
    }

    IEnumerator UpdateTurnPhase(BattleTurnPhase turnPhase, float time)
    {
        yield return new WaitForSeconds(time);

        switch(turnPhase)
        {
            case BattleTurnPhase.START:
                TurnStart();
                break;
            case BattleTurnPhase.IDLE:
                break;
            case BattleTurnPhase.MOVE_SHOW:
                ShowMoveTiles();
                break;
            case BattleTurnPhase.MOVE:
                break;
            case BattleTurnPhase.ATTACK_SHOW:
                break;
            case BattleTurnPhase.ATTACK:
                break;
            case BattleTurnPhase.SKILL_SHOW:
                break;
            case BattleTurnPhase.USESKILL:
                break;
            case BattleTurnPhase.FACEING_SHOW:
                ShowFaceingTiles();
                break;
            case BattleTurnPhase.END:
                TurnEnd();
                break;
            default:
                break;
        }

        currentTurnPhase = turnPhase;
        BroadcastPhase.Invoke(currentTurnPhase);
    }

    void ShowFaceingTiles()
    {
        List<HexTile> directions = HexPathfinding.GetNeighbors(currentTurnUnit.tile, hexGridGenerator.hexTiles);
        foreach (HexTile hex in directions)
        {
            hex.GetComponent<HexTileHighlight>().ShowOutline(TileHighlight.FACE);
        }
    }

    public void MoveUnit(HexTile hexTile)
    {
        TileHighlight highlight = hexTile.GetComponent<HexTileHighlight>().currentHighlight;

        if (highlight != TileHighlight.MOVE && highlight != TileHighlight.DASH)
            return;

        StartTurnPhaseCoroutine(BattleTurnPhase.MOVE, 0.0f);
        if(highlight == TileHighlight.MOVE)
        {

        }
        else if(highlight == TileHighlight.DASH)
        {

        }

        currentTurnUnit.PlaceUnit(hexTile);
        StartTurnPhaseCoroutine(BattleTurnPhase.IDLE, 1.0f);
    }

    public void FaceUnit(HexTile hexTile)
    {
        currentTurnUnit.RotateTowards(hexTile);
        StartTurnPhaseCoroutine(BattleTurnPhase.END, 1.0f);
    }

    void HexTileSelected(HexTile hexTile)
    {
        switch (currentTurnPhase)
        {
            case BattleTurnPhase.MOVE_SHOW:
                MoveUnit(hexTile);
                break;
            case BattleTurnPhase.ATTACK_SHOW:
                break;
            case BattleTurnPhase.SKILL_SHOW:
                break;
            case BattleTurnPhase.FACEING_SHOW:
                FaceUnit(hexTile);
                break;

        }
    }

    void TurnEnd()
    {
        StartTurnPhaseCoroutine(BattleTurnPhase.START, 1.0f);
    }
}
