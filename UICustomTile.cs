using System;
using System.Collections.Generic;
using NewNet;

// Token: 0x020002BD RID: 701
public class UICustomTile : UICustomObject<UICustomTile>
{
	// Token: 0x1700047D RID: 1149
	// (get) Token: 0x060022B5 RID: 8885 RVA: 0x000F7FE4 File Offset: 0x000F61E4
	// (set) Token: 0x060022B6 RID: 8886 RVA: 0x000F7FF1 File Offset: 0x000F61F1
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

	// Token: 0x1700047E RID: 1150
	// (get) Token: 0x060022B7 RID: 8887 RVA: 0x000F7FFF File Offset: 0x000F61FF
	// (set) Token: 0x060022B8 RID: 8888 RVA: 0x000F800C File Offset: 0x000F620C
	private string CustomImageSecondaryURL
	{
		get
		{
			return this.ImageInput2.value;
		}
		set
		{
			this.ImageInput2.value = value;
		}
	}

	// Token: 0x1700047F RID: 1151
	// (get) Token: 0x060022B9 RID: 8889 RVA: 0x000F801C File Offset: 0x000F621C
	// (set) Token: 0x060022BA RID: 8890 RVA: 0x000F8058 File Offset: 0x000F6258
	private int TypeInt
	{
		get
		{
			for (int i = 0; i < this.TileToggles.Count; i++)
			{
				if (this.TileToggles[i].value)
				{
					return i;
				}
			}
			return 0;
		}
		set
		{
			int group = this.TileToggles[0].group;
			for (int i = 0; i < this.TileToggles.Count; i++)
			{
				this.TileToggles[i].group = 0;
				this.TileToggles[i].value = (i == value);
				this.TileToggles[i].group = group;
			}
		}
	}

	// Token: 0x060022BB RID: 8891 RVA: 0x000F80C8 File Offset: 0x000F62C8
	protected override void OnEnable()
	{
		base.OnEnable();
		this.TargetCustomTile = this.TargetCustomObject.GetComponent<CustomTile>();
		if (!this.TargetCustomTile)
		{
			return;
		}
		this.ImageInput.SelectAllTextOnClick = true;
		this.ImageInput2.SelectAllTextOnClick = true;
		this.CustomImageURL = this.TargetCustomTile.CustomImageURL;
		this.CustomImageSecondaryURL = this.TargetCustomTile.CustomImageSecondaryURL;
		this.ThicknessSlider.value = (this.TargetCustomTile.Thickness - 0.1f) / 0.9f;
		this.StackableToggle.value = this.TargetCustomTile.bStackable;
		this.StretchToggle.value = this.TargetCustomTile.bStretch;
		this.TypeInt = (int)this.TargetCustomTile.CurrentTileType;
	}

	// Token: 0x060022BC RID: 8892 RVA: 0x000F8194 File Offset: 0x000F6394
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
			this.TargetCustomTile.CustomImageURL = this.CustomImageURL;
			this.TargetCustomTile.CustomImageSecondaryURL = this.CustomImageSecondaryURL;
			this.TargetCustomTile.Thickness = 0.9f * this.ThicknessSlider.value + 0.1f;
			this.TargetCustomTile.CurrentTileType = (TileType)this.TypeInt;
			this.TargetCustomTile.bStackable = this.StackableToggle.value;
			this.TargetCustomTile.bStretch = this.StretchToggle.value;
			this.TargetCustomTile.CallCustomRPC();
		}
		base.Close();
	}

	// Token: 0x040015EE RID: 5614
	public List<UIToggle> TileToggles;

	// Token: 0x040015EF RID: 5615
	public UIInput ImageInput;

	// Token: 0x040015F0 RID: 5616
	public UIInput ImageInput2;

	// Token: 0x040015F1 RID: 5617
	public UISlider ThicknessSlider;

	// Token: 0x040015F2 RID: 5618
	public UIToggle StackableToggle;

	// Token: 0x040015F3 RID: 5619
	public UIToggle StretchToggle;

	// Token: 0x040015F4 RID: 5620
	private CustomTile TargetCustomTile;
}
