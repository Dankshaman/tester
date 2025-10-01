using System;
using NewNet;

// Token: 0x020002BE RID: 702
public class UICustomToken : UICustomObject<UICustomToken>
{
	// Token: 0x17000480 RID: 1152
	// (get) Token: 0x060022BE RID: 8894 RVA: 0x000F8275 File Offset: 0x000F6475
	// (set) Token: 0x060022BF RID: 8895 RVA: 0x000F8282 File Offset: 0x000F6482
	private string CustomImageURL
	{
		get
		{
			return this.ImageInput.value;
		}
		set
		{
			this.ImageInput.value = value;
		}
	}

	// Token: 0x060022C0 RID: 8896 RVA: 0x000F8290 File Offset: 0x000F6490
	protected override void OnEnable()
	{
		base.OnEnable();
		this.TargetCustomToken = this.TargetCustomObject.GetComponent<CustomToken>();
		if (!this.TargetCustomToken)
		{
			return;
		}
		this.ImageInput.SelectAllTextOnClick = true;
		this.CustomImageURL = this.TargetCustomToken.CustomImageURL;
		this.ThicknessSlider.value = (this.TargetCustomToken.Thickness - 0.1f) / 0.9f;
		this.MergeDistanceSlider.value = (this.TargetCustomToken.MergeDistancePixels - 5f) / 20f;
		this.StandupToggle.value = this.TargetCustomToken.bStandUp;
		this.StackableToggle.value = this.TargetCustomToken.bStackable;
	}

	// Token: 0x060022C1 RID: 8897 RVA: 0x000F8350 File Offset: 0x000F6550
	public override void Import()
	{
		this.CustomImageURL = this.CustomImageURL.Trim();
		if (string.IsNullOrEmpty(this.CustomImageURL))
		{
			Chat.LogError("You must supply a custom image URL.", true);
			return;
		}
		if (Network.isServer)
		{
			base.CheckUpdateMatchingCustomObjects();
			this.TargetCustomToken.CustomImageURL = this.CustomImageURL;
			this.TargetCustomToken.Thickness = 0.9f * this.ThicknessSlider.value + 0.1f;
			this.TargetCustomToken.MergeDistancePixels = 20f * this.MergeDistanceSlider.value + 5f;
			this.TargetCustomToken.bStandUp = this.StandupToggle.value;
			this.TargetCustomToken.bStackable = this.StackableToggle.value;
			this.TargetCustomToken.CallCustomRPC();
		}
		base.Close();
	}

	// Token: 0x040015F5 RID: 5621
	public UIInput ImageInput;

	// Token: 0x040015F6 RID: 5622
	public UISlider ThicknessSlider;

	// Token: 0x040015F7 RID: 5623
	public UISlider MergeDistanceSlider;

	// Token: 0x040015F8 RID: 5624
	public UIToggle StandupToggle;

	// Token: 0x040015F9 RID: 5625
	public UIToggle StackableToggle;

	// Token: 0x040015FA RID: 5626
	private CustomToken TargetCustomToken;
}
