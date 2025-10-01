using System;
using UnityEngine;

// Token: 0x0200014F RID: 335
public static class LibVector
{
	// Token: 0x060010F6 RID: 4342 RVA: 0x0007552E File Offset: 0x0007372E
	public static Vector3 NormalizedPosition(Vector3 objectPosition, Vector3 containerPosition, Quaternion containerRotation)
	{
		return Quaternion.Inverse(containerRotation) * (objectPosition - containerPosition);
	}

	// Token: 0x060010F7 RID: 4343 RVA: 0x00075542 File Offset: 0x00073742
	public static Quaternion NormalizedRotation(Quaternion objectRotation, Quaternion containerRotation)
	{
		return Quaternion.Inverse(containerRotation) * objectRotation;
	}

	// Token: 0x060010F8 RID: 4344 RVA: 0x00075550 File Offset: 0x00073750
	public static Vector3 TransformedPosition(Vector3 normalPosition, Vector3 containerPosition, Quaternion containerRotation)
	{
		return containerPosition + containerRotation * normalPosition;
	}

	// Token: 0x060010F9 RID: 4345 RVA: 0x0007555F File Offset: 0x0007375F
	public static Quaternion TransformedRotation(Quaternion normalRotation, Quaternion containerRotation)
	{
		return containerRotation * normalRotation;
	}

	// Token: 0x060010FA RID: 4346 RVA: 0x00075568 File Offset: 0x00073768
	public static Vector3 RecontainerPosition(Vector3 objectPosition, Vector3 fromContainerPosition, Quaternion fromContainerRotation, Vector3 toContainerPosition, Quaternion toContainerRotation)
	{
		return LibVector.TransformedPosition(LibVector.NormalizedPosition(objectPosition, fromContainerPosition, fromContainerRotation), toContainerPosition, toContainerRotation);
	}

	// Token: 0x060010FB RID: 4347 RVA: 0x0007557A File Offset: 0x0007377A
	public static Quaternion RecontainerRotation(Quaternion objectRotation, Quaternion fromContainerRotation, Quaternion toContainerRotation)
	{
		return LibVector.TransformedRotation(LibVector.NormalizedRotation(objectRotation, fromContainerRotation), toContainerRotation);
	}

	// Token: 0x060010FC RID: 4348 RVA: 0x00075589 File Offset: 0x00073789
	public static float StandardizeAngle(float angle)
	{
		while (angle < -180f)
		{
			angle += 360f;
		}
		while (angle > 180f)
		{
			angle -= 360f;
		}
		return angle;
	}

	// Token: 0x060010FD RID: 4349 RVA: 0x000755B2 File Offset: 0x000737B2
	public static Vector3 StandardizeEulerAngles(Vector3 angles)
	{
		return new Vector3(LibVector.StandardizeAngle(angles.x), LibVector.StandardizeAngle(angles.y), LibVector.StandardizeAngle(angles.z));
	}

	// Token: 0x060010FE RID: 4350 RVA: 0x000755DA File Offset: 0x000737DA
	public static Vector3 StandardizeEulerAnglesXZ(Vector3 angles)
	{
		return new Vector3(LibVector.StandardizeAngle(angles.x), LibVector.StandardizeAngle(angles.z));
	}

	// Token: 0x060010FF RID: 4351 RVA: 0x000755F7 File Offset: 0x000737F7
	public static float ComponentMax(Vector3 a)
	{
		return Mathf.Max(Mathf.Max(a.x, a.y), a.z);
	}

	// Token: 0x06001100 RID: 4352 RVA: 0x00075615 File Offset: 0x00073815
	public static Vector3 Mul(Vector3 a, Vector3 b)
	{
		return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
	}

	// Token: 0x06001101 RID: 4353 RVA: 0x00075643 File Offset: 0x00073843
	public static Vector3 Div(Vector3 a, Vector3 b)
	{
		return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
	}

	// Token: 0x06001102 RID: 4354 RVA: 0x00075671 File Offset: 0x00073871
	public static Vector4 Vector4FromPairs(float x1, float y1, float x2, float y2)
	{
		if (x2 < x1)
		{
			float num = x1;
			x1 = x2;
			x2 = num;
		}
		if (y2 < y1)
		{
			float num2 = y1;
			y1 = y2;
			y2 = num2;
		}
		return new Vector4(x1, y1, x2, y2);
	}

	// Token: 0x06001103 RID: 4355 RVA: 0x00075690 File Offset: 0x00073890
	public static Vector3 nearestPointOnNPO(Vector3 from, NetworkPhysicsObject npo)
	{
		Vector3 vector = npo.transform.position;
		float num = (vector - from).magnitude;
		foreach (Collider collider in npo.GetComponentsInChildren<Collider>())
		{
			if (collider.enabled && !npo.TriggerOnlyColliders.Contains(collider))
			{
				if (collider.bounds.Contains(from))
				{
					return Vector3.zero;
				}
				Vector3 vector2 = collider.ClosestPoint(from);
				float magnitude = (vector2 - from).magnitude;
				if (magnitude < num)
				{
					num = magnitude;
					vector = vector2;
				}
			}
		}
		return vector;
	}

	// Token: 0x06001104 RID: 4356 RVA: 0x00075734 File Offset: 0x00073934
	public static Vector3 level(Vector3 position, Vector3 to)
	{
		Vector3 result = position;
		result.y = to.y;
		return result;
	}

	// Token: 0x06001105 RID: 4357 RVA: 0x00075754 File Offset: 0x00073954
	public static bool SameWayUp(Vector3 rotation1, Vector3 rotation2)
	{
		bool flag = LibMath.CloseEnoughDegrees(rotation1.z, 0f, 90f);
		bool flag2 = LibMath.CloseEnoughDegrees(rotation2.z, 0f, 90f);
		return flag == flag2;
	}
}
