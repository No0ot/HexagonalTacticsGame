using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public HexTile hexPrefab;
    public int mapRadius;

    public List<HexTile> hexList = new List<HexTile>();



    public void BuildGrid()
    {
        for (int q = -mapRadius; q <= mapRadius; q++)
        {
            int r1 = Mathf.Max(-mapRadius, -q - mapRadius);

            int r2 = Mathf.Min(mapRadius, -q + mapRadius);

            for (int r = r1; r <= r2; r++)
            {
                HexTile temp = Instantiate(hexPrefab,this.transform);
                temp.coordinates = new Vector3(q, r, -q - r);
                hexList.Add(temp);
                Vector2 position = temp.hex_to_pixel(temp.coordinates);
                temp.gameObject.transform.position = new Vector3(position.x, position.y, 0.0f);
            }
        }
    }
}
