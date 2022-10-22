using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public List<HexTile> hexPrefabs;
    public int mapRadius;

    public List<HexTile> hexList = new List<HexTile>();

    static List<Vector3Int> directions = new List<Vector3Int>() { new Vector3Int(1,0,-1), new Vector3Int(1,-1,0), new Vector3Int(0,-1,1), new Vector3Int(-1,0,1), new Vector3Int(-1,1,0), new Vector3Int(0,1,-1) };

    public void BuildGrid()
    {
        for (int q = -mapRadius; q <= mapRadius; q++)
        {
            int r1 = Mathf.Max(-mapRadius, -q - mapRadius);

            int r2 = Mathf.Min(mapRadius, -q + mapRadius);

            for (int r = r1; r <= r2; r++)
            {
                int rand = Random.Range(0,hexPrefabs.Count);
                HexTile temp = Instantiate(hexPrefabs[rand], this.transform);
                temp.coordinates = new Vector3Int(q, r, -q - r);
                temp.SetSprites();
                hexList.Add(temp);
                Vector2 position = temp.hex_to_pixel(temp.coordinates);
                temp.gameObject.transform.position = new Vector3(position.x, position.y, 0.0f);
            }
        }

        SetGridNeighbours();
    }

    HexTile GetHex(Vector3Int coord)
    {
        foreach(HexTile tile in hexList)
        {
            if(tile.coordinates == coord)
            {
                return tile;
            }
        }
        return null;
    }

    void SetGridNeighbours()
    {
        foreach(HexTile tile in hexList)
        {
            SetTileNeighbours(tile);
        }
    }

    void SetTileNeighbours(HexTile tile)
    {
        Vector3Int temp = tile.coordinates;

        for(int i = 0; i < 6; i++)
        {
            tile.neighbours.Add(GetHex(temp + directions[i]));
        }
    }
    
}
