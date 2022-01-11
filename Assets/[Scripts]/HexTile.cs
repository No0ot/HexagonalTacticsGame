using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Orientation
{
	public float f0, f1, f2, f3;
	public float b0, b1, b2, b3;
	public float start_angle; // in multiples of 60°
	public Orientation(float f0_, float f1_, float f2_, float f3_,
		float b0_, float b1_, float b2_, float b3_,
		float start_angle_)
	{
		f0 = f0_;
		f1 = f1_;
		f2 = f2_;
		f3 = f3_;
		b0 = b0_;
		b1 = b1_;
		b2 = b2_;
		b3 = b3_;
		start_angle = start_angle_;
	}
};

public struct Layout
{
	public Orientation orientation;
	public Vector2 size;
	public Vector2 origin;
	public Layout(Orientation temporientation, Vector2 tempsize, Vector2 temporigin)
	{
		orientation = temporientation;
		size = tempsize;
		origin = temporigin;
	}
};

public class HexTile : MonoBehaviour
{
    public Vector3 coordinates;
	static Orientation layout_pointy = new Orientation(Mathf.Sqrt(3.0f), Mathf.Sqrt(3.0f) / 2.0f, 0.0f, 3.0f / 2.0f,
				Mathf.Sqrt(3.0f) / 3.0f, -1.0f / 3.0f, 0.0f, 2.0f / 3.0f,
				0.5f);
	public Layout hexLayout = new Layout(layout_pointy, new Vector2(0.45f, 0.4f), new Vector2(0, 0));

	public Vector2 hex_to_pixel(Layout layout, Vector3 h)
	{
		Orientation M = layout.orientation;
		float x = (M.f0 * h.x + M.f1 * h.y) * layout.size.x;
		float y = (M.f2 * h.x + M.f3 * h.y) * layout.size.y;
		return new Vector2(x + layout.origin.x, y + layout.origin.y);
	}
}
