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
        job.text = unitReference.pawn.job.name;
        race.text = unitReference.pawn.race.name;
        image.sprite = unitReference.pawn.job.sprite;
        race.color = unitReference.pawn.race.color;
    
        UpdateReferenceHealth();
    }
    
    public void UpdateReferenceHealth()
    {
        if (unitReference)
        {
            health.text = unitReference.pawn.stats.currentHealth + "/" + unitReference.pawn.stats.maxHealth;
            healthBar.value = unitReference.pawn.stats.currentHealth / unitReference.pawn.stats.maxHealth;
        }
    }
}
