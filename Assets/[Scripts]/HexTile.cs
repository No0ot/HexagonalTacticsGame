using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexTile : MonoBehaviour
{
    public Vector3 coordinates;
	//References
	public GameObject selector;



	//Layout and Orientation stuff
	static Orientation pointyOrientation = new Orientation( Mathf.Sqrt(3.0f), Mathf.Sqrt(3.0f) / 2.0f, 0.0f, 3.0f / 2.0f,
															Mathf.Sqrt(3.0f) / 3.0f, -1.0f / 3.0f, 0.0f, 2.0f / 3.0f,
															0.5f);
	public Layout hexLayout = new Layout(pointyOrientation, new Vector2(0.45f, 0.4f), new Vector2(0, 0));
	//Methods

	public Unit occupant;
	public Vector2 hex_to_pixel( Vector3 h)
	{
		Orientation M = hexLayout.orientation;
		float x = (M.f0 * h.x + M.f1 * h.y) * hexLayout.size.x;
		float y = (M.f2 * h.x + M.f3 * h.y) * hexLayout.size.y;
		return new Vector2(x + hexLayout.origin.x, y + hexLayout.origin.y);
	}

    private void OnMouseEnter()
    {
		selector.SetActive(true);
	}
    private void OnMouseExit()
    {
		selector.SetActive(false);
	}

    private void OnMouseDown()
    {
		BattleManager.Instance.selectHex?.Invoke(this);
	}
}
