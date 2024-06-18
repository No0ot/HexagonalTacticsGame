using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ProfileViewer : MonoBehaviour
{
    public Unit unitReference;

    public TMP_Text name;
    public TMP_Text job;
    public TMP_Text race;
    public TMP_Text health;
    public Slider healthBar;
    public Image image;


    public void UpdateProfile(Unit newUnit)
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

        name.text = unitReference.name;
        job.text = unitReference.job.name;
        race.text = unitReference.race.name;
        image.sprite = unitReference.job.sprite;
        image.color = unitReference.race.color;
        race.color = unitReference.race.color;

        UpdateReferenceHealth();
    }

    public void UpdateReferenceHealth()
    {
        if (unitReference)
        {
            health.text = unitReference.localStats.GetStat(Stat.CURRENT_HEALTH).CalculateFinalValue() + "/" + unitReference.localStats.GetStat(Stat.MAX_HEALTH).CalculateFinalValue();
            healthBar.value = unitReference.localStats.GetStat(Stat.CURRENT_HEALTH).CalculateFinalValue() / unitReference.localStats.GetStat(Stat.MAX_HEALTH).CalculateFinalValue();
        }
    }
}
