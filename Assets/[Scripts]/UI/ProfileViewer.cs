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

    // NICK
    public List<Effect> effects = new List<Effect>();
    public Transform scrollViewContent;
    public GameObject prefab;


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
        UpdateReferenceEffects();
    }

    public void UpdateReferenceHealth()
    {
        if (unitReference)
        {
            health.text = unitReference.unitInfo.localStats.GetStat(Stat.CURRENT_HEALTH).CalculateFinalValue() + "/" + unitReference.unitInfo.localStats.GetStat(Stat.MAX_HEALTH).CalculateFinalValue();
            healthBar.value = unitReference.unitInfo.localStats.GetStat(Stat.CURRENT_HEALTH).CalculateFinalValue() / unitReference.unitInfo.localStats.GetStat(Stat.MAX_HEALTH).CalculateFinalValue();
        }
    }

    public void UpdateReferenceEffects()
    {
        if (unitReference)
        {
            effects = unitReference.effects;

            foreach (Transform effect in scrollViewContent) 
            {
                Destroy(effect.gameObject);
            }
            foreach (Effect effect in unitReference.effects)
            {
                GameObject newEffect = Instantiate(prefab, scrollViewContent);
                var x = newEffect.transform.Find("JobSprite").GetComponent<Image>();
                x.sprite = effect.sprite;
                newEffect.AddComponent<TooltipTrigger>();
                var tooltip = newEffect.GetComponent<TooltipTrigger>();
                
                if (tooltip != null)
                {
                    tooltip.header = effect.name;
                    tooltip.content = effect.description;
                }
                
            }
        }
    }
}
