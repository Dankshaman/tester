using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NewNet;

// Token: 0x020000CE RID: 206
public static class CustomCache
{
	// Token: 0x06000A30 RID: 2608 RVA: 0x000481BC File Offset: 0x000463BC
	public static async Task SaveAsync(string path, byte[] data)
	{
		await CustomCache.SaveAsync(path, data, 0, data.Length);
	}

	// Token: 0x06000A31 RID: 2609 RVA: 0x0004820C File Offset: 0x0004640C
	public static async Task SaveAsync(string path, byte[] data, int offset, int length)
	{
		CustomCache.<>c__DisplayClass1_0 CS$<>8__locals1 = new CustomCache.<>c__DisplayClass1_0();
		CS$<>8__locals1.path = path;
		CS$<>8__locals1.data = data;
		CS$<>8__locals1.offset = offset;
		CS$<>8__locals1.length = length;
		await Task.Run(delegate()
		{
			CustomCache.<>c__DisplayClass1_0.<<SaveAsync>b__0>d <<SaveAsync>b__0>d;
			<<SaveAsync>b__0>d.<>4__this = CS$<>8__locals1;
			<<SaveAsync>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
			<<SaveAsync>b__0>d.<>1__state = -1;
			AsyncTaskMethodBuilder <>t__builder = <<SaveAsync>b__0>d.<>t__builder;
			<>t__builder.Start<CustomCache.<>c__DisplayClass1_0.<<SaveAsync>b__0>d>(ref <<SaveAsync>b__0>d);
			return <<SaveAsync>b__0>d.<>t__builder.Task;
		});
	}

	// Token: 0x06000A32 RID: 2610 RVA: 0x0004826C File Offset: 0x0004646C
	public static async Task SaveImageAsync(byte[] imageBytes, string URL, string extension)
	{
		await CustomCache.SaveAsync(DirectoryScript.imageFilePath + CustomCache.ConvertURL(URL) + extension, imageBytes);
	}

	// Token: 0x06000A33 RID: 2611 RVA: 0x000482C4 File Offset: 0x000464C4
	public static async Task SaveTextRawAsync(byte[] textRawBytes, string URL, RawTextureMetaData rawImageMetaData)
	{
		CustomCache.<>c__DisplayClass3_0 CS$<>8__locals1 = new CustomCache.<>c__DisplayClass3_0();
		CS$<>8__locals1.URL = URL;
		CS$<>8__locals1.rawImageMetaData = rawImageMetaData;
		CS$<>8__locals1.textRawBytes = textRawBytes;
		await Task.Run(delegate()
		{
			CustomCache.<>c__DisplayClass3_0.<<SaveTextRawAsync>b__0>d <<SaveTextRawAsync>b__0>d;
			<<SaveTextRawAsync>b__0>d.<>4__this = CS$<>8__locals1;
			<<SaveTextRawAsync>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
			<<SaveTextRawAsync>b__0>d.<>1__state = -1;
			AsyncTaskMethodBuilder <>t__builder = <<SaveTextRawAsync>b__0>d.<>t__builder;
			<>t__builder.Start<CustomCache.<>c__DisplayClass3_0.<<SaveTextRawAsync>b__0>d>(ref <<SaveTextRawAsync>b__0>d);
			return <<SaveTextRawAsync>b__0>d.<>t__builder.Task;
		});
	}

	// Token: 0x06000A34 RID: 2612 RVA: 0x0004831C File Offset: 0x0004651C
	public static async Task SaveModelAsync(byte[] data, string URL, string extension)
	{
		await CustomCache.SaveAsync(DirectoryScript.modelFilePath + CustomCache.ConvertURL(URL) + extension, data);
	}

	// Token: 0x06000A35 RID: 2613 RVA: 0x00048374 File Offset: 0x00046574
	public static async Task SaveModelAsync(string objText, string URL, string extension)
	{
		await CustomCache.SaveAsync(DirectoryScript.modelFilePath + CustomCache.ConvertURL(URL) + extension, Encoding.UTF8.GetBytes(objText));
	}

