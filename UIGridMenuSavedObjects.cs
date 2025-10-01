using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x020002DD RID: 733
public class UIGridMenuSavedObjects : UIGridMenu
{
	// Token: 0x06002403 RID: 9219 RVA: 0x000FF494 File Offset: 0x000FD694
	protected override void Awake()
	{
		base.Awake();
		EventDelegate.Add(this.CreateFolderButton.onClick, new EventDelegate.Callback(this.OnCreateFolder));
		EventManager.OnFileSave += this.OnFileSave;
		EventManager.OnFileDelete += this.OnFileDelete;
		this.SortPopup.items = this.SavedObjectsSorts.Keys.ToList<string>();
		EventDelegate.Add(this.SortPopup.onChange, new EventDelegate.Callback(this.SortChange));
		this.LoadSorts();
	}

	// Token: 0x06002404 RID: 9220 RVA: 0x000FF524 File Offset: 0x000FD724
	protected override void OnDestroy()
	{
		base.OnDestroy();
		EventDelegate.Remove(this.CreateFolderButton.onClick, new EventDelegate.Callback(this.OnCreateFolder));
		EventManager.OnFileSave -= this.OnFileSave;
		EventManager.OnFileDelete -= this.OnFileDelete;
		EventDelegate.Remove(this.SortPopup.onChange, new EventDelegate.Callback(this.SortChange));
	}

	// Token: 0x06002405 RID: 9221 RVA: 0x000FF593 File Offset: 0x000FD793
	protected override void OnEnable()
	{
		base.OnEnable();
		this.Init();
	}

	// Token: 0x06002406 RID: 9222 RVA: 0x000FF5A1 File Offset: 0x000FD7A1
	protected override void OnDisable()
	{
		base.OnDisable();
		this.SaveSorts();
	}

	// Token: 0x06002407 RID: 9223 RVA: 0x000FF5AF File Offset: 0x000FD7AF
	private void Init()
	{
		base.ClearBackStates();
		this.Load("SAVED OBJECTS", false, "");
	}

	// Token: 0x06002408 RID: 9224 RVA: 0x000FF5C8 File Offset: 0x000FD7C8
	public async void Load(string name, bool addBack, string path = "")
	{
		bool bGetAllSaveFileWithoutFolders = this.loadWithoutFolders;
		this.LoadingWheel.SetActive(true);
		List<UIGridMenu.GridButtonPath> buttons = await this.AsyncGetSavedObjectGridButtons(path, bGetAllSaveFileWithoutFolders);
		this.LoadingWheel.SetActive(false);
		try
		{
			base.Load<UIGridMenu.GridButtonPath>(buttons, 1, name, addBack, true);
		}
		catch (Exception e)
		{
			Chat.LogException("loading saved objects", e, true, false);
		}
	}

	// Token: 0x06002409 RID: 9225 RVA: 0x000FF61C File Offset: 0x000FD81C
	private void LoadSorts()
	{
		if (PlayerPrefs.HasKey("SavedObjectsSort"))
		{
			string @string = PlayerPrefs.GetString("SavedObjectsSort");
			if (this.SavedObjectsSorts.ContainsKey(@string))
			{
				this.currentSort = @string;
			}
		}
	}

	// Token: 0x0600240A RID: 9226 RVA: 0x000FF655 File Offset: 0x000FD855
	private void SaveSorts()
	{
		PlayerPrefs.SetString("SavedObjectsSort", this.currentSort);
	}

	// Token: 0x0600240B RID: 9227 RVA: 0x000FF668 File Offset: 0x000FD868
	private void OnCreateFolder()
	{
		UIDialog.ShowDropDownInput("Create folder in Saved Objects", "Create", "Cancel", base.Folders, new Action<string, string>(this.CreateFolder), null, "", "Folder Name", "");
	}

	// Token: 0x0600240C RID: 9228 RVA: 0x000FF6AB File Offset: 0x000FD8AB
	private void SortChange()
	{
		this.currentSort = this.SortPopup.value;
		base.Reload(true);
	}

	// Token: 0x0600240D RID: 9229 RVA: 0x000FF6C8 File Offset: 0x000FD8C8
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

