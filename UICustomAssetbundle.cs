using System;
using System.Collections.Generic;
using NewNet;

// Token: 0x020002AF RID: 687
public class UICustomAssetbundle : UICustomObject<UICustomAssetbundle>
{
	// Token: 0x1700045B RID: 1115
	// (get) Token: 0x06002224 RID: 8740 RVA: 0x000F59ED File Offset: 0x000F3BED
	// (set) Token: 0x06002225 RID: 8741 RVA: 0x000F59FA File Offset: 0x000F3BFA
	private string CustomAssetbundleURL
	{
		get
		{
			return this.AssetbundleInput.value;
		}
		set
		{
			this.AssetbundleInput.value = value;
		}
	}

	// Token: 0x1700045C RID: 1116
	// (get) Token: 0x06002226 RID: 8742 RVA: 0x000F5A08 File Offset: 0x000F3C08
	// (set) Token: 0x06002227 RID: 8743 RVA: 0x000F5A15 File Offset: 0x000F3C15
	private string CustomAssetbundleSecondaryURL
	{
		get
		{
			return this.AssetbundleSencondaryInput.value;
		}
		set
		{
			this.AssetbundleSencondaryInput.value = value;
		}
	}

	// Token: 0x1700045D RID: 1117
	// (get) Token: 0x06002228 RID: 8744 RVA: 0x000F5A24 File Offset: 0x000F3C24
	// (set) Token: 0x06002229 RID: 8745 RVA: 0x000F5A58 File Offset: 0x000F3C58
	private int TypeInt
	{
		get
		{
			for (int i = 0; i < this.TypeToggles.Length; i++)
			{
				if (this.TypeToggles[i].value)
				{
					return i;
				}
			}
			return 0;
		}
		set
		{
			int group = this.TypeToggles[0].group;
			for (int i = 0; i < this.TypeToggles.Length; i++)
			{
				this.TypeToggles[i].group = 0;
				this.TypeToggles[i].value = (i == value);
				this.TypeToggles[i].group = group;
			}
		}
	}

	// Token: 0x1700045E RID: 1118
	// (get) Token: 0x0600222A RID: 8746 RVA: 0x000F5AB3 File Offset: 0x000F3CB3
	// (set) Token: 0x0600222B RID: 8747 RVA: 0x000F5AD0 File Offset: 0x000F3CD0
	private int MaterialInt
	{
		get
		{
			return this.MaterialPopupList.items.IndexOf(this.MaterialPopupList.value);
		}
		set
		{
			this.MaterialPopupList.value = this.MaterialPopupList.items[value];
		}
	}

	// Token: 0x0600222C RID: 8748 RVA: 0x000F5AF0 File Offset: 0x000F3CF0
	protected override void OnEnable()
	{
		base.OnEnable();
		this.TargetCustomAssetbundle = this.TargetCustomObject.GetComponent<CustomAssetbundle>();
		if (!this.TargetCustomAssetbundle)
		{
			return;
		}
		this.MaterialPopupList.items = new List<string>(CustomMesh.MaterialList);
		this.AssetbundleInput.SelectAllTextOnClick = true;
		this.AssetbundleSencondaryInput.SelectAllTextOnClick = true;
		this.CustomAssetbundleURL = this.TargetCustomAssetbundle.CustomAssetbundleURL;
		this.CustomAssetbundleSecondaryURL = this.TargetCustomAssetbundle.CustomAssetbundleSecondaryURL;
		this.TypeInt = this.TargetCustomAssetbundle.TypeInt;
		this.MaterialInt = this.TargetCustomAssetbundle.MaterialInt;
	}

	// Token: 0x0600222D RID: 8749 RVA: 0x000F5B94 File Offset: 0x000F3D94
	public override void Import()
	{
		this.CustomAssetbundleURL = this.CustomAssetbundleURL.Trim();
		this.CustomAssetbundleSecondaryURL = this.CustomAssetbundleSecondaryURL.Trim();
		if (string.IsNullOrEmpty(this.CustomAssetbundleURL))
		{
			Chat.LogError("You must supply a custom assetbundle URL.", true);
			return;
		}
		if (Network.isServer)
		{
			base.CheckUpdateMatchingCustomObjects();
			this.TargetCustomAssetbundle.CustomAssetbundleURL = this.CustomAssetbundleURL;
			this.TargetCustomAssetbundle.CustomAssetbundleSecondaryURL = this.CustomAssetbundleSecondaryURL;
			this.TargetCustomAssetbundle.TypeInt = this.TypeInt;
			this.TargetCustomAssetbundle.MaterialInt = this.MaterialInt;
			this.TargetCustomAssetbundle.CallCustomRPC();
		}
		base.Close();
	}

	// Token: 0x04001594 RID: 5524
	private CustomAssetbundle TargetCustomAssetbundle;

	// Token: 0x04001595 RID: 5525
	public UIInput AssetbundleInput;

	// Token: 0x04001596 RID: 5526
	public UIInput AssetbundleSencondaryInput;

	// Token: 0x04001597 RID: 5527
	public UIToggle[] TypeToggles;

	// Token: 0x04001598 RID: 5528
	public UIPopupList MaterialPopupList;
}
