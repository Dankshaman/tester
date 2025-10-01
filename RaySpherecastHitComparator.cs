using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200012C RID: 300
public class RaySpherecastHitComparator : IComparer<RaycastHit>
{
	// Token: 0x06001006 RID: 4102 RVA: 0x0006D024 File Offset: 0x0006B224
	public int Compare(RaycastHit x, RaycastHit y)
	{
		if (x.collider == null && y.collider == null)
		{
			return 0;
		}
		if (x.collider == null)
		{
			return 1;
		}
		if (y.collider == null)
		{
			return -1;
		}
		NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromCollider(x.collider);
		NetworkPhysicsObject networkPhysicsObject2 = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromCollider(y.collider);
		if (networkPhysicsObject && !networkPhysicsObject.IsLocked && (!networkPhysicsObject2 || networkPhysicsObject2.IsLocked))
		{
			return -1;
		}
		if (networkPhysicsObject2 && !networkPhysicsObject2.IsLocked && (!networkPhysicsObject || networkPhysicsObject.IsLocked))
		{
			return 1;
		}
		if (x.barycentricCoordinate.y != y.barycentricCoordinate.y)
		{
			if (x.barycentricCoordinate.y == 1f)
			{
				return -1;
			}
			return 1;
		}
		else
		{
			if (x.distance < y.distance)
			{
				return -1;
			}
			if (y.distance < x.distance)
			{
				return 1;
			}
			return 0;
		}
	}
}
