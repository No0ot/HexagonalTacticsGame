using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    UnitGenerator unitGenerator;
    Battle currentBattle;

    List<BattleLocation> battleLocations = new List<BattleLocation>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        unitGenerator = GetComponent<UnitGenerator>();
        battleLocations = GetComponentsInChildren<BattleLocation>().ToList();
    }

    private void Start()
    {
        List<Unit> teamOne = new List<Unit>();
        List<Unit> teamTwo = new List<Unit>();

        teamOne.Add(unitGenerator.CreateUnit(10));
        teamTwo.Add(unitGenerator.CreateUnit(10));


        currentBattle = new Battle(teamOne, teamTwo);

        currentBattle.StartBattle();
    }

    public void PlaceUnitInBattleLocation(Unit unit, int slot)
    {
        battleLocations[slot].PlaceUnit(unit);
    }


    #region Old Hex
    //    public static BattleManager Instance { get; private set; }
    //    public delegate void SelectHexDelegate(HexTile hex);
    //    public SelectHexDelegate selectHex;
    //
    //    public List<Unit>[] unitList = new List<Unit>[2] { new List<Unit>(), new List<Unit>()};
    //    Queue<Unit> turnOrder = new Queue<Unit>();
    //    HexGrid grid;
    //
    //    public Unit currentTurnUnit = null;
    //    public HexTile selectedTile { get; set; }
    //    public Unit selectedUnit { get; set; }
    //    public Unit attackedUnit = null;
    //
    //    TurnPhase phase;
    //    bool canMove = true;
    //    UnitGenerator unitGenerator;
    //    public List<Player> players;
    //    public Color[] playerColors;
    //
    //    private void Awake()
    //    {
    //        if (Instance != null && Instance != this)
    //            Destroy(this);
    //        else
    //            Instance = this;
    //
    //        selectHex += SelectHex;
    //        grid = FindObjectOfType<HexGrid>();
    //        unitGenerator = GetComponent<UnitGenerator>();
    //
    //        
    //    }
    //    // Start is called before the first frame update
    //    void Start()
    //    {
    //        players = GameManager.Instance.players;
    //        grid.BuildGrid();
    //
    //        for(int i = 0; i < players.Count; i++)
    //        {
    //            for(int j = 0; j < 4; j++)
    //            {
    //                Unit newUnit = unitGenerator.CreateUnit(players[i], 10);
    //                unitList[i].Add(newUnit);
    //            }
    //        }
    //
    //        foreach(Unit unit in unitList[0])
    //        {
    //            HexTile spawnTile = grid.GetSpawnTile(false);
    //            List<HexTile> spawnTiles = new List<HexTile>();
    //            if (spawnTile.type != HexType.FOREST && !spawnTile.occupant)
    //                spawnTiles.Add(spawnTile);
    //            foreach(HexTile tile in spawnTile.neighbours)
    //            {
    //                if (tile)
    //                {
    //                    if(tile.type != HexType.FOREST && !tile.occupant)
    //                        spawnTiles.Add(tile);
    //                }
    //            }
    //
    //            if(spawnTiles.Count > 0)
    //            {
    //                unit.PlaceUnit(spawnTiles[0]);
    //            }
    //        }
    //
    //        foreach (Unit unit in unitList[1])
    //        {
    //            HexTile spawnTile = grid.GetSpawnTile(true);
    //            List<HexTile> spawnTiles = new List<HexTile>();
    //            if (spawnTile.type != HexType.FOREST && !spawnTile.occupant)
    //                spawnTiles.Add(spawnTile);
    //            foreach (HexTile tile in spawnTile.neighbours)
    //            {
    //                if (tile)
    //                {
    //                    if (tile.type != HexType.FOREST && !tile.occupant)
    //                        spawnTiles.Add(tile);
    //                }
    //            }
    //
    //            if (spawnTiles.Count > 0)
    //            {
    //                unit.PlaceUnit(spawnTiles[0]);
    //            }
    //        }
    //
    //        RoundStart();
    //    }
    //
    //    void SelectHex(HexTile hex)
    //    {
    //        switch (phase)
    //        {
    //            case TurnPhase.MOVE:
    //                //if (!selectedUnit)
    //                //{
    //                //    selectedTile = hex;
    //                //    if (selectedTile.occupant)
    //                //    {
    //                //        selectedUnit = selectedTile.occupant;
    //                //        UIManager.Instance.selectedUnitProfile.UpdateProfile(selectedUnit);
    //                //    }
    //                //}
    //                //else
    //                //{
    //                //    if (!hex.occupant)
    //                //    {
    //                //        //move is done here for now but later the hex will need to be passed to an object that will determine whether a move or attack or rotation needs to be done.
    //                //        currentTurnUnit.PlaceUnit(hex);
    //                //        selectedUnit = null;
    //                //        UIManager.Instance.selectedUnitProfile.UpdateProfile(null);
    //                //    }
    //                //}
    //                foreach(HexTile tile in grid.highlightedTiles)
    //                {
    //                    if(hex == tile)
    //                    {
    //                        currentTurnUnit.PlaceUnit(hex);
    //                        UIManager.Instance.DisableAction(0);
    //                        if(tile.currentHighlight == HighlightColor.MOVE)
    //                        {
    //                            UIManager.Instance.DisableAction(1);
    //                        }
    //                        grid.ResetTiles();
    //                        grid.RecomputeGlobalValues(currentTurnUnit.tile.coordinates);
    //                        break;
    //                    }
    //                }
    //                break;
    //            case TurnPhase.ATTACK:
    //                break;
    //            case TurnPhase.SKILL:
    //                foreach (HexTile tile in grid.highlightedTiles)
    //                {
    //                    if (hex == tile)
    //                    {
    //                        List<HexTile> open = new List<HexTile>();
    //                        open.Add(hex);
    //
    //                        for(int i = 0; i < currentTurnUnit.activeSkill.radius; i++)
    //                        {
    //                            for(int j = 0; j < open.Count; j++)
    //                            {
    //                                List<HexTile> open2 = new List<HexTile>();
    //                                foreach(HexTile n in open[i].neighbours)
    //                                {
    //                                    if(!open.Contains(n))
    //                                        open2.Add(n);
    //                                }
    //                                open.AddRange(open2);
    //                            }
    //                        }
    //
    //                        List<Unit> units = new List<Unit>();
    //                        foreach(HexTile t in open)
    //                        {
    //                            if (t.occupant)
    //                                units.Add(t.occupant);
    //                        }
    //
    //                        currentTurnUnit.activeSkill.UseSkill(units);
    //                        //if(currentTurnUnit.activeSkill.type == SkillType.AOE)
    //                        //{
    //                        //
    //                        //}
    //                        //else
    //                        //{
    //                        //    Debug.Log("using skill");
    //                        //    if (hex.occupant)
    //                        //    {
    //                        //        currentTurnUnit.UseAbility(hex.occupant);
    //                        //    }
    //                        //}
    //                        break;
    //                    }
    //                }
    //                        break;
    //            case TurnPhase.FACE:
    //
    //                for(int i = 0; i < currentTurnUnit.tile.neighbours.Count; i++)
    //                {
    //                    if (currentTurnUnit.tile.neighbours[i])
    //                    {
    //                        if(currentTurnUnit.tile.neighbours[i] == hex)
    //                        {
    //                            currentTurnUnit.RotateTowards((HexDirection)i);
    //                            EndTurn();
    //                        }
    //                    }
    //                }
    //
    //                break;
    //            default:
    //                break;
    //        }
    //        
    //    }
    //
    //    public void Unselect()
    //    {
    //        selectedUnit = null;
    //        selectedTile = null;
    //    }
    //
    //    public void RoundStart()
    //    {
    //        RollInitiative();
    //
    //        InitComparison comp = new InitComparison();
    //        List<Unit> sortList = new List<Unit>();
    //        sortList.AddRange(unitList[0]);
    //        sortList.AddRange(unitList[1]);
    //
    //        sortList.Sort(comp);
    //        foreach(Unit unit in sortList)
    //        {
    //            turnOrder.Enqueue(unit);
    //        }
    //
    //        TurnStart();
    //    }
    //    static int SortByInitiative(Unit p1, Unit p2)
    //    {
    //        return -p1.initiative.CompareTo(-p2.initiative);
    //    }
    //
    //    void RollInitiative()
    //    {
    //        foreach (List<Unit> list in unitList)
    //        {
    //            foreach (Unit unit in list)
    //            {
    //                unit.RollInitiative();
    //            }
    //        }
    //    }
    //
    //    public void TurnStart()
    //    {
    //        phase = TurnPhase.NONE;
    //        currentTurnUnit = turnOrder.Dequeue();
    //
    //        currentTurnUnit.Activate();
    //
    //        //Pass currenturnUnit into currenturnUnit UI(Bottom left)
    //        UIManager.Instance.currentUnitProfile.UpdateProfile(currentTurnUnit);
    //
    //        UIManager.Instance.actionBar.SetActive(true);
    //        UIManager.Instance.ResetActions();
    //        grid.RecomputeGlobalValues(currentTurnUnit.tile.coordinates);
    //        //while(turnOrder.Count > 0)
    //        //{
    //        //    Unit temp = turnOrder.Dequeue();
    //        //    Debug.Log(temp.gameObject.name);
    //        //}
    //    }
    //
    //    public void HighlightTiles()
    //    {
    //        switch(phase)
    //        {
    //            case TurnPhase.MOVE:
    //                // just call a function in grid to do all this instead, like in attack phase
    //                grid.highlightedTiles = grid.GetReachableHexes(currentTurnUnit.tile, currentTurnUnit.movementRange);
    //                grid.ResetHexPathfindingValues();
    //                List<HexTile> temp = new List<HexTile>();
    //                temp = grid.GetReachableHexes(currentTurnUnit.tile, currentTurnUnit.dashRange);
    //                foreach (HexTile tile in grid.highlightedTiles)
    //                {
    //                    if (tile)
    //                        tile.ActivateHighlight(HighlightColor.MOVE);
    //                }
    //
    //                foreach (HexTile tile in temp)
    //                {
    //                    if (tile)
    //                        tile.ActivateHighlight(HighlightColor.DASH);
    //                }
    //                break;
    //            case TurnPhase.ATTACK:
    //                grid.GetThreatenedTiles(currentTurnUnit.tile, currentTurnUnit.job.attackRange);
    //                List<Unit> threatenedUnits = new List<Unit>();
    //                foreach (HexTile tile in grid.highlightedTiles)
    //                {
    //                    if (!tile.occupant || tile.occupant.playerNum == currentTurnUnit.playerNum)
    //                        continue;
    //
    //                    threatenedUnits.Add(tile.occupant);
    //                }
    //
    //                threatenedUnits.Sort(new ThreatComparison());
    //
    //                if (threatenedUnits.Count > 0)
    //                {
    //                    attackedUnit = threatenedUnits[0];
    //                    attackedUnit.Threatened();
    //                    UIManager.Instance.selectedUnitProfile.UpdateProfile(attackedUnit);
    //                }
    //                else
    //                {
    //                    Debug.Log("No Target");
    //                    break;
    //                }
    //                attackedUnit.tile.ActivateHighlight(HighlightColor.ATTACK);
    //                List<HexTile> direction = grid.HexLineDraw(currentTurnUnit.tile, attackedUnit.tile);
    //
    //                foreach (HexTile lineTile in direction)
    //                {
    //                    foreach (HexTile neighbour in attackedUnit.tile.neighbours)
    //                    {
    //                        if (lineTile == neighbour)
    //                        {
    //                            attackedUnit.attackDirection = neighbour;
    //                            neighbour.ActivateHighlight(HighlightColor.SKILL);
    //                            break;
    //                        }
    //                    }
    //                    if (attackedUnit.attackDirection != null)
    //                        break;
    //                }
    //                break;
    //            case TurnPhase.SKILL:
    //                grid.highlightedTiles = grid.GetThreatenedTiles(currentTurnUnit.tile, currentTurnUnit.activeSkill.range);
    //                foreach (HexTile tile in grid.highlightedTiles)
    //                {
    //                    if (tile)
    //                        tile.ActivateHighlight(HighlightColor.SKILL);
    //                }
    //                break;
    //            case TurnPhase.FACE:
    //                List<HexTile> neighbours = new List<HexTile>();
    //                foreach(HexTile n in currentTurnUnit.tile.neighbours)
    //                {
    //                    neighbours.Add(n);
    //                }
    //                grid.highlightedTiles = neighbours;
    //                foreach(HexTile tile in grid.highlightedTiles)
    //                {
    //                    if(tile)
    //                        tile.ActivateHighlight(HighlightColor.FACE);
    //                }
    //
    //                break;
    //            default:
    //                break;
    //        }
    //    }
    //
    //    public void FaceUnit()
    //    {
    //        grid.ResetTiles();
    //        phase = TurnPhase.FACE;
    //        HighlightTiles();
    //    }
    //
    //    public void MoveUnit()
    //    {
    //        grid.ResetTiles();
    //        phase = TurnPhase.MOVE;
    //        HighlightTiles();
    //    }
    //
    //    public void AttackUnit()
    //    {
    //        if (!attackedUnit)
    //            return;
    //
    //        
    //        currentTurnUnit.Attack(attackedUnit, attackedUnit.attackDirection);
    //
    //        UIManager.Instance.DisableAction(1);
    //        ShowThreatendHexs(false);
    //        UIManager.Instance.selectedUnitProfile.UpdateProfile(null);
    //    }    
    //
    //    public void UseSkill(int skillnum)
    //    {
    //        currentTurnUnit.activeSkill = currentTurnUnit.skills[skillnum];
    //        phase = TurnPhase.SKILL;
    //        HighlightTiles();
    //    }
    //
    //    public void EndTurn()
    //    {
    //        foreach (HexTile tile in grid.highlightedTiles)
    //        {
    //            if (tile)
    //                tile.ActivateHighlight(HighlightColor.NONE);
    //        }
    //        canMove = true;
    //        grid.ResetTiles();
    //        currentTurnUnit.Deactivate();
    //        if (turnOrder.Count > 0)
    //            TurnStart();
    //        else
    //            RoundStart();
    //    }
    //
    //    public void ShowThreatendHexs(bool show)
    //    {
    //        if (!show)
    //        {
    //            grid.ResetTiles();
    //            if (attackedUnit)
    //            {
    //                attackedUnit.Deactivate();
    //                attackedUnit.attackDirection = null;
    //                attackedUnit = null;
    //            }
    //        }
    //        else
    //        {
    //            phase = TurnPhase.ATTACK;
    //            HighlightTiles();
    //        }
    //    }
    //
    //    public void KillUnit(Unit deadUnit)
    //    {
    //
    //    }
    //
    //    public List<Unit> GetSkillTargets(Skill usedSkill)
    //    {
    //        List<Unit> targets = new List<Unit>();
    //
    //
    //        return targets;
    //    }
    #endregion
}