	// Token: 0x06000A36 RID: 2614 RVA: 0x000483CC File Offset: 0x000465CC
	public static async Task SaveModelRawAsync(byte[] rawmData, int length, string url)
	{
		await CustomCache.SaveAsync(DirectoryScript.modelRawFilePath + CustomCache.ConvertModelRawURL(url), rawmData, 0, length);
	}

	// Token: 0x06000A37 RID: 2615 RVA: 0x00048424 File Offset: 0x00046624
	public static async Task SaveModelRawAsync(RawMeshData rawMeshData, string url)
	{
		CustomCache.<>c__DisplayClass7_0 CS$<>8__locals1 = new CustomCache.<>c__DisplayClass7_0();
		CS$<>8__locals1.rawMeshData = rawMeshData;
		CS$<>8__locals1.url = url;
		CS$<>8__locals1.bitStream = NetworkManager.GetStream();
		await Task.Run(delegate()
		{
			CustomCache.<>c__DisplayClass7_0.<<SaveModelRawAsync>b__0>d <<SaveModelRawAsync>b__0>d;
			<<SaveModelRawAsync>b__0>d.<>4__this = CS$<>8__locals1;
			<<SaveModelRawAsync>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
			<<SaveModelRawAsync>b__0>d.<>1__state = -1;
			AsyncTaskMethodBuilder <>t__builder = <<SaveModelRawAsync>b__0>d.<>t__builder;
			<>t__builder.Start<CustomCache.<>c__DisplayClass7_0.<<SaveModelRawAsync>b__0>d>(ref <<SaveModelRawAsync>b__0>d);
			return <<SaveModelRawAsync>b__0>d.<>t__builder.Task;
		});
		NetworkManager.ReturnStream(CS$<>8__locals1.bitStream);
	}

	// Token: 0x06000A38 RID: 2616 RVA: 0x00048474 File Offset: 0x00046674
	public static async Task SaveAudioAsync(byte[] AudioBytes, string URL, string extension)
	{
		await CustomCache.SaveAsync(DirectoryScript.audioFilePath + CustomCache.ConvertURL(URL) + extension, AudioBytes);
	}

	// Token: 0x06000A39 RID: 2617 RVA: 0x000484CC File Offset: 0x000466CC
	public static async Task SavePDFAsync(byte[] PDFBytes, string URL, string extension)
	{
		await CustomCache.SaveAsync(DirectoryScript.pdfFilePath + CustomCache.ConvertURL(URL) + extension, PDFBytes);
	}

	// Token: 0x06000A3A RID: 2618 RVA: 0x00048524 File Offset: 0x00046724
	public static async Task SaveTextAsync(byte[] TextBytes, string URL, string extension)
	{
		await CustomCache.SaveAsync(DirectoryScript.textFilePath + CustomCache.ConvertURL(URL) + extension, TextBytes);
	}

	// Token: 0x06000A3B RID: 2619 RVA: 0x0004857C File Offset: 0x0004677C
	public static async Task SaveAssetbundleAsync(byte[] AssetbundleBytes, string URL)
	{
		await CustomCache.SaveAsync(DirectoryScript.assetbundleFilePath + CustomCache.ConvertAssetbundleURL(URL), AssetbundleBytes);
	}

	// Token: 0x06000A3C RID: 2620 RVA: 0x000485CC File Offset: 0x000467CC
	public static async Task SaveDLCSaveAsync(string text, string URL)
	{
		await CustomCache.SaveAsync(DirectoryScript.dlcFilePath + CustomCache.ConvertJsonURL(URL), Encoding.UTF8.GetBytes(text));
	}

	// Token: 0x06000A3D RID: 2621 RVA: 0x0004861C File Offset: 0x0004681C
	public static async Task SaveDLCAssetBundleAsync(byte[] data, string URL)
	{
		await CustomCache.SaveAsync(DirectoryScript.dlcFilePath + CustomCache.ConvertAssetbundleURL(URL), data);
	}

