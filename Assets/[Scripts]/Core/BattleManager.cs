using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.CanvasScaler;

public enum BattleTurnPhase
{
    NONE = -1,
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
    public UnitObject attackTargetedUnit = null;
    public List<UnitObject> attackedUnits = new List<UnitObject>();

    BattleTurnPhase currentTurnPhase = BattleTurnPhase.NONE;
    public BattleTurnPhase GetCurrentTurnPhase()
    { return currentTurnPhase; }
    public UnityEvent<BattleTurnPhase> BroadcastPhase;

     public int activeSkillNum = -1;

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

    static int SortByThreat(UnitObject p1, UnitObject p2)
    {
        return -p1.unitInfo.localStats.GetStat(Stat.THREAT).CalculateFinalValue().CompareTo(-p2.unitInfo.localStats.GetStat(Stat.THREAT).CalculateFinalValue());
    }

    void RollInitiative()
    {
        foreach(UnitObject unit in totalUnits)
        {
            unit.unitInfo.RollInitiative();
        }

        InitComparison comp = new InitComparison();
        List<Unit> sortList = new List<Unit>();

        foreach (Player player in GameManager.Instance.players)
        {
            sortList.AddRange(player.units);
        }

        sortList.Sort(comp);
        foreach (Unit unit in sortList)
        {
            foreach (UnitObject uObj in totalUnits)
            {
                if (uObj.unitInfo == unit)
                {
                    turnOrder.Enqueue(uObj);

                }

            }
        }
    }

    UnitObject GetAttackThreatenedTarget(List<UnitObject> sortList)
    {
        ThreatComparison comp = new ThreatComparison();
        List<Unit> unitlist = new List<Unit>();

        foreach(UnitObject uObj in sortList)
        {
            unitlist.Add(uObj.unitInfo);
        }

        unitlist.Sort(comp);

        if(unitlist.Count >= 1)
        {
            foreach(UnitObject uObj in sortList)
            {
                if(uObj.unitInfo == unitlist[0])
                {
                    return uObj;
                }
            }
        }

        Debug.Log("No threatened targets.");
        return null;
    }

    void ShowMoveTiles()
    {
        List<HexTile> DashTiles = new List<HexTile>();
        List<HexTile> MoveTiles = new List<HexTile>();

        if (!currentTurnUnit.hasDashed)
        {
            int dashRange = (int)currentTurnUnit.unitInfo.localStats.GetStat(Stat.DASH_RANGE).CalculateFinalValue();
            DashTiles = HexPathfinding.GetTilesWithinMovementRange(currentTurnUnit.tile, hexGridGenerator.hexTiles, dashRange);
        }

        if (!currentTurnUnit.hasMoved && currentTurnUnit.actionPoint > 0)
        {
            int moveRange = 0;

            if (currentTurnUnit.hasDashed)
                moveRange = (int)currentTurnUnit.unitInfo.localStats.GetStat(Stat.MOVEMENT_RANGE).CalculateFinalValue() - (int)currentTurnUnit.unitInfo.localStats.GetStat(Stat.DASH_RANGE).CalculateFinalValue();
            else
                moveRange = (int)currentTurnUnit.unitInfo.localStats.GetStat(Stat.MOVEMENT_RANGE).CalculateFinalValue();

            MoveTiles = HexPathfinding.GetTilesWithinMovementRange(currentTurnUnit.tile, hexGridGenerator.hexTiles, moveRange);
        }

        foreach(HexTile hex in MoveTiles)
        {
            hex.GetComponent<HexTileHighlight>().ShowOutline(TileHighlight.MOVE);
        }

        foreach (HexTile hex in DashTiles)
        {
            hex.GetComponent<HexTileHighlight>().ShowOutline(TileHighlight.DASH);
        }
    }

