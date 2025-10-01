using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x020002DB RID: 731
public class UIGridMenuObjects : UIGridMenu, INotifySceneAwake
{
	// Token: 0x060023CF RID: 9167 RVA: 0x000FE2EC File Offset: 0x000FC4EC
	protected override void Awake()
	{
		base.Awake();
		EventDelegate.Add(this.ComponentsButton.onClick, new EventDelegate.Callback(this.OnClickComponents));
		EventDelegate.Add(this.TablesButton.onClick, new EventDelegate.Callback(this.OnClickTables));
		EventDelegate.Add(this.BackgroundsButton.onClick, new EventDelegate.Callback(this.OnClickBackgrounds));
		EventDelegate.Add(this.SavedObjectsButton.onClick, new EventDelegate.Callback(this.OnClickSavedObjects));
		EventDelegate.Add(this.CreateFolderButton.onClick, new EventDelegate.Callback(this.OnCreateFolder));
		EventManager.OnFileSave += this.OnFileSave;
		EventManager.OnFileDelete += this.OnFileDelete;
		this.SortPopup.items = this.SavedObjectsSorts.Keys.ToList<string>();
		EventDelegate.Add(this.SortPopup.onChange, new EventDelegate.Callback(this.SortChange));
		this.LoadSorts();
	}

	// Token: 0x060023D0 RID: 9168 RVA: 0x000FE3F0 File Offset: 0x000FC5F0
	protected override void OnDestroy()
	{
		base.OnDestroy();
		EventDelegate.Remove(this.ComponentsButton.onClick, new EventDelegate.Callback(this.OnClickComponents));
		EventDelegate.Remove(this.TablesButton.onClick, new EventDelegate.Callback(this.OnClickTables));
		EventDelegate.Remove(this.BackgroundsButton.onClick, new EventDelegate.Callback(this.OnClickBackgrounds));
		EventDelegate.Remove(this.SavedObjectsButton.onClick, new EventDelegate.Callback(this.OnClickSavedObjects));
		EventDelegate.Remove(this.CreateFolderButton.onClick, new EventDelegate.Callback(this.OnCreateFolder));
		EventManager.OnFileSave -= this.OnFileSave;
		EventManager.OnFileDelete -= this.OnFileDelete;
		EventDelegate.Remove(this.SortPopup.onChange, new EventDelegate.Callback(this.SortChange));
	}

	// Token: 0x060023D1 RID: 9169 RVA: 0x000FE4D3 File Offset: 0x000FC6D3
	protected override void OnEnable()
	{
		base.OnEnable();
		this.Init();
	}

	// Token: 0x060023D2 RID: 9170 RVA: 0x000FE4E1 File Offset: 0x000FC6E1
	protected override void OnDisable()
	{
		base.OnDisable();
		this.SaveSorts();
	}

	// Token: 0x060023D3 RID: 9171 RVA: 0x000FE4EF File Offset: 0x000FC6EF
	private void Init()
	{
		this.LoadMainMenu();
		if (SystemConsole.AutofocusSearch)
		{
			this.SearchInput.isSelected = true;
		}
	}

	// Token: 0x060023D4 RID: 9172 RVA: 0x000FE50C File Offset: 0x000FC70C
	protected override void OnLoad()
	{
		base.OnLoad();
		this.MainMenu.SetActive(this.BackStates.Count == 0);
		if (this.MainMenu.activeSelf)
		{
			this.LoadHelp(0);
			this.CreateFolderButton.gameObject.SetActive(false);
			this.SortPopup.gameObject.SetActive(false);
			this.LastMenuAction = null;
			this.ResetSearch(true);
		}
	}

	// Token: 0x060023D5 RID: 9173 RVA: 0x000FE57C File Offset: 0x000FC77C
	private void OnClickComponents()
	{
		this.LoadHelp(1);
		this.LoadCategory(new Action<int, bool>(this.LoadComponents));
	}

	// Token: 0x060023D6 RID: 9174 RVA: 0x000FE597 File Offset: 0x000FC797
	private void OnClickTables()
	{
		this.LoadHelp(2);
		this.LoadCategory(new Action<int, bool>(this.LoadTables));
	}

