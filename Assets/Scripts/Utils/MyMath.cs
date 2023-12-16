using UnityEngine;

public class MyMath
{
	public static Vector2 Rotate(Vector2 v, float degrees)
	{
		float rads = degrees * Mathf.Deg2Rad;
		return new Vector2(
			v.x * Mathf.Cos(rads) - v.y * Mathf.Sin(rads),
			v.x * Mathf.Sin(rads) + v.y * Mathf.Cos(rads)
		);
	}
}