using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000335 RID: 821
public class UISetEffect : MonoBehaviour
{
	// Token: 0x0600272F RID: 10031 RVA: 0x00116AA0 File Offset: 0x00114CA0
	private void OnEnable()
	{
		if (this.Loop)
		{
			this.UpdateSelection(PlayerScript.PointerScript.InfoObject.GetComponent<CustomAssetbundle>().LoopEffectIndex + 1);
			return;
		}
		this.Label = base.GetComponentsInChildren<UILabel>(true)[0];
		this.Number = int.Parse(base.gameObject.name);
	}

	// Token: 0x06002730 RID: 10032 RVA: 0x00116AF8 File Offset: 0x00114CF8
	private void OnClick()
	{
		if (!PlayerScript.Pointer || !PlayerScript.PointerScript.InfoObject)
		{
			return;
		}
		using (List<GameObject>.Enumerator enumerator = PlayerScript.PointerScript.GetSelectedObjects(-1, true, false).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current && PlayerScript.PointerScript.InfoObject.GetComponent<CustomAssetbundle>())
				{
					if (this.Loop)
					{
						PlayerScript.PointerScript.InfoObject.GetComponent<CustomAssetbundle>().RPCLoopEffect(this.Number - 1);
					}
					else
					{
						PlayerScript.PointerScript.InfoObject.GetComponent<CustomAssetbundle>().RPCTriggerEffect(this.Number - 1);
					}
				}
			}
		}
		if (this.Loop)
		{
			Transform parent = base.transform.parent;
			for (int i = 0; i < parent.childCount; i++)
			{
				parent.GetChild(i).GetComponent<UISetEffect>().UpdateSelection(this.Number);
			}
		}
	}

	// Token: 0x06002731 RID: 10033 RVA: 0x00116C04 File Offset: 0x00114E04
	private void UpdateSelection(int number)
	{
		this.Label = base.GetComponentsInChildren<UILabel>(true)[0];
		this.Number = int.Parse(base.gameObject.name);
		if (!PlayerScript.Pointer || !PlayerScript.PointerScript.InfoObject)
		{
			return;
		}
		if (number == this.Number)
		{
			this.Label.ThemeAs = UIPalette.UI.ContextMenuHighlight;
			this.Label.color = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ContextMenuHighlight];
			return;
		}
		this.Label.ThemeAs = UIPalette.UI.ContextMenuText;
		this.Label.color = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ContextMenuText];
	}

	// Token: 0x040019A8 RID: 6568
	public bool Loop = true;

	// Token: 0x040019A9 RID: 6569
	private UILabel Label;

	// Token: 0x040019AA RID: 6570
	private int Number;
}
