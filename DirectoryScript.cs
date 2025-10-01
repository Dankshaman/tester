using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x020000FF RID: 255
public class DirectoryScript : MonoBehaviour
{
	// Token: 0x17000205 RID: 517
	// (get) Token: 0x06000C74 RID: 3188 RVA: 0x00055523 File Offset: 0x00053723
	// (set) Token: 0x06000C75 RID: 3189 RVA: 0x0005552A File Offset: 0x0005372A
	public static string dataPath { get; private set; }

	// Token: 0x17000206 RID: 518
	// (get) Token: 0x06000C76 RID: 3190 RVA: 0x00055532 File Offset: 0x00053732
	public static string modsFilePath
	{
		get
		{
			if (ConfigGame.Settings.ConfigMods.Location != ConfigGame.ModLocation.GameData || Application.isEditor)
			{
				return DirectoryScript.rootPath + "//Mods";
			}
			return DirectoryScript.dataPath + "//Mods";
		}
	}

	// Token: 0x17000207 RID: 519
	// (get) Token: 0x06000C77 RID: 3191 RVA: 0x0005556C File Offset: 0x0005376C
	public static string workshopFilePath
	{
		get
		{
			return DirectoryScript.modsFilePath + "//Workshop";
		}
	}

	// Token: 0x17000208 RID: 520
	// (get) Token: 0x06000C78 RID: 3192 RVA: 0x0005557D File Offset: 0x0005377D
	public static string oldworkshopThumbnailsFilePath
	{
		get
		{
			return DirectoryScript.modsFilePath + "//Workshop//Thumbnails";
		}
	}

	// Token: 0x17000209 RID: 521
	// (get) Token: 0x06000C79 RID: 3193 RVA: 0x0005558E File Offset: 0x0005378E
	public static string imageFilePath
	{
		get
		{
			return DirectoryScript.modsFilePath + "//Images//";
		}
	}

	// Token: 0x1700020A RID: 522
	// (get) Token: 0x06000C7A RID: 3194 RVA: 0x0005559F File Offset: 0x0005379F
	public static string imageRawFilePath
	{
		get
		{
			return DirectoryScript.modsFilePath + "//Images Raw//";
		}
	}

	// Token: 0x1700020B RID: 523
	// (get) Token: 0x06000C7B RID: 3195 RVA: 0x000555B0 File Offset: 0x000537B0
	public static string modelFilePath
	{
		get
		{
			return DirectoryScript.modsFilePath + "//Models//";
		}
	}

	// Token: 0x1700020C RID: 524
	// (get) Token: 0x06000C7C RID: 3196 RVA: 0x000555C1 File Offset: 0x000537C1
	public static string modelRawFilePath
	{
		get
		{
			return DirectoryScript.modsFilePath + "//Models Raw//";
		}
	}

	// Token: 0x1700020D RID: 525
	// (get) Token: 0x06000C7D RID: 3197 RVA: 0x000555D2 File Offset: 0x000537D2
	public static string assetbundleFilePath
	{
		get
		{
			return DirectoryScript.modsFilePath + "//Assetbundles//";
		}
	}

	// Token: 0x1700020E RID: 526
	// (get) Token: 0x06000C7E RID: 3198 RVA: 0x000555E3 File Offset: 0x000537E3
	public static string audioFilePath
	{
		get
		{
			return DirectoryScript.modsFilePath + "//Audio//";
		}
	}

	// Token: 0x1700020F RID: 527
	// (get) Token: 0x06000C7F RID: 3199 RVA: 0x000555F4 File Offset: 0x000537F4
	public static string pdfFilePath
	{
		get
		{
			return DirectoryScript.modsFilePath + "//PDF//";
		}
	}

	// Token: 0x17000210 RID: 528
	// (get) Token: 0x06000C80 RID: 3200 RVA: 0x00055605 File Offset: 0x00053805
	public static string textFilePath
	{
		get
		{
			return DirectoryScript.modsFilePath + "//Text//";
		}
	}

