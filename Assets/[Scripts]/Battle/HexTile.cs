using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LocalValueComparison : IComparer<HexTile>
{
	public int Compare(HexTile x, HexTile y)
	{
		if (x.localValue == 0 || y.localValue == 0)
			return 0;

		return x.localValue.CompareTo(y.localValue);
	}
}

public enum HexType
{
	OPEN,
	FOREST,
	ROUGH,
	IMPASSABLE,
}

public enum HighlightColor
{
	NONE,
	MOVE,
	DASH,
	THREATEN,
	ATTACK,
	SKILL,
	FACE
}

public enum HexDirection
{
	RIGHT,
	RIGHTDOWN,
	LEFTDOWN,
	LEFT,
	LEFTUP,
	RIGHTUP

}

public class HexTile : MonoBehaviour
{
    public Vector3Int coordinates;
	//References
	public GameObject selector;
	public SpriteRenderer highlight;
	public Color[] highlightColors;
	public HighlightColor currentHighlight;
	Color selectorColor;

	//Layout and Orientation stuff
	static Orientation pointyOrientation = new Orientation( Mathf.Sqrt(3.0f), Mathf.Sqrt(3.0f) / 2.0f, 0.0f, 3.0f / 2.0f,
															Mathf.Sqrt(3.0f) / 3.0f, -1.0f / 3.0f, 0.0f, 2.0f / 3.0f,
															0.5f);
	public Layout hexLayout = new Layout(pointyOrientation, new Vector2(0.45f, 0.4f), new Vector2(0, 0));
	//Methods

	public Unit occupant;

	public HexType type;

	public List<SpriteRenderer> sprites;

	public List<HexTile> neighbours = new List<HexTile>();
	public float pathfindingCost;
	public float localValue = 100;
	public float globalValue;
	public bool pathfindingVisited = false;

    private void Awake()
    {
		selectorColor = selector.GetComponent<SpriteRenderer>().color;
    }

	public float ComputeGlobalValue(Vector3Int goalcoordiante)
    {
		Vector3Int a = coordinates;
		Vector3Int b = goalcoordiante;

		float value = (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z)) / 2;

		globalValue = value;
		return globalValue;
    }

    public Vector2 hex_to_pixel( Vector3 h)
	{
		Orientation M = hexLayout.orientation;
		float x = (M.f0 * h.x + M.f1 * h.y) * hexLayout.size.x;
		float y = (M.f2 * h.x + M.f3 * h.y) * hexLayout.size.y;
		return new Vector2(x + hexLayout.origin.x, y + hexLayout.origin.y);
	}

	public void SetSprites()
    {
		foreach(SpriteRenderer s in sprites)
        {
			s.sortingOrder -= (int)coordinates.y;
        }
    }

    private void OnMouseEnter()
    {
		selector.SetActive(true);
		if(occupant)
        {
			UIManager.Instance.selectedUnitProfile.UpdateProfile(occupant);
        }
	}
    private void OnMouseExit()
    {
		selector.SetActive(false);
		UIManager.Instance.selectedUnitProfile.UpdateProfile(null);
	}

    private void OnMouseDown()
    {
		BattleManager.Instance.selectHex?.Invoke(this);
		selector.GetComponent<SpriteRenderer>().color = Color.red;
	}

    private void OnMouseUp()
    {
		selector.GetComponent<SpriteRenderer>().color = selectorColor;
	}

	public void ActivateHighlight(HighlightColor color)
    {
		currentHighlight = color;
		highlight.gameObject.SetActive(true);
		highlight.color = highlightColors[(int)color];
    }

	public void DeactivateHighlight()
    {
		highlight.gameObject.SetActive(false);
	}
}
