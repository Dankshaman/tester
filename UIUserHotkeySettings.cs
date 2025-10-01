using System;
using NewNet;
using UnityEngine;

// Token: 0x02000358 RID: 856
public class UIUserHotkeySettings : MonoBehaviour
{
	// Token: 0x0600289D RID: 10397 RVA: 0x0011EF3D File Offset: 0x0011D13D
	private void OnEnable()
	{
		this.Refresh(false);
	}

	// Token: 0x0600289E RID: 10398 RVA: 0x0011EF46 File Offset: 0x0011D146
	private void Awake()
	{
		EventManager.OnLanguageChange += this.OnLanguageChange;
	}

	// Token: 0x0600289F RID: 10399 RVA: 0x0011EF59 File Offset: 0x0011D159
	private void Start()
	{
		this.Initialize();
	}

	// Token: 0x060028A0 RID: 10400 RVA: 0x0011EF61 File Offset: 0x0011D161
	private void OnDestroy()
	{
		EventManager.OnLanguageChange -= this.OnLanguageChange;
	}

	// Token: 0x060028A1 RID: 10401 RVA: 0x0011EF74 File Offset: 0x0011D174
	private void Initialize()
	{
		this.Title.text = Network.gameName + " " + Language.Translate("CONTROLS");
		this.Grid = base.transform.GetChild(0).gameObject;
		TTSUtilities.DestroyChildren(this.Grid.transform);
		base.GetComponent<UIScrollView>().ResetPosition();
		base.GetComponent<UIScrollView>().verticalScrollBar.GetComponent<UIScrollBar>().barSize = 1f;
		UserDefinedHotkeyManager instance = NetworkSingleton<UserDefinedHotkeyManager>.Instance;
		for (int i = 0; i < instance.Hotkeys.Count; i++)
		{
			UserDefinedHotkeyManager.HotkeyIdentifier hotkeyIdentifier = instance.Hotkeys[i];
			if (!(hotkeyIdentifier.label == ""))
			{
				GameObject gameObject = this.Grid.AddChild(this.HotkeyButton);
				gameObject.name = hotkeyIdentifier.cInputID;
				this.SetupButton(gameObject, hotkeyIdentifier.label);
			}
		}
		this.Grid.GetComponent<UIGrid>().repositionNow = true;
		this.FixScrollBar(base.GetComponent<UIScrollView>());
	}

	// Token: 0x060028A2 RID: 10402 RVA: 0x0011F074 File Offset: 0x0011D274
	private void OnLanguageChange(string oldCode, string newCode)
	{
		this.Title.text = Network.gameName + " " + Language.Translate("CONTROLS");
	}

	// Token: 0x060028A3 RID: 10403 RVA: 0x0011F09C File Offset: 0x0011D29C
	private void FixScrollBar(UIScrollView view)
	{
		UIScrollBar scrollbar = view.verticalScrollBar.GetComponent<UIScrollBar>();
		scrollbar.value = 0f;
		Wait.Frames(delegate
		{
			view.UpdateScrollbars(true);
			view.ResetPosition();
			view.UpdatePosition();
		}, 1);
		Wait.Frames(delegate
		{
			scrollbar.value = 0f;
			view.ResetPosition();
			view.UpdatePosition();
			view.UpdateScrollbars();
		}, 2);
		Wait.Frames(delegate
		{
			scrollbar.value = 0.5f;
			view.ResetPosition();
			view.UpdatePosition();
			view.UpdateScrollbars();
		}, 3);
		Wait.Frames(delegate
		{
			scrollbar.value = 0f;
			view.ResetPosition();
			view.UpdatePosition();
			view.UpdateScrollbars();
		}, 4);
	}

	// Token: 0x060028A4 RID: 10404 RVA: 0x0011F128 File Offset: 0x0011D328
	private void SetupButton(GameObject Button, string label = "")
	{
		UILabel[] componentsInChildren = Button.GetComponentsInChildren<UILabel>();
		if (label != "")
		{
			componentsInChildren[0].text = label;
		}
		componentsInChildren[1].text = cInput.GetText(Button.name, 1);
		componentsInChildren[2].text = cInput.GetText(Button.name, 2);
	}

	// Token: 0x060028A5 RID: 10405 RVA: 0x0011F17A File Offset: 0x0011D37A
	public void ResetButton()
	{
		UIDialog.Show(Language.Translate("Remove all {0} controls?", Network.gameName), "Yes", "No", delegate()
		{
			NetworkSingleton<UserDefinedHotkeyManager>.Instance.ClearBindings();
			this.Refresh(true);
		}, null);
	}

	// Token: 0x060028A6 RID: 10406 RVA: 0x0011F1A7 File Offset: 0x0011D3A7
	private void Update()
	{
		if (this.StartScan && !cInput.scanning)
		{
			NetworkSingleton<UserDefinedHotkeyManager>.Instance.WritePrefs();
			this.StartScan = false;
			this.Refresh(true);
		}
		if (cInput.scanning)
		{
			this.StartScan = true;
		}
	}

	// Token: 0x060028A7 RID: 10407 RVA: 0x0011F1E0 File Offset: 0x0011D3E0
	private void Refresh(bool minor = false)
	{
		if (!minor)
		{
			this.Initialize();
			return;
		}
		for (int i = 0; i < this.Grid.transform.childCount; i++)
		{
			GameObject gameObject = this.Grid.transform.GetChild(i).gameObject;
			this.SetupButton(gameObject, "");
		}
	}

	// Token: 0x04001AC1 RID: 6849
	public GameObject HotkeyButton;

	// Token: 0x04001AC2 RID: 6850
	public UILabel Title;

	// Token: 0x04001AC3 RID: 6851
	public UIScrollBar ScrollBar;

	// Token: 0x04001AC4 RID: 6852
	private bool StartScan;

	// Token: 0x04001AC5 RID: 6853
	private GameObject Grid;
}
