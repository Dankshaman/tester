using System;
using NewNet;
using UnityEngine;

// Token: 0x020000B6 RID: 182
public class CheckStackObject : MonoBehaviour
{
	// Token: 0x0600090E RID: 2318 RVA: 0x0004166A File Offset: 0x0003F86A
	private void Awake()
	{
		this.NPO = base.GetComponent<NetworkPhysicsObject>();
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x00041678 File Offset: 0x0003F878
	public bool CheckStackable(NetworkPhysicsObject npo)
	{
		return !(npo.InternalName != this.NPO.InternalName) && !npo.stackObject && StackObject.CheckStackable(npo, this.NPO);
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x000416B0 File Offset: 0x0003F8B0
	private void OnCollisionEnter(Collision info)
	{
		if (Network.isServer)
		{
			GameObject grabbable = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromCollider(info.collider);
			NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGO(grabbable);
			if (networkPhysicsObject == null || networkPhysicsObject.CurrentPlayerHand || this.NPO.CurrentPlayerHand)
			{
				return;
			}
			if (this.CheckStackable(networkPhysicsObject) && this.NPO.IsHeldByNobody && networkPhysicsObject.IsHeldByNobody)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.NPOHitNPO(networkPhysicsObject, this.NPO, true);
			}
		}
	}

	// Token: 0x04000681 RID: 1665
	private NetworkPhysicsObject NPO;
}
