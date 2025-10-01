using System;
using System.Collections.Generic;

// Token: 0x020002D8 RID: 728
public class UIGridMenuDecals : UIGridMenu
{
	// Token: 0x0600239B RID: 9115 RVA: 0x000FC3C7 File Offset: 0x000FA5C7
	protected override void OnEnable()
	{
		base.OnEnable();
		this.Init();
	}

	// Token: 0x0600239C RID: 9116 RVA: 0x000FC3D8 File Offset: 0x000FA5D8
	private void Init()
	{
		List<UIGridMenu.GridButtonDecal> list = new List<UIGridMenu.GridButtonDecal>();
		List<CustomDecalState> decalPallet = NetworkSingleton<DecalManager>.Instance.DecalPallet;
		for (int i = 0; i < decalPallet.Count; i++)
		{
			CustomDecalState customDecalState = decalPallet[i];
			UIGridMenu.GridButtonDecal gridButtonDecal = new UIGridMenu.GridButtonDecal();
			gridButtonDecal.Name = customDecalState.Name;
			gridButtonDecal.customDecal = customDecalState;
			if (customDecalState == NetworkSingleton<DecalManager>.Instance.SelectedDecal)
			{
				gridButtonDecal.TopLeftText = "Selected";
			}
			gridButtonDecal.ButtonHoverColor = Colour.UIBlue;
			list.Add(gridButtonDecal);
		}
		list.Sort((UIGridMenu.GridButtonDecal x, UIGridMenu.GridButtonDecal y) => AlphanumComparatorFast.Compare(x.Name, y.Name));
		base.Load<UIGridMenu.GridButtonDecal>(list, 1, "DECALS", false, true);
	}

	// Token: 0x0600239D RID: 9117 RVA: 0x000FC496 File Offset: 0x000FA696
	public override void Reload(int page, bool DelayThumbnailCleanup = true)
	{
		base.Reload(page, DelayThumbnailCleanup);
		this.Init();
	}

	// Token: 0x040016A9 RID: 5801
	public UIButton AddDecalButton;

	// Token: 0x040016AA RID: 5802
	public UICustomDecal AddCustomDecalMenu;
}
