using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTEditor
{
	// Token: 0x0200041A RID: 1050
	public static class PhysicsHelper
	{
		// Token: 0x06003099 RID: 12441 RVA: 0x0014CE88 File Offset: 0x0014B088
		public static Collider RaycastAllClosest(Ray ray, out RaycastHit rayHit, List<Type> allowedColliderTypes, HashSet<GameObject> ignoreObjects)
		{
			rayHit = default(RaycastHit);
			RaycastHit[] array = Physics.RaycastAll(ray, float.MaxValue);
			if (array.Length != 0)
			{
				float num = float.MaxValue;
				Collider result = null;
				foreach (RaycastHit raycastHit in array)
				{
					Collider collider = raycastHit.collider;
					if (allowedColliderTypes.Contains(collider.GetType()))
					{
						GameObject gameObject = collider.gameObject;
						if (!ignoreObjects.Contains(gameObject) && raycastHit.distance < num)
						{
							rayHit = raycastHit;
							num = raycastHit.distance;
							result = collider;
						}
					}
				}
				return result;
			}
			return null;
		}

		// Token: 0x0600309A RID: 12442 RVA: 0x0014CF20 File Offset: 0x0014B120
		public static Collider RaycastAllClosest(Ray ray, out RaycastHit rayHit)
		{
			rayHit = default(RaycastHit);
			RaycastHit[] array = Physics.RaycastAll(ray, float.MaxValue);
			if (array.Length != 0)
			{
				float num = float.MaxValue;
				Collider result = null;
				foreach (RaycastHit raycastHit in array)
				{
					Collider collider = raycastHit.collider;
					if (raycastHit.distance < num)
					{
						rayHit = raycastHit;
						num = raycastHit.distance;
						result = collider;
					}
				}
				return result;
			}
			return null;
		}

		// Token: 0x0600309B RID: 12443 RVA: 0x0014CF94 File Offset: 0x0014B194
		public static List<RaycastHit> RaycastAllSorted(Ray ray, int layerMask)
		{
			RaycastHit[] array = Physics.RaycastAll(ray, float.MaxValue, layerMask);
			if (array.Length == 0)
			{
				return new List<RaycastHit>();
			}
			List<RaycastHit> list = new List<RaycastHit>(array);
			list.Sort((RaycastHit firstHit, RaycastHit secondHit) => firstHit.distance.CompareTo(secondHit.distance));
			return list;
		}
	}
}