	// Token: 0x0600240E RID: 9230 RVA: 0x000FF701 File Offset: 0x000FD901
	private void OnFileSave(string Path)
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		if (Path.StartsWith(DirectoryScript.savedObjectsFilePath))
		{
			base.Reload(false);
		}
	}

	// Token: 0x0600240F RID: 9231 RVA: 0x000FF725 File Offset: 0x000FD925
	private void OnFileDelete(string Path)
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		base.Reload(true);
	}

	// Token: 0x06002410 RID: 9232 RVA: 0x000FF73C File Offset: 0x000FD93C
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

	// Token: 0x06002411 RID: 9233 RVA: 0x000FF794 File Offset: 0x000FD994
	public List<UIGridMenu.GridButtonPath> GetSavedObjectGridButtons(string path, bool bGetAllSaveFileWithoutFolders)
	{
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
				string path2 = this.SaveChestFolderList[i];
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path2);
				list.Add(new UIGridMenu.GridButtonFolderSavedObject
				{
					Path = path2,
					Name = fileNameWithoutExtension
				});
			}
			list.Sort((UIGridMenu.GridButtonPath x, UIGridMenu.GridButtonPath y) => AlphanumComparatorFast.Compare(x.Name, y.Name));
		}
		List<UIGridMenu.GridButtonSavedObject> list2 = new List<UIGridMenu.GridButtonSavedObject>();
		for (int j = 0; j < this.SaveChestList.Count; j++)
		{
			string text = this.SaveChestList[j];
			UIGridMenu.GridButtonSavedObject gridButtonSavedObject = new UIGridMenu.GridButtonSavedObject();
			gridButtonSavedObject.Path = text;
			gridButtonSavedObject.Name = Path.GetFileNameWithoutExtension(text);
			FileInfo fileInfo = new FileInfo(text);
			gridButtonSavedObject.UpdateTime = SerializationScript.GetTimeFromEpoch(fileInfo.LastWriteTimeUtc);
			gridButtonSavedObject.LoadTime = SerializationScript.GetTimeFromEpoch(fileInfo.LastAccessTimeUtc);
			gridButtonSavedObject.ThumbnailPath = Path.ChangeExtension(text, ".png");
			list2.Add(gridButtonSavedObject);
		}
		Action<List<UIGridMenu.GridButtonSavedObject>> action;
		if (this.SavedObjectsSorts.TryGetValue(this.currentSort, out action) && action != null)
		{
			action(list2);
		}
		for (int k = 0; k < list2.Count; k++)
		{
			list.Add(list2[k]);
		}
		return list;
	}

	// Token: 0x06002412 RID: 9234 RVA: 0x000FF911 File Offset: 0x000FDB11
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

	// Token: 0x06002413 RID: 9235 RVA: 0x000FF950 File Offset: 0x000FDB50
	public override void Search(string search)
	{
		if (!string.IsNullOrEmpty(search) && string.IsNullOrEmpty(this.prevSearch))
		{
			this.loadWithoutFolders = true;
			this.Reload(base.currentPage, true);
			this.loadWithoutFolders = false;
		}
		if (string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(this.prevSearch))
		{
			this.Reload(base.currentPage, true);
		}
		this.prevSearch = search;
		base.Search(search);
	}

	// Token: 0x06002414 RID: 9236 RVA: 0x000FF9BD File Offset: 0x000FDBBD
	public override void Reload(int page, bool DelayThumbnailCleanup = true)
	{
		base.Reload(page, DelayThumbnailCleanup);
		this.Init();
	}

	// Token: 0x06002415 RID: 9237 RVA: 0x000FF9D0 File Offset: 0x000FDBD0
	public UIGridMenuSavedObjects()
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

	// Token: 0x04001718 RID: 5912
	public UIButton CreateFolderButton;

	// Token: 0x04001719 RID: 5913
	public UIPopupList SortPopup;

	// Token: 0x0400171A RID: 5914
	public GameObject LoadingWheel;

	// Token: 0x0400171B RID: 5915
	private string currentSort = "Name";

	// Token: 0x0400171C RID: 5916
	private Dictionary<string, Action<List<UIGridMenu.GridButtonSavedObject>>> SavedObjectsSorts;

	// Token: 0x0400171D RID: 5917
	private const string SavedObjectsSortPrefs = "SavedObjectsSort";

	// Token: 0x0400171E RID: 5918
	private List<string> SaveChestList;

	// Token: 0x0400171F RID: 5919
	private List<string> SaveChestFolderList;

	// Token: 0x04001720 RID: 5920
	private string prevSearch;

	// Token: 0x04001721 RID: 5921
	private bool loadWithoutFolders;
}
