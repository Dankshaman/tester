using System;
using UnityEngine;

// Token: 0x02000316 RID: 790
public class UIPhysicsMode : MonoBehaviour
{
	// Token: 0x06002667 RID: 9831 RVA: 0x001122E4 File Offset: 0x001104E4
	private void OnEnable()
	{
		int group = this.Full.group;
		this.Full.group = 0;
		this.SemiLock.group = 0;
		this.Locked.group = 0;
		this.Full.value = (!ServerOptions.isPhysicsSemi && !ServerOptions.isPhysicsLock);
		this.SemiLock.value = ServerOptions.isPhysicsSemi;
		this.Locked.value = ServerOptions.isPhysicsLock;
		this.Full.group = group;
		this.SemiLock.group = group;
		this.Locked.group = group;
	}

	// Token: 0x0400190C RID: 6412
	public UIToggle Full;

	// Token: 0x0400190D RID: 6413
	public UIToggle SemiLock;

	// Token: 0x0400190E RID: 6414
	public UIToggle Locked;
}
