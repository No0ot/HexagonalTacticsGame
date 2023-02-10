using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
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

//[System.Serializable]
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

