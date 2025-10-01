using System;
using NewNet;
using UnityEngine;

// Token: 0x02000227 RID: 551
public class ScriptingZone : Zone
{
	// Token: 0x06001B63 RID: 7011 RVA: 0x000BCCE0 File Offset: 0x000BAEE0
	protected override void Awake()
	{
		base.Awake();
		NetworkEvents.OnDisconnectedFromServer += this.OnDisconnect;
	}

	// Token: 0x06001B64 RID: 7012 RVA: 0x000BCCF9 File Offset: 0x000BAEF9
	protected override void OnDestroy()
	{
		base.OnDestroy();
		NetworkEvents.OnDisconnectedFromServer -= this.OnDisconnect;
	}

	// Token: 0x06001B65 RID: 7013 RVA: 0x000BCD14 File Offset: 0x000BAF14
	private void OnTriggerEnter(Collider otherCollider)
	{
		NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromCollider(otherCollider);
		if (networkPhysicsObject == null)
		{
			return;
		}
		base.AddObject(otherCollider, networkPhysicsObject);
	}

	// Token: 0x06001B66 RID: 7014 RVA: 0x000BCD40 File Offset: 0x000BAF40
	private void OnTriggerExit(Collider otherCollider)
	{
		NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromCollider(otherCollider);
		if (networkPhysicsObject == null)
		{
			return;
		}
		base.RemoveObject(otherCollider, networkPhysicsObject);
	}

	// Token: 0x06001B67 RID: 7015 RVA: 0x000BCD6B File Offset: 0x000BAF6B
	protected override bool ValidateAddObject(NetworkPhysicsObject npo)
	{
		return !npo.IsDestroyed && !npo.zone && !npo.tableScript && base.NPO.TagsAllowActingUpon(npo);
	}

	// Token: 0x06001B68 RID: 7016 RVA: 0x000BCD9D File Offset: 0x000BAF9D
	private void OnDisconnect(DisconnectInfo info)
	{
		base.gameObject.SetActive(false);
	}
}
