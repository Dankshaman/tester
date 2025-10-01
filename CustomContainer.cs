using System;

// Token: 0x020000D7 RID: 215
public abstract class CustomContainer
{
	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x06000AAA RID: 2730 RVA: 0x0004B822 File Offset: 0x00049A22
	// (set) Token: 0x06000AAB RID: 2731 RVA: 0x0004B82A File Offset: 0x00049A2A
	public string url { get; protected set; }

	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x06000AAC RID: 2732 RVA: 0x0004B833 File Offset: 0x00049A33
	// (set) Token: 0x06000AAD RID: 2733 RVA: 0x0004B83B File Offset: 0x00049A3B
	public string nonCodeStrippedURL { get; protected set; }

	// Token: 0x06000AAE RID: 2734
	public abstract void Cleanup(bool forceCleanup = false);

	// Token: 0x06000AAF RID: 2735
	public abstract bool IsError();
}
