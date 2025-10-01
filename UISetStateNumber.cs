using System;
using UnityEngine;

// Token: 0x02000339 RID: 825
public class UISetStateNumber : MonoBehaviour
{
	// Token: 0x0600273F RID: 10047 RVA: 0x0011725A File Offset: 0x0011545A
	private void OnEnable()
	{
		this.UpdateSelection(PlayerScript.PointerScript.InfoObject.GetComponent<NetworkPhysicsObject>().GetSelectedStateId());
	}

	// Token: 0x06002740 RID: 10048 RVA: 0x00117278 File Offset: 0x00115478
	private void OnClick()
	{
		if (!PlayerScript.Pointer || !PlayerScript.PointerScript.InfoObject)
		{
			return;
		}
		foreach (GameObject gameObject in PlayerScript.PointerScript.GetSelectedObjects(-1, true, false))
		{
			if (gameObject && gameObject.GetComponent<NetworkPhysicsObject>().GetSelectedStateId() != -1)
			{
				gameObject.GetComponent<NetworkPhysicsObject>().ChangeState(this.Number);
			}
		}
		Transform parent = base.transform.parent;
		for (int i = 0; i < parent.childCount; i++)
		{
			parent.GetChild(i).GetComponent<UISetStateNumber>().UpdateSelection(this.Number);
		}
	}

	// Token: 0x06002741 RID: 10049 RVA: 0x00117348 File Offset: 0x00115548
	private void UpdateSelection(int number)
	{
		this.Label = base.GetComponentsInChildren<UILabel>(true)[0];
		this.Number = int.Parse(base.gameObject.name);
		if (!PlayerScript.Pointer || !PlayerScript.PointerScript.InfoObject || PlayerScript.PointerScript.InfoObject.GetComponent<NetworkPhysicsObject>().GetSelectedStateId() == -1)
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

	// Token: 0x040019B1 RID: 6577
	private UILabel Label;

	// Token: 0x040019B2 RID: 6578
	private int Number;
}
