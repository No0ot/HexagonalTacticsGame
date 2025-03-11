using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TileSelector : MonoBehaviour
{
    public LayerMask tileLayerMask;
    public TMP_Text tileInfoText;  // Reference to the UI Text component
    private Camera mainCamera;
    private GameObject currentTile;

    public UnityEvent<HexTile> hexTileSelected;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleTileSelection();
    }

    private void HandleTileSelection()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log("Clicked on the UI");
            return;
        }
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Check if the raycast hits a tile using the LayerMask
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayerMask))
        {
            GameObject tile = hit.transform.gameObject;

            //Debug.Log(tile.layer);

            if (tile.layer != LayerMask.NameToLayer("Hex"))
                return;

            // Check if we're hovering over a new tile
            if (currentTile != tile)
            {
                // Hide the previous outline
                RemoveOutline(currentTile);

                // Set the new current tile and show its outline
                currentTile = tile;
                ShowOutline(currentTile);
            }

            // Check for tile selection on mouse click
            if (Input.GetMouseButtonDown(0)) // Left mouse button
            {

                SelectTile(tile);
            }
        }
        else
        {
            // If not hovering over a tile, remove the outline
            RemoveOutline(currentTile);
            currentTile = null;
        }
    }

    private void ShowOutline(GameObject tile)
    {
        HexTileHighlight highlight = tile.GetComponentInParent<HexTileHighlight>();
        if (highlight != null)
        {
            highlight.ShowRay(); // Show the outline
        }

        if (BattleManager.Instance)
        {
            if (tile.GetComponentInParent<HexTile>().Occupant)
            {
                UIManager.Instance.selectedUnitProfile.UpdateProfile(tile.GetComponentInParent<HexTile>().Occupant);
            }
            else
                UIManager.Instance.selectedUnitProfile.UpdateProfile(null);
        }
    }

    private void RemoveOutline(GameObject tile)
    {
        if (tile != null)
        {
            HexTileHighlight highlight = tile.GetComponentInParent<HexTileHighlight>();
            if (highlight != null)
            {
                highlight.HideRay(); // Remove highlight
            }
        }
    }

    private void SelectTile(GameObject tile)
    {
        // Implement tile selection logic here
        HexTile hex = tile.GetComponentInParent<HexTile>();
        if (hex)
        {
            Debug.Log($"Tile {hex.AxialCoordinates} selected!");
            hexTileSelected.Invoke(hex);
        }
        else
        {
            Debug.Log("Cant find Component");
        }
        //tile.GetComponentInParent<HexTileHighlight>().ShowOutline(Color.blue);
        // You can add additional logic for the selected tile
    }
    void DisplayTileInfo(HexTileHighlight hexTile)
    {
        // Access the HexTile component from the highlight script
        HexTile tile = hexTile.GetComponent<HexTile>();

        if (tile != null)
        {
            tileInfoText.text = $"Position: {tile.AxialCoordinates}\n" +
                                $"Height: {tile.Height}\n" +
                                $"Terrain: {tile.Type}\n" +
                                $"Movement Cost: {tile.MovementCost}";
        }
    }
}