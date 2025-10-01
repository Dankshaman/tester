using System;
using UnityEngine;

// Token: 0x02000119 RID: 281
public class GenerateCollidersFromJson : MonoBehaviour
{
	// Token: 0x06000EFF RID: 3839 RVA: 0x0006629F File Offset: 0x0006449F
	private void Awake()
	{
		BoxColliderState.CreateBoxColliders(base.gameObject, Json.Load<BoxColliderState[]>(this.ColliderStateJson.text));
		base.GetComponent<NetworkPhysicsObject>().ResetBounds();
	}

	// Token: 0x04000951 RID: 2385
	public TextAsset ColliderStateJson;
}
