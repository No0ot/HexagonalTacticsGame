using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;
    public LayoutElement layoutElement;
    public int chararacterWrapLimit;
    //public RectTransform rectTransform;
   
    //private void Awake()
    //{
    //    rectTransform = GetComponent<RectTransform>();
    //}

    private void Update()
    {
        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > chararacterWrapLimit || contentLength > chararacterWrapLimit) ? true : false;

        //Vector2 position = Input.mousePosition;
        //float pivotX = position.x / Screen.width;
        //float pivotY = position.y / Screen.height;
        //rectTransform.pivot = new Vector2(pivotX, pivotY);
        //transform.position = position;
    }
}
