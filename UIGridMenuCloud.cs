using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

// Token: 0x020002D6 RID: 726
public class UIGridMenuCloud : UIGridMenu
{
	// Token: 0x0600237C RID: 9084 RVA: 0x000FB440 File Offset: 0x000F9640
	protected override void Awake()
	{
		base.Awake();
		EventManager.OnCloudUploadFinish += this.CloudUploadFinish;
		EventManager.OnLanguageChange += this.OnLanguageChange;
		EventDelegate.Add(this.UploadAllFiles.onClick, new EventDelegate.Callback(this.OnClickUploadAllFiles));
		EventDelegate.Add(this.CreateFolderButton.onClick, new EventDelegate.Callback(this.OnCreateFolder));
		this.SortPopup.items = this.CloudSorts.Keys.ToList<string>();
		EventDelegate.Add(this.SortPopup.onChange, new EventDelegate.Callback(this.SortChange));
		this.LoadSorts();
	}

	// Token: 0x0600237D RID: 9085 RVA: 0x000FB4F0 File Offset: 0x000F96F0
	protected override void OnDestroy()
	{
		base.OnDestroy();
		EventManager.OnCloudUploadFinish -= this.CloudUploadFinish;
		EventManager.OnLanguageChange -= this.OnLanguageChange;
		EventDelegate.Remove(this.UploadAllFiles.onClick, new EventDelegate.Callback(this.OnClickUploadAllFiles));
		EventDelegate.Remove(this.CreateFolderButton.onClick, new EventDelegate.Callback(this.OnCreateFolder));
		EventDelegate.Remove(this.SortPopup.onChange, new EventDelegate.Callback(this.SortChange));
	}

	// Token: 0x0600237E RID: 9086 RVA: 0x000FB57C File Offset: 0x000F977C
	protected override void OnDisable()
	{
		base.OnDisable();
		this.SaveSorts();
	}

	// Token: 0x0600237F RID: 9087 RVA: 0x000FB58A File Offset: 0x000F978A
	private void SortChange()
	{
		this.currentSort = this.SortPopup.value;
		base.Reload(true);
	}

	// Token: 0x06002380 RID: 9088 RVA: 0x000FB5A4 File Offset: 0x000F97A4
	private void LoadSorts()
	{
		if (PlayerPrefs.HasKey("CloudSort"))
		{
			string @string = PlayerPrefs.GetString("CloudSort");
			if (this.CloudSorts.ContainsKey(@string))
			{
				this.currentSort = @string;
			}
		}
	}

	// Token: 0x06002381 RID: 9089 RVA: 0x000FB5DD File Offset: 0x000F97DD
	private void SaveSorts()
	{
		PlayerPrefs.SetString("CloudSort", this.currentSort);
	}

	// Token: 0x06002382 RID: 9090 RVA: 0x000FB5F0 File Offset: 0x000F97F0
	private void CloudUploadFinish(string name, string url)
	{
		if (base.gameObject.activeSelf && url != "")
		{
			base.Reload(true);
		}
		if (this.cloudNameAndURL.ContainsKey(name))
		{
			string key = this.cloudNameAndURL[name];
			this.cloudNameAndURL.Remove(name);
			if (!string.IsNullOrEmpty(url))
			{
				this.urlReplacer.Add(key, url);
			}
			this.ContinueCloudUpload();
		}
	}

	// Token: 0x06002383 RID: 9091 RVA: 0x000FB661 File Offset: 0x000F9861
	protected override void OnEnable()
	{
		base.OnEnable();
		this.Init();
	}

	// Token: 0x06002384 RID: 9092 RVA: 0x000FB670 File Offset: 0x000F9870
	private void Init()
	{
		base.ClearBackStates();
		base.Load<UIGridMenu.GridButtonCloudBase>(this.GetCloudButtons(null), 1, "CLOUD", false, true);
		ulong num;
		Singleton<SteamManager>.Instance.GetCloudQuota(out num, out this.availabledSize);
		this.UpdateQuotaText();
	}

	// Token: 0x06002385 RID: 9093 RVA: 0x000FB6B0 File Offset: 0x000F98B0
	private void OnLanguageChange(string oldCode, string newCode)
	{
		this.UpdateQuotaText();
	}

	// Token: 0x06002386 RID: 9094 RVA: 0x000FB6B8 File Offset: 0x000F98B8
	private void UpdateQuotaText()
	{
		this.QuotaLabel.text = Language.Translate("{0} free", Utilities.BytesToFileSizeString((long)this.availabledSize));
	}

