using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using NewNet;
using UnityEngine;

// Token: 0x02000229 RID: 553
public static class SerializationScript
{
	// Token: 0x06001B6B RID: 7019 RVA: 0x000BCDB4 File Offset: 0x000BAFB4
	public static void Save(List<SaveFileInfo> SaveList, string filePath)
	{
		try
		{
			string json = Json.GetJson(SaveList, true);
			File.WriteAllText(filePath, json);
		}
		catch (Exception ex)
		{
			Chat.LogError("Save .json error: " + ex.Message, true);
		}
	}

	// Token: 0x06001B6C RID: 7020 RVA: 0x000BCDFC File Offset: 0x000BAFFC
	public static void Save(List<BlockedPlayer> BlockList)
	{
		try
		{
			string json = Json.GetJson(BlockList, true);
			File.WriteAllText(DirectoryScript.blockFilePath, json);
		}
		catch (Exception ex)
		{
			Chat.LogError("Save .json error: " + ex.Message, true);
		}
	}

	// Token: 0x06001B6D RID: 7021 RVA: 0x000BCE48 File Offset: 0x000BB048
	public static void Save(GraphicsState GS)
	{
		try
		{
			string json = Json.GetJson(GS, true);
			File.WriteAllText(DirectoryScript.graphicsFilePath, json);
		}
		catch (Exception ex)
		{
			Chat.LogError("Save .json error: " + ex.Message, true);
		}
	}

	// Token: 0x06001B6E RID: 7022 RVA: 0x000BCE94 File Offset: 0x000BB094
	public static void Save(string filePath, string SaveName, bool Log = true)
	{
		if (!SerializationScript.FileIsJson(filePath))
		{
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
			filePath = filePath.Remove(filePath.Length - 4);
			filePath += ".json";
		}
		SaveState saveState = NetworkSingleton<ManagerPhysicsObject>.Instance.CurrentState();
		if (SaveName != "")
		{
			saveState.SaveName = SaveName;
		}
		ScreenshotScript.SaveThumbnail(Path.ChangeExtension(filePath, ".png"));
		SerializationScript.Save(filePath, saveState, Log);
	}

	// Token: 0x06001B6F RID: 7023 RVA: 0x000BCF0C File Offset: 0x000BB10C
	public static void Save(SaveState SS, string filePath, string SaveName, bool SaveThumbnail = true)
	{
		if (!SerializationScript.FileIsJson(filePath))
		{
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
			filePath = filePath.Remove(filePath.Length - 4);
			filePath += ".json";
		}
		if (SaveName != "")
		{
			SS.SaveName = SaveName;
		}
		if (SaveThumbnail)
		{
			ScreenshotScript.SaveThumbnail(Path.ChangeExtension(filePath, ".png"));
		}
		SerializationScript.Save(filePath, SS, true);
	}

	// Token: 0x06001B70 RID: 7024 RVA: 0x000BCF7C File Offset: 0x000BB17C
	public static void Save(SteamManager.ModInfo.ModType modType, byte[] Bytes, string fileName, string SaveName)
	{
		string path;
		SerializationScript.WorkshopFileExists(fileName, out path);
		if (modType == SteamManager.ModInfo.ModType.Translation)
		{
			try
			{
				string path2 = Path.ChangeExtension(path, ".csv");
				CSV csv = new CSV(Encoding.UTF8.GetString(Bytes));
				File.WriteAllText(path2, csv.ToString());
				return;
			}
			catch (Exception e)
			{
				Chat.LogException(string.Format("updating Workshop mod {0} (most likely corrupt)", SaveName), e, true, false);
				return;
			}
		}
		string text = Path.ChangeExtension(path, ".cjc");
		string filePath = Path.ChangeExtension(path, ".json");
		try
		{
			List<PhysicsState> physicsState = SerializationScript.GetPhysicsState(Bytes);
			physicsState[0].name = SaveName;
			SerializationScript.Save(text, physicsState);
		}
		catch (Exception)
		{
			try
			{
				SaveState saveState = Json.Load<SaveState>(Bytes, false);
				saveState.SaveName = SaveName;
				if (File.Exists(text))
				{
					File.Delete(text);
				}
				SerializationScript.Save(filePath, saveState, true);
			}
			catch (Exception e2)
			{
				Chat.LogException(string.Format("updating Workshop mod {0} (most likely corrupt)", SaveName), e2, true, false);
			}
		}
	}

