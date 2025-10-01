using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000278 RID: 632
public class SelectionTrigger : MonoBehaviour
{
	// Token: 0x06002122 RID: 8482 RVA: 0x000EF9D8 File Offset: 0x000EDBD8
	private void OnDisable()
	{
		this.SelectionDictionary.Clear();
	}

	// Token: 0x06002123 RID: 8483 RVA: 0x000EF9E8 File Offset: 0x000EDBE8
	private void OnTriggerEnter(Collider Other)
	{
		NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromCollider(Other);
		if (!networkPhysicsObject || !networkPhysicsObject.IsGrabbable || networkPhysicsObject.IsLocked)
		{
			return;
		}
		if (!this.SelectionDictionary.ContainsKey(Other))
		{
			if (!this.SelectionDictionary.ContainsValue(networkPhysicsObject) && PlayerScript.Pointer)
			{
				PlayerScript.PointerScript.AddHighlight(networkPhysicsObject, false);
			}
			this.SelectionDictionary.Add(Other, networkPhysicsObject);
		}
	}

	// Token: 0x06002124 RID: 8484 RVA: 0x000EFA5C File Offset: 0x000EDC5C
	private void OnTriggerExit(Collider Other)
	{
		if (this.SelectionDictionary.ContainsKey(Other))
		{
			NetworkPhysicsObject networkPhysicsObject = this.SelectionDictionary[Other];
			this.SelectionDictionary.Remove(Other);
			if (!this.SelectionDictionary.ContainsValue(networkPhysicsObject) && PlayerScript.Pointer)
			{
				PlayerScript.PointerScript.RemoveHighlight(networkPhysicsObject, false);
			}
		}
	}

	// Token: 0x04001474 RID: 5236
	private Dictionary<Collider, NetworkPhysicsObject> SelectionDictionary = new Dictionary<Collider, NetworkPhysicsObject>();
}
