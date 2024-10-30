using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitiativeCharacterPortrait : MonoBehaviour
{
    Image teamColorSprite;
    public Image jobSprite;
    public UnitObject unitReference;
    void Awake()
    {
        teamColorSprite = GetComponent<Image>();
    }

    public void Initialize(UnitObject newUnitRef)
    {
        unitReference = newUnitRef;
        teamColorSprite.color = unitReference.unitInfo.GetPlayer().color;
        jobSprite.sprite = unitReference.unitInfo.job.sprite;
        jobSprite.color = unitReference.unitInfo.race.color;
    }
}