	// Token: 0x060023D7 RID: 9175 RVA: 0x000FE5B2 File Offset: 0x000FC7B2
	private void OnClickBackgrounds()
	{
		this.LoadHelp(3);
		this.LoadCategory(new Action<int, bool>(this.LoadBackgrounds));
	}

	// Token: 0x060023D8 RID: 9176 RVA: 0x000FE5CD File Offset: 0x000FC7CD
	private void OnClickSavedObjects()
	{
		this.LoadHelp(4);
		this.LoadCategory(new Action<int, bool>(this.LoadSavedObjects));
	}

	// Token: 0x060023D9 RID: 9177 RVA: 0x000FE5E8 File Offset: 0x000FC7E8
	public void LoadMainMenu()
	{
		this.LoadHelp(0);
		base.ClearBackStates();
		base.Load<UIGridMenu.GridButton>(new List<UIGridMenu.GridButton>(), 1, "OBJECTS", false, false);
	}

	// Token: 0x060023DA RID: 9178 RVA: 0x000FE60C File Offset: 0x000FC80C
	private void LoadHelp(int index)
	{
		for (int i = 0; i < this.Helps.Count; i++)
		{
			this.Helps[i].SetActive(index == i);
		}
	}

	// Token: 0x060023DB RID: 9179 RVA: 0x000FE644 File Offset: 0x000FC844
	public void LoadCategory(Action<int, bool> action)
	{
		this.LastMenuAction = action;
		action(1, true);
	}

	// Token: 0x060023DC RID: 9180 RVA: 0x000FE655 File Offset: 0x000FC855
	public void LoadComponents(int page, bool addBack)
	{
		if (this.loadWithoutFolders)
		{
			base.Load<UIGridMenu.GridButtonComponent>(this.GetAllComponents(), page, "COMPONENTS", addBack, false);
			return;
		}
		base.Load<UIGridMenu.GridButtonFolder>(this.ComponentsButtons, page, "COMPONENTS", addBack, false);
	}

	// Token: 0x060023DD RID: 9181 RVA: 0x000FE688 File Offset: 0x000FC888
	public void LoadTables(int page, bool addBack)
	{
		base.Load<UIGridMenu.GridButtonTable>(this.TablesButtons, page, "TABLES", addBack, false);
	}

	// Token: 0x060023DE RID: 9182 RVA: 0x000FE69E File Offset: 0x000FC89E
	public void LoadBackgrounds(int page, bool addBack)
	{
		base.Load<UIGridMenu.GridButtonBackground>(this.BackgroundsButtons, page, "BACKGROUNDS", addBack, false);
	}

	// Token: 0x060023DF RID: 9183 RVA: 0x000FE6B4 File Offset: 0x000FC8B4
	public void LoadSavedObjects(int page, bool addBack)
	{
		this.LoadSavedObjects("SAVED OBJECTS", addBack, "");
	}

	// Token: 0x060023E0 RID: 9184 RVA: 0x000FE6C8 File Offset: 0x000FC8C8
	private async void LoadAll()
	{
		this.MainMenu.SetActive(false);
		this.LoadingWheel.SetActive(true);
		List<UIGridMenu.GridButton> buttons = await this.AsyncGetAllObjects();
		this.LoadingWheel.SetActive(false);
		base.Load<UIGridMenu.GridButton>(buttons, 1, "ALL OBJECTS", true, false);
	}

	// Token: 0x060023E1 RID: 9185 RVA: 0x000FE704 File Offset: 0x000FC904
	public async void LoadSavedObjects(string name, bool addBack, string path = "")
	{
		bool bGetAllSaveFileWithoutFolders = this.loadWithoutFolders;
		this.MainMenu.SetActive(false);
		this.LoadingWheel.SetActive(true);
		List<UIGridMenu.GridButtonPath> buttons = await this.AsyncGetSavedObjectGridButtons(path, bGetAllSaveFileWithoutFolders);
		this.LoadingWheel.SetActive(false);
		this.CreateFolderButton.gameObject.SetActive(true);
		this.SortPopup.gameObject.SetActive(true);
		try
		{
			base.Load<UIGridMenu.GridButtonPath>(buttons, 1, name, addBack, false);
		}
		catch (Exception e)
		{
			Chat.LogException("loading saved objects", e, true, false);
		}
	}

