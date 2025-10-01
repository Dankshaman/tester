using System;
using UnityEngine;

// Token: 0x020000B9 RID: 185
public class CollisionEventStay : MonoBehaviour
{
	// Token: 0x0600093C RID: 2364 RVA: 0x000421D8 File Offset: 0x000403D8
	private void OnCollisionStay(Collision info)
	{
		this.collisionEvents.TriggerCollisionStay(info);
	}

	// Token: 0x0400068E RID: 1678
	public CollisionEvents collisionEvents;
}