    void ShowAttackTiles()
    {
        List<HexTile> AttackTiles = new List<HexTile>();
        List<UnitObject> UnitsInTiles = new List<UnitObject>();
        UnitObject threatenedTarget;

        if (currentTurnUnit.specialAttackSkill != null)
        {
            SpecialAttack specialAttack = (SpecialAttack)currentTurnUnit.specialAttackSkill;
            AttackTiles = specialAttack.GetThreatenedHexs(hexGridGenerator.hexTiles);

            UnitsInTiles = specialAttack.GetThreatenedUnits(AttackTiles);
            threatenedTarget = GetAttackThreatenedTarget(UnitsInTiles);
            attackTargetedUnit = threatenedTarget;

            foreach (HexTile hex in AttackTiles)
            {
                hex.GetComponent<HexTileHighlight>().ShowOutline(TileHighlight.ATTACK_RANGE);
            }

            if(specialAttack.radius > 0)
            { 
                foreach(UnitObject unit in UnitsInTiles)
                {
                    unit.tile.GetComponent<HexTileHighlight>().ShowOutline(TileHighlight.ATTACK_TARGET);
                }

                attackedUnits = UnitsInTiles;
            }
            else
            {
                threatenedTarget.tile.GetComponent<HexTileHighlight>().ShowOutline(TileHighlight.ATTACK_TARGET);
                

                attackedUnits.Add(threatenedTarget);
            }
            return;
        }

        AttackTiles = HexPathfinding.GetTilesWithinAttackRange(currentTurnUnit.tile, hexGridGenerator.hexTiles, (int)currentTurnUnit.unitInfo.localStats.GetStat(Stat.RANGE).CalculateFinalValue());

        foreach (HexTile hex in AttackTiles)
        {
            hex.GetComponent<HexTileHighlight>().ShowOutline(TileHighlight.ATTACK_RANGE);

            if(hex.Occupant != null)
            {
                UnitObject unitOnTile = hex.Occupant; 
                if(unitOnTile.unitInfo.GetPlayer() != currentTurnUnit.unitInfo.GetPlayer())
                {
                    UnitsInTiles.Add(unitOnTile);
                }

            }
        }
        if (UnitsInTiles.Count <= 0)
            return;

        threatenedTarget = GetAttackThreatenedTarget(UnitsInTiles);
        attackTargetedUnit = threatenedTarget;

        List<HexTile> AOETiles = HexPathfinding.GetTilesWithinAttackRange(attackTargetedUnit.tile, hexGridGenerator.hexTiles, (int)currentTurnUnit.unitInfo.localStats.GetStat(Stat.RADIUS).CalculateFinalValue());
        
        foreach (HexTile hex in AOETiles)
        {
            hex.GetComponent<HexTileHighlight>().ShowOutline(TileHighlight.ATTACK_TARGET);
            if(hex.Occupant)
            {
                UnitObject unitOnTile = hex.Occupant;
                if (unitOnTile.unitInfo.GetPlayer() != currentTurnUnit.unitInfo.GetPlayer())
                {
                    attackedUnits.Add(hex.Occupant);
                }
                
            }
        }

    }
    public void ResolveAttack()
    {
        if(attackedUnits.Count <= 0)
        {
            return;
        }

        if (currentTurnUnit.specialAttackSkill != null)
        {
            Debug.Log("Special Attack Skill used");
            currentTurnUnit.specialAttackSkill.UseSkill(attackTargetedUnit.tile, hexGridGenerator.hexTiles);
        }
        else
        {
            foreach (UnitObject unit in attackedUnits)
            {
                currentTurnUnit.Attack(unit);
            }
        }

        currentTurnUnit.specialAttackSkill = null;
        UIManager.Instance.DisableAction(1);
        currentTurnUnit.UseActionPoint();
        currentTurnUnit.hasAttacked = true;
       StartTurnPhaseCoroutine(BattleTurnPhase.IDLE, 1.0f);
    }

