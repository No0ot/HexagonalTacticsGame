using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public HexTile hexPrefab;
    public int mapRadius;

    List<GameObject> hexList;
    // Start is called before the first frame update
    void Start()
    {
        hexList = new List<GameObject>();
        BuildGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BuildGrid()
    {
        for (int q = -mapRadius; q <= mapRadius; q++)
        {
            int r1 = Mathf.Max(-mapRadius, -q - mapRadius);
            Debug.Log(r1);
            int r2 = Mathf.Min(mapRadius, -q + mapRadius);
            Debug.Log(r2);
            for (int r = r1; r <= r2; r++)
            {
                HexTile temp = Instantiate(hexPrefab);
                temp.coordinates = new Vector3(q, r, -q - r);
                hexList.Add(temp.gameObject);
                Vector2 position = temp.hex_to_pixel(temp.hexLayout, temp.coordinates);
                temp.gameObject.transform.position = new Vector3(position.x, position.y, 0.0f);
            }
        }
    }
}
