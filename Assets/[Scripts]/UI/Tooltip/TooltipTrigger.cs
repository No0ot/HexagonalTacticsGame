using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string header;
    public string content;
    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.UpdateTooltip(header, content);
        TooltipSystem.Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
        TooltipSystem.ClearTooltip();
    }
}
