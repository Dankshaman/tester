using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000312 RID: 786
public class UIPalette : Singleton<UIPalette>
{
	// Token: 0x0600262A RID: 9770 RVA: 0x0010CFB8 File Offset: 0x0010B1B8
	public static Dictionary<UIPalette.UI, Colour> NewThemeColours(Dictionary<UIPalette.UI, Colour> fromTheme, bool writeBaseValues = true)
	{
		Dictionary<UIPalette.UI, Colour> dictionary = new Dictionary<UIPalette.UI, Colour>(fromTheme);
		if (writeBaseValues)
		{
			dictionary[UIPalette.UI.PlayerWhite] = Colour.White;
			dictionary[UIPalette.UI.PlayerBrown] = Colour.Brown;
			dictionary[UIPalette.UI.PlayerRed] = Colour.Red;
			dictionary[UIPalette.UI.PlayerOrange] = Colour.Orange;
			dictionary[UIPalette.UI.PlayerYellow] = Colour.Yellow;
			dictionary[UIPalette.UI.PlayerGreen] = Colour.Green;
			dictionary[UIPalette.UI.PlayerTeal] = Colour.Teal;
			dictionary[UIPalette.UI.PlayerBlue] = Colour.Blue;
			dictionary[UIPalette.UI.PlayerPurple] = Colour.Purple;
			dictionary[UIPalette.UI.PlayerPink] = Colour.Pink;
			dictionary[UIPalette.UI.PlayerGrey] = Colour.Grey;
			dictionary[UIPalette.UI.PlayerBlack] = Colour.Black;
			dictionary[UIPalette.UI.Glow] = new Colour(0, 0, 0, byte.MaxValue);
		}
		return dictionary;
	}

	// Token: 0x0600262B RID: 9771 RVA: 0x0010D084 File Offset: 0x0010B284
	public static string StringFromUI(UIPalette.UI ui)
	{
		return LibString.CamelCaseFromUnderscore(LibString.UnderscoreFromCamelCase(ui.ToString()), true, true);
	}

	// Token: 0x0600262C RID: 9772 RVA: 0x0010D0A0 File Offset: 0x0010B2A0
	public static string GetUILabel(UIPalette.UI ui, bool simple = false)
	{
		switch (ui)
		{
		case UIPalette.UI.ButtonNormal:
			if (!simple)
			{
				return UIPalette.StringFromUI(ui);
			}
			return "Button";
		case UIPalette.UI.ButtonHover:
			if (!simple)
			{
				return UIPalette.StringFromUI(ui);
			}
			return "Hover";
		case UIPalette.UI.ButtonPressed:
			if (!simple)
			{
				return UIPalette.StringFromUI(ui);
			}
			return "Pressed";
		case UIPalette.UI.ButtonDisabled:
			break;
		case UIPalette.UI.ButtonHighlightA:
			if (!simple)
			{
				return UIPalette.StringFromUI(ui);
			}
			return "Highlight A";
		case UIPalette.UI.ButtonHighlightB:
			if (!simple)
			{
				return UIPalette.StringFromUI(ui);
			}
			return "Highlight B";
		case UIPalette.UI.ButtonNeutral:
			if (!simple)
			{
				return UIPalette.StringFromUI(ui);
			}
			return "Neutral";
		case UIPalette.UI.Label:
			if (!simple)
			{
				return UIPalette.StringFromUI(ui);
			}
			return "Text";
		default:
			switch (ui)
			{
			case UIPalette.UI.TabNormal:
				if (!simple)
				{
					return UIPalette.StringFromUI(ui);
				}
				return "Tab";
			case UIPalette.UI.ChatOutputControls:
				if (!simple)
				{
					return "Chat Output Borders";
				}
				return "Chat Window";
			case UIPalette.UI.ChatInputControls:
				return "Chat Input Borders";
			case UIPalette.UI.ChatInputText:
				if (!simple)
				{
					return UIPalette.StringFromUI(ui);
				}
				return "Chat Text";
			case UIPalette.UI.ConsoleOutputControls:
				if (!simple)
				{
					return "Console Output Borders";
				}
				return "Console Window";
			case UIPalette.UI.ConsoleInputControls:
				return "Console Input Borders";
			case UIPalette.UI.ConsoleInputText:
				if (!simple)
				{
					return UIPalette.StringFromUI(ui);
				}
				return "Console Text";
			case UIPalette.UI.LuaBackground:
				return "Script Editor Background";
			case UIPalette.UI.LuaText:
				return "Script Editor Text";
			case UIPalette.UI.LuaCaret:
				return "Script Editor Caret";
			case UIPalette.UI.LuaSelection:
				return "Script Editor Selection";
			case UIPalette.UI.ContextMenuBackground:
				if (!simple)
				{
					return UIPalette.StringFromUI(ui);
				}
				return "Menu Background";
			case UIPalette.UI.PlayerWhite:
			case UIPalette.UI.PlayerBrown:
			case UIPalette.UI.PlayerRed:
			case UIPalette.UI.PlayerOrange:
			case UIPalette.UI.PlayerYellow:
			case UIPalette.UI.PlayerGreen:
			case UIPalette.UI.PlayerTeal:
			case UIPalette.UI.PlayerBlue:
			case UIPalette.UI.PlayerPurple:
			case UIPalette.UI.PlayerPink:
			case UIPalette.UI.PlayerGrey:
			case UIPalette.UI.PlayerBlack:
				return UIPalette.StringFromUI(ui) + " UI Tint";
			case UIPalette.UI.ButtonHighlightC:
				if (!simple)
				{
					return UIPalette.StringFromUI(ui);
				}
				return "Highlight C";
			case UIPalette.UI.PureTableA:
				if (!simple)
				{
					return "Pure Mode: Table Color A";
				}
				return "Pure Mode: Table";
			case UIPalette.UI.PureTableASpecular:
				return "Pure Mode: Table Specularity B";
			case UIPalette.UI.PureTableB:
				return "Pure Mode: Table Color B";
			case UIPalette.UI.PureTableBSpecular:
				return "Pure Mode: Table Specularity B";
			case UIPalette.UI.PureSkyAbove:
				return "Pure Mode: Sky Above Horizon";
			case UIPalette.UI.PureSplash:
				return "Pure Mode: Table Splash Color";
			case UIPalette.UI.PureSplashSpecular:
				return "Pure Mode: Table Splash Specularity";
			case UIPalette.UI.MeasurementInner:
				return "Measurement Text Inner";
			case UIPalette.UI.MeasurementOuter:
				return "Measurement Text Outer";
			case UIPalette.UI.PureSkyHorizon:
				if (!simple)
				{
					return "Pure Mode: Sky At Horizon";
				}
				return "Pure Mode: Sky";
			case UIPalette.UI.PureSkyBelow:
				return "Pure Mode: Sky Below Horizon";
			}
			break;
		}
		return UIPalette.StringFromUI(ui);
	}

	// Token: 0x0600262D RID: 9773 RVA: 0x0010D32C File Offset: 0x0010B52C
	public static string GetUITooltip(UIPalette.UI ui, bool simple = false)
	{
		switch (ui)
		{
		case UIPalette.UI.ButtonNormal:
			if (!simple)
			{
				return "Colour of buttons.";
			}
			return "Colour of buttons and controls.";
		case UIPalette.UI.ButtonHover:
			return "Colour of buttons when the mouse hovers over them.";
		case UIPalette.UI.ButtonPressed:
			return "Colour of buttons while pressed.";
		case UIPalette.UI.ButtonDisabled:
			return "Colour of disabled buttons";
		case UIPalette.UI.ButtonHighlightA:
			return "Colour highlight for some special buttons.  For instance, the currently selected tool.";
		case UIPalette.UI.ButtonHighlightB:
			return "Colour highlight for some special buttons.  For instance, when hovering over tools or the top menu bar buttons.";
		case UIPalette.UI.ButtonNeutral:
			return "Intermediate colour for low-contrast buttons & controls.";
		case UIPalette.UI.Label:
			if (!simple)
			{
				return "Colour of button & control labels.";
			}
			return "Colour of most text.";
		case UIPalette.UI.WindowBackground:
			return "Background colour of most windows.";
		case UIPalette.UI.ControlBackground:
			return "Colour of controls not set anywhere else (i.e. not a button, slider, checkbox, radiobutton).  Typically used for panels, should be near but different from Window background.";
		case UIPalette.UI.Low:
			return "'Lowest' colour in the theme.  This would typically be black for a light-styled theme and white for a dark-styled theme";
		case UIPalette.UI.High:
			return "'Highest' colour in the theme.  This would typically be white for a light-styled theme and black for a dark-styled theme";
		case UIPalette.UI.TransparentBackground:
			return "Colour of some semi-transparent regions.  Typically has low alpha.";
		case UIPalette.UI.FloatingText:
			return "Colour of text left floating on its own; for instance, on-screen notes.";
		case UIPalette.UI.NoteEditText:
			return "Colour of text currently being editted in the on-screen note editor.";
		case UIPalette.UI.Caret:
			return "Colour of the input control insertion cursor.";
		case UIPalette.UI.Selection:
			return "Colour of the selection in an input control.";
		case UIPalette.UI.TooltipText:
			return "Colour of the text in tooltip popups.";
		case UIPalette.UI.TooltipBackground:
			return "Background colour of tooltip popups.  Usually has some transparency.";
		case UIPalette.UI.TooltipBorder:
			return "Colour of the border of tooltip popups.";
		case UIPalette.UI.InputTextInactive:
			return "Colour of text in an input control when it is not being actively editted.";
		case UIPalette.UI.InputTextActive:
			return "Colour of text in an input control while it is being actively editted.";
		case UIPalette.UI.Splash:
			return "Colour used sparingly, to call attention to a control.";
		case UIPalette.UI.SplashHighlight:
			return "Colour used sparingly, to call attention to a control.  Splash Highlight is typically used for when a Splash coloured button is being hovered over.";
		case UIPalette.UI.RadioButtonBackground:
			return "Colour of radio buttons.";
		case UIPalette.UI.RadioButtonPressed:
			return "Colour of the selected radio button.";
		case UIPalette.UI.CheckBoxBackground:
			return "Colour of checkbox controls.";
		case UIPalette.UI.CheckBoxPressed:
			return "Colour of the tick in a checkbox control.";
		case UIPalette.UI.SliderNormal:
			return "Colour of slider controls.";
		case UIPalette.UI.SliderPressed:
			return "Colour of slider groove while it is being pressed.";
		case UIPalette.UI.SliderLabel:
			return "Colour of text on a slider control.  Should contrast with Slider Normal.";
		case UIPalette.UI.TabNormal:
			return "Colour of tabs.";
		case UIPalette.UI.TabActive:
			return "Colour of the active tab. Typically the same as the Motif.";
		case UIPalette.UI.TabStrip:
			return "Colour of the strip below tabs. Typically the same as the Motif.";
		case UIPalette.UI.Motif:
			return "Usually the defining colour of the theme.";
		case UIPalette.UI.MotifHighlightA:
			return "Colour used to highlight controls which are Motif coloured.  Typically a shade of Motif.";
		case UIPalette.UI.MotifHighlightB:
			return "Colour used to highlight controls which are Motif coloured.  Typically a shade of Motif.";
		case UIPalette.UI.BackgroundTint:
			return "Background tint applied to the start screen.  Typically has low alpha.";
		case UIPalette.UI.ChatOutputControls:
			if (!simple)
			{
				return "Colour of the chat log's border.";
			}
			return "Colour of the background and frame of the chat windows.";
		case UIPalette.UI.ChatOutputBackground:
			return "Background colour of the chat log.";
		case UIPalette.UI.ChatInputControls:
			return "Chat Input Borders.";
		case UIPalette.UI.ChatInputBackground:
			return "Background colour of the chat input control.";
		case UIPalette.UI.ChatInputText:
			if (!simple)
			{
				return "Colour if text in the chat input box.";
			}
			return "Colour of text in the chat windows.";
		case UIPalette.UI.ConsoleOutputControls:
			if (!simple)
			{
				return "Colour of the console log's border.";
			}
			return "Colour of the background and frame of the console window.";
		case UIPalette.UI.ConsoleOutputBackground:
			return "Background colour of the console log.";
		case UIPalette.UI.ConsoleInputControls:
			return "Chat Input Borders.";
		case UIPalette.UI.ConsoleInputBackground:
			return "Background colour of the console input control.";
		case UIPalette.UI.ConsoleInputText:
			if (!simple)
			{
				return "Colour if text in the console input box.";
			}
			return "Colour of text in the console window.";
		case UIPalette.UI.Divider:
			return "Colour of dividers, like the horizontal lines in this list.";
		case UIPalette.UI.TooltipMotif:
			return "Colour of important text in tooltip popups.  Typically a brighter shade of Motif.";
		case UIPalette.UI.LuaBackground:
			return "Background colour of the script editor.";
		case UIPalette.UI.LuaText:
			return "Text colour of the script editor.";
		case UIPalette.UI.LuaCaret:
			return "Colour of the insertion cursor in the script editor.";
		case UIPalette.UI.LuaSelection:
			return "Colour of the selected area in the script editor.";
		case UIPalette.UI.ChatTabBackground:
			return "Background colour of the chat/console window tabs.";
		case UIPalette.UI.ChatTabHighlight:
			return "Background colour of the active chat/console window tab.";
		case UIPalette.UI.ContextMenuBackground:
			return "Colour of the background of the right-click context menu.  Typically has less than full alpha.";
		case UIPalette.UI.ContextMenuText:
			return "Colour of text in the right-click context menu.";
		case UIPalette.UI.ContextMenuHighlight:
			return "Colour of text on a selected sub-item in the right-click context menu.";
		case UIPalette.UI.HoverHighlight:
			return "Colour which icons on buttons will change to when they are hovered over.  Used on the buttons on the start screen.";
		case UIPalette.UI.PlayerWhite:
			return "Colour used to represent the white player in the UI.  Typically left at its default value.";
		case UIPalette.UI.PlayerBrown:
			return "Colour used to represent the brown player in the UI.  Typically left at its default value.";
		case UIPalette.UI.PlayerRed:
			return "Colour used to represent the red player in the UI.  Typically left at its default value.";
		case UIPalette.UI.PlayerOrange:
			return "Colour used to represent the orange player in the UI.  Typically left at its default value.";
		case UIPalette.UI.PlayerYellow:
			return "Colour used to represent the yellow player in the UI.  Typically left at its default value.";
		case UIPalette.UI.PlayerGreen:
			return "Colour used to represent the green player in the UI.  Typically left at its default value.";
		case UIPalette.UI.PlayerTeal:
			return "Colour used to represent the teal player in the UI.  Typically left at its default value.";
		case UIPalette.UI.PlayerBlue:
			return "Colour used to represent the blue player in the UI.  Typically left at its default value.";
		case UIPalette.UI.PlayerPurple:
			return "Colour used to represent the purple player in the UI.  Typically left at its default value.";
		case UIPalette.UI.PlayerPink:
			return "Colour used to represent the pink player in the UI.  Typically left at its default value.";
		case UIPalette.UI.PlayerGrey:
			return "Colour used to represent the grey player in the UI.  Typically left at its default value.";
		case UIPalette.UI.PlayerBlack:
			return "Colour used to represent the black player in the UI.  Typically left at its default value.";
		case UIPalette.UI.Glow:
			return "Colour of glow surrounding windows and buttons.  Default is black, which creates a drop shadow.";
		case UIPalette.UI.ContextMenuHover:
			return "Colour of items in the right-click context menu when the mouse hovers over them.";
		case UIPalette.UI.PureTableA:
			return "Primary colour of the table in Pure Mode.";
		case UIPalette.UI.PureTableASpecular:
			return "Primary specularity of the table in Pure Mode.";
		case UIPalette.UI.PureTableB:
			return "Secondary colour of the table in Pure Mode.";
		case UIPalette.UI.PureTableBSpecular:
			return "Secondary specularity of the table in Pure Mode.";
		case UIPalette.UI.PureSkyAbove:
			return "Colour of the sky above the horizon in Pure Mode.  Alpha sets weight.";
		case UIPalette.UI.PureSplash:
			return "Splash colour of the table in Pure Mode.";
		case UIPalette.UI.PureSplashSpecular:
			return "Splash specularity of the table in Pure Mode.";
		case UIPalette.UI.MeasurementInner:
			return "Colour of the text when measuring with the Line Tool.";
		case UIPalette.UI.MeasurementOuter:
			return "Colour of the text border when measuring with the Line Tool.";
		case UIPalette.UI.PureSkyHorizon:
			if (!simple)
			{
				return "Colour of the sky at the horizon in Pure Mode.  Alpha sets sky intensity.";
			}
			return "Colour of the sky in Pure Mode.";
		case UIPalette.UI.PureSkyBelow:
			return "Colour of the sky below the horizon in Pure Mode.  Alpha sets weight.";
		}
		return "";
	}