	// Token: 0x17000211 RID: 529
	// (get) Token: 0x06000C81 RID: 3201 RVA: 0x00055616 File Offset: 0x00053816
	public static string translationsPath
	{
		get
		{
			return DirectoryScript.modsFilePath + "//Translations//";
		}
	}

	// Token: 0x06000C82 RID: 3202 RVA: 0x00055627 File Offset: 0x00053827
	public static bool IsBlacklistedFolder(string folderName)
	{
		return DirectoryScript.BlacklistedFolderNames.Contains(folderName);
	}

	// Token: 0x06000C83 RID: 3203 RVA: 0x00055634 File Offset: 0x00053834
	public static bool IsBlacklistedFile(string fileName)
	{
		return DirectoryScript.BlacklistedFileNames.Contains(fileName);
	}

	// Token: 0x17000212 RID: 530
	// (get) Token: 0x06000C84 RID: 3204 RVA: 0x00055641 File Offset: 0x00053841
	public static List<string> SavedObjectsDirectories
	{
		get
		{
			return new List<string>
			{
				DirectoryScript.savedObjectsFilePath,
				DirectoryScript.oldChestFilePath
			};
		}
	}

	// Token: 0x06000C85 RID: 3205 RVA: 0x0005565E File Offset: 0x0005385E
	private void Awake()
	{
		DirectoryScript.dataPath = Application.dataPath;
		DirectoryScript.MoveDirectoryForMacLinux();
		DirectoryScript.RenameSavedObjects();
		DirectoryScript.CheckDirectories();
		DirectoryScript.MoveThumbnails();
	}

	// Token: 0x06000C86 RID: 3206 RVA: 0x000025B8 File Offset: 0x000007B8
	private static void MoveDirectoryForMacLinux()
	{
	}

	// Token: 0x06000C87 RID: 3207 RVA: 0x00055680 File Offset: 0x00053880
	private static void RenameSavedObjects()
	{
		if (Directory.Exists(DirectoryScript.oldChestFilePath))
		{
			if (!Directory.Exists(DirectoryScript.savedObjectsFilePath))
			{
				try
				{
					Directory.Move(DirectoryScript.oldChestFilePath, DirectoryScript.savedObjectsFilePath);
					Chat.Log("Renamed folder Chest to Saved Objects.", Colour.Blue, ChatMessageType.All, true);
				}
				catch (Exception e)
				{
					Chat.LogException("rename folder Chest to Saved Objects", e, true, false);
				}
			}
			if (Directory.GetFiles(DirectoryScript.oldChestFilePath).Length == 0)
			{
				SerializationScript.Delete(DirectoryScript.oldChestFilePath);
			}
		}
	}

