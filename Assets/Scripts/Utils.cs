using UnityEngine;

public class Utils
{
	public static void DrawCircle(Vector3 position, float range, Color color, int sides = 16)
	{
		Vector3 arm = Vector3.up * range;
		Quaternion q = Quaternion.AngleAxis(360.0f / sides, Vector3.forward);
		for (int i = 0; i < sides; i++)
		{
			Vector3 lastArm = arm;
			arm = q * arm;
			Debug.DrawLine(position + lastArm, position + arm, color);
		}
	}

	public static bool InRange(Vector3 a, Vector3 b, float range)
	{
		return (a - b).sqrMagnitude <= (range * range);
	}
}