	// Token: 0x06000A3E RID: 2622 RVA: 0x00048669 File Offset: 0x00046869
	public static string ConvertURL(string URL)
	{
		return Regex.Replace(URL, "[^A-Za-z0-9]+", "");
	}

	// Token: 0x06000A3F RID: 2623 RVA: 0x0004867B File Offset: 0x0004687B
	public static string ConvertTextRawURL(string URL)
	{
		return CustomCache.ConvertURL(URL) + ".rawt";
	}

	// Token: 0x06000A40 RID: 2624 RVA: 0x0004868D File Offset: 0x0004688D
	public static string ConvertModelRawURL(string URL)
	{
		return CustomCache.ConvertURL(URL) + ".rawm";
	}

	// Token: 0x06000A41 RID: 2625 RVA: 0x0004869F File Offset: 0x0004689F
	public static string ConvertModelURL(string URL)
	{
		return CustomCache.ConvertURL(URL) + ".obj";
	}

	// Token: 0x06000A42 RID: 2626 RVA: 0x000486B1 File Offset: 0x000468B1
	public static string ConvertAssetbundleURL(string URL)
	{
		return CustomCache.ConvertURL(URL) + ".unity3d";
	}

	// Token: 0x06000A43 RID: 2627 RVA: 0x000486C3 File Offset: 0x000468C3
	public static string ConvertJsonURL(string URL)
	{
		return CustomCache.ConvertURL(URL) + ".json";
	}

	// Token: 0x06000A44 RID: 2628 RVA: 0x000486D5 File Offset: 0x000468D5
	public static CacheMode GetCacheMode()
	{
		if (ConfigGame.Settings.ConfigMods.Caching && ConfigGame.Settings.ConfigMods.RawCaching)
		{
			return CacheMode.All;
		}
		if (ConfigGame.Settings.ConfigMods.Caching)
		{
			return CacheMode.NoRawCache;
		}
		return CacheMode.None;
	}

	// Token: 0x06000A45 RID: 2629 RVA: 0x00048710 File Offset: 0x00046910
	public static string CheckImageExist(string URL, CacheMode cacheMode, bool updateAccessTime, bool fixCloudURLs = true)
	{
		if (!string.IsNullOrEmpty(URL) && cacheMode != CacheMode.None)
		{
			string text = DataRequest.TryRemoveLocalFile(URL);
			string text2 = DirectoryScript.imageFilePath + URL;
			string str = DirectoryScript.imageFilePath + CustomCache.ConvertURL(URL);
			string text3 = DirectoryScript.imageRawFilePath + CustomCache.ConvertTextRawURL(URL);
			if (File.Exists(text))
			{
				if (updateAccessTime)
				{
					SerializationScript.UpdateAccessTime(text);
				}
				if (cacheMode == CacheMode.All && File.Exists(text3) && File.GetLastWriteTimeUtc(text3) >= File.GetLastWriteTimeUtc(text))
				{
					if (updateAccessTime)
					{
						SerializationScript.UpdateAccessTime(text3);
					}
					return "file:///" + text3;
				}
				return URL;
			}
			else if (File.Exists(text2))
			{
				if (updateAccessTime)
				{
					SerializationScript.UpdateAccessTime(text2);
				}
				if (cacheMode == CacheMode.All && File.Exists(text3) && File.GetLastWriteTimeUtc(text3) >= File.GetLastWriteTimeUtc(text2))
				{
					if (updateAccessTime)
					{
						SerializationScript.UpdateAccessTime(text3);
					}
					return "file:///" + text3;
				}
				return "file:///" + text2;
			}
			else
			{
				List<string> supportedImageFormatExtensions = CustomLoadingManager.SupportedImageFormatExtensions;
				int i = 0;
				while (i < supportedImageFormatExtensions.Count)
				{
					string text4 = str + supportedImageFormatExtensions[i];
					if (File.Exists(text4))
					{
						if (updateAccessTime)
						{
							SerializationScript.UpdateAccessTime(text4);
						}
						if (cacheMode == CacheMode.All && File.Exists(text3) && File.GetLastWriteTimeUtc(text3) >= File.GetLastWriteTimeUtc(text4))
						{
							if (updateAccessTime)
							{
								SerializationScript.UpdateAccessTime(text3);
							}
							return "file:///" + text3;
						}
						return "file:///" + text4;
					}
					else
					{
						i++;
					}
				}
			}
		}
		if (fixCloudURLs)
		{
			if (SteamCloudURL.IsOldCloudURL(URL))
			{
				return CustomCache.CheckImageExist(SteamCloudURL.ConvertOldToNewCloudURL(URL), cacheMode, updateAccessTime, false);
			}
			if (SteamCloudURL.IsNewCloudURL(URL))
			{
				string text5 = CustomCache.CheckImageExist(SteamCloudURL.ConvertNewToOldCloudURL(URL), cacheMode, updateAccessTime, false);
				if (DataRequest.IsLocalFile(text5))
				{
					return text5;
				}
			}
		}
		return URL;
	}

