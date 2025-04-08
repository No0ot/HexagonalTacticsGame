using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;
public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;
    public Tooltip tooltip;

    public void Awake()
    {
        current = this;
        Hide();
    }


    public static void Show()
    {
        
        current.tooltip.gameObject.SetActive(true);
    }

   
    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }

    public static void UpdateTooltip(string header, string content)
    {
        current.tooltip.headerField.text = header;
        current.tooltip.contentField.text = content;
    }

    public static void ClearTooltip()
    {
        current.tooltip.headerField.text = "";
        current.tooltip.contentField.text = "";
    }
}