	// Token: 0x060023E2 RID: 9186 RVA: 0x000FE758 File Offset: 0x000FC958
	private void LoadSorts()
	{
		if (PlayerPrefs.HasKey("SavedObjectsSort"))
		{
			string @string = PlayerPrefs.GetString("SavedObjectsSort");
			if (this.SavedObjectsSorts.ContainsKey(@string))
			{
				this.currentSavedObjectsSort = @string;
			}
		}
	}

	// Token: 0x060023E3 RID: 9187 RVA: 0x000FE791 File Offset: 0x000FC991
	private void SaveSorts()
	{
		PlayerPrefs.SetString("SavedObjectsSort", this.currentSavedObjectsSort);
	}

	// Token: 0x060023E4 RID: 9188 RVA: 0x000FE7A4 File Offset: 0x000FC9A4
	private void OnCreateFolder()
	{
		UIDialog.ShowDropDownInput("Create folder in Saved Objects", "Create", "Cancel", base.Folders, new Action<string, string>(this.CreateFolder), null, "", "Folder Name", this.GetCurrentFolder());
	}

	// Token: 0x060023E5 RID: 9189 RVA: 0x000FE7E8 File Offset: 0x000FC9E8
	private void SortChange()
	{
		this.currentSavedObjectsSort = this.SortPopup.value;
		base.Reload(true);
	}

	// Token: 0x060023E6 RID: 9190 RVA: 0x000FE804 File Offset: 0x000FCA04
	private void CreateFolder(string LocalFolder, string Name)
	{
		string text = SerializationScript.RemoveInvalidCharsFromFileName(Name);
		if (DirectoryScript.IsBlacklistedFolder(text))
		{
			Chat.LogError("This folder name is not allowed.", true);
			return;
		}
		SerializationScript.CreateDirectory(SerializationScript.CombineLocalFolder(DirectoryScript.savedObjectsFilePath, LocalFolder, text));
	}

	// Token: 0x060023E7 RID: 9191 RVA: 0x000FE83D File Offset: 0x000FCA3D
	private void OnFileSave(string Path)
	{
		if (!base.gameObject.activeSelf || this.LastMenuAction != new Action<int, bool>(this.LoadSavedObjects))
		{
			return;
		}
		if (Path.StartsWith(DirectoryScript.savedObjectsFilePath))
		{
			base.Reload(false);
		}
	}

	// Token: 0x060023E8 RID: 9192 RVA: 0x000FE87A File Offset: 0x000FCA7A
	private void OnFileDelete(string Path)
	{
		if (!base.gameObject.activeSelf || this.LastMenuAction != new Action<int, bool>(this.LoadSavedObjects))
		{
			return;
		}
		if (Path.StartsWith(DirectoryScript.savedObjectsFilePath))
		{
			base.Reload(true);
		}
	}

	// Token: 0x060023E9 RID: 9193 RVA: 0x000FE8B8 File Offset: 0x000FCAB8
	public async Task<List<UIGridMenu.GridButtonPath>> AsyncGetSavedObjectGridButtons(string path, bool bGetAllSaveFileWithoutFolders)
	{
		return await Task.Run<List<UIGridMenu.GridButtonPath>>(delegate()
		{
			List<UIGridMenu.GridButtonPath> result;
			try
			{
				result = this.GetSavedObjectGridButtons(path, bGetAllSaveFileWithoutFolders);
			}
			catch (Exception e)
			{
				Chat.LogException("loading saved objects", e, true, false);
				result = new List<UIGridMenu.GridButtonPath>();
			}
			return result;
		});
	}