	// Token: 0x06000A46 RID: 2630 RVA: 0x000488C0 File Offset: 0x00046AC0
	public static string CheckAudioExist(string URL, CacheMode cacheMode, bool updateAccessTime, bool fixCloudURLs = true)
	{
		if (!string.IsNullOrEmpty(URL) && cacheMode != CacheMode.None)
		{
			string text = DataRequest.TryRemoveLocalFile(URL);
			string text2 = DirectoryScript.audioFilePath + URL;
			string str = DirectoryScript.audioFilePath + CustomCache.ConvertURL(URL);
			if (File.Exists(text))
			{
				if (updateAccessTime)
				{
					SerializationScript.UpdateAccessTime(text);
				}
				return URL;
			}
			if (File.Exists(text2))
			{
				if (updateAccessTime)
				{
					SerializationScript.UpdateAccessTime(text2);
				}
				return "file:///" + text2;
			}
			foreach (string str2 in CustomLoadingManager.SupportedAudioFormatExtensions)
			{
				string text3 = str + str2;
				if (File.Exists(text3))
				{
					if (updateAccessTime)
					{
						SerializationScript.UpdateAccessTime(text3);
					}
					return "file:///" + text3;
				}
			}
		}
		if (fixCloudURLs)
		{
			if (SteamCloudURL.IsOldCloudURL(URL))
			{
				return CustomCache.CheckAudioExist(SteamCloudURL.ConvertOldToNewCloudURL(URL), cacheMode, updateAccessTime, false);
			}
			if (SteamCloudURL.IsNewCloudURL(URL))
			{
				string text4 = CustomCache.CheckAudioExist(SteamCloudURL.ConvertNewToOldCloudURL(URL), cacheMode, updateAccessTime, false);
				if (DataRequest.IsLocalFile(text4))
				{
					return text4;
				}
			}
		}
		return URL;
	}