	// Token: 0x06001B71 RID: 7025 RVA: 0x000BD080 File Offset: 0x000BB280
	public static void Save(SaveState SS, string SaveChestName, GameObject ThumbnailObject)
	{
		string text = DirectoryScript.savedObjectsFilePath + "//" + SaveChestName + ".json";
		try
		{
			ScreenshotScript.SaveThumbnail(Path.ChangeExtension(text, ".png"), ThumbnailObject);
		}
		catch (Exception e)
		{
			Chat.LogException("creating Saved Object thumbnail", e, true, false);
		}
		SerializationScript.Save(text, SS, true);
	}

	// Token: 0x06001B72 RID: 7026 RVA: 0x000BD0E0 File Offset: 0x000BB2E0
	public static void Save(string filePath, SaveState SS, bool Log = true)
	{
		string cleanPath = SerializationScript.GetCleanPath(filePath);
		try
		{
			string json = Json.GetJson(SS, true);
			File.WriteAllText(filePath, json);
			EventManager.TriggerFileSave(filePath);
			if (Log)
			{
				Chat.LogSystem("File saved to " + cleanPath, false);
			}
		}
		catch (Exception ex)
		{
			Chat.LogError("Save .json error: " + ex.Message, true);
		}
	}

	// Token: 0x06001B73 RID: 7027 RVA: 0x000BD148 File Offset: 0x000BB348
	public static void Save(string filePath, List<PhysicsState> PSList)
	{
		string cleanPath = SerializationScript.GetCleanPath(filePath);
		try
		{
			using (Stream stream = File.Open(filePath, FileMode.Create))
			{
				new BinaryFormatter
				{
					Binder = new PhysicsStateDeserializationBinder()
				}.Serialize(stream, PSList);
			}
			EventManager.TriggerFileSave(filePath);
			Chat.LogSystem("File saved to " + cleanPath, false);
		}
		catch (Exception ex)
		{
			Chat.LogError("Save .cjc error: " + ex.Message, true);
		}
	}

	// Token: 0x06001B74 RID: 7028 RVA: 0x000BD1D8 File Offset: 0x000BB3D8
	public static List<SaveState> LoadJsonFolder(string FolderfilePath, bool bLoad = true)
	{
		string[] files = Directory.GetFiles(FolderfilePath, "*.json", SearchOption.TopDirectoryOnly);
		List<SaveState> list = new List<SaveState>();
		foreach (string text in files)
		{
			if (File.Exists(text))
			{
				try
				{
					StreamReader streamReader = File.OpenText(text);
					string json = streamReader.ReadToEnd();
					streamReader.Close();
					SaveState saveState = Json.Load<SaveState>(json);
					string[] array2 = text.Split(new string[]
					{
						".cjc",
						"/",
						"\\",
						".json"
					}, StringSplitOptions.RemoveEmptyEntries);
					saveState.SaveName = array2[array2.Length - 1];
					list.Add(saveState);
					if (bLoad && NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
					{
						NetworkSingleton<ManagerPhysicsObject>.Instance.SpawnObjectsOffset(saveState.ObjectStates, NetworkSingleton<NetworkUI>.Instance.SpawnPos, true);
					}
				}
				catch (Exception ex)
				{
					Chat.LogError("Load .json error: " + ex.Message, true);
					Debug.LogException(ex);
				}
			}
		}
		return list;
	}

	// Token: 0x06001B75 RID: 7029 RVA: 0x000BD2E0 File Offset: 0x000BB4E0
	public static SaveState LoadJson(string filePath, bool bLoad = true)
	{
		if (!File.Exists(filePath))
		{
			return null;
		}
		SaveState result;
		try
		{
			StreamReader streamReader = File.OpenText(filePath);
			string text = streamReader.ReadToEnd();
			streamReader.Close();
			text = SteamCloudURL.ConvertOldToNewCloudURL(text);
			SaveState saveState = Json.Load<SaveState>(text);
			if (bLoad)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.LoadSaveState(saveState, false, true);
				SerializationScript.LastLoadedJsonFilePath = filePath;
			}
			result = saveState;
		}
		catch (Exception ex)
		{
			Chat.LogError("Load .json error: " + ex.Message, true);
			Debug.LogException(ex);
			result = null;
		}
		return result;
	}

