using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200034E RID: 846
public class UIThemeEditor : Singleton<UIThemeEditor>
{
	// Token: 0x170004BC RID: 1212
	// (get) Token: 0x06002801 RID: 10241 RVA: 0x0011AA26 File Offset: 0x00118C26
	// (set) Token: 0x06002802 RID: 10242 RVA: 0x0011AA30 File Offset: 0x00118C30
	[HideInInspector]
	public int CurrentThemeID
	{
		get
		{
			return this.rawCurrentThemeID;
		}
		set
		{
			this.rawCurrentThemeID = value;
			if (this.ThemeGrid)
			{
				for (int i = 0; i < this.ThemeGrid.childCount; i++)
				{
					UIThemeButton component = this.ThemeGrid.GetChild(i).GetComponent<UIThemeButton>();
					if (component)
					{
						component.CurrentIndicator.SetActive(component.ID == value);
					}
				}
			}
		}
	}

	// Token: 0x170004BD RID: 1213
	// (get) Token: 0x06002803 RID: 10243 RVA: 0x0011AA95 File Offset: 0x00118C95
	// (set) Token: 0x06002804 RID: 10244 RVA: 0x0011AAB6 File Offset: 0x00118CB6
	[HideInInspector]
	public string CurrentThemeName
	{
		get
		{
			if (!this.ThemeIsDirty)
			{
				return this.rawCurrentThemeName;
			}
			return this.rawCurrentThemeName + "*";
		}
		set
		{
			this.rawCurrentThemeName = value;
			if (this.CurrentThemeDisplay)
			{
				this.CurrentThemeDisplay.text = this.CurrentThemeName;
			}
		}
	}

	// Token: 0x170004BE RID: 1214
	// (get) Token: 0x06002805 RID: 10245 RVA: 0x0011AADD File Offset: 0x00118CDD
	// (set) Token: 0x06002806 RID: 10246 RVA: 0x0011AAE5 File Offset: 0x00118CE5
	[HideInInspector]
	public bool ThemeIsDirty
	{
		get
		{
			return this.rawThemeIsDirty;
		}
		set
		{
			this.rawThemeIsDirty = value;
			if (value)
			{
				this.beenDirty = true;
			}
			if (base.gameObject.activeInHierarchy && this.CurrentThemeDisplay)
			{
				this.CurrentThemeDisplay.text = this.CurrentThemeName;
			}
		}
	}