	// Token: 0x06000A47 RID: 2631 RVA: 0x000489E0 File Offset: 0x00046BE0
	public static string CheckPDFExist(string URL, CacheMode cacheMode, bool updateAccessTime, bool fixCloudURLs = true)
	{
		if (!string.IsNullOrEmpty(URL) && cacheMode != CacheMode.None)
		{
			string text = DataRequest.TryRemoveLocalFile(URL);
			string text2 = DirectoryScript.pdfFilePath + URL;
			string str = DirectoryScript.pdfFilePath + CustomCache.ConvertURL(URL);
			if (File.Exists(text))
			{
				if (updateAccessTime)
				{
					SerializationScript.UpdateAccessTime(text);
				}
				return URL;
			}
			if (File.Exists(text2))
			{
				if (updateAccessTime)
				{
					SerializationScript.UpdateAccessTime(text2);
				}
				return "file:///" + text2;
			}
			foreach (string str2 in CustomLoadingManager.SupportedPDFFormatExtensions)
			{
				string text3 = str + str2;
				if (File.Exists(text3))
				{
					if (updateAccessTime)
					{
						SerializationScript.UpdateAccessTime(text3);
					}
					return "file:///" + text3;
				}
			}
		}
		if (fixCloudURLs)
		{
			if (SteamCloudURL.IsOldCloudURL(URL))
			{
				return CustomCache.CheckPDFExist(SteamCloudURL.ConvertOldToNewCloudURL(URL), cacheMode, updateAccessTime, false);
			}
			if (SteamCloudURL.IsNewCloudURL(URL))
			{
				string text4 = CustomCache.CheckPDFExist(SteamCloudURL.ConvertNewToOldCloudURL(URL), cacheMode, updateAccessTime, false);
				if (DataRequest.IsLocalFile(text4))
				{
					return text4;
				}
			}
		}
		return URL;
	}

	// Token: 0x06000A48 RID: 2632 RVA: 0x00048B00 File Offset: 0x00046D00
	public static string CheckTextExist(string URL, CacheMode cacheMode, bool updateAccessTime, bool fixCloudURLs = true)
	{
		if (!string.IsNullOrEmpty(URL) && cacheMode != CacheMode.None)
		{
			string text = DataRequest.TryRemoveLocalFile(URL);
			string text2 = DirectoryScript.textFilePath + URL;
			string str = DirectoryScript.textFilePath + CustomCache.ConvertURL(URL);
			if (File.Exists(text))
			{
				if (updateAccessTime)
				{
					SerializationScript.UpdateAccessTime(text);
				}
				return URL;
			}
			if (File.Exists(text2))
			{
				if (updateAccessTime)
				{
					SerializationScript.UpdateAccessTime(text2);
				}
				return "file:///" + text2;
			}
			foreach (string str2 in CustomLoadingManager.SupportedTextFormatExtensions)
			{
				string text3 = str + str2;
				if (File.Exists(text3))
				{
					if (updateAccessTime)
					{
						SerializationScript.UpdateAccessTime(text3);
					}
					return "file:///" + text3;
				}
			}
		}
		if (fixCloudURLs)
		{
			if (SteamCloudURL.IsOldCloudURL(URL))
			{
				return CustomCache.CheckTextExist(SteamCloudURL.ConvertOldToNewCloudURL(URL), cacheMode, updateAccessTime, false);
			}
			if (SteamCloudURL.IsNewCloudURL(URL))
			{
				string text4 = CustomCache.CheckTextExist(SteamCloudURL.ConvertNewToOldCloudURL(URL), cacheMode, updateAccessTime, false);
				if (DataRequest.IsLocalFile(text4))
				{
					return text4;
				}
			}
		}
		return URL;
	}

