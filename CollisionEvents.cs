using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000BA RID: 186
public class CollisionEvents : MonoBehaviour
{
	// Token: 0x0600093E RID: 2366 RVA: 0x000421E6 File Offset: 0x000403E6
	public void ReqisterForCollisionStay(Component component)
	{
		this.registerdForCollisionStay.TryAddUnique(component);
		this.bOnCollisionStay = (this.registerdForCollisionStay.Count > 0);
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x00042209 File Offset: 0x00040409
	public void UnreqisterForCollisionStay(Component component)
	{
		this.registerdForCollisionStay.Remove(component);
		this.bOnCollisionStay = (this.registerdForCollisionStay.Count > 0);
	}

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x06000940 RID: 2368 RVA: 0x0004222C File Offset: 0x0004042C
	// (set) Token: 0x06000941 RID: 2369 RVA: 0x0004223C File Offset: 0x0004043C
	public bool bOnCollisionStay
	{
		get
		{
			return this.collisionEventStay != null;
		}
		private set
		{
			if (value)
			{
				if (!this.collisionEventStay)
				{
					this.collisionEventStay = base.gameObject.AddComponent<CollisionEventStay>();
					this.collisionEventStay.collisionEvents = this;
					return;
				}
			}
			else if (this.collisionEventStay)
			{
				UnityEngine.Object.Destroy(this.collisionEventStay);
			}
		}
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x0004228F File Offset: 0x0004048F
	private void OnCollisionEnter(Collision info)
	{
		this.TriggerCollisionEnter(info);
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x00042298 File Offset: 0x00040498
	private void OnCollisionExit(Collision info)
	{
		this.TriggerCollisionExit(info);
	}

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06000944 RID: 2372 RVA: 0x000422A4 File Offset: 0x000404A4
	// (remove) Token: 0x06000945 RID: 2373 RVA: 0x000422DC File Offset: 0x000404DC
	public event CollisionEvents.CollisionEnter OnCollisionEnterEvent;

	// Token: 0x06000946 RID: 2374 RVA: 0x00042311 File Offset: 0x00040511
	private void TriggerCollisionEnter(Collision info)
	{
		CollisionEvents.CollisionEnter onCollisionEnterEvent = this.OnCollisionEnterEvent;
		if (onCollisionEnterEvent == null)
		{
			return;
		}
		onCollisionEnterEvent(info);
	}

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x06000947 RID: 2375 RVA: 0x00042324 File Offset: 0x00040524
	// (remove) Token: 0x06000948 RID: 2376 RVA: 0x0004235C File Offset: 0x0004055C
	public event CollisionEvents.CollisionExit OnCollisionExitEvent;

	// Token: 0x06000949 RID: 2377 RVA: 0x00042391 File Offset: 0x00040591
	private void TriggerCollisionExit(Collision info)
	{
		CollisionEvents.CollisionExit onCollisionExitEvent = this.OnCollisionExitEvent;
		if (onCollisionExitEvent == null)
		{
			return;
		}
		onCollisionExitEvent(info);
	}

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x0600094A RID: 2378 RVA: 0x000423A4 File Offset: 0x000405A4
	// (remove) Token: 0x0600094B RID: 2379 RVA: 0x000423DC File Offset: 0x000405DC
	public event CollisionEvents.CollisionStay OnCollisionStayEvent;

	// Token: 0x0600094C RID: 2380 RVA: 0x00042411 File Offset: 0x00040611
	public void TriggerCollisionStay(Collision info)
	{
		CollisionEvents.CollisionStay onCollisionStayEvent = this.OnCollisionStayEvent;
		if (onCollisionStayEvent == null)
		{
			return;
		}
		onCollisionStayEvent(info);
	}

	// Token: 0x0400068F RID: 1679
	private CollisionEventStay collisionEventStay;

	// Token: 0x04000690 RID: 1680
	private readonly List<Component> registerdForCollisionStay = new List<Component>();

	// Token: 0x0200058C RID: 1420
	// (Invoke) Token: 0x06003863 RID: 14435
	public delegate void CollisionEnter(Collision info);

	// Token: 0x0200058D RID: 1421
	// (Invoke) Token: 0x06003867 RID: 14439
	public delegate void CollisionExit(Collision info);

	// Token: 0x0200058E RID: 1422
	// (Invoke) Token: 0x0600386B RID: 14443
	public delegate void CollisionStay(Collision info);
}
