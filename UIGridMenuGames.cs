using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x020002DA RID: 730
public class UIGridMenuGames : UIGridMenu
{
	// Token: 0x060023A0 RID: 9120 RVA: 0x000FC4B0 File Offset: 0x000FA6B0
	protected override void Awake()
	{
		base.Awake();
		EventDelegate.Add(this.ClassicButton.onClick, new EventDelegate.Callback(this.OnClickClassic));
		EventDelegate.Add(this.DLCButton.onClick, new EventDelegate.Callback(this.OnClickDLC));
		EventDelegate.Add(this.WorkshopButton.onClick, new EventDelegate.Callback(this.OnClickWorkshop));
		EventDelegate.Add(this.SavesButton.onClick, new EventDelegate.Callback(this.OnClickSaves));
		EventDelegate.Add(this.BackButtonGames.onClick, new EventDelegate.Callback(this.OnClickBack));
		EventDelegate.Add(this.ClearButton.onClick, new EventDelegate.Callback(this.OnClearTable));
		EventDelegate.Add(this.WorkshopAdditionalButton.onClick, new EventDelegate.Callback(this.OnWorkshopBrowse));
		EventDelegate.Add(this.SavesAdditionalButton.onClick, new EventDelegate.Callback(this.OnCreateSave));
		this.SaveFileInfos = SerializationScript.LoadSaveFileInfos();
		this.WorkshopFileInfos = SerializationScript.LoadWorkshopFileInfos();
		EventDelegate.Add(this.SortPopup.onChange, new EventDelegate.Callback(this.SortChange));
		EventDelegate.Add(this.CreateFolderButton.onClick, new EventDelegate.Callback(this.OnCreateFolder));
		EventManager.OnFileSave += this.OnFileSave;
		EventManager.OnFileDelete += this.OnFileDelete;
		EventManager.OnLanguageChange += this.OnLanguageChange;
	}

	// Token: 0x060023A1 RID: 9121 RVA: 0x000FC62F File Offset: 0x000FA82F
	protected override void OnEnable()
	{
		base.OnEnable();
		this.LoadSorts();
		this.OnClickFeatured();
		Singleton<DLCManager>.Instance.CheckSales();
		if (SystemConsole.AutofocusSearch)
		{
			this.SearchInput.isSelected = true;
		}
	}

	// Token: 0x060023A2 RID: 9122 RVA: 0x000FC660 File Offset: 0x000FA860
	protected override void OnDisable()
	{
		base.OnDisable();
		this.SaveSorts();
	}

	// Token: 0x060023A3 RID: 9123 RVA: 0x000FC670 File Offset: 0x000FA870
	protected override void OnDestroy()
	{
		base.OnDestroy();
		EventDelegate.Remove(this.ClassicButton.onClick, new EventDelegate.Callback(this.OnClickClassic));
		EventDelegate.Remove(this.DLCButton.onClick, new EventDelegate.Callback(this.OnClickDLC));
		EventDelegate.Remove(this.WorkshopButton.onClick, new EventDelegate.Callback(this.OnClickWorkshop));
		EventDelegate.Remove(this.SavesButton.onClick, new EventDelegate.Callback(this.OnClickSaves));
		EventDelegate.Remove(this.BackButtonGames.onClick, new EventDelegate.Callback(this.OnClickBack));
		EventDelegate.Remove(this.ClearButton.onClick, new EventDelegate.Callback(this.OnClearTable));
		EventDelegate.Remove(this.WorkshopAdditionalButton.onClick, new EventDelegate.Callback(this.OnWorkshopBrowse));
		EventDelegate.Remove(this.SavesAdditionalButton.onClick, new EventDelegate.Callback(this.OnCreateSave));
		EventDelegate.Remove(this.SortPopup.onChange, new EventDelegate.Callback(this.SortChange));
		EventDelegate.Remove(this.CreateFolderButton.onClick, new EventDelegate.Callback(this.OnCreateFolder));
		EventManager.OnFileSave -= this.OnFileSave;
		EventManager.OnFileDelete -= this.OnFileDelete;
		EventManager.OnLanguageChange -= this.OnLanguageChange;
	}

	// Token: 0x060023A4 RID: 9124 RVA: 0x000FC7DC File Offset: 0x000FA9DC
	private void LoadSorts()
	{
		if (PlayerPrefs.HasKey("GamesClassicSort"))
		{
			string @string = PlayerPrefs.GetString("GamesClassicSort");
			if (this.ClassicSorts.ContainsKey(@string))
			{
				this.currentClassicSort = @string;
			}
		}
		if (PlayerPrefs.HasKey("GamesDLCSort"))
		{
			string string2 = PlayerPrefs.GetString("GamesDLCSort");
			if (this.DLCSorts.ContainsKey(string2))
			{
				this.currentDLCSort = string2;
			}
		}
		if (PlayerPrefs.HasKey("GamesWorkshopSort"))
		{
			string string3 = PlayerPrefs.GetString("GamesWorkshopSort");
			if (this.WorkshopSorts.ContainsKey(string3))
			{
				this.currentWorkshopSort = string3;
			}
		}
		if (PlayerPrefs.HasKey("GamesSavesSort"))
		{
			string string4 = PlayerPrefs.GetString("GamesSavesSort");
			if (this.SavesSorts.ContainsKey(string4))
			{
				this.currentSavesSort = string4;
			}
		}
	}

	// Token: 0x060023A5 RID: 9125 RVA: 0x000FC89C File Offset: 0x000FAA9C
	private void SaveSorts()
	{
		PlayerPrefs.SetString("GamesClassicSort", this.currentClassicSort);
		PlayerPrefs.SetString("GamesDLCSort", this.currentDLCSort);
		PlayerPrefs.SetString("GamesWorkshopSort", this.currentWorkshopSort);
		PlayerPrefs.SetString("GamesSavesSort", this.currentSavesSort);
	}

