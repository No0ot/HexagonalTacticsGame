using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public LayerMask mask;
    public Camera camera;
    Vector3 point;
    GameObject mouseOverHex;

    private void Awake()
    {
        camera = Camera.main;
    }
    void Update()
    {
        Vector2 point = camera.ScreenToWorldPoint(Input.mousePosition);
        
        Collider2D hit = Physics2D.OverlapPoint(point, mask);
        if (hit)
        {
            mouseOverHex = hit.gameObject;
        }
        else
            mouseOverHex = null;

        if(Input.GetMouseButtonDown(0))
        {
            HexTile hex = mouseOverHex.GetComponent<HexTile>();
            BattleManager.Instance.selectHex?.Invoke(hex);
        }
    }

}
