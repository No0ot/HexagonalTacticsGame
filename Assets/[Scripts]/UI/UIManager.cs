using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject actionBar;
    public GameObject skillBar;

    public ProfileViewer currentUnitProfile;
    public ProfileViewer selectedUnitProfile;

    public GameObject turnOrderParent;
    public List<InitiativeCharacterPortrait> initiativeCharacterPortraits = new List<InitiativeCharacterPortrait>();

    public TMP_Text roundText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        initiativeCharacterPortraits.AddRange(GetComponentsInChildren<InitiativeCharacterPortrait>());
        foreach(InitiativeCharacterPortrait port in initiativeCharacterPortraits)
        {
            port.gameObject.SetActive(false);
        }
    }
   

    public void ActionMove()
    {
        BattleManager.Instance.MoveUnit();
    }

    public void ActionEnd()
    {
        BattleManager.Instance.FaceUnit();
    }

    public void DisableAction(int button)
    {
        actionBar.transform.GetChild(button).GetComponent<Button>().interactable = false;
    }

    public void ActionHoverAttack(bool tf)
    {
        if(actionBar.transform.GetChild(1).GetComponent<Button>().interactable)
            BattleManager.Instance.ShowThreatendHexs(tf);
    }

    public void ActionAttack()
    {
        BattleManager.Instance.AttackUnit();
    
    }

    public void ActionSkill(int buttonNum)
    {
        BattleManager.Instance.UseSkill(buttonNum);
    }

    public void ShowSkills(bool tf)
    {
        if(skillBar.activeInHierarchy && tf)
        {
            skillBar.SetActive(false);
            return;
        }
        skillBar.SetActive(tf);
    }

    public void ResetActions()
    {
        actionBar.transform.GetChild(0).GetComponent<Button>().interactable = true;
        actionBar.transform.GetChild(1).GetComponent<Button>().interactable = true;
        actionBar.transform.GetChild(2).GetComponent<Button>().interactable = true;
        actionBar.transform.GetChild(3).GetComponent<Button>().interactable = true;
    }

    public void AddToTurnOrder(Unit unitRef)
    {
        var newRef = Instantiate(new InitiativeCharacterPortrait(), turnOrderParent.transform);
        newRef.unitReference = unitRef;
        //newRef.Initialize();
    }

    public void UpdateTurnOrderBar()
    {
        var turnOrder = BattleManager.Instance.turnOrder;
        foreach (InitiativeCharacterPortrait port in initiativeCharacterPortraits)
        {
            port.gameObject.SetActive(false);
        }

        int count = 0;
        foreach(Unit u in turnOrder)
        {
            initiativeCharacterPortraits[count].gameObject.SetActive(true);
            initiativeCharacterPortraits[count].Initialize(u);
            count++;
        }
    }

    public void UpdateRoundCounter(int round)
    {
        roundText.text = "Round: " + round;
    }
}