	// Token: 0x0600262E RID: 9774 RVA: 0x0010D6E0 File Offset: 0x0010B8E0
	public Dictionary<UIPalette.UI, Colour> CopyOfCurrentThemeColours()
	{
		return new Dictionary<UIPalette.UI, Colour>(this.CurrentThemeColours);
	}

	// Token: 0x0600262F RID: 9775 RVA: 0x0010D6ED File Offset: 0x0010B8ED
	public Dictionary<UIPalette.UI, Colour> CopyOfThemeColours(int id)
	{
		if (this.Themes.Count > id)
		{
			return new Dictionary<UIPalette.UI, Colour>(this.Themes[id].colours);
		}
		return null;
	}

	// Token: 0x06002630 RID: 9776 RVA: 0x0010D718 File Offset: 0x0010B918
	protected override void Awake()
	{
		base.Awake();
		Singleton<SystemConsole>.Instance.CreateThemeCommand("Light");
		Singleton<SystemConsole>.Instance.CreateThemeCommand("Dark");
		int num = 2;
		Theme theme;
		for (theme = this.LoadThemeFromPrefs(num); theme != null; theme = this.LoadThemeFromPrefs(num))
		{
			this.Themes.Add(theme);
			Singleton<SystemConsole>.Instance.CreateThemeCommand(theme.name);
			num++;
		}
		theme = this.LoadThemeFromPrefs(-1);
		if (theme != null)
		{
			Singleton<UIThemeEditor>.Instance.CurrentThemeName = theme.name;
			Singleton<UIThemeEditor>.Instance.CurrentThemeID = -1;
			for (int i = 0; i < this.Themes.Count; i++)
			{
				if (this.Themes[i].name.ToLower() == theme.name)
				{
					Singleton<UIThemeEditor>.Instance.CurrentThemeID = i;
					break;
				}
			}
			this.RefreshThemeColours(theme.colours, false);
		}
		EventManager.OnResetTable += this.OnResetTable;
	}

	// Token: 0x06002631 RID: 9777 RVA: 0x0010D80A File Offset: 0x0010BA0A
	private void OnDestroy()
	{
		EventManager.OnResetTable -= this.OnResetTable;
	}

	// Token: 0x06002632 RID: 9778 RVA: 0x0010D81D File Offset: 0x0010BA1D
	private void OnResetTable()
	{
		Singleton<UIThemeEditor>.Instance.ImportGameThemeString("");
	}

	// Token: 0x06002633 RID: 9779 RVA: 0x0010D82E File Offset: 0x0010BA2E
	public void ToggleThemeEditor()
	{
		if (Singleton<UIThemeEditor>.Instance.gameObject.activeInHierarchy)
		{
			Singleton<UIThemeEditor>.Instance.gameObject.SetActive(false);
			return;
		}
		Singleton<UIThemeEditor>.Instance.Display();
	}

	// Token: 0x06002634 RID: 9780 RVA: 0x0010D85C File Offset: 0x0010BA5C
	private void ApplyTheme()
	{
		if (this.storeTheme)
		{
			this.storeTheme = false;
			this.StoreThemeInPrefs(-1);
		}
		List<GameObject> list = new List<GameObject>();
		List<GameObject> sceneRootGameObjects = Utilities.GetSceneRootGameObjects();
		foreach (GameObject gameObject in sceneRootGameObjects)
		{
			foreach (Transform transform in gameObject.GetComponentsInChildren<Transform>(true))
			{
				list.Add(transform.gameObject);
			}
		}
		sceneRootGameObjects.Clear();
		foreach (GameObject gameObject2 in list)
		{
			UIButton[] components = gameObject2.GetComponents<UIButton>();
			for (int j = 0; j < components.Length; j++)
			{
				this.SetColours(components[j], this.CurrentThemeColours, false);
			}
			UILabel component = gameObject2.GetComponent<UILabel>();
			if (component)
			{
				this.SetColours(component, this.CurrentThemeColours, false);
			}
			UISprite component2 = gameObject2.GetComponent<UISprite>();
			if (component2)
			{
				this.SetColours(component2, this.CurrentThemeColours, false);
			}
			UIInput component3 = gameObject2.GetComponent<UIInput>();
			if (component3)
			{
				this.SetColours(component3, this.CurrentThemeColours, false);
			}
			UIPopupList component4 = gameObject2.GetComponent<UIPopupList>();
			if (component4)
			{
				this.SetColours(component4, this.CurrentThemeColours, false);
			}
		}
		if (TableScript.PURE_MODE)
		{
			TableScript.UpdatePureMode();
		}
		if (this.lastUIThemeEvent < Time.frameCount)
		{
			this.lastUIThemeEvent = Time.frameCount;
			EventManager.TriggerUIThemeChange();
		}
	}

	// Token: 0x06002635 RID: 9781 RVA: 0x0010DA10 File Offset: 0x0010BC10
	public void RefreshThemeColours(Dictionary<UIPalette.UI, Colour> colours = null, bool store = true)
	{
		if (colours != null)
		{
			this.CurrentThemeColours = new Dictionary<UIPalette.UI, Colour>(colours);
		}
		if (store)
		{
			this.storeTheme = true;
		}
		if (this.themeRefreshing)
		{
			return;
		}
		this.themeRefreshing = true;
		Wait.Frames(delegate
		{
			this.themeRefreshing = false;
			this.ApplyTheme();
		}, 1);
	}

	// Token: 0x06002636 RID: 9782 RVA: 0x0010DA4E File Offset: 0x0010BC4E
	public void RefreshTheme(int id)
	{
		if (id >= this.Themes.Count)
		{
			return;
		}
		if (this.Themes[id].id == -1)
		{
			return;
		}
		this.RefreshThemeColours(this.Themes[id].colours, true);
	}

	// Token: 0x06002637 RID: 9783 RVA: 0x0010DA8C File Offset: 0x0010BC8C
	public void RefreshTransparency()
	{
		if (UIPalette.RESTRICT_TRANSPARENCY)
		{
			foreach (UIPalette.UI ui in UIPalette.AdvancedThemeUIs)
			{
				if (ui != UIPalette.UI.Auto && ui != UIPalette.UI.DoNotTheme)
				{
					Colour colour = this.CurrentThemeColours[ui];
					if (UIPalette.RestrictTransparency(ui) && colour.a < UIPalette.MIN_ALPHA)
					{
						colour.a = UIPalette.MIN_ALPHA;
					}
					this.CurrentThemeColours[ui] = colour;
				}
			}
			this.ApplyTheme();
		}
	}

	// Token: 0x06002638 RID: 9784 RVA: 0x0010DB28 File Offset: 0x0010BD28
	private UIPalette.UI buttonThemeFromColour(Colour buttonColour, UIPalette.UI primary, UIPalette.UI secondary = UIPalette.UI.Auto, UIPalette.UI tertiary = UIPalette.UI.Auto)
	{
		if (buttonColour == UIPalette.OriginalThemePresets[primary])
		{
			return primary;
		}
		if (secondary != UIPalette.UI.Auto && buttonColour == UIPalette.OriginalThemePresets[secondary])
		{
			return secondary;
		}
		if (tertiary != UIPalette.UI.Auto && buttonColour == UIPalette.OriginalThemePresets[tertiary])
		{
			return tertiary;
		}
		if (buttonColour == UIPalette.OriginalThemePresets[UIPalette.UI.ButtonHighlightA])
		{
			return UIPalette.UI.ButtonHighlightA;
		}
		if (buttonColour == UIPalette.OriginalThemePresets[UIPalette.UI.ButtonHighlightB])
		{
			return UIPalette.UI.ButtonHighlightB;
		}
		if (buttonColour == UIPalette.OriginalThemePresets[UIPalette.UI.ButtonHighlightC])
		{
			return UIPalette.UI.ButtonHighlightC;
		}
		if (buttonColour == UIPalette.OriginalThemePresets[UIPalette.UI.ButtonNeutral])
		{
			return UIPalette.UI.ButtonNeutral;
		}
		if (buttonColour == UIPalette.OriginalThemePresets[UIPalette.UI.Low])
		{
			return UIPalette.UI.Low;
		}
		if (buttonColour == UIPalette.OriginalThemePresets[UIPalette.UI.High])
		{
			return UIPalette.UI.High;
		}
		if (buttonColour == UIPalette.OriginalThemePresets[UIPalette.UI.TabNormal])
		{
			return UIPalette.UI.TabNormal;
		}
		if (buttonColour == UIPalette.OriginalThemePresets[UIPalette.UI.Splash])
		{
			return UIPalette.UI.Splash;
		}
		if (buttonColour == UIPalette.OriginalThemePresets[UIPalette.UI.SplashHighlight])
		{
			return UIPalette.UI.SplashHighlight;
		}
		if (buttonColour == UIPalette.OriginalThemePresets[UIPalette.UI.Motif])
		{
			return UIPalette.UI.Motif;
		}
		if (buttonColour == UIPalette.OriginalThemePresets[UIPalette.UI.MotifHighlightA])
		{
			return UIPalette.UI.MotifHighlightA;
		}
		if (buttonColour == UIPalette.OriginalThemePresets[UIPalette.UI.MotifHighlightB])
		{
			return UIPalette.UI.MotifHighlightB;
		}
		return UIPalette.UI.DoNotTheme;
	}

