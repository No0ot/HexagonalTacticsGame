using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TileHighlight
{ 
    NONE = 0,
    MOVE,
    DASH,
    ATTACK_RANGE,
    ATTACK_TARGET,
    SKILL_RANGE,
    SKILL_TARGET,
    FACE
}


public class HexTileHighlight : MonoBehaviour
{
    public Material outlineMaterial; // Assign the OutlineMaterial here
    public GameObject outlineGO;
    public GameObject rayGO;

    public TileHighlight currentHighlight;

    public List<Color> highlightColors;
    private void Start()
    {
        if(BattleManager.Instance)
            BattleManager.Instance.BroadcastPhase.AddListener(BattlePhaseNotify);
    }
    public void ShowRay()
    {
        rayGO.SetActive(true);
    }

    public void ShowRay(Color newColor)
    {
        rayGO.GetComponent<MeshRenderer>().material.color = newColor;
        rayGO.SetActive(true);
    }

    public void HideRay()
    {
        rayGO.SetActive(false);
    }
    public void ShowOutline()
    {
        outlineGO.SetActive(true); // Show the outline
    }

    public void ShowOutline(TileHighlight highlight)
    {
        switch (highlight)
        {
            case TileHighlight.MOVE:
                outlineGO.GetComponent<MeshRenderer>().material.color = highlightColors[1];
                break;
            case TileHighlight.DASH:
                outlineGO.GetComponent<MeshRenderer>().material.color = highlightColors[2];
                break;
            case TileHighlight.ATTACK_RANGE:
                outlineGO.GetComponent<MeshRenderer>().material.color = highlightColors[3];
                break;
            case TileHighlight.ATTACK_TARGET:
                outlineGO.GetComponent<MeshRenderer>().material.color = highlightColors[4];
                break;
            case TileHighlight.SKILL_RANGE:
                outlineGO.GetComponent<MeshRenderer>().material.color = highlightColors[5];
                break;
            case TileHighlight.SKILL_TARGET:
                outlineGO.GetComponent<MeshRenderer>().material.color = highlightColors[6];
                break;
            case TileHighlight.FACE:
                outlineGO.GetComponent<MeshRenderer>().material.color = highlightColors[7];
                break;
            default:
                break;
        }

        currentHighlight = highlight;
        outlineGO.SetActive(true); // Show the outline
    }

    public void RemoveHighlight()
    {
        outlineGO.SetActive(false); // Hide the outline
    }

    void BattlePhaseNotify(BattleTurnPhase phase)
    {
        switch (phase)
        {
            case BattleTurnPhase.START:
                break;
            case BattleTurnPhase.IDLE:
                RemoveHighlight();
                break;
            case BattleTurnPhase.MOVE_SHOW:
                break;
            case BattleTurnPhase.MOVE:
                RemoveHighlight();
                break;
            case BattleTurnPhase.ATTACK_SHOW:
                break;
            case BattleTurnPhase.ATTACK:
                RemoveHighlight();
                break;
            case BattleTurnPhase.SKILL_SHOW:
                break;
            case BattleTurnPhase.USESKILL:
                RemoveHighlight();
                break;
            case BattleTurnPhase.END:
                RemoveHighlight();
                break;
            default:
                break;
        }
    }
}