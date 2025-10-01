using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200033A RID: 826
public class UISettings : Singleton<UISettings>
{
	// Token: 0x06002743 RID: 10051 RVA: 0x00117418 File Offset: 0x00115618
	private void Start()
	{
		UIEventListener uieventListener = UIEventListener.Get(this.applyButton);
		uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnCloseSettings));
		UIEventListener uieventListener2 = UIEventListener.Get(this.resetToDefault);
		uieventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener2.onClick, new UIEventListener.VoidDelegate(this.OnResetToDefault));
		int @int = PlayerPrefs.GetInt("showChatUI", 1);
		this.chat.value = (@int > 0);
		int int2 = PlayerPrefs.GetInt("showToolUI", 1);
		this.tools.value = (int2 > 0);
		int int3 = PlayerPrefs.GetInt("showNotepadUI", 1);
		this.notepad.value = (int3 > 0);
		int int4 = PlayerPrefs.GetInt("showPlayerUI", 1);
		this.players.value = (int4 > 0);
		int int5 = PlayerPrefs.GetInt("showTopMenuUI", 1);
		this.topMenu.value = (int5 > 0);
		int int6 = PlayerPrefs.GetInt("scaleAutoUI", 1);
		this.autoScale.value = (int6 > 0);
		float @float = PlayerPrefs.GetFloat("scaleSliderUI", 0.5f);
		this.sliderScale.value = @float;
		this.sliderLabel.GetComponent<UISliderRange>().Update();
		base.StartCoroutine(this.ApplySettings());
	}

	// Token: 0x06002744 RID: 10052 RVA: 0x00117578 File Offset: 0x00115778
	private void OnDestroy()
	{
		UIEventListener uieventListener = UIEventListener.Get(this.applyButton);
		uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnCloseSettings));
		UIEventListener uieventListener2 = UIEventListener.Get(this.resetToDefault);
		uieventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener2.onClick, new UIEventListener.VoidDelegate(this.OnResetToDefault));
	}

	// Token: 0x06002745 RID: 10053 RVA: 0x001175E0 File Offset: 0x001157E0
	private void Update()
	{
		if (TTSInput.GetKey(KeyCode.LeftControl) || TTSInput.GetKey(KeyCode.RightControl))
		{
			if (TTSInput.GetKeyDown(KeyCode.F12))
			{
				Singleton<UIThemeEditor>.Instance.LoadTheme(0, "");
			}
			bool flag = false;
			bool flag2 = false;
			if (TTSInput.GetKeyDown(KeyCode.F6))
			{
				flag2 = (!this.chat.value || !this.notepad.value || !this.players.value || !this.topMenu.value || !this.tools.value);
				flag = true;
			}
			if (TTSInput.GetKeyDown(KeyCode.F1) || flag)
			{
				this.tools.value = (flag ? flag2 : (!this.tools.value));
				int value = this.tools.value ? 1 : 0;
				PlayerPrefs.SetInt("showToolUI", value);
				this.toolsGroup.SetActive(this.tools.value);
			}
			if (TTSInput.GetKeyDown(KeyCode.F2) || flag)
			{
				this.topMenu.value = (flag ? flag2 : (!this.topMenu.value));
				int value2 = this.topMenu.value ? 1 : 0;
				PlayerPrefs.SetInt("showTopMenuUI", value2);
				this.topMenuGroup.SetActive(this.topMenu.value);
			}
			if (TTSInput.GetKeyDown(KeyCode.F3) || flag)
			{
				this.players.value = (flag ? flag2 : (!this.players.value));
				int value3 = this.players.value ? 1 : 0;
				PlayerPrefs.SetInt("showPlayerUI", value3);
				this.playersGroup.SetActive(this.players.value);
			}
			if (TTSInput.GetKeyDown(KeyCode.F4) || flag)
			{
				this.notepad.value = (flag ? flag2 : (!this.notepad.value));
				int value4 = this.notepad.value ? 1 : 0;
				PlayerPrefs.SetInt("showNotepadUI", value4);
				this.notepadGroup.SetActive(this.notepad.value);
			}
			if (TTSInput.GetKeyDown(KeyCode.F5) || flag)
			{
				this.chat.value = (flag ? flag2 : (!this.chat.value));
				int value5 = this.chat.value ? 1 : 0;
				PlayerPrefs.SetInt("showChatUI", value5);
				this.chatGroup.SetActive(this.chat.value);
				this.chatInputIcon.SetActive(this.chat.value);
			}
		}
	}

	// Token: 0x06002746 RID: 10054 RVA: 0x00117878 File Offset: 0x00115A78
	private IEnumerator ApplySettings()
	{
		yield return null;
		this.chatGroup.SetActive(this.chat.value);
		this.toolsGroup.SetActive(this.tools.value);
		this.notepadGroup.SetActive(this.notepad.value);
		this.playersGroup.SetActive(this.players.value);
		this.topMenuGroup.SetActive(this.topMenu.value);
		if (this.keyboardEnabled.value)
		{
			Singleton<SystemConsole>.Instance.ProcessCommand("ui_keyboard_default_state 2", true, SystemConsole.CommandEcho.Silent);
		}
		else if (this.keyboardInVR.value)
		{
			Singleton<SystemConsole>.Instance.ProcessCommand("ui_keyboard_default_state 1", true, SystemConsole.CommandEcho.Silent);
		}
		else if (this.keyboardDisabled.value)
		{
			Singleton<SystemConsole>.Instance.ProcessCommand("ui_keyboard_default_state 0", true, SystemConsole.CommandEcho.Silent);
		}
		UIRoot component = NetworkSingleton<NetworkUI>.Instance.GUIUIRoot.GetComponent<UIRoot>();
		float num = Screen.dpi / 96f;
		if (this.autoScale.value)
		{
			if (!VRHMD.isVR)
			{
				component.scalingStyle = UIRoot.Scaling.Flexible;
				component.minimumHeight = 800;
				component.maximumHeight = 1500;
			}
			else
			{
				component.scalingStyle = UIRoot.Scaling.Constrained;
				component.manualHeight = 800;
			}
			this.sliderScale.value = 0.5f;
		}
		else
		{
			float num2 = float.Parse(this.sliderLabel.text) / 100f;
			component.scalingStyle = UIRoot.Scaling.Constrained;
			if (!VRHMD.isVR)
			{
				component.manualHeight = (int)((float)Screen.height / num2 / num);
			}
			else
			{
				component.manualHeight = (int)(800f / num2);
			}
		}
		yield break;
	}

	// Token: 0x06002747 RID: 10055 RVA: 0x00117888 File Offset: 0x00115A88
	private void OnResetToDefault(GameObject go)
	{
		this.chat.value = true;
		this.tools.value = true;
		this.notepad.value = true;
		this.players.value = true;
		this.topMenu.value = true;
		this.autoScale.value = true;
		Singleton<SystemConsole>.Instance.ProcessCommand("reset ui_keyboard_default_state", true, SystemConsole.CommandEcho.Silent);
		this.sliderScale.value = 0.5f;
	}

	// Token: 0x06002748 RID: 10056 RVA: 0x00117900 File Offset: 0x00115B00
	private void OnCloseSettings(GameObject go)
	{
		int value = this.chat.value ? 1 : 0;
		PlayerPrefs.SetInt("showChatUI", value);
		int value2 = this.tools.value ? 1 : 0;
		PlayerPrefs.SetInt("showToolUI", value2);
		int value3 = this.notepad.value ? 1 : 0;
		PlayerPrefs.SetInt("showNotepadUI", value3);
		int value4 = this.players.value ? 1 : 0;
		PlayerPrefs.SetInt("showPlayerUI", value4);
		int value5 = this.topMenu.value ? 1 : 0;
		PlayerPrefs.SetInt("showTopMenuUI", value5);
		int value6 = this.autoScale.value ? 1 : 0;
		PlayerPrefs.SetInt("scaleAutoUI", value6);
		PlayerPrefs.SetFloat("scaleSliderUI", this.sliderScale.value);
		base.StartCoroutine(this.ApplySettings());
	}

	// Token: 0x040019B3 RID: 6579
	public UIToggle chat;

	// Token: 0x040019B4 RID: 6580
	public GameObject chatGroup;

	// Token: 0x040019B5 RID: 6581
	public GameObject chatInputIcon;

	// Token: 0x040019B6 RID: 6582
	public UIToggle tools;

	// Token: 0x040019B7 RID: 6583
	public GameObject toolsGroup;

	// Token: 0x040019B8 RID: 6584
	public UIToggle notepad;

	// Token: 0x040019B9 RID: 6585
	public GameObject notepadGroup;

	// Token: 0x040019BA RID: 6586
	public UIToggle players;

	// Token: 0x040019BB RID: 6587
	public GameObject playersGroup;

	// Token: 0x040019BC RID: 6588
	public UIToggle topMenu;

	// Token: 0x040019BD RID: 6589
	public GameObject topMenuGroup;

	// Token: 0x040019BE RID: 6590
	public UIToggle keyboardDisabled;

	// Token: 0x040019BF RID: 6591
	public UIToggle keyboardInVR;

	// Token: 0x040019C0 RID: 6592
	public UIToggle keyboardEnabled;

	// Token: 0x040019C1 RID: 6593
	public UIToggle autoScale;

	// Token: 0x040019C2 RID: 6594
	public UISlider sliderScale;

	// Token: 0x040019C3 RID: 6595
	public UILabel sliderLabel;

	// Token: 0x040019C4 RID: 6596
	public GameObject applyAndCloseButton;

	// Token: 0x040019C5 RID: 6597
	public GameObject applyButton;

	// Token: 0x040019C6 RID: 6598
	public GameObject resetToDefault;
}