	// Token: 0x06000C88 RID: 3208 RVA: 0x00055700 File Offset: 0x00053900
	private static void MoveThumbnails()
	{
		if (Directory.Exists(DirectoryScript.oldsaveThumbnailsFilePath))
		{
			try
			{
				string[] files = Directory.GetFiles(DirectoryScript.oldsaveThumbnailsFilePath, "*.png", SearchOption.TopDirectoryOnly);
				int i = 0;
				while (i < files.Length)
				{
					string text = files[i].Replace(DirectoryScript.oldsaveThumbnailsFilePath, DirectoryScript.saveFilePath);
					if (!File.Exists(text))
					{
						try
						{
							File.Move(files[i], text);
							goto IL_63;
						}
						catch (Exception e)
						{
							Chat.LogException("move Save thumbnail", e, true, false);
							goto IL_63;
						}
						goto IL_5A;
					}
					goto IL_5A;
					IL_63:
					i++;
					continue;
					IL_5A:
					SerializationScript.Delete(files[i]);
					goto IL_63;
				}
				if (files.Length != 0)
				{
					Chat.Log("Moved Save thumbnails.", Colour.Blue, ChatMessageType.All, true);
				}
			}
			catch (Exception e2)
			{
				Chat.LogException("move Save thumbnails", e2, true, false);
			}
			if (Directory.GetFiles(DirectoryScript.oldsaveThumbnailsFilePath).Length == 0)
			{
				SerializationScript.Delete(DirectoryScript.oldsaveThumbnailsFilePath);
			}
		}
		if (Directory.Exists(DirectoryScript.oldworkshopThumbnailsFilePath))
		{
			try
			{
				string[] files2 = Directory.GetFiles(DirectoryScript.oldworkshopThumbnailsFilePath, "*.png", SearchOption.TopDirectoryOnly);
				int j = 0;
				while (j < files2.Length)
				{
					string text2 = files2[j].Replace(DirectoryScript.oldworkshopThumbnailsFilePath, DirectoryScript.workshopFilePath);
					if (!File.Exists(text2))
					{
						try
						{
							File.Move(files2[j], text2);
							goto IL_11E;
						}
						catch (Exception e3)
						{
							Chat.LogException("move Workshop thumbnail", e3, true, false);
							goto IL_11E;
						}
						goto IL_113;
					}
					goto IL_113;
					IL_11E:
					j++;
					continue;
					IL_113:
					SerializationScript.Delete(files2[j]);
					goto IL_11E;
				}
				if (files2.Length != 0)
				{
					Chat.Log("Moved Workshop thumbnails.", Colour.Blue, ChatMessageType.All, true);
				}
			}
			catch (Exception e4)
			{
				Chat.LogException("move Workshop thumbnails", e4, true, false);
			}
			if (Directory.GetFiles(DirectoryScript.oldworkshopThumbnailsFilePath).Length == 0)
			{
				SerializationScript.Delete(DirectoryScript.oldworkshopThumbnailsFilePath);
			}
		}
		if (Directory.Exists(DirectoryScript.oldsavedObjectsThumbnailsFilePath) && Directory.GetFiles(DirectoryScript.oldsavedObjectsThumbnailsFilePath).Length == 0)
		{
			SerializationScript.Delete(DirectoryScript.oldsavedObjectsThumbnailsFilePath);
		}
	}

	// Token: 0x06000C89 RID: 3209 RVA: 0x000558D4 File Offset: 0x00053AD4
	public static void CheckDirectories()
	{
		DirectoryScript.CheckDirectory(DirectoryScript.saveFilePath);
		DirectoryScript.CheckDirectory(DirectoryScript.savedObjectsFilePath);
		DirectoryScript.CheckDirectory(DirectoryScript.workshopFilePath);
		DirectoryScript.CheckDirectory(DirectoryScript.imageFilePath);
		DirectoryScript.CheckDirectory(DirectoryScript.imageRawFilePath);
		DirectoryScript.CheckDirectory(DirectoryScript.modelFilePath);
		DirectoryScript.CheckDirectory(DirectoryScript.modelRawFilePath);
		DirectoryScript.CheckDirectory(DirectoryScript.assetbundleFilePath);
		DirectoryScript.CheckDirectory(DirectoryScript.screenshotsFilePath);
		DirectoryScript.CheckDirectory(DirectoryScript.dlcFilePath);
		DirectoryScript.CheckDirectory(DirectoryScript.audioFilePath);
		DirectoryScript.CheckDirectory(DirectoryScript.pdfFilePath);
		DirectoryScript.CheckDirectory(DirectoryScript.textFilePath);
		DirectoryScript.CheckDirectory(DirectoryScript.translationsPath);
	}

	// Token: 0x06000C8A RID: 3210 RVA: 0x0005596D File Offset: 0x00053B6D
	public static void CheckDirectory(string filePath)
	{
		if (!Directory.Exists(filePath))
		{
			Directory.CreateDirectory(filePath);
		}
	}

