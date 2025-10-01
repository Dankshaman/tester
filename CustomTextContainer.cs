using System;

// Token: 0x020000DC RID: 220
public class CustomTextContainer : CustomContainer
{
	// Token: 0x170001D2 RID: 466
	// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x0004BC12 File Offset: 0x00049E12
	// (set) Token: 0x06000AD6 RID: 2774 RVA: 0x0004BC1A File Offset: 0x00049E1A
	public string Text { get; private set; }

	// Token: 0x06000AD7 RID: 2775 RVA: 0x0004BC23 File Offset: 0x00049E23
	public CustomTextContainer(string url, string text)
	{
		base.url = url;
		base.nonCodeStrippedURL = Singleton<CustomLoadingManager>.Instance.Text.NonCodeStrippedFromURL(url);
		this.Text = text;
	}

	// Token: 0x06000AD8 RID: 2776 RVA: 0x0004BC4F File Offset: 0x00049E4F
	public override void Cleanup(bool forceCleanup = false)
	{
		if (!forceCleanup && DLCManager.URLisDLC(base.url))
		{
			return;
		}
		this.Text = null;
	}

	// Token: 0x06000AD9 RID: 2777 RVA: 0x0004BC69 File Offset: 0x00049E69
	public override bool IsError()
	{
		return this.Text == null;
	}
}