	// Token: 0x060023EA RID: 9194 RVA: 0x000FE910 File Offset: 0x000FCB10
	public List<UIGridMenu.GridButtonPath> GetSavedObjectGridButtons(string path, bool bGetAllSaveFileWithoutFolders)
	{
		base.CurrentPath = path;
		if (string.IsNullOrEmpty(path))
		{
			path = DirectoryScript.savedObjectsFilePath;
		}
		this.GetChestName(path, bGetAllSaveFileWithoutFolders);
		List<UIGridMenu.GridButtonPath> list = new List<UIGridMenu.GridButtonPath>();
		if (!bGetAllSaveFileWithoutFolders)
		{
			for (int i = 0; i < this.SaveChestFolderList.Count; i++)
			{
				try
				{
					string path2 = this.SaveChestFolderList[i];
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path2);
					list.Add(new UIGridMenu.GridButtonFolderSavedObject
					{
						Path = path2,
						Name = fileNameWithoutExtension,
						ButtonColor = this.SavedObjectsColor,
						ButtonHoverColor = Colour.White
					});
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
			list.Sort((UIGridMenu.GridButtonPath x, UIGridMenu.GridButtonPath y) => AlphanumComparatorFast.Compare(x.Name, y.Name));
		}
		List<UIGridMenu.GridButtonSavedObject> list2 = new List<UIGridMenu.GridButtonSavedObject>();
		for (int j = 0; j < this.SaveChestList.Count; j++)
		{
			try
			{
				string text = this.SaveChestList[j];
				UIGridMenu.GridButtonSavedObject gridButtonSavedObject = new UIGridMenu.GridButtonSavedObject();
				gridButtonSavedObject.Path = text;
				gridButtonSavedObject.Name = Path.GetFileNameWithoutExtension(text);
				gridButtonSavedObject.ButtonColor = this.SavedObjectsColor;
				gridButtonSavedObject.ButtonHoverColor = Colour.White;
				FileInfo fileInfo = new FileInfo(text);
				gridButtonSavedObject.UpdateTime = SerializationScript.GetTimeFromEpoch(fileInfo.LastWriteTimeUtc);
				gridButtonSavedObject.LoadTime = SerializationScript.GetTimeFromEpoch(fileInfo.LastAccessTimeUtc);
				gridButtonSavedObject.ThumbnailPath = Path.ChangeExtension(text, ".png");
				list2.Add(gridButtonSavedObject);
			}
			catch (Exception exception2)
			{
				Debug.LogException(exception2);
			}
		}
		Action<List<UIGridMenu.GridButtonSavedObject>> action;
		if (this.SavedObjectsSorts.TryGetValue(this.currentSavedObjectsSort, out action) && action != null)
		{
			action(list2);
		}
		for (int k = 0; k < list2.Count; k++)
		{
			list.Add(list2[k]);
		}
		return list;
	}

	// Token: 0x060023EB RID: 9195 RVA: 0x000FEB08 File Offset: 0x000FCD08
	private void GetChestName(string Path, bool bGetAllSaveFileWithoutFolders)
	{
		this.SaveChestList = SerializationScript.GetSaveFiles(Path, bGetAllSaveFileWithoutFolders, null);
		if (!bGetAllSaveFileWithoutFolders)
		{
			this.SaveChestFolderList = SerializationScript.GetDirectories(Path, false, null);
		}
		else
		{
			this.SaveChestFolderList.Clear();
		}
		base.SetPathFolders(DirectoryScript.savedObjectsFilePath, "");
	}

	// Token: 0x060023EC RID: 9196 RVA: 0x000FEB48 File Offset: 0x000FCD48
	public void SceneAwake()
	{
		this.ComponentsColor = Colour.ColourFromRGBHex("FFDF7B");
		this.TablesColor = Colour.ColourFromRGBHex("87F387");
		this.BackgroundsColor = Colour.ColourFromRGBHex("71B9FF");
		this.SavedObjectsColor = Colour.ColourFromRGBHex("FF7B83");
		this.ComponentsButton.GetComponent<UISprite>().color = this.ComponentsColor;
		this.TablesButton.GetComponent<UISprite>().color = this.TablesColor;
		this.BackgroundsButton.GetComponent<UISprite>().color = this.BackgroundsColor;
		this.SavedObjectsButton.GetComponent<UISprite>().color = this.SavedObjectsColor;
		this.ComponentsButton.GetComponent<UIButton>().hover = Colour.White;
		this.TablesButton.GetComponent<UIButton>().hover = Colour.White;
		this.BackgroundsButton.GetComponent<UIButton>().hover = Colour.White;
		this.SavedObjectsButton.GetComponent<UIButton>().hover = Colour.White;
		this.ComponentsButton.gameObject.AddMissingComponent<UIHoverShadow>();
		this.TablesButton.gameObject.AddMissingComponent<UIHoverShadow>();
		this.BackgroundsButton.gameObject.AddMissingComponent<UIHoverShadow>();
		this.SavedObjectsButton.gameObject.AddMissingComponent<UIHoverShadow>();
		this.InitComponents();
		this.InitTables();
		this.InitBackgrounds();
	}

	// Token: 0x060023ED RID: 9197 RVA: 0x000FECC0 File Offset: 0x000FCEC0
	private void InitComponents()
	{
		foreach (UIGridMenu.GridButtonFolder gridButtonFolder in this.ComponentsButtons)
		{
			gridButtonFolder.ButtonColor = this.ComponentsColor;
			gridButtonFolder.Autotranslate = true;
			gridButtonFolder.ButtonHoverColor = Colour.White;
			this.<InitComponents>g__InitButton|52_0(new List<string>(), gridButtonFolder);
		}
	}

	// Token: 0x060023EE RID: 9198 RVA: 0x000FED3C File Offset: 0x000FCF3C
	public List<UIGridMenu.GridButtonComponent> GetAllComponents()
	{
		List<UIGridMenu.GridButtonComponent> list = new List<UIGridMenu.GridButtonComponent>();
		foreach (UIGridMenu.GridButtonFolder folderButton in this.ComponentsButtons)
		{
			UIGridMenuObjects.<GetAllComponents>g__AddChildren|53_0(list, folderButton);
		}
		return list;
	}

	// Token: 0x060023EF RID: 9199 RVA: 0x000FED98 File Offset: 0x000FCF98
	private void InitTables()
	{
		foreach (UIGridMenu.GridButtonTable gridButtonTable in this.TablesButtons)
		{
			gridButtonTable.SpriteColor = ((gridButtonTable.Name == "None") ? Color.red : Color.white);
			gridButtonTable.Tags.Add("tables");
			gridButtonTable.ButtonColor = this.TablesColor;
			gridButtonTable.Autotranslate = true;
			gridButtonTable.ButtonHoverColor = Color.white;
			gridButtonTable.CloseMenu = base.gameObject;
		}
	}

	// Token: 0x060023F0 RID: 9200 RVA: 0x000FEE40 File Offset: 0x000FD040
	public List<UIGridMenu.GridButtonTable> GetAllTables()
	{
		return this.TablesButtons;
	}

	// Token: 0x060023F1 RID: 9201 RVA: 0x000FEE48 File Offset: 0x000FD048
	private void InitBackgrounds()
	{
		foreach (UIGridMenu.GridButtonBackground gridButtonBackground in this.BackgroundsButtons)
		{
			gridButtonBackground.SpriteColor = Color.white;
			gridButtonBackground.Tags.Add("backgrounds");
			gridButtonBackground.ButtonColor = this.BackgroundsColor;
			gridButtonBackground.Autotranslate = true;
			gridButtonBackground.ButtonHoverColor = Color.white;
			gridButtonBackground.CloseMenu = base.gameObject;
		}
	}

	// Token: 0x060023F2 RID: 9202 RVA: 0x000FEED8 File Offset: 0x000FD0D8
	public List<UIGridMenu.GridButtonBackground> GetAllBackgrounds()
	{
		return this.BackgroundsButtons;
	}

	// Token: 0x060023F3 RID: 9203 RVA: 0x000FEEE0 File Offset: 0x000FD0E0
	public async Task<List<UIGridMenu.GridButton>> AsyncGetAllObjects()
	{
		List<UIGridMenu.GridButtonPath> collection = await this.AsyncGetSavedObjectGridButtons("", true);
		List<UIGridMenu.GridButton> list = new List<UIGridMenu.GridButton>();
		list.AddRange(this.GetAllComponents());
		list.AddRange(this.GetAllTables());
		list.AddRange(this.GetAllBackgrounds());
		list.AddRange(collection);
		return list;
	}

	// Token: 0x060023F4 RID: 9204 RVA: 0x000FEF25 File Offset: 0x000FD125
	public override void ResetSearch(bool blockOnChange = true)
	{
		base.ResetSearch(blockOnChange);
		this.prevSearch = "";
	}

	// Token: 0x060023F5 RID: 9205 RVA: 0x000FEF3C File Offset: 0x000FD13C
	public override void Search(string search)
	{
		if (this.MainMenu.activeSelf && !string.IsNullOrEmpty(search) && string.IsNullOrEmpty(this.prevSearch))
		{
			this.LoadAll();
		}
		else if (!string.IsNullOrEmpty(search) && string.IsNullOrEmpty(this.prevSearch))
		{
			this.loadWithoutFolders = true;
			this.Reload(base.currentPage, true);
			this.loadWithoutFolders = false;
		}
		else if (string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(this.prevSearch))
		{
			this.Reload(base.currentPage, true);
		}
		this.prevSearch = search;
		base.Search(search);
	}

	// Token: 0x060023F6 RID: 9206 RVA: 0x000FEFD5 File Offset: 0x000FD1D5
	public override void Reload(int page, bool DelayThumbnailCleanup = true)
	{
		base.Reload(page, DelayThumbnailCleanup);
		if (this.LastMenuAction == null)
		{
			this.LoadMainMenu();
			return;
		}
		this.LastMenuAction(base.currentPage, false);
	}

	// Token: 0x060023F7 RID: 9207 RVA: 0x000FF000 File Offset: 0x000FD200
	public UIGridMenuObjects()
	{
		Dictionary<string, Action<List<UIGridMenu.GridButtonSavedObject>>> dictionary = new Dictionary<string, Action<List<UIGridMenu.GridButtonSavedObject>>>();
		dictionary.Add("Name", delegate(List<UIGridMenu.GridButtonSavedObject> list)
		{
			list.Sort((UIGridMenu.GridButtonSavedObject x, UIGridMenu.GridButtonSavedObject y) => AlphanumComparatorFast.Compare(x.Name, y.Name));
		});
		dictionary.Add("Updated", delegate(List<UIGridMenu.GridButtonSavedObject> list)
		{
			list.Sort((UIGridMenu.GridButtonSavedObject x, UIGridMenu.GridButtonSavedObject y) => y.UpdateTime.CompareTo(x.UpdateTime));
		});
		dictionary.Add("Loaded", delegate(List<UIGridMenu.GridButtonSavedObject> list)
		{
			list.Sort((UIGridMenu.GridButtonSavedObject x, UIGridMenu.GridButtonSavedObject y) => y.LoadTime.CompareTo(x.LoadTime));
		});
		dictionary.Add("Random", delegate(List<UIGridMenu.GridButtonSavedObject> list)
		{
			list.Randomize<UIGridMenu.GridButtonSavedObject>();
		});
		this.SavedObjectsSorts = dictionary;
		this.SaveChestList = new List<string>();
		this.SaveChestFolderList = new List<string>();
		this.prevSearch = "";
		base..ctor();
	}

	// Token: 0x060023F8 RID: 9208 RVA: 0x000FF114 File Offset: 0x000FD314
	[CompilerGenerated]
	private void <InitComponents>g__InitButton|52_0(List<string> tags, UIGridMenu.GridButtonFolder folderButton)
	{
		tags.Add(folderButton.Name.ToLower());
		foreach (UIGridMenu.GridButtonComponent gridButtonComponent in folderButton.ComponentButtons)
		{
			if (string.IsNullOrEmpty(gridButtonComponent.SpawnName))
			{
				gridButtonComponent.SpawnName = gridButtonComponent.Name;
			}
			gridButtonComponent.Tags.Add("components");
			gridButtonComponent.Tags.AddRange(tags);
			gridButtonComponent.ButtonColor = this.ComponentsColor;
			gridButtonComponent.Autotranslate = true;
			gridButtonComponent.ButtonHoverColor = Colour.White;
		}
		foreach (UIGridMenu.GridButtonFolder gridButtonFolder in folderButton.FolderButtons)
		{
			gridButtonFolder.ButtonColor = this.ComponentsColor;
			gridButtonFolder.Autotranslate = true;
			gridButtonFolder.ButtonHoverColor = Colour.White;
			this.<InitComponents>g__InitButton|52_0(new List<string>(tags), gridButtonFolder);
		}
	}

	// Token: 0x060023F9 RID: 9209 RVA: 0x000FF234 File Offset: 0x000FD434
	[CompilerGenerated]
	internal static void <GetAllComponents>g__AddChildren|53_0(List<UIGridMenu.GridButtonComponent> buttons, UIGridMenu.GridButtonFolder folderButton)
	{
		foreach (UIGridMenu.GridButtonComponent gridButtonComponent in folderButton.ComponentButtons)
		{
			bool flag = true;
			using (List<UIGridMenu.GridButtonComponent>.Enumerator enumerator2 = buttons.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.SpawnName == gridButtonComponent.SpawnName)
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				buttons.Add(gridButtonComponent);
			}
		}
		foreach (UIGridMenu.GridButtonFolder folderButton2 in folderButton.FolderButtons)
		{
			UIGridMenuObjects.<GetAllComponents>g__AddChildren|53_0(buttons, folderButton2);
		}
	}

