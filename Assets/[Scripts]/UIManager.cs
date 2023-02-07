using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject actionBar;
    public GameObject skillBar;

    public ProfileViewer currentUnitProfile;
    public ProfileViewer selectedUnitProfile;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
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
        BattleManager.Instance.ShowThreatendHexs(tf);
    }

    public void ActionAttack()
    {
        BattleManager.Instance.AttackUnit();
    }

    public void ResetActions()
    {
        actionBar.transform.GetChild(0).GetComponent<Button>().interactable = true;
        actionBar.transform.GetChild(1).GetComponent<Button>().interactable = true;
        actionBar.transform.GetChild(2).GetComponent<Button>().interactable = true;
        actionBar.transform.GetChild(3).GetComponent<Button>().interactable = true;
    }
}
