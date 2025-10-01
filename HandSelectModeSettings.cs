using System;

// Token: 0x02000122 RID: 290
public class HandSelectModeSettings
{
	// Token: 0x06000F39 RID: 3897 RVA: 0x00068370 File Offset: 0x00066570
	public HandSelectModeSettings()
	{
		this.preset = null;
		this.label = "";
		this.showCancel = false;
		this.minCount = 0;
		this.maxCount = 0;
		this.prompt = "";
	}

	// Token: 0x06000F3A RID: 3898 RVA: 0x000683AA File Offset: 0x000665AA
	public HandSelectModeSettings(string label, bool showCancel, int minCount, int maxCount, string prompt = "")
	{
		this.preset = null;
		this.label = label;
		this.showCancel = showCancel;
		this.minCount = minCount;
		this.maxCount = maxCount;
		this.prompt = prompt;
	}

	// Token: 0x06000F3B RID: 3899 RVA: 0x000683DE File Offset: 0x000665DE
	public HandSelectModeSettings(HandSelectModePreset preset, string label, bool showCancel, int minCount, int maxCount, string prompt = "")
	{
		this.preset = preset;
		this.label = label;
		this.showCancel = showCancel;
		this.minCount = minCount;
		this.maxCount = maxCount;
		this.prompt = prompt;
	}

	// Token: 0x0400097E RID: 2430
	public HandSelectModePreset preset;

	// Token: 0x0400097F RID: 2431
	public string label;

	// Token: 0x04000980 RID: 2432
	public bool showCancel;

	// Token: 0x04000981 RID: 2433
	public int minCount;

	// Token: 0x04000982 RID: 2434
	public int maxCount;

	// Token: 0x04000983 RID: 2435
	public string prompt;
}