	// Token: 0x06002639 RID: 9785 RVA: 0x0010DC8C File Offset: 0x0010BE8C
	public void InitTheme(UIButton button, Dictionary<UIPalette.UI, Colour> theme = null)
	{
		if (button.BeenInit)
		{
			return;
		}
		button.BeenInit = true;
		if (button.ThemeNormalAsSetting == UIPalette.UI.Auto)
		{
			if (button.normalSprite == "Icon-RadioButtonEmpty")
			{
				button.ThemeNormalAs = UIPalette.UI.RadioButtonBackground;
			}
			else if (button.normalSprite == "Icon-CheckBoxEmpty")
			{
				button.ThemeNormalAs = UIPalette.UI.CheckBoxBackground;
			}
			else if (button.normalSprite == "Scrollbar-NUB-Square")
			{
				button.ThemeNormalAs = UIPalette.UI.SliderNormal;
			}
			else if (button.normalSprite == "Scrollbartest-2")
			{
				button.ThemeNormalAs = UIPalette.UI.ControlBackground;
			}
			else if (button.normalSprite == "Normal tab")
			{
				button.ThemeNormalAs = UIPalette.UI.TabNormal;
			}
			else
			{
				button.ThemeNormalAs = this.buttonThemeFromColour(button.defaultColor, UIPalette.UI.ButtonNormal, UIPalette.UI.ButtonHover, UIPalette.UI.Auto);
			}
		}
		else
		{
			button.ThemeNormalAs = button.ThemeNormalAsSetting;
		}
		if (button.ThemeHoverAsSetting == UIPalette.UI.Auto)
		{
			if (button.transform.IsChildOf(NetworkSingleton<NetworkUI>.Instance.GUIContextualGlobalMenu.transform) || button.transform.IsChildOf(NetworkSingleton<NetworkUI>.Instance.GUIContextualMenu.transform))
			{
				button.ThemeHoverAs = UIPalette.UI.ContextMenuHover;
			}
			else if (button.normalSprite == "Scrollbartest-2")
			{
				button.ThemeHoverAs = UIPalette.UI.ControlBackground;
			}
			else
			{
				button.ThemeHoverAs = this.buttonThemeFromColour(button.hover, UIPalette.UI.ButtonHover, UIPalette.UI.SplashHighlight, UIPalette.UI.Auto);
				if (button.ThemeHoverAs == UIPalette.UI.DoNotTheme)
				{
					button.ThemeHoverAs = UIPalette.UI.ButtonHover;
				}
			}
		}
		else
		{
			button.ThemeHoverAs = button.ThemeHoverAsSetting;
		}
		if (button.ThemePressedAsSetting == UIPalette.UI.Auto)
		{
			if (button.normalSprite == "Icon-RadioButtonEmpty")
			{
				button.ThemePressedAs = UIPalette.UI.RadioButtonPressed;
			}
			else if (button.normalSprite == "Icon-CheckBoxEmpty")
			{
				button.ThemePressedAs = UIPalette.UI.CheckBoxPressed;
			}
			else if (button.normalSprite == "Scrollbar-NUB-Square")
			{
				button.ThemePressedAs = UIPalette.UI.SliderPressed;
			}
			else if (button.normalSprite == "Normal tab")
			{
				button.ThemePressedAs = UIPalette.UI.TabActive;
			}
			else
			{
				button.ThemePressedAs = this.buttonThemeFromColour(button.pressed, UIPalette.UI.ButtonPressed, UIPalette.UI.ButtonHover, UIPalette.UI.ButtonNormal);
			}
		}
		else
		{
			button.ThemePressedAs = button.ThemePressedAsSetting;
		}
		if (button.ThemeDisabledAsSetting == UIPalette.UI.Auto)
		{
			button.ThemeDisabledAs = this.buttonThemeFromColour(button.disabledColor, UIPalette.UI.ButtonDisabled, UIPalette.UI.ButtonNormal, UIPalette.UI.Auto);
		}
		else
		{
			button.ThemeDisabledAs = button.ThemeDisabledAsSetting;
		}
		Wait.Frames(delegate
		{
			this.SetColours(button, (theme == null) ? this.CurrentThemeColours : theme, true);
		}, 1);
	}

	// Token: 0x0600263A RID: 9786 RVA: 0x0010E008 File Offset: 0x0010C208
	public void InitTheme(UILabel label, Dictionary<UIPalette.UI, Colour> theme = null)
	{
		if (label.BeenInit)
		{
			return;
		}
		label.BeenInit = true;
		if (label.GetComponent<UIInput>() != null)
		{
			label.ThemeAs = UIPalette.UI.DoNotTheme;
		}
		else if (label.ThemeAsSetting == UIPalette.UI.Auto)
		{
			if (label.GetComponent<UIInput>())
			{
				label.ThemeAs = UIPalette.UI.DoNotTheme;
			}
			else if (label.GetComponent<UISliderRange>())
			{
				label.ThemeAs = UIPalette.UI.SliderLabel;
			}
			else if (label.color == UIPalette.OriginalThemePresets[UIPalette.UI.Label])
			{
				label.ThemeAs = UIPalette.UI.Label;
			}
			else if (label.color == UIPalette.OriginalThemePresets[UIPalette.UI.Low])
			{
				label.ThemeAs = UIPalette.UI.Low;
			}
			else if (label.color == UIPalette.OriginalThemePresets[UIPalette.UI.High])
			{
				label.ThemeAs = UIPalette.UI.High;
			}
			else
			{
				label.ThemeAs = UIPalette.UI.DoNotTheme;
			}
		}
		else
		{
			label.ThemeAs = label.ThemeAsSetting;
		}
		Wait.Frames(delegate
		{
			this.SetColours(label, (theme == null) ? this.CurrentThemeColours : theme, true);
		}, 1);
	}

	// Token: 0x0600263B RID: 9787 RVA: 0x0010E184 File Offset: 0x0010C384
	public void InitTheme(UIPopupList popup, Dictionary<UIPalette.UI, Colour> theme = null)
	{
		if (popup.BeenInit)
		{
			return;
		}
		popup.BeenInit = true;
		if (popup.ThemeBackgroundAsSetting == UIPalette.UI.Auto)
		{
			if (popup.backgroundColor == UIPalette.OriginalThemePresets[UIPalette.UI.WindowBackground])
			{
				popup.ThemeBackgroundAs = UIPalette.UI.WindowBackground;
			}
			else if (popup.backgroundColor == UIPalette.OriginalThemePresets[UIPalette.UI.ControlBackground])
			{
				popup.ThemeBackgroundAs = UIPalette.UI.ControlBackground;
			}
			else if (popup.backgroundColor == UIPalette.OriginalThemePresets[UIPalette.UI.Low])
			{
				popup.ThemeBackgroundAs = UIPalette.UI.Low;
			}
			else if (popup.backgroundColor == UIPalette.OriginalThemePresets[UIPalette.UI.High])
			{
				popup.ThemeBackgroundAs = UIPalette.UI.High;
			}
			else
			{
				popup.ThemeBackgroundAs = UIPalette.UI.DoNotTheme;
			}
		}
		else
		{
			popup.ThemeBackgroundAs = popup.ThemeBackgroundAsSetting;
		}
		if (popup.ThemeHighlightAsSetting == UIPalette.UI.Auto)
		{
			if (popup.highlightColor == UIPalette.OriginalThemePresets[UIPalette.UI.ButtonHighlightA])
			{
				popup.ThemeHighlightAs = UIPalette.UI.ButtonHighlightA;
			}
			else if (popup.highlightColor == UIPalette.OriginalThemePresets[UIPalette.UI.ButtonHighlightB])
			{
				popup.ThemeHighlightAs = UIPalette.UI.ButtonHighlightB;
			}
			else if (popup.highlightColor == UIPalette.OriginalThemePresets[UIPalette.UI.ButtonHover])
			{
				popup.ThemeHighlightAs = UIPalette.UI.ButtonHover;
			}
			else if (popup.highlightColor == UIPalette.OriginalThemePresets[UIPalette.UI.Low])
			{
				popup.ThemeHighlightAs = UIPalette.UI.Low;
			}
			else if (popup.highlightColor == UIPalette.OriginalThemePresets[UIPalette.UI.High])
			{
				popup.ThemeHighlightAs = UIPalette.UI.High;
			}
			else
			{
				popup.ThemeHighlightAs = UIPalette.UI.ButtonHover;
			}
		}
		else
		{
			popup.ThemeHighlightAs = popup.ThemeHighlightAsSetting;
		}
		if (popup.ThemeLabelAsSetting == UIPalette.UI.Auto)
		{
			if (popup.textColor == UIPalette.OriginalThemePresets[UIPalette.UI.Label])
			{
				popup.ThemeLabelAs = UIPalette.UI.Label;
			}
			else if (popup.highlightColor == UIPalette.OriginalThemePresets[UIPalette.UI.Low])
			{
				popup.ThemeLabelAs = UIPalette.UI.Low;
			}
			else if (popup.highlightColor == UIPalette.OriginalThemePresets[UIPalette.UI.High])
			{
				popup.ThemeLabelAs = UIPalette.UI.High;
			}
			else
			{
				popup.ThemeLabelAs = UIPalette.UI.DoNotTheme;
			}
		}
		else
		{
			popup.ThemeLabelAs = popup.ThemeLabelAsSetting;
		}
		Wait.Frames(delegate
		{
			this.SetColours(popup, (theme == null) ? this.CurrentThemeColours : theme, true);
		}, 1);
	}

	// Token: 0x0600263C RID: 9788 RVA: 0x0010E494 File Offset: 0x0010C694
	public void InitTheme(UISprite sprite, Dictionary<UIPalette.UI, Colour> theme = null)
	{
		if (sprite.BeenInit)
		{
			return;
		}
		sprite.BeenInit = true;
		if (sprite.ThemeAsSetting == UIPalette.UI.DoNotTheme || this.UnthemeableSprites.Contains(sprite.spriteName))
		{
			sprite.ThemeAs = UIPalette.UI.DoNotTheme;
			return;
		}
		if (sprite.ThemeAsSetting != UIPalette.UI.Auto)
		{
			sprite.ThemeAs = sprite.ThemeAsSetting;
		}
		else if (sprite.spriteName == "SquareWithDropShadow")
		{
			if (sprite.color == UIPalette.OriginalThemePresets[UIPalette.UI.WindowBackground])
			{
				sprite.ThemeAs = UIPalette.UI.WindowBackground;
			}
			else if (sprite.color == UIPalette.OriginalThemePresets[UIPalette.UI.ControlBackground])
			{
				sprite.ThemeAs = UIPalette.UI.ControlBackground;
			}
			else if (sprite.color == UIPalette.OriginalThemePresets[UIPalette.UI.ButtonHover])
			{
				sprite.ThemeAs = UIPalette.UI.ButtonHover;
			}
			else
			{
				sprite.ThemeAs = UIPalette.UI.DoNotTheme;
			}
		}
		else if (sprite.spriteName == "WhiteTrans-square")
		{
			if (sprite.color == UIPalette.OriginalThemePresets[UIPalette.UI.TransparentBackground])
			{
				sprite.ThemeAs = UIPalette.UI.TransparentBackground;
			}
			else
			{
				sprite.ThemeAs = UIPalette.UI.DoNotTheme;
			}
		}
		else if (sprite.spriteName == "DropShadow")
		{
			sprite.ThemeAs = UIPalette.UI.Glow;
		}
		else if (sprite.spriteName == "CM")
		{
			sprite.ThemeAs = UIPalette.UI.ContextMenuBackground;
		}
		else if (sprite.spriteName == "Icon-RadioButtonEmpty")
		{
			sprite.ThemeAs = UIPalette.UI.RadioButtonBackground;
		}
		else if (sprite.spriteName == "Icon-RadioButtonMark")
		{
			sprite.ThemeAs = UIPalette.UI.Motif;
		}
		else if (sprite.spriteName == "Icon-CheckBoxEmpty")
		{
			sprite.ThemeAs = UIPalette.UI.CheckBoxBackground;
		}
		else if (sprite.spriteName == "Scrollbar-NUB-Square")
		{
			sprite.ThemeAs = UIPalette.UI.SliderNormal;
		}
		else if (sprite.spriteName == "Normal tab")
		{
			sprite.ThemeAs = UIPalette.UI.TabActive;
		}
		else if (sprite.spriteName == "White_square")
		{
			if (sprite.GetComponent<UIColorPickerInput>())
			{
				sprite.ThemeAs = UIPalette.UI.DoNotTheme;
			}
			else if (sprite.color == UIPalette.OriginalThemePresets[UIPalette.UI.Divider])
			{
				sprite.ThemeAs = UIPalette.UI.Divider;
			}
			else if (sprite.color == UIPalette.OriginalThemePresets[UIPalette.UI.ControlBackground])
			{
				sprite.ThemeAs = UIPalette.UI.ControlBackground;
			}
			else if (sprite.gameObject.name.StartsWith("!Line"))
			{
				sprite.ThemeAs = UIPalette.UI.Divider;
			}
			else
			{
				sprite.ThemeAs = UIPalette.UI.DoNotTheme;
			}
		}
		else if (sprite.color == UIPalette.OriginalThemePresets[UIPalette.UI.TabNormal])
		{
			sprite.ThemeAs = UIPalette.UI.TabNormal;
		}
		else if (sprite.color == UIPalette.OriginalThemePresets[UIPalette.UI.TabStrip])
		{
			sprite.ThemeAs = UIPalette.UI.TabStrip;
		}
		else if (sprite.color == UIPalette.OriginalThemePresets[UIPalette.UI.Low])
		{
			sprite.ThemeAs = UIPalette.UI.Low;
		}
		else if (sprite.color == UIPalette.OriginalThemePresets[UIPalette.UI.High])
		{
			sprite.ThemeAs = UIPalette.UI.High;
		}
		else if (sprite.color == UIPalette.OriginalThemePresets[UIPalette.UI.Motif])
		{
			sprite.ThemeAs = UIPalette.UI.Motif;
		}
		else
		{
			sprite.ThemeAs = UIPalette.UI.DoNotTheme;
		}
		Wait.Frames(delegate
		{
			this.SetColours(sprite, (theme == null) ? this.CurrentThemeColours : theme, true);
		}, 1);
	}