	// Token: 0x06000A49 RID: 2633 RVA: 0x00048C20 File Offset: 0x00046E20
	public static string CheckModelExist(string URL, CacheMode cacheMode, bool updateAccessTime, bool fixCloudURLs = true)
	{
		if (!string.IsNullOrEmpty(URL) && cacheMode != CacheMode.None)
		{
			string text = DataRequest.TryRemoveLocalFile(URL);
			string text2 = DirectoryScript.modelFilePath + URL;
			string str = DirectoryScript.modelFilePath + CustomCache.ConvertURL(URL);
			string text3 = DirectoryScript.modelRawFilePath + CustomCache.ConvertModelRawURL(URL);
			if (File.Exists(text))
			{
				if (updateAccessTime)
				{
					SerializationScript.UpdateAccessTime(text);
				}
				if (cacheMode == CacheMode.All && File.Exists(text3) && File.GetLastWriteTimeUtc(text3) >= File.GetLastWriteTimeUtc(text))
				{
					if (updateAccessTime)
					{
						SerializationScript.UpdateAccessTime(text3);
					}
					return "file:///" + text3;
				}
				return URL;
			}
			else if (File.Exists(text2))
			{
				if (updateAccessTime)
				{
					SerializationScript.UpdateAccessTime(text2);
				}
				if (cacheMode == CacheMode.All && File.Exists(text3) && File.GetLastWriteTimeUtc(text3) >= File.GetLastWriteTimeUtc(text2))
				{
					if (updateAccessTime)
					{
						SerializationScript.UpdateAccessTime(text3);
					}
					return "file:///" + text3;
				}
				return "file:///" + text2;
			}
			else
			{
				List<string> supportedMeshFormatExtensions = CustomLoadingManager.SupportedMeshFormatExtensions;
				int i = 0;
				while (i < supportedMeshFormatExtensions.Count)
				{
					string text4 = str + supportedMeshFormatExtensions[i];
					if (File.Exists(text4))
					{
						if (updateAccessTime)
						{
							SerializationScript.UpdateAccessTime(text4);
						}
						if (cacheMode == CacheMode.All && File.Exists(text3) && File.GetLastWriteTimeUtc(text3) >= File.GetLastWriteTimeUtc(text4))
						{
							if (updateAccessTime)
							{
								SerializationScript.UpdateAccessTime(text3);
							}
							return "file:///" + text3;
						}
						return "file:///" + text4;
					}
					else
					{
						i++;
					}
				}
			}
		}
		if (fixCloudURLs)
		{
			if (SteamCloudURL.IsOldCloudURL(URL))
			{
				return CustomCache.CheckModelExist(SteamCloudURL.ConvertOldToNewCloudURL(URL), cacheMode, updateAccessTime, false);
			}
			if (SteamCloudURL.IsNewCloudURL(URL))
			{
				string text5 = CustomCache.CheckModelExist(SteamCloudURL.ConvertNewToOldCloudURL(URL), cacheMode, updateAccessTime, false);
				if (DataRequest.IsLocalFile(text5))
				{
					return text5;
				}
			}
		}
		return URL;
	}

	// Token: 0x06000A4A RID: 2634 RVA: 0x00048DD0 File Offset: 0x00046FD0
	public static string CheckAssetbundleExist(string URL, CacheMode cacheMode, bool updateAccessTime, bool fixCloudURLs = true)
	{
		if (!string.IsNullOrEmpty(URL) && cacheMode != CacheMode.None)
		{
			string text = DataRequest.TryRemoveLocalFile(URL);
			string text2 = DirectoryScript.assetbundleFilePath + URL;
			string text3 = DirectoryScript.assetbundleFilePath + CustomCache.ConvertAssetbundleURL(URL);
			if (File.Exists(text))
			{
				if (updateAccessTime)
				{
					SerializationScript.UpdateAccessTime(text);
				}
				return URL;
			}
			if (File.Exists(text2))
			{
				if (updateAccessTime)
				{
					SerializationScript.UpdateAccessTime(text2);
				}
				return "file:///" + text2;
			}
			if (File.Exists(text3))
			{
				if (updateAccessTime)
				{
					SerializationScript.UpdateAccessTime(text3);
				}
				return "file:///" + text3;
			}
		}
		if (fixCloudURLs)
		{
			if (SteamCloudURL.IsOldCloudURL(URL))
			{
				return CustomCache.CheckAssetbundleExist(SteamCloudURL.ConvertOldToNewCloudURL(URL), cacheMode, updateAccessTime, false);
			}
			if (SteamCloudURL.IsNewCloudURL(URL))
			{
				string text4 = CustomCache.CheckAssetbundleExist(SteamCloudURL.ConvertNewToOldCloudURL(URL), cacheMode, updateAccessTime, false);
				if (DataRequest.IsLocalFile(text4))
				{
					return text4;
				}
			}
		}
		return URL;
	}