	// Token: 0x06001B76 RID: 7030 RVA: 0x000BD364 File Offset: 0x000BB564
	public static List<PhysicsState> LoadCJCFolder(string FolderfilePath, bool bLoad = true)
	{
		string[] files = Directory.GetFiles(FolderfilePath, "*.cjc", SearchOption.TopDirectoryOnly);
		List<PhysicsState> list = new List<PhysicsState>();
		foreach (string text in files)
		{
			if (File.Exists(text))
			{
				try
				{
					List<PhysicsState> list2;
					using (Stream stream = File.Open(text, FileMode.Open))
					{
						list2 = (List<PhysicsState>)new BinaryFormatter
						{
							Binder = new PhysicsStateDeserializationBinder()
						}.Deserialize(stream);
					}
					PhysicsState physicsState = list2[0];
					string[] array2 = text.Split(new string[]
					{
						".cjc",
						"/",
						"\\",
						".json"
					}, StringSplitOptions.RemoveEmptyEntries);
					physicsState.PlayerTurn = array2[array2.Length - 1];
					list.Add(physicsState);
					if (bLoad)
					{
						physicsState.posX = NetworkSingleton<NetworkUI>.Instance.SpawnPos.x;
						physicsState.posY = NetworkSingleton<NetworkUI>.Instance.SpawnPos.y;
						physicsState.posZ = NetworkSingleton<NetworkUI>.Instance.SpawnPos.z;
						if (Network.isServer)
						{
							NetworkSingleton<ManagerPhysicsObject>.Instance.LoadPhysicsState(physicsState);
						}
						else
						{
							NetworkSingleton<ManagerPhysicsObject>.Instance.SpawnCJCObjectOffset(physicsState, true);
						}
					}
				}
				catch (Exception ex)
				{
					Chat.LogError("Load .cjc error: " + ex.Message, true);
				}
			}
		}
		return list;
	}

