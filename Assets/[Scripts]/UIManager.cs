using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    }

    public void ActionEnd()
    {
        BattleManager.Instance.FaceUnit();
    }
}
