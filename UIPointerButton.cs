using System;
using UnityEngine;

// Token: 0x02000318 RID: 792
public class UIPointerButton : MonoBehaviour
{
	// Token: 0x0600266F RID: 9839 RVA: 0x00112576 File Offset: 0x00110776
	private void Awake()
	{
		EventManager.OnUIThemeChange += this.OnThemeChange;
		Wait.Frames(new Action(this.OnThemeChange), 1);
	}

	// Token: 0x06002670 RID: 9840 RVA: 0x0011259C File Offset: 0x0011079C
	private void OnDestroy()
	{
		EventManager.OnUIThemeChange -= this.OnThemeChange;
	}

	// Token: 0x06002671 RID: 9841 RVA: 0x001125B0 File Offset: 0x001107B0
	private void OnThemeChange()
	{
		if (this.PointerInt == PlayerScript.PointerInt)
		{
			base.GetComponent<UIButton>().defaultColor = base.GetComponent<UIButton>().hover;
			return;
		}
		base.GetComponent<UIButton>().defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNormal];
	}

	// Token: 0x06002672 RID: 9842 RVA: 0x00112601 File Offset: 0x00110801
	private void OnEnable()
	{
		this.OnThemeChange();
	}

	// Token: 0x06002673 RID: 9843 RVA: 0x00112609 File Offset: 0x00110809
	private void OnDisable()
	{
		base.GetComponent<UIButton>().defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNormal];
	}

	// Token: 0x06002674 RID: 9844 RVA: 0x0011262C File Offset: 0x0011082C
	private void OnClick()
	{
		PlayerScript.PointerInt = this.PointerInt;
		for (int i = 0; i < base.transform.parent.childCount; i++)
		{
			base.transform.parent.GetChild(i).GetComponent<UIButton>().defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNormal];
		}
		base.GetComponent<UIButton>().defaultColor = base.GetComponent<UIButton>().hover;
	}

	// Token: 0x04001914 RID: 6420
	public int PointerInt;
}
