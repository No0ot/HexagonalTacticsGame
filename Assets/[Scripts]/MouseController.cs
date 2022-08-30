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
        {
            //BattleManager.Instance.Unselect();
            mouseOverHex = null;
        }

        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize + -Input.GetAxis("Mouse ScrollWheel"), 1.0f, 3.5f);

        
        float posY = camera.transform.position.y;
        float posX = camera.transform.position.x;

        if (Input.GetKey("w"))
        {
            posY = Mathf.Clamp(camera.transform.position.y + 0.1f, -3.0f, 3.0f);
        }
        else if (Input.GetKey("s"))
        {
            posY = Mathf.Clamp(camera.transform.position.y - 0.1f, -3.0f, 3.0f);
        }

        if (Input.GetKey("a"))
        {
            posX = Mathf.Clamp(camera.transform.position.x - 0.1f, -3.0f, 3.0f);
        }
        if (Input.GetKey("d"))
        {
            posX = Mathf.Clamp(camera.transform.position.x + 0.1f, -3.0f, 3.0f);
        }

        camera.transform.position = new Vector3(posX, posY, -10f);
    }

}