	// Token: 0x0600263D RID: 9789 RVA: 0x0010E944 File Offset: 0x0010CB44
	public void InitTheme(UIInput input, Dictionary<UIPalette.UI, Colour> theme = null)
	{
		if (input.BeenInit)
		{
			return;
		}
		input.BeenInit = true;
		if (input.ThemeActiveAsSetting == UIPalette.UI.Auto)
		{
			if (input.activeTextColor == UIPalette.OriginalThemePresets[UIPalette.UI.InputTextActive])
			{
				input.ThemeActiveAs = UIPalette.UI.InputTextActive;
			}
			else if (input.activeTextColor == UIPalette.OriginalThemePresets[UIPalette.UI.Label])
			{
				input.ThemeActiveAs = UIPalette.UI.Label;
			}
			else if (input.activeTextColor == UIPalette.OriginalThemePresets[UIPalette.UI.High])
			{
				input.ThemeActiveAs = UIPalette.UI.High;
			}
			else if (input.activeTextColor == UIPalette.OriginalThemePresets[UIPalette.UI.Low])
			{
				input.ThemeActiveAs = UIPalette.UI.Low;
			}
			else if (input.activeTextColor == UIPalette.OriginalThemePresets[UIPalette.UI.ButtonNeutral])
			{
				input.ThemeActiveAs = UIPalette.UI.ButtonNeutral;
			}
			else
			{
				input.ThemeActiveAs = UIPalette.UI.DoNotTheme;
			}
		}
		else
		{
			input.ThemeActiveAs = input.ThemeActiveAsSetting;
		}
		if (input.ThemeInactiveAsSetting == UIPalette.UI.Auto)
		{
			if (input.defaultColor == UIPalette.OriginalThemePresets[UIPalette.UI.InputTextInactive])
			{
				input.ThemeInactiveAs = UIPalette.UI.InputTextInactive;
			}
			else if (input.defaultColor == UIPalette.OriginalThemePresets[UIPalette.UI.Label])
			{
				input.ThemeInactiveAs = UIPalette.UI.Label;
			}
			else if (input.defaultColor == UIPalette.OriginalThemePresets[UIPalette.UI.High])
			{
				input.ThemeInactiveAs = UIPalette.UI.High;
			}
			else if (input.defaultColor == UIPalette.OriginalThemePresets[UIPalette.UI.Low])
			{
				input.ThemeInactiveAs = UIPalette.UI.Low;
			}
			else if (input.defaultColor == UIPalette.OriginalThemePresets[UIPalette.UI.ButtonNeutral])
			{
				input.ThemeInactiveAs = UIPalette.UI.ButtonNeutral;
			}
			else
			{
				input.ThemeInactiveAs = UIPalette.UI.DoNotTheme;
			}
		}
		else
		{
			input.ThemeInactiveAs = input.ThemeInactiveAsSetting;
		}
		if (input.ThemeCaretAsSetting == UIPalette.UI.Auto)
		{
			if (input.caretColor == UIPalette.OriginalThemePresets[UIPalette.UI.Caret])
			{
				input.ThemeCaretAs = UIPalette.UI.Caret;
			}
			else
			{
				input.ThemeCaretAs = UIPalette.UI.DoNotTheme;
			}
		}
		else
		{
			input.ThemeCaretAs = input.ThemeCaretAsSetting;
		}
		if (input.ThemeSelectionAsSetting == UIPalette.UI.Auto)
		{
			if (input.selectionColor == UIPalette.OriginalThemePresets[UIPalette.UI.Selection])
			{
				input.ThemeSelectionAs = UIPalette.UI.Selection;
			}
			else
			{
				input.ThemeSelectionAs = UIPalette.UI.DoNotTheme;
			}
		}
		else
		{
			input.ThemeSelectionAs = input.ThemeSelectionAsSetting;
		}
		Wait.Frames(delegate
		{
			this.SetColours(input, (theme == null) ? this.CurrentThemeColours : theme, true);
		}, 1);
	}

