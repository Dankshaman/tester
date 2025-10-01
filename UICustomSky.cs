using System;
using NewNet;

// Token: 0x020002BC RID: 700
public class UICustomSky : UICustomObject<UICustomSky>
{
	// Token: 0x1700047C RID: 1148
	// (get) Token: 0x060022B0 RID: 8880 RVA: 0x000F7F0F File Offset: 0x000F610F
	// (set) Token: 0x060022B1 RID: 8881 RVA: 0x000F7F1C File Offset: 0x000F611C
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

	// Token: 0x060022B2 RID: 8882 RVA: 0x000F7F2C File Offset: 0x000F612C
	protected override void OnEnable()
	{
		base.OnEnable();
		this.TargetCustomSky = this.TargetCustomObject.GetComponent<CustomSky>();
		if (!this.TargetCustomSky)
		{
			return;
		}
		this.ImageInput.SelectAllTextOnClick = true;
		this.CustomImageURL = this.TargetCustomSky.CustomSkyURL;
	}

	// Token: 0x060022B3 RID: 8883 RVA: 0x000F7F7C File Offset: 0x000F617C
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
			this.TargetCustomSky.CustomSkyURL = this.CustomImageURL;
			this.TargetCustomSky.CallCustomRPC();
		}
		base.Close();
	}

	// Token: 0x040015EC RID: 5612
	private CustomSky TargetCustomSky;

	// Token: 0x040015ED RID: 5613
	public UIInput ImageInput;
}