	// Token: 0x040016FD RID: 5885
	public List<UIGridMenu.GridButtonFolder> ComponentsButtons = new List<UIGridMenu.GridButtonFolder>();

	// Token: 0x040016FE RID: 5886
	public List<UIGridMenu.GridButtonTable> TablesButtons = new List<UIGridMenu.GridButtonTable>();

	// Token: 0x040016FF RID: 5887
	public List<UIGridMenu.GridButtonBackground> BackgroundsButtons = new List<UIGridMenu.GridButtonBackground>();

	// Token: 0x04001700 RID: 5888
	public UIButton CreateFolderButton;

	// Token: 0x04001701 RID: 5889
	public UIPopupList SortPopup;

	// Token: 0x04001702 RID: 5890
	public GameObject LoadingWheel;

	// Token: 0x04001703 RID: 5891
	public List<GameObject> Helps;

	// Token: 0x04001704 RID: 5892
	public GameObject MainMenu;

	// Token: 0x04001705 RID: 5893
	private Color ComponentsColor;

	// Token: 0x04001706 RID: 5894
	private Color TablesColor;

	// Token: 0x04001707 RID: 5895
	private Color BackgroundsColor;

	// Token: 0x04001708 RID: 5896
	private Color SavedObjectsColor;

	// Token: 0x04001709 RID: 5897
	public UIButton ComponentsButton;

	// Token: 0x0400170A RID: 5898
	public UIButton TablesButton;

	// Token: 0x0400170B RID: 5899
	public UIButton BackgroundsButton;

	// Token: 0x0400170C RID: 5900
	public UIButton SavedObjectsButton;

	// Token: 0x0400170D RID: 5901
	private string currentSavedObjectsSort = "Name";

	// Token: 0x0400170E RID: 5902
	private Dictionary<string, Action<List<UIGridMenu.GridButtonSavedObject>>> SavedObjectsSorts;

	// Token: 0x0400170F RID: 5903
	private Action<int, bool> LastMenuAction;

	// Token: 0x04001710 RID: 5904
	private const string SavedObjectsSortPrefs = "SavedObjectsSort";

	// Token: 0x04001711 RID: 5905
	private List<string> SaveChestList;

	// Token: 0x04001712 RID: 5906
	private List<string> SaveChestFolderList;

	// Token: 0x04001713 RID: 5907
	private string prevSearch;

	// Token: 0x04001714 RID: 5908
	private bool loadWithoutFolders;
}
