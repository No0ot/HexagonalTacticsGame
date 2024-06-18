using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitiativeCharacterPortrait : MonoBehaviour
{
    Image teamColorSprite;
    public Image jobSprite;
    public Unit unitReference;
    void Awake()
    {
        teamColorSprite = GetComponent<Image>();
    }

    public void Initialize(Unit newUnitRef)
    {
        unitReference = newUnitRef;
        teamColorSprite.color = unitReference.playerColor;
        jobSprite.sprite = unitReference.job.sprite;
        jobSprite.color = unitReference.race.color;
    }
}
