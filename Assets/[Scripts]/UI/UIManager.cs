using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject actionBar;
    public GameObject skillBar;
    public GameObject attackConfirmButton;
    public List<GameObject> skillButtons = new List<GameObject>();

    public ProfileViewer currentUnitProfile;
    public ProfileViewer selectedUnitProfile;

    public GameObject turnOrderParent;
    public InitiativeCharacterPortrait initCharacterPortrait;
    public List<InitiativeCharacterPortrait> initiativeCharacterPortraits = new List<InitiativeCharacterPortrait>();

    public TMP_Text roundText;

    public UnityEvent<BattleTurnPhase, float> UIButtonPressed;

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
        UIButtonPressed.Invoke(BattleTurnPhase.MOVE_SHOW, 0.0f);
    }

    public void ActionEnd()
    {
        UIButtonPressed.Invoke(BattleTurnPhase.FACEING_SHOW, 0.0f);
    }

    public void DisableAction(int button)
    {
        actionBar.transform.GetChild(button).GetComponent<Button>().interactable = false;
    }

    public void ActionHoverAttack(bool tf)
    {
        //if(actionBar.transform.GetChild(1).GetComponent<Button>().interactable)
        //    BattleManager.Instance.ShowThreatendHexs(tf);
    }

    public void ActionAttack()
    {
        UIButtonPressed.Invoke(BattleTurnPhase.ATTACK_SHOW, 0.0f);
        attackConfirmButton.SetActive(!attackConfirmButton.activeInHierarchy);
    }

    public void AttackConfirm()
    {
        UIButtonPressed.Invoke(BattleTurnPhase.ATTACK, 0.0f);
        attackConfirmButton.SetActive(!attackConfirmButton.activeInHierarchy);
    }

    public void ShowSkill(int buttonNum)
    {
        UIButtonPressed.Invoke(BattleTurnPhase.SKILL_SHOW, 0.0f);
        //attackConfirmButton.SetActive(!attackConfirmButton.activeInHierarchy);
        BattleManager.Instance.activeSkillNum = buttonNum;
    }

    public void SkillConfirm()
    {
        //BattleManager.Instance.UseSkill(buttonNum);
    }

    public void ShowSkills(bool tf)
    {

        if(skillBar.activeInHierarchy && tf)
        {
            skillBar.SetActive(false);
            return;
        }
        skillBar.SetActive(tf);

        if(tf)
        {
            for(int i = 0; i < skillButtons.Count; i++)
            {
                if(i + 1 > BattleManager.Instance.currentTurnUnit.skills.Count)
                {
                    skillButtons[i].GetComponent<Button>().interactable = false;
                    continue;
                }
            
                if (BattleManager.Instance.currentTurnUnit.skillCooldowns[i] > 0)
                {
                    skillButtons[i].GetComponent<Button>().interactable = false;
                }
                else
                {
                    TMP_Text buttonLabel = skillButtons[i].GetComponentInChildren<TMP_Text>();
                    buttonLabel.text = BattleManager.Instance.currentTurnUnit.skills[i].name;
                    skillButtons[i].GetComponent<Button>().interactable = true;
                }
            }
        }
        
        // NICK - Update Effects UI after using effect
        currentUnitProfile.UpdateReferenceEffects();
        selectedUnitProfile.UpdateReferenceEffects();
    }

    public void ResetActions()
    {
        actionBar.transform.GetChild(0).GetComponent<Button>().interactable = true;
        actionBar.transform.GetChild(1).GetComponent<Button>().interactable = true;
        actionBar.transform.GetChild(2).GetComponent<Button>().interactable = true;
        actionBar.transform.GetChild(3).GetComponent<Button>().interactable = true;
    }

    public void UpdateTurnOrderBar()
    {
        var turnOrder = BattleManager.Instance.turnOrder;
        foreach (InitiativeCharacterPortrait port in initiativeCharacterPortraits)
        {
            port.gameObject.SetActive(false);
        }
        
        int count = 0;
        foreach(UnitObject u in turnOrder)
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