	// Token: 0x06002387 RID: 9095 RVA: 0x000FB6DC File Offset: 0x000F98DC
	public List<UIGridMenu.GridButtonCloudBase> GetCloudButtons(string targetFolder = null)
	{
		List<UIGridMenu.GridButtonCloudBase> list = new List<UIGridMenu.GridButtonCloudBase>();
		List<string> cloudFolders = Singleton<SteamManager>.Instance.GetCloudFolders();
		Dictionary<string, CloudInfo> cloudInfos = Singleton<SteamManager>.Instance.GetCloudInfos();
		base.Folders.Clear();
		base.Folders.Add("<Root Folder>");
		for (int i = 0; i < cloudFolders.Count; i++)
		{
			base.Folders.Add(cloudFolders[i]);
		}
		base.Folders.Sort(new Comparison<string>(AlphanumComparatorFast.Compare));
		base.RootPath = targetFolder;
		base.CurrentPath = targetFolder;
		if (!this.loadWithoutFolders)
		{
			foreach (string text in cloudFolders)
			{
				try
				{
					if (!(text == targetFolder))
					{
						string text2 = text;
						if (!string.IsNullOrEmpty(targetFolder))
						{
							if (!text.StartsWith(targetFolder))
							{
								continue;
							}
							text2 = text.Substring(targetFolder.Length);
						}
						bool flag = false;
						for (int j = 1; j < text2.Length; j++)
						{
							char c = text2[j];
							if (c == '/' || c == '\\')
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							list.Add(new UIGridMenu.GridButtonCloudFolder
							{
								Name = Path.GetFileNameWithoutExtension(text),
								folder = text
							});
							if (string.IsNullOrEmpty(targetFolder))
							{
								text.Contains("/");
							}
						}
					}
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
			list.Sort((UIGridMenu.GridButtonCloudBase x, UIGridMenu.GridButtonCloudBase y) => AlphanumComparatorFast.Compare(x.Name, y.Name));
		}
		List<UIGridMenu.GridButtonCloudBase> list2 = new List<UIGridMenu.GridButtonCloudBase>();
		foreach (KeyValuePair<string, CloudInfo> keyValuePair in cloudInfos)
		{
			try
			{
				string key = keyValuePair.Key;
				CloudInfo value = keyValuePair.Value;
				if (this.loadWithoutFolders || (string.IsNullOrEmpty(targetFolder) && !string.IsNullOrEmpty(value.Folder) && !base.Folders.Contains(value.Folder)) || SteamManager.FolderNameEquals(value.Folder, targetFolder))
				{
					UIGridMenu.GridButtonCloud gridButtonCloud = new UIGridMenu.GridButtonCloud();
					gridButtonCloud.Name = value.Name;
					gridButtonCloud.Size = value.Size;
					gridButtonCloud.TopLeftText = Utilities.BytesToFileSizeString((long)value.Size);
					gridButtonCloud.URL = value.URL;
					gridButtonCloud.DelayTooltip = value.Date;
					gridButtonCloud.CloudName = key;
					string text3 = Path.GetExtension(value.Name);
					if (!string.IsNullOrEmpty(text3))
					{
						text3 = text3.ToLower();
					}
					ObjectState objectState = new ObjectState();
					objectState.Transform = new TransformState(base.transform);
					if (value.Size < 10000000)
					{
						if (CustomLoadingManager.SupportedImageFormatExtensions.Contains(text3) && text3 != ".unity3d")
						{
							objectState.Name = "Custom_Tile";
							objectState.CustomImage = new CustomImageState();
							objectState.CustomImage.ImageURL = value.URL;
							gridButtonCloud.ThumbnailPath = gridButtonCloud.URL;
						}
						else if (text3 == ".obj")
						{
							objectState.Name = "Custom_Model";
							objectState.CustomMesh = new CustomMeshState();
							objectState.CustomMesh.MeshURL = value.URL;
						}
						else if (text3 == ".unity3d")
						{
							objectState.Name = "Custom_AssetBundle";
							objectState.CustomAssetbundle = new CustomAssetbundleState();
							objectState.CustomAssetbundle.AssetbundleURL = value.URL;
						}
					}
					if (!string.IsNullOrEmpty(objectState.Name))
					{
						gridButtonCloud.objectState = objectState;
					}
					list2.Add(gridButtonCloud);
				}
			}
			catch (Exception exception2)
			{
				Debug.LogException(exception2);
			}
		}
		Action<List<UIGridMenu.GridButtonCloudBase>> action;
		if (this.CloudSorts.TryGetValue(this.currentSort, out action) && action != null)
		{
			action(list2);
		}
		for (int k = 0; k < list2.Count; k++)
		{
			list.Add(list2[k]);
		}
		return list;
	}

	// Token: 0x06002388 RID: 9096 RVA: 0x000FBB5C File Offset: 0x000F9D5C
	private void OnClickUploadAllFiles()
	{
		UIDialog.ShowDropDownInput("Upload all loaded custom files to the Steam Cloud?", "Upload", "Cancel", base.Folders, delegate(string LocalFolder, string Name)
		{
			if (LocalFolder == "<Root Folder>")
			{
				LocalFolder = "";
			}
			string text = SerializationScript.RemoveInvalidCharsFromFileName(Name);
			if (string.IsNullOrEmpty(text))
			{
				Chat.LogError("Folder name is invalid", true);
				return;
			}
			this.uploadAllFolderName = Path.Combine(LocalFolder, text);
			Singleton<SteamManager>.Instance.AddCloudFolder(this.uploadAllFolderName, true);
			this.ConvertAssetsToCloud();
		}, null, NetworkSingleton<GameOptions>.Instance.GameName, "Name", this.GetCurrentFolder());
	}

	// Token: 0x06002389 RID: 9097 RVA: 0x000FBBA8 File Offset: 0x000F9DA8
	public void ConvertAssetsToCloud()
	{
		if (this.UploadingToCloud)
		{
			Chat.LogError("Already uploading files to cloud.", true);
			return;
		}
		this.UploadingToCloud = true;
		this.LoadingWheel.SetActive(true);
		this.saveState = NetworkSingleton<ManagerPhysicsObject>.Instance.CurrentState();
		this.urls = SaveScript.GetURLs(this.saveState);
		this.ContinueCloudUpload();
	}

	// Token: 0x0600238A RID: 9098 RVA: 0x000FBC04 File Offset: 0x000F9E04
	private void ContinueCloudUpload()
	{
		if (this.urls.Count <= 0)
		{
			if (this.urlReplacer.Count > 0)
			{
				SaveScript.SetURLs(this.saveState, this.urlReplacer);
				Chat.Log("Reloading game to replace all files with Steam Cloud version. Resave this game now.", Colour.Purple, ChatMessageType.All, true);
				NetworkSingleton<ManagerPhysicsObject>.Instance.LoadSaveState(this.saveState, false, true);
			}
			else
			{
				Chat.LogError("No files need to be uploaded to the Steam Cloud.", true);
			}
			this.UploadingToCloud = false;
			this.LoadingWheel.SetActive(false);
			this.saveState = null;
			this.uploadAllFolderName = null;
			this.urls.Clear();
			this.cloudNameAndURL.Clear();
			this.urlReplacer.Clear();
			return;
		}
		string text = this.urls[0];
		this.urls.Remove(text);
		if (SteamCloudURL.IsCloudURL(text) || DLCManager.URLisDLC(text))
		{
			Debug.Log("Skip: " + text);
			this.ContinueCloudUpload();
			return;
		}
		string text2 = CustomCache.CheckImageExist(text, CacheMode.NoRawCache, false, true);
		if (!DataRequest.IsLocalFile(text2))
		{
			text2 = CustomCache.CheckModelExist(text, CacheMode.NoRawCache, false, true);
			if (!DataRequest.IsLocalFile(text2))
			{
				text2 = CustomCache.CheckAssetbundleExist(text, CacheMode.NoRawCache, false, true);
				if (!DataRequest.IsLocalFile(text2))
				{
					Chat.LogError("Failed to find file cached on disk: " + text, true);
					this.ContinueCloudUpload();
					return;
				}
			}
		}
		text2 = text2.Replace("file:///", "");
		string fileName = Path.GetFileName(text2);
		this.cloudNameAndURL.Add(fileName, text);
		Singleton<SteamManager>.Instance.UploadToCloud(fileName, File.ReadAllBytes(text2), this.uploadAllFolderName);
	}

	// Token: 0x0600238B RID: 9099 RVA: 0x000FBD7F File Offset: 0x000F9F7F
	public List<string> GetFolders()
	{
		List<string> cloudFolders = Singleton<SteamManager>.Instance.GetCloudFolders();
		cloudFolders.Add("<Root Folder>");
		cloudFolders.Sort(new Comparison<string>(AlphanumComparatorFast.Compare));
		return cloudFolders;
	}

	// Token: 0x0600238C RID: 9100 RVA: 0x000FBDA8 File Offset: 0x000F9FA8
	public override string GetCurrentFolder()
	{
		if (!string.IsNullOrEmpty(base.CurrentPath))
		{
			return base.CurrentPath;
		}
		return "<Root Folder>";
	}

	// Token: 0x0600238D RID: 9101 RVA: 0x000FBDC4 File Offset: 0x000F9FC4
	private void OnCreateFolder()
	{
		UIDialog.ShowDropDownInput("Create folder in Cloud", "Create", "Cancel", base.Folders, new Action<string, string>(this.CreateFolder), null, "", "Folder Name", this.GetCurrentFolder());
	}

	// Token: 0x0600238E RID: 9102 RVA: 0x000FBE08 File Offset: 0x000FA008
	private void CreateFolder(string LocalFolder, string Name)
	{
		if (LocalFolder == "<Root Folder>")
		{
			LocalFolder = "";
		}
		string text = SerializationScript.RemoveInvalidCharsFromFileName(Name);
		if (string.IsNullOrEmpty(text))
		{
			Chat.LogError("Folder name is invalid", true);
			return;
		}
		string name = Path.Combine(LocalFolder, text);
		Singleton<SteamManager>.Instance.AddCloudFolder(name, false);
		base.Reload(true);
	}

	// Token: 0x0600238F RID: 9103 RVA: 0x000FBE60 File Offset: 0x000FA060
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

	// Token: 0x06002390 RID: 9104 RVA: 0x000FBED0 File Offset: 0x000FA0D0
	public override void Reload(int page, bool DelayThumbnailCleanup = true)
	{
		base.Reload(page, DelayThumbnailCleanup);
		int currentPage = base.currentPage;
		string currentPath = base.CurrentPath;
		this.Init();
		if (!string.IsNullOrEmpty(currentPath))
		{
			foreach (string b in currentPath.Split(new char[]
			{
				'\\'
			}))
			{
				using (List<UIGridMenu.GridButton>.Enumerator enumerator = this.currentButtons.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						UIGridMenu.GridButtonCloudFolder gridButtonCloudFolder;
						if ((gridButtonCloudFolder = (enumerator.Current as UIGridMenu.GridButtonCloudFolder)) != null && gridButtonCloudFolder.Name == b)
						{
							gridButtonCloudFolder.OnClick();
							break;
						}
					}
				}
			}
		}
		base.SafeSetPage(currentPage);
	}

	// Token: 0x06002391 RID: 9105 RVA: 0x000FBF90 File Offset: 0x000FA190
	public UIGridMenuCloud()
	{
		Dictionary<string, Action<List<UIGridMenu.GridButtonCloudBase>>> dictionary = new Dictionary<string, Action<List<UIGridMenu.GridButtonCloudBase>>>();
		dictionary.Add("Created", delegate(List<UIGridMenu.GridButtonCloudBase> list)
		{
			list.Reverse();
		});
		dictionary.Add("Name", delegate(List<UIGridMenu.GridButtonCloudBase> list)
		{
			list.Sort((UIGridMenu.GridButtonCloudBase x, UIGridMenu.GridButtonCloudBase y) => AlphanumComparatorFast.Compare(x.Name, y.Name));
		});
		dictionary.Add("Size", delegate(List<UIGridMenu.GridButtonCloudBase> list)
		{
			list.Sort((UIGridMenu.GridButtonCloudBase x, UIGridMenu.GridButtonCloudBase y) => y.Size.CompareTo(x.Size));
		});
		dictionary.Add("Random", delegate(List<UIGridMenu.GridButtonCloudBase> list)
		{
			list.Randomize<UIGridMenu.GridButtonCloudBase>();
		});
		this.CloudSorts = dictionary;
		this.urls = new List<string>();
		this.cloudNameAndURL = new Dictionary<string, string>();
		this.urlReplacer = new Dictionary<string, string>();
		this.prevSearch = "";
		base..ctor();
	}

	// Token: 0x04001696 RID: 5782
	public UILabel QuotaLabel;

	// Token: 0x04001697 RID: 5783
	public UIButton UploadAllFiles;

	// Token: 0x04001698 RID: 5784
	public GameObject LoadingWheel;

	// Token: 0x04001699 RID: 5785
	public UIPopupList SortPopup;

	// Token: 0x0400169A RID: 5786
	public UIButton CreateFolderButton;

	// Token: 0x0400169B RID: 5787
	private ulong availabledSize;

	// Token: 0x0400169C RID: 5788
	private string currentSort = "Created";

	// Token: 0x0400169D RID: 5789
	private Dictionary<string, Action<List<UIGridMenu.GridButtonCloudBase>>> CloudSorts;

	// Token: 0x0400169E RID: 5790
	private const string CloudSortPrefs = "CloudSort";

	// Token: 0x0400169F RID: 5791
	private bool UploadingToCloud;

	// Token: 0x040016A0 RID: 5792
	private string uploadAllFolderName;

	// Token: 0x040016A1 RID: 5793
	private SaveState saveState;

	// Token: 0x040016A2 RID: 5794
	private List<string> urls;

	// Token: 0x040016A3 RID: 5795
	private Dictionary<string, string> cloudNameAndURL;

	// Token: 0x040016A4 RID: 5796
	private Dictionary<string, string> urlReplacer;

	// Token: 0x040016A5 RID: 5797
	private string prevSearch;

	// Token: 0x040016A6 RID: 5798
	private bool loadWithoutFolders;
}
