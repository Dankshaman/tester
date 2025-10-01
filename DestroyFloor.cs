using System;
using NewNet;
using UnityEngine;

// Token: 0x020000FB RID: 251
public class DestroyFloor : MonoBehaviour
{
	// Token: 0x06000C67 RID: 3175 RVA: 0x000547F8 File Offset: 0x000529F8
	private void OnTriggerEnter(Collider Other)
	{
		if (Network.isServer)
		{
			NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromCollider(Other);
			if (networkPhysicsObject && !networkPhysicsObject.tableScript && !networkPhysicsObject.IsDestroyed)
			{
				Debug.Log("Destroyed this " + networkPhysicsObject.name);
				NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(networkPhysicsObject);
			}
		}
	}
}
