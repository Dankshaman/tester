using System;
using System.Collections.Generic;

// Token: 0x020002E0 RID: 736
public class UIGridMenuUIAssets : UIGridMenu
{
	// Token: 0x1700048A RID: 1162
	// (get) Token: 0x06002420 RID: 9248 RVA: 0x000FFC6E File Offset: 0x000FDE6E
	// (set) Token: 0x06002421 RID: 9249 RVA: 0x000FFC76 File Offset: 0x000FDE76
	public XmlUIScript targetXmlUI { get; private set; }

	// Token: 0x06002422 RID: 9250 RVA: 0x000FFC7F File Offset: 0x000FDE7F
	public void Show(XmlUIScript XmlUI)
	{
		this.targetXmlUI = XmlUI;
		base.gameObject.SetActive(true);
	}

	// Token: 0x06002423 RID: 9251 RVA: 0x000FFC94 File Offset: 0x000FDE94
	protected override void OnEnable()
	{
		base.OnEnable();
		this.Init();
	}

	// Token: 0x06002424 RID: 9252 RVA: 0x000FFCA2 File Offset: 0x000FDEA2
	protected override void OnDisable()
	{
		base.OnDisable();
		this.targetXmlUI = null;
	}

	// Token: 0x06002425 RID: 9253 RVA: 0x000FFCB4 File Offset: 0x000FDEB4
	private void Init()
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		List<UIGridMenu.GridButtonAsset> list = new List<UIGridMenu.GridButtonAsset>();
		List<CustomAssetState> customAssets = this.targetXmlUI.CustomAssets;
		for (int i = 0; i < customAssets.Count; i++)
		{
			CustomAssetState customAssetState = customAssets[i];
			list.Add(new UIGridMenu.GridButtonAsset
			{
				Name = customAssetState.Name,
				customAsset = customAssetState,
				ButtonHoverColor = Colour.UIBlue
			});
		}
		list.Sort((UIGridMenu.GridButtonAsset x, UIGridMenu.GridButtonAsset y) => AlphanumComparatorFast.Compare(x.Name, y.Name));
		base.Load<UIGridMenu.GridButtonAsset>(list, 1, "UI ASSETS", false, true);
	}

	// Token: 0x06002426 RID: 9254 RVA: 0x000FFD63 File Offset: 0x000FDF63
	public override void Reload(int page, bool DelayThumbnailCleanup = true)
	{
		base.Reload(page, DelayThumbnailCleanup);
		this.Init();
	}

	// Token: 0x04001727 RID: 5927
	public UIButton AddAssetButton;

	// Token: 0x04001728 RID: 5928
	public UICustomUIAsset customUIAsset;
}
