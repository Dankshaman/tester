using System;
using UnityEngine;

// Token: 0x020000A7 RID: 167
public class BoardScript : MonoBehaviour
{
	// Token: 0x0600084C RID: 2124 RVA: 0x0003A967 File Offset: 0x00038B67
	private void Awake()
	{
		base.GetComponent<NetworkPhysicsObject>().IsLocked = true;
	}
}
