using System;
using UnityEngine;

// Token: 0x020002BF RID: 703
public class UICustomUIAsset : Singleton<UICustomUIAsset>
{
	// Token: 0x17000481 RID: 1153
	// (get) Token: 0x060022C3 RID: 8899 RVA: 0x000F8434 File Offset: 0x000F6634
	// (set) Token: 0x060022C4 RID: 8900 RVA: 0x000F8468 File Offset: 0x000F6668
	private CustomAssetType AssetType
	{
		get
		{
			for (int i = 0; i < this.TypeToggles.Length; i++)
			{
				if (this.TypeToggles[i].value)
				{
					return (CustomAssetType)i;
				}
			}
			return CustomAssetType.Image;
		}
		set
		{
			int group = this.TypeToggles[0].group;
			for (int i = 0; i < this.TypeToggles.Length; i++)
			{
				this.TypeToggles[i].group = 0;
				this.TypeToggles[i].value = (i == (int)value);
				this.TypeToggles[i].group = group;
			}
		}
	}

	// Token: 0x060022C5 RID: 8901 RVA: 0x000F84C5 File Offset: 0x000F66C5
	protected override void Awake()
	{
		base.Awake();
		EventDelegate.Add(this.ImportButton.onClick, new EventDelegate.Callback(this.Import));
	}

	// Token: 0x060022C6 RID: 8902 RVA: 0x000F84EA File Offset: 0x000F66EA
	private void OnDestroy()
	{
		EventDelegate.Remove(this.ImportButton.onClick, new EventDelegate.Callback(this.Import));
	}

	// Token: 0x060022C7 RID: 8903 RVA: 0x000F8509 File Offset: 0x000F6709
	private void OnEnable()
	{
		this.targetXmlUI = UnityEngine.Object.FindObjectOfType<UIGridMenuUIAssets>().targetXmlUI;
	}

	// Token: 0x060022C8 RID: 8904 RVA: 0x000F851B File Offset: 0x000F671B
	private void OnDisable()
	{
		this.OverrideCustomAsset = null;
	}

	// Token: 0x060022C9 RID: 8905 RVA: 0x000F8524 File Offset: 0x000F6724
	public void Override(CustomAssetState customAsset)
	{
		this.OverrideCustomAsset = customAsset;
		this.NameInput.value = customAsset.Name;
		this.AssetType = customAsset.Type;
		this.URLInput.value = customAsset.URL;
		base.gameObject.SetActive(true);
	}

	// Token: 0x060022CA RID: 8906 RVA: 0x000F8574 File Offset: 0x000F6774
	private void Import()
	{
		if (string.IsNullOrEmpty(this.NameInput.value))
		{
			Chat.LogError("No name provided.", true);
			return;
		}
		if (string.IsNullOrEmpty(this.URLInput.value))
		{
			Chat.LogError("No url provided.", true);
			return;
		}
		CustomAssetState customAssetState = new CustomAssetState();
		customAssetState.Name = this.NameInput.value;
		customAssetState.Type = this.AssetType;
		customAssetState.URL = this.URLInput.value;
		if (this.OverrideCustomAsset != null)
		{
			this.targetXmlUI.UpdateCustomAsset(this.OverrideCustomAsset, customAssetState);
		}
		else
		{
			this.targetXmlUI.AddCustomAsset(customAssetState);
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x040015FB RID: 5627
	public UIInput NameInput;

	// Token: 0x040015FC RID: 5628
	public UIToggle[] TypeToggles;

	// Token: 0x040015FD RID: 5629
	public UIInput URLInput;

	// Token: 0x040015FE RID: 5630
	public UIButton ImportButton;

	// Token: 0x040015FF RID: 5631
	private CustomAssetState OverrideCustomAsset;

	// Token: 0x04001600 RID: 5632
	private XmlUIScript targetXmlUI;
}
