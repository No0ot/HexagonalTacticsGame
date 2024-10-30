using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ProfileViewer : MonoBehaviour
{
    public UnitObject unitReference;

    public TMP_Text name;
    public TMP_Text job;
    public TMP_Text race;
    public TMP_Text health;
    public Slider healthBar;
    public Image image;


    public void UpdateProfile(UnitObject newUnit)
    {
        if(newUnit == null)
        {
            gameObject.SetActive(false);
            unitReference = null;
            return;
        }
        else
        {
            if (!gameObject.activeInHierarchy)
                gameObject.SetActive(true);
        }

        unitReference = newUnit;

        name.text = unitReference.unitInfo.name;
        job.text = unitReference.unitInfo.job.name;
        race.text = unitReference.unitInfo.race.name;
        image.sprite = unitReference.unitInfo.job.sprite;
        image.color = unitReference.unitInfo.race.color;
        race.color = unitReference.unitInfo.race.color;

        UpdateReferenceHealth();
    }

    public void UpdateReferenceHealth()
    {
        if (unitReference)
        {
            health.text = unitReference.unitInfo.localStats.GetStat(Stat.CURRENT_HEALTH).CalculateFinalValue() + "/" + unitReference.unitInfo.localStats.GetStat(Stat.MAX_HEALTH).CalculateFinalValue();
            healthBar.value = unitReference.unitInfo.localStats.GetStat(Stat.CURRENT_HEALTH).CalculateFinalValue() / unitReference.unitInfo.localStats.GetStat(Stat.MAX_HEALTH).CalculateFinalValue();
        }
    }
}
