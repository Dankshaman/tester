using System;
using NewNet;
using UnityEngine;

// Token: 0x020001D7 RID: 471
public class PhysicsReduceY : MonoBehaviour
{
	// Token: 0x06001874 RID: 6260 RVA: 0x000A6353 File Offset: 0x000A4553
	private void Start()
	{
		if (Network.isClient)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		this.NPO = base.GetComponent<NetworkPhysicsObject>();
	}

	// Token: 0x06001875 RID: 6261 RVA: 0x000A6370 File Offset: 0x000A4570
	private void FixedUpdate()
	{
		if (this.NPO != null && this.NPO.IsHeldByNobody && !this.NPO.IsSmoothMoving && !this.NPO.rigidbody.isKinematic && !this.NPO.rigidbody.IsSleeping())
		{
			Vector3 velocity = this.NPO.rigidbody.velocity;
			if (velocity.y <= 0.01f)
			{
				return;
			}
			velocity.y = 0f;
			this.NPO.rigidbody.velocity = velocity;
		}
	}

	// Token: 0x04000EA4 RID: 3748
	private NetworkPhysicsObject NPO;
}