	// Token: 0x06001B77 RID: 7031 RVA: 0x000BD4D0 File Offset: 0x000BB6D0
	public static List<PhysicsState> LoadCJC(string filePath, bool bLoad = true)
	{
		if (!File.Exists(filePath))
		{
			return null;
		}
		List<PhysicsState> result;
		try
		{
			List<PhysicsState> list;
			using (Stream stream = File.Open(filePath, FileMode.Open))
			{
				list = (List<PhysicsState>)new BinaryFormatter
				{
					Binder = new PhysicsStateDeserializationBinder()
				}.Deserialize(stream);
			}
			if (bLoad)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.LoadStateList(list, false, true);
			}
			result = list;
		}
		catch (Exception ex)
		{
			Chat.LogError("Load .cjc error: " + ex.Message, true);
			Debug.LogException(ex);
			result = null;
		}
		return result;
	}

	// Token: 0x06001B78 RID: 7032 RVA: 0x000BD56C File Offset: 0x000BB76C
	public static List<BlockedPlayer> LoadBlock()
	{
		if (!File.Exists(DirectoryScript.blockFilePath))
		{
			return new List<BlockedPlayer>();
		}
		List<BlockedPlayer> result;
		try
		{
			StreamReader streamReader = File.OpenText(DirectoryScript.blockFilePath);
			string json = streamReader.ReadToEnd();
			streamReader.Close();
			List<BlockedPlayer> list = Json.Load<List<BlockedPlayer>>(json);
			if (list == null)
			{
				list = new List<BlockedPlayer>();
			}
			result = list;
		}
		catch (Exception ex)
		{
			Chat.LogError("Load block list error: " + ex.Message, true);
			result = new List<BlockedPlayer>();
		}
		return result;
	}

	// Token: 0x06001B79 RID: 7033 RVA: 0x000BD5E8 File Offset: 0x000BB7E8
	public static List<SaveFileInfo> LoadWorkshopFileInfos()
	{
		if (!File.Exists(DirectoryScript.workshopFilePath + "//WorkshopFileInfos.json"))
		{
			return new List<SaveFileInfo>();
		}
		List<SaveFileInfo> result;
		try
		{
			StreamReader streamReader = File.OpenText(DirectoryScript.workshopFilePath + "//WorkshopFileInfos.json");
			string json = streamReader.ReadToEnd();
			streamReader.Close();
			List<SaveFileInfo> list = Json.Load<List<SaveFileInfo>>(json);
			if (list == null)
			{
				list = new List<SaveFileInfo>();
			}
			result = list;
		}
		catch (Exception ex)
		{
			Chat.LogError("Load UI Workshop list error: " + ex.Message, true);
			result = new List<SaveFileInfo>();
		}
		return result;
	}

	// Token: 0x06001B7A RID: 7034 RVA: 0x000BD678 File Offset: 0x000BB878
	public static List<SaveFileInfo> LoadSaveFileInfos()
	{
		if (!File.Exists(DirectoryScript.saveFilePath + "//SaveFileInfos.json"))
		{
			return new List<SaveFileInfo>();
		}
		List<SaveFileInfo> result;
		try
		{
			StreamReader streamReader = File.OpenText(DirectoryScript.saveFilePath + "//SaveFileInfos.json");
			string json = streamReader.ReadToEnd();
			streamReader.Close();
			List<SaveFileInfo> list = Json.Load<List<SaveFileInfo>>(json);
			if (list == null)
			{
				list = new List<SaveFileInfo>();
			}
			result = list;
		}
		catch (Exception ex)
		{
			Chat.LogError("Load UI Save list error: " + ex.Message, true);
			result = new List<SaveFileInfo>();
		}
		return result;
	}

	// Token: 0x06001B7B RID: 7035 RVA: 0x000BD708 File Offset: 0x000BB908
	public static GraphicsState LoadGraphics()
	{
		if (!File.Exists(DirectoryScript.graphicsFilePath))
		{
			return null;
		}
		GraphicsState result;
		try
		{
			StreamReader streamReader = File.OpenText(DirectoryScript.graphicsFilePath);
			string json = streamReader.ReadToEnd();
			streamReader.Close();
			result = Json.Load<GraphicsState>(json);
		}
		catch (Exception ex)
		{
			Chat.LogError("Load block list error: " + ex.Message, true);
			result = null;
		}
		return result;
	}

	// Token: 0x06001B7C RID: 7036 RVA: 0x000BD770 File Offset: 0x000BB970
	public static void SavePlayerPref(string playerPref, string filePath)
	{
		try
		{
			string @string = PlayerPrefs.GetString(playerPref, "");
			File.WriteAllText(filePath, @string);
		}
		catch (Exception ex)
		{
			Chat.LogError("Save " + playerPref + " error: " + ex.Message, true);
		}
	}

	// Token: 0x06001B7D RID: 7037 RVA: 0x000BD7C4 File Offset: 0x000BB9C4
	public static string LoadPlayerPref(string playerPref, string filePath)
	{
		if (!File.Exists(filePath))
		{
			return "";
		}
		string result;
		try
		{
			StreamReader streamReader = File.OpenText(filePath);
			string text = streamReader.ReadToEnd();
			streamReader.Close();
			text = text.Replace("\r", "");
			PlayerPrefs.SetString(playerPref, text);
			result = text;
		}
		catch (Exception ex)
		{
			Chat.LogError("Load " + playerPref + " error: " + ex.Message, true);
			Debug.LogException(ex);
			result = "";
		}
		return result;
	}

	// Token: 0x06001B7E RID: 7038 RVA: 0x000BD84C File Offset: 0x000BBA4C
	public static bool Delete(string filePath)
	{
		string cleanPath = SerializationScript.GetCleanPath(filePath);
		try
		{
			string thumbnailPath = SerializationScript.GetThumbnailPath(filePath);
			if (!string.IsNullOrEmpty(thumbnailPath) && File.Exists(thumbnailPath))
			{
				File.Delete(thumbnailPath);
				Chat.LogSystem("Thumbnail deleted at " + SerializationScript.GetCleanPath(thumbnailPath), false);
			}
		}
		catch (Exception e)
		{
			Chat.LogException("deleting thumbnail", e, true, false);
		}
		bool result;
		try
		{
			if (SerializationScript.FileIsDirectory(filePath))
			{
				Directory.Delete(filePath, true);
				Chat.LogSystem("Folder deleted at " + cleanPath, false);
			}
			else
			{
				File.Delete(filePath);
				Chat.LogSystem("File deleted at " + cleanPath, false);
			}
			EventManager.TriggerFileDelete(filePath);
			result = true;
		}
		catch (Exception)
		{
			Chat.LogError("Error deleting at " + cleanPath, true);
			result = false;
		}
		return result;
	}

	// Token: 0x06001B7F RID: 7039 RVA: 0x000BD91C File Offset: 0x000BBB1C
	public static string GetSaveName(string filePath)
	{
		if (!File.Exists(filePath))
		{
			return "";
		}
		string result;
		try
		{
			if (!SerializationScript.FileIsJson(filePath))
			{
				result = SerializationScript.LoadCJC(filePath, false)[0].name;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				using (StreamReader streamReader = File.OpenText(filePath))
				{
					SaveInfo saveInfo = default(SaveInfo);
					streamReader.ReadLine();
					for (int i = 0; i < 20; i++)
					{
						string text = streamReader.ReadLine();
						if (text != null)
						{
							if (text.StartsWith("  \"SaveName\": "))
							{
								stringBuilder.Clear();
								byte b = 0;
								for (int j = 0; j < text.Length - 2; j++)
								{
									char c = text[j];
									if (b >= 3)
									{
										stringBuilder.Append(c);
									}
									else if (c == '"')
									{
										b += 1;
									}
								}
								saveInfo.SaveName = stringBuilder.ToString();
								if (saveInfo.Date != null)
								{
									break;
								}
							}
							else if (text.StartsWith("  \"Date\": "))
							{
								stringBuilder.Clear();
								byte b2 = 0;
								for (int k = 0; k < text.Length - 2; k++)
								{
									char c2 = text[k];
									if (b2 >= 3)
									{
										stringBuilder.Append(c2);
									}
									else if (c2 == '"')
									{
										b2 += 1;
									}
								}
								saveInfo.Date = stringBuilder.ToString();
								if (saveInfo.SaveName != null)
								{
									break;
								}
							}
						}
					}
					if (saveInfo.SaveName != null && saveInfo.Date != null)
					{
						return " - " + saveInfo.SaveName + " - " + saveInfo.Date;
					}
				}
				Debug.Log(filePath);
				SaveInfo saveInfo2 = Json.Load<SaveInfo>(File.ReadAllText(filePath));
				result = " - " + saveInfo2.SaveName + " - " + saveInfo2.Date;
			}
		}
		catch (Exception)
		{
			result = " - Error Loading This Save";
		}
		return result;
	}

	// Token: 0x06001B80 RID: 7040 RVA: 0x000BDB24 File Offset: 0x000BBD24
	public static string GetSaveNameWorkshop(string filePath)
	{
		if (!File.Exists(filePath))
		{
			return "";
		}
		string result;
		try
		{
			if (!SerializationScript.FileIsJson(filePath))
			{
				result = SerializationScript.LoadCJC(filePath, false)[0].name;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				using (StreamReader streamReader = File.OpenText(filePath))
				{
					SaveInfo saveInfo = default(SaveInfo);
					streamReader.ReadLine();
					for (int i = 0; i < 20; i++)
					{
						string text = streamReader.ReadLine();
						if (text != null)
						{
							if (text.StartsWith("  \"SaveName\": "))
							{
								stringBuilder.Clear();
								byte b = 0;
								for (int j = 0; j < text.Length - 2; j++)
								{
									char c = text[j];
									if (b >= 3)
									{
										stringBuilder.Append(c);
									}
									else if (c == '"')
									{
										b += 1;
									}
								}
								saveInfo.SaveName = stringBuilder.ToString();
								if (saveInfo.Date != null)
								{
									break;
								}
							}
							else if (text.StartsWith("  \"Date\": "))
							{
								stringBuilder.Clear();
								byte b2 = 0;
								for (int k = 0; k < text.Length - 2; k++)
								{
									char c2 = text[k];
									if (b2 >= 3)
									{
										stringBuilder.Append(c2);
									}
									else if (c2 == '"')
									{
										b2 += 1;
									}
								}
								saveInfo.Date = stringBuilder.ToString();
								if (saveInfo.SaveName != null)
								{
									break;
								}
							}
						}
					}
					if (saveInfo.SaveName != null && saveInfo.Date != null)
					{
						return saveInfo.SaveName;
					}
				}
				Debug.Log(filePath);
				result = Json.Load<SaveInfo>(File.ReadAllText(filePath)).SaveName;
			}
		}
		catch (Exception)
		{
			result = "Error Loading This Save";
		}
		return result;
	}

	// Token: 0x06001B81 RID: 7041 RVA: 0x000BDCFC File Offset: 0x000BBEFC
	public static uint GetFileTimeWorkshop(string FileName)
	{
		string text;
		if (!SerializationScript.WorkshopFileExists(FileName, out text))
		{
			return 0U;
		}
		if (text == "")
		{
			return 0U;
		}
		if (new FileInfo(text).Length == 0L)
		{
			return 0U;
		}
		return SerializationScript.GetFileTime(text);
	}

	// Token: 0x06001B82 RID: 7042 RVA: 0x000BDD3C File Offset: 0x000BBF3C
	public static uint GetTimeFromEpoch(DateTime dateTimeUTC)
	{
		return (uint)(dateTimeUTC - SerializationScript.UnixEpoch).TotalSeconds;
	}

	// Token: 0x06001B83 RID: 7043 RVA: 0x000BDD5D File Offset: 0x000BBF5D
	public static uint GetFileTime(string FilePath)
	{
		if (string.IsNullOrEmpty(FilePath) || !File.Exists(FilePath))
		{
			return 0U;
		}
		return SerializationScript.GetTimeFromEpoch(File.GetLastWriteTimeUtc(FilePath));
	}

	// Token: 0x06001B84 RID: 7044 RVA: 0x000BDD7C File Offset: 0x000BBF7C
	public static uint GetAccessTime(string FilePath)
	{
		if (string.IsNullOrEmpty(FilePath) || !File.Exists(FilePath))
		{
			return 0U;
		}
		return SerializationScript.GetTimeFromEpoch(File.GetLastAccessTimeUtc(FilePath));
	}

	// Token: 0x06001B85 RID: 7045 RVA: 0x000BDD9C File Offset: 0x000BBF9C
	public static void UpdateAccessTime(string FilePath)
	{
		try
		{
			File.SetLastAccessTime(FilePath, DateTime.Now);
		}
		catch (Exception)
		{
		}
	}

	// Token: 0x06001B86 RID: 7046 RVA: 0x000BDDCC File Offset: 0x000BBFCC
	public static DateTime GetDateTime(uint FileTime)
	{
		return SerializationScript.UnixEpoch.AddSeconds(FileTime).ToLocalTime();
	}

	// Token: 0x06001B87 RID: 7047 RVA: 0x000BDDF0 File Offset: 0x000BBFF0
	public static uint GetNewSaveSlot()
	{
		List<string> saveFiles = SerializationScript.GetSaveFiles(DirectoryScript.saveFilePath, true, DirectoryScript.SavedObjectsDirectories);
		uint num = 0U;
		for (int i = 0; i < saveFiles.Count; i++)
		{
			uint num2;
			if (SerializationScript.SaveNameToSlot(Path.GetFileNameWithoutExtension(saveFiles[i]), out num2) && num2 > num)
			{
				num = num2;
			}
		}
		return num + 1U;
	}

	// Token: 0x06001B88 RID: 7048 RVA: 0x000BDE3F File Offset: 0x000BC03F
	public static bool SaveNameToSlot(string name, out uint slot)
	{
		if (name.StartsWith("TS_Save_"))
		{
			return uint.TryParse(name.Substring("TS_Save_".Length, name.Length - "TS_Save_".Length), out slot);
		}
		slot = 0U;
		return false;
	}

	// Token: 0x06001B89 RID: 7049 RVA: 0x000BDE7C File Offset: 0x000BC07C
	public static bool AutoSaveNameToSlot(string name, out uint slot)
	{
		if (name == "TS_AutoSave")
		{
			slot = 1U;
			return true;
		}
		if (name.StartsWith("TS_AutoSave_"))
		{
			return uint.TryParse(name.Substring("TS_AutoSave_".Length, name.Length - "TS_AutoSave_".Length), out slot);
		}
		slot = 0U;
		return false;
	}

	// Token: 0x06001B8A RID: 7050 RVA: 0x000BDED4 File Offset: 0x000BC0D4
	public static void CacheWorkshopFileExist()
	{
		List<string> saveFiles = SerializationScript.GetSaveFiles(DirectoryScript.workshopFilePath, true, null);
		saveFiles.AddRange(SerializationScript.GetTranslationFiles(DirectoryScript.workshopFilePath, true, null));
		if (SerializationScript.cachedWorkshopPathDictionary == null)
		{
			SerializationScript.cachedWorkshopPathDictionary = new Dictionary<string, string>();
		}
		else
		{
			SerializationScript.cachedWorkshopPathDictionary.Clear();
		}
		for (int i = 0; i < saveFiles.Count; i++)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(saveFiles[i]);
			if (!SerializationScript.cachedWorkshopPathDictionary.ContainsKey(fileNameWithoutExtension))
			{
				SerializationScript.cachedWorkshopPathDictionary.Add(fileNameWithoutExtension, saveFiles[i]);
			}
			else
			{
				Debug.Log("Duplicate workshop file names: " + saveFiles[i]);
			}
		}
	}

	// Token: 0x06001B8B RID: 7051 RVA: 0x000BDF74 File Offset: 0x000BC174
	public static bool WorkshopFileExists(string workshopFileName, out string path)
	{
		if (SerializationScript.cachedWorkshopPathDictionary == null)
		{
			SerializationScript.CacheWorkshopFileExist();
		}
		string text;
		if (SerializationScript.cachedWorkshopPathDictionary.TryGetValue(workshopFileName, out text))
		{
			if (File.Exists(text))
			{
				path = text;
				return true;
			}
			SerializationScript.CacheWorkshopFileExist();
			if (SerializationScript.cachedWorkshopPathDictionary.TryGetValue(workshopFileName, out text) && File.Exists(text))
			{
				path = text;
				return true;
			}
		}
		path = DirectoryScript.workshopFilePath + "//" + workshopFileName + ".json";
		return false;
	}

	// Token: 0x06001B8C RID: 7052 RVA: 0x000BDFE4 File Offset: 0x000BC1E4
	public static List<PhysicsState> GetPhysicsState(byte[] Bytes)
	{
		List<PhysicsState> result;
		using (Stream stream = new MemoryStream(Bytes))
		{
			result = (List<PhysicsState>)new BinaryFormatter
			{
				Binder = new PhysicsStateDeserializationBinder()
			}.Deserialize(stream);
		}
		return result;
	}

	// Token: 0x06001B8D RID: 7053 RVA: 0x000BE034 File Offset: 0x000BC234
	public static byte[] GetBytes(List<PhysicsState> ListPS)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		binaryFormatter.Binder = new PhysicsStateDeserializationBinder();
		MemoryStream memoryStream = new MemoryStream();
		binaryFormatter.Serialize(memoryStream, ListPS);
		return memoryStream.ToArray();
	}

	// Token: 0x06001B8E RID: 7054 RVA: 0x000BE064 File Offset: 0x000BC264
	public static List<string> GetSaveFiles(string filePath, bool recursive = false, List<string> excludePaths = null)
	{
		string[] files = Directory.GetFiles(filePath, "*.cjc", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
		string[] files2 = Directory.GetFiles(filePath, "*.json", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
		List<string> list = new List<string>(files.Length + files2.Length);
		SerializationScript.CheckAddPath(files, list, true, excludePaths);
		SerializationScript.CheckAddPath(files2, list, true, excludePaths);
		return list;
	}

	// Token: 0x06001B8F RID: 7055 RVA: 0x000BE0B5 File Offset: 0x000BC2B5
	public static List<string> GetTranslationFiles(string filePath, bool recursive = false, List<string> excludePaths = null)
	{
		return Directory.GetFiles(filePath, "*.csv", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).ToList<string>();
	}

	// Token: 0x06001B90 RID: 7056 RVA: 0x000BE0D0 File Offset: 0x000BC2D0
	public static List<string> GetDirectories(string filePath, bool recursive = false, List<string> excludePaths = null)
	{
		string[] directories = Directory.GetDirectories(filePath, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
		List<string> list = new List<string>();
		SerializationScript.CheckAddPath(directories, list, false, excludePaths);
		return list;
	}

	// Token: 0x06001B91 RID: 7057 RVA: 0x000BE100 File Offset: 0x000BC300
	private static void CheckAddPath(string[] source, List<string> destination, bool isFile, List<string> excludePaths = null)
	{
		bool flag = false;
		if (excludePaths != null)
		{
			flag = true;
			for (int i = 0; i < excludePaths.Count; i++)
			{
				excludePaths[i] = Path.GetFullPath(excludePaths[i]);
			}
		}
		foreach (string text in source)
		{
			bool flag2 = false;
			if (flag)
			{
				string fullPath = Path.GetFullPath(text);
				for (int k = 0; k < excludePaths.Count; k++)
				{
					if (fullPath.StartsWith(excludePaths[k]))
					{
						flag2 = true;
					}
				}
			}
			if (!flag2)
			{
				if (isFile)
				{
					if (!DirectoryScript.IsBlacklistedFile(Path.GetFileName(text)))
					{
						destination.Add(text);
					}
				}
				else if (!DirectoryScript.IsBlacklistedFolder(Path.GetFileName(text)))
				{
					destination.Add(text);
				}
			}
		}
	}

	// Token: 0x06001B92 RID: 7058 RVA: 0x000BE1B4 File Offset: 0x000BC3B4
	public static void CreateDirectory(string filePath)
	{
		string cleanPath = SerializationScript.GetCleanPath(filePath);
		try
		{
			Directory.CreateDirectory(filePath);
			Chat.LogSystem("Folder created at " + cleanPath, false);
			EventManager.TriggerFileSave(filePath);
		}
		catch (Exception)
		{
			Chat.LogError("Error creating directory at " + cleanPath, true);
		}
	}

	// Token: 0x06001B93 RID: 7059 RVA: 0x000BE20C File Offset: 0x000BC40C
	public static void Move(string sourcePath, string destinationPath)
	{
		if (SerializationScript.FileIsDirectory(sourcePath))
		{
			SerializationScript.MoveDirectory(sourcePath, destinationPath);
			return;
		}
		SerializationScript.MoveFile(sourcePath, destinationPath);
	}

	// Token: 0x06001B94 RID: 7060 RVA: 0x000BE228 File Offset: 0x000BC428
	public static void MoveFile(string sourceFilePath, string destinationFilePath)
	{
		string cleanPath = SerializationScript.GetCleanPath(destinationFilePath);
		if (sourceFilePath == destinationFilePath)
		{
			return;
		}
		string thumbnailPath = SerializationScript.GetThumbnailPath(sourceFilePath);
		if (!string.IsNullOrEmpty(thumbnailPath) && File.Exists(thumbnailPath))
		{
			string str = Path.ChangeExtension(cleanPath, ".png");
			try
			{
				File.Move(thumbnailPath, Path.ChangeExtension(destinationFilePath, ".png"));
				Chat.LogSystem("Thumbnail moved to " + str, false);
			}
			catch (Exception)
			{
				Chat.LogError("Error moving thumbnail to " + str, true);
			}
		}
		try
		{
			File.Move(sourceFilePath, destinationFilePath);
			Chat.LogSystem("File moved to " + cleanPath, false);
			EventManager.TriggerFileSave(destinationFilePath);
		}
		catch (Exception)
		{
			Chat.LogError("Error moving file to " + cleanPath, true);
		}
	}

	// Token: 0x06001B95 RID: 7061 RVA: 0x000BE2F4 File Offset: 0x000BC4F4
	public static void MoveDirectory(string sourceDirectoryPath, string destinationDirectoryPath)
	{
		string cleanPath = SerializationScript.GetCleanPath(destinationDirectoryPath);
		if (sourceDirectoryPath == destinationDirectoryPath)
		{
			return;
		}
		try
		{
			Directory.Move(sourceDirectoryPath, destinationDirectoryPath);
			Chat.LogSystem("Directory moved to " + cleanPath, false);
			EventManager.TriggerFileSave(destinationDirectoryPath);
		}
		catch (Exception)
		{
			Chat.LogError("Error moving directory to " + cleanPath, true);
		}
	}

	// Token: 0x06001B96 RID: 7062 RVA: 0x000BE358 File Offset: 0x000BC558
	public static List<string> GetLocalFolders(string path, bool includeRoot = true, string excludePath = "")
	{
		List<string> directories = SerializationScript.GetDirectories(path, true, null);
		List<string> list = new List<string>();
		if (includeRoot)
		{
			list.Add("<Root Folder>");
		}
		if (!string.IsNullOrEmpty(excludePath))
		{
			excludePath = Path.GetFullPath(excludePath);
		}
		for (int i = 0; i < directories.Count; i++)
		{
			if (string.IsNullOrEmpty(excludePath) || !Path.GetFullPath(directories[i]).StartsWith(excludePath))
			{
				list.Add(directories[i].Replace(path, ""));
			}
		}
		return list;
	}

	// Token: 0x06001B97 RID: 7063 RVA: 0x000BE3D8 File Offset: 0x000BC5D8
	public static string CombineLocalFolder(string rootPath, string localFolder, string name)
	{
		string result;
		if (localFolder == "<Root Folder>")
		{
			result = rootPath + "//" + name;
		}
		else
		{
			result = rootPath + localFolder + "//" + name;
		}
		return result;
	}

	// Token: 0x06001B98 RID: 7064 RVA: 0x000BE416 File Offset: 0x000BC616
	public static bool FileIsJson(string filePath)
	{
		return Path.GetExtension(filePath) != ".cjc";
	}

	// Token: 0x06001B99 RID: 7065 RVA: 0x000BE428 File Offset: 0x000BC628
	public static bool FileIsDirectory(string filePath)
	{
		return (File.GetAttributes(filePath) & FileAttributes.Directory) == FileAttributes.Directory;
	}

	// Token: 0x06001B9A RID: 7066 RVA: 0x000BE437 File Offset: 0x000BC637
	public static string GetCleanPath(string filePath)
	{
		if (!string.IsNullOrEmpty(filePath))
		{
			return Path.GetFullPath(filePath);
		}
		return filePath;
	}

	// Token: 0x06001B9B RID: 7067 RVA: 0x000BE44C File Offset: 0x000BC64C
	public static string GetThumbnailPath(string filePath)
	{
		string result = null;
		if (!Path.HasExtension(filePath))
		{
			return result;
		}
		return Path.ChangeExtension(filePath, ".png");
	}

	// Token: 0x06001B9C RID: 7068 RVA: 0x000BE472 File Offset: 0x000BC672
	public static string RemoveInvalidCharsFromFileName(string fileName)
	{
		if (fileName == null)
		{
			return null;
		}
		return SerializationScript.RemoveNewLineAndWhiteSpace(Regex.Replace(fileName, "[^\\w\\s]", string.Empty));
	}

	// Token: 0x06001B9D RID: 7069 RVA: 0x000BE48E File Offset: 0x000BC68E
	public static string RemoveNewLineAndWhiteSpace(string s)
	{
		if (s == null)
		{
			return null;
		}
		return s.Replace(Environment.NewLine, string.Empty).Trim();
	}

	// Token: 0x04001156 RID: 4438
	public static string LastLoadedJsonFilePath = "";

	// Token: 0x04001157 RID: 4439
	public static DateTime UnixEpoch = new DateTime(1970, 1, 1);

	// Token: 0x04001158 RID: 4440
	private const string FILE_SAVE = "TS_Save_";

	// Token: 0x04001159 RID: 4441
	private const string AUTO_SAVE = "TS_AutoSave_";

	// Token: 0x0400115A RID: 4442
	private static Dictionary<string, string> cachedWorkshopPathDictionary = null;

	// Token: 0x0400115B RID: 4443
	public const string RootName = "<Root Folder>";
}
