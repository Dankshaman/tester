using System;

namespace NewNet
{
	// Token: 0x020003AA RID: 938
	public enum NetworkHeader
	{
		// Token: 0x04001DD1 RID: 7633
		RPC,
		// Token: 0x04001DD2 RID: 7634
		VarSync,
		// Token: 0x04001DD3 RID: 7635
		Instantiate,
		// Token: 0x04001DD4 RID: 7636
		Destroy,
		// Token: 0x04001DD5 RID: 7637
		AddPlayer,
		// Token: 0x04001DD6 RID: 7638
		RemovePlayer,
		// Token: 0x04001DD7 RID: 7639
		AddAdmin,
		// Token: 0x04001DD8 RID: 7640
		RemoveAdmin,
		// Token: 0x04001DD9 RID: 7641
		ReliableSync,
		// Token: 0x04001DDA RID: 7642
		UnreliableSync,
		// Token: 0x04001DDB RID: 7643
		PointerSync,
		// Token: 0x04001DDC RID: 7644
		Ping,
		// Token: 0x04001DDD RID: 7645
		Pong
	}
}
