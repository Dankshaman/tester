using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using AOT;
using Steamworks;
using UnityEngine;
using UnityEngine.Rendering;
using VacuumShaders.TextureExtensions;

// Token: 0x02000242 RID: 578
[DisallowMultipleComponent]
public class SteamManager : Singleton<SteamManager>
{
	// Token: 0x17000431 RID: 1073
	// (get) Token: 0x06001C9D RID: 7325 RVA: 0x000C4DE8 File Offset: 0x000C2FE8
	private static SteamManager _Instance
	{
		get
		{
			if (SteamManager.s_instance == null)
			{
				return new GameObject("SteamManager").AddComponent<SteamManager>();
			}
			return SteamManager.s_instance;
		}
	}

	// Token: 0x17000432 RID: 1074
	// (get) Token: 0x06001C9E RID: 7326 RVA: 0x000C4E0C File Offset: 0x000C300C
	public static bool Initialized
	{
		get
		{
			return SteamManager._Instance.m_bInitialized;
		}
	}

	// Token: 0x06001C9F RID: 7327 RVA: 0x000C4E18 File Offset: 0x000C3018
	[MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
	private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	// Token: 0x06001CA0 RID: 7328 RVA: 0x000C4E20 File Offset: 0x000C3020
	protected override void Awake()
	{
		base.Awake();
		if (Utilities.IsLaunchOption("-nosteam"))
		{
			Chat.LogSystem("Steam is disabled by -nosteam launch option. (Multiplayer will not work)", Colour.Blue, true);
			return;
		}
		Debug.Log("Start init Steamwork.NET Wrapper");
		if (SteamManager.s_instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		SteamManager.s_instance = this;
		if (SteamManager.s_EverInialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!Packsize.Test())
		{
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary(AppId_t.Invalid))
			{
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException arg)
		{
			Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + arg, this);
			Application.Quit();
			return;
		}
		this.m_bInitialized = SteamAPI.Init();
		if (!this.m_bInitialized)
		{
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
			Chat.LogError("Could not connect to Steam. Please restart Steam.", true);
			return;
		}
		SteamManager.s_EverInialized = true;
		this.Init();
	}

	// Token: 0x06001CA1 RID: 7329 RVA: 0x000C4F34 File Offset: 0x000C3134
	private void OnEnable()
	{
		if (SteamManager.s_instance == null)
		{
			SteamManager.s_instance = this;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
		{
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	// Token: 0x06001CA2 RID: 7330 RVA: 0x000C4F82 File Offset: 0x000C3182
	private void OnDestroy()
	{
		if (SteamManager.s_instance != this)
		{
			return;
		}
		SteamManager.s_instance = null;
		if (!this.m_bInitialized)
		{
			return;
		}
		this.StoreStats();
		SteamAPI.Shutdown();
	}

	// Token: 0x06001CA3 RID: 7331 RVA: 0x000C4FAC File Offset: 0x000C31AC
	private void Update()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x17000433 RID: 1075
	// (get) Token: 0x06001CA4 RID: 7332 RVA: 0x000C4FBC File Offset: 0x000C31BC
	public static bool IsOnline
	{
		get
		{
			return SteamUser.BLoggedOn();
		}
	}

	// Token: 0x17000434 RID: 1076
	// (get) Token: 0x06001CA5 RID: 7333 RVA: 0x000C4FC3 File Offset: 0x000C31C3
	// (set) Token: 0x06001CA6 RID: 7334 RVA: 0x000C4FCB File Offset: 0x000C31CB
	public int SubscribeCounter { get; private set; }

	// Token: 0x17000435 RID: 1077
	// (get) Token: 0x06001CA7 RID: 7335 RVA: 0x000C4FD4 File Offset: 0x000C31D4
	// (set) Token: 0x06001CA8 RID: 7336 RVA: 0x000C4FDC File Offset: 0x000C31DC
	public int SubscribeNumber { get; private set; }

	// Token: 0x17000436 RID: 1078
	// (get) Token: 0x06001CA9 RID: 7337 RVA: 0x000C4FE5 File Offset: 0x000C31E5
	// (set) Token: 0x06001CAA RID: 7338 RVA: 0x000C4FED File Offset: 0x000C31ED
	public bool bCheckingSubscribe { get; private set; }

	// Token: 0x17000437 RID: 1079
	// (get) Token: 0x06001CAB RID: 7339 RVA: 0x000C4FF6 File Offset: 0x000C31F6
	// (set) Token: 0x06001CAC RID: 7340 RVA: 0x000C4FFE File Offset: 0x000C31FE
	public bool bDownloading { get; private set; }

	// Token: 0x17000438 RID: 1080
	// (get) Token: 0x06001CAD RID: 7341 RVA: 0x000C5007 File Offset: 0x000C3207
	// (set) Token: 0x06001CAE RID: 7342 RVA: 0x000C500F File Offset: 0x000C320F
	public bool bDownloadingThumbnails { get; private set; }

	// Token: 0x06001CAF RID: 7343 RVA: 0x000C5018 File Offset: 0x000C3218
	private void Init()
	{
		SteamManager.bSteam = SteamApps.BIsSubscribed();
		if (!SteamManager.bSteam)
		{
			Chat.LogError("Connected to Steam, but is not subscribed.", true);
			return;
		}
		Chat.LogSystem("Connected to Steam.", Colour.GreyDark, false);
		Debug.Log("Steam working");
		SteamManager.bKickstarterPointer = SteamApps.BIsSubscribedApp(SteamManager.KickstarterPointer);
		SteamManager.bKickstarterGold = SteamApps.BIsSubscribedApp(SteamManager.KickstarterGold);
		if (SteamManager.bKickstarterPointer)
		{
			Chat.Log("Kickstarter Pointer.", Colour.Green, ChatMessageType.All, false);
		}
		if (SteamManager.bKickstarterGold)
		{
			Chat.Log("Kickstarter Gold.", Colour.Yellow, ChatMessageType.All, false);
		}
		SteamManager.UserSteamID = SteamUser.GetSteamID();
		SteamManager.StringSteamID = SteamManager.UserSteamID.ToString();
		SteamManager.SteamName = SteamFriends.GetPersonaName();
		NetworkSingleton<NetworkUI>.Instance.SetPlayerName(SteamManager.SteamName);
		this.AID = SteamManager.SteamIDtoAccountID(SteamManager.UserSteamID);
		this.OnRemoteStorageDownloadUGCResult = CallResult<RemoteStorageDownloadUGCResult_t>.Create(new CallResult<RemoteStorageDownloadUGCResult_t>.APIDispatchDelegate(this.CallResultWorkshopReadDownload));
		this.OnSteamUGCQueryCompleted = CallResult<SteamUGCQueryCompleted_t>.Create(new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(this.CallResultWorkshopQuery));
		this.OnSteamUGCDetailsResult = CallResult<SteamUGCRequestUGCDetailsResult_t>.Create(new CallResult<SteamUGCRequestUGCDetailsResult_t>.APIDispatchDelegate(this.CallResultWorkshopDetails));
		this.OnRemoteStorageFileShareResult = CallResult<RemoteStorageFileShareResult_t>.Create(new CallResult<RemoteStorageFileShareResult_t>.APIDispatchDelegate(this.CallResultRemoteStorageShare));
		this.OnRemoteStoragePublishFileResult = CallResult<RemoteStoragePublishFileResult_t>.Create(new CallResult<RemoteStoragePublishFileResult_t>.APIDispatchDelegate(this.CallResultUploadWorkshop));
		this.OnRemoteStorageUpdatePublishedFileResult = CallResult<RemoteStorageUpdatePublishedFileResult_t>.Create(new CallResult<RemoteStorageUpdatePublishedFileResult_t>.APIDispatchDelegate(this.CallResultUpdateWorkshop));
		this.m_RemoteStoragePublishedFileSubscribed = Callback<RemoteStoragePublishedFileSubscribed_t>.Create(new Callback<RemoteStoragePublishedFileSubscribed_t>.DispatchDelegate(this.CallbackFileSubscribed));
		this.OnRemoteStorageUnsubscribePublishedFileResult = CallResult<RemoteStorageUnsubscribePublishedFileResult_t>.Create(new CallResult<RemoteStorageUnsubscribePublishedFileResult_t>.APIDispatchDelegate(this.UnsubscribeResults));
		this.JoinClanChatRoomCompletionResult = CallResult<JoinClanChatRoomCompletionResult_t>.Create(new CallResult<JoinClanChatRoomCompletionResult_t>.APIDispatchDelegate(this.GroupChatJoinComplete));
		this.GameConnectedChatJoin = Callback<GameConnectedChatJoin_t>.Create(new Callback<GameConnectedChatJoin_t>.DispatchDelegate(this.GroupChatUserJoin));
		this.GameConnectedChatLeave = Callback<GameConnectedChatLeave_t>.Create(new Callback<GameConnectedChatLeave_t>.DispatchDelegate(this.GroudChatUserLeave));
		this.GameConnectedClanChatMsg = Callback<GameConnectedClanChatMsg_t>.Create(new Callback<GameConnectedClanChatMsg_t>.DispatchDelegate(this.GroupChatUserMsg));
		if (Utilities.IsLaunchOption("-nosubscription"))
		{
			Chat.LogSystem("Workshop subscriptions is disabled by -nosubscription launch option.", Colour.Blue, true);
		}
		else
		{
			this.CheckSubscribed();
		}
		this.m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate(this.CallbackUserStatsReceived));
		SteamUserStats.RequestCurrentStats();
		this.m_AvatarImageLoaded = Callback<AvatarImageLoaded_t>.Create(new Callback<AvatarImageLoaded_t>.DispatchDelegate(this.CallbackAvatarImageLoaded));
		SteamManager.ISOCountry = SteamUtils.GetIPCountry();
		if (string.IsNullOrEmpty(SteamManager.ISOCountry))
		{
			SteamManager.ISOCountry = "n/a";
		}
	}

	// Token: 0x06001CB0 RID: 7344 RVA: 0x000C5288 File Offset: 0x000C3488
	public static bool IsSubscribedApp(int AppId)
	{
		return SteamManager.bSteam && (SteamManager.OverrideIsSubscribedAppTrue || SteamManager.IsInPrivateBetaBranch() || SteamApps.BIsSubscribedApp(new AppId_t((uint)AppId)));
	}

	// Token: 0x06001CB1 RID: 7345 RVA: 0x000C52B0 File Offset: 0x000C34B0
	public static uint SubscribeDate(int AppId)
	{
		if (!SteamManager.bSteam)
		{
			return 0U;
		}
		return SteamApps.GetEarliestPurchaseUnixTime(new AppId_t((uint)AppId));
	}

	// Token: 0x06001CB2 RID: 7346 RVA: 0x000C52C8 File Offset: 0x000C34C8
	public static string GetBetaName()
	{
		if (!SteamManager.bSteam)
		{
			return "";
		}
		string result;
		SteamApps.GetCurrentBetaName(out result, 256);
		return result;
	}

	// Token: 0x06001CB3 RID: 7347 RVA: 0x000C52F0 File Offset: 0x000C34F0
	public static bool IsInPrivateBetaBranch()
	{
		return SteamManager.GetBetaName() == "beta";
	}

	// Token: 0x06001CB4 RID: 7348 RVA: 0x000C5301 File Offset: 0x000C3501
	public void GetCloudQuota(out ulong totalBytes, out ulong availableBytes)
	{
		SteamRemoteStorage.GetQuota(out totalBytes, out availableBytes);
	}

	// Token: 0x06001CB5 RID: 7349 RVA: 0x000C530C File Offset: 0x000C350C
	public Dictionary<string, int> GetCloudUGC()
	{
		int fileCount = SteamRemoteStorage.GetFileCount();
		Dictionary<string, int> dictionary = new Dictionary<string, int>(fileCount);
		for (int i = 0; i < fileCount; i++)
		{
			int value;
			string fileNameAndSize = SteamRemoteStorage.GetFileNameAndSize(i, out value);
			dictionary.Add(fileNameAndSize, value);
		}
		return dictionary;
	}

	// Token: 0x06001CB6 RID: 7350 RVA: 0x000C5348 File Offset: 0x000C3548
	private void DeleteAllCloudFiles()
	{
		foreach (KeyValuePair<string, int> keyValuePair in this.GetCloudUGC())
		{
			SteamRemoteStorage.FileDelete(keyValuePair.Key);
		}
	}

	// Token: 0x06001CB7 RID: 7351 RVA: 0x000C53A4 File Offset: 0x000C35A4
	public Dictionary<string, CloudInfo> GetCloudInfos()
	{
		Dictionary<string, CloudInfo> dictionary = new Dictionary<string, CloudInfo>();
		byte[] array = this.CloudToByte("CloudInfo.bson");
		if (array != null)
		{
			try
			{
				dictionary = Json.Load<Dictionary<string, CloudInfo>>(array, false);
				Dictionary<string, CloudInfo> dictionary2 = new Dictionary<string, CloudInfo>();
				foreach (KeyValuePair<string, CloudInfo> keyValuePair in dictionary)
				{
					CloudInfo value = keyValuePair.Value;
					value.Folder = SerializationScript.RemoveNewLineAndWhiteSpace(value.Folder);
					value.URL = SteamCloudURL.ConvertOldToNewCloudURL(value.URL);
					dictionary2.Add(keyValuePair.Key, value);
				}
				dictionary = dictionary2;
			}
			catch (Exception)
			{
				Chat.LogError("Error deserializing data GetCloudInfos.", true);
			}
		}
		return dictionary;
	}

	// Token: 0x06001CB8 RID: 7352 RVA: 0x000C5470 File Offset: 0x000C3670
	public List<string> GetCloudFolders()
	{
		List<string> list = new List<string>();
		byte[] array = this.CloudToByte("CloudFolder.bson");
		if (array != null)
		{
			try
			{
				list = Json.Load<List<string>>(array, true);
				for (int i = 0; i < list.Count; i++)
				{
					list[i] = SerializationScript.RemoveNewLineAndWhiteSpace(list[i]);
				}
			}
			catch (Exception)
			{
				Chat.LogError("Error deserializing data GetCloudFolders.", true);
			}
		}
		return list;
	}

	// Token: 0x06001CB9 RID: 7353 RVA: 0x000C54E0 File Offset: 0x000C36E0
	public static bool FolderNameEquals(string str1, string str2)
	{
		if (!string.IsNullOrEmpty(str1))
		{
			return string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
		}
		return string.IsNullOrEmpty(str2);
	}

	// Token: 0x06001CBA RID: 7354 RVA: 0x000C54F9 File Offset: 0x000C36F9
	public static bool IsSubFolderOf(string folder, string checkFolder)
	{
		return Utilities.IsSubPathOf(folder ?? "", checkFolder ?? "");
	}

	// Token: 0x06001CBB RID: 7355 RVA: 0x000C5514 File Offset: 0x000C3714
	public void AddCloudFolder(string name, bool quiet = false)
	{
		List<string> cloudFolders = this.GetCloudFolders();
		bool flag = true;
		using (List<string>.Enumerator enumerator = cloudFolders.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (SteamManager.FolderNameEquals(enumerator.Current, name))
				{
					flag = false;
					break;
				}
			}
		}
		if (flag)
		{
			cloudFolders.Add(name);
			this.WriteCloudFolder(cloudFolders);
			return;
		}
		Chat.LogError("Cloud folder '" + name + "' already exists.", true);
	}

	// Token: 0x06001CBC RID: 7356 RVA: 0x000C5598 File Offset: 0x000C3798
	public void RemoveCloudFolder(string name)
	{
		List<string> cloudFolders = this.GetCloudFolders();
		List<string> list = new List<string>();
		for (int i = cloudFolders.Count - 1; i >= 0; i--)
		{
			string text = cloudFolders[i];
			if (SteamManager.IsSubFolderOf(text, name))
			{
				cloudFolders.RemoveAt(i);
				list.Add(text);
			}
		}
		this.WriteCloudFolder(cloudFolders);
		Dictionary<string, CloudInfo> cloudInfos = this.GetCloudInfos();
		List<string> list2 = new List<string>();
		foreach (KeyValuePair<string, CloudInfo> keyValuePair in cloudInfos)
		{
			foreach (string str in list)
			{
				if (SteamManager.FolderNameEquals(keyValuePair.Value.Folder, str))
				{
					list2.Add(keyValuePair.Key);
				}
			}
		}
		foreach (string key in list2)
		{
			cloudInfos.Remove(key);
		}
		this.WriteCloudInfo(cloudInfos);
	}

	// Token: 0x06001CBD RID: 7357 RVA: 0x000C56E0 File Offset: 0x000C38E0
	public void MoveCloudFolder(string name, string newName)
	{
		if (SteamManager.IsSubFolderOf(newName, name))
		{
			Chat.LogError(string.Concat(new string[]
			{
				"Error moving cloud folder '",
				name,
				"' to '",
				newName,
				"', because it is a subfolder."
			}), true);
			return;
		}
		List<string> cloudFolders = this.GetCloudFolders();
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		for (int i = 0; i < cloudFolders.Count; i++)
		{
			string text = cloudFolders[i];
			if (SteamManager.IsSubFolderOf(text, name))
			{
				string value = newName + text.Substring(name.Length);
				cloudFolders[i] = value;
				dictionary.Add(text, value);
			}
		}
		this.WriteCloudFolder(cloudFolders);
		Dictionary<string, CloudInfo> cloudInfos = this.GetCloudInfos();
		Dictionary<string, CloudInfo> dictionary2 = new Dictionary<string, CloudInfo>();
		foreach (KeyValuePair<string, CloudInfo> keyValuePair in cloudInfos)
		{
			foreach (KeyValuePair<string, string> keyValuePair2 in dictionary)
			{
				if (SteamManager.FolderNameEquals(keyValuePair.Value.Folder, keyValuePair2.Key))
				{
					CloudInfo value2 = keyValuePair.Value;
					value2.Folder = keyValuePair2.Value;
					dictionary2.Add(keyValuePair.Key, value2);
				}
			}
		}
		foreach (KeyValuePair<string, CloudInfo> keyValuePair3 in dictionary2)
		{
			cloudInfos[keyValuePair3.Key] = keyValuePair3.Value;
		}
		this.WriteCloudInfo(cloudInfos);
	}

	// Token: 0x06001CBE RID: 7358 RVA: 0x000C58A4 File Offset: 0x000C3AA4
	public void MoveCloudInfo(string name, string folderName)
	{
		Dictionary<string, CloudInfo> cloudInfos = this.GetCloudInfos();
		if (cloudInfos.ContainsKey(name))
		{
			CloudInfo value = cloudInfos[name];
			value.Folder = folderName;
			cloudInfos[name] = value;
		}
		else
		{
			Chat.LogError("Error removing cloud info.", true);
		}
		this.WriteCloudInfo(cloudInfos);
	}

	// Token: 0x06001CBF RID: 7359 RVA: 0x000C58F0 File Offset: 0x000C3AF0
	private void AddCloudInfo(string name, CloudInfo cloudInfo)
	{
		Dictionary<string, CloudInfo> cloudInfos = this.GetCloudInfos();
		if (cloudInfos.ContainsKey(name))
		{
			cloudInfos[name] = cloudInfo;
		}
		else
		{
			cloudInfos.Add(name, cloudInfo);
		}
		this.WriteCloudInfo(cloudInfos);
	}

	// Token: 0x06001CC0 RID: 7360 RVA: 0x000C5928 File Offset: 0x000C3B28
	private void RemoveCloudInfo(string name)
	{
		Dictionary<string, CloudInfo> cloudInfos = this.GetCloudInfos();
		if (cloudInfos.ContainsKey(name))
		{
			cloudInfos.Remove(name);
		}
		else
		{
			Chat.LogError("Error removing cloud info.", true);
		}
		this.WriteCloudInfo(cloudInfos);
	}

	// Token: 0x06001CC1 RID: 7361 RVA: 0x000C5964 File Offset: 0x000C3B64
	private void WriteCloudInfo(Dictionary<string, CloudInfo> CloudInfos)
	{
		byte[] bson = Json.GetBson(CloudInfos);
		this.ByteToCloud("CloudInfo.bson", bson);
	}

	// Token: 0x06001CC2 RID: 7362 RVA: 0x000C5988 File Offset: 0x000C3B88
	private void WriteCloudFolder(List<string> folders)
	{
		byte[] bson = Json.GetBson(folders);
		this.ByteToCloud("CloudFolder.bson", bson);
	}

	// Token: 0x06001CC3 RID: 7363 RVA: 0x000C59AC File Offset: 0x000C3BAC
	public void DeleteFromCloud(string name)
	{
		if (SteamRemoteStorage.FileDelete(name))
		{
			Chat.LogSystem(name + " was successful deleted from Steam Cloud.", Colour.Green, false);
		}
		else
		{
			Chat.LogError("Failed to delete " + name + " from Steam Cloud.", true);
		}
		this.RemoveCloudInfo(name);
	}

	// Token: 0x06001CC4 RID: 7364 RVA: 0x000C59FC File Offset: 0x000C3BFC
	public void UploadToCloud(string name, byte[] data, string folder = null)
	{
		Wait.Condition(delegate
		{
			this.bUploadingToCloud = true;
			this.currentUploadToCloudName = name;
			this.currentUploadToCloudData = data;
			this.currentCloudFolder = folder;
			byte[] ba = this.sha1.ComputeHash(this.currentUploadToCloudData);
			this.currentSHA1 = Utilities.ByteArrayToString(ba);
			this.currentCloudName = this.currentSHA1 + "_" + name;
			Debug.Log("Cloud Name: " + this.currentCloudName);
			if (!this.FileWriteChunkShare(this.currentCloudName, data, new CallResult<RemoteStorageFileShareResult_t>.APIDispatchDelegate(this.ShareResultsUploadToCloud)))
			{
				Chat.LogError("Failed to write file to Steam Cloud: " + this.currentUploadToCloudName, true);
				EventManager.TriggerCloudUploadFinish(this.currentUploadToCloudName, "");
				this.ResetUploadToCloud();
			}
		}, () => !this.bUploadingToCloud, float.PositiveInfinity, null);
	}

	// Token: 0x06001CC5 RID: 7365 RVA: 0x000C5A50 File Offset: 0x000C3C50
	private bool FileWriteChunkShare(string fileName, byte[] data, CallResult<RemoteStorageFileShareResult_t>.APIDispatchDelegate CallBack)
	{
		UGCFileWriteStreamHandle_t writeHandle = SteamRemoteStorage.FileWriteStreamOpen(this.currentCloudName);
		int num = data.Length / 104857600;
		bool flag;
		if (num == 0)
		{
			flag = SteamRemoteStorage.FileWriteStreamWriteChunk(writeHandle, data, data.Length);
		}
		else
		{
			flag = SteamRemoteStorage.FileWriteStreamWriteChunk(writeHandle, data, 104857600);
			if (flag)
			{
				for (int i = 0; i < num; i++)
				{
					int num2 = i + 1;
					int num3 = 104857600 * num2;
					int num4 = (num2 < num) ? 104857600 : (data.Length - num3);
					Array.Copy(data, num3, data, 0, num4);
					Debug.Log(num3 + " : " + num4);
					flag = SteamRemoteStorage.FileWriteStreamWriteChunk(writeHandle, data, num4);
					if (!flag)
					{
						break;
					}
				}
			}
		}
		if (!SteamRemoteStorage.FileWriteStreamClose(writeHandle))
		{
			return false;
		}
		if (flag)
		{
			this.FileShare(this.currentCloudName, new CallResult<RemoteStorageFileShareResult_t>.APIDispatchDelegate(this.ShareResultsUploadToCloud));
		}
		return flag;
	}

	// Token: 0x06001CC6 RID: 7366 RVA: 0x000C5B20 File Offset: 0x000C3D20
	private bool FileWriteShare(string fileName, byte[] data, CallResult<RemoteStorageFileShareResult_t>.APIDispatchDelegate CallBack)
	{
		if (SteamRemoteStorage.FileWrite(fileName, data, data.Length))
		{
			this.FileShare(fileName, CallBack);
			return true;
		}
		return false;
	}

	// Token: 0x06001CC7 RID: 7367 RVA: 0x000C5B3C File Offset: 0x000C3D3C
	private void FileShare(string fileName, CallResult<RemoteStorageFileShareResult_t>.APIDispatchDelegate CallBack)
	{
		SteamAPICall_t hAPICall = SteamRemoteStorage.FileShare(fileName);
		this.OnRemoteStorageFileShareResult.Set(hAPICall, CallBack);
	}

	// Token: 0x06001CC8 RID: 7368 RVA: 0x000025B8 File Offset: 0x000007B8
	private void CallResultRemoteStorageShare(RemoteStorageFileShareResult_t pCallback, bool Failure)
	{
	}

	// Token: 0x06001CC9 RID: 7369 RVA: 0x000C5B60 File Offset: 0x000C3D60
	private void ShareResultsUploadToCloud(RemoteStorageFileShareResult_t CFSR, bool Failure)
	{
		if (SteamManager.CheckCallbackFailure("upload Cloud", CFSR.m_eResult, Failure, null, null))
		{
			EventManager.TriggerCloudUploadFinish(this.currentUploadToCloudName, "");
			this.ResetUploadToCloud();
			return;
		}
		Debug.Log(string.Concat(new object[]
		{
			CFSR.m_eResult,
			" : ",
			CFSR.m_hFile,
			" : ",
			CFSR.m_rgchFilename
		}));
		string url = string.Concat(new string[]
		{
			"https://steamusercontent-a.akamaihd.net/ugc/",
			CFSR.m_hFile.ToString(),
			"/",
			this.currentSHA1,
			"/"
		});
		this.AddCloudInfo(this.currentCloudName, new CloudInfo(this.currentUploadToCloudName, url, this.currentUploadToCloudData.Length, this.currentCloudFolder));
		Chat.LogSystem(string.Concat(new string[]
		{
			"Success uploading ",
			this.currentUploadToCloudName,
			" to Steam Cloud. (",
			Utilities.BytesToFileSizeString((long)this.currentUploadToCloudData.Length),
			")"
		}), Colour.Green, false);
		ulong num;
		ulong num2;
		this.GetCloudQuota(out num, out num2);
		if ((float)(num2 / num) < 0.1f)
		{
			Chat.LogError("Steam Cloud storage is starting to get low. " + Utilities.BytesToFileSizeString((long)num2) + " available. Delete old files in the Cloud Manager (Upload -> Cloud Manager).", true);
		}
		EventManager.TriggerCloudUploadFinish(this.currentUploadToCloudName, url);
		this.ResetUploadToCloud();
	}

	// Token: 0x06001CCA RID: 7370 RVA: 0x000C5CDE File Offset: 0x000C3EDE
	private void ResetUploadToCloud()
	{
		this.bUploadingToCloud = false;
		this.currentUploadToCloudName = "";
		this.currentCloudName = "";
		this.currentUploadToCloudData = null;
		this.currentSHA1 = "";
	}

	// Token: 0x06001CCB RID: 7371 RVA: 0x000C5D10 File Offset: 0x000C3F10
	public void GetAvatarFromSteamID(CSteamID steamID, Action<CSteamID, Texture2D> callback)
	{
		int largeFriendAvatar = SteamFriends.GetLargeFriendAvatar(steamID);
		if (largeFriendAvatar == -1)
		{
			if (!this.AvatarCallbacks.ContainsKey(steamID))
			{
				this.AvatarCallbacks.Add(steamID, callback);
			}
			return;
		}
		if (largeFriendAvatar == 0 || largeFriendAvatar == 3)
		{
			base.StartCoroutine(this.DelayGetAvatarFromSteamID(steamID, callback));
			return;
		}
		Debug.Log(string.Concat(new object[]
		{
			"GetAvatarFromSteamID: ",
			steamID,
			" : ",
			largeFriendAvatar
		}));
		callback(steamID, SteamManager.GetSteamImageAsTexture2D(largeFriendAvatar, SteamManager.IsDirectX(), SystemInfo.graphicsDeviceType != GraphicsDeviceType.Direct3D11, true));
	}

	// Token: 0x06001CCC RID: 7372 RVA: 0x000C5DAB File Offset: 0x000C3FAB
	private IEnumerator DelayGetAvatarFromSteamID(CSteamID steamID, Action<CSteamID, Texture2D> callback)
	{
		yield return new WaitForSeconds(0.5f);
		this.GetAvatarFromSteamID(steamID, callback);
		yield break;
	}

	// Token: 0x06001CCD RID: 7373 RVA: 0x000C5DC8 File Offset: 0x000C3FC8
	private void CallbackAvatarImageLoaded(AvatarImageLoaded_t pCallback)
	{
		Debug.Log(string.Concat(new object[]
		{
			"[",
			334,
			" - AvatarImageLoaded] - ",
			pCallback.m_steamID,
			" -- ",
			pCallback.m_iImage,
			" -- ",
			pCallback.m_iWide,
			" -- ",
			pCallback.m_iTall
		}));
		CSteamID steamID = pCallback.m_steamID;
		if (this.AvatarCallbacks.ContainsKey(steamID))
		{
			Action<CSteamID, Texture2D> callback = this.AvatarCallbacks[steamID];
			this.AvatarCallbacks.Remove(steamID);
			this.GetAvatarFromSteamID(steamID, callback);
			return;
		}
		Debug.Log("AvatarImageLoaded not in callbacks");
	}

	// Token: 0x06001CCE RID: 7374 RVA: 0x000C5E98 File Offset: 0x000C4098
	public static bool CheckCallbackFailure(string name, EResult result, bool failure, string additionalInfo = null, List<EResult> ignorePrint = null)
	{
		if (result != EResult.k_EResultOK && result != EResult.k_EResultAdministratorOK)
		{
			if (ignorePrint == null || !ignorePrint.Contains(result))
			{
				if (string.IsNullOrEmpty(additionalInfo))
				{
					Chat.LogError(string.Concat(new object[]
					{
						"Error Steam ",
						name,
						": ",
						result
					}), true);
				}
				else
				{
					Chat.LogError(string.Concat(new object[]
					{
						"Error Steam ",
						name,
						": ",
						result,
						" (",
						additionalInfo,
						")"
					}), true);
				}
			}
			return true;
		}
		if (failure)
		{
			if (ignorePrint == null || !ignorePrint.Contains(result))
			{
				if (string.IsNullOrEmpty(additionalInfo))
				{
					Chat.LogError("Error IO Steam " + name, true);
				}
				else
				{
					Chat.LogError(string.Concat(new string[]
					{
						"Error IO Steam ",
						name,
						" (",
						additionalInfo,
						")"
					}), true);
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x06001CCF RID: 7375 RVA: 0x000C5F9E File Offset: 0x000C419E
	private static string Hash(string input)
	{
		return SteamManager.Hash(Encoding.UTF8.GetBytes(input));
	}

	// Token: 0x06001CD0 RID: 7376 RVA: 0x000C5FB0 File Offset: 0x000C41B0
	private static string Hash(byte[] input)
	{
		string result;
		using (SHA1Managed sha1Managed = new SHA1Managed())
		{
			byte[] array = sha1Managed.ComputeHash(input);
			StringBuilder stringBuilder = new StringBuilder(array.Length * 2);
			foreach (byte b in array)
			{
				stringBuilder.Append(b.ToString("X2"));
			}
			result = stringBuilder.ToString();
		}
		return result;
	}

	// Token: 0x06001CD1 RID: 7377 RVA: 0x000C6024 File Offset: 0x000C4224
	public static CSteamID StringToSteamID(string ID)
	{
		return new CSteamID(Convert.ToUInt64(ID));
	}

	// Token: 0x06001CD2 RID: 7378 RVA: 0x000C6034 File Offset: 0x000C4234
	private byte[] CloudToByte(string fileName)
	{
		int fileSize = SteamRemoteStorage.GetFileSize(fileName);
		if (fileSize == 0)
		{
			return null;
		}
		byte[] array = new byte[fileSize];
		SteamRemoteStorage.FileRead(fileName, array, fileSize);
		return array;
	}

	// Token: 0x06001CD3 RID: 7379 RVA: 0x000C605E File Offset: 0x000C425E
	private bool ByteToCloud(string fileName, byte[] data)
	{
		return SteamRemoteStorage.FileWrite(fileName, data, data.Length);
	}

	// Token: 0x06001CD4 RID: 7380 RVA: 0x000C606C File Offset: 0x000C426C
	public static Texture2D GetSteamImageAsTexture2D(int iImage, bool flipTexture = true, bool mipmaps = true, bool compress = true)
	{
		Texture2D texture2D = null;
		uint num;
		uint num2;
		if (SteamUtils.GetImageSize(iImage, out num, out num2))
		{
			byte[] array = new byte[num * num2 * 4U];
			if (SteamUtils.GetImageRGBA(iImage, array, array.Length))
			{
				texture2D = new Texture2D((int)num, (int)num2, TextureFormat.RGBA32, false);
				texture2D.LoadRawTextureData(array);
				if (flipTexture || mipmaps)
				{
					Color32[] pixels = texture2D.GetPixels32();
					if (flipTexture)
					{
						Array.Reverse(pixels);
					}
					if (mipmaps)
					{
						UnityEngine.Object.Destroy(texture2D);
						texture2D = new Texture2D((int)num, (int)num2, TextureFormat.RGBA32, true);
					}
					texture2D.SetPixels32(pixels);
				}
				texture2D.Apply();
				if (compress)
				{
					texture2D.Compress(true);
				}
			}
		}
		return texture2D;
	}

	// Token: 0x06001CD5 RID: 7381 RVA: 0x000C60F4 File Offset: 0x000C42F4
	public static bool IsDirectX()
	{
		GraphicsDeviceType graphicsDeviceType = SystemInfo.graphicsDeviceType;
		if (graphicsDeviceType != GraphicsDeviceType.Direct3D11)
		{
		}
		return true;
	}

	// Token: 0x06001CD6 RID: 7382 RVA: 0x000C6111 File Offset: 0x000C4311
	public void SetPlayedWith(string PlayerId)
	{
		SteamFriends.SetPlayedWith(SteamManager.StringToSteamID(PlayerId));
	}

	// Token: 0x06001CD7 RID: 7383 RVA: 0x000C611E File Offset: 0x000C431E
	public bool IsFriend(CSteamID PlayerId)
	{
		return SteamFriends.HasFriend(PlayerId, EFriendFlags.k_EFriendFlagAll);
	}

	// Token: 0x06001CD8 RID: 7384 RVA: 0x000C612B File Offset: 0x000C432B
	public bool IsFriend(string PlayerId)
	{
		return this.IsFriend(SteamManager.StringToSteamID(PlayerId));
	}

	// Token: 0x06001CD9 RID: 7385 RVA: 0x000C613C File Offset: 0x000C433C
	public List<CSteamID> GetFriendLobbies()
	{
		List<CSteamID> list = new List<CSteamID>();
		int friendCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);
		for (int i = 0; i < friendCount; i++)
		{
			FriendGameInfo_t friendGameInfo_t;
			if (SteamFriends.GetFriendGamePlayed(SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate), out friendGameInfo_t) && friendGameInfo_t.m_gameID.IsSteamApp() && friendGameInfo_t.m_gameID.AppID() == SteamManager.TabletopSimulator && friendGameInfo_t.m_steamIDLobby.IsValid())
			{
				list.Add(friendGameInfo_t.m_steamIDLobby);
			}
		}
		return list;
	}

	// Token: 0x06001CDA RID: 7386 RVA: 0x000C61B3 File Offset: 0x000C43B3
	public bool IsOverlayEnable()
	{
		return SteamUtils.IsOverlayEnabled();
	}

	// Token: 0x06001CDB RID: 7387 RVA: 0x000C61BA File Offset: 0x000C43BA
	public void OpenURLOverlay(string URL)
	{
		SteamFriends.ActivateGameOverlayToWebPage(URL, EActivateGameOverlayToWebPageMode.k_EActivateGameOverlayToWebPageMode_Default);
	}

	// Token: 0x06001CDC RID: 7388 RVA: 0x000C61C4 File Offset: 0x000C43C4
	private static AccountID_t SteamIDtoAccountID(CSteamID SID)
	{
		string text = SID.ToString();
		return new AccountID_t((uint)(ulong.Parse(text.Substring(3, text.Length - 3)) - 61197960265728UL));
	}

	// Token: 0x06001CDD RID: 7389 RVA: 0x000C6204 File Offset: 0x000C4404
	private void CheckSubscribed()
	{
		if (this.bCheckingSubscribe || this.bDownloading)
		{
			return;
		}
		this.bCheckingSubscribe = true;
		this.ResetSubscribedCheck();
		this.SubscribeNumber = (int)SteamUGC.GetNumSubscribedItems();
		Chat.LogSystem(this.SubscribeNumber + " Subscribed Workshop Mods.", Color.cyan, false);
		EventManager.TriggerUnityAnalytic("Subscribed_Workshop", "Number", this.SubscribeNumber, 1);
		this.QuerySubscribe();
	}

	// Token: 0x06001CDE RID: 7390 RVA: 0x000C627B File Offset: 0x000C447B
	private void ResetSubscribedCheck()
	{
		this.SubscribeCounter = 0;
		this.Page = 0U;
	}

	// Token: 0x06001CDF RID: 7391 RVA: 0x000C628C File Offset: 0x000C448C
	private void QuerySubscribe()
	{
		this.Page += 1U;
		Chat.LogSystem("Querying Workshop..." + this.Page, false);
		SteamAPICall_t hAPICall = SteamUGC.SendQueryUGCRequest(SteamUGC.CreateQueryUserUGCRequest(this.AID, EUserUGCList.k_EUserUGCList_Subscribed, EUGCMatchingUGCType.k_EUGCMatchingUGCType_UsableInGame, EUserUGCListSortOrder.k_EUserUGCListSortOrder_LastUpdatedDesc, SteamUtils.GetAppID(), SteamUtils.GetAppID(), this.Page));
		this.OnSteamUGCQueryCompleted.Set(hAPICall, new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(this.CallResultWorkshopQuery));
	}

	// Token: 0x06001CE0 RID: 7392 RVA: 0x000C6300 File Offset: 0x000C4500
	private void CallResultWorkshopQuery(SteamUGCQueryCompleted_t UGCQC, bool Failure)
	{
		SteamManager.CheckCallbackFailure("query Workshop", UGCQC.m_eResult, Failure, null, null);
		for (uint num = 0U; num < UGCQC.m_unNumResultsReturned; num += 1U)
		{
			SteamUGCDetails_t details;
			if (SteamUGC.GetQueryUGCResult(UGCQC.m_handle, num, out details))
			{
				int subscribeCounter = this.SubscribeCounter;
				this.SubscribeCounter = subscribeCounter + 1;
				this.CheckUpdateMod(details);
			}
		}
		SteamUGC.ReleaseQueryUGCRequest(UGCQC.m_handle);
		if (UGCQC.m_unNumResultsReturned >= 50U)
		{
			this.QuerySubscribe();
			return;
		}
		this.bCheckingSubscribe = false;
		if (this.PendingModDownloads.Count == 0)
		{
			this.FinishModDownloads();
		}
	}

	// Token: 0x06001CE1 RID: 7393 RVA: 0x000C6390 File Offset: 0x000C4590
	private static bool HasTag(string tags, string tag)
	{
		int num = tags.IndexOf(tag, StringComparison.OrdinalIgnoreCase);
		if (num >= 0)
		{
			int num2 = num + tag.Length;
			return tags.Length <= num2 + 1 || tags[num2] == ',';
		}
		return false;
	}

	// Token: 0x06001CE2 RID: 7394 RVA: 0x000C63D0 File Offset: 0x000C45D0
	private void CheckUpdateMod(SteamUGCDetails_t details)
	{
		if (details.m_nCreatorAppID != SteamUtils.GetAppID())
		{
			return;
		}
		uint rtimeUpdated = details.m_rtimeUpdated;
		uint num = 0U;
		SteamManager.ModInfo.ModType type = SteamManager.ModInfo.ModType.Save;
		if (SteamManager.HasTag(details.m_rgchTags, "Translation"))
		{
			type = SteamManager.ModInfo.ModType.Translation;
			Debug.Log(details.m_rgchTags);
		}
		string filePath;
		if (SerializationScript.WorkshopFileExists(details.m_nPublishedFileId.ToString(), out filePath))
		{
			num = SerializationScript.GetFileTime(filePath);
		}
		string thumbnailPath = SerializationScript.GetThumbnailPath(filePath);
		bool flag = rtimeUpdated > num;
		if (details.m_ulSteamIDOwner == (ulong)SteamManager.UserSteamID)
		{
			SteamManager.ModInfo item = new SteamManager.ModInfo(details, type);
			this.YourMods.TryAddUnique(item);
		}
		if (flag)
		{
			Debug.Log("Update mod: " + details.m_rgchTitle);
			SteamManager.ModInfo mi = new SteamManager.ModInfo(details, type);
			this.CheckDownloadMod(mi);
		}
		if (flag || (!string.IsNullOrEmpty(thumbnailPath) && !File.Exists(thumbnailPath)))
		{
			Debug.Log("Update mod thumbnail: " + details.m_rgchTitle);
			SteamManager.ModInfo item2 = new SteamManager.ModInfo(details, SteamManager.ModInfo.ModType.Thumbnail);
			this.PendingModThumbnailDownloads.TryAddUnique(item2);
		}
	}

	// Token: 0x06001CE3 RID: 7395 RVA: 0x000C64D8 File Offset: 0x000C46D8
	private void CheckDownloadMod(SteamManager.ModInfo MI)
	{
		if (this.PendingModDownloads.TryAddUnique(MI) && !this.bDownloading)
		{
			this.bDownloading = true;
			SteamAPICall_t hAPICall = SteamRemoteStorage.UGCDownload(this.PendingModDownloads[0].Handle, 1U);
			this.OnRemoteStorageDownloadUGCResult.Set(hAPICall, new CallResult<RemoteStorageDownloadUGCResult_t>.APIDispatchDelegate(this.CallResultWorkshopReadDownload));
		}
	}

	// Token: 0x06001CE4 RID: 7396 RVA: 0x000C6538 File Offset: 0x000C4738
	private void FinishModDownloads()
	{
		if (this.YourMods.Count > Stats.INT_UPLOAD_STEAM_WORKSHOP_ITEMS)
		{
			Stats.INT_UPLOAD_STEAM_WORKSHOP_ITEMS = this.YourMods.Count;
		}
		this.ResetSubscribedCheck();
		Chat.LogSystem("Workshop mods are up-to-date.", Colour.Green, false);
		this.CheckThumbnailDownload();
		EventManager.TriggerWorkshopUpToDate();
	}

	// Token: 0x06001CE5 RID: 7397 RVA: 0x000C6590 File Offset: 0x000C4790
	private void CheckThumbnailDownload()
	{
		if (this.PendingModThumbnailDownloads.Count > 0)
		{
			if (!this.bDownloadingThumbnails)
			{
				Chat.LogSystem(this.PendingModThumbnailDownloads.Count + " workshop thumbnails to download.", Colour.Teal, false);
				if (this.PendingModThumbnailDownloads.Count > 5)
				{
					Chat.LogWarning("Downloading workshop thumbnails there might be some stuttering during this time.", true);
				}
			}
			this.bDownloadingThumbnails = true;
			SteamAPICall_t hAPICall = SteamRemoteStorage.UGCDownload(this.PendingModThumbnailDownloads[0].Handle, 1U);
			this.OnRemoteStorageDownloadUGCResult.Set(hAPICall, new CallResult<RemoteStorageDownloadUGCResult_t>.APIDispatchDelegate(this.CallResultWorkshopReadDownload));
			return;
		}
		Chat.LogSystem("Workshop thumbnails are up-to-date.", Colour.Green, false);
		this.bDownloadingThumbnails = false;
	}

	// Token: 0x06001CE6 RID: 7398 RVA: 0x000C6650 File Offset: 0x000C4850
	private void CallResultWorkshopReadDownload(RemoteStorageDownloadUGCResult_t result, bool Failure)
	{
		SteamManager.CheckCallbackFailure("read download", result.m_eResult, Failure, result.m_pchFileName, null);
		foreach (SteamManager.ModInfo modInfo in this.PendingModThumbnailDownloads)
		{
			if (modInfo.Handle == result.m_hFile)
			{
				if (result.m_nSizeInBytes > 0)
				{
					try
					{
						byte[] array = new byte[result.m_nSizeInBytes];
						SteamRemoteStorage.UGCRead(result.m_hFile, array, result.m_nSizeInBytes, 0U, EUGCReadAction.k_EUGCRead_ContinueReadingUntilFinished);
						Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false);
						texture2D.LoadImage(array);
						if (texture2D.width > 256 || texture2D.height > 256)
						{
							texture2D.ResizePro(256, 256);
						}
						byte[] bytes = texture2D.EncodeToPNG();
						string filePath;
						SerializationScript.WorkshopFileExists(modInfo.Id.ToString(), out filePath);
						File.WriteAllBytes(SerializationScript.GetThumbnailPath(filePath), bytes);
						UnityEngine.Object.Destroy(texture2D);
						goto IL_132;
					}
					catch (Exception e)
					{
						Chat.LogException("reading Thumbnail data", e, true, false);
						goto IL_132;
					}
				}
				Chat.LogError(string.Format("Thumbnail {0} data sent by Steam is empty.", modInfo.Title), true);
				if (result.m_eResult == EResult.k_EResultFileNotFound)
				{
					this.UpsubscribeDialogForBrokenMod(modInfo);
				}
				IL_132:
				this.PendingModThumbnailDownloads.Remove(modInfo);
				this.CheckThumbnailDownload();
				return;
			}
		}
		int num = -1;
		SteamManager.ModInfo modInfo2 = default(SteamManager.ModInfo);
		for (int i = 0; i < this.PendingModDownloads.Count; i++)
		{
			SteamManager.ModInfo modInfo3 = this.PendingModDownloads[i];
			if (modInfo3.Handle == result.m_hFile)
			{
				num = i;
				modInfo2 = modInfo3;
				break;
			}
		}
		if (num == -1)
		{
			Chat.LogError("Workshop file id not matching!", true);
		}
		else
		{
			if (result.m_nSizeInBytes > 0)
			{
				try
				{
					byte[] array2 = new byte[result.m_nSizeInBytes];
					SteamRemoteStorage.UGCRead(result.m_hFile, array2, result.m_nSizeInBytes, 0U, EUGCReadAction.k_EUGCRead_ContinueReadingUntilFinished);
					string fileName = modInfo2.Id.ToString();
					SerializationScript.Save(modInfo2.Type, array2, fileName, modInfo2.Title);
					Chat.LogSystem(modInfo2.Title + " has been updated.", Colour.Blue, false);
					goto IL_284;
				}
				catch (Exception e2)
				{
					Chat.LogException("reading Workshop data", e2, true, false);
					goto IL_284;
				}
			}
			Chat.LogError(string.Format("Workshop {0} data sent by Steam is empty.", modInfo2.Title), true);
			if (result.m_eResult == EResult.k_EResultFileNotFound)
			{
				this.UpsubscribeDialogForBrokenMod(modInfo2);
			}
		}
		IL_284:
		if (num + 1 < this.PendingModDownloads.Count)
		{
			SteamAPICall_t hAPICall = SteamRemoteStorage.UGCDownload(this.PendingModDownloads[num + 1].Handle, 1U);
			this.OnRemoteStorageDownloadUGCResult.Set(hAPICall, new CallResult<RemoteStorageDownloadUGCResult_t>.APIDispatchDelegate(this.CallResultWorkshopReadDownload));
			this.PendingModDownloads.RemoveAt(num);
			return;
		}
		this.PendingModDownloads.Clear();
		this.bDownloading = false;
		if (!this.bCheckingSubscribe)
		{
			this.FinishModDownloads();
		}
	}

	// Token: 0x06001CE7 RID: 7399 RVA: 0x000C69A4 File Offset: 0x000C4BA4
	private void UpsubscribeDialogForBrokenMod(SteamManager.ModInfo modInfo)
	{
		UIConfirmUnsubscribe componentInChildren = NetworkSingleton<NetworkUI>.Instance.GUIConfirmUnsubscribe.GetComponentInChildren<UIConfirmUnsubscribe>();
		componentInChildren.WorkshopSteamTitle = modInfo.Title;
		componentInChildren.WorkshopSteamID = modInfo.Id.m_PublishedFileId;
		componentInChildren.gameObject.SetActive(true);
	}

	// Token: 0x06001CE8 RID: 7400 RVA: 0x000C69E0 File Offset: 0x000C4BE0
	public void UploadToWorkshop(SteamManager.ModUploadInfo modUploadInfo)
	{
		this.modNewUploadInfo = modUploadInfo;
		Chat.LogSystem("Beginning upload to Workshop.", Colour.Blue, true);
		Singleton<CustomLoadingManager>.Instance.Texture.Load(this.modNewUploadInfo.ThumbnailURL, new Action<CustomTextureContainer>(this.UploadPreviewImage), false, false, false, false, true, true, 8192, CustomLoadingManager.LoadType.Auto);
	}

	// Token: 0x06001CE9 RID: 7401 RVA: 0x000C6A3C File Offset: 0x000C4C3C
	private void UploadPreviewImage(CustomTextureContainer customTextureContainer)
	{
		if (customTextureContainer.texture == null)
		{
			Chat.LogError("Thumbnail image failed to load", true);
			return;
		}
		Texture2D texture2D = customTextureContainer.texture as Texture2D;
		if (texture2D)
		{
			byte[] data = texture2D.EncodeToPNG();
			if (!this.FileWriteShare("WorkshopImageUpload.png", data, new CallResult<RemoteStorageFileShareResult_t>.APIDispatchDelegate(this.ShareImageResults)))
			{
				Chat.LogError("Error writing thumbnail image to Steam Cloud.", true);
			}
		}
		else
		{
			Chat.LogError("Error thumbnail isn't a Texture2D.", true);
		}
		Singleton<CustomLoadingManager>.Instance.Texture.Cleanup(customTextureContainer.nonCodeStrippedURL, new Action<CustomTextureContainer>(this.UploadPreviewImage), true);
	}

	// Token: 0x06001CEA RID: 7402 RVA: 0x000C6AD2 File Offset: 0x000C4CD2
	private void ShareImageResults(RemoteStorageFileShareResult_t CFSR, bool Failure)
	{
		if (SteamManager.CheckCallbackFailure("share thumbnail", CFSR.m_eResult, Failure, null, null))
		{
			return;
		}
		Chat.LogSystem("Uploaded Workshop thumbnail.", Colour.Green, false);
		this.UploadMod();
	}

	// Token: 0x06001CEB RID: 7403 RVA: 0x000C6B05 File Offset: 0x000C4D05
	private void UploadMod()
	{
		if (!this.FileWriteShare("WorkshopUpload", this.modNewUploadInfo.Data, new CallResult<RemoteStorageFileShareResult_t>.APIDispatchDelegate(this.ShareWorkshopResults)))
		{
			Chat.LogError("Error writing mod to Steam Cloud.", true);
		}
	}

	// Token: 0x06001CEC RID: 7404 RVA: 0x000C6B38 File Offset: 0x000C4D38
	private void ShareWorkshopResults(RemoteStorageFileShareResult_t CFSR, bool Failure)
	{
		if (SteamManager.CheckCallbackFailure("share mod", CFSR.m_eResult, Failure, null, null))
		{
			return;
		}
		Chat.LogSystem("Shared Workshop mod.", Colour.Green, false);
		SteamAPICall_t hAPICall = SteamRemoteStorage.PublishWorkshopFile("WorkshopUpload", "WorkshopImageUpload.png", SteamUtils.GetAppID(), this.modNewUploadInfo.Title, this.modNewUploadInfo.Desc, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPrivate, this.modNewUploadInfo.Tags, EWorkshopFileType.k_EWorkshopFileTypeFirst);
		this.OnRemoteStoragePublishFileResult.Set(hAPICall, new CallResult<RemoteStoragePublishFileResult_t>.APIDispatchDelegate(this.CallResultUploadWorkshop));
	}

	// Token: 0x06001CED RID: 7405 RVA: 0x000C6BC0 File Offset: 0x000C4DC0
	private void CallResultUploadWorkshop(RemoteStoragePublishFileResult_t CPFR, bool Failure)
	{
		if (SteamManager.CheckCallbackFailure("publish to Workshop", CPFR.m_eResult, Failure, null, null))
		{
			return;
		}
		Chat.LogSystem("Upload Workshop mod successful. Id = " + CPFR.m_nPublishedFileId, Colour.Green, true);
		Stats.INT_UPLOAD_STEAM_WORKSHOP_ITEMS++;
		NetworkSingleton<NetworkUI>.Instance.GUISetConfirmWorkshop(CPFR.m_nPublishedFileId.ToString());
		this.mySubscribedMod = CPFR.m_nPublishedFileId;
		SteamRemoteStorage.SubscribePublishedFile(this.mySubscribedMod);
	}

	// Token: 0x06001CEE RID: 7406 RVA: 0x000C6C48 File Offset: 0x000C4E48
	public void UpdateToWorkshop(SteamManager.ModUploadInfo modUploadInfo)
	{
		this.modUpdateUploadInfo = modUploadInfo;
		Chat.LogSystem("Beginning upload to Workshop.", Colour.Blue, true);
		if (!string.IsNullOrEmpty(this.modUpdateUploadInfo.ThumbnailURL))
		{
			Singleton<CustomLoadingManager>.Instance.Texture.Load(this.modUpdateUploadInfo.ThumbnailURL, new Action<CustomTextureContainer>(this.UpdatePreviewImage), false, false, false, false, true, true, 8192, CustomLoadingManager.LoadType.Auto);
			return;
		}
		this.UpdateMod();
	}

	// Token: 0x06001CEF RID: 7407 RVA: 0x000C6CBC File Offset: 0x000C4EBC
	private void UpdatePreviewImage(CustomTextureContainer customTextureContainer)
	{
		if (customTextureContainer.texture == null)
		{
			Chat.LogError("Thumbnail image failed to load", true);
			return;
		}
		Texture2D texture2D = customTextureContainer.texture as Texture2D;
		if (texture2D)
		{
			byte[] data = texture2D.EncodeToPNG();
			if (!this.FileWriteShare("WorkshopImageUpload.png", data, new CallResult<RemoteStorageFileShareResult_t>.APIDispatchDelegate(this.ShareImageResultsUpdate)))
			{
				Chat.LogError("Failed to write thumbnail to Steam Cloud.", true);
			}
		}
		else
		{
			Chat.LogError("Error thumbnail isn't a Texture2D.", true);
		}
		Singleton<CustomLoadingManager>.Instance.Texture.Cleanup(customTextureContainer.nonCodeStrippedURL, new Action<CustomTextureContainer>(this.UpdatePreviewImage), true);
	}

	// Token: 0x06001CF0 RID: 7408 RVA: 0x000C6D54 File Offset: 0x000C4F54
	private void ShareImageResultsUpdate(RemoteStorageFileShareResult_t CFSR, bool Failure)
	{
		if (SteamManager.CheckCallbackFailure("share thumbnail", CFSR.m_eResult, Failure, null, null))
		{
			return;
		}
		Debug.Log(string.Concat(new object[]
		{
			"ShareImageResultsUpdate",
			CFSR.m_eResult,
			" : ",
			CFSR.m_hFile,
			" : ",
			Failure.ToString()
		}));
		PublishedFileUpdateHandle_t updateHandle = SteamRemoteStorage.CreatePublishedFileUpdateRequest(this.modUpdateUploadInfo.PublisherId);
		if (SteamRemoteStorage.UpdatePublishedFilePreviewFile(updateHandle, "WorkshopImageUpload.png"))
		{
			SteamAPICall_t hAPICall = SteamRemoteStorage.CommitPublishedFileUpdate(updateHandle);
			this.OnRemoteStorageUpdatePublishedFileResult.Set(hAPICall, new CallResult<RemoteStorageUpdatePublishedFileResult_t>.APIDispatchDelegate(this.CallResultUpdateThumbnail));
			return;
		}
		Debug.LogError("Failed to update thumbnail.");
	}

	// Token: 0x06001CF1 RID: 7409 RVA: 0x000C6E0D File Offset: 0x000C500D
	private void CallResultUpdateThumbnail(RemoteStorageUpdatePublishedFileResult_t result, bool Failure)
	{
		if (SteamManager.CheckCallbackFailure("thumbnail update", result.m_eResult, Failure, null, null))
		{
			return;
		}
		Chat.LogSystem("Updated Workshop thumbnail.", Colour.Green, false);
		this.UpdateMod();
	}

	// Token: 0x06001CF2 RID: 7410 RVA: 0x000C6E40 File Offset: 0x000C5040
	private void UpdateMod()
	{
		if (!this.FileWriteShare("WorkshopUpload", this.modUpdateUploadInfo.Data, new CallResult<RemoteStorageFileShareResult_t>.APIDispatchDelegate(this.ShareResultsUpdate)))
		{
			Debug.LogError("Failed to write mod to Steam Cloud.");
		}
	}

	// Token: 0x06001CF3 RID: 7411 RVA: 0x000C6E70 File Offset: 0x000C5070
	private void ShareResultsUpdate(RemoteStorageFileShareResult_t CFSR, bool Failure)
	{
		if (SteamManager.CheckCallbackFailure("share mod", CFSR.m_eResult, Failure, null, null))
		{
			return;
		}
		Chat.LogSystem("Shared Workshop mod.", Colour.Green, false);
		this.WorkshopUpdate(SteamRemoteStorage.CreatePublishedFileUpdateRequest(this.modUpdateUploadInfo.PublisherId));
	}

	// Token: 0x06001CF4 RID: 7412 RVA: 0x000C6EC0 File Offset: 0x000C50C0
	private void WorkshopUpdate(PublishedFileUpdateHandle_t PFUH)
	{
		if (SteamRemoteStorage.UpdatePublishedFileFile(PFUH, "WorkshopUpload"))
		{
			if (this.modUpdateUploadInfo.Tags.Count > 0 && !SteamRemoteStorage.UpdatePublishedFileTags(PFUH, this.modUpdateUploadInfo.Tags))
			{
				Chat.LogError("Update tags failed.", true);
			}
			SteamAPICall_t hAPICall = SteamRemoteStorage.CommitPublishedFileUpdate(PFUH);
			this.OnRemoteStorageUpdatePublishedFileResult.Set(hAPICall, new CallResult<RemoteStorageUpdatePublishedFileResult_t>.APIDispatchDelegate(this.CallResultUpdateWorkshop));
			return;
		}
		Chat.LogError("Update Workshop has failed.", true);
	}

	// Token: 0x06001CF5 RID: 7413 RVA: 0x000C6F38 File Offset: 0x000C5138
	private void CallResultUpdateWorkshop(RemoteStorageUpdatePublishedFileResult_t result, bool Failure)
	{
		if (SteamManager.CheckCallbackFailure("Workshop update", result.m_eResult, Failure, null, null))
		{
			return;
		}
		Chat.LogSystem("Update Workshop mod successful. Id = " + result.m_nPublishedFileId, Colour.Green, true);
		this.mySubscribedMod = result.m_nPublishedFileId;
		SteamRemoteStorage.SubscribePublishedFile(this.mySubscribedMod);
	}

	// Token: 0x06001CF6 RID: 7414 RVA: 0x000C6F98 File Offset: 0x000C5198
	private void CallbackFileSubscribed(RemoteStoragePublishedFileSubscribed_t CPFS)
	{
		if (CPFS.m_nAppID != SteamUtils.GetAppID())
		{
			return;
		}
		if (CPFS.m_nPublishedFileId == this.mySubscribedMod)
		{
			Chat.LogSystem("Downloading your mod.", Colour.Blue, false);
		}
		else
		{
			Chat.LogSystem("Downloading your new Workshop subscription.", Colour.Blue, false);
		}
		SteamAPICall_t hAPICall = SteamUGC.RequestUGCDetails(CPFS.m_nPublishedFileId, 15U);
		this.OnSteamUGCDetailsResult.Set(hAPICall, new CallResult<SteamUGCRequestUGCDetailsResult_t>.APIDispatchDelegate(this.CallResultWorkshopDetails));
	}

	// Token: 0x06001CF7 RID: 7415 RVA: 0x000C701D File Offset: 0x000C521D
	private void CallResultWorkshopDetails(SteamUGCRequestUGCDetailsResult_t result, bool failure)
	{
		if (failure)
		{
			return;
		}
		this.CheckUpdateMod(result.m_details);
	}

	// Token: 0x06001CF8 RID: 7416 RVA: 0x000C7030 File Offset: 0x000C5230
	public void UnsubscribeFromId(ulong WorkshopId)
	{
		try
		{
			SteamAPICall_t hAPICall = SteamRemoteStorage.UnsubscribePublishedFile(new PublishedFileId_t(WorkshopId));
			this.OnRemoteStorageUnsubscribePublishedFileResult.Set(hAPICall, new CallResult<RemoteStorageUnsubscribePublishedFileResult_t>.APIDispatchDelegate(this.UnsubscribeResults));
		}
		catch (Exception)
		{
			Chat.LogError("Failed to unsubscribed from Workshop mod. Id = " + WorkshopId, true);
		}
	}

	// Token: 0x06001CF9 RID: 7417 RVA: 0x000C708C File Offset: 0x000C528C
	private void UnsubscribeResults(RemoteStorageUnsubscribePublishedFileResult_t CUPFR, bool Failure)
	{
		if (SteamManager.CheckCallbackFailure("unsubscribe from Workshop mod", CUPFR.m_eResult, Failure, null, null))
		{
			return;
		}
		Chat.LogSystem("Unsubscribed from Workshop mod. Id = " + CUPFR.m_nPublishedFileId, Colour.Green, false);
	}

	// Token: 0x06001CFA RID: 7418 RVA: 0x000C70CC File Offset: 0x000C52CC
	private void CallbackUserStatsReceived(UserStatsReceived_t USR)
	{
		if (USR.m_eResult != EResult.k_EResultOK)
		{
			Chat.LogError("Error fetching Steam Stats: " + USR.m_eResult, true);
			return;
		}
		this.bStatsInitialized = true;
		int num;
		if (SteamUserStats.GetStat("STAT_CHANGE_COLOR", out num))
		{
			Stats.INT_CHANGE_COLOR_TIMES = num;
		}
		if (SteamUserStats.GetStat("STAT_FLIP_TABLE", out num))
		{
			Stats.INT_FLIP_TABLE_TIMES = num;
		}
		if (SteamUserStats.GetStat("STAT_LOCK_ITEMS", out num))
		{
			Stats.INT_LOCK_ITEMS = num;
		}
		if (SteamUserStats.GetStat("STAT_SAVE_ITEMS_TO_CHEST", out num))
		{
			Stats.INT_SAVE_ITEMS_TO_CHEST = num;
		}
		if (SteamUserStats.GetStat("STAT_SPAWN_CHIPS", out num))
		{
			Stats.INT_SPAWN_CHIPS = num;
		}
		if (SteamUserStats.GetStat("STAT_TINT_OBJECTS", out num))
		{
			Stats.INT_TINT_OBJECTS = num;
		}
		if (SteamUserStats.GetStat("STAT_UPLOAD_STEAM_WORKSHOP_ITEMS", out num))
		{
			Stats.INT_UPLOAD_STEAM_WORKSHOP_ITEMS = num;
		}
		if (SteamUserStats.GetStat("STAT_HOURS_PLAYED", out num))
		{
			Stats.INT_HOURS_PLAYED = num;
		}
	}

	// Token: 0x06001CFB RID: 7419 RVA: 0x000C71A5 File Offset: 0x000C53A5
	public void SetAchievement(string AchievementName)
	{
		if (SteamManager.bSteam)
		{
			if (SteamUserStats.SetAchievement(AchievementName))
			{
				this.StoreStats();
				return;
			}
			Debug.LogError("Error setting achievment: " + AchievementName);
		}
	}

	// Token: 0x06001CFC RID: 7420 RVA: 0x000C71CD File Offset: 0x000C53CD
	public void SetStat(string StatName, int Value)
	{
		if (SteamManager.bSteam && this.bStatsInitialized && !SteamUserStats.SetStat(StatName, Value))
		{
			Debug.LogError("Error setting stat: " + StatName);
		}
	}

	// Token: 0x06001CFD RID: 7421 RVA: 0x000C71F7 File Offset: 0x000C53F7
	public void StoreStats()
	{
		if (SteamManager.bSteam && !SteamUserStats.StoreStats())
		{
			Debug.LogError("Error storing stat.");
		}
	}

	// Token: 0x06001CFE RID: 7422 RVA: 0x000C7211 File Offset: 0x000C5411
	public void JoinGlobalChat()
	{
		Debug.Log("Start join global chat");
		this.JoinChat(this.TabletopSimulatorGroupChatID);
	}

	// Token: 0x06001CFF RID: 7423 RVA: 0x000C722C File Offset: 0x000C542C
	public void JoinChat(CSteamID steamID)
	{
		SteamAPICall_t hAPICall = SteamFriends.JoinClanChatRoom(steamID);
		this.JoinClanChatRoomCompletionResult.Set(hAPICall, null);
	}

	// Token: 0x06001D00 RID: 7424 RVA: 0x000C7250 File Offset: 0x000C5450
	private void GroupChatJoinComplete(JoinClanChatRoomCompletionResult_t joinClanChatRoomCompletionResult_T, bool Failure)
	{
		Debug.Log(string.Concat(new object[]
		{
			"GroupChatJoinComplete: ",
			joinClanChatRoomCompletionResult_T.m_steamIDClanChat,
			" : ",
			joinClanChatRoomCompletionResult_T.m_eChatRoomEnterResponse
		}));
		if (joinClanChatRoomCompletionResult_T.m_eChatRoomEnterResponse != EChatRoomEnterResponse.k_EChatRoomEnterResponseSuccess || Failure)
		{
			Chat.LogError("Failed to join group chat: " + joinClanChatRoomCompletionResult_T.m_eChatRoomEnterResponse, true);
			return;
		}
		int clanCount = SteamFriends.GetClanCount();
		for (int i = 0; i < clanCount; i++)
		{
			CSteamID clanByIndex = SteamFriends.GetClanByIndex(i);
			string clanName = SteamFriends.GetClanName(clanByIndex);
			string clanTag = SteamFriends.GetClanTag(clanByIndex);
			Debug.Log(string.Concat(new object[]
			{
				clanName,
				" : ",
				clanTag,
				" : ",
				clanByIndex
			}));
		}
		Debug.Log(SteamFriends.GetClanChatMemberCount(this.TabletopSimulatorGroupChatID));
		SteamFriends.SendClanChatMessage(this.TabletopSimulatorGroupChatID, "Hello world!");
	}

	// Token: 0x06001D01 RID: 7425 RVA: 0x000C7345 File Offset: 0x000C5545
	private void GroupChatUserJoin(GameConnectedChatJoin_t gameConnectedChatJoin_T)
	{
		Debug.Log("GroupChatUserJoin");
	}

	// Token: 0x06001D02 RID: 7426 RVA: 0x000C7351 File Offset: 0x000C5551
	private void GroudChatUserLeave(GameConnectedChatLeave_t gameConnectedChatLeave_T)
	{
		Debug.Log("GroudChatUserLeave");
	}

	// Token: 0x06001D03 RID: 7427 RVA: 0x000C7360 File Offset: 0x000C5560
	private void GroupChatUserMsg(GameConnectedClanChatMsg_t gameConnectedClanChatMsg_T)
	{
		Debug.Log("GroupChatMsg");
		string text;
		EChatEntryType echatEntryType;
		CSteamID steamIDFriend;
		int clanChatMessage = SteamFriends.GetClanChatMessage(gameConnectedClanChatMsg_T.m_steamIDClanChat, gameConnectedClanChatMsg_T.m_iMessageID, out text, 2048, out echatEntryType, out steamIDFriend);
		Debug.Log(string.Concat(new object[]
		{
			clanChatMessage,
			" : ",
			SteamFriends.GetFriendPersonaName(steamIDFriend),
			" : ",
			text
		}));
	}

	// Token: 0x04001247 RID: 4679
	private static SteamManager s_instance;

	// Token: 0x04001248 RID: 4680
	private static bool s_EverInialized;

	// Token: 0x04001249 RID: 4681
	private bool m_bInitialized;

	// Token: 0x0400124A RID: 4682
	private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

	// Token: 0x0400124B RID: 4683
	public static AppId_t TabletopSimulator = new AppId_t(286160U);

	// Token: 0x0400124C RID: 4684
	public static AppId_t KickstarterPointer = new AppId_t(325400U);

	// Token: 0x0400124D RID: 4685
	public static AppId_t KickstarterGold = new AppId_t(325401U);

	// Token: 0x0400124E RID: 4686
	public static CSteamID UserSteamID;

	// Token: 0x0400124F RID: 4687
	public static string StringSteamID = "";

	// Token: 0x04001250 RID: 4688
	public static string ISOCountry;

	// Token: 0x04001251 RID: 4689
	public static string SessionTicketHex = "";

	// Token: 0x04001252 RID: 4690
	public static bool bSteam = false;

	// Token: 0x04001253 RID: 4691
	public static bool bKickstarterPointer = false;

	// Token: 0x04001254 RID: 4692
	public static bool bKickstarterGold = false;

	// Token: 0x04001255 RID: 4693
	public static bool OverrideIsSubscribedAppTrue = false;

	// Token: 0x04001256 RID: 4694
	public static string SteamName = "";

	// Token: 0x0400125C RID: 4700
	private AccountID_t AID;

	// Token: 0x0400125D RID: 4701
	private uint Page;

	// Token: 0x0400125E RID: 4702
	private const string UPLOAD_NAME = "WorkshopUpload";

	// Token: 0x0400125F RID: 4703
	private const string UPLOAD_IMAGE_NAME = "WorkshopImageUpload.png";

	// Token: 0x04001260 RID: 4704
	private SteamManager.ModUploadInfo modNewUploadInfo;

	// Token: 0x04001261 RID: 4705
	private SteamManager.ModUploadInfo modUpdateUploadInfo;

	// Token: 0x04001262 RID: 4706
	public readonly List<SteamManager.ModInfo> PendingModDownloads = new List<SteamManager.ModInfo>();

	// Token: 0x04001263 RID: 4707
	public readonly List<SteamManager.ModInfo> PendingModThumbnailDownloads = new List<SteamManager.ModInfo>();

	// Token: 0x04001264 RID: 4708
	public readonly List<SteamManager.ModInfo> YourMods = new List<SteamManager.ModInfo>();

	// Token: 0x04001265 RID: 4709
	private CallResult<RemoteStorageDownloadUGCResult_t> OnRemoteStorageDownloadUGCResult;

	// Token: 0x04001266 RID: 4710
	private CallResult<SteamUGCQueryCompleted_t> OnSteamUGCQueryCompleted;

	// Token: 0x04001267 RID: 4711
	private CallResult<SteamUGCRequestUGCDetailsResult_t> OnSteamUGCDetailsResult;

	// Token: 0x04001268 RID: 4712
	private CallResult<RemoteStorageFileShareResult_t> OnRemoteStorageFileShareResult;

	// Token: 0x04001269 RID: 4713
	private CallResult<RemoteStoragePublishFileResult_t> OnRemoteStoragePublishFileResult;

	// Token: 0x0400126A RID: 4714
	private CallResult<RemoteStorageUpdatePublishedFileResult_t> OnRemoteStorageUpdatePublishedFileResult;

	// Token: 0x0400126B RID: 4715
	private Callback<RemoteStoragePublishedFileSubscribed_t> m_RemoteStoragePublishedFileSubscribed;

	// Token: 0x0400126C RID: 4716
	private CallResult<RemoteStorageEnumerateUserSubscribedFilesResult_t> OnRemoteStorageEnumerateUserSubscribedFilesResult;

	// Token: 0x0400126D RID: 4717
	private Callback<RemoteStoragePublishedFileUnsubscribed_t> m_RemoteStoragePublishedFileUnsubscribed;

	// Token: 0x0400126E RID: 4718
	private CallResult<RemoteStorageUnsubscribePublishedFileResult_t> OnRemoteStorageUnsubscribePublishedFileResult;

	// Token: 0x0400126F RID: 4719
	private Callback<UserStatsReceived_t> m_UserStatsReceived;

	// Token: 0x04001270 RID: 4720
	private Callback<GetAuthSessionTicketResponse_t> m_GetAuthSessionTicketResponse;

	// Token: 0x04001271 RID: 4721
	private Callback<ValidateAuthTicketResponse_t> m_ValidateAuthTicketResponse;

	// Token: 0x04001272 RID: 4722
	private Callback<AvatarImageLoaded_t> m_AvatarImageLoaded;

	// Token: 0x04001273 RID: 4723
	private CallResult<JoinClanChatRoomCompletionResult_t> JoinClanChatRoomCompletionResult;

	// Token: 0x04001274 RID: 4724
	private Callback<GameConnectedChatJoin_t> GameConnectedChatJoin;

	// Token: 0x04001275 RID: 4725
	private Callback<GameConnectedChatLeave_t> GameConnectedChatLeave;

	// Token: 0x04001276 RID: 4726
	private Callback<GameConnectedClanChatMsg_t> GameConnectedClanChatMsg;

	// Token: 0x04001277 RID: 4727
	private const string CLOUD_INFO_NAME = "CloudInfo.bson";

	// Token: 0x04001278 RID: 4728
	private const string CLOUD_FOLDER_NAME = "CloudFolder.bson";

	// Token: 0x04001279 RID: 4729
	private bool bUploadingToCloud;

	// Token: 0x0400127A RID: 4730
	private string currentUploadToCloudName = "";

	// Token: 0x0400127B RID: 4731
	private string currentCloudName = "";

	// Token: 0x0400127C RID: 4732
	private byte[] currentUploadToCloudData;

	// Token: 0x0400127D RID: 4733
	private string currentSHA1 = "";

	// Token: 0x0400127E RID: 4734
	private string currentCloudFolder;

	// Token: 0x0400127F RID: 4735
	private SHA1 sha1 = new SHA1CryptoServiceProvider();

	// Token: 0x04001280 RID: 4736
	private const int MAX_CLOUD_CHUNK_SIZE = 104857600;

	// Token: 0x04001281 RID: 4737
	private Dictionary<CSteamID, Action<CSteamID, Texture2D>> AvatarCallbacks = new Dictionary<CSteamID, Action<CSteamID, Texture2D>>();

	// Token: 0x04001282 RID: 4738
	public const string TranslationTag = "Translation";

	// Token: 0x04001283 RID: 4739
	private PublishedFileId_t mySubscribedMod;

	// Token: 0x04001284 RID: 4740
	private bool bStatsInitialized;

	// Token: 0x04001285 RID: 4741
	private CSteamID TabletopSimulatorGroupChatID = new CSteamID(103582791435376161UL);

	// Token: 0x020006CA RID: 1738
	public class ModUploadInfo
	{
		// Token: 0x0400294E RID: 10574
		public string Title;

		// Token: 0x0400294F RID: 10575
		public string Desc;

		// Token: 0x04002950 RID: 10576
		public string ThumbnailURL;

		// Token: 0x04002951 RID: 10577
		public IList<string> Tags;

		// Token: 0x04002952 RID: 10578
		public PublishedFileId_t PublisherId;

		// Token: 0x04002953 RID: 10579
		public byte[] Data;
	}

	// Token: 0x020006CB RID: 1739
	public struct ModInfo
	{
		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x06003C92 RID: 15506 RVA: 0x001798B8 File Offset: 0x00177AB8
		// (set) Token: 0x06003C93 RID: 15507 RVA: 0x001798C0 File Offset: 0x00177AC0
		public PublishedFileId_t Id { get; private set; }

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x06003C94 RID: 15508 RVA: 0x001798C9 File Offset: 0x00177AC9
		// (set) Token: 0x06003C95 RID: 15509 RVA: 0x001798D1 File Offset: 0x00177AD1
		public string Title { get; private set; }

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x06003C96 RID: 15510 RVA: 0x001798DA File Offset: 0x00177ADA
		// (set) Token: 0x06003C97 RID: 15511 RVA: 0x001798E2 File Offset: 0x00177AE2
		public UGCHandle_t Handle { get; private set; }

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06003C98 RID: 15512 RVA: 0x001798EB File Offset: 0x00177AEB
		// (set) Token: 0x06003C99 RID: 15513 RVA: 0x001798F3 File Offset: 0x00177AF3
		public SteamManager.ModInfo.ModType Type { get; private set; }

		// Token: 0x06003C9A RID: 15514 RVA: 0x001798FC File Offset: 0x00177AFC
		public ModInfo(SteamUGCDetails_t details, SteamManager.ModInfo.ModType Type)
		{
			this.Id = details.m_nPublishedFileId;
			this.Title = details.m_rgchTitle;
			switch (Type)
			{
			case SteamManager.ModInfo.ModType.Save:
			case SteamManager.ModInfo.ModType.Translation:
				this.Handle = details.m_hFile;
				break;
			case SteamManager.ModInfo.ModType.Thumbnail:
				this.Handle = details.m_hPreviewFile;
				break;
			default:
				this.Handle = default(UGCHandle_t);
				break;
			}
			this.Type = Type;
		}

		// Token: 0x020008A8 RID: 2216
		public enum ModType
		{
			// Token: 0x04002F79 RID: 12153
			Save,
			// Token: 0x04002F7A RID: 12154
			Thumbnail,
			// Token: 0x04002F7B RID: 12155
			Translation
		}
	}
}