	// Token: 0x06002807 RID: 10247 RVA: 0x0011AB23 File Offset: 0x00118D23
	public void Start()
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
	}

	// Token: 0x06002808 RID: 10248 RVA: 0x0011AB33 File Offset: 0x00118D33
	public void Display()
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
		this.RefreshColours();
		this.RefreshThemes();
		base.gameObject.SetActive(true);
	}

	// Token: 0x06002809 RID: 10249 RVA: 0x0011AB5C File Offset: 0x00118D5C
	private void FixScrollBars(UIScrollView view, GameObject bar)
	{
		bar.GetComponent<UIScrollBar>().value = 0f;
		Wait.Frames(delegate
		{
			UIThemeEditor.DisplayHelper(view, null, 0f);
		}, 1);
		Wait.Frames(delegate
		{
			UIThemeEditor.DisplayHelper(view, bar, 0f);
		}, 2);
		Wait.Frames(delegate
		{
			UIThemeEditor.DisplayHelper(view, bar, 0.5f);
		}, 3);
		Wait.Frames(delegate
		{
			UIThemeEditor.DisplayHelper(view, bar, 0f);
		}, 4);
	}

	// Token: 0x0600280A RID: 10250 RVA: 0x00103BB4 File Offset: 0x00101DB4
	private static void DisplayHelper(UIScrollView view, GameObject bar = null, float position = 0f)
	{
		if (bar == null)
		{
			view.UpdateScrollbars(true);
		}
		else
		{
			bar.GetComponent<UIScrollBar>().value = position;
		}
		view.ResetPosition();
		view.UpdatePosition();
		if (bar != null)
		{
			view.UpdateScrollbars();
		}
	}

	// Token: 0x0600280B RID: 10251 RVA: 0x0011ABDC File Offset: 0x00118DDC
	private void Initialize()
	{
		this.initialized = true;
		this.UI = base.transform.Find("Panel").gameObject;
		Transform transform = this.UI.transform;
		transform = transform.GetChild(0);
		this.UI = transform.gameObject;
		this.AdvancedMode = transform.Find("Advanced").GetComponent<UIToggle>();
		this.GameCanTheme = transform.Find("Game Can Theme").GetComponent<UIToggle>();
		this.CurrentThemeDisplay = transform.Find("ThemeValue").GetComponent<UILabel>();
		this.ColourScrollView = transform.Find("Colour Controls/Colours View").GetComponent<UIScrollView>();
		this.ColourGrid = transform.Find("Colour Controls/Colours View/Grid");
		this.ColourScrollbar = transform.Find("Colour Controls/Colours Scroll Bar").gameObject;
		this.ThemeScrollView = transform.Find("Theme Controls/Theme View").GetComponent<UIScrollView>();
		this.ThemeGrid = transform.Find("Theme Controls/Theme View/Grid");
		this.ThemeScrollbar = transform.Find("Theme Controls/Theme Scroll Bar").gameObject;
		Transform transform2 = base.transform.Find("Row Templates");
		this.ColorTemplate = transform2.Find("Color Row Template").gameObject;
		this.ThemeTemplate = transform2.Find("Theme Row Template").gameObject;
		this.Line = transform2.Find("Line Template").gameObject;
		this.DummyLine = transform2.Find("Dummy Line Template").gameObject;
		this.LightThemeTemplate = transform2.Find("Light Theme Row Template").gameObject;
		this.advanced = (PlayerPrefs.GetInt("ThemeEditorAdvanced", 0) != 0);
		UIPalette.ALLOW_GAME_TO_THEME = (PlayerPrefs.GetInt("GameCanTheme", 0) != 0);
		if (this.AdvancedMode.value != this.advanced)
		{
			this.AdvancedMode.value = this.advanced;
		}
		this.CurrentThemeDisplay.text = this.CurrentThemeName;
	}

	// Token: 0x0600280C RID: 10252 RVA: 0x0011ADC4 File Offset: 0x00118FC4
	public void AddColours()
	{
		List<UIPalette.UI> list = this.advanced ? UIPalette.AdvancedThemeUIs : UIPalette.SimpleThemeUIs;
		for (int i = 0; i < list.Count; i++)
		{
			this.AddColour(list[i]);
		}
		this.ColourGrid.GetComponent<UIGrid>().repositionNow = true;
	}

	// Token: 0x0600280D RID: 10253 RVA: 0x0011AE18 File Offset: 0x00119018
	public void RefreshColours()
	{
		if (!this.initialized)
		{
			return;
		}
		TTSUtilities.DestroyChildren(this.ColourGrid.transform);
		this.doNotApplyChanges = true;
		this.AddColours();
		this.doNotApplyChanges = false;
		this.FixScrollBars(this.ColourScrollView, this.ColourScrollbar);
	}

	// Token: 0x0600280E RID: 10254 RVA: 0x0011AE64 File Offset: 0x00119064
	public void AddThemes()
	{
		this.AddTheme(0, "Light");
		this.AddTheme(1, "Dark");
		this.AddTheme(-1, "Previous Edit");
		if (UIPalette.GameTheme.colours != null)
		{
			this.AddTheme(-2, "From Game");
		}
		this.NextGroup();
		this.AddLine(this.ThemeScrollView);
		for (int i = 2; i < Singleton<UIPalette>.Instance.Themes.Count; i++)
		{
			if (Singleton<UIPalette>.Instance.Themes[i].id != -1)
			{
				this.AddTheme(Singleton<UIPalette>.Instance.Themes[i].id, Singleton<UIPalette>.Instance.Themes[i].name);
			}
		}
		this.ThemeGrid.GetComponent<UIGrid>().repositionNow = true;
	}

	// Token: 0x0600280F RID: 10255 RVA: 0x0011AF33 File Offset: 0x00119133
	public void RefreshThemes()
	{
		if (!this.initialized)
		{
			return;
		}
		TTSUtilities.DestroyChildren(this.ThemeGrid.transform);
		this.AddThemes();
		this.FixScrollBars(this.ThemeScrollView, this.ThemeScrollbar);
	}

	// Token: 0x06002810 RID: 10256 RVA: 0x0011AF68 File Offset: 0x00119168
	public void ResetSettings()
	{
		foreach (UICommandAction uicommandAction in this.ColourGrid.GetComponentsInChildren<UICommandAction>())
		{
			Singleton<SystemConsole>.Instance.ProcessCommand("reset " + uicommandAction.command, true, SystemConsole.CommandEcho.Silent);
		}
	}

	// Token: 0x06002811 RID: 10257 RVA: 0x0011AFAF File Offset: 0x001191AF
	public void NextGroup()
	{
		this.nextLineVisible = true;
		this.inGroup = true;
	}

	// Token: 0x06002812 RID: 10258 RVA: 0x0011AFBF File Offset: 0x001191BF
	public void IndividualEntries()
	{
		this.nextLineVisible = true;
		this.inGroup = false;
	}

	// Token: 0x06002813 RID: 10259 RVA: 0x0011AFD0 File Offset: 0x001191D0
	private void AddLine(UIScrollView scrollView)
	{
		Transform child = scrollView.transform.GetChild(0);
		GameObject gameObject;
		if (this.nextLineVisible)
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Line);
			if (this.inGroup)
			{
				this.nextLineVisible = false;
			}
		}
		else
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.DummyLine);
		}
		gameObject.transform.parent = child;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		UIWidget component = gameObject.GetComponent<UIWidget>();
		component.leftAnchor.target = child.parent;
		component.rightAnchor.target = child.parent;
	}

	// Token: 0x06002814 RID: 10260 RVA: 0x0011B070 File Offset: 0x00119270
	public void AddColour(UIPalette.UI uiTheme)
	{
		this.AddLine(this.ColourScrollView);
		if (uiTheme == UIPalette.UI.DoNotTheme)
		{
			this.NextGroup();
			this.AddLine(this.ColourScrollView);
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ColorTemplate);
		gameObject.transform.parent = this.ColourGrid;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		UIThemeColorPicker component = gameObject.GetComponent<UIThemeColorPicker>();
		component.UITheme = uiTheme;
		component.Tooltip.Tooltip = UIPalette.GetUITooltip(uiTheme, !this.advanced);
		component.ColorPicker.Theme = uiTheme;
		component.ColorPicker.SetColor(Singleton<UIPalette>.Instance.CurrentThemeColours[uiTheme]);
		Color color = Singleton<UIPalette>.Instance.CurrentThemeColours[uiTheme];
		color.a = 1f;
		component.ColorBackground.color = color;
		string uilabel = UIPalette.GetUILabel(uiTheme, !this.advanced);
		UILabel component2 = gameObject.GetComponent<UILabel>();
		component2.text = uilabel;
		component2.leftAnchor.target = this.ColourGrid.parent;
		component2.rightAnchor.target = this.ColourGrid.parent;
	}

	// Token: 0x06002815 RID: 10261 RVA: 0x0011B1A4 File Offset: 0x001193A4
	public void AddTheme(int id, string name)
	{
		GameObject gameObject;
		if (id == 0)
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.LightThemeTemplate);
		}
		else
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ThemeTemplate);
		}
		gameObject.transform.parent = this.ThemeGrid;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		UILabel component = gameObject.GetComponent<UILabel>();
		component.text = " " + name;
		component.leftAnchor.target = this.ThemeGrid.parent;
		component.rightAnchor.target = this.ThemeGrid.parent;
		UIThemeButton component2 = gameObject.transform.GetComponent<UIThemeButton>();
		component2.ID = id;
		component2.Name = name;
		component2.CurrentIndicator.SetActive(id == this.CurrentThemeID);
		if (id < 2)
		{
			if (component2.DeleteButton)
			{
				component2.DeleteButton.SetActive(false);
			}
			if (component2.RenameButton)
			{
				component2.RenameButton.SetActive(false);
			}
		}
	}

	// Token: 0x06002816 RID: 10262 RVA: 0x0011B2A4 File Offset: 0x001194A4
	public void ApplyThemeChange(UIPalette.UI ui, Colour colour)
	{
		if (this.doNotApplyChanges)
		{
			return;
		}
		if (UIPalette.RestrictTransparency(ui) && colour.a < UIPalette.MIN_ALPHA)
		{
			colour.a = UIPalette.MIN_ALPHA;
		}
		Singleton<UIPalette>.Instance.CurrentThemeColours[ui] = colour;
		if (!this.advanced)
		{
			Singleton<UIPalette>.Instance.SetAdvancedFromSimple(ui, colour);
		}
		this.ThemeIsDirty = true;
		Singleton<UIPalette>.Instance.RefreshThemeColours(null, true);
	}

	// Token: 0x06002817 RID: 10263 RVA: 0x0011B314 File Offset: 0x00119514
	public void LoadTheme(int id, string name = "")
	{
		if (this.CurrentThemeID != -2 && id == -2)
		{
			this.LastNonGameThemeID = this.CurrentThemeID;
			this.LastNonGameThemeName = this.CurrentThemeName;
		}
		if (id == -1)
		{
			if (!this.lastThemeWasDeleted && (this.ThemeIsDirty || !this.beenDirty))
			{
				return;
			}
			this.ThemeIsDirty = true;
			this.CurrentThemeID = this.lastThemeID;
			this.CurrentThemeName = this.lastThemeName;
			Singleton<UIPalette>.Instance.RefreshThemeColours(UIPalette.LastThemeColours, true);
		}
		else
		{
			if (this.ThemeIsDirty)
			{
				this.lastThemeWasDeleted = false;
				this.lastThemeID = this.CurrentThemeID;
				this.lastThemeName = this.rawCurrentThemeName;
				UIPalette.LastThemeColours = Singleton<UIPalette>.Instance.CopyOfCurrentThemeColours();
			}
			this.CurrentThemeID = id;
			if (id == -2)
			{
				this.CurrentThemeName = "From Game";
			}
			else if (name == "")
			{
				this.CurrentThemeName = Singleton<UIPalette>.Instance.Themes[id].name;
			}
			else
			{
				this.CurrentThemeName = name;
			}
			this.ThemeIsDirty = false;
			switch (id)
			{
			case -2:
				Singleton<UIPalette>.Instance.RefreshThemeColours(UIPalette.GameTheme.colours, true);
				break;
			case -1:
				break;
			case 0:
				Singleton<UIPalette>.Instance.RefreshThemeColours(UIPalette.LightThemeColours, true);
				break;
			case 1:
				Singleton<UIPalette>.Instance.RefreshThemeColours(UIPalette.DarkThemeColours, true);
				break;
			default:
				Singleton<UIPalette>.Instance.RefreshTheme(id);
				break;
			}
		}
		this.UpdateEditorSprites();
	}

	// Token: 0x06002818 RID: 10264 RVA: 0x0011B488 File Offset: 0x00119688
	public void UpdateEditorSprites()
	{
		if (this.ColourGrid == null)
		{
			return;
		}
		for (int i = 0; i < this.ColourGrid.childCount; i++)
		{
			UIThemeColorPicker component = this.ColourGrid.GetChild(i).GetComponent<UIThemeColorPicker>();
			if (component)
			{
				component.ColorPicker.SetColor(Singleton<UIPalette>.Instance.CurrentThemeColours[component.ColorPicker.Theme]);
				Color color = Singleton<UIPalette>.Instance.CurrentThemeColours[component.ColorPicker.Theme];
				color.a = 1f;
				component.ColorBackground.color = color;
			}
		}
	}

	// Token: 0x06002819 RID: 10265 RVA: 0x0011B53C File Offset: 0x0011973C
	public void AdvancedModeToggle()
	{
		if (this.AdvancedMode && this.advanced != this.AdvancedMode.value)
		{
			this.advanced = this.AdvancedMode.value;
			PlayerPrefs.SetInt("ThemeEditorAdvanced", this.advanced ? 1 : 0);
			this.Display();
		}
	}

	// Token: 0x0600281A RID: 10266 RVA: 0x0011B598 File Offset: 0x00119798
	public void GameCanThemeToggle()
	{
		if (this.GameCanTheme && this.GameCanTheme.value != UIPalette.ALLOW_GAME_TO_THEME)
		{
			UIPalette.ALLOW_GAME_TO_THEME = this.GameCanTheme.value;
			PlayerPrefs.SetInt("GameCanTheme", UIPalette.ALLOW_GAME_TO_THEME ? 1 : 0);
			if (UIPalette.ALLOW_GAME_TO_THEME)
			{
				if (UIPalette.GameTheme.colours != null)
				{
					this.LoadTheme(-2, "From Game");
					return;
				}
			}
			else if (this.CurrentThemeID == -2)
			{
				this.LoadTheme(this.LastNonGameThemeID, this.LastNonGameThemeName);
			}
		}
	}

	// Token: 0x0600281B RID: 10267 RVA: 0x0011B628 File Offset: 0x00119828
	public void PostStoreTheme(int id = -1, string name = "")
	{
		if (id == -1)
		{
			this.CurrentThemeID = this.rawCurrentThemeID;
		}
		else
		{
			this.CurrentThemeID = id;
		}
		if (name == "")
		{
			this.CurrentThemeName = this.rawCurrentThemeName;
		}
		else
		{
			this.CurrentThemeName = name;
		}
		this.ThemeIsDirty = false;
		this.RefreshThemes();
	}

	// Token: 0x0600281C RID: 10268 RVA: 0x0011B67D File Offset: 0x0011987D
	public void SaveTheme(string name, int id)
	{
		id = Singleton<UIPalette>.Instance.StoreTheme(Singleton<UIPalette>.Instance.CurrentThemeColours, name, id);
		if (id == -1)
		{
			UIDialog.Show("Error storing theme (this should never happen!).", "OK", null);
			return;
		}
		this.PostStoreTheme(id, name);
	}

	// Token: 0x0600281D RID: 10269 RVA: 0x0011B6B4 File Offset: 0x001198B4
	public void OpenSaveThemeAsDiaog()
	{
		if (this.CurrentThemeID == 0 || this.CurrentThemeID == 1 || this.CurrentThemeID == -2)
		{
			UIDialog.ShowInput("Save current theme as...", "Save", "Cancel", delegate(string name)
			{
				this.TrySaveThemeAs(name);
			}, null, "", "");
			return;
		}
		UIDialog.ShowInput("Save current theme as...", "Save", "Cancel", delegate(string name)
		{
			this.TrySaveThemeAs(name);
		}, null, this.rawCurrentThemeName, "");
	}

	// Token: 0x0600281E RID: 10270 RVA: 0x0011B734 File Offset: 0x00119934
	public void TrySaveThemeAs(string name)
	{
		UIThemeEditor.<>c__DisplayClass64_0 CS$<>8__locals1 = new UIThemeEditor.<>c__DisplayClass64_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.name = name;
		if (CS$<>8__locals1.name.ToLower() == "light" || CS$<>8__locals1.name.ToLower() == "dark" || CS$<>8__locals1.name.ToLower() == "previous edit" || CS$<>8__locals1.name.ToLower() == "from game")
		{
			UIDialog.Show("Cannot overwrite default themes.", "OK", null);
			return;
		}
		int i;
		int j;
		for (i = 2; i < Singleton<UIPalette>.Instance.Themes.Count; i = j + 1)
		{
			if (Singleton<UIPalette>.Instance.Themes[i].name.ToLower() == CS$<>8__locals1.name.ToLower())
			{
				UIDialog.Show(Language.Translate("Overwrite theme {0}?", Singleton<UIPalette>.Instance.Themes[i].name), "Overwrite", "Cancel", delegate()
				{
					CS$<>8__locals1.<>4__this.SaveTheme(CS$<>8__locals1.name, i);
				}, null);
				return;
			}
			j = i;
		}
		int num = Singleton<UIPalette>.Instance.StoreTheme(Singleton<UIPalette>.Instance.CurrentThemeColours, CS$<>8__locals1.name, -1);
		if (num == -1)
		{
			UIDialog.Show("Error storing theme (this should never happen!).", "OK", null);
			return;
		}
		this.PostStoreTheme(num, CS$<>8__locals1.name);
	}

	// Token: 0x0600281F RID: 10271 RVA: 0x0011B8BC File Offset: 0x00119ABC
	public void SaveCurrentTheme()
	{
		if (this.CurrentThemeID == 0 || this.CurrentThemeID == 1 || this.CurrentThemeID == -2)
		{
			this.OpenSaveThemeAsDiaog();
			return;
		}
		if (Singleton<UIPalette>.Instance.StoreTheme(Singleton<UIPalette>.Instance.CurrentThemeColours, this.rawCurrentThemeName, this.CurrentThemeID) == -1)
		{
			UIDialog.Show("Error storing theme (this should never happen!).", "OK", null);
			return;
		}
		this.PostStoreTheme(-1, "");
	}

	// Token: 0x06002820 RID: 10272 RVA: 0x0011B92C File Offset: 0x00119B2C
	public void AskDeleteTheme(int id)
	{
		if (id < 2 || id >= Singleton<UIPalette>.Instance.Themes.Count)
		{
			return;
		}
		UIDialog.Show(Language.Translate("Delete theme {0}?", Singleton<UIPalette>.Instance.Themes[id].name), "Delete", "Cancel", delegate()
		{
			this.DeleteTheme(id);
		}, null);
	}

	// Token: 0x06002821 RID: 10273 RVA: 0x0011B9B0 File Offset: 0x00119BB0
	public void DeleteTheme(int id)
	{
		this.lastThemeWasDeleted = true;
		this.lastThemeID = id;
		this.lastThemeName = Singleton<UIPalette>.Instance.Themes[id].name;
		UIPalette.LastThemeColours = Singleton<UIPalette>.Instance.CopyOfThemeColours(id);
		Singleton<UIPalette>.Instance.DeleteTheme(id);
		if (this.CurrentThemeID == id)
		{
			this.CurrentThemeID = Singleton<UIPalette>.Instance.Themes.Count;
			this.ThemeIsDirty = true;
		}
		this.RefreshThemes();
	}

	// Token: 0x06002822 RID: 10274 RVA: 0x0011BA2C File Offset: 0x00119C2C
	public void AskRenameTheme(int id)
	{
		if (id < 2 || id >= Singleton<UIPalette>.Instance.Themes.Count)
		{
			return;
		}
		UIDialog.ShowInput("Rename theme?", "Rename", "Cancel", delegate(string name)
		{
			this.RenameTheme(id, name);
		}, null, Singleton<UIPalette>.Instance.Themes[id].name, "");
	}

	// Token: 0x06002823 RID: 10275 RVA: 0x0011BAB0 File Offset: 0x00119CB0
	public void RenameTheme(int id, string name)
	{
		if (name.ToLower() == "light" || name.ToLower() == "dark" || name.ToLower() == "previous edit" || name.ToLower() == "from game")
		{
			UIDialog.Show("Theme with that name already exists.", "OK", null);
		}
		else
		{
			for (int i = 2; i < Singleton<UIPalette>.Instance.Themes.Count; i++)
			{
				if (i != id && Singleton<UIPalette>.Instance.Themes[i].name.ToLower() == name.ToLower())
				{
					UIDialog.Show("Theme with that name already exists.", "OK", null);
					return;
				}
			}
		}
		Singleton<SystemConsole>.Instance.RemoveThemeCommand(Singleton<UIPalette>.Instance.Themes[id].name);
		Singleton<UIPalette>.Instance.Themes[id].name = name;
		Singleton<SystemConsole>.Instance.CreateThemeCommand(Singleton<UIPalette>.Instance.Themes[id].name);
		if (this.CurrentThemeID == id)
		{
			this.CurrentThemeName = name;
		}
		this.RefreshThemes();
	}

	// Token: 0x06002824 RID: 10276 RVA: 0x0011BBD8 File Offset: 0x00119DD8
	public void ImportExport()
	{
		UIDialog.ShowCodeInput(Language.Translate("Theme: {0}", this.rawCurrentThemeName), "Import", "Close", new Action<string, string>(this.ImportString), null, Singleton<UIPalette>.Instance.ThemeExport(-1), "", true, "", null);
	}

	// Token: 0x06002825 RID: 10277 RVA: 0x0011BC28 File Offset: 0x00119E28
	public void ImportGameThemeString(string themeString)
	{
		if (themeString == "")
		{
			UIPalette.GameTheme.colours = null;
			if (this.CurrentThemeID == -2)
			{
				this.LoadTheme(this.LastNonGameThemeID, this.LastNonGameThemeName);
				this.RefreshColours();
			}
		}
		else
		{
			UIPalette.GameTheme.colours = Singleton<UIPalette>.Instance.CopyOfCurrentThemeColours();
			UIPalette.InGameThemeUpdate = true;
			this.ImportString(themeString);
			UIPalette.InGameThemeUpdate = false;
			if (UIPalette.ALLOW_GAME_TO_THEME)
			{
				this.LoadTheme(-2, "From Game");
				this.RefreshColours();
			}
		}
		this.RefreshThemes();
	}

	// Token: 0x06002826 RID: 10278 RVA: 0x0011BCB8 File Offset: 0x00119EB8
	public void ImportString(string themeString, string variable)
	{
		this.ImportString(themeString);
	}

	// Token: 0x06002827 RID: 10279 RVA: 0x0011BCC4 File Offset: 0x00119EC4
	public void ImportString(string themeString)
	{
		if (themeString == null)
		{
			return;
		}
		UIPalette.InBatchThemeUpdate = true;
		for (string text = LibString.bite(ref themeString, '\n'); text != null; text = LibString.bite(ref themeString, '\n'))
		{
			text = text.Trim();
			if (text != "")
			{
				if (text.StartsWith("name "))
				{
					if (!UIPalette.InGameThemeUpdate)
					{
						Singleton<SystemConsole>.Instance.ProcessCommand("ui_theme_" + text, true, SystemConsole.CommandEcho.Silent);
					}
				}
				else
				{
					string text2 = UIPalette.CommandFromThemeString(text);
					if (Singleton<SystemConsole>.Instance.CommandAvailable(LibString.lookAhead(text2, false, ' ', 0, false, false)))
					{
						Singleton<SystemConsole>.Instance.ProcessCommand(text2, true, SystemConsole.CommandEcho.Silent);
					}
				}
			}
		}
		UIPalette.InBatchThemeUpdate = false;
		Singleton<UIPalette>.Instance.RefreshThemeColours(null, true);
		this.ThemeIsDirty = true;
		this.RefreshColours();
	}

	// Token: 0x04001A48 RID: 6728
	private const string STORE_THEME_ERROR = "Error storing theme (this should never happen!).";

	// Token: 0x04001A49 RID: 6729
	private const string ALREADY_EXISTS_ERROR = "Theme with that name already exists.";

	// Token: 0x04001A4A RID: 6730
	[HideInInspector]
	public Transform PanelTemplate;

	// Token: 0x04001A4B RID: 6731
	[HideInInspector]
	public GameObject UI;

	// Token: 0x04001A4C RID: 6732
	[HideInInspector]
	public Transform ColourGrid;

	// Token: 0x04001A4D RID: 6733
	[HideInInspector]
	public UIScrollView ColourScrollView;

	// Token: 0x04001A4E RID: 6734
	[HideInInspector]
	public GameObject ColourScrollbar;

	// Token: 0x04001A4F RID: 6735
	[HideInInspector]
	public GameObject ColorTemplate;

	// Token: 0x04001A50 RID: 6736
	[HideInInspector]
	public Transform ThemeGrid;

	// Token: 0x04001A51 RID: 6737
	[HideInInspector]
	public UIScrollView ThemeScrollView;

	// Token: 0x04001A52 RID: 6738
	[HideInInspector]
	public GameObject ThemeScrollbar;

	// Token: 0x04001A53 RID: 6739
	[HideInInspector]
	public GameObject ThemeTemplate;

	// Token: 0x04001A54 RID: 6740
	[HideInInspector]
	public GameObject LightThemeTemplate;

	// Token: 0x04001A55 RID: 6741
	[HideInInspector]
	public GameObject Line;

	// Token: 0x04001A56 RID: 6742
	[HideInInspector]
	public GameObject DummyLine;

	// Token: 0x04001A57 RID: 6743
	[HideInInspector]
	public UIToggle AdvancedMode;

	// Token: 0x04001A58 RID: 6744
	[HideInInspector]
	public UIToggle GameCanTheme;

	// Token: 0x04001A59 RID: 6745
	[HideInInspector]
	public UILabel CurrentThemeDisplay;

	// Token: 0x04001A5A RID: 6746
	private int lastThemeID;

	// Token: 0x04001A5B RID: 6747
	private int rawCurrentThemeID;

	// Token: 0x04001A5C RID: 6748
	private string lastThemeName = "Light";

	// Token: 0x04001A5D RID: 6749
	public string rawCurrentThemeName = "Light";

	// Token: 0x04001A5E RID: 6750
	private int LastNonGameThemeID;

	// Token: 0x04001A5F RID: 6751
	private string LastNonGameThemeName = "Light";

	// Token: 0x04001A60 RID: 6752
	private bool rawThemeIsDirty;

	// Token: 0x04001A61 RID: 6753
	private bool beenDirty;

	// Token: 0x04001A62 RID: 6754
	private bool lastThemeWasDeleted;

	// Token: 0x04001A63 RID: 6755
	private bool initialized;

	// Token: 0x04001A64 RID: 6756
	private bool nextLineVisible;

	// Token: 0x04001A65 RID: 6757
	private bool inGroup;

	// Token: 0x04001A66 RID: 6758
	private bool advanced;

	// Token: 0x04001A67 RID: 6759
	private bool doNotApplyChanges;
}