    void StartTurnPhaseCoroutine(BattleTurnPhase turnPhase, float time)
    {
        if(turnPhase == currentTurnPhase)
        {
            turnPhase = BattleTurnPhase.IDLE;
        }

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
                attackedUnits.Clear();
                attackTargetedUnit = null;
                UIManager.Instance.ResetActions();
                break;
            case BattleTurnPhase.MOVE_SHOW:
                ShowMoveTiles();
                break;
            case BattleTurnPhase.MOVE:
                break;
            case BattleTurnPhase.ATTACK_SHOW:
                ShowAttackTiles();
                break;
            case BattleTurnPhase.ATTACK:
                ResolveAttack();
                break;
            case BattleTurnPhase.SKILL_SHOW:
                ShowSkillTiles();
                break;
            case BattleTurnPhase.USESKILL:
                UIManager.Instance.ShowSkills(false);
                StartTurnPhaseCoroutine(BattleTurnPhase.IDLE, 1.0f);
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

    public bool IsActiveSkillSpecialAttack()
    {
        if (currentTurnUnit.activeSkill as SpecialAttack != null)
        {
            return true;
        }
        return false;
    }

    void ShowSkillTiles()
    {
        currentTurnUnit.activeSkill = currentTurnUnit.skills[activeSkillNum];
        currentTurnUnit.activeSkill.user = currentTurnUnit;

        //if(currentTurnUnit.activeSkill as SpecialAttack != null)
        //{
        //    UIManager.Instance.ShowSkillConfirm();
        //    return;
        //}

        List<HexTile> skillThreatenedHexes = currentTurnUnit.activeSkill.GetHexesInRange(hexGridGenerator.hexTiles);
        foreach (HexTile hex in skillThreatenedHexes)
        {
            hex.GetComponent<HexTileHighlight>().ShowOutline(TileHighlight.SKILL_RANGE);
        }
    }

    void ShowFaceingTiles()
    {
        List<HexTile> directions = HexPathfinding.GetNeighbors(currentTurnUnit.tile, hexGridGenerator.hexTiles);
        foreach (HexTile hex in directions)
        {
            hex.GetComponent<HexTileHighlight>().ShowOutline(TileHighlight.FACE);
        }
    }

    void UseActiveSkill(HexTile hexTile)
    {
        currentTurnUnit.activeSkill.UseSkill(hexTile, hexGridGenerator.hexTiles);
        StartTurnPhaseCoroutine(BattleTurnPhase.USESKILL, 0.0f);
        UIManager.Instance.DisableAction(2);
        currentTurnUnit.UseActionPoint();
        currentTurnUnit.hasSkilled = true;
    }

    public void MoveUnit(HexTile hexTile)
    {
        TileHighlight highlight = hexTile.GetComponent<HexTileHighlight>().currentHighlight;

        if (highlight == TileHighlight.MOVE || highlight == TileHighlight.DASH)
        {

        }
        else
            return;

        StartTurnPhaseCoroutine(BattleTurnPhase.MOVE, 0.0f);
        if(highlight == TileHighlight.MOVE)
        {
            currentTurnUnit.hasMoved = true;
            currentTurnUnit.UseActionPoint();
        }
        else if(highlight == TileHighlight.DASH)
        {
            currentTurnUnit.hasDashed = true;
        }

        UIManager.Instance.DisableActionBar();

        currentTurnUnit.PlaceUnit(hexTile);
        StartTurnPhaseCoroutine(BattleTurnPhase.IDLE, 1.0f);
    }

    public void FaceUnit(HexTile hexTile)
    {
        TileHighlight highlight = hexTile.GetComponent<HexTileHighlight>().currentHighlight;

        if (highlight != TileHighlight.FACE)
            return;

        currentTurnUnit.RotateTowards(hexTile);
        StartTurnPhaseCoroutine(BattleTurnPhase.END, 0.0f);
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
                UseActiveSkill(hexTile);
                break;
            case BattleTurnPhase.FACEING_SHOW:
                FaceUnit(hexTile);
                break;

        }
    }

    void TurnEnd()
    {
        currentTurnUnit.EndTurn();
        UIManager.Instance.ResetActionBar();
        StartTurnPhaseCoroutine(BattleTurnPhase.START, 1.0f);
    }

    public Dictionary<Vector2Int,HexTile> GetHexTiles()
    {
        return hexGridGenerator.hexTiles;
    }
}
