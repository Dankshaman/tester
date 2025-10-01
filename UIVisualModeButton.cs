using System;
using UnityEngine;

// Token: 0x0200035C RID: 860
public class UIVisualModeButton : MonoBehaviour
{
	// Token: 0x060028B8 RID: 10424 RVA: 0x0011F8E3 File Offset: 0x0011DAE3
	private void Awake()
	{
		EventManager.OnUIThemeChange += this.OnThemeChange;
		Wait.Frames(new Action(this.OnThemeChange), 1);
	}

	// Token: 0x060028B9 RID: 10425 RVA: 0x0011F909 File Offset: 0x0011DB09
	private void OnDestroy()
	{
		EventManager.OnUIThemeChange -= this.OnThemeChange;
	}

	// Token: 0x060028BA RID: 10426 RVA: 0x0011F91C File Offset: 0x0011DB1C
	private void OnThemeChange()
	{
		this.showingHighlight = (this.IsPureMode == TableScript.PURE_MODE);
		if (this.showingHighlight)
		{
			base.GetComponent<UIButton>().defaultColor = base.GetComponent<UIButton>().hover;
			return;
		}
		base.GetComponent<UIButton>().defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNormal];
	}

	// Token: 0x060028BB RID: 10427 RVA: 0x0011F97B File Offset: 0x0011DB7B
	private void OnEnable()
	{
		this.OnThemeChange();
	}

	// Token: 0x060028BC RID: 10428 RVA: 0x00112609 File Offset: 0x00110809
	private void OnDisable()
	{
		base.GetComponent<UIButton>().defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNormal];
	}

	// Token: 0x060028BD RID: 10429 RVA: 0x0011F983 File Offset: 0x0011DB83
	private void Update()
	{
		if (this.showingHighlight != this.IsPureMode == TableScript.PURE_MODE)
		{
			this.showingHighlight = !this.showingHighlight;
			if (this.showingHighlight)
			{
				this.UpdateButton();
			}
		}
	}

	// Token: 0x060028BE RID: 10430 RVA: 0x0011F9BA File Offset: 0x0011DBBA
	private void OnClick()
	{
		if (this.IsPureMode)
		{
			Singleton<SystemConsole>.Instance.ProcessCommand("+pure_mode", false, SystemConsole.CommandEcho.Silent);
		}
		else
		{
			Singleton<SystemConsole>.Instance.ProcessCommand("-pure_mode", false, SystemConsole.CommandEcho.Silent);
		}
		this.UpdateButton();
	}

	// Token: 0x060028BF RID: 10431 RVA: 0x0011F9F0 File Offset: 0x0011DBF0
	private void UpdateButton()
	{
		for (int i = 0; i < base.transform.parent.childCount; i++)
		{
			base.transform.parent.GetChild(i).GetComponent<UIButton>().defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ButtonNormal];
		}
		if (this.showingHighlight)
		{
			base.GetComponent<UIButton>().defaultColor = base.GetComponent<UIButton>().hover;
		}
	}

	// Token: 0x04001ACD RID: 6861
	public bool IsPureMode;

	// Token: 0x04001ACE RID: 6862
	private bool showingHighlight;
}