	// Token: 0x0600263E RID: 9790 RVA: 0x0010EC88 File Offset: 0x0010CE88
	public bool IsWindow(Transform t)
	{
		UIPopupList component = t.GetComponent<UIPopupList>();
		if (component != null)
		{
			if (component.ThemeBackgroundAs == UIPalette.UI.WindowBackground || component.ThemeBackgroundAsSetting == UIPalette.UI.WindowBackground)
			{
				return true;
			}
			if (component.ThemeBackgroundAsSetting == UIPalette.UI.Auto && component.backgroundColor == UIPalette.OriginalThemePresets[UIPalette.UI.WindowBackground])
			{
				return true;
			}
		}
		UISprite component2 = t.GetComponent<UISprite>();
		if (component2 != null)
		{
			if (component2.ThemeAs == UIPalette.UI.WindowBackground || component2.ThemeAsSetting == UIPalette.UI.WindowBackground)
			{
				return true;
			}
			if (component2.spriteName == "SquareWithDropShadow" && component2.color == UIPalette.OriginalThemePresets[UIPalette.UI.WindowBackground])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600263F RID: 9791 RVA: 0x0010ED34 File Offset: 0x0010CF34
	private void CheckClearTween(TweenColor tweener, bool instant)
	{
		if (tweener && (!tweener.gameObject.activeInHierarchy || instant))
		{
			tweener.value = tweener.to;
			tweener.from = tweener.to;
			tweener.enabled = false;
			Wait.Frames(delegate
			{
				if (tweener == null)
				{
					return;
				}
				tweener.value = tweener.to;
				tweener.from = tweener.to;
				tweener.enabled = false;
			}, 1);
		}
	}

	// Token: 0x06002640 RID: 9792 RVA: 0x0010EDBD File Offset: 0x0010CFBD
	private void CheckClearTween(UIWidgetContainer widget, bool instant)
	{
		this.CheckClearTween(widget.GetComponent<TweenColor>(), instant);
	}

	// Token: 0x06002641 RID: 9793 RVA: 0x0010EDBD File Offset: 0x0010CFBD
	private void CheckClearTween(UIRect rect, bool instant)
	{
		this.CheckClearTween(rect.GetComponent<TweenColor>(), instant);
	}

	// Token: 0x06002642 RID: 9794 RVA: 0x0010EDBD File Offset: 0x0010CFBD
	private void CheckClearTween(UIInput input, bool instant)
	{
		this.CheckClearTween(input.GetComponent<TweenColor>(), instant);
	}

	// Token: 0x06002643 RID: 9795 RVA: 0x0010EDCC File Offset: 0x0010CFCC
	public void SetColours(UIButton button, Dictionary<UIPalette.UI, Colour> theme, bool instant = false)
	{
		if (!button.BeenInit)
		{
			this.InitTheme(button, theme);
			return;
		}
		if (theme == null)
		{
			theme = this.CurrentThemeColours;
		}
		if (button.ThemeNormalAs != UIPalette.UI.DoNotTheme)
		{
			button.defaultColor = theme[button.ThemeNormalAs];
			button.CacheDefaultColor();
		}
		if (button.ThemeHoverAs != UIPalette.UI.DoNotTheme)
		{
			button.hover = theme[button.ThemeHoverAs];
		}
		if (button.ThemePressedAs != UIPalette.UI.DoNotTheme)
		{
			button.pressed = theme[button.ThemePressedAs];
		}
		if (button.ThemeDisabledAs != UIPalette.UI.DoNotTheme)
		{
			button.disabledColor = theme[button.ThemeDisabledAs];
		}
		button.UpdateColor(false);
		if (button)
		{
			this.CheckClearTween(button, instant);
		}
	}

	// Token: 0x06002644 RID: 9796 RVA: 0x0010EE94 File Offset: 0x0010D094
	public void SetColours(UILabel label, Dictionary<UIPalette.UI, Colour> theme, bool instant = false)
	{
		if (!label.BeenInit)
		{
			this.InitTheme(label, theme);
			return;
		}
		if (label.ThemeAs == UIPalette.UI.DoNotTheme)
		{
			return;
		}
		if (theme == null)
		{
			theme = this.CurrentThemeColours;
		}
		label.color = theme[label.ThemeAs];
		if (label)
		{
			this.CheckClearTween(label, instant);
		}
	}

	// Token: 0x06002645 RID: 9797 RVA: 0x0010EEF0 File Offset: 0x0010D0F0
	public void SetColours(UISprite sprite, Dictionary<UIPalette.UI, Colour> theme, bool instant = false)
	{
		if (!sprite.BeenInit)
		{
			this.InitTheme(sprite, theme);
			return;
		}
		if (sprite.ThemeAs == UIPalette.UI.DoNotTheme)
		{
			return;
		}
		if (theme == null)
		{
			theme = this.CurrentThemeColours;
		}
		if (sprite.spriteName == "RadioButton-Active" || sprite.spriteName == "Icon-RadioButtonMark" || sprite.spriteName == "Normal tab" || sprite.spriteName == "DropShadow")
		{
			Color color = theme[sprite.ThemeAs];
			color.a = sprite.color.a;
			sprite.color = color;
		}
		else
		{
			sprite.color = theme[sprite.ThemeAs];
		}
		if (sprite)
		{
			this.CheckClearTween(sprite, instant);
		}
	}

	// Token: 0x06002646 RID: 9798 RVA: 0x0010EFC0 File Offset: 0x0010D1C0
	public void SetColours(UIPopupList popup, Dictionary<UIPalette.UI, Colour> theme, bool instant = false)
	{
		if (!popup.BeenInit)
		{
			this.InitTheme(popup, theme);
			return;
		}
		if (theme == null)
		{
			theme = this.CurrentThemeColours;
		}
		if (popup.ThemeBackgroundAs != UIPalette.UI.DoNotTheme)
		{
			popup.backgroundColor = theme[popup.ThemeBackgroundAs];
		}
		if (popup.ThemeHighlightAs != UIPalette.UI.DoNotTheme)
		{
			popup.highlightColor = theme[popup.ThemeHighlightAs];
		}
		if (popup.ThemeLabelAs != UIPalette.UI.DoNotTheme)
		{
			popup.textColor = theme[popup.ThemeLabelAs];
		}
		if (popup)
		{
			this.CheckClearTween(popup, instant);
		}
	}

	// Token: 0x06002647 RID: 9799 RVA: 0x0010F05C File Offset: 0x0010D25C
	public void SetColours(UIInput input, Dictionary<UIPalette.UI, Colour> theme, bool instant = false)
	{
		if (!input.BeenInit)
		{
			this.InitTheme(input, theme);
			return;
		}
		if (theme == null)
		{
			theme = this.CurrentThemeColours;
		}
		if (input.ThemeActiveAs != UIPalette.UI.DoNotTheme)
		{
			input.activeTextColor = theme[input.ThemeActiveAs];
		}
		if (input.ThemeInactiveAs != UIPalette.UI.DoNotTheme)
		{
			input.defaultColor = theme[input.ThemeInactiveAs];
		}
		if (input.ThemeCaretAs != UIPalette.UI.DoNotTheme)
		{
			input.caretColor = theme[input.ThemeCaretAs];
		}
		if (input.ThemeSelectionAs != UIPalette.UI.DoNotTheme)
		{
			input.selectionColor = theme[input.ThemeSelectionAs];
		}
		input.UpdateLabel();
		if (input)
		{
			this.CheckClearTween(input, instant);
		}
	}

	// Token: 0x06002648 RID: 9800 RVA: 0x0010F11C File Offset: 0x0010D31C
	public void SetAdvancedFromSimple(UIPalette.UI simple, Colour colour)
	{
		if (!UIPalette.AdvancedFromSimple.ContainsKey(simple))
		{
			return;
		}
		for (int i = 0; i < UIPalette.AdvancedFromSimple[simple].Count; i++)
		{
			this.CurrentThemeColours[UIPalette.AdvancedFromSimple[simple][i]] = colour;
		}
		if (simple <= UIPalette.UI.Splash)
		{
			if (simple == UIPalette.UI.ButtonNormal)
			{
				colour.a = 0.21568628f;
				this.CurrentThemeColours[UIPalette.UI.TransparentBackground] = colour;
				return;
			}
			if (simple == UIPalette.UI.ButtonNeutral)
			{
				colour.a = 0.5f;
				this.CurrentThemeColours[UIPalette.UI.ChatTabBackground] = colour;
				return;
			}
			if (simple != UIPalette.UI.Splash)
			{
				return;
			}
			float a = colour.a;
			colour = Colour.Lighten(colour, 0.2f);
			colour.a = a;
			this.CurrentThemeColours[UIPalette.UI.SplashHighlight] = colour;
			this.CurrentThemeColours[UIPalette.UI.ContextMenuHighlight] = colour;
			return;
		}
		else if (simple <= UIPalette.UI.ChatOutputControls)
		{
			if (simple == UIPalette.UI.Motif)
			{
				float a = colour.a;
				colour = Colour.Lighten(colour, 0.1f);
				colour.a = a;
				this.CurrentThemeColours[UIPalette.UI.MotifHighlightA] = colour;
				this.CurrentThemeColours[UIPalette.UI.TooltipMotif] = colour;
				colour = Colour.Lighten(colour, 0.1f);
				colour.a = a;
				this.CurrentThemeColours[UIPalette.UI.MotifHighlightB] = colour;
				return;
			}
			if (simple != UIPalette.UI.ChatOutputControls)
			{
				return;
			}
			colour.a = Mathf.Lerp(0.8f, 0.2f, (colour.r + colour.g + colour.b) / 3f);
			this.CurrentThemeColours[UIPalette.UI.ChatInputBackground] = colour;
			this.CurrentThemeColours[UIPalette.UI.ChatOutputBackground] = colour;
			return;
		}
		else
		{
			if (simple == UIPalette.UI.PureTableA)
			{
				this.CurrentThemeColours[UIPalette.UI.PureSplash] = colour;
				float a = colour.a;
				colour = Colour.Lerp(colour, Colour.Grey, 0.2f);
				colour.a = a;
				this.CurrentThemeColours[UIPalette.UI.PureTableB] = colour;
				this.CurrentThemeColours[UIPalette.UI.PureTableASpecular] = Colour.White;
				this.CurrentThemeColours[UIPalette.UI.PureTableBSpecular] = Colour.White;
				this.CurrentThemeColours[UIPalette.UI.PureSplashSpecular] = Colour.White;
				return;
			}
			if (simple != UIPalette.UI.PureSkyHorizon)
			{
				return;
			}
			colour.a = 0.039215688f;
			this.CurrentThemeColours[UIPalette.UI.PureSkyHorizon] = colour;
			colour.a = 0.078431375f;
			this.CurrentThemeColours[UIPalette.UI.PureSkyAbove] = Colour.Lighten(colour, 0.2f);
			this.CurrentThemeColours[UIPalette.UI.PureSkyBelow] = Colour.Darken(colour, 0.2f);
			return;
		}
	}

	// Token: 0x06002649 RID: 9801 RVA: 0x0010F38C File Offset: 0x0010D58C
	public void DeleteTheme(int id)
	{
		if (id < 2 || id >= this.Themes.Count)
		{
			return;
		}
		Singleton<SystemConsole>.Instance.RemoveThemeCommand(this.Themes[id].name);
		if (id == this.Themes.Count - 1)
		{
			this.Themes.RemoveAt(id);
		}
		else
		{
			for (int i = id; i < this.Themes.Count - 1; i++)
			{
				this.Themes[i] = this.Themes[i + 1];
				if (this.Themes[i].id != -1)
				{
					this.Themes[i].id = i;
				}
			}
			this.Themes.RemoveAt(this.Themes.Count - 1);
		}
		this.StoreAllThemesInPrefs();
	}

	// Token: 0x0600264A RID: 9802 RVA: 0x0010F45C File Offset: 0x0010D65C
	public int StoreTheme(Dictionary<UIPalette.UI, Colour> colours, string name, int id = -1)
	{
		colours = new Dictionary<UIPalette.UI, Colour>(colours);
		if (id == -1)
		{
			for (int i = 0; i < this.Themes.Count; i++)
			{
				if (this.Themes[i].name.ToLower() == name.ToLower())
				{
					id = i;
					break;
				}
			}
			if (id == -1)
			{
				id = this.Themes.Count;
				this.Themes.Add(new Theme(id, name, colours));
			}
		}
		else if (id >= this.Themes.Count)
		{
			for (int j = this.Themes.Count; j < id; j++)
			{
				this.Themes.Add(new Theme(-1, "", null));
			}
			this.Themes.Add(new Theme(id, name, colours));
		}
		else if (this.Themes[id].name.ToLower() == name.ToLower())
		{
			this.Themes[id].colours = colours;
		}
		else
		{
			id = -1;
		}
		if (id != -1)
		{
			this.StoreThemeInPrefs(id);
			Singleton<SystemConsole>.Instance.CreateThemeCommand(name);
		}
		return id;
	}

	// Token: 0x0600264B RID: 9803 RVA: 0x0010F580 File Offset: 0x0010D780
	public void StoreThemeInPrefs(int id)
	{
		if (id == -2 || (id == -1 && Singleton<UIThemeEditor>.Instance.CurrentThemeID == -2))
		{
			return;
		}
		Theme theme;
		if (id == -1)
		{
			theme = new Theme(-1, Singleton<UIThemeEditor>.Instance.rawCurrentThemeName, this.CurrentThemeColours);
		}
		else
		{
			theme = this.Themes[id];
			if (theme.id == -1)
			{
				return;
			}
		}
		PlayerPrefs.SetString(UIPalette.PrefKey(id, UIPalette.UI.Auto), theme.name);
		foreach (KeyValuePair<UIPalette.UI, Colour> keyValuePair in theme.colours)
		{
			PlayerPrefs.SetString(UIPalette.PrefKey(id, keyValuePair.Key), UIPalette.ThemePrefColour(keyValuePair.Value));
		}
	}

	// Token: 0x0600264C RID: 9804 RVA: 0x0010F648 File Offset: 0x0010D848
	public void StoreAllThemesInPrefs()
	{
		for (int i = 2; i < this.Themes.Count; i++)
		{
			this.StoreThemeInPrefs(i);
		}
		this.ClearThemeInPrefs(this.Themes.Count);
	}

	// Token: 0x0600264D RID: 9805 RVA: 0x0010F684 File Offset: 0x0010D884
	public void ClearThemeInPrefs(int id)
	{
		PlayerPrefs.DeleteKey(UIPalette.PrefKey(id, UIPalette.UI.Auto));
		for (int i = 2; i < 88; i++)
		{
			PlayerPrefs.DeleteKey(UIPalette.PrefKey(id, (UIPalette.UI)i));
		}
	}

	// Token: 0x0600264E RID: 9806 RVA: 0x0010F6B8 File Offset: 0x0010D8B8
	public Theme LoadThemeFromPrefs(int id)
	{
		string @string = PlayerPrefs.GetString(UIPalette.PrefKey(id, UIPalette.UI.Auto), "");
		if (@string == "")
		{
			return null;
		}
		bool flag = true;
		Dictionary<UIPalette.UI, Colour> dictionary = new Dictionary<UIPalette.UI, Colour>(UIPalette.LightThemeColours);
		for (int i = 2; i < 88; i++)
		{
			string string2 = PlayerPrefs.GetString(UIPalette.PrefKey(id, (UIPalette.UI)i), "");
			if (string2 != "")
			{
				dictionary[(UIPalette.UI)i] = Colour.ColourFromRGBHex(string2);
				if (i == 13 && dictionary[(UIPalette.UI)i].r + dictionary[(UIPalette.UI)i].g + dictionary[(UIPalette.UI)i].b < 1.5f)
				{
					flag = false;
				}
			}
			else if (i >= 77 && i <= 83)
			{
				if (flag)
				{
					dictionary[(UIPalette.UI)i] = UIPalette.LightThemeColours[(UIPalette.UI)i];
				}
				else
				{
					dictionary[(UIPalette.UI)i] = UIPalette.DarkThemeColours[(UIPalette.UI)i];
				}
			}
		}
		return new Theme(id, @string, dictionary);
	}

	// Token: 0x0600264F RID: 9807 RVA: 0x0010F7A8 File Offset: 0x0010D9A8
	public static string PrefKey(int id, UIPalette.UI ui = UIPalette.UI.Auto)
	{
		if (ui == UIPalette.UI.Auto)
		{
			return "Theme" + id;
		}
		return string.Concat(new object[]
		{
			"Theme",
			id,
			":",
			(int)ui
		});
	}

	// Token: 0x06002650 RID: 9808 RVA: 0x0010F7F8 File Offset: 0x0010D9F8
	public static string ThemePrefColour(Colour colour)
	{
		string rawRGBHex = colour.RawRGBHex;
		return "#" + rawRGBHex;
	}

	// Token: 0x06002651 RID: 9809 RVA: 0x0010F81C File Offset: 0x0010DA1C
	public static string ThemeString(UIPalette.UI ui, Colour colour, int padCmdTo = 0)
	{
		string text = LibString.UnderscoreFromCamelCase(ui.ToString());
		while (text.Length < padCmdTo)
		{
			text += " ";
		}
		string text2 = colour.RawRGBHex;
		text2 = "#" + text2;
		return text + " " + text2;
	}

	// Token: 0x06002652 RID: 9810 RVA: 0x0010F873 File Offset: 0x0010DA73
	public static string CommandFromThemeString(string themeString)
	{
		return "ui_theme_color_" + themeString;
	}

	// Token: 0x06002653 RID: 9811 RVA: 0x0010F880 File Offset: 0x0010DA80
	public string ThemeExport(int id = -1)
	{
		Dictionary<UIPalette.UI, Colour> dictionary;
		string str;
		if (id == -1)
		{
			dictionary = this.CurrentThemeColours;
			str = Singleton<UIThemeEditor>.Instance.rawCurrentThemeName;
		}
		else
		{
			if (id < 0 || id >= this.Themes.Count)
			{
				return "";
			}
			if (this.Themes[id].id == -1)
			{
				return "";
			}
			dictionary = this.Themes[id].colours;
			str = this.Themes[id].name;
		}
		int num = 0;
		for (int i = 2; i < 88; i++)
		{
			UIPalette.UI ui = (UIPalette.UI)i;
			int length = LibString.UnderscoreFromCamelCase(ui.ToString()).Length;
			if (length > num)
			{
				num = length;
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("name " + str);
		stringBuilder.AppendLine();
		for (int j = 0; j < UIPalette.AdvancedThemeUIs.Count; j++)
		{
			UIPalette.UI ui2 = UIPalette.AdvancedThemeUIs[j];
			if (ui2 != UIPalette.UI.DoNotTheme && ui2 != UIPalette.UI.Auto)
			{
				stringBuilder.AppendLine(UIPalette.ThemeString(ui2, dictionary[ui2], num));
			}
			else
			{
				stringBuilder.AppendLine();
			}
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06002654 RID: 9812 RVA: 0x0010F9A8 File Offset: 0x0010DBA8
	public void ResetToDefaultLightTheme()
	{
		Singleton<UIThemeEditor>.Instance.LoadTheme(0, "");
	}

	// Token: 0x06002655 RID: 9813 RVA: 0x0010F9BA File Offset: 0x0010DBBA
	public static bool RestrictTransparency(UIPalette.UI ui)
	{
		return UIPalette.RESTRICT_TRANSPARENCY && ui != UIPalette.UI.PureSkyAbove && ui != UIPalette.UI.PureSkyHorizon && ui != UIPalette.UI.PureSkyBelow;
	}

	// Token: 0x040018D4 RID: 6356
	private const bool ALWAYS_CHANGE_COLOUR_INSTANTLY = false;

	// Token: 0x040018D5 RID: 6357
	public static bool ALLOW_GAME_TO_THEME = true;

	// Token: 0x040018D6 RID: 6358
	public static bool RESTRICT_TRANSPARENCY = true;

	// Token: 0x040018D7 RID: 6359
	public static float MIN_ALPHA = 0.5f;

	// Token: 0x040018D8 RID: 6360
	public static bool InBatchThemeUpdate = false;

	// Token: 0x040018D9 RID: 6361
	public static bool InGameThemeUpdate = false;

	// Token: 0x040018DA RID: 6362
	public const int UICount = 88;

	// Token: 0x040018DB RID: 6363
	public const int THEME_GAME = -2;

	// Token: 0x040018DC RID: 6364
	public const int THEME_LAST = -1;

	// Token: 0x040018DD RID: 6365
	public const int THEME_LIGHT = 0;

	// Token: 0x040018DE RID: 6366
	public const int THEME_DARK = 1;

	// Token: 0x040018DF RID: 6367
	public const int THEME_START = 2;

	// Token: 0x040018E0 RID: 6368
	public static Dictionary<UIPalette.UI, Colour> OriginalThemePresets = new Dictionary<UIPalette.UI, Colour>
	{
		{
			UIPalette.UI.ButtonNormal,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.ButtonHover,
			new Colour(28, 151, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.ButtonPressed,
			new Colour(142, 142, 142, byte.MaxValue)
		},
		{
			UIPalette.UI.ButtonDisabled,
			new Colour(142, 142, 142, byte.MaxValue)
		},
		{
			UIPalette.UI.ButtonHighlightA,
			new Colour(0.118f, 0.53f, 1f, 1f)
		},
		{
			UIPalette.UI.ButtonHighlightB,
			new Colour(byte.MaxValue, 73, 73, byte.MaxValue)
		},
		{
			UIPalette.UI.ButtonHighlightC,
			new Colour(245, 45, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.ButtonNeutral,
			new Colour(0.5f, 0.5f, 0.5f, 1f)
		},
		{
			UIPalette.UI.Label,
			new Colour(0, 0, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.WindowBackground,
			new Colour(235, 235, 235, byte.MaxValue)
		},
		{
			UIPalette.UI.Glow,
			new Colour(0, 0, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.ControlBackground,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.Low,
			new Colour(0, 0, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.High,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.TransparentBackground,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, 55)
		},
		{
			UIPalette.UI.FloatingText,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.NoteEditText,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.Caret,
			new Colour(0, 0, 0, 204)
		},
		{
			UIPalette.UI.Selection,
			new Colour(byte.MaxValue, 223, 141, 128)
		},
		{
			UIPalette.UI.TooltipText,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.TooltipBackground,
			new Colour(21, 21, 21, 200)
		},
		{
			UIPalette.UI.TooltipBorder,
			new Colour(0, 0, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.TooltipMotif,
			new Colour(0, 137, 234, byte.MaxValue)
		},
		{
			UIPalette.UI.InputTextInactive,
			new Colour(137, 137, 137, byte.MaxValue)
		},
		{
			UIPalette.UI.InputTextActive,
			new Colour(0, 0, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.Splash,
			new Colour(94, 183, 89, byte.MaxValue)
		},
		{
			UIPalette.UI.SplashHighlight,
			new Colour(0.527f, 0.689f, 0.517f, 1f)
		},
		{
			UIPalette.UI.RadioButtonBackground,
			new Colour(232, 232, 232, byte.MaxValue)
		},
		{
			UIPalette.UI.RadioButtonPressed,
			new Colour(232, 232, 232, byte.MaxValue)
		},
		{
			UIPalette.UI.CheckBoxBackground,
			new Colour(244, 244, 244, byte.MaxValue)
		},
		{
			UIPalette.UI.CheckBoxPressed,
			new Colour(232, 232, 232, byte.MaxValue)
		},
		{
			UIPalette.UI.SliderNormal,
			new Colour(114, 114, 114, byte.MaxValue)
		},
		{
			UIPalette.UI.SliderPressed,
			new Colour(193, 193, 193, byte.MaxValue)
		},
		{
			UIPalette.UI.SliderLabel,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.TabNormal,
			new Colour(byte.MaxValue, 251, 251, byte.MaxValue)
		},
		{
			UIPalette.UI.TabActive,
			new Colour(0, 137, 234, byte.MaxValue)
		},
		{
			UIPalette.UI.TabStrip,
			new Colour(0f, 0.539f, 0.919f, 1f)
		},
		{
			UIPalette.UI.Motif,
			new Colour(0f, 0.539f, 0.919f, 1f)
		},
		{
			UIPalette.UI.MotifHighlightA,
			new Colour(0f, 0.678f, 0.933f, 1f)
		},
		{
			UIPalette.UI.MotifHighlightB,
			new Colour(0.475f, 0.812f, 0.902f, 1f)
		},
		{
			UIPalette.UI.BackgroundTint,
			new Colour(0, 0, 0, 0)
		},
		{
			UIPalette.UI.ChatInputControls,
			new Colour(1f, 1f, 1f, 1f)
		},
		{
			UIPalette.UI.ChatInputBackground,
			new Colour(1f, 1f, 1f, 0.2f)
		},
		{
			UIPalette.UI.ChatInputText,
			new Colour(1f, 1f, 1f, 1f)
		},
		{
			UIPalette.UI.ChatOutputControls,
			new Colour(1f, 1f, 1f, 1f)
		},
		{
			UIPalette.UI.ChatOutputBackground,
			new Colour(1f, 1f, 1f, 0.2f)
		},
		{
			UIPalette.UI.ConsoleInputControls,
			new Colour(0f, 0f, 0f, 1f)
		},
		{
			UIPalette.UI.ConsoleInputBackground,
			new Colour(0f, 0f, 0f, 0.9f)
		},
		{
			UIPalette.UI.ConsoleInputText,
			new Colour(1f, 1f, 1f, 1f)
		},
		{
			UIPalette.UI.ConsoleOutputControls,
			new Colour(0f, 0f, 0f, 1f)
		},
		{
			UIPalette.UI.ConsoleOutputBackground,
			new Colour(0f, 0f, 0f, 0.9f)
		},
		{
			UIPalette.UI.Divider,
			new Colour(182, 182, 182, byte.MaxValue)
		},
		{
			UIPalette.UI.LuaBackground,
			new Colour(77, 77, 77, byte.MaxValue)
		},
		{
			UIPalette.UI.LuaText,
			new Colour(200, 200, 200, byte.MaxValue)
		},
		{
			UIPalette.UI.LuaCaret,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.LuaSelection,
			new Colour(200, 200, 200, 128)
		},
		{
			UIPalette.UI.ChatTabBackground,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, 128)
		},
		{
			UIPalette.UI.ChatTabHighlight,
			new Colour(231, 229, 44, 128)
		},
		{
			UIPalette.UI.ContextMenuBackground,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, 220)
		},
		{
			UIPalette.UI.ContextMenuText,
			new Colour(0, 0, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.ContextMenuHighlight,
			new Colour(byte.MaxValue, 0, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.ContextMenuHover,
			new Colour(223, 0, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.HoverHighlight,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.PureTableA,
			new Colour(217, 217, 217, byte.MaxValue)
		},
		{
			UIPalette.UI.PureTableASpecular,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.PureTableB,
			new Colour(195, 195, 195, byte.MaxValue)
		},
		{
			UIPalette.UI.PureTableBSpecular,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.PureSkyAbove,
			new Colour(190, 210, 230, 20)
		},
		{
			UIPalette.UI.PureSkyHorizon,
			new Colour(170, 180, 190, 10)
		},
		{
			UIPalette.UI.PureSkyBelow,
			new Colour(140, 160, 180, 10)
		},
		{
			UIPalette.UI.PureSplash,
			new Colour(217, 217, 217, byte.MaxValue)
		},
		{
			UIPalette.UI.PureSplashSpecular,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.MeasurementInner,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.MeasurementOuter,
			new Colour(0, 0, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.PlayerWhite,
			Colour.White
		},
		{
			UIPalette.UI.PlayerBrown,
			Colour.Brown
		},
		{
			UIPalette.UI.PlayerRed,
			Colour.Red
		},
		{
			UIPalette.UI.PlayerOrange,
			Colour.Orange
		},
		{
			UIPalette.UI.PlayerYellow,
			Colour.Yellow
		},
		{
			UIPalette.UI.PlayerGreen,
			Colour.Green
		},
		{
			UIPalette.UI.PlayerTeal,
			Colour.Teal
		},
		{
			UIPalette.UI.PlayerBlue,
			Colour.Blue
		},
		{
			UIPalette.UI.PlayerPurple,
			Colour.Purple
		},
		{
			UIPalette.UI.PlayerPink,
			Colour.Pink
		},
		{
			UIPalette.UI.PlayerGrey,
			Colour.Grey
		},
		{
			UIPalette.UI.PlayerBlack,
			Colour.Black
		}
	};

	// Token: 0x040018E1 RID: 6369
	public static Dictionary<UIPalette.UI, Colour> LightThemeColours = new Dictionary<UIPalette.UI, Colour>
	{
		{
			UIPalette.UI.ButtonNormal,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.ButtonHover,
			new Colour(28, 151, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.ButtonPressed,
			new Colour(142, 142, 142, byte.MaxValue)
		},
		{
			UIPalette.UI.ButtonDisabled,
			new Colour(142, 142, 142, byte.MaxValue)
		},
		{
			UIPalette.UI.ButtonHighlightA,
			new Colour(56, 155, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.ButtonHighlightB,
			new Colour(236, 46, 46, byte.MaxValue)
		},
		{
			UIPalette.UI.ButtonHighlightC,
			new Colour(245, 45, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.ButtonNeutral,
			new Colour(127, 127, 127, byte.MaxValue)
		},
		{
			UIPalette.UI.Label,
			new Colour(0, 0, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.WindowBackground,
			new Colour(235, 235, 235, byte.MaxValue)
		},
		{
			UIPalette.UI.Glow,
			new Colour(0, 0, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.ControlBackground,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.Low,
			new Colour(0, 0, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.High,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.TransparentBackground,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, 55)
		},
		{
			UIPalette.UI.FloatingText,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.NoteEditText,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.Caret,
			new Colour(0, 0, 0, 204)
		},
		{
			UIPalette.UI.Selection,
			new Colour(byte.MaxValue, 223, 141, 128)
		},
		{
			UIPalette.UI.TooltipText,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.TooltipBackground,
			new Colour(21, 21, 21, 200)
		},
		{
			UIPalette.UI.TooltipBorder,
			new Colour(0, 0, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.TooltipMotif,
			new Colour(0, 137, 234, byte.MaxValue)
		},
		{
			UIPalette.UI.InputTextInactive,
			new Colour(137, 137, 137, byte.MaxValue)
		},
		{
			UIPalette.UI.InputTextActive,
			new Colour(0, 0, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.Splash,
			new Colour(94, 183, 89, byte.MaxValue)
		},
		{
			UIPalette.UI.SplashHighlight,
			new Colour(131, 209, 78, byte.MaxValue)
		},
		{
			UIPalette.UI.RadioButtonBackground,
			new Colour(232, 232, 232, byte.MaxValue)
		},
		{
			UIPalette.UI.RadioButtonPressed,
			new Colour(232, 232, 232, byte.MaxValue)
		},
		{
			UIPalette.UI.CheckBoxBackground,
			new Colour(244, 244, 244, byte.MaxValue)
		},
		{
			UIPalette.UI.CheckBoxPressed,
			new Colour(232, 232, 232, byte.MaxValue)
		},
		{
			UIPalette.UI.SliderNormal,
			new Colour(114, 114, 114, byte.MaxValue)
		},
		{
			UIPalette.UI.SliderPressed,
			new Colour(193, 193, 193, byte.MaxValue)
		},
		{
			UIPalette.UI.SliderLabel,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.TabNormal,
			new Colour(byte.MaxValue, 251, 251, byte.MaxValue)
		},
		{
			UIPalette.UI.TabActive,
			new Colour(0, 137, 234, byte.MaxValue)
		},
		{
			UIPalette.UI.TabStrip,
			new Colour(0, 137, 234, byte.MaxValue)
		},
		{
			UIPalette.UI.Motif,
			new Colour(0, 137, 234, byte.MaxValue)
		},
		{
			UIPalette.UI.MotifHighlightA,
			new Colour(0, 173, 238, byte.MaxValue)
		},
		{
			UIPalette.UI.MotifHighlightB,
			new Colour(121, 207, 230, byte.MaxValue)
		},
		{
			UIPalette.UI.BackgroundTint,
			new Colour(0, 0, 0, 0)
		},
		{
			UIPalette.UI.ChatInputControls,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.ChatInputText,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.ChatInputBackground,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, 51)
		},
		{
			UIPalette.UI.ChatOutputControls,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.ChatOutputBackground,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, 51)
		},
		{
			UIPalette.UI.ConsoleInputControls,
			new Colour(0, 0, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.ConsoleInputText,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.ConsoleInputBackground,
			new Colour(0, 0, 0, 230)
		},
		{
			UIPalette.UI.ConsoleOutputControls,
			new Colour(0, 0, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.ConsoleOutputBackground,
			new Colour(0, 0, 0, 230)
		},
		{
			UIPalette.UI.Divider,
			new Colour(182, 182, 182, byte.MaxValue)
		},
		{
			UIPalette.UI.LuaBackground,
			new Colour(77, 77, 77, byte.MaxValue)
		},
		{
			UIPalette.UI.LuaText,
			new Colour(200, 200, 200, byte.MaxValue)
		},
		{
			UIPalette.UI.LuaCaret,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.LuaSelection,
			new Colour(200, 200, 200, 128)
		},
		{
			UIPalette.UI.ChatTabBackground,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, 128)
		},
		{
			UIPalette.UI.ChatTabHighlight,
			new Colour(231, 229, 44, 128)
		},
		{
			UIPalette.UI.ContextMenuBackground,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, 220)
		},
		{
			UIPalette.UI.ContextMenuText,
			new Colour(0, 0, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.ContextMenuHighlight,
			new Colour(byte.MaxValue, 0, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.ContextMenuHover,
			new Colour(28, 151, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.HoverHighlight,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.PureTableA,
			new Colour(217, 217, 217, byte.MaxValue)
		},
		{
			UIPalette.UI.PureTableASpecular,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.PureTableB,
			new Colour(195, 195, 195, byte.MaxValue)
		},
		{
			UIPalette.UI.PureTableBSpecular,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.PureSkyAbove,
			new Colour(190, 210, 230, 20)
		},
		{
			UIPalette.UI.PureSkyHorizon,
			new Colour(170, 180, 190, 10)
		},
		{
			UIPalette.UI.PureSkyBelow,
			new Colour(140, 160, 180, 10)
		},
		{
			UIPalette.UI.PureSplash,
			new Colour(217, 217, 217, byte.MaxValue)
		},
		{
			UIPalette.UI.PureSplashSpecular,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.MeasurementInner,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.MeasurementOuter,
			new Colour(0, 0, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.PlayerWhite,
			Colour.White
		},
		{
			UIPalette.UI.PlayerBrown,
			Colour.Brown
		},
		{
			UIPalette.UI.PlayerRed,
			Colour.Red
		},
		{
			UIPalette.UI.PlayerOrange,
			Colour.Orange
		},
		{
			UIPalette.UI.PlayerYellow,
			Colour.Yellow
		},
		{
			UIPalette.UI.PlayerGreen,
			Colour.Green
		},
		{
			UIPalette.UI.PlayerTeal,
			Colour.Teal
		},
		{
			UIPalette.UI.PlayerBlue,
			Colour.Blue
		},
		{
			UIPalette.UI.PlayerPurple,
			Colour.Purple
		},
		{
			UIPalette.UI.PlayerPink,
			Colour.Pink
		},
		{
			UIPalette.UI.PlayerGrey,
			Colour.Grey
		},
		{
			UIPalette.UI.PlayerBlack,
			Colour.Black
		}
	};

	// Token: 0x040018E2 RID: 6370
	public static Dictionary<UIPalette.UI, Colour> DarkThemeColours = new Dictionary<UIPalette.UI, Colour>
	{
		{
			UIPalette.UI.ButtonNormal,
			new Colour(40, 40, 40, byte.MaxValue)
		},
		{
			UIPalette.UI.ButtonHover,
			new Colour(225, 141, 21, byte.MaxValue)
		},
		{
			UIPalette.UI.ButtonPressed,
			new Colour(142, 142, 142, byte.MaxValue)
		},
		{
			UIPalette.UI.ButtonDisabled,
			new Colour(142, 142, 142, byte.MaxValue)
		},
		{
			UIPalette.UI.ButtonHighlightA,
			new Colour(byte.MaxValue, 155, 56, byte.MaxValue)
		},
		{
			UIPalette.UI.ButtonHighlightB,
			new Colour(200, 50, 155, byte.MaxValue)
		},
		{
			UIPalette.UI.ButtonHighlightC,
			new Colour(50, 155, 200, byte.MaxValue)
		},
		{
			UIPalette.UI.ButtonNeutral,
			new Colour(127, 127, 127, byte.MaxValue)
		},
		{
			UIPalette.UI.Label,
			new Colour(240, 240, 240, byte.MaxValue)
		},
		{
			UIPalette.UI.WindowBackground,
			new Colour(30, 30, 30, byte.MaxValue)
		},
		{
			UIPalette.UI.Glow,
			new Colour(0, 0, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.ControlBackground,
			new Colour(20, 20, 20, byte.MaxValue)
		},
		{
			UIPalette.UI.Low,
			new Colour(240, 240, 240, byte.MaxValue)
		},
		{
			UIPalette.UI.High,
			new Colour(0, 0, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.TransparentBackground,
			new Colour(105, 105, 105, 155)
		},
		{
			UIPalette.UI.FloatingText,
			new Colour(55, 55, 55, byte.MaxValue)
		},
		{
			UIPalette.UI.NoteEditText,
			new Colour(155, 155, 155, byte.MaxValue)
		},
		{
			UIPalette.UI.Caret,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, 204)
		},
		{
			UIPalette.UI.Selection,
			new Colour(byte.MaxValue, 223, 141, 128)
		},
		{
			UIPalette.UI.TooltipText,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.TooltipBackground,
			new Colour(21, 21, 21, 200)
		},
		{
			UIPalette.UI.TooltipBorder,
			new Colour(0, 0, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.TooltipMotif,
			new Colour(220, 130, 50, byte.MaxValue)
		},
		{
			UIPalette.UI.InputTextInactive,
			new Colour(137, 137, 137, byte.MaxValue)
		},
		{
			UIPalette.UI.InputTextActive,
			new Colour(220, 220, 220, byte.MaxValue)
		},
		{
			UIPalette.UI.Splash,
			new Colour(95, 183, 89, byte.MaxValue)
		},
		{
			UIPalette.UI.SplashHighlight,
			new Colour(131, 209, 78, byte.MaxValue)
		},
		{
			UIPalette.UI.RadioButtonBackground,
			new Colour(40, 40, 40, byte.MaxValue)
		},
		{
			UIPalette.UI.RadioButtonPressed,
			new Colour(60, 60, 60, byte.MaxValue)
		},
		{
			UIPalette.UI.CheckBoxBackground,
			new Colour(40, 40, 40, byte.MaxValue)
		},
		{
			UIPalette.UI.CheckBoxPressed,
			new Colour(60, 60, 60, byte.MaxValue)
		},
		{
			UIPalette.UI.SliderNormal,
			new Colour(60, 60, 60, byte.MaxValue)
		},
		{
			UIPalette.UI.SliderPressed,
			new Colour(80, 80, 80, byte.MaxValue)
		},
		{
			UIPalette.UI.SliderLabel,
			new Colour(155, 155, 155, byte.MaxValue)
		},
		{
			UIPalette.UI.TabNormal,
			new Colour(30, 30, 30, byte.MaxValue)
		},
		{
			UIPalette.UI.TabActive,
			new Colour(200, 110, 30, byte.MaxValue)
		},
		{
			UIPalette.UI.TabStrip,
			new Colour(200, 110, 30, byte.MaxValue)
		},
		{
			UIPalette.UI.Motif,
			new Colour(200, 110, 30, byte.MaxValue)
		},
		{
			UIPalette.UI.MotifHighlightA,
			new Colour(180, 90, 20, byte.MaxValue)
		},
		{
			UIPalette.UI.MotifHighlightB,
			new Colour(160, 70, 20, byte.MaxValue)
		},
		{
			UIPalette.UI.BackgroundTint,
			new Colour(0, 0, 0, 210)
		},
		{
			UIPalette.UI.ChatInputControls,
			new Colour(127, 127, 127, byte.MaxValue)
		},
		{
			UIPalette.UI.ChatInputText,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.ChatInputBackground,
			new Colour(127, 127, 127, 51)
		},
		{
			UIPalette.UI.ChatOutputControls,
			new Colour(127, 127, 127, byte.MaxValue)
		},
		{
			UIPalette.UI.ChatOutputBackground,
			new Colour(127, 127, 127, 51)
		},
		{
			UIPalette.UI.ConsoleInputControls,
			new Colour(0, 0, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.ConsoleInputText,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.ConsoleInputBackground,
			new Colour(0, 0, 0, 230)
		},
		{
			UIPalette.UI.ConsoleOutputControls,
			new Colour(0, 0, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.ConsoleOutputBackground,
			new Colour(0, 0, 0, 230)
		},
		{
			UIPalette.UI.Divider,
			new Colour(64, 64, 64, byte.MaxValue)
		},
		{
			UIPalette.UI.LuaBackground,
			new Colour(77, 77, 77, byte.MaxValue)
		},
		{
			UIPalette.UI.LuaText,
			new Colour(200, 200, 200, byte.MaxValue)
		},
		{
			UIPalette.UI.LuaCaret,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.LuaSelection,
			new Colour(200, 200, 200, 128)
		},
		{
			UIPalette.UI.ChatTabBackground,
			new Colour(127, 127, 127, 128)
		},
		{
			UIPalette.UI.ChatTabHighlight,
			new Colour(byte.MaxValue, 192, 50, 128)
		},
		{
			UIPalette.UI.ContextMenuBackground,
			new Colour(30, 30, 30, 250)
		},
		{
			UIPalette.UI.ContextMenuText,
			new Colour(240, 240, 240, byte.MaxValue)
		},
		{
			UIPalette.UI.ContextMenuHighlight,
			new Colour(131, 209, 78, byte.MaxValue)
		},
		{
			UIPalette.UI.ContextMenuHover,
			new Colour(225, 141, 21, byte.MaxValue)
		},
		{
			UIPalette.UI.HoverHighlight,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.PureTableA,
			new Colour(59, 59, 59, byte.MaxValue)
		},
		{
			UIPalette.UI.PureTableASpecular,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.PureTableB,
			new Colour(94, 94, 94, byte.MaxValue)
		},
		{
			UIPalette.UI.PureTableBSpecular,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.PureSkyAbove,
			new Colour(32, 42, 52, 10)
		},
		{
			UIPalette.UI.PureSkyHorizon,
			new Colour(30, 40, 50, 20)
		},
		{
			UIPalette.UI.PureSkyBelow,
			new Colour(30, 37, 47, 10)
		},
		{
			UIPalette.UI.PureSplash,
			new Colour(59, 59, 59, byte.MaxValue)
		},
		{
			UIPalette.UI.PureSplashSpecular,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.MeasurementInner,
			new Colour(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			UIPalette.UI.MeasurementOuter,
			new Colour(0, 0, 0, byte.MaxValue)
		},
		{
			UIPalette.UI.PlayerWhite,
			Colour.White
		},
		{
			UIPalette.UI.PlayerBrown,
			Colour.Brown
		},
		{
			UIPalette.UI.PlayerRed,
			Colour.Red
		},
		{
			UIPalette.UI.PlayerOrange,
			Colour.Orange
		},
		{
			UIPalette.UI.PlayerYellow,
			Colour.Yellow
		},
		{
			UIPalette.UI.PlayerGreen,
			Colour.Green
		},
		{
			UIPalette.UI.PlayerTeal,
			Colour.Teal
		},
		{
			UIPalette.UI.PlayerBlue,
			Colour.Blue
		},
		{
			UIPalette.UI.PlayerPurple,
			Colour.Purple
		},
		{
			UIPalette.UI.PlayerPink,
			Colour.Pink
		},
		{
			UIPalette.UI.PlayerGrey,
			Colour.Grey
		},
		{
			UIPalette.UI.PlayerBlack,
			Colour.Black
		}
	};

	// Token: 0x040018E3 RID: 6371
	public static Dictionary<UIPalette.UI, Colour> LastThemeColours = new Dictionary<UIPalette.UI, Colour>(UIPalette.LightThemeColours);

	// Token: 0x040018E4 RID: 6372
	public static Theme GameTheme = new Theme(-2, "Game", null);

	// Token: 0x040018E5 RID: 6373
	public List<Theme> Themes = new List<Theme>
	{
		new Theme(0, "Light", UIPalette.LightThemeColours),
		new Theme(1, "Dark", UIPalette.DarkThemeColours)
	};

	// Token: 0x040018E6 RID: 6374
	public static List<UIPalette.UI> AdvancedThemeUIs = new List<UIPalette.UI>
	{
		UIPalette.UI.ButtonNormal,
		UIPalette.UI.ButtonHover,
		UIPalette.UI.ButtonPressed,
		UIPalette.UI.ButtonDisabled,
		UIPalette.UI.ButtonHighlightA,
		UIPalette.UI.ButtonHighlightB,
		UIPalette.UI.ButtonHighlightC,
		UIPalette.UI.ButtonNeutral,
		UIPalette.UI.DoNotTheme,
		UIPalette.UI.WindowBackground,
		UIPalette.UI.ControlBackground,
		UIPalette.UI.TransparentBackground,
		UIPalette.UI.Divider,
		UIPalette.UI.DoNotTheme,
		UIPalette.UI.TabNormal,
		UIPalette.UI.TabActive,
		UIPalette.UI.TabStrip,
		UIPalette.UI.DoNotTheme,
		UIPalette.UI.Motif,
		UIPalette.UI.MotifHighlightA,
		UIPalette.UI.MotifHighlightB,
		UIPalette.UI.Splash,
		UIPalette.UI.SplashHighlight,
		UIPalette.UI.Glow,
		UIPalette.UI.DoNotTheme,
		UIPalette.UI.ContextMenuBackground,
		UIPalette.UI.ContextMenuHover,
		UIPalette.UI.ContextMenuText,
		UIPalette.UI.ContextMenuHighlight,
		UIPalette.UI.DoNotTheme,
		UIPalette.UI.Label,
		UIPalette.UI.InputTextInactive,
		UIPalette.UI.InputTextActive,
		UIPalette.UI.FloatingText,
		UIPalette.UI.NoteEditText,
		UIPalette.UI.Caret,
		UIPalette.UI.Selection,
		UIPalette.UI.DoNotTheme,
		UIPalette.UI.TooltipBackground,
		UIPalette.UI.TooltipBorder,
		UIPalette.UI.TooltipText,
		UIPalette.UI.TooltipMotif,
		UIPalette.UI.DoNotTheme,
		UIPalette.UI.MeasurementInner,
		UIPalette.UI.MeasurementOuter,
		UIPalette.UI.DoNotTheme,
		UIPalette.UI.RadioButtonBackground,
		UIPalette.UI.RadioButtonPressed,
		UIPalette.UI.DoNotTheme,
		UIPalette.UI.CheckBoxBackground,
		UIPalette.UI.CheckBoxPressed,
		UIPalette.UI.DoNotTheme,
		UIPalette.UI.SliderNormal,
		UIPalette.UI.SliderPressed,
		UIPalette.UI.SliderLabel,
		UIPalette.UI.DoNotTheme,
		UIPalette.UI.ChatTabBackground,
		UIPalette.UI.ChatTabHighlight,
		UIPalette.UI.DoNotTheme,
		UIPalette.UI.ChatOutputBackground,
		UIPalette.UI.ChatOutputControls,
		UIPalette.UI.ChatInputBackground,
		UIPalette.UI.ChatInputControls,
		UIPalette.UI.ChatInputText,
		UIPalette.UI.DoNotTheme,
		UIPalette.UI.ConsoleOutputBackground,
		UIPalette.UI.ConsoleOutputControls,
		UIPalette.UI.ConsoleInputBackground,
		UIPalette.UI.ConsoleInputControls,
		UIPalette.UI.ConsoleInputText,
		UIPalette.UI.DoNotTheme,
		UIPalette.UI.LuaBackground,
		UIPalette.UI.LuaText,
		UIPalette.UI.LuaCaret,
		UIPalette.UI.LuaSelection,
		UIPalette.UI.DoNotTheme,
		UIPalette.UI.High,
		UIPalette.UI.Low,
		UIPalette.UI.HoverHighlight,
		UIPalette.UI.BackgroundTint,
		UIPalette.UI.DoNotTheme,
		UIPalette.UI.PureTableA,
		UIPalette.UI.PureTableB,
		UIPalette.UI.PureSplash,
		UIPalette.UI.PureSkyAbove,
		UIPalette.UI.PureSkyHorizon,
		UIPalette.UI.PureSkyBelow
	};

	// Token: 0x040018E7 RID: 6375
	public static List<UIPalette.UI> SimpleThemeUIs = new List<UIPalette.UI>
	{
		UIPalette.UI.ButtonNormal,
		UIPalette.UI.ButtonHover,
		UIPalette.UI.ButtonPressed,
		UIPalette.UI.ButtonHighlightA,
		UIPalette.UI.ButtonHighlightB,
		UIPalette.UI.ButtonNeutral,
		UIPalette.UI.DoNotTheme,
		UIPalette.UI.WindowBackground,
		UIPalette.UI.Divider,
		UIPalette.UI.TabNormal,
		UIPalette.UI.Motif,
		UIPalette.UI.Splash,
		UIPalette.UI.Label,
		UIPalette.UI.Selection,
		UIPalette.UI.DoNotTheme,
		UIPalette.UI.ContextMenuBackground,
		UIPalette.UI.TooltipBackground,
		UIPalette.UI.TooltipText,
		UIPalette.UI.DoNotTheme,
		UIPalette.UI.ChatTabHighlight,
		UIPalette.UI.ChatOutputControls,
		UIPalette.UI.ChatInputText,
		UIPalette.UI.ConsoleOutputControls,
		UIPalette.UI.ConsoleInputText,
		UIPalette.UI.DoNotTheme,
		UIPalette.UI.LuaBackground,
		UIPalette.UI.LuaText,
		UIPalette.UI.DoNotTheme,
		UIPalette.UI.PureTableA,
		UIPalette.UI.PureSkyHorizon
	};

	// Token: 0x040018E8 RID: 6376
	public static Dictionary<UIPalette.UI, List<UIPalette.UI>> AdvancedFromSimple = new Dictionary<UIPalette.UI, List<UIPalette.UI>>
	{
		{
			UIPalette.UI.ButtonNormal,
			new List<UIPalette.UI>
			{
				UIPalette.UI.ControlBackground,
				UIPalette.UI.High,
				UIPalette.UI.FloatingText,
				UIPalette.UI.NoteEditText,
				UIPalette.UI.Caret,
				UIPalette.UI.LuaCaret,
				UIPalette.UI.CheckBoxBackground,
				UIPalette.UI.SliderLabel,
				UIPalette.UI.HoverHighlight,
				UIPalette.UI.ContextMenuHover,
				UIPalette.UI.MeasurementInner
			}
		},
		{
			UIPalette.UI.ButtonPressed,
			new List<UIPalette.UI>
			{
				UIPalette.UI.ButtonDisabled,
				UIPalette.UI.InputTextInactive,
				UIPalette.UI.SliderPressed
			}
		},
		{
			UIPalette.UI.ButtonNeutral,
			new List<UIPalette.UI>
			{
				UIPalette.UI.SliderNormal
			}
		},
		{
			UIPalette.UI.Label,
			new List<UIPalette.UI>
			{
				UIPalette.UI.Low,
				UIPalette.UI.TooltipBorder,
				UIPalette.UI.InputTextActive,
				UIPalette.UI.ContextMenuText,
				UIPalette.UI.MeasurementOuter
			}
		},
		{
			UIPalette.UI.WindowBackground,
			new List<UIPalette.UI>
			{
				UIPalette.UI.RadioButtonBackground,
				UIPalette.UI.RadioButtonPressed,
				UIPalette.UI.CheckBoxPressed
			}
		},
		{
			UIPalette.UI.Motif,
			new List<UIPalette.UI>
			{
				UIPalette.UI.TabActive,
				UIPalette.UI.TabStrip
			}
		},
		{
			UIPalette.UI.Selection,
			new List<UIPalette.UI>
			{
				UIPalette.UI.LuaSelection
			}
		},
		{
			UIPalette.UI.Splash,
			new List<UIPalette.UI>()
		},
		{
			UIPalette.UI.ChatOutputControls,
			new List<UIPalette.UI>
			{
				UIPalette.UI.ChatInputControls
			}
		},
		{
			UIPalette.UI.ConsoleOutputControls,
			new List<UIPalette.UI>
			{
				UIPalette.UI.ConsoleInputControls,
				UIPalette.UI.ConsoleInputBackground,
				UIPalette.UI.ConsoleOutputBackground
			}
		},
		{
			UIPalette.UI.PureTableA,
			new List<UIPalette.UI>()
		},
		{
			UIPalette.UI.PureSkyHorizon,
			new List<UIPalette.UI>()
		}
	};

	// Token: 0x040018E9 RID: 6377
	[NonSerialized]
	public Dictionary<UIPalette.UI, Colour> CurrentThemeColours = new Dictionary<UIPalette.UI, Colour>(UIPalette.LightThemeColours);

	// Token: 0x040018EA RID: 6378
	private int lastUIThemeEvent;

	// Token: 0x040018EB RID: 6379
	private bool storeTheme;

	// Token: 0x040018EC RID: 6380
	private bool themeRefreshing;

	// Token: 0x040018ED RID: 6381
	private List<string> UnthemeableSprites = new List<string>
	{
		"CheckMarkSM",
		"Icon-CheckBoxMark",
		"Trash"
	};

	// Token: 0x02000776 RID: 1910
	public enum UI
	{
		// Token: 0x04002C02 RID: 11266
		Auto,
		// Token: 0x04002C03 RID: 11267
		DoNotTheme,
		// Token: 0x04002C04 RID: 11268
		ButtonNormal,
		// Token: 0x04002C05 RID: 11269
		ButtonHover,
		// Token: 0x04002C06 RID: 11270
		ButtonPressed,
		// Token: 0x04002C07 RID: 11271
		ButtonDisabled,
		// Token: 0x04002C08 RID: 11272
		ButtonHighlightA,
		// Token: 0x04002C09 RID: 11273
		ButtonHighlightB,
		// Token: 0x04002C0A RID: 11274
		ButtonNeutral,
		// Token: 0x04002C0B RID: 11275
		Label,
		// Token: 0x04002C0C RID: 11276
		WindowBackground,
		// Token: 0x04002C0D RID: 11277
		ControlBackground,
		// Token: 0x04002C0E RID: 11278
		Low,
		// Token: 0x04002C0F RID: 11279
		High,
		// Token: 0x04002C10 RID: 11280
		TransparentBackground,
		// Token: 0x04002C11 RID: 11281
		FloatingText,
		// Token: 0x04002C12 RID: 11282
		NoteEditText,
		// Token: 0x04002C13 RID: 11283
		Caret,
		// Token: 0x04002C14 RID: 11284
		Selection,
		// Token: 0x04002C15 RID: 11285
		TooltipText,
		// Token: 0x04002C16 RID: 11286
		TooltipBackground,
		// Token: 0x04002C17 RID: 11287
		TooltipBorder,
		// Token: 0x04002C18 RID: 11288
		InputTextInactive,
		// Token: 0x04002C19 RID: 11289
		InputTextActive,
		// Token: 0x04002C1A RID: 11290
		Splash,
		// Token: 0x04002C1B RID: 11291
		SplashHighlight,
		// Token: 0x04002C1C RID: 11292
		RadioButtonBackground,
		// Token: 0x04002C1D RID: 11293
		RadioButtonPressed,
		// Token: 0x04002C1E RID: 11294
		CheckBoxBackground,
		// Token: 0x04002C1F RID: 11295
		CheckBoxPressed,
		// Token: 0x04002C20 RID: 11296
		SliderNormal,
		// Token: 0x04002C21 RID: 11297
		SliderPressed,
		// Token: 0x04002C22 RID: 11298
		SliderLabel,
		// Token: 0x04002C23 RID: 11299
		TabNormal,
		// Token: 0x04002C24 RID: 11300
		TabActive,
		// Token: 0x04002C25 RID: 11301
		TabStrip,
		// Token: 0x04002C26 RID: 11302
		Motif,
		// Token: 0x04002C27 RID: 11303
		MotifHighlightA,
		// Token: 0x04002C28 RID: 11304
		MotifHighlightB,
		// Token: 0x04002C29 RID: 11305
		BackgroundTint,
		// Token: 0x04002C2A RID: 11306
		ChatOutputControls,
		// Token: 0x04002C2B RID: 11307
		ChatOutputBackground,
		// Token: 0x04002C2C RID: 11308
		ChatInputControls,
		// Token: 0x04002C2D RID: 11309
		ChatInputBackground,
		// Token: 0x04002C2E RID: 11310
		ChatInputText,
		// Token: 0x04002C2F RID: 11311
		ConsoleOutputControls,
		// Token: 0x04002C30 RID: 11312
		ConsoleOutputBackground,
		// Token: 0x04002C31 RID: 11313
		ConsoleInputControls,
		// Token: 0x04002C32 RID: 11314
		ConsoleInputBackground,
		// Token: 0x04002C33 RID: 11315
		ConsoleInputText,
		// Token: 0x04002C34 RID: 11316
		Divider,
		// Token: 0x04002C35 RID: 11317
		TooltipMotif,
		// Token: 0x04002C36 RID: 11318
		LuaBackground,
		// Token: 0x04002C37 RID: 11319
		LuaText,
		// Token: 0x04002C38 RID: 11320
		LuaCaret,
		// Token: 0x04002C39 RID: 11321
		LuaSelection,
		// Token: 0x04002C3A RID: 11322
		ChatTabBackground,
		// Token: 0x04002C3B RID: 11323
		ChatTabHighlight,
		// Token: 0x04002C3C RID: 11324
		ContextMenuBackground,
		// Token: 0x04002C3D RID: 11325
		ContextMenuText,
		// Token: 0x04002C3E RID: 11326
		ContextMenuHighlight,
		// Token: 0x04002C3F RID: 11327
		HoverHighlight,
		// Token: 0x04002C40 RID: 11328
		PlayerWhite,
		// Token: 0x04002C41 RID: 11329
		PlayerBrown,
		// Token: 0x04002C42 RID: 11330
		PlayerRed,
		// Token: 0x04002C43 RID: 11331
		PlayerOrange,
		// Token: 0x04002C44 RID: 11332
		PlayerYellow,
		// Token: 0x04002C45 RID: 11333
		PlayerGreen,
		// Token: 0x04002C46 RID: 11334
		PlayerTeal,
		// Token: 0x04002C47 RID: 11335
		PlayerBlue,
		// Token: 0x04002C48 RID: 11336
		PlayerPurple,
		// Token: 0x04002C49 RID: 11337
		PlayerPink,
		// Token: 0x04002C4A RID: 11338
		PlayerGrey,
		// Token: 0x04002C4B RID: 11339
		PlayerBlack,
		// Token: 0x04002C4C RID: 11340
		Glow,
		// Token: 0x04002C4D RID: 11341
		ContextMenuHover,
		// Token: 0x04002C4E RID: 11342
		ButtonHighlightC,
		// Token: 0x04002C4F RID: 11343
		PureTableA,
		// Token: 0x04002C50 RID: 11344
		PureTableASpecular,
		// Token: 0x04002C51 RID: 11345
		PureTableB,
		// Token: 0x04002C52 RID: 11346
		PureTableBSpecular,
		// Token: 0x04002C53 RID: 11347
		PureSkyAbove,
		// Token: 0x04002C54 RID: 11348
		PureSplash,
		// Token: 0x04002C55 RID: 11349
		PureSplashSpecular,
		// Token: 0x04002C56 RID: 11350
		MeasurementInner,
		// Token: 0x04002C57 RID: 11351
		MeasurementOuter,
		// Token: 0x04002C58 RID: 11352
		PureSkyHorizon,
		// Token: 0x04002C59 RID: 11353
		PureSkyBelow
	}
}
