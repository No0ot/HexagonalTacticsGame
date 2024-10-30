using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FloatingCombatText : MonoBehaviour
{
    public RectTransform rect;
    TMP_Text text;
    public float duration;
    public float size;
    public float speed;
    Camera camera;
    Vector3 startingPosition;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        rect = GetComponent<RectTransform>();
        camera = Camera.main;
    }
    private void Update()
    {

        startingPosition.y += speed  * Time.deltaTime;
        rect.position = startingPosition;
        rect.position = camera.WorldToScreenPoint(startingPosition);
    }
    public void UpdateText(float damage, UnitObject unit)
    {
        //unitRef = unit;
        startingPosition = unit.transform.position;
        if (damage == 0)
            text.text = "Missed";
        else
            text.text = "-" + damage;
        text.fontSize = size;
        StartCoroutine(Decay(duration));
    }

    IEnumerator Decay(float duration)
    {
        float timer = 0f;
        while(timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