	// Token: 0x06000A4B RID: 2635 RVA: 0x00048E98 File Offset: 0x00047098
	public static string CheckDLCSaveExist(string URL, bool updateAccessTime, bool fixCloudURLs = true)
	{
		if (!string.IsNullOrEmpty(URL))
		{
			string text = DataRequest.TryRemoveLocalFile(URL);
			string text2 = DirectoryScript.dlcFilePath + URL;
			string text3 = DirectoryScript.dlcFilePath + CustomCache.ConvertJsonURL(URL);
			if (File.Exists(text))
			{
				if (updateAccessTime)
				{
					SerializationScript.UpdateAccessTime(text);
				}
				return URL;
			}
			if (File.Exists(text2))
			{
				if (updateAccessTime)
				{
					SerializationScript.UpdateAccessTime(text2);
				}
				return "file:///" + text2;
			}
			if (File.Exists(text3))
			{
				if (updateAccessTime)
				{
					SerializationScript.UpdateAccessTime(text3);
				}
				return "file:///" + text3;
			}
		}
		if (fixCloudURLs)
		{
			if (SteamCloudURL.IsOldCloudURL(URL))
			{
				return CustomCache.CheckDLCSaveExist(SteamCloudURL.ConvertOldToNewCloudURL(URL), updateAccessTime, false);
			}
			if (SteamCloudURL.IsNewCloudURL(URL))
			{
				string text4 = CustomCache.CheckDLCSaveExist(SteamCloudURL.ConvertNewToOldCloudURL(URL), updateAccessTime, false);
				if (DataRequest.IsLocalFile(text4))
				{
					return text4;
				}
			}
		}
		return URL;
	}

	// Token: 0x06000A4C RID: 2636 RVA: 0x00048F58 File Offset: 0x00047158
	public static string CheckDLCAssetBundleExist(string URL, bool updateAccessTime, bool fixCloudURLs = true)
	{
		if (!string.IsNullOrEmpty(URL))
		{
			string text = DataRequest.TryRemoveLocalFile(URL);
			string text2 = DirectoryScript.dlcFilePath + URL;
			string text3 = DirectoryScript.dlcFilePath + CustomCache.ConvertAssetbundleURL(URL);
			if (File.Exists(text))
			{
				if (updateAccessTime)
				{
					SerializationScript.UpdateAccessTime(text);
				}
				return URL;
			}
			if (File.Exists(text2))
			{
				if (updateAccessTime)
				{
					SerializationScript.UpdateAccessTime(text2);
				}
				return "file:///" + text2;
			}
			if (File.Exists(text3))
			{
				if (updateAccessTime)
				{
					SerializationScript.UpdateAccessTime(text3);
				}
				return "file:///" + text3;
			}
		}
		if (fixCloudURLs)
		{
			if (SteamCloudURL.IsOldCloudURL(URL))
			{
				return CustomCache.CheckDLCAssetBundleExist(SteamCloudURL.ConvertOldToNewCloudURL(URL), updateAccessTime, false);
			}
			if (SteamCloudURL.IsNewCloudURL(URL))
			{
				string text4 = CustomCache.CheckDLCAssetBundleExist(SteamCloudURL.ConvertNewToOldCloudURL(URL), updateAccessTime, false);
				if (DataRequest.IsLocalFile(text4))
				{
					return text4;
				}
			}
		}
		return URL;
	}

	// Token: 0x06000A4D RID: 2637 RVA: 0x00049018 File Offset: 0x00047218
	public static void DeleteRAWCache()
	{
		foreach (string path in Directory.GetFiles(DirectoryScript.imageRawFilePath, "*.rawt"))
		{
			try
			{
				File.Delete(path);
			}
			catch
			{
			}
		}
		foreach (string path2 in Directory.GetFiles(DirectoryScript.modelRawFilePath, "*.rawm"))
		{
			try
			{
				File.Delete(path2);
			}
			catch
			{
			}
		}
	}
}
