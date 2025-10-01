using System;
using System.Collections.Generic;
using System.IO;
using I2.Loc;
using UnityEngine;

// Token: 0x020002FC RID: 764
public class UILanguageSettings : Singleton<UILanguageSettings>
{
	// Token: 0x060024C9 RID: 9417 RVA: 0x001039C8 File Offset: 0x00101BC8
	public void Start()
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
	}

	// Token: 0x060024CA RID: 9418 RVA: 0x001039D8 File Offset: 0x00101BD8
	protected override void Awake()
	{
		base.Awake();
		EventManager.OnUIThemeChange += this.OnThemeChange;
	}

	// Token: 0x060024CB RID: 9419 RVA: 0x001039F1 File Offset: 0x00101BF1
	private void OnDestroy()
	{
		EventManager.OnUIThemeChange -= this.OnThemeChange;
	}

	// Token: 0x060024CC RID: 9420 RVA: 0x00103A04 File Offset: 0x00101C04
	private void OnEnable()
	{
		Singleton<Language>.Instance.CheckForTranslationFiles();
		this.OnThemeChange();
	}

	// Token: 0x060024CD RID: 9421 RVA: 0x00103A18 File Offset: 0x00101C18
	public void Display()
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
		this.selectedLanguageCode = (this.storedLanguageCode = Language.CurrentLanguageCode);
		this.selectedTranslationFilename = (this.storedTranslationFilename = Language.CSVFilenameFromCode[this.storedLanguageCode]);
		Translation translation = Singleton<Language>.Instance.TranslationFromCodeAndFilename(this.selectedLanguageCode, this.selectedTranslationFilename);
		this.CurrentLanguageLabel.text = string.Concat(new string[]
		{
			Language.NativeLanguageFromCode[this.selectedLanguageCode],
			" : ",
			translation.title,
			" [",
			translation.author,
			"]"
		});
		this.RefreshLanguages();
		this.RefreshTranslations();
		base.gameObject.SetActive(true);
	}

	// Token: 0x060024CE RID: 9422 RVA: 0x00103AE8 File Offset: 0x00101CE8
	public void ConfirmAndClose()
	{
		this.Hide();
	}

	// Token: 0x060024CF RID: 9423 RVA: 0x00103AF0 File Offset: 0x00101CF0
	public void CancelAndClose()
	{
		Singleton<Language>.Instance.SetLanguageAndTranslation(this.storedLanguageCode, this.storedTranslationFilename);
		this.Hide();
	}

	// Token: 0x060024D0 RID: 9424 RVA: 0x00103B0F File Offset: 0x00101D0F
	public void ToggleVisibility()
	{
		if (this.IsVisible())
		{
			this.Hide();
			return;
		}
		this.Display();
	}

	// Token: 0x060024D1 RID: 9425 RVA: 0x000BCD9D File Offset: 0x000BAF9D
	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x060024D2 RID: 9426 RVA: 0x00103B26 File Offset: 0x00101D26
	public bool IsVisible()
	{
		return base.gameObject.activeInHierarchy;
	}

	// Token: 0x060024D3 RID: 9427 RVA: 0x00103B34 File Offset: 0x00101D34
	private void FixScrollBars(UIScrollView view, GameObject bar)
	{
		bar.GetComponent<UIScrollBar>().value = 0f;
		Wait.Frames(delegate
		{
			UILanguageSettings.DisplayHelper(view, null, 0f);
		}, 1);
		Wait.Frames(delegate
		{
			UILanguageSettings.DisplayHelper(view, bar, 0f);
		}, 2);
		Wait.Frames(delegate
		{
			UILanguageSettings.DisplayHelper(view, bar, 0.5f);
		}, 3);
		Wait.Frames(delegate
		{
			UILanguageSettings.DisplayHelper(view, bar, 0f);
		}, 4);
	}

	// Token: 0x060024D4 RID: 9428 RVA: 0x00103BB4 File Offset: 0x00101DB4
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

	// Token: 0x060024D5 RID: 9429 RVA: 0x00103BF0 File Offset: 0x00101DF0
	private void Initialize()
	{
		this.initialized = true;
		this.UI = base.transform.Find("Panel").gameObject;
		Transform transform = this.UI.transform;
		transform = transform.GetChild(0);
		this.UI = transform.gameObject;
		this.CurrentLanguageLabel = transform.Find("CurrentLanguage").GetComponent<UILabel>();
		this.LanguageScrollView = transform.Find("Select Language/Language View").GetComponent<UIScrollView>();
		this.LanguageGrid = transform.Find("Select Language/Language View/Grid");
		this.LanguageScrollbar = transform.Find("Select Language/Language Scroll Bar").gameObject;
		this.TranslationScrollView = transform.Find("Select Translation/Translation View").GetComponent<UIScrollView>();
		this.TranslationGrid = transform.Find("Select Translation/Translation View/Grid");
		this.TranslationScrollbar = transform.Find("Select Translation/Translation Scroll Bar").gameObject;
		Transform transform2 = base.transform.Find("Row Templates");
		this.LanguageTemplate = transform2.Find("Language Row Template").gameObject;
		this.TranslationTemplate = transform2.Find("Translation Row Template").gameObject;
		this.Line = transform2.Find("Line Template").gameObject;
		this.DummyLine = transform2.Find("Dummy Line Template").gameObject;
	}

	// Token: 0x060024D6 RID: 9430 RVA: 0x00103D38 File Offset: 0x00101F38
	public void AddLanguages()
	{
		for (int i = 0; i < Language.OrderedLanguageCodes.Count; i++)
		{
			string text = Language.OrderedLanguageCodes[i];
			string languageName = Language.NativeLanguageFromCode[text];
			this.AddLanguage(languageName, text, i != 0);
			if (i == 3)
			{
				this.NextGroup();
				this.AddLine(this.LanguageScrollView);
			}
		}
		this.LanguageGrid.GetComponent<UIGrid>().repositionNow = true;
	}

	// Token: 0x060024D7 RID: 9431 RVA: 0x00103DA5 File Offset: 0x00101FA5
	public void RefreshLanguages()
	{
		if (!this.initialized)
		{
			return;
		}
		TTSUtilities.DestroyChildren(this.LanguageGrid.transform);
		this.AddLanguages();
		this.FixScrollBars(this.LanguageScrollView, this.LanguageScrollbar);
	}

	// Token: 0x060024D8 RID: 9432 RVA: 0x00103DD8 File Offset: 0x00101FD8
	public void AddTranslations()
	{
		List<Translation> list = Singleton<Language>.Instance.TranslationsFromCode[this.selectedLanguageCode];
		for (int i = 0; i < list.Count; i++)
		{
			Translation translation = list[i];
			UITranslationButton uitranslationButton = this.AddTranslation(translation.title, translation.author, translation.filename, translation.workshopID, false);
			if (i == 0)
			{
				uitranslationButton.DisableDelete();
			}
		}
		this.TranslationGrid.GetComponent<UIGrid>().repositionNow = true;
	}

	// Token: 0x060024D9 RID: 9433 RVA: 0x00103E50 File Offset: 0x00102050
	public void CheckTranslations()
	{
		List<Translation> list = Singleton<Language>.Instance.TranslationsFromCode[this.selectedLanguageCode];
		for (int i = list.Count - 1; i >= 1; i--)
		{
			CSV csv;
			if (!Language.TryReadValidCSVFile(list[i].code, list[i].filename, out csv))
			{
				list.RemoveAt(i);
			}
		}
	}

	// Token: 0x060024DA RID: 9434 RVA: 0x00103EB3 File Offset: 0x001020B3
	public void RefreshTranslations()
	{
		if (!this.initialized)
		{
			return;
		}
		TTSUtilities.DestroyChildren(this.TranslationGrid.transform);
		this.CheckTranslations();
		this.AddTranslations();
		this.FixScrollBars(this.TranslationScrollView, this.TranslationScrollbar);
	}

	// Token: 0x060024DB RID: 9435 RVA: 0x00103EEC File Offset: 0x001020EC
	public void RefreshTranslations(string code)
	{
		if (this.selectedLanguageCode == code)
		{
			this.RefreshTranslations();
		}
	}

	// Token: 0x060024DC RID: 9436 RVA: 0x00103F02 File Offset: 0x00102102
	public void NextGroup()
	{
		this.nextLineVisible = true;
		this.inGroup = true;
	}

	// Token: 0x060024DD RID: 9437 RVA: 0x00103F12 File Offset: 0x00102112
	public void IndividualEntries()
	{
		this.nextLineVisible = true;
		this.inGroup = false;
	}

	// Token: 0x060024DE RID: 9438 RVA: 0x00103F22 File Offset: 0x00102122
	public void SelectLanguage(string code)
	{
		this.selectedLanguageCode = code;
		this.RefreshTranslations();
		this.OnThemeChange();
	}

	// Token: 0x060024DF RID: 9439 RVA: 0x00103F38 File Offset: 0x00102138
	public void LoadTranslation(string title, string author, string filename)
	{
		if (Singleton<Language>.Instance.SetLanguageAndTranslation(this.selectedLanguageCode, filename))
		{
			this.selectedTranslationFilename = filename;
			this.CurrentLanguageLabel.text = string.Concat(new string[]
			{
				Language.NativeLanguageFromCode[this.selectedLanguageCode],
				" : ",
				title,
				" [",
				author,
				"]"
			});
			this.RefreshTranslations();
		}
	}

	// Token: 0x060024E0 RID: 9440 RVA: 0x00103FB0 File Offset: 0x001021B0
	public void AskDeleteTranslation(string title, string author, string filename, ulong workshopID)
	{
		string description;
		if (workshopID == 0UL)
		{
			description = Language.Translate("Are you sure you want to delete {0}?", string.Concat(new string[]
			{
				"\"",
				title,
				" : ",
				author,
				"\""
			}));
		}
		else
		{
			description = Language.Translate("Delete and Unsubscribe from {0}?", string.Concat(new string[]
			{
				"\"",
				title,
				" : ",
				author,
				"\""
			}));
		}
		UIDialog.Show(description, "Delete", "Cancel", delegate()
		{
			this.DeleteTranslation(filename, workshopID);
		}, null);
	}

	// Token: 0x060024E1 RID: 9441 RVA: 0x0010406C File Offset: 0x0010226C
	private void DeleteTranslation(string filename, ulong workshopID)
	{
		File.Delete(filename);
		if (workshopID != 0UL)
		{
			Singleton<SteamManager>.Instance.UnsubscribeFromId(workshopID);
		}
		this.RefreshTranslations();
	}

	// Token: 0x060024E2 RID: 9442 RVA: 0x00104088 File Offset: 0x00102288
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

	// Token: 0x060024E3 RID: 9443 RVA: 0x00104128 File Offset: 0x00102328
	public void AddLanguage(string languageName, string languageCode, bool addLine)
	{
		if (addLine)
		{
			this.AddLine(this.LanguageScrollView);
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.LanguageTemplate);
		gameObject.transform.parent = this.LanguageGrid;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		UILanguageRow component = gameObject.GetComponent<UILanguageRow>();
		component.LanguageCode = languageCode;
		component.LanguageName = languageName;
		UILabel component2 = gameObject.GetComponent<UILabel>();
		component2.text = languageName;
		component2.leftAnchor.target = this.LanguageGrid.parent;
		component2.rightAnchor.target = this.LanguageGrid.parent;
		UILabel component3 = gameObject.transform.GetChild(0).GetComponent<UILabel>();
		component3.text = languageCode;
		component3.leftAnchor.target = this.LanguageGrid.parent;
		component3.rightAnchor.target = this.LanguageGrid.parent;
		UIButton component4 = gameObject.transform.GetChild(1).GetComponent<UIButton>();
		if (languageCode == this.selectedLanguageCode)
		{
			component4.defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.MotifHighlightA];
			return;
		}
		component4.defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ControlBackground];
	}

	// Token: 0x060024E4 RID: 9444 RVA: 0x00104268 File Offset: 0x00102468
	public UITranslationButton AddTranslation(string title, string author, string filename, ulong workshopID, bool addLine)
	{
		if (addLine)
		{
			this.AddLine(this.TranslationScrollView);
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.TranslationTemplate);
		gameObject.transform.parent = this.TranslationGrid;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		UITranslationButton component = gameObject.GetComponent<UITranslationButton>();
		component.Title = title;
		component.Author = author;
		component.Filename = filename;
		component.WorkshopID = workshopID;
		UILabel component2 = gameObject.GetComponent<UILabel>();
		component2.text = title;
		component2.leftAnchor.target = this.TranslationGrid.parent;
		component2.rightAnchor.target = this.TranslationGrid.parent;
		component2 = gameObject.transform.GetChild(0).GetComponent<UILabel>();
		component2.text = author;
		if (workshopID != 0UL)
		{
			UILabel uilabel = component2;
			uilabel.text += " (workshop)";
		}
		component2.leftAnchor.target = this.TranslationGrid.parent;
		component2.rightAnchor.target = this.TranslationGrid.parent;
		UIButton component3 = gameObject.transform.GetChild(1).GetComponent<UIButton>();
		if (filename == this.selectedTranslationFilename)
		{
			component3.defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.MotifHighlightA];
		}
		else
		{
			component3.defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.RadioButtonBackground];
		}
		return gameObject.GetComponent<UITranslationButton>();
	}

	// Token: 0x060024E5 RID: 9445 RVA: 0x001043DC File Offset: 0x001025DC
	public void ExportCurrentLanguage()
	{
		CSV csv = Language.ExportCurrentCSV("Custom");
		int num = 1;
		string currentLanguageCode = LocalizationManager.CurrentLanguageCode;
		string arg = DirectoryScript.translationsPath + currentLanguageCode + "_";
		while (File.Exists(arg + num + ".csv"))
		{
			num++;
		}
		int index = csv.RowFromKey("_Author");
		int index2 = csv.RowFromKey("_Title");
		string text = csv[1][index];
		string value = "Custom " + num;
		csv[1][index2] = value;
		string text2 = arg + num + ".csv";
		try
		{
			File.WriteAllText(text2, csv.ToString());
			Chat.LogSystem(string.Format("Exported currently loaded translation to {0}", SerializationScript.GetCleanPath(text2)), false);
			Application.OpenURL("file://" + DirectoryScript.translationsPath);
			Singleton<Language>.Instance.CheckForTranslationFiles();
		}
		catch (Exception e)
		{
			Chat.LogException("saving translation export", e, true, false);
		}
	}

	// Token: 0x060024E6 RID: 9446 RVA: 0x001044F0 File Offset: 0x001026F0
	public void UploadWorkshop()
	{
		NetworkSingleton<NetworkUI>.Instance.GUIWorkshopUpload.GetComponent<UIUploadWorkshop>().ModType = SteamManager.ModInfo.ModType.Translation;
		NetworkSingleton<NetworkUI>.Instance.GUIWorkshopUpload.SetActive(!NetworkSingleton<NetworkUI>.Instance.GUIWorkshopUpload.activeSelf);
	}

	// Token: 0x060024E7 RID: 9447 RVA: 0x00104528 File Offset: 0x00102728
	public void ShowWorkshop()
	{
		TTSUtilities.OpenURL("https://steamcommunity.com/workshop/browse/?appid=286160&requiredtags[]=Translation");
	}

	// Token: 0x060024E8 RID: 9448 RVA: 0x00104534 File Offset: 0x00102734
	private void OnThemeChange()
	{
		for (int i = 0; i < this.LanguageGrid.childCount; i++)
		{
			UILanguageRow component = this.LanguageGrid.GetChild(i).GetComponent<UILanguageRow>();
			if (component)
			{
				UIButton component2 = component.transform.GetChild(1).GetComponent<UIButton>();
				if (component.LanguageCode == this.selectedLanguageCode)
				{
					component2.defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.MotifHighlightA];
				}
				else
				{
					component2.defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.ControlBackground];
				}
			}
		}
		for (int j = 0; j < this.TranslationGrid.childCount; j++)
		{
			UITranslationButton componentInChildren = this.TranslationGrid.GetChild(j).GetComponentInChildren<UITranslationButton>();
			if (componentInChildren)
			{
				UIButton component3 = componentInChildren.transform.GetChild(1).GetComponent<UIButton>();
				if (this.selectedLanguageCode == Language.CurrentLanguageCode && componentInChildren.Filename == this.selectedTranslationFilename)
				{
					component3.defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.MotifHighlightA];
				}
				else
				{
					component3.defaultColor = Singleton<UIPalette>.Instance.CurrentThemeColours[UIPalette.UI.RadioButtonBackground];
				}
			}
		}
	}

	// Token: 0x040017CE RID: 6094
	[NonSerialized]
	public Transform PanelTemplate;

	// Token: 0x040017CF RID: 6095
	[NonSerialized]
	public GameObject UI;

	// Token: 0x040017D0 RID: 6096
	[NonSerialized]
	public UILabel CurrentLanguageLabel;

	// Token: 0x040017D1 RID: 6097
	[NonSerialized]
	public Transform LanguageGrid;

	// Token: 0x040017D2 RID: 6098
	[NonSerialized]
	public UIScrollView LanguageScrollView;

	// Token: 0x040017D3 RID: 6099
	[NonSerialized]
	public GameObject LanguageScrollbar;

	// Token: 0x040017D4 RID: 6100
	[NonSerialized]
	public GameObject LanguageTemplate;

	// Token: 0x040017D5 RID: 6101
	[NonSerialized]
	public GameObject TranslationTemplate;

	// Token: 0x040017D6 RID: 6102
	[NonSerialized]
	public Transform TranslationGrid;

	// Token: 0x040017D7 RID: 6103
	[NonSerialized]
	public UIScrollView TranslationScrollView;

	// Token: 0x040017D8 RID: 6104
	[NonSerialized]
	public GameObject TranslationScrollbar;

	// Token: 0x040017D9 RID: 6105
	[NonSerialized]
	public GameObject Line;

	// Token: 0x040017DA RID: 6106
	[NonSerialized]
	public GameObject DummyLine;

	// Token: 0x040017DB RID: 6107
	private bool initialized;

	// Token: 0x040017DC RID: 6108
	private bool nextLineVisible;

	// Token: 0x040017DD RID: 6109
	private bool inGroup;

	// Token: 0x040017DE RID: 6110
	private string selectedLanguageCode;

	// Token: 0x040017DF RID: 6111
	private string selectedTranslationFilename;

	// Token: 0x040017E0 RID: 6112
	private string storedLanguageCode;

	// Token: 0x040017E1 RID: 6113
	private string storedTranslationFilename;
}
