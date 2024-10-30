using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DebugUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text turnPhaseText;
    // Start is called before the first frame update
    void Start()
    {
        BattleManager.Instance.BroadcastPhase.AddListener(UpdateTurnPhaseText);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateTurnPhaseText(BattleTurnPhase newPhase)
    {
        turnPhaseText.text = newPhase.ToString();
    }


}
