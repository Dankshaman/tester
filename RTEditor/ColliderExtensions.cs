using System;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x020003F4 RID: 1012
	public static class ColliderExtensions
	{
		// Token: 0x06002E6C RID: 11884 RVA: 0x0013ED54 File Offset: 0x0013CF54
		public static bool RaycastReverseIfFail(this Collider collider, Ray ray, out RaycastHit rayHit)
		{
			Vector3 origin = ray.origin;
			ray.origin -= ray.direction * 0.1f;
			if (collider.Raycast(ray, out rayHit, 3.4028235E+38f))
			{
				return true;
			}
			ray.direction *= -1f;
			ray.origin = origin - ray.direction * 0.1f;
			return collider.Raycast(ray, out rayHit, float.MaxValue);
		}
	}
}