	// Token: 0x060023A6 RID: 9126 RVA: 0x000FC8E9 File Offset: 0x000FAAE9
	private void OnFileSave(string Path)
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		if (this.Isloading())
		{
			return;
		}
		base.Reload(false);
	}

	// Token: 0x060023A7 RID: 9127 RVA: 0x000FC909 File Offset: 0x000FAB09
	private void OnFileDelete(string Path)
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		if (this.Isloading())
		{
			return;
		}
		base.Reload(true);
	}

	// Token: 0x060023A8 RID: 9128 RVA: 0x000FC929 File Offset: 0x000FAB29
	private bool Isloading()
	{
		if (this.FeaturedMenu.activeSelf)
		{
			return this.LoadingWheelWorkshop.activeSelf || this.LoadingWheelSaves.activeSelf;
		}
		return this.LoadingWheelGridMenu.activeSelf;
	}

	// Token: 0x060023A9 RID: 9129 RVA: 0x000FC95E File Offset: 0x000FAB5E
	private void OnLanguageChange(string oldCode, string newCode)
	{
		base.Reload(true);
	}

	// Token: 0x060023AA RID: 9130 RVA: 0x000FC968 File Offset: 0x000FAB68
	private void OnCreateFolder()
	{
		UIDialog.ShowDropDownInput(Language.Translate("Create folder in {0}", this.TitleLabel.text), "Create", "Cancel", base.Folders, new Action<string, string>(this.CreateFolder), null, "", "Folder Name", this.GetCurrentFolder());
	}

	// Token: 0x060023AB RID: 9131 RVA: 0x000FC9BC File Offset: 0x000FABBC
	private void CreateFolder(string LocalFolder, string Name)
	{
		string text = SerializationScript.RemoveInvalidCharsFromFileName(Name);
		if (DirectoryScript.IsBlacklistedFolder(text))
		{
			Chat.LogError("This folder name is not allowed.", true);
			return;
		}
		SerializationScript.CreateDirectory(SerializationScript.CombineLocalFolder(base.RootPath, LocalFolder, text));
	}

	// Token: 0x060023AC RID: 9132 RVA: 0x000FC9F6 File Offset: 0x000FABF6
	private void OnClearTable()
	{
		UIDialog.Show("Are you sure you want to clear the table?", "Clear", "Cancel", new Action(this.ClearTable), null);
	}

	// Token: 0x060023AD RID: 9133 RVA: 0x000FCA19 File Offset: 0x000FAC19
	private void ClearTable()
	{
		if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
		{
			NetworkSingleton<NetworkUI>.Instance.GUIChangeGame(NetworkSingleton<NetworkUI>.Instance.GameNone);
			base.gameObject.SetActive(false);
			return;
		}
		Chat.NotPromotedErrorMessage("clear table");
	}

	// Token: 0x060023AE RID: 9134 RVA: 0x000FCA54 File Offset: 0x000FAC54
	private void SortChange()
	{
		string a = this.currentCategory;
		if (!(a == "CLASSIC"))
		{
			if (!(a == "DLC"))
			{
				if (!(a == "WORKSHOP"))
				{
					if (a == "SAVE & LOAD")
					{
						this.currentSavesSort = (this.CurrentSortLabel.text = this.SortPopup.value);
					}
				}
				else
				{
					this.currentWorkshopSort = (this.CurrentSortLabel.text = this.SortPopup.value);
				}
			}
			else
			{
				this.currentDLCSort = (this.CurrentSortLabel.text = this.SortPopup.value);
			}
		}
		else
		{
			this.currentClassicSort = (this.CurrentSortLabel.text = this.SortPopup.value);
		}
		base.Reload(true);
	}

	// Token: 0x060023AF RID: 9135 RVA: 0x000FCB2A File Offset: 0x000FAD2A
	public void Open<T>(string name, GameObject prefabButton, Action<int, string> caller, string path, List<T> buttons, List<string> PopupListItems, int page) where T : UIGridMenu.GridButton
	{
		base.ChangeButtonPrefab(prefabButton);
		this.ToggleGridMenu(true, caller, path, PopupListItems);
		this.TitleLabel.text = name;
		NGUIHelper.ClampAndAddDots(this.TitleLabel);
		base.Load<T>(buttons, page, "", true, false);
	}

	// Token: 0x060023B0 RID: 9136 RVA: 0x000FCB68 File Offset: 0x000FAD68
	public override void OnClickBack()
	{
		if (this.PrevMenuActions.Count == 0)
		{
			this.OpenFeatured(1, "");
			return;
		}
		this.blockAddBack = true;
		UIGridMenuGames.GridMenuGameState gridMenuGameState = this.PrevMenuActions[this.PrevMenuActions.Count - 1];
		this.PrevMenuActions.RemoveAt(this.PrevMenuActions.Count - 1);
		gridMenuGameState.MenuAction(gridMenuGameState.page, gridMenuGameState.Path);
		this.SearchInput.value = gridMenuGameState.search;
	}

	// Token: 0x060023B1 RID: 9137 RVA: 0x000FCBEF File Offset: 0x000FADEF
	private void ResetMenuActions()
	{
		this.PrevMenuActions.Clear();
		this.LastMenuAction = null;
	}

	// Token: 0x060023B2 RID: 9138 RVA: 0x000FCC03 File Offset: 0x000FAE03
	private void OnClickFeatured()
	{
		this.OpenFeatured(1, "");
	}

	// Token: 0x060023B3 RID: 9139 RVA: 0x000FCC14 File Offset: 0x000FAE14
	public async void OpenFeatured(int page, string path = "")
	{
		UIGridMenuGames.<>c__DisplayClass90_0 CS$<>8__locals1 = new UIGridMenuGames.<>c__DisplayClass90_0();
		CS$<>8__locals1.<>4__this = this;
		this.ToggleGridMenu(false, new Action<int, string>(this.OpenFeatured), path, new List<string>());
		this.ResetSearch(true);
		CS$<>8__locals1.closeMenu = base.gameObject;
		try
		{
			this.FeaturedClassicGridMenu.Load<UIGridMenu.GridButtonClassic>(this.GetClassicGridButtons(CS$<>8__locals1.closeMenu), 1, "", true, false);
		}
		catch (Exception e)
		{
			Chat.LogException("loading classic games", e, true, false);
		}
		try
		{
			this.FeaturedDLCGridMenu.Load<UIGridMenu.GridButtonDLC>(this.GetDLCGridButtons(CS$<>8__locals1.closeMenu), 1, "", true, false);
		}
		catch (Exception e2)
		{
			Chat.LogException("loading DLC games", e2, true, false);
		}
		Task task = CS$<>8__locals1.<OpenFeatured>g__OpenFeaturedWorkshop|0();
		Task task2 = CS$<>8__locals1.<OpenFeatured>g__OpenFeaturedSaves|1();
		await Task.WhenAll(new Task[]
		{
			task,
			task2
		});
		this.ResetMenuActions();
	}

	// Token: 0x060023B4 RID: 9140 RVA: 0x000FCC55 File Offset: 0x000FAE55
	private void OnClickClassic()
	{
		this.OpenClassic(1, "");
	}

	// Token: 0x060023B5 RID: 9141 RVA: 0x000FCC64 File Offset: 0x000FAE64
	public void OpenClassic(int page, string path = "")
	{
		base.SetPathFolders("", "");
		this.currentCategory = "CLASSIC";
		this.CurrentSortLabel.text = this.currentClassicSort;
		this.Open<UIGridMenu.GridButtonClassic>("CLASSIC", this.PrefabButtonLessText, new Action<int, string>(this.OpenClassic), path, this.GetClassicGridButtons(base.gameObject), this.ClassicSorts.Keys.ToList<string>(), page);
		this.ClassicHelp.SetActive(true);
	}

	// Token: 0x060023B6 RID: 9142 RVA: 0x000FCCE4 File Offset: 0x000FAEE4
	private void OnClickDLC()
	{
		this.OpenDLC(1, "");
	}

	// Token: 0x060023B7 RID: 9143 RVA: 0x000FCCF4 File Offset: 0x000FAEF4
	public void OpenDLC(int page, string path = "")
	{
		base.SetPathFolders("", "");
		this.currentCategory = "DLC";
		this.CurrentSortLabel.text = this.currentDLCSort;
		this.Open<UIGridMenu.GridButtonDLC>("DLC", this.PrefabButtonLessText, new Action<int, string>(this.OpenDLC), path, this.GetDLCGridButtons(base.gameObject), this.DLCSorts.Keys.ToList<string>(), page);
		this.DLCHelp.SetActive(true);
	}

	// Token: 0x060023B8 RID: 9144 RVA: 0x000FCD74 File Offset: 0x000FAF74
	private void OnClickWorkshop()
	{
		if (this.LoadingWheelGridMenu.activeSelf)
		{
			Chat.LogWarning("Already loading games", true);
			return;
		}
		this.OpenWorkshop(1, "");
	}

	// Token: 0x060023B9 RID: 9145 RVA: 0x000FCD9C File Offset: 0x000FAF9C
	public async void OpenWorkshop(int page, string path = "")
	{
		base.SetPathFolders(DirectoryScript.workshopFilePath, "");
		this.currentCategory = "WORKSHOP";
		this.CurrentSortLabel.text = this.currentWorkshopSort;
		GameObject gameObject = base.gameObject;
		bool bGetAllSaveFileWithoutFolders = this.loadWithoutFolders;
		this.ToggleGridMenu(true, null, null, this.WorkshopSorts.Keys.ToList<string>());
		this.LoadingWheelGridMenu.SetActive(true);
		List<UIGridMenu.GridButtonPath> buttons = await this.AsyncGetWorkshopGridButtons(gameObject, bGetAllSaveFileWithoutFolders, path);
		this.LoadingWheelGridMenu.SetActive(false);
		this.Open<UIGridMenu.GridButtonPath>(string.IsNullOrEmpty(path) ? "WORKSHOP" : Path.GetFileName(path), this.PrefabButtonMoreText, new Action<int, string>(this.OpenWorkshop), path, buttons, this.WorkshopSorts.Keys.ToList<string>(), page);
		this.CreateFolderButton.gameObject.SetActive(true);
		this.WorkshopAdditionalButton.gameObject.SetActive(true);
		this.WorkshopHelp.SetActive(true);
	}

	// Token: 0x060023BA RID: 9146 RVA: 0x000FCDE5 File Offset: 0x000FAFE5
	private void OnClickSaves()
	{
		if (this.LoadingWheelGridMenu.activeSelf)
		{
			Chat.LogWarning("Already loading games", true);
			return;
		}
		this.OpenSaves(1, "");
	}

	// Token: 0x060023BB RID: 9147 RVA: 0x000FCE0C File Offset: 0x000FB00C
	public async void OpenSaves(int page, string path = "")
	{
		base.SetPathFolders(DirectoryScript.saveFilePath, DirectoryScript.savedObjectsFilePath);
		this.currentCategory = "SAVE & LOAD";
		this.CurrentSortLabel.text = this.currentSavesSort;
		GameObject gameObject = base.gameObject;
		bool bGetAllSaveFileWithoutFolders = this.loadWithoutFolders;
		this.ToggleGridMenu(true, null, null, this.SavesSorts.Keys.ToList<string>());
		this.LoadingWheelGridMenu.SetActive(true);
		List<UIGridMenu.GridButtonPath> buttons = await this.AsyncGetSaveGridButtons(gameObject, bGetAllSaveFileWithoutFolders, path);
		this.LoadingWheelGridMenu.SetActive(false);
		this.Open<UIGridMenu.GridButtonPath>(string.IsNullOrEmpty(path) ? "SAVE & LOAD" : Path.GetFileName(path), this.PrefabButtonMoreText, new Action<int, string>(this.OpenSaves), path, buttons, this.SavesSorts.Keys.ToList<string>(), page);
		this.CreateFolderButton.gameObject.SetActive(true);
		this.SavesAdditionalButton.gameObject.SetActive(true);
		this.SavesHelp.SetActive(true);
	}

	// Token: 0x060023BC RID: 9148 RVA: 0x000FCE58 File Offset: 0x000FB058
	public async void OpenAll(int page, string path = "")
	{
		base.SetPathFolders("", "");
		this.currentCategory = "ALL GAMES";
		this.CurrentSortLabel.text = "Name";
		GameObject gameObject = base.gameObject;
		this.ToggleGridMenu(true, null, null, this.WorkshopSorts.Keys.ToList<string>());
		this.LoadingWheelGridMenu.SetActive(true);
		List<UIGridMenu.GridButton> buttons = await this.AsyncGetAllGridButtons(gameObject);
		this.LoadingWheelGridMenu.SetActive(false);
		this.Open<UIGridMenu.GridButton>("ALL GAMES", this.PrefabButtonMoreText, new Action<int, string>(this.OpenAll), path, buttons, new List<string>(), page);
	}

	// Token: 0x060023BD RID: 9149 RVA: 0x000FCEA4 File Offset: 0x000FB0A4
	private void ToggleGridMenu(bool isGridMenu, Action<int, string> caller, string path, List<string> PopupListItems)
	{
		this.SortPopup.gameObject.SetActive(PopupListItems.Count > 0);
		this.SortPopup.items = PopupListItems;
		if (caller != null)
		{
			if (!this.blockAddBack && this.LastMenuAction != null)
			{
				UIGridMenuGames.GridMenuGameState gridMenuGameState = new UIGridMenuGames.GridMenuGameState(this.TitleLabel.text, null, base.currentPage, this.SearchInput.value);
				gridMenuGameState.MenuAction = this.LastMenuAction;
				gridMenuGameState.Path = base.CurrentPath;
				gridMenuGameState.Sort = -1;
				this.PrevMenuActions.Add(gridMenuGameState);
			}
			this.LastMenuAction = caller;
			base.CurrentPath = path;
			this.blockAddBack = false;
		}
		this.CreateFolderButton.gameObject.SetActive(false);
		this.WorkshopAdditionalButton.gameObject.SetActive(false);
		this.SavesAdditionalButton.gameObject.SetActive(false);
		this.ClassicHelp.SetActive(false);
		this.DLCHelp.SetActive(false);
		this.WorkshopHelp.SetActive(false);
		this.SavesHelp.SetActive(false);
		this.FeaturedMenu.SetActive(!isGridMenu);
		this.GridMenu.SetActive(isGridMenu);
	}

	// Token: 0x060023BE RID: 9150 RVA: 0x000FCFCC File Offset: 0x000FB1CC
	private void OnWorkshopBrowse()
	{
		TTSUtilities.OpenURL("http://steamcommunity.com/app/286160/workshop/");
	}

	// Token: 0x060023BF RID: 9151 RVA: 0x000FCFD8 File Offset: 0x000FB1D8
	private void OnCreateSave()
	{
		UIDialog.ShowDropDownInput(Language.Translate("Save Slot {0}", SerializationScript.GetNewSaveSlot().ToString()), "Save", "Cancel", base.Folders, new Action<string, string>(this.CreateSave), null, NetworkSingleton<GameOptions>.Instance.GameName, "Save Name", this.GetCurrentFolder());
	}

	// Token: 0x060023C0 RID: 9152 RVA: 0x000FD034 File Offset: 0x000FB234
	private void CreateSave(string LocalFolder, string Name)
	{
		string filePath = SerializationScript.CombineLocalFolder(DirectoryScript.saveFilePath, LocalFolder, "TS_Save_" + SerializationScript.GetNewSaveSlot() + ".json");
		NetworkSingleton<NetworkUI>.Instance.GUISaveSlot(filePath, Name);
	}

	// Token: 0x060023C1 RID: 9153 RVA: 0x000FD074 File Offset: 0x000FB274
	public async Task<List<UIGridMenu.GridButton>> AsyncGetAllGridButtons(GameObject closeMenu)
	{
		UIGridMenuGames.<>c__DisplayClass105_0 CS$<>8__locals1 = new UIGridMenuGames.<>c__DisplayClass105_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.closeMenu = closeMenu;
		CS$<>8__locals1.classicGridButtons = this.GetClassicGridButtons(CS$<>8__locals1.closeMenu);
		CS$<>8__locals1.dlcGridButtons = this.GetDLCGridButtons(CS$<>8__locals1.closeMenu);
		return await Task.Run<List<UIGridMenu.GridButton>>(delegate()
		{
			UIGridMenuGames.<>c__DisplayClass105_0.<<AsyncGetAllGridButtons>b__0>d <<AsyncGetAllGridButtons>b__0>d;
			<<AsyncGetAllGridButtons>b__0>d.<>4__this = CS$<>8__locals1;
			<<AsyncGetAllGridButtons>b__0>d.<>t__builder = AsyncTaskMethodBuilder<List<UIGridMenu.GridButton>>.Create();
			<<AsyncGetAllGridButtons>b__0>d.<>1__state = -1;
			AsyncTaskMethodBuilder<List<UIGridMenu.GridButton>> <>t__builder = <<AsyncGetAllGridButtons>b__0>d.<>t__builder;
			<>t__builder.Start<UIGridMenuGames.<>c__DisplayClass105_0.<<AsyncGetAllGridButtons>b__0>d>(ref <<AsyncGetAllGridButtons>b__0>d);
			return <<AsyncGetAllGridButtons>b__0>d.<>t__builder.Task;
		});
	}

	// Token: 0x060023C2 RID: 9154 RVA: 0x000FD0C4 File Offset: 0x000FB2C4
	public List<UIGridMenu.GridButtonClassic> GetClassicGridButtons(GameObject closeMenu)
	{
		List<UIGridMenu.GridButtonClassic> list = new List<UIGridMenu.GridButtonClassic>();
		for (int i = 0; i < this.buttonsClassic.Count; i++)
		{
			UIGridMenu.GridButtonClassic gridButtonClassic = this.buttonsClassic[i];
			gridButtonClassic.LastPlayed = (uint)PlayerPrefs.GetInt("LastPlayed" + gridButtonClassic.Name, 0);
			gridButtonClassic.ButtonColor = this.ClassicColor;
			gridButtonClassic.BackgroundColor = this.ClassicDarkColor;
			gridButtonClassic.Autotranslate = true;
			gridButtonClassic.CloseMenu = closeMenu;
			gridButtonClassic.Tags.TryAddUnique("classic");
			gridButtonClassic.Autotranslate = true;
			list.Add(gridButtonClassic);
		}
		Action<List<UIGridMenu.GridButtonClassic>> action;
		if (this.ClassicSorts.TryGetValue(this.currentClassicSort, out action) && action != null)
		{
			action(list);
		}
		list.Reverse();
		return list;
	}

	// Token: 0x060023C3 RID: 9155 RVA: 0x000FD184 File Offset: 0x000FB384
	public List<UIGridMenu.GridButtonDLC> GetDLCGridButtons(GameObject closeMenu)
	{
		List<UIGridMenu.GridButtonDLC> list = new List<UIGridMenu.GridButtonDLC>();
		List<DLCWebsiteInfo> dlcinfos = DLCManager.DLCInfos;
		for (int i = 0; i < dlcinfos.Count; i++)
		{
			DLCWebsiteInfo dlcwebsiteInfo = dlcinfos[i];
			bool flag = SteamManager.IsSubscribedApp(dlcwebsiteInfo.AppId);
			if (!dlcwebsiteInfo.HideIfNotOwned || flag)
			{
				UIGridMenu.GridButtonDLC gridButtonDLC = new UIGridMenu.GridButtonDLC();
				gridButtonDLC.Name = (string.IsNullOrEmpty(dlcwebsiteInfo.DisplayName) ? dlcwebsiteInfo.Name : dlcwebsiteInfo.DisplayName);
				gridButtonDLC.LoadName = dlcwebsiteInfo.Name;
				gridButtonDLC.ThumbnailURL = dlcwebsiteInfo.ThumbnailURL;
				gridButtonDLC.AppId = dlcwebsiteInfo.AppId;
				gridButtonDLC.DiscountPercent = dlcwebsiteInfo.DiscountPercent;
				gridButtonDLC.New = dlcwebsiteInfo.New;
				gridButtonDLC.Lock = !flag;
				gridButtonDLC.Purchased = SteamManager.SubscribeDate(dlcwebsiteInfo.AppId);
				gridButtonDLC.ButtonColor = this.DLCColor;
				gridButtonDLC.BackgroundColor = this.DLCDarkColor;
				gridButtonDLC.CloseMenu = closeMenu;
				gridButtonDLC.Tags.TryAddUnique("dlc");
				list.Add(gridButtonDLC);
			}
		}
		Action<List<UIGridMenu.GridButtonDLC>> action;
		if (this.DLCSorts.TryGetValue(this.currentDLCSort, out action) && action != null)
		{
			action(list);
		}
		list.Reverse();
		return list;
	}

	// Token: 0x060023C4 RID: 9156 RVA: 0x000FD2D0 File Offset: 0x000FB4D0
	public async Task<List<UIGridMenu.GridButtonPath>> AsyncGetWorkshopGridButtons(GameObject closeMenu, bool bGetAllSaveFileWithoutFolders = false, string path = "")
	{
		return await Task.Run<List<UIGridMenu.GridButtonPath>>(delegate()
		{
			List<UIGridMenu.GridButtonPath> result;
			try
			{
				result = this.GetWorkshopGridButtons(closeMenu, bGetAllSaveFileWithoutFolders, path);
			}
			catch (Exception e)
			{
				Chat.LogException("loading Workshop games", e, true, false);
				result = new List<UIGridMenu.GridButtonPath>();
			}
			return result;
		});
	}

	// Token: 0x060023C5 RID: 9157 RVA: 0x000FD330 File Offset: 0x000FB530
	public List<UIGridMenu.GridButtonPath> GetWorkshopGridButtons(GameObject closeMenu, bool bGetAllSaveFileWithoutFolders = false, string path = "")
	{
		if (string.IsNullOrEmpty(path))
		{
			path = DirectoryScript.workshopFilePath;
		}
		this.GetWorkshopFileInfos(path, bGetAllSaveFileWithoutFolders);
		List<UIGridMenu.GridButtonPath> list = new List<UIGridMenu.GridButtonPath>();
		if (!bGetAllSaveFileWithoutFolders)
		{
			string[] array = SerializationScript.GetDirectories(path, false, null).ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				try
				{
					string path2 = array[i];
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path2);
					list.Add(new UIGridMenu.GridButtonFolderWorkshop
					{
						Path = path2,
						Name = fileNameWithoutExtension,
						ButtonColor = this.WorkshopColor,
						BackgroundColor = this.WorkshopDarkColor
					});
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
			list.Sort((UIGridMenu.GridButtonPath x, UIGridMenu.GridButtonPath y) => AlphanumComparatorFast.Compare(x.Name, y.Name));
		}
		List<UIGridMenu.GridButtonWorkshop> list2 = new List<UIGridMenu.GridButtonWorkshop>();
		for (int j = 0; j < this.CurrentWorkshopFileInfos.Count; j++)
		{
			try
			{
				SaveFileInfo saveFileInfo = this.CurrentWorkshopFileInfos[j];
				UIGridMenu.GridButtonWorkshop gridButtonWorkshop = new UIGridMenu.GridButtonWorkshop();
				gridButtonWorkshop.Name = saveFileInfo.Name;
				gridButtonWorkshop.Path = saveFileInfo.Directory;
				gridButtonWorkshop.UpdateTime = saveFileInfo.UpdateTime;
				if (this.MoreWorkshopFileInfos.Count > j)
				{
					gridButtonWorkshop.LoadTime = this.MoreWorkshopFileInfos[j].LoadTime;
					gridButtonWorkshop.DownloadTime = this.MoreWorkshopFileInfos[j].CreationTime;
				}
				ulong workshopId;
				ulong.TryParse(Path.GetFileNameWithoutExtension(saveFileInfo.Directory), out workshopId);
				gridButtonWorkshop.WorkshopId = workshopId;
				gridButtonWorkshop.ThumbnailPath = Path.ChangeExtension(saveFileInfo.Directory, ".png");
				gridButtonWorkshop.ButtonColor = this.WorkshopColor;
				gridButtonWorkshop.BackgroundColor = this.WorkshopDarkColor;
				gridButtonWorkshop.CloseMenu = closeMenu;
				gridButtonWorkshop.Tags.TryAddUnique("workshop");
				list2.Add(gridButtonWorkshop);
			}
			catch (Exception exception2)
			{
				Debug.LogException(exception2);
			}
		}
		Action<List<UIGridMenu.GridButtonWorkshop>> action;
		if (this.WorkshopSorts.TryGetValue(this.currentWorkshopSort, out action) && action != null)
		{
			action(list2);
		}
		list2.Reverse();
		for (int k = 0; k < list2.Count; k++)
		{
			list.Add(list2[k]);
		}
		return list;
	}

	// Token: 0x060023C6 RID: 9158 RVA: 0x000FD584 File Offset: 0x000FB784
	public async Task<List<UIGridMenu.GridButtonPath>> AsyncGetSaveGridButtons(GameObject closeMenu, bool bGetAllSaveFileWithoutFolders = false, string path = "")
	{
		return await Task.Run<List<UIGridMenu.GridButtonPath>>(delegate()
		{
			List<UIGridMenu.GridButtonPath> result;
			try
			{
				result = this.GetSaveGridButtons(closeMenu, bGetAllSaveFileWithoutFolders, path);
			}
			catch (Exception e)
			{
				Chat.LogException("loading Save games", e, true, false);
				result = new List<UIGridMenu.GridButtonPath>();
			}
			return result;
		});
	}

	// Token: 0x060023C7 RID: 9159 RVA: 0x000FD5E4 File Offset: 0x000FB7E4
	public List<UIGridMenu.GridButtonPath> GetSaveGridButtons(GameObject closeMenu, bool bGetAllSaveFileWithoutFolders = false, string path = "")
	{
		if (string.IsNullOrEmpty(path))
		{
			path = DirectoryScript.saveFilePath;
		}
		this.GetSaveFileInfos(path, bGetAllSaveFileWithoutFolders);
		List<UIGridMenu.GridButtonPath> list = new List<UIGridMenu.GridButtonPath>();
		if (!bGetAllSaveFileWithoutFolders)
		{
			string[] array = SerializationScript.GetDirectories(path, false, null).ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				try
				{
					string path2 = array[i];
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path2);
					UIGridMenu.GridButtonFolderSave item = new UIGridMenu.GridButtonFolderSave
					{
						Path = path2,
						Name = fileNameWithoutExtension,
						ButtonColor = this.SavesColor,
						BackgroundColor = this.SavesDarkColor
					};
					list.Add(item);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
			list.Sort((UIGridMenu.GridButtonPath x, UIGridMenu.GridButtonPath y) => AlphanumComparatorFast.Compare(x.Name, y.Name));
		}
		List<UIGridMenu.GridButtonSave> list2 = new List<UIGridMenu.GridButtonSave>();
		for (int j = 0; j < this.CurrentSaveFileInfos.Count; j++)
		{
			try
			{
				SaveFileInfo saveFileInfo = this.CurrentSaveFileInfos[j];
				UIGridMenu.GridButtonSave gridButtonSave = new UIGridMenu.GridButtonSave();
				gridButtonSave.Path = saveFileInfo.Directory;
				gridButtonSave.UpdateTime = saveFileInfo.UpdateTime;
				if (this.MoreSaveFileInfos.Count > j)
				{
					gridButtonSave.LoadTime = this.MoreSaveFileInfos[j].LoadTime;
				}
				string[] array2 = saveFileInfo.Name.Split(new string[]
				{
					" - "
				}, StringSplitOptions.RemoveEmptyEntries);
				if (array2.Length != 0)
				{
					if (array2[0] == "-1")
					{
						string text = Path.GetFileNameWithoutExtension(saveFileInfo.Directory);
						if (text.Length > 16)
						{
							text = text.Substring(0, 14) + "...";
						}
						gridButtonSave.TopLeftText = text;
					}
					else
					{
						gridButtonSave.TopLeftText = array2[0];
					}
					if (array2[0].StartsWith("Auto Save"))
					{
						if (gridButtonSave.OptionsPopupActions.ContainsKey("Overwrite"))
						{
							gridButtonSave.OptionsPopupActions.Remove("Overwrite");
						}
						if (gridButtonSave.OptionsPopupActions.ContainsKey("Move"))
						{
							gridButtonSave.OptionsPopupActions.Remove("Move");
						}
					}
				}
				string text2 = "";
				for (int k = 1; k < array2.Length - 1; k++)
				{
					if (!string.IsNullOrEmpty(text2))
					{
						text2 += " - ";
					}
					text2 += array2[k];
				}
				gridButtonSave.SaveName = text2;
				UIGridMenu.GridButtonSave gridButtonSave2 = gridButtonSave;
				gridButtonSave2.Name += text2;
				uint updateTime;
				if (!SerializationScript.SaveNameToSlot(Path.GetFileNameWithoutExtension(saveFileInfo.Directory), out updateTime))
				{
					updateTime = gridButtonSave.UpdateTime;
				}
				gridButtonSave.Slot = updateTime;
				gridButtonSave.ThumbnailPath = Path.ChangeExtension(saveFileInfo.Directory, ".png");
				gridButtonSave.ButtonColor = this.SavesColor;
				gridButtonSave.BackgroundColor = this.SavesDarkColor;
				gridButtonSave.CloseMenu = closeMenu;
				gridButtonSave.Tags.TryAddUnique("saves");
				list2.Add(gridButtonSave);
			}
			catch (Exception exception2)
			{
				Debug.LogException(exception2);
			}
		}
		Action<List<UIGridMenu.GridButtonSave>> action;
		if (this.SavesSorts.TryGetValue(this.currentSavesSort, out action) && action != null)
		{
			action(list2);
		}
		list2.Reverse();
		for (int l = 0; l < list2.Count; l++)
		{
			list.Add(list2[l]);
		}
		return list;
	}

	// Token: 0x060023C8 RID: 9160 RVA: 0x000FD960 File Offset: 0x000FBB60
	private bool GetWorkshopFileInfos(string path, bool bGetAllSaveFileWithoutFolders)
	{
		this.MoreWorkshopFileInfos.Clear();
		this.CurrentWorkshopFileInfos.Clear();
		List<string> saveFiles = SerializationScript.GetSaveFiles(path, bGetAllSaveFileWithoutFolders, null);
		bool flag = false;
		for (int i = 0; i < saveFiles.Count; i++)
		{
			SaveFileInfo saveFileInfo = new SaveFileInfo();
			saveFileInfo.Directory = saveFiles[i];
			FileInfo fileInfo = new FileInfo(saveFileInfo.Directory);
			uint timeFromEpoch = SerializationScript.GetTimeFromEpoch(fileInfo.LastWriteTimeUtc);
			uint timeFromEpoch2 = SerializationScript.GetTimeFromEpoch(fileInfo.LastAccessTimeUtc);
			uint timeFromEpoch3 = SerializationScript.GetTimeFromEpoch(fileInfo.CreationTimeUtc);
			saveFileInfo.UpdateTime = timeFromEpoch;
			UIGridMenuGames.MoreSaveFileInfo moreSaveFileInfo = new UIGridMenuGames.MoreSaveFileInfo();
			moreSaveFileInfo.LoadTime = timeFromEpoch2;
			moreSaveFileInfo.CreationTime = timeFromEpoch3;
			this.MoreWorkshopFileInfos.Add(moreSaveFileInfo);
			SaveFileInfo saveFileInfo2 = null;
			for (int j = 0; j < this.WorkshopFileInfos.Count; j++)
			{
				if (this.WorkshopFileInfos[j].Directory == saveFileInfo.Directory)
				{
					saveFileInfo2 = this.WorkshopFileInfos[j];
					break;
				}
			}
			if (saveFileInfo2 == null || timeFromEpoch != saveFileInfo2.UpdateTime)
			{
				flag = true;
				string saveNameWorkshop = SerializationScript.GetSaveNameWorkshop(saveFiles[i]);
				saveFileInfo.Name = saveNameWorkshop;
				if (saveFileInfo2 != null)
				{
					this.WorkshopFileInfos.Remove(saveFileInfo2);
				}
				this.WorkshopFileInfos.Add(saveFileInfo);
			}
			else
			{
				saveFileInfo.Name = saveFileInfo2.Name;
			}
			this.CurrentWorkshopFileInfos.Add(saveFileInfo);
		}
		List<string> saveFiles2 = SerializationScript.GetSaveFiles(DirectoryScript.workshopFilePath, true, null);
		for (int k = this.WorkshopFileInfos.Count - 1; k >= 0; k--)
		{
			if (!saveFiles2.Contains(this.WorkshopFileInfos[k].Directory))
			{
				Debug.Log("Cleanup deleted files: " + this.WorkshopFileInfos[k].Directory);
				flag = true;
				this.WorkshopFileInfos.RemoveAt(k);
			}
		}
		if (flag)
		{
			SerializationScript.Save(this.WorkshopFileInfos, DirectoryScript.workshopFilePath + "//WorkshopFileInfos.json");
			Debug.Log("bNeedToUpdate");
		}
		return flag;
	}

	// Token: 0x060023C9 RID: 9161 RVA: 0x000FDB68 File Offset: 0x000FBD68
	private bool GetSaveFileInfos(string path, bool bGetAllSaveFileWithoutFolders)
	{
		this.MoreSaveFileInfos.Clear();
		this.CurrentSaveFileInfos.Clear();
		List<string> saveFiles = SerializationScript.GetSaveFiles(path, bGetAllSaveFileWithoutFolders, DirectoryScript.SavedObjectsDirectories);
		bool flag = false;
		for (int i = 0; i < saveFiles.Count; i++)
		{
			SaveFileInfo saveFileInfo = new SaveFileInfo();
			saveFileInfo.Directory = saveFiles[i];
			FileInfo fileInfo = new FileInfo(saveFileInfo.Directory);
			uint timeFromEpoch = SerializationScript.GetTimeFromEpoch(fileInfo.LastWriteTimeUtc);
			uint timeFromEpoch2 = SerializationScript.GetTimeFromEpoch(fileInfo.LastAccessTimeUtc);
			saveFileInfo.UpdateTime = timeFromEpoch;
			UIGridMenuGames.MoreSaveFileInfo moreSaveFileInfo = new UIGridMenuGames.MoreSaveFileInfo();
			moreSaveFileInfo.LoadTime = timeFromEpoch2;
			this.MoreSaveFileInfos.Add(moreSaveFileInfo);
			SaveFileInfo saveFileInfo2 = null;
			for (int j = 0; j < this.SaveFileInfos.Count; j++)
			{
				if (this.SaveFileInfos[j].Directory == saveFileInfo.Directory)
				{
					saveFileInfo2 = this.SaveFileInfos[j];
					break;
				}
			}
			if (saveFileInfo2 == null || timeFromEpoch != saveFileInfo2.UpdateTime)
			{
				flag = true;
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(saveFiles[i]);
				uint num;
				string name;
				uint num2;
				if (SerializationScript.SaveNameToSlot(fileNameWithoutExtension, out num))
				{
					name = num + SerializationScript.GetSaveName(saveFiles[i]);
				}
				else if (SerializationScript.AutoSaveNameToSlot(fileNameWithoutExtension, out num2))
				{
					name = "Auto Save " + num2 + SerializationScript.GetSaveName(saveFiles[i]);
				}
				else
				{
					name = "-1" + SerializationScript.GetSaveName(saveFiles[i]);
				}
				saveFileInfo.Name = name;
				if (saveFileInfo2 != null)
				{
					this.SaveFileInfos.Remove(saveFileInfo2);
				}
				this.SaveFileInfos.Add(saveFileInfo);
			}
			else
			{
				saveFileInfo.Name = saveFileInfo2.Name;
			}
			this.CurrentSaveFileInfos.Add(saveFileInfo);
		}
		string b = path + "//TS_AutoSave.json";
		List<string> saveFiles2 = SerializationScript.GetSaveFiles(DirectoryScript.saveFilePath, true, DirectoryScript.SavedObjectsDirectories);
		for (int k = this.SaveFileInfos.Count - 1; k >= 0; k--)
		{
			if (!(this.SaveFileInfos[k].Directory == b) && !(this.SaveFileInfos[k].Directory == DirectoryScript.saveFilePath + "//TS_AutoSave.json") && !saveFiles2.Contains(this.SaveFileInfos[k].Directory))
			{
				Debug.Log("Cleanup deleted files: " + this.SaveFileInfos[k].Directory);
				flag = true;
				this.SaveFileInfos.RemoveAt(k);
			}
		}
		if (flag)
		{
			SerializationScript.Save(this.SaveFileInfos, DirectoryScript.saveFilePath + "//SaveFileInfos.json");
			Debug.Log("bNeedToUpdate");
		}
		return flag;
	}

	// Token: 0x060023CA RID: 9162 RVA: 0x000FDE34 File Offset: 0x000FC034
	public override void ResetSearch(bool blockOnChange = true)
	{
		base.ResetSearch(blockOnChange);
		this.prevSearch = "";
	}

	// Token: 0x060023CB RID: 9163 RVA: 0x000FDE48 File Offset: 0x000FC048
	public override void Search(string search)
	{
		if (this.FeaturedMenu.activeSelf && !string.IsNullOrEmpty(search) && string.IsNullOrEmpty(this.prevSearch))
		{
			this.OpenAll(1, "");
		}
		else if (!string.IsNullOrEmpty(search) && string.IsNullOrEmpty(this.prevSearch) && this.LastMenuAction != null)
		{
			this.loadWithoutFolders = true;
			this.Reload(base.currentPage, true);
			this.loadWithoutFolders = false;
		}
		else if (string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(this.prevSearch) && this.LastMenuAction != null)
		{
			this.Reload(base.currentPage, true);
		}
		this.prevSearch = search;
		base.Search(search);
	}

	// Token: 0x060023CC RID: 9164 RVA: 0x000FDEF7 File Offset: 0x000FC0F7
	public override void Reload(int page, bool DelayThumbnailCleanup = true)
	{
		base.Reload(page, DelayThumbnailCleanup);
		if (this.LastMenuAction == null)
		{
			this.OpenFeatured(1, "");
			return;
		}
		this.blockAddBack = true;
		this.LastMenuAction(base.currentPage, base.CurrentPath);
	}

	// Token: 0x060023CD RID: 9165 RVA: 0x000FDF34 File Offset: 0x000FC134
	public UIGridMenuGames()
	{
		Dictionary<string, Action<List<UIGridMenu.GridButtonClassic>>> dictionary = new Dictionary<string, Action<List<UIGridMenu.GridButtonClassic>>>();
		dictionary.Add("Random", delegate(List<UIGridMenu.GridButtonClassic> list)
		{
			list.Randomize<UIGridMenu.GridButtonClassic>();
		});
		dictionary.Add("Name", delegate(List<UIGridMenu.GridButtonClassic> list)
		{
			list.Sort((UIGridMenu.GridButtonClassic x, UIGridMenu.GridButtonClassic y) => y.Name.CompareTo(x.Name));
		});
		dictionary.Add("Last Played", delegate(List<UIGridMenu.GridButtonClassic> list)
		{
			list.Sort((UIGridMenu.GridButtonClassic x, UIGridMenu.GridButtonClassic y) => x.LastPlayed.CompareTo(y.LastPlayed));
		});
		this.ClassicSorts = dictionary;
		this.currentDLCSort = "Random";
		Dictionary<string, Action<List<UIGridMenu.GridButtonDLC>>> dictionary2 = new Dictionary<string, Action<List<UIGridMenu.GridButtonDLC>>>();
		dictionary2.Add("Random", delegate(List<UIGridMenu.GridButtonDLC> list)
		{
			list.Randomize<UIGridMenu.GridButtonDLC>();
			list.Sort(delegate(UIGridMenu.GridButtonDLC x, UIGridMenu.GridButtonDLC y)
			{
				if (x.New)
				{
					return 1;
				}
				if (!y.New)
				{
					return 0;
				}
				return -1;
			});
		});
		dictionary2.Add("Released", null);
		dictionary2.Add("Purchased", delegate(List<UIGridMenu.GridButtonDLC> list)
		{
			list.Sort((UIGridMenu.GridButtonDLC x, UIGridMenu.GridButtonDLC y) => x.Purchased.CompareTo(y.Purchased));
		});
		dictionary2.Add("Name", delegate(List<UIGridMenu.GridButtonDLC> list)
		{
			list.Sort((UIGridMenu.GridButtonDLC x, UIGridMenu.GridButtonDLC y) => y.Name.CompareTo(x.Name));
		});
		this.DLCSorts = dictionary2;
		this.currentWorkshopSort = "Updated";
		Dictionary<string, Action<List<UIGridMenu.GridButtonWorkshop>>> dictionary3 = new Dictionary<string, Action<List<UIGridMenu.GridButtonWorkshop>>>();
		dictionary3.Add("Updated", delegate(List<UIGridMenu.GridButtonWorkshop> list)
		{
			list.Sort((UIGridMenu.GridButtonWorkshop x, UIGridMenu.GridButtonWorkshop y) => x.UpdateTime.CompareTo(y.UpdateTime));
		});
		dictionary3.Add("Created", delegate(List<UIGridMenu.GridButtonWorkshop> list)
		{
			list.Sort((UIGridMenu.GridButtonWorkshop x, UIGridMenu.GridButtonWorkshop y) => x.WorkshopId.CompareTo(y.WorkshopId));
		});
		dictionary3.Add("Loaded", delegate(List<UIGridMenu.GridButtonWorkshop> list)
		{
			list.Sort((UIGridMenu.GridButtonWorkshop x, UIGridMenu.GridButtonWorkshop y) => x.LoadTime.CompareTo(y.LoadTime));
		});
		dictionary3.Add("Downloaded", delegate(List<UIGridMenu.GridButtonWorkshop> list)
		{
			list.Sort((UIGridMenu.GridButtonWorkshop x, UIGridMenu.GridButtonWorkshop y) => x.DownloadTime.CompareTo(y.DownloadTime));
		});
		dictionary3.Add("Name", delegate(List<UIGridMenu.GridButtonWorkshop> list)
		{
			list.Sort((UIGridMenu.GridButtonWorkshop x, UIGridMenu.GridButtonWorkshop y) => AlphanumComparatorFast.Compare(y.Name, x.Name));
		});
		dictionary3.Add("Random", delegate(List<UIGridMenu.GridButtonWorkshop> list)
		{
			list.Randomize<UIGridMenu.GridButtonWorkshop>();
		});
		this.WorkshopSorts = dictionary3;
		this.currentSavesSort = "Slot";
		Dictionary<string, Action<List<UIGridMenu.GridButtonSave>>> dictionary4 = new Dictionary<string, Action<List<UIGridMenu.GridButtonSave>>>();
		dictionary4.Add("Slot", delegate(List<UIGridMenu.GridButtonSave> list)
		{
			list.Sort((UIGridMenu.GridButtonSave x, UIGridMenu.GridButtonSave y) => x.Slot.CompareTo(y.Slot));
		});
		dictionary4.Add("Updated", delegate(List<UIGridMenu.GridButtonSave> list)
		{
			list.Sort((UIGridMenu.GridButtonSave x, UIGridMenu.GridButtonSave y) => x.UpdateTime.CompareTo(y.UpdateTime));
		});
		dictionary4.Add("Loaded", delegate(List<UIGridMenu.GridButtonSave> list)
		{
			list.Sort((UIGridMenu.GridButtonSave x, UIGridMenu.GridButtonSave y) => x.LoadTime.CompareTo(y.LoadTime));
		});
		dictionary4.Add("Name", delegate(List<UIGridMenu.GridButtonSave> list)
		{
			list.Sort((UIGridMenu.GridButtonSave x, UIGridMenu.GridButtonSave y) => AlphanumComparatorFast.Compare(y.SaveName, x.SaveName));
		});
		dictionary4.Add("Random", delegate(List<UIGridMenu.GridButtonSave> list)
		{
			list.Randomize<UIGridMenu.GridButtonSave>();
		});
		this.SavesSorts = dictionary4;
		this.currentCategory = "";
		this.SaveInfos = new Dictionary<string, SaveInfo>();
		base..ctor();
	}

	// Token: 0x040016B4 RID: 5812
	public static int MaxAutosavesOnFeatureRow = 1;

	// Token: 0x040016B5 RID: 5813
	public List<UIGridMenu.GridButtonClassic> buttonsClassic = new List<UIGridMenu.GridButtonClassic>();

	// Token: 0x040016B6 RID: 5814
	public GameObject PrefabButtonLessText;

	// Token: 0x040016B7 RID: 5815
	public GameObject PrefabButtonMoreText;

	// Token: 0x040016B8 RID: 5816
	public Color ClassicColor;

	// Token: 0x040016B9 RID: 5817
	public Color ClassicDarkColor;

	// Token: 0x040016BA RID: 5818
	public Color DLCColor;

	// Token: 0x040016BB RID: 5819
	public Color DLCDarkColor;

	// Token: 0x040016BC RID: 5820
	public Color WorkshopColor;

	// Token: 0x040016BD RID: 5821
	public Color WorkshopDarkColor;

	// Token: 0x040016BE RID: 5822
	public Color SavesColor;

	// Token: 0x040016BF RID: 5823
	public Color SavesDarkColor;

	// Token: 0x040016C0 RID: 5824
	public UIButton ClassicButton;

	// Token: 0x040016C1 RID: 5825
	public UIButton DLCButton;

	// Token: 0x040016C2 RID: 5826
	public UIButton WorkshopButton;

	// Token: 0x040016C3 RID: 5827
	public UIButton SavesButton;

	// Token: 0x040016C4 RID: 5828
	public UIButton BackButtonGames;

	// Token: 0x040016C5 RID: 5829
	public UIButton ClearButton;

	// Token: 0x040016C6 RID: 5830
	public UILabel TitleLabel;

	// Token: 0x040016C7 RID: 5831
	public GameObject LoadingWheelWorkshop;

	// Token: 0x040016C8 RID: 5832
	public GameObject LoadingWheelSaves;

	// Token: 0x040016C9 RID: 5833
	public GameObject LoadingWheelGridMenu;

	// Token: 0x040016CA RID: 5834
	public UIPopupList SortPopup;

	// Token: 0x040016CB RID: 5835
	public UILabel CurrentSortLabel;

	// Token: 0x040016CC RID: 5836
	public UIButton SortOrderButton;

	// Token: 0x040016CD RID: 5837
	public UIDualSlider PlayerRangeSlider;

	// Token: 0x040016CE RID: 5838
	public GameObject FilterByTags;

	// Token: 0x040016CF RID: 5839
	public UIInput TagInput;

	// Token: 0x040016D0 RID: 5840
	public GameObject Tag;

	// Token: 0x040016D1 RID: 5841
	public UIButton CreateFolderButton;

	// Token: 0x040016D2 RID: 5842
	public GameObject FeaturedMenu;

	// Token: 0x040016D3 RID: 5843
	public GameObject GridMenu;

	// Token: 0x040016D4 RID: 5844
	public GameObject FilterMenu;

	// Token: 0x040016D5 RID: 5845
	public UIButton WorkshopAdditionalButton;

	// Token: 0x040016D6 RID: 5846
	public UIButton SavesAdditionalButton;

	// Token: 0x040016D7 RID: 5847
	public GameObject ClassicHelp;

	// Token: 0x040016D8 RID: 5848
	public GameObject DLCHelp;

	// Token: 0x040016D9 RID: 5849
	public GameObject WorkshopHelp;

	// Token: 0x040016DA RID: 5850
	public GameObject SavesHelp;

	// Token: 0x040016DB RID: 5851
	public UIGridMenu FeaturedClassicGridMenu;

	// Token: 0x040016DC RID: 5852
	public UIGridMenu FeaturedDLCGridMenu;

	// Token: 0x040016DD RID: 5853
	public UIGridMenu FeaturedWorkshopGridMenu;

	// Token: 0x040016DE RID: 5854
	public UIGridMenu FeaturedSavesGridMenu;

	// Token: 0x040016DF RID: 5855
	public const string ClassicTitle = "CLASSIC";

	// Token: 0x040016E0 RID: 5856
	public const string DLCTitle = "DLC";

	// Token: 0x040016E1 RID: 5857
	public const string WorkshopTitle = "WORKSHOP";

	// Token: 0x040016E2 RID: 5858
	public const string SavesTitle = "SAVE & LOAD";

	// Token: 0x040016E3 RID: 5859
	public const string AllTitle = "ALL GAMES";

	// Token: 0x040016E4 RID: 5860
	private List<SaveFileInfo> SaveFileInfos = new List<SaveFileInfo>();

	// Token: 0x040016E5 RID: 5861
	private List<SaveFileInfo> CurrentSaveFileInfos = new List<SaveFileInfo>();

	// Token: 0x040016E6 RID: 5862
	private List<SaveFileInfo> WorkshopFileInfos = new List<SaveFileInfo>();

	// Token: 0x040016E7 RID: 5863
	private List<SaveFileInfo> CurrentWorkshopFileInfos = new List<SaveFileInfo>();

	// Token: 0x040016E8 RID: 5864
	private List<UIGridMenuGames.MoreSaveFileInfo> MoreSaveFileInfos = new List<UIGridMenuGames.MoreSaveFileInfo>();

	// Token: 0x040016E9 RID: 5865
	private List<UIGridMenuGames.MoreSaveFileInfo> MoreWorkshopFileInfos = new List<UIGridMenuGames.MoreSaveFileInfo>();

	// Token: 0x040016EA RID: 5866
	private Action<int, string> LastMenuAction;

	// Token: 0x040016EB RID: 5867
	private List<UIGridMenuGames.GridMenuGameState> PrevMenuActions = new List<UIGridMenuGames.GridMenuGameState>();

	// Token: 0x040016EC RID: 5868
	private string currentClassicSort = "Random";

	// Token: 0x040016ED RID: 5869
	private Dictionary<string, Action<List<UIGridMenu.GridButtonClassic>>> ClassicSorts;

	// Token: 0x040016EE RID: 5870
	private string currentDLCSort;

	// Token: 0x040016EF RID: 5871
	private Dictionary<string, Action<List<UIGridMenu.GridButtonDLC>>> DLCSorts;

	// Token: 0x040016F0 RID: 5872
	private string currentWorkshopSort;

	// Token: 0x040016F1 RID: 5873
	private Dictionary<string, Action<List<UIGridMenu.GridButtonWorkshop>>> WorkshopSorts;

	// Token: 0x040016F2 RID: 5874
	private string currentSavesSort;

	// Token: 0x040016F3 RID: 5875
	private Dictionary<string, Action<List<UIGridMenu.GridButtonSave>>> SavesSorts;

	// Token: 0x040016F4 RID: 5876
	private const string ClassicSortPrefs = "GamesClassicSort";

	// Token: 0x040016F5 RID: 5877
	private const string DLCSortPrefs = "GamesDLCSort";

	// Token: 0x040016F6 RID: 5878
	private const string WorkshopSortPrefs = "GamesWorkshopSort";

	// Token: 0x040016F7 RID: 5879
	private const string SavesSortPrefs = "GamesSavesSort";

	// Token: 0x040016F8 RID: 5880
	private string currentCategory;

	// Token: 0x040016F9 RID: 5881
	private bool blockAddBack;

	// Token: 0x040016FA RID: 5882
	private Dictionary<string, SaveInfo> SaveInfos;

	// Token: 0x040016FB RID: 5883
	private string prevSearch;

	// Token: 0x040016FC RID: 5884
	private bool loadWithoutFolders;

	// Token: 0x02000745 RID: 1861
	public class MoreSaveFileInfo
	{
		// Token: 0x04002B24 RID: 11044
		public uint LoadTime;

		// Token: 0x04002B25 RID: 11045
		public uint CreationTime;
	}

	// Token: 0x02000746 RID: 1862
	public class GridMenuGameState : UIGridMenu.GridMenuState
	{
		// Token: 0x06003E5A RID: 15962 RVA: 0x0017EF79 File Offset: 0x0017D179
		public GridMenuGameState(string title, List<UIGridMenu.GridButton> buttons, int page, string search) : base(title, buttons, page, search, false)
		{
		}

		// Token: 0x04002B26 RID: 11046
		public Action<int, string> MenuAction;

		// Token: 0x04002B27 RID: 11047
		public string Path;

		// Token: 0x04002B28 RID: 11048
		public int Sort;
	}
}