	// Token: 0x06000C8B RID: 3211 RVA: 0x00055980 File Offset: 0x00053B80
	public static bool CheckSpecialChars(string text)
	{
		for (int i = 0; i < text.Length; i++)
		{
			if (text[i] > '\u007f')
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000C8C RID: 3212 RVA: 0x000559B0 File Offset: 0x00053BB0
	public static string CombinePath(string first, params string[] others)
	{
		foreach (string path in others)
		{
			first = Path.Combine(first, path);
		}
		return first;
	}

	// Token: 0x06000C8D RID: 3213 RVA: 0x000559DC File Offset: 0x00053BDC
	public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
	{
		if (string.Equals(source.FullName, target.FullName, StringComparison.CurrentCultureIgnoreCase))
		{
			return;
		}
		if (!Directory.Exists(target.FullName))
		{
			Directory.CreateDirectory(target.FullName);
		}
		foreach (FileInfo fileInfo in source.GetFiles())
		{
			Console.WriteLine("Copying {0}\\{1}", target.FullName, fileInfo.Name);
			fileInfo.CopyTo(Path.Combine(target.ToString(), fileInfo.Name), true);
		}
		foreach (DirectoryInfo directoryInfo in source.GetDirectories())
		{
			DirectoryInfo target2 = target.CreateSubdirectory(directoryInfo.Name);
			DirectoryScript.CopyAll(directoryInfo, target2);
		}
	}

	// Token: 0x04000870 RID: 2160
	public static readonly string crossPlatformPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//My Games";

	// Token: 0x04000871 RID: 2161
	public static readonly string rootPath = DirectoryScript.crossPlatformPath + "//Tabletop Simulator";

	// Token: 0x04000872 RID: 2162
	public static readonly string saveFilePath = DirectoryScript.rootPath + "//Saves";

	// Token: 0x04000873 RID: 2163
	public static readonly string savedObjectsFilePath = DirectoryScript.rootPath + "//Saves//Saved Objects";

	// Token: 0x04000874 RID: 2164
	public static readonly string blockFilePath = DirectoryScript.rootPath + "//BlockList.json";

	// Token: 0x04000875 RID: 2165
	public static readonly string graphicsFilePath = DirectoryScript.rootPath + "//Graphics.json";

	// Token: 0x04000876 RID: 2166
	public static readonly string screenshotsFilePath = DirectoryScript.rootPath + "//Screenshots";

	// Token: 0x04000877 RID: 2167
	public static readonly string autoexecFilePath = DirectoryScript.rootPath + "//autoexec.cfg";

	// Token: 0x04000878 RID: 2168
	public static readonly string bootexecFilePath = DirectoryScript.rootPath + "//bootexec.cfg";

	// Token: 0x04000879 RID: 2169
	public static readonly string dlcFilePath = DirectoryScript.rootPath + "//DLC//";

	// Token: 0x0400087A RID: 2170
	public static readonly string oldsaveThumbnailsFilePath = DirectoryScript.rootPath + "//Saves//Thumbnails";

	// Token: 0x0400087B RID: 2171
	public static readonly string oldsavedObjectsThumbnailsFilePath = DirectoryScript.rootPath + "//Saves//Saved Objects//Thumbnails";

	// Token: 0x0400087C RID: 2172
	public static readonly string oldChestFilePath = DirectoryScript.rootPath + "//Saves//Chest";

	// Token: 0x0400087E RID: 2174
	private static readonly HashSet<string> BlacklistedFolderNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
	{
		"THUMBNAILS",
		"SAVED OBJECTS",
		"SAVES",
		"WORKSHOP",
		"CHEST"
	};

	// Token: 0x0400087F RID: 2175
	private static readonly HashSet<string> BlacklistedFileNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
	{
		"SAVEFILEINFOS.JSON",
		"WORKSHOPFILEINFOS.JSON"
	};
}
