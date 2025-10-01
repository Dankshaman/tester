using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NewNet;
using NLayer;
using Paroxe.PdfRenderer;
using UnityEngine;
using UnityEngine.Video;
using VacuumShaders.TextureExtensions;

// Token: 0x020000D6 RID: 214
public class CustomLoadingManager : Singleton<CustomLoadingManager>
{
	// Token: 0x06000A8A RID: 2698 RVA: 0x0004AB70 File Offset: 0x00048D70
	private void CheckCleanupDLC(string url)
	{
		string dlcname;
		string text;
		if (DLCManager.URLisDLC(url, out dlcname, out text))
		{
			using (List<CustomLoadingManager.ICustomLoadingAsset>.Enumerator enumerator = this.CustomLoadingAssets.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsDLCLoaded(dlcname))
					{
						return;
					}
				}
			}
			Singleton<DLCManager>.Instance.CleanupDLC(dlcname, true);
		}
	}

	// Token: 0x06000A8B RID: 2699 RVA: 0x0004ABE0 File Offset: 0x00048DE0
	private bool GetAvailableWWW()
	{
		if (this.current_WWW < CustomLoadingManager.MAX_NUMBER_WWW)
		{
			this.current_WWW++;
			return true;
		}
		return false;
	}

	// Token: 0x06000A8C RID: 2700 RVA: 0x0004AC00 File Offset: 0x00048E00
	private void ReleaseWWW()
	{
		this.current_WWW--;
		if (this.current_WWW < 0)
		{
			this.current_WWW = 0;
		}
	}

	// Token: 0x06000A8D RID: 2701 RVA: 0x0004AC20 File Offset: 0x00048E20
	private async Task<Dictionary<string, string>> GetHeaders(string url)
	{
		Dictionary<string, string> result;
		try
		{
			HttpWebRequest httpWebRequest = WebRequest.CreateHttp((!url.StartsWith("http")) ? ("http://" + url) : url);
			httpWebRequest.Method = "HEAD";
			WebResponse webResponse = await httpWebRequest.GetResponseAsync();
			Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			for (int i = 0; i < webResponse.Headers.Count; i++)
			{
				string[] values = webResponse.Headers.GetValues(i);
				if (values.Length != 0)
				{
					dictionary.Add(webResponse.Headers.GetKey(i), values[0]);
				}
			}
			result = dictionary;
		}
		catch (Exception arg)
		{
			Debug.LogError(url + " : " + arg);
			result = null;
		}
		return result;
	}

	// Token: 0x06000A8E RID: 2702 RVA: 0x0004AC68 File Offset: 0x00048E68
	private IEnumerator CoroutineLoadTexture(string url, bool checkModifiedHeader, CustomLoadingManager.LoadType loadType, int imageMaxSize, bool mipMaps, bool addLoading, bool linear, bool compress, bool normalMap, bool readable)
	{
		normalMap = false;
		Texture2D texture = null;
		CustomTextureContainer customTextureContainer = new CustomTextureContainer(url, null, 1f);
		if (imageMaxSize > SystemInfo.maxTextureSize)
		{
			imageMaxSize = SystemInfo.maxTextureSize;
		}
		bool UseThreading = (loadType == CustomLoadingManager.LoadType.Auto && ConfigGame.Settings.ConfigMods.Threading) || loadType == CustomLoadingManager.LoadType.Async;
		if (normalMap)
		{
			linear = true;
			compress = false;
		}
		if (addLoading)
		{
			Singleton<UILoading>.Instance.AddLoading();
		}
		string DLCName;
		string DLCUrl;
		if (DLCManager.URLisDLC(url, out DLCName, out DLCUrl))
		{
			customTextureContainer = null;
			while (customTextureContainer == null && this.Texture.ContainsURL(url))
			{
				customTextureContainer = Singleton<DLCManager>.Instance.LoadDLCTexture(DLCName, DLCUrl, url);
				yield return null;
			}
			if (addLoading)
			{
				Singleton<UILoading>.Instance.RemoveLoading();
			}
			this.Texture.Finished(customTextureContainer);
			yield break;
		}
		while (!this.GetAvailableWWW())
		{
			yield return null;
		}
		string ConvertedURL = CustomCache.CheckImageExist(url, CustomCache.GetCacheMode(), true, true);
		if (checkModifiedHeader && url != ConvertedURL && !DataRequest.IsLocalFile(url) && DataRequest.IsLocalFile(ConvertedURL) && !CustomLoadingManager.UrlHeaders.ContainsKey(url))
		{
			Task<Dictionary<string, string>> task = this.GetHeaders(url);
			while (!task.IsCompleted)
			{
				yield return null;
			}
			Dictionary<string, string> result = task.Result;
			CustomLoadingManager.UrlHeaders.Add(url, result);
			string s;
			DateTime dateTime;
			if (result != null && result.TryGetValue("Last-Modified", out s) && DateTime.TryParse(s, out dateTime))
			{
				DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(DataRequest.TryRemoveLocalFile(CustomCache.CheckImageExist(url, CacheMode.NoRawCache, false, true)));
				if (dateTime.ToUniversalTime() > lastWriteTimeUtc)
				{
					Debug.Log(string.Concat(new object[]
					{
						"Cache is stale: ",
						url,
						" server: ",
						dateTime.ToUniversalTime(),
						" file: ",
						lastWriteTimeUtc
					}));
					ConvertedURL = url;
				}
			}
			task = null;
		}
		FileMagicNumbers.FileFormat fileFormat = FileMagicNumbers.GetFileFormat(ConvertedURL);
		bool flag = DataRequest.IsLocalFile(ConvertedURL) && CustomLoadingManager.SupportedVideoPlayerFormats.Contains(fileFormat);
		DataRequest dataRequest = null;
		if (!flag)
		{
			dataRequest = DataRequest.Get(ConvertedURL, null);
			while (!dataRequest.isDone)
			{
				yield return null;
			}
		}
		bool LoadSuccessful = false;
		RawTextureMetaData MetaData = null;
		int RawByteOffset = 0;
		byte[] wwwBytes = null;
		bool isError = false;
		string errorMessage = null;
		if (dataRequest != null)
		{
			wwwBytes = dataRequest.data;
			fileFormat = FileMagicNumbers.GetFileFormat(wwwBytes);
			if (fileFormat == FileMagicNumbers.FileFormat.RAWT)
			{
				try
				{
					BitStream bitStream = new BitStream(wwwBytes);
					bitStream.ReadUint();
					string json = bitStream.ReadString();
					RawByteOffset = bitStream.ByteIndex;
					MetaData = Json.Load<RawTextureMetaData>(json);
				}
				catch (Exception e)
				{
					Chat.LogException("load .rawt header [" + url + "]", e, true, false);
				}
				if (MetaData == null || compress != CustomLoadingManager.CompressedFormats.Contains(MetaData.format) || normalMap != MetaData.normal_map || (mipMaps && !MetaData.mip_maps) || (imageMaxSize > MetaData.max_size && (MetaData.max_size == MetaData.width_new || MetaData.max_size == MetaData.height_new)))
				{
					string text2 = CustomCache.CheckImageExist(url, CacheMode.NoRawCache, false, true);
					if (text2 != ConvertedURL)
					{
						Debug.Log("RAW cache texture settings do not match! Loading regular cache!");
						ConvertedURL = text2;
						fileFormat = FileMagicNumbers.GetFileFormat(ConvertedURL);
						MetaData = null;
						dataRequest.Dispose();
						dataRequest = DataRequest.Get(ConvertedURL, null);
						while (!dataRequest.isDone)
						{
							yield return null;
						}
						wwwBytes = dataRequest.data;
						fileFormat = FileMagicNumbers.GetFileFormat(wwwBytes);
					}
					else
					{
						Debug.Log("RAW textures settings do not match! This might produce different a texture then requested!");
					}
				}
			}
			if (dataRequest.isError)
			{
				isError = dataRequest.isError;
				errorMessage = dataRequest.error;
			}
		}
		if (dataRequest != null)
		{
			dataRequest.Dispose();
		}
		this.ReleaseWWW();
		RawTextureMetaData rawMetaData = null;
		byte[] rawTextData = null;
		if (this.Texture.ContainsURL(url))
		{
			if (isError)
			{
				Chat.LogError(string.Concat(new string[]
				{
					"WWW Image Error: ",
					errorMessage,
					"\n   at [",
					url,
					"]"
				}), true);
			}
			else if (!CustomLoadingManager.SupportedImageFormats.Contains(fileFormat))
			{
				Chat.LogError(string.Format("Load image failed unsupported format: {0}\n    Supported formats: {1}\n    at [{2}]", fileFormat, CustomLoadingManager.GetSupportedImageFormatExtensionsString(), url), true);
			}
			else if (fileFormat == FileMagicNumbers.FileFormat.RAWT)
			{
				if (MetaData != null)
				{
					yield return null;
					texture = new Texture2D(MetaData.width_new, MetaData.height_new, MetaData.format, MetaData.mip_maps, linear);
					TextureScript.ApplyTextSettings(texture);
					yield return null;
					GCHandle gchandle = GCHandle.Alloc(wwwBytes, GCHandleType.Pinned);
					IntPtr data = new IntPtr(gchandle.AddrOfPinnedObject().ToInt64() + (long)RawByteOffset);
					try
					{
						texture.LoadRawTextureData(data, wwwBytes.Length - RawByteOffset);
						texture.Apply(false, !readable);
						LoadSuccessful = true;
						customTextureContainer = new CustomTextureContainer(url, texture, (float)MetaData.width_original / (float)MetaData.height_original);
					}
					catch (Exception e2)
					{
						Chat.LogException("loading cached raw texture [" + url + "]", e2, true, false);
					}
					gchandle.Free();
				}
			}
			else if (fileFormat == FileMagicNumbers.FileFormat.ASSETBUNDLE)
			{
				AssetBundle assetBundle = null;
				Texture text = null;
				AssetBundleCreateRequest bundleCreateRequest = AssetBundle.LoadFromMemoryAsync(wwwBytes);
				yield return bundleCreateRequest;
				try
				{
					assetBundle = bundleCreateRequest.assetBundle;
				}
				catch (Exception e3)
				{
					Chat.LogException("Loading Image AssetBundle Material [" + url + "]", e3, true, false);
				}
				if (assetBundle != null)
				{
					AssetBundleRequest assetBundleRequest = assetBundle.LoadAllAssetsAsync<Material>();
					yield return assetBundleRequest;
					Material[] array = Array.ConvertAll<UnityEngine.Object, Material>(assetBundleRequest.allAssets, (UnityEngine.Object item) => (Material)item);
					if (array.Length != 0)
					{
						Material material = array[0];
						if (material.HasProperty("_MainTex"))
						{
							text = material.mainTexture;
							if (text != null)
							{
								if (text.filterMode != FilterMode.Point)
								{
									TextureScript.ApplyTextSettings(text);
								}
								customTextureContainer = new CustomTextureContainer(url, text, (float)text.width / (float)text.height, assetBundle, material);
								LoadSuccessful = true;
							}
							else
							{
								Chat.LogError("Image AssetBundle Material main texture is null: " + url, true);
							}
						}
						else
						{
							Chat.LogError("Image AssetBundle Material does not contain a main texture: " + url, true);
						}
					}
					else
					{
						Chat.LogError("Image AssetBundle does not contain any materials: " + url, true);
					}
					assetBundleRequest = null;
				}
				if (!LoadSuccessful)
				{
					if (assetBundle != null)
					{
						assetBundle.Unload(true);
					}
					UnityEngine.Object.Destroy(text);
				}
				assetBundle = null;
				text = null;
				bundleCreateRequest = null;
			}
			else if (CustomLoadingManager.SupportedVideoPlayerFormats.Contains(fileFormat))
			{
				VideoPlayer videoPlayer = base.gameObject.AddChild<VideoPlayer>();
				videoPlayer.url = DataRequest.TryRemoveLocalFile(ConvertedURL);
				videoPlayer.renderMode = VideoRenderMode.RenderTexture;
				videoPlayer.isLooping = true;
				videoPlayer.playOnAwake = false;
				videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
				videoPlayer.SetDirectAudioVolume(0, 0f);
				videoPlayer.SetDirectAudioMute(0, true);
				videoPlayer.errorReceived += delegate(VideoPlayer vp, string message)
				{
					Chat.LogError(string.Format("Error Video Player: {0}\n    Format: {1}\n   at [{2}]", message, fileFormat, url), true);
					if (vp)
					{
						if (vp.targetTexture)
						{
							vp.targetTexture.Release();
						}
						UnityEngine.Object.Destroy(vp.gameObject);
					}
				};
				videoPlayer.Prepare();
				float timeOutTime = Time.time + 30f;
				while (videoPlayer && !videoPlayer.isPrepared)
				{
					yield return null;
					if (Time.time > timeOutTime)
					{
						Chat.LogError(string.Format("Video Player creation has timed out.\n    Format: {0}\n    at [{1}]", fileFormat, url), true);
						if (videoPlayer)
						{
							UnityEngine.Object.Destroy(videoPlayer.gameObject);
							videoPlayer = null;
							break;
						}
						break;
					}
				}
				if (videoPlayer)
				{
					RenderTexture renderTexture = new RenderTexture(videoPlayer.texture.width, videoPlayer.texture.height, 0)
					{
						useMipMap = true
					};
					videoPlayer.targetTexture = renderTexture;
					videoPlayer.Play();
					LoadSuccessful = true;
					customTextureContainer = new CustomTextureContainer(url, renderTexture, (float)renderTexture.width / (float)renderTexture.height, videoPlayer);
				}
				videoPlayer = null;
			}
			else if (!UseThreading)
			{
				texture = new Texture2D(4, 4, compress ? TextureFormat.DXT1 : TextureFormat.RGB24, mipMaps, linear);
				TextureScript.ApplyTextSettings(texture);
				yield return null;
				bool markNonReadable = !normalMap;
				LoadSuccessful = texture.LoadImage(wwwBytes, markNonReadable);
				if (!LoadSuccessful)
				{
					this.LogImageError(url);
				}
				else
				{
					yield return null;
					if (normalMap)
					{
						texture = this.ConvertToNormalMap(texture);
						yield return null;
					}
					customTextureContainer = new CustomTextureContainer(url, texture, (float)texture.width / (float)texture.height);
					if (!markNonReadable && !readable)
					{
						texture.Apply(false, true);
					}
				}
			}
			else
			{
				ImageLoaderJob imageJob = new ImageLoaderJob
				{
					InRawData = wwwBytes,
					image_max_size = imageMaxSize,
					compress = compress,
					mipMaps = mipMaps,
					normalMap = normalMap
				};
				imageJob.Start(true, false);
				while (!imageJob.Update())
				{
					yield return null;
				}
				byte[] OutRawData = imageJob.OutRawData;
				TextureFormat format = imageJob.format;
				int width_original = imageJob.width_original;
				int height_original = imageJob.height_original;
				int width_new = imageJob.width_new;
				int height_new = imageJob.height_new;
				bool isError2 = imageJob.isError;
				string errorMessage2 = imageJob.errorMessage;
				if (this.Texture.ContainsURL(url))
				{
					if (isError2)
					{
						Chat.LogError(string.Concat(new string[]
						{
							"Async loading images: ",
							errorMessage2,
							"\n    at[",
							url,
							"]"
						}), true);
					}
					else
					{
						texture = new Texture2D(width_new, height_new, format, mipMaps && width_new > 2 && height_new > 2, linear);
						TextureScript.ApplyTextSettings(texture);
						yield return null;
						try
						{
							texture.LoadRawTextureData(OutRawData);
							texture.Apply(false, !readable);
							if (ConfigGame.Settings.ConfigMods.RawCaching)
							{
								rawTextData = OutRawData;
								rawMetaData = new RawTextureMetaData(format, width_original, height_original, width_new, height_new, texture.mipmapCount > 1, normalMap, imageMaxSize);
							}
							LoadSuccessful = true;
							customTextureContainer = new CustomTextureContainer(url, texture, (float)width_original / (float)height_original);
						}
						catch (Exception e4)
						{
							Chat.LogException("loading raw data into texture [" + url + "]", e4, true, false);
						}
					}
				}
				imageJob = null;
				OutRawData = null;
			}
		}
		if (LoadSuccessful)
		{
			if (!DataRequest.IsLocalFile(ConvertedURL))
			{
				Task task2 = CustomCache.SaveImageAsync(wwwBytes, url, FileMagicNumbers.GetExtension(fileFormat));
				while (!task2.IsCompleted)
				{
					yield return null;
				}
				task2 = null;
			}
			if (rawTextData != null)
			{
				Task task2 = CustomCache.SaveTextRawAsync(rawTextData, url, rawMetaData);
				while (!task2.IsCompleted)
				{
					yield return null;
				}
				task2 = null;
			}
		}
		else
		{
			UnityEngine.Object.Destroy(texture);
		}
		if (addLoading)
		{
			Singleton<UILoading>.Instance.RemoveLoading();
		}
		this.Texture.Finished(customTextureContainer);
		yield break;
	}

	// Token: 0x06000A8F RID: 2703 RVA: 0x0004ACD0 File Offset: 0x00048ED0
	private Texture2D ConvertToNormalMap(Texture2D source)
	{
		Texture2D texture2D = new Texture2D(source.width, source.height, TextureFormat.ARGB32, source.mipmapCount > 0, true);
		TextureScript.ApplyTextSettings(texture2D);
		for (int i = 0; i < source.width; i++)
		{
			for (int j = 0; j < source.height; j++)
			{
				Color pixel = source.GetPixel(i, j);
				pixel.a = pixel.r;
				pixel.r = 0f;
				pixel.b = 0f;
				texture2D.SetPixel(i, j, pixel);
			}
		}
		texture2D.Apply(true, false);
		UnityEngine.Object.Destroy(source);
		return texture2D;
	}

	// Token: 0x06000A90 RID: 2704 RVA: 0x0004AD68 File Offset: 0x00048F68
	private IEnumerator CoroutineLoadAudio(string url, bool checkModifiedHeader, CustomLoadingManager.LoadType loadType, bool addLoading, string audioClipName)
	{
		AudioClip audioClip = null;
		CustomAudioContainer customAudioContainer = new CustomAudioContainer(url, null);
		if (addLoading)
		{
			Singleton<UILoading>.Instance.AddLoading();
		}
		string DLCName;
		string DLCUrl;
		if (DLCManager.URLisDLC(url, out DLCName, out DLCUrl))
		{
			customAudioContainer = null;
			while (customAudioContainer == null && this.Audio.ContainsURL(url))
			{
				customAudioContainer = Singleton<DLCManager>.Instance.LoadDLCAudio(DLCName, DLCUrl, url);
				yield return null;
			}
			if (addLoading)
			{
				Singleton<UILoading>.Instance.RemoveLoading();
			}
			this.Audio.Finished(customAudioContainer);
			yield break;
		}
		while (!this.GetAvailableWWW())
		{
			yield return null;
		}
		string convertedUrl = CustomCache.CheckAudioExist(url, CustomCache.GetCacheMode(), true, true);
		if (checkModifiedHeader && url != convertedUrl && !DataRequest.IsLocalFile(url) && DataRequest.IsLocalFile(convertedUrl) && !CustomLoadingManager.UrlHeaders.ContainsKey(url))
		{
			Task<Dictionary<string, string>> task = this.GetHeaders(url);
			while (!task.IsCompleted)
			{
				yield return null;
			}
			Dictionary<string, string> result = task.Result;
			CustomLoadingManager.UrlHeaders.Add(url, result);
			string s;
			DateTime dateTime;
			if (result != null && result.TryGetValue("Last-Modified", out s) && DateTime.TryParse(s, out dateTime))
			{
				DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(DataRequest.TryRemoveLocalFile(CustomCache.CheckAudioExist(url, CacheMode.NoRawCache, false, true)));
				if (dateTime.ToUniversalTime() > lastWriteTimeUtc)
				{
					Debug.Log(string.Concat(new object[]
					{
						"Cache is stale: ",
						url,
						" server: ",
						dateTime.ToUniversalTime(),
						" file: ",
						lastWriteTimeUtc
					}));
					convertedUrl = url;
				}
			}
			task = null;
		}
		bool loadSuccessful = false;
		DataRequest dataRequest = null;
		FileMagicNumbers.FileFormat fileFormat = FileMagicNumbers.GetFileFormat(convertedUrl);
		if (fileFormat == FileMagicNumbers.FileFormat.UNKNOWN || !DataRequest.IsLocalFile(convertedUrl))
		{
			dataRequest = DataRequest.Get(convertedUrl, null);
			while (!dataRequest.isDone)
			{
				yield return null;
			}
			this.ReleaseWWW();
			if (dataRequest.isError)
			{
				Chat.LogError(string.Concat(new string[]
				{
					"WWW AudioClip Error: ",
					dataRequest.error,
					"\n    at[",
					url,
					"]"
				}), true);
				dataRequest.Dispose();
				if (addLoading)
				{
					Singleton<UILoading>.Instance.RemoveLoading();
				}
				this.Audio.Finished(new CustomAudioContainer(url, null));
				yield break;
			}
			fileFormat = FileMagicNumbers.GetFileFormat(dataRequest.data);
			if (!CustomLoadingManager.SupportedAudioFormats.Contains(fileFormat))
			{
				Chat.LogError("AudioClip Error: Unsupported File Format!\n    at [" + url + "]", true);
				dataRequest.Dispose();
				if (addLoading)
				{
					Singleton<UILoading>.Instance.RemoveLoading();
				}
				this.Audio.Finished(new CustomAudioContainer(url, null));
				yield break;
			}
			if (!DataRequest.IsLocalFile(convertedUrl))
			{
				Task task2 = CustomCache.SaveAudioAsync(dataRequest.data, url, "." + fileFormat);
				while (!task2.IsCompleted)
				{
					yield return null;
				}
				task2 = null;
			}
		}
		string localPath = CustomCache.CheckAudioExist(string.Concat(new object[]
		{
			DirectoryScript.audioFilePath,
			CustomCache.ConvertURL(url),
			".",
			fileFormat
		}), CustomCache.GetCacheMode(), true, true);
		if (fileFormat == FileMagicNumbers.FileFormat.MP3)
		{
			if (dataRequest == null)
			{
				dataRequest = DataRequest.Get(convertedUrl, null);
				while (!dataRequest.isDone)
				{
					yield return null;
				}
				this.ReleaseWWW();
				if (dataRequest.isError)
				{
					Chat.LogError(string.Concat(new string[]
					{
						"WWW AudioClip Error: ",
						dataRequest.error,
						"\n    at [",
						url,
						"]"
					}), true);
					dataRequest.Dispose();
					if (addLoading)
					{
						Singleton<UILoading>.Instance.RemoveLoading();
					}
					this.Audio.Finished(new CustomAudioContainer(url, null));
					yield break;
				}
			}
			try
			{
				audioClipName = CustomLoadingManager.GetAudioTag(audioClipName, dataRequest.data);
				Stream stream = new MemoryStream(dataRequest.data);
				MpegFile mpegFile = new MpegFile(stream);
				int lengthSamples = (int)(mpegFile.Length / 4L / (long)mpegFile.Channels);
				audioClip = AudioClip.Create(audioClipName, lengthSamples, mpegFile.Channels, mpegFile.SampleRate, true, delegate(float[] data)
				{
					mpegFile.ReadSamples(data, 0, data.Length);
				}, delegate(int position)
				{
					mpegFile.Position = (long)(position * mpegFile.Channels * 4);
				});
			}
			catch (Exception e)
			{
				Chat.LogException("loading AudioClip [" + url + "]", e, true, false);
			}
			if (audioClip != null)
			{
				loadSuccessful = true;
			}
		}
		else
		{
			DataRequest dataRequest2 = dataRequest;
			if (dataRequest2 != null)
			{
				dataRequest2.Dispose();
			}
			yield return 0;
			AudioType audioType = AudioType.UNKNOWN;
			if (fileFormat == FileMagicNumbers.FileFormat.OGV || fileFormat == FileMagicNumbers.FileFormat.OGG)
			{
				audioType = AudioType.OGGVORBIS;
			}
			else if (fileFormat == FileMagicNumbers.FileFormat.WAV)
			{
				audioType = AudioType.WAV;
			}
			if (audioType == AudioType.UNKNOWN)
			{
				Chat.LogError("AudioClip Error: Unsupported File Format!\n    at [" + url + "]", true);
				if (addLoading)
				{
					Singleton<UILoading>.Instance.RemoveLoading();
				}
				this.Audio.Finished(new CustomAudioContainer(url, null));
				yield break;
			}
			dataRequest = DataRequest.GetAudio((!File.Exists(localPath)) ? convertedUrl : localPath, audioType);
			while (!dataRequest.isDone)
			{
				yield return null;
			}
			this.ReleaseWWW();
			if (dataRequest.isError)
			{
				Chat.LogError(string.Concat(new string[]
				{
					"WWW AudioClip Error: ",
					dataRequest.error,
					"\n    at [",
					url,
					"]"
				}), true);
			}
			else
			{
				try
				{
					audioClip = dataRequest.AudioClip;
				}
				catch (Exception e2)
				{
					Chat.LogException("loading AudioClip [" + url + "]", e2, true, false);
				}
				if (audioClip != null)
				{
					audioClipName = CustomLoadingManager.GetAudioTag(audioClipName, dataRequest.data);
					audioClip.name = audioClipName;
					loadSuccessful = true;
				}
			}
		}
		if (!loadSuccessful && File.Exists(localPath))
		{
			File.Delete(localPath);
		}
		if (addLoading)
		{
			Singleton<UILoading>.Instance.RemoveLoading();
		}
		dataRequest.Dispose();
		this.Audio.Finished(new CustomAudioContainer(url, audioClip));
		yield break;
	}

	// Token: 0x06000A91 RID: 2705 RVA: 0x0004AD98 File Offset: 0x00048F98
	private static string GetAudioTag(string audioClipName, byte[] data)
	{
		if (audioClipName != "Unknown Title" || data.Length <= 128)
		{
			return audioClipName;
		}
		int num = data.Length - 126;
		int num2 = Math.Max(data.Length - 256, 0);
		while (num > num2 && data[num] != 71 && data[num - 1] != 65 && data[num - 2] != 84)
		{
			num--;
		}
		if (num > num2)
		{
			num++;
			string @string = Encoding.UTF8.GetString(data, num, 30);
			audioClipName = Encoding.UTF8.GetString(data, num + 30, 30) + " - " + @string;
		}
		return audioClipName;
	}

	// Token: 0x06000A92 RID: 2706 RVA: 0x0004AE2E File Offset: 0x0004902E
	private IEnumerator CoroutineLoadPDF(string url, bool checkModifiedHeader, CustomLoadingManager.LoadType loadType, bool addLoading, string password)
	{
		PDFDocument pdfDocument = null;
		CustomPDFContainer customContainer = new CustomPDFContainer(url, null);
		if (addLoading)
		{
			Singleton<UILoading>.Instance.AddLoading();
		}
		string text;
		string text2;
		if (DLCManager.URLisDLC(url, out text, out text2))
		{
			while (pdfDocument == null && this.PDF.ContainsURL(url))
			{
				pdfDocument = new PDFDocument("");
				yield return null;
			}
			customContainer = new CustomPDFContainer(url, pdfDocument);
			if (addLoading)
			{
				Singleton<UILoading>.Instance.RemoveLoading();
			}
			this.PDF.Finished(customContainer);
			yield break;
		}
		while (!this.GetAvailableWWW())
		{
			yield return null;
		}
		string convertedUrl = CustomCache.CheckPDFExist(url, CustomCache.GetCacheMode(), true, true);
		if (checkModifiedHeader && url != convertedUrl && !DataRequest.IsLocalFile(url) && DataRequest.IsLocalFile(convertedUrl) && !CustomLoadingManager.UrlHeaders.ContainsKey(url))
		{
			Task<Dictionary<string, string>> task = this.GetHeaders(url);
			while (!task.IsCompleted)
			{
				yield return null;
			}
			Dictionary<string, string> result = task.Result;
			CustomLoadingManager.UrlHeaders.Add(url, result);
			string s;
			DateTime dateTime;
			if (result != null && result.TryGetValue("Last-Modified", out s) && DateTime.TryParse(s, out dateTime))
			{
				DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(DataRequest.TryRemoveLocalFile(CustomCache.CheckPDFExist(url, CacheMode.NoRawCache, false, true)));
				if (dateTime.ToUniversalTime() > lastWriteTimeUtc)
				{
					Debug.Log(string.Concat(new object[]
					{
						"Cache is stale: ",
						url,
						" server: ",
						dateTime.ToUniversalTime(),
						" file: ",
						lastWriteTimeUtc
					}));
					convertedUrl = url;
				}
			}
			task = null;
		}
		DataRequest dataRequest = null;
		dataRequest = DataRequest.Get(convertedUrl, null);
		while (!dataRequest.isDone)
		{
			yield return null;
		}
		this.ReleaseWWW();
		if (dataRequest.isError)
		{
			Chat.LogError(string.Concat(new string[]
			{
				"WWW PDF Error: ",
				dataRequest.error,
				"\n    at [",
				url,
				"]"
			}), true);
			dataRequest.Dispose();
			if (addLoading)
			{
				Singleton<UILoading>.Instance.RemoveLoading();
			}
			this.PDF.Finished(new CustomPDFContainer(url, null));
			yield break;
		}
		FileMagicNumbers.FileFormat fileFormat = FileMagicNumbers.GetFileFormat(dataRequest.data);
		if (!CustomLoadingManager.SupportedPDFFormats.Contains(fileFormat))
		{
			Chat.LogError("PDF Error: Unsupported File Format!\n    at [" + url + "]", true);
			dataRequest.Dispose();
			if (addLoading)
			{
				Singleton<UILoading>.Instance.RemoveLoading();
			}
			this.PDF.Finished(new CustomPDFContainer(url, null));
			yield break;
		}
		try
		{
			pdfDocument = new PDFDocument(dataRequest.data, password);
		}
		catch (Exception message)
		{
			Debug.Log(message);
			dataRequest.Dispose();
			if (addLoading)
			{
				Singleton<UILoading>.Instance.RemoveLoading();
			}
			this.PDF.Finished(new CustomPDFContainer(url, null));
			yield break;
		}
		if (!DataRequest.IsLocalFile(convertedUrl))
		{
			Task task2 = CustomCache.SavePDFAsync(dataRequest.data, url, "." + fileFormat);
			while (!task2.IsCompleted)
			{
				yield return null;
			}
			task2 = null;
		}
		if (addLoading)
		{
			Singleton<UILoading>.Instance.RemoveLoading();
		}
		dataRequest.Dispose();
		this.PDF.Finished(new CustomPDFContainer(url, pdfDocument));
		yield break;
	}

	// Token: 0x06000A93 RID: 2707 RVA: 0x0004AE5B File Offset: 0x0004905B
	private IEnumerator CoroutineLoadText(string url, bool checkModifiedHeader, CustomLoadingManager.LoadType loadType, bool addLoading, bool forceFetch)
	{
		string text = null;
		CustomTextContainer customContainer = new CustomTextContainer(url, null);
		if (addLoading)
		{
			Singleton<UILoading>.Instance.AddLoading();
		}
		string text2;
		string text3;
		if (DLCManager.URLisDLC(url, out text2, out text3))
		{
			while (text == null && this.Text.ContainsURL(url))
			{
				text = "";
				yield return null;
			}
			customContainer = new CustomTextContainer(url, text);
			if (addLoading)
			{
				Singleton<UILoading>.Instance.RemoveLoading();
			}
			this.Text.Finished(customContainer);
			yield break;
		}
		while (!this.GetAvailableWWW())
		{
			yield return null;
		}
		string convertedUrl;
		if (forceFetch)
		{
			convertedUrl = url;
		}
		else
		{
			convertedUrl = CustomCache.CheckTextExist(url, CustomCache.GetCacheMode(), true, true);
			if (checkModifiedHeader && url != convertedUrl && !DataRequest.IsLocalFile(url) && DataRequest.IsLocalFile(convertedUrl) && !CustomLoadingManager.UrlHeaders.ContainsKey(url))
			{
				Task<Dictionary<string, string>> task = this.GetHeaders(url);
				while (!task.IsCompleted)
				{
					yield return null;
				}
				Dictionary<string, string> result = task.Result;
				CustomLoadingManager.UrlHeaders.Add(url, result);
				string s;
				DateTime dateTime;
				if (result != null && result.TryGetValue("Last-Modified", out s) && DateTime.TryParse(s, out dateTime))
				{
					DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(DataRequest.TryRemoveLocalFile(CustomCache.CheckPDFExist(url, CacheMode.NoRawCache, false, true)));
					if (dateTime.ToUniversalTime() > lastWriteTimeUtc)
					{
						Debug.Log(string.Concat(new object[]
						{
							"Cache is stale: ",
							url,
							" server: ",
							dateTime.ToUniversalTime(),
							" file: ",
							lastWriteTimeUtc
						}));
						convertedUrl = url;
					}
				}
				task = null;
			}
		}
		DataRequest dataRequest = null;
		dataRequest = DataRequest.Get(convertedUrl, null);
		while (!dataRequest.isDone)
		{
			yield return null;
		}
		this.ReleaseWWW();
		if (dataRequest.isError)
		{
			Chat.LogError(string.Concat(new string[]
			{
				"WWW TXT Error: ",
				dataRequest.error,
				"\n    at [",
				url,
				"]"
			}), true);
			dataRequest.Dispose();
			if (addLoading)
			{
				Singleton<UILoading>.Instance.RemoveLoading();
			}
			this.Text.Finished(new CustomTextContainer(url, null));
			yield break;
		}
		FileMagicNumbers.FileFormat fileFormat = FileMagicNumbers.FileFormat.TXT;
		try
		{
			if (DataRequest.IsLocalFile(convertedUrl))
			{
				text = Encoding.UTF8.GetString(LibString.DecompressBytes(dataRequest.data));
			}
			else
			{
				text = Encoding.UTF8.GetString(dataRequest.data);
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
			dataRequest.Dispose();
			if (addLoading)
			{
				Singleton<UILoading>.Instance.RemoveLoading();
			}
			this.Text.Finished(new CustomTextContainer(url, null));
			yield break;
		}
		if (!DataRequest.IsLocalFile(convertedUrl))
		{
			Task task2 = CustomCache.SaveTextAsync(LibString.CompressBytes(dataRequest.data), url, "." + fileFormat);
			while (!task2.IsCompleted)
			{
				yield return null;
			}
			task2 = null;
		}
		if (addLoading)
		{
			Singleton<UILoading>.Instance.RemoveLoading();
		}
		dataRequest.Dispose();
		this.Text.Finished(new CustomTextContainer(url, text));
		yield break;
	}

	// Token: 0x06000A94 RID: 2708 RVA: 0x0004AE88 File Offset: 0x00049088
	private IEnumerator CoroutineLoadMesh(string url, bool checkModifiedHeader, CustomLoadingManager.LoadType loadType, bool addLoading)
	{
		Mesh mesh = null;
		CustomMeshContainer customMeshContainer = new CustomMeshContainer(url, null);
		bool UseThreading = (loadType == CustomLoadingManager.LoadType.Auto && ConfigGame.Settings.ConfigMods.Threading) || loadType == CustomLoadingManager.LoadType.Async;
		if (addLoading)
		{
			Singleton<UILoading>.Instance.AddLoading();
		}
		string DLCName;
		string DLCUrl;
		if (DLCManager.URLisDLC(url, out DLCName, out DLCUrl))
		{
			customMeshContainer = null;
			while (customMeshContainer == null && this.Mesh.ContainsURL(url))
			{
				customMeshContainer = Singleton<DLCManager>.Instance.LoadDLCMesh(DLCName, DLCUrl, url);
				yield return null;
			}
			if (addLoading)
			{
				Singleton<UILoading>.Instance.RemoveLoading();
			}
			this.Mesh.Finished(customMeshContainer);
			yield break;
		}
		while (!this.GetAvailableWWW())
		{
			yield return null;
		}
		string ConvertedURL = CustomCache.CheckModelExist(url, CustomCache.GetCacheMode(), true, true);
		if (checkModifiedHeader && url != ConvertedURL && !DataRequest.IsLocalFile(url) && DataRequest.IsLocalFile(ConvertedURL) && !CustomLoadingManager.UrlHeaders.ContainsKey(url))
		{
			Task<Dictionary<string, string>> task = this.GetHeaders(url);
			while (!task.IsCompleted)
			{
				yield return null;
			}
			Dictionary<string, string> result = task.Result;
			CustomLoadingManager.UrlHeaders.Add(url, result);
			string s;
			DateTime dateTime;
			if (result != null && result.TryGetValue("Last-Modified", out s) && DateTime.TryParse(s, out dateTime))
			{
				DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(DataRequest.TryRemoveLocalFile(CustomCache.CheckModelExist(url, CacheMode.NoRawCache, false, true)));
				if (dateTime.ToUniversalTime() > lastWriteTimeUtc)
				{
					Debug.Log(string.Concat(new object[]
					{
						"Cache is stale: ",
						url,
						" server: ",
						dateTime.ToUniversalTime(),
						" file: ",
						lastWriteTimeUtc
					}));
					ConvertedURL = url;
				}
			}
			task = null;
		}
		DataRequest dataRequest = DataRequest.Get(ConvertedURL, null);
		while (!dataRequest.isDone)
		{
			yield return null;
		}
		bool LoadSuccessful = false;
		string ModelString = null;
		byte[] data = null;
		FileMagicNumbers.FileFormat fileFormat = FileMagicNumbers.FileFormat.UNKNOWN;
		if (DataRequest.IsLocalFile(ConvertedURL) && FileMagicNumbers.GetFileFormat(ConvertedURL) == FileMagicNumbers.FileFormat.RAWM)
		{
			data = dataRequest.data;
			fileFormat = FileMagicNumbers.GetFileFormat(data);
			if (fileFormat != FileMagicNumbers.FileFormat.RAWM)
			{
				fileFormat = FileMagicNumbers.FileFormat.OBJ;
				ModelString = dataRequest.text;
			}
		}
		else
		{
			ModelString = dataRequest.text;
			fileFormat = (ModelString.StartsWith("rawm") ? FileMagicNumbers.FileFormat.RAWM : FileMagicNumbers.FileFormat.OBJ);
			if (fileFormat == FileMagicNumbers.FileFormat.RAWM)
			{
				data = dataRequest.data;
			}
		}
		bool isError = dataRequest.isError;
		string error = dataRequest.error;
		RawMeshData rawMeshData = default(RawMeshData);
		dataRequest.Dispose();
		this.ReleaseWWW();
		if (this.Mesh.ContainsURL(url))
		{
			if (isError)
			{
				Chat.Log(string.Concat(new string[]
				{
					"WWW Model Error: ",
					error,
					"\n    at [",
					url,
					"]"
				}), Colour.Red, ChatMessageType.Game, false);
			}
			else if (fileFormat == FileMagicNumbers.FileFormat.RAWM)
			{
				RAWMLoaderJob rawmLoaderJob = new RAWMLoaderJob
				{
					inData = data
				};
				rawmLoaderJob.Start(true, false);
				while (!rawmLoaderJob.Update())
				{
					yield return null;
				}
				try
				{
					mesh = new Mesh();
					mesh.SetVertices(rawmLoaderJob.vertices);
					mesh.SetUVs(0, rawmLoaderJob.uv);
					mesh.SetNormals(rawmLoaderJob.normals);
					mesh.SetTangents(rawmLoaderJob.tangents);
					mesh.subMeshCount = rawmLoaderJob.triangles.Length;
					for (int i = 0; i < mesh.subMeshCount; i++)
					{
						mesh.SetTriangles(rawmLoaderJob.triangles[i], i);
					}
					LoadSuccessful = true;
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
					Chat.LogError("Error loading raw model: " + url, true);
				}
				rawmLoaderJob = null;
			}
			else
			{
				if (!UseThreading)
				{
					try
					{
						GameObject[] array = this.objReader.ConvertString(ModelString);
						mesh = array[0].GetComponent<MeshFilter>().sharedMesh;
						for (int j = 0; j < array.Length; j++)
						{
							UnityEngine.Object.Destroy(array[j]);
						}
						LoadSuccessful = true;
						goto IL_A6B;
					}
					catch (Exception)
					{
						this.LogModelError(url);
						goto IL_A6B;
					}
				}
				MeshLoaderJob meshJob = new MeshLoaderJob
				{
					ModelString = ModelString
				};
				meshJob.Start(true, false);
				while (!meshJob.Update())
				{
					yield return null;
				}
				TTSGameObject go = meshJob.go;
				if (this.Mesh.ContainsURL(url))
				{
					if (meshJob.isError)
					{
						this.LogModelError(url);
					}
					else if (go.meshes.Count < 1 || go.meshes[0].vertices == null || go.meshes[0].triangles == null)
					{
						this.LogModelError(url);
						Debug.LogError("Mesh failed to parse.");
					}
					else
					{
						mesh = new Mesh();
						mesh.vertices = go.meshes[0].vertices;
						if (go.meshes[0].uv != null)
						{
							mesh.uv = go.meshes[0].uv;
						}
						if (go.meshes[0].normals != null)
						{
							mesh.normals = go.meshes[0].normals;
						}
						if (go.meshes[0].tangents != null)
						{
							mesh.tangents = go.meshes[0].tangents;
						}
						mesh.subMeshCount = go.meshes.Count;
						for (int k = 0; k < go.meshes.Count; k++)
						{
							mesh.SetTriangles(go.meshes[k].triangles, k);
						}
						if (go.meshes[0].normals == null)
						{
							mesh.RecalculateNormals();
						}
						Vector2[] uv = go.meshes[0].uv ?? mesh.uv;
						Vector3[] array2 = go.meshes[0].normals ?? mesh.normals;
						int[][] array3 = new int[go.meshes.Count][];
						for (int l = 0; l < array3.Length; l++)
						{
							array3[l] = go.meshes[l].triangles;
						}
						rawMeshData = new RawMeshData
						{
							vertices = go.meshes[0].vertices,
							uv = uv,
							normals = array2,
							tangents = go.meshes[0].tangents,
							triangles = array3
						};
						if (go.meshes[0].tangents == null && go.meshes[0].vertices != null && array2 != null && go.meshes[0].uv != null && go.meshes[0].triangles != null)
						{
							MeshTangentsJob tangentsJob = new MeshTangentsJob
							{
								vertices = go.meshes[0].vertices,
								normals = array2,
								uv = go.meshes[0].uv,
								triangles = ((go.meshes.Count > 1) ? mesh.triangles : go.meshes[0].triangles)
							};
							tangentsJob.Start(true, true);
							while (!tangentsJob.Update())
							{
								yield return null;
							}
							mesh.tangents = tangentsJob.tangents;
							rawMeshData.tangents = tangentsJob.tangents;
							tangentsJob = null;
						}
						if (rawMeshData.tangents == null)
						{
							mesh.RecalculateTangents();
							rawMeshData.tangents = mesh.tangents;
						}
						LoadSuccessful = true;
					}
				}
				meshJob = null;
			}
		}
		IL_A6B:
		if (LoadSuccessful)
		{
			customMeshContainer = new CustomMeshContainer(url, mesh);
			if (!DataRequest.IsLocalFile(ConvertedURL))
			{
				if (fileFormat == FileMagicNumbers.FileFormat.RAWM)
				{
					Task task2 = CustomCache.SaveModelAsync(data, url, FileMagicNumbers.GetExtension(fileFormat));
					while (!task2.IsCompleted)
					{
						yield return null;
					}
					task2 = null;
				}
				else if (ModelString != null)
				{
					Task task2 = CustomCache.SaveModelAsync(ModelString, url, FileMagicNumbers.GetExtension(fileFormat));
					while (!task2.IsCompleted)
					{
						yield return null;
					}
					task2 = null;
				}
			}
			if (ConfigGame.Settings.ConfigMods.RawCaching && rawMeshData.vertices != null)
			{
				Task task2 = CustomCache.SaveModelRawAsync(rawMeshData, url);
				while (!task2.IsCompleted)
				{
					yield return null;
				}
				task2 = null;
			}
		}
		else
		{
			UnityEngine.Object.Destroy(mesh);
		}
		if (addLoading)
		{
			Singleton<UILoading>.Instance.RemoveLoading();
		}
		this.Mesh.Finished(customMeshContainer);
		yield break;
	}

	// Token: 0x06000A95 RID: 2709 RVA: 0x0004AEB4 File Offset: 0x000490B4
	private IEnumerator CoroutineLoadAssetbundle(string url, bool checkModifiedHeader, CustomLoadingManager.LoadType loadType, bool addLoading)
	{
		GameObject[] gameObjects = null;
		AssetBundle assetBundle = null;
		CustomAssetbundleContainer customAssetbundleContainer = new CustomAssetbundleContainer(url, null, null);
		bool UseThreading = (loadType == CustomLoadingManager.LoadType.Auto && ConfigGame.Settings.ConfigMods.Threading) || loadType == CustomLoadingManager.LoadType.Async;
		UseThreading = true;
		if (addLoading)
		{
			Singleton<UILoading>.Instance.AddLoading();
		}
		string DLCName;
		string DLCUrl;
		if (DLCManager.URLisDLC(url, out DLCName, out DLCUrl))
		{
			customAssetbundleContainer = null;
			while (customAssetbundleContainer == null && this.Assetbundle.ContainsURL(url))
			{
				customAssetbundleContainer = Singleton<DLCManager>.Instance.LoadDLCAssetBundle(DLCName, DLCUrl, url);
				yield return null;
			}
			if (addLoading)
			{
				Singleton<UILoading>.Instance.RemoveLoading();
			}
			this.Assetbundle.Finished(customAssetbundleContainer);
			yield break;
		}
		yield return null;
		while (!this.GetAvailableWWW())
		{
			yield return null;
		}
		string ConvertedURL = CustomCache.CheckAssetbundleExist(url, CustomCache.GetCacheMode(), true, true);
		if (checkModifiedHeader && url != ConvertedURL && !DataRequest.IsLocalFile(url) && DataRequest.IsLocalFile(ConvertedURL) && !CustomLoadingManager.UrlHeaders.ContainsKey(url))
		{
			Task<Dictionary<string, string>> task = this.GetHeaders(url);
			while (!task.IsCompleted)
			{
				yield return null;
			}
			Dictionary<string, string> result = task.Result;
			CustomLoadingManager.UrlHeaders.Add(url, result);
			string s;
			DateTime dateTime;
			if (result != null && result.TryGetValue("Last-Modified", out s) && DateTime.TryParse(s, out dateTime))
			{
				DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(DataRequest.TryRemoveLocalFile(ConvertedURL));
				if (dateTime.ToUniversalTime() > lastWriteTimeUtc)
				{
					Debug.Log(string.Concat(new object[]
					{
						"Cache is stale: ",
						url,
						" server: ",
						dateTime.ToUniversalTime(),
						" file: ",
						lastWriteTimeUtc
					}));
					ConvertedURL = url;
				}
			}
			task = null;
		}
		DataRequest dataRequest = DataRequest.GetAssetBundle(ConvertedURL, false);
		while (!dataRequest.isDone)
		{
			yield return null;
		}
		this.ReleaseWWW();
		bool LoadSuccessful = false;
		try
		{
			assetBundle = dataRequest.assetBundle;
		}
		catch (Exception e)
		{
			Chat.LogException("loading AssetBundle [" + url + "]", e, true, false);
		}
		if (this.Assetbundle.ContainsURL(url))
		{
			if (dataRequest.isError)
			{
				Chat.LogError(string.Concat(new string[]
				{
					"WWW AssetBundle Error: ",
					dataRequest.error,
					"\n    at [",
					url,
					"]"
				}), true);
			}
			else
			{
				if (!UseThreading)
				{
					try
					{
						UnityEngine.Object[] array = assetBundle.LoadAllAssets<UnityEngine.Object>();
						List<GameObject> list = new List<GameObject>();
						List<Material> list2 = new List<Material>();
						foreach (UnityEngine.Object @object in array)
						{
							if (@object is GameObject)
							{
								list.Add(@object as GameObject);
							}
							else if (@object is Material)
							{
								list2.Add(@object as Material);
							}
						}
						gameObjects = list.ToArray();
						CustomAssetbundle.CleanupAssetBundleGameObjects(gameObjects);
						CustomAssetbundle.CleanupAssetBundleMaterials(list2);
						LoadSuccessful = true;
						goto IL_643;
					}
					catch (Exception e2)
					{
						Chat.LogException("loading AssetBundle [" + url + "]", e2, true, false);
						goto IL_643;
					}
				}
				if (assetBundle != null)
				{
					AssetBundleRequest assetBundleRequest = assetBundle.LoadAllAssetsAsync<UnityEngine.Object>();
					yield return assetBundleRequest;
					if (!this.Assetbundle.ContainsURL(url))
					{
						UnityEngine.Object[] allAssets = assetBundleRequest.allAssets;
						int num = 0;
						UnityEngine.Object[] array2 = allAssets;
						for (int j = 0; j < array2.Length; j++)
						{
							if (array2[j] is GameObject)
							{
								num++;
							}
						}
						gameObjects = new GameObject[num];
						num = 0;
						array2 = allAssets;
						for (int j = 0; j < array2.Length; j++)
						{
							GameObject gameObject;
							if ((gameObject = (array2[j] as GameObject)) != null)
							{
								gameObjects[num] = gameObject;
								num++;
							}
						}
					}
					else
					{
						try
						{
							UnityEngine.Object[] allAssets2 = assetBundleRequest.allAssets;
							int num2 = 0;
							List<Material> list3 = new List<Material>();
							foreach (UnityEngine.Object object2 in allAssets2)
							{
								if (object2 != null)
								{
									if (!(object2 is GameObject))
									{
										Material material;
										if ((material = (object2 as Material)) != null)
										{
											Material item = material;
											list3.Add(item);
										}
									}
									else
									{
										num2++;
									}
								}
							}
							gameObjects = new GameObject[num2];
							num2 = 0;
							for (int l = 0; l < allAssets2.Length; l++)
							{
								GameObject gameObject2;
								if ((gameObject2 = (allAssets2[l] as GameObject)) != null)
								{
									gameObjects[num2] = gameObject2;
									num2++;
								}
							}
							CustomAssetbundle.CleanupAssetBundleGameObjects(gameObjects);
							CustomAssetbundle.CleanupAssetBundleMaterials(list3);
							LoadSuccessful = true;
						}
						catch (Exception e3)
						{
							Chat.LogException("converting and cleaning AssetBundle [" + url + "]", e3, true, false);
						}
					}
					assetBundleRequest = null;
				}
			}
		}
		IL_643:
		if (LoadSuccessful)
		{
			customAssetbundleContainer = new CustomAssetbundleContainer(url, assetBundle, gameObjects);
			if (!DataRequest.IsLocalFile(ConvertedURL))
			{
				Task task2 = CustomCache.SaveAssetbundleAsync(dataRequest.data, url);
				while (!task2.IsCompleted)
				{
					yield return null;
				}
				task2 = null;
			}
		}
		else
		{
			CustomLoadingManager.DestroyAssetbundle(assetBundle, gameObjects);
		}
		if (addLoading)
		{
			Singleton<UILoading>.Instance.RemoveLoading();
		}
		this.Assetbundle.Finished(customAssetbundleContainer);
		dataRequest.Dispose();
		yield break;
	}

	// Token: 0x06000A96 RID: 2710 RVA: 0x0004AEE0 File Offset: 0x000490E0
	private IEnumerator CoroutineLoadToken(TokenSettings settings, CustomLoadingManager.LoadType loadType, bool addLoading)
	{
		float num = 14f;
		float aspectRatio = settings.AspectRatio;
		Texture2D image = settings.Image;
		float thickness = settings.Thickness;
		float mergeDistancePixels = settings.MergeDistancePixels;
		float num2 = Mathf.Sqrt(num / aspectRatio);
		CustomTokenContainer customTokenContainer = new CustomTokenContainer(settings, null, null);
		if (!TextureScript.HasAlpha(image))
		{
			Mesh mesh = UnityEngine.Object.Instantiate<Mesh>(this.NonAlphaTokenMesh);
			Vector3 vector = new Vector3(num2 * aspectRatio, thickness, num2);
			if (vector != Vector3.one)
			{
				Utilities.ScaleMesh(mesh, vector, false);
			}
			BoxColliderState boxColliderState = new BoxColliderState
			{
				LocalPosition = default(VectorState),
				LocalEulerRotation = default(VectorState),
				Center = default(VectorState),
				Size = new VectorState(vector),
				Name = "CompoundColliders"
			};
			customTokenContainer = new CustomTokenContainer(settings, mesh, new BoxColliderState[]
			{
				boxColliderState
			});
			this.Token.Finished(customTokenContainer);
			yield break;
		}
		bool flag = (loadType == CustomLoadingManager.LoadType.Auto && ConfigGame.Settings.ConfigMods.Threading) || loadType == CustomLoadingManager.LoadType.Async;
		if (addLoading)
		{
			Singleton<UILoading>.Instance.AddLoading();
		}
		GameObject meshObject = new GameObject();
		meshObject.transform.Reset();
		meshObject.transform.position = new Vector3(0f, 100000f, 0f);
		TTSMeshCreatorData ttsmeshCreatorData = meshObject.AddComponent<TTSMeshCreatorData>();
		bool flag2 = true;
		try
		{
			image.GetPixel(0, 0);
		}
		catch (Exception)
		{
			flag2 = false;
		}
		int num3 = image.width;
		int num4 = image.height;
		int num5 = 0;
		while (num3 > 1024 && num4 > 1024)
		{
			num3 /= 2;
			num4 /= 2;
			num5++;
		}
		Texture2D LowResImage = null;
		if (!flag2)
		{
			LowResImage = new Texture2D(num3, num4);
			image.ResizePro(num3, num4, out LowResImage);
			ttsmeshCreatorData.outlineTexture = LowResImage;
		}
		else if (image.width > 1024 && image.height > 1024)
		{
			LowResImage = new Texture2D(num3, num4);
			LowResImage.SetPixels32(image.GetPixels32(num5));
			ttsmeshCreatorData.outlineTexture = LowResImage;
		}
		else
		{
			ttsmeshCreatorData.outlineTexture = image;
		}
		ttsmeshCreatorData.meshDepth = thickness;
		ttsmeshCreatorData.mergeClosePoints = true;
		ttsmeshCreatorData.mergeDistance = mergeDistancePixels;
		ttsmeshCreatorData.maxNumberBoxes = 20;
		ttsmeshCreatorData.meshWidth = num2 * aspectRatio;
		ttsmeshCreatorData.meshHeight = num2;
		bool LoadSuccessful = false;
		if (!flag)
		{
			try
			{
				TTSMeshCreator.UpdateMesh(meshObject);
				LoadSuccessful = true;
				goto IL_423;
			}
			catch (Exception e)
			{
				Chat.LogException("creating Token Mesh", e, true, false);
				goto IL_423;
			}
		}
		TokenMeshJob tokenMeshJob = new TokenMeshJob();
		tokenMeshJob.GO = meshObject;
		tokenMeshJob.TGO = new TTSGameObject(false);
		tokenMeshJob.TGO.mcd = TTSMeshCreatorData.toThreadSafe(ttsmeshCreatorData);
		if (tokenMeshJob.TGO.mcd != null)
		{
			tokenMeshJob.Start(true, false);
			while (!tokenMeshJob.Update())
			{
				yield return null;
			}
			if (!tokenMeshJob.isError)
			{
				TokenColliderJob tokenColliderJob = new TokenColliderJob();
				tokenColliderJob.GO = tokenMeshJob.GO;
				tokenColliderJob.TGO = tokenMeshJob.TGO;
				tokenColliderJob.Start(true, true);
				while (!tokenColliderJob.Update())
				{
					yield return null;
				}
				if (!tokenColliderJob.isError)
				{
					LoadSuccessful = true;
				}
				tokenColliderJob = null;
			}
		}
		tokenMeshJob = null;
		IL_423:
		if (LoadSuccessful)
		{
			Transform transform = meshObject.transform.Find(meshObject.name + "CompoundColliders");
			if (transform)
			{
				customTokenContainer = new CustomTokenContainer(settings, meshObject.GetComponent<MeshFilter>().sharedMesh, BoxColliderState.GetBoxColliders(transform.gameObject, ""));
			}
			else
			{
				Chat.LogError("Something went wrong with Token colliders.", true);
			}
		}
		else
		{
			Chat.LogError("Failed to create Token from Image.", true);
		}
		if (addLoading)
		{
			Singleton<UILoading>.Instance.RemoveLoading();
		}
		this.Token.Finished(customTokenContainer);
		UnityEngine.Object.Destroy(LowResImage);
		UnityEngine.Object.Destroy(meshObject);
		yield break;
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x0004AF04 File Offset: 0x00049104
	private IEnumerator CoroutineLoadUIAssetbundle(string url, bool checkModifiedHeader, CustomLoadingManager.LoadType loadType, bool addLoading)
	{
		UnityEngine.Object[] resources = null;
		AssetBundle assetBundle = null;
		CustomUIAssetbundleContainer container = new CustomUIAssetbundleContainer(url, null, null);
		if (addLoading)
		{
			Singleton<UILoading>.Instance.AddLoading();
		}
		yield return null;
		while (!this.GetAvailableWWW())
		{
			yield return null;
		}
		string loadUrl = CustomCache.CheckAssetbundleExist(url, CustomCache.GetCacheMode(), true, true);
		if (checkModifiedHeader && url != loadUrl && !DataRequest.IsLocalFile(url) && DataRequest.IsLocalFile(loadUrl) && !CustomLoadingManager.UrlHeaders.ContainsKey(url))
		{
			Task<Dictionary<string, string>> task = this.GetHeaders(url);
			while (!task.IsCompleted)
			{
				yield return null;
			}
			Dictionary<string, string> result = task.Result;
			CustomLoadingManager.UrlHeaders.Add(url, result);
			string s;
			DateTime dateTime;
			if (result != null && result.TryGetValue("Last-Modified", out s) && DateTime.TryParse(s, out dateTime))
			{
				DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(DataRequest.TryRemoveLocalFile(loadUrl));
				if (dateTime.ToUniversalTime() > lastWriteTimeUtc)
				{
					Debug.Log(string.Concat(new object[]
					{
						"Cache is stale: ",
						url,
						" server: ",
						dateTime.ToUniversalTime(),
						" file: ",
						lastWriteTimeUtc
					}));
					loadUrl = url;
				}
			}
			task = null;
		}
		DataRequest dataRequest = DataRequest.GetAssetBundle(loadUrl, false);
		while (!dataRequest.isDone)
		{
			yield return null;
		}
		this.ReleaseWWW();
		try
		{
			assetBundle = dataRequest.assetBundle;
		}
		catch (Exception e)
		{
			Chat.LogException("loading UI AssetBundle [" + url + "]", e, true, false);
		}
		if (dataRequest.isError)
		{
			Chat.LogError(string.Concat(new string[]
			{
				"WWW AssetBundle Error: ",
				dataRequest.error,
				"\n    at [",
				url,
				"]"
			}), true);
		}
		else if (assetBundle != null)
		{
			AssetBundleRequest assetBundleRequest = assetBundle.LoadAllAssetsAsync<UnityEngine.Object>();
			yield return assetBundleRequest;
			UnityEngine.Object[] allAssets = assetBundleRequest.allAssets;
			int num = 0;
			int num2 = 0;
			foreach (UnityEngine.Object @object in allAssets)
			{
				if (@object is Sprite)
				{
					num++;
					num2++;
				}
				else if (@object is Font)
				{
					num++;
					num2 += 2;
				}
				else if (@object is AudioClip)
				{
					num++;
				}
			}
			resources = new UnityEngine.Object[num];
			UnityEngine.Object[] array2 = new UnityEngine.Object[num2];
			num = 0;
			num2 = 0;
			foreach (UnityEngine.Object object2 in allAssets)
			{
				Sprite sprite;
				Font font;
				AudioClip audioClip;
				if ((sprite = (object2 as Sprite)) != null)
				{
					resources[num++] = sprite;
					array2[num2++] = sprite.texture;
				}
				else if ((font = (object2 as Font)) != null)
				{
					resources[num++] = font;
					array2[num2++] = font.material;
					array2[num2++] = font.material.mainTexture;
				}
				else if ((audioClip = (object2 as AudioClip)) != null)
				{
					resources[num++] = audioClip;
				}
			}
			foreach (UnityEngine.Object object3 in allAssets)
			{
				bool flag = false;
				if (object3 is Sprite || object3 is Font || object3 is AudioClip)
				{
					flag = true;
				}
				else if (object3 is Material || object3 is Texture2D)
				{
					foreach (UnityEngine.Object y in array2)
					{
						if (object3 == y)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					Debug.Log("Destroying incompatible bundled UI asset: " + object3);
					UnityEngine.Object.DestroyImmediate(object3, true);
				}
			}
			assetBundleRequest = null;
		}
		container = new CustomUIAssetbundleContainer(url, assetBundle, resources);
		if (!DataRequest.IsLocalFile(loadUrl))
		{
			Task task2 = CustomCache.SaveAssetbundleAsync(dataRequest.data, url);
			while (!task2.IsCompleted)
			{
				yield return null;
			}
			task2 = null;
		}
		if (addLoading)
		{
			Singleton<UILoading>.Instance.RemoveLoading();
		}
		this.UIAssetbundle.Finished(container);
		dataRequest.Dispose();
		yield break;
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x0004AF29 File Offset: 0x00049129
	public static string GetSupportedAllFormatExtensionsString()
	{
		return CustomLoadingManager.GetFormatExtensionsString(CustomLoadingManager.SupportedAllFormats);
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x0004AF35 File Offset: 0x00049135
	public static string GetSupportedImageFormatExtensionsString()
	{
		return CustomLoadingManager.GetFormatExtensionsString(CustomLoadingManager.SupportedImageFormats);
	}

	// Token: 0x06000A9A RID: 2714 RVA: 0x0004AF41 File Offset: 0x00049141
	public static string GetSupportedAudioFormatExtensionsString()
	{
		return CustomLoadingManager.GetFormatExtensionsString(CustomLoadingManager.SupportedAudioFormats);
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x0004AF4D File Offset: 0x0004914D
	public static string GetSupportedPDFFormatExtensionsString()
	{
		return CustomLoadingManager.GetFormatExtensionsString(CustomLoadingManager.SupportedPDFFormats);
	}

	// Token: 0x06000A9C RID: 2716 RVA: 0x0004AF59 File Offset: 0x00049159
	public static string GetSupportedMeshFormatExtensionsString()
	{
		return CustomLoadingManager.GetFormatExtensionsString(CustomLoadingManager.SupportedMeshFormats);
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x0004AF65 File Offset: 0x00049165
	public static string GetSupportedAssetBundleFormatExtensionsString()
	{
		return CustomLoadingManager.GetFormatExtensionsString(CustomLoadingManager.SupportedAssetBundleFormats);
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x0004AF74 File Offset: 0x00049174
	public static string GetFormatExtensionsString(List<FileMagicNumbers.FileFormat> formats)
	{
		string text = null;
		for (int i = 0; i < formats.Count; i++)
		{
			if (string.IsNullOrEmpty(text))
			{
				text = FileMagicNumbers.GetExtension(formats[i]);
			}
			else
			{
				text = text + ", " + FileMagicNumbers.GetExtension(formats[i]);
			}
		}
		return text;
	}

	// Token: 0x06000A9F RID: 2719 RVA: 0x0004AFC4 File Offset: 0x000491C4
	private void LogImageError(string url)
	{
		Chat.LogError("Failed to load Image (" + CustomLoadingManager.GetSupportedImageFormatExtensionsString() + "): " + url, true);
	}

	// Token: 0x06000AA0 RID: 2720 RVA: 0x0004AFE1 File Offset: 0x000491E1
	private void LogModelError(string url)
	{
		Chat.LogError("Failed to load Model (.obj): " + url, true);
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x0004AFF4 File Offset: 0x000491F4
	private void LogAssetbundleError(string url)
	{
		Chat.LogError("Failed to load AssetBundle (.unity3d): " + url, true);
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x0004B008 File Offset: 0x00049208
	private void Start()
	{
		this.CustomLoadingAssets = new List<CustomLoadingManager.ICustomLoadingAsset>
		{
			this.Texture,
			this.Mesh,
			this.Assetbundle,
			this.Audio,
			this.PDF,
			this.Text,
			this.Token
		};
		Wait.Time(new Action(this.CheckCleanupLeaks), 60f, -1);
		if (Utilities.IsLaunchOption("-nothreading"))
		{
			ConfigGame.Settings.ConfigMods.Threading = false;
			Chat.LogSystem("Threading is disabled by -nothreading launch option.", Colour.Blue, true);
			Chat.LogSystem("Threading mod loading = " + ConfigGame.Settings.ConfigMods.Threading.ToString(), Colour.Green, false);
		}
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x0004B0EC File Offset: 0x000492EC
	private void CheckCleanupLeaks()
	{
		foreach (CustomLoadingManager.ICustomLoadingAsset customLoadingAsset in this.CustomLoadingAssets)
		{
			customLoadingAsset.CheckCleanupLeaks();
		}
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x0004B13C File Offset: 0x0004933C
	private void Update()
	{
		if (!Debugging.bDebug)
		{
			return;
		}
		int num = 0;
		foreach (CustomLoadingManager.ICustomLoadingAsset customLoadingAsset in this.CustomLoadingAssets)
		{
			num += customLoadingAsset.GetCount();
		}
		if (num > 0)
		{
			this.DebugLabel.gameObject.SetActive(true);
			this.DebugLabel.text = num.ToString();
			if (UICamera.HoveredUIObject == this.DebugLabel.gameObject && zInput.GetButtonDown("Grab", ControlType.All))
			{
				string text = "Debug:\n";
				foreach (CustomLoadingManager.ICustomLoadingAsset customLoadingAsset2 in this.CustomLoadingAssets)
				{
					List<string> keys = customLoadingAsset2.GetKeys();
					if (keys.Count != 0)
					{
						text = text + "\n" + customLoadingAsset2.GetType().Name + ":\n";
						foreach (string str in keys)
						{
							text = text + str + "\n";
						}
					}
				}
				Chat.Log(text, ChatMessageType.Game);
				string text2 = "AssetBundles Unity (" + AssetBundle.GetAllLoadedAssetBundles().Count<AssetBundle>().ToString() + "):\n";
				foreach (AssetBundle arg in AssetBundle.GetAllLoadedAssetBundles())
				{
					text2 = text2 + arg + "\n";
				}
				Chat.Log(text2, ChatMessageType.Game);
				return;
			}
		}
		else
		{
			this.DebugLabel.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000AA5 RID: 2725 RVA: 0x0004B338 File Offset: 0x00049538
	private void OnDestroy()
	{
		this.ForceDestroy();
	}

	// Token: 0x06000AA6 RID: 2726 RVA: 0x0004B340 File Offset: 0x00049540
	private void ForceDestroy()
	{
		foreach (CustomLoadingManager.ICustomLoadingAsset customLoadingAsset in this.CustomLoadingAssets)
		{
			customLoadingAsset.ForceDestroy();
		}
		foreach (AssetBundle assetBundle in AssetBundle.GetAllLoadedAssetBundles())
		{
			assetBundle.Unload(true);
		}
	}

	// Token: 0x06000AA7 RID: 2727 RVA: 0x0004B3CC File Offset: 0x000495CC
	public static void DestroyAssetbundle(AssetBundle assetBundle, GameObject[] gameObjects)
	{
		if (assetBundle != null)
		{
			assetBundle.Unload(true);
		}
		if (gameObjects != null)
		{
			for (int i = 0; i < gameObjects.Length; i++)
			{
				UnityEngine.Object.Destroy(gameObjects[i]);
			}
		}
	}

	// Token: 0x04000776 RID: 1910
	public UILabel DebugLabel;

	// Token: 0x04000777 RID: 1911
	public ObjReader objReader;

	// Token: 0x04000778 RID: 1912
	public Mesh NonAlphaTokenMesh;

	// Token: 0x04000779 RID: 1913
	public readonly CustomLoadingManager.CustomLoadingTexture Texture = new CustomLoadingManager.CustomLoadingTexture();

	// Token: 0x0400077A RID: 1914
	public readonly CustomLoadingManager.CustomLoadingMesh Mesh = new CustomLoadingManager.CustomLoadingMesh();

	// Token: 0x0400077B RID: 1915
	public readonly CustomLoadingManager.CustomLoadingAssetbundle Assetbundle = new CustomLoadingManager.CustomLoadingAssetbundle();

	// Token: 0x0400077C RID: 1916
	public readonly CustomLoadingManager.CustomLoadingAudio Audio = new CustomLoadingManager.CustomLoadingAudio();

	// Token: 0x0400077D RID: 1917
	public readonly CustomLoadingManager.CustomLoadingPDF PDF = new CustomLoadingManager.CustomLoadingPDF();

	// Token: 0x0400077E RID: 1918
	public readonly CustomLoadingManager.CustomLoadingText Text = new CustomLoadingManager.CustomLoadingText();

	// Token: 0x0400077F RID: 1919
	public readonly CustomLoadingManager.CustomLoadingToken Token = new CustomLoadingManager.CustomLoadingToken();

	// Token: 0x04000780 RID: 1920
	public readonly CustomLoadingManager.CustomLoadingUIAssetbundle UIAssetbundle = new CustomLoadingManager.CustomLoadingUIAssetbundle();

	// Token: 0x04000781 RID: 1921
	private List<CustomLoadingManager.ICustomLoadingAsset> CustomLoadingAssets;

	// Token: 0x04000782 RID: 1922
	public static int MAX_NUMBER_WWW = 16;

	// Token: 0x04000783 RID: 1923
	private int current_WWW;

	// Token: 0x04000784 RID: 1924
	private static readonly Dictionary<string, Dictionary<string, string>> UrlHeaders = new Dictionary<string, Dictionary<string, string>>();

	// Token: 0x04000785 RID: 1925
	public static readonly List<FileMagicNumbers.FileFormat> SupportedAllFormats = new List<FileMagicNumbers.FileFormat>
	{
		FileMagicNumbers.FileFormat.JPG,
		FileMagicNumbers.FileFormat.PNG,
		FileMagicNumbers.FileFormat.OBJ,
		FileMagicNumbers.FileFormat.ASSETBUNDLE,
		FileMagicNumbers.FileFormat.WEBM,
		FileMagicNumbers.FileFormat.MP4,
		FileMagicNumbers.FileFormat.M4V,
		FileMagicNumbers.FileFormat.MOV,
		FileMagicNumbers.FileFormat.MP3,
		FileMagicNumbers.FileFormat.OGV,
		FileMagicNumbers.FileFormat.OGG,
		FileMagicNumbers.FileFormat.WAV,
		FileMagicNumbers.FileFormat.PDF,
		FileMagicNumbers.FileFormat.RAWT,
		FileMagicNumbers.FileFormat.RAWM,
		FileMagicNumbers.FileFormat.TXT
	};

	// Token: 0x04000786 RID: 1926
	public static readonly List<string> SupportedAllFormatExtensions = new List<string>
	{
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.JPG),
		".jpeg",
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.PNG),
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.OBJ),
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.ASSETBUNDLE),
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.WEBM),
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.MP4),
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.M4V),
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.MOV),
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.MP3),
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.OGV),
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.OGG),
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.WAV),
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.PDF),
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.RAWT),
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.RAWM),
		".sh",
		".json"
	};

	// Token: 0x04000787 RID: 1927
	public static readonly List<FileMagicNumbers.FileFormat> SupportedVideoPlayerFormats = new List<FileMagicNumbers.FileFormat>
	{
		FileMagicNumbers.FileFormat.WEBM,
		FileMagicNumbers.FileFormat.MP4,
		FileMagicNumbers.FileFormat.M4V,
		FileMagicNumbers.FileFormat.MOV
	};

	// Token: 0x04000788 RID: 1928
	public static readonly List<FileMagicNumbers.FileFormat> SupportedImageFormats = new List<FileMagicNumbers.FileFormat>
	{
		FileMagicNumbers.FileFormat.JPG,
		FileMagicNumbers.FileFormat.PNG,
		FileMagicNumbers.FileFormat.WEBM,
		FileMagicNumbers.FileFormat.MP4,
		FileMagicNumbers.FileFormat.M4V,
		FileMagicNumbers.FileFormat.MOV,
		FileMagicNumbers.FileFormat.RAWT,
		FileMagicNumbers.FileFormat.ASSETBUNDLE
	};

	// Token: 0x04000789 RID: 1929
	public static readonly List<string> SupportedImageFormatExtensions = new List<string>
	{
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.JPG),
		".jpeg",
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.PNG),
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.WEBM),
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.MP4),
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.M4V),
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.MOV),
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.RAWT),
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.ASSETBUNDLE)
	};

	// Token: 0x0400078A RID: 1930
	public static readonly List<TextureFormat> CompressedFormats = new List<TextureFormat>
	{
		TextureFormat.DXT1,
		TextureFormat.DXT5,
		TextureFormat.DXT1Crunched,
		TextureFormat.DXT5Crunched
	};

	// Token: 0x0400078B RID: 1931
	public static readonly List<FileMagicNumbers.FileFormat> SupportedAudioFormats = new List<FileMagicNumbers.FileFormat>
	{
		FileMagicNumbers.FileFormat.MP3,
		FileMagicNumbers.FileFormat.OGV,
		FileMagicNumbers.FileFormat.OGG,
		FileMagicNumbers.FileFormat.WAV
	};

	// Token: 0x0400078C RID: 1932
	public static readonly List<string> SupportedAudioFormatExtensions = new List<string>
	{
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.MP3),
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.OGV),
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.OGG),
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.WAV)
	};

	// Token: 0x0400078D RID: 1933
	public static readonly List<FileMagicNumbers.FileFormat> SupportedPDFFormats = new List<FileMagicNumbers.FileFormat>
	{
		FileMagicNumbers.FileFormat.PDF
	};

	// Token: 0x0400078E RID: 1934
	public static readonly List<string> SupportedPDFFormatExtensions = new List<string>
	{
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.PDF)
	};

	// Token: 0x0400078F RID: 1935
	public static readonly List<FileMagicNumbers.FileFormat> SupportedTextFormats = new List<FileMagicNumbers.FileFormat>
	{
		FileMagicNumbers.FileFormat.TXT
	};

	// Token: 0x04000790 RID: 1936
	public static readonly List<string> SupportedTextFormatExtensions = new List<string>
	{
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.TXT)
	};

	// Token: 0x04000791 RID: 1937
	public static readonly List<FileMagicNumbers.FileFormat> SupportedMeshFormats = new List<FileMagicNumbers.FileFormat>
	{
		FileMagicNumbers.FileFormat.OBJ,
		FileMagicNumbers.FileFormat.RAWM
	};

	// Token: 0x04000792 RID: 1938
	public static readonly List<string> SupportedMeshFormatExtensions = new List<string>
	{
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.OBJ),
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.RAWM)
	};

	// Token: 0x04000793 RID: 1939
	public static readonly List<FileMagicNumbers.FileFormat> SupportedAssetBundleFormats = new List<FileMagicNumbers.FileFormat>
	{
		FileMagicNumbers.FileFormat.ASSETBUNDLE
	};

	// Token: 0x04000794 RID: 1940
	public static readonly List<string> SupportedAssetBundleFormatExtensions = new List<string>
	{
		FileMagicNumbers.GetExtension(FileMagicNumbers.FileFormat.ASSETBUNDLE)
	};

	// Token: 0x020005AD RID: 1453
	public enum LoadType
	{
		// Token: 0x040025DC RID: 9692
		Auto,
		// Token: 0x040025DD RID: 9693
		Async,
		// Token: 0x040025DE RID: 9694
		Sync
	}

	// Token: 0x020005AE RID: 1454
	private interface ICustomLoadingAsset
	{
		// Token: 0x060038B8 RID: 14520
		void CheckCleanupLeaks();

		// Token: 0x060038B9 RID: 14521
		void ForceDestroy();

		// Token: 0x060038BA RID: 14522
		bool IsDLCLoaded(string DLCName);

		// Token: 0x060038BB RID: 14523
		List<string> GetKeys();

		// Token: 0x060038BC RID: 14524
		int GetCount();
	}

	// Token: 0x020005AF RID: 1455
	public class CustomLoadingToken : CustomLoadingManager.ICustomLoadingAsset
	{
		// Token: 0x060038BD RID: 14525 RVA: 0x0016E7EC File Offset: 0x0016C9EC
		public void Load(TokenSettings tokenSettings, Action<CustomTokenContainer> onLoadTokenFinish, CustomLoadingManager.LoadType loadType = CustomLoadingManager.LoadType.Auto, bool addLoading = true)
		{
			if (this.CustomTokens.ContainsKey(tokenSettings))
			{
				CustomLoadingManager.CustomLoadingToken.CustomTokenLoader customTokenLoader = this.CustomTokens[tokenSettings];
				if (!customTokenLoader.tokenCallbacks.Contains(onLoadTokenFinish))
				{
					customTokenLoader.tokenCallbacks.Add(onLoadTokenFinish);
				}
				if (customTokenLoader.isDone)
				{
					onLoadTokenFinish(customTokenLoader.tokenContainer);
					return;
				}
			}
			else
			{
				CustomLoadingManager.CustomLoadingToken.CustomTokenLoader customTokenLoader2 = new CustomLoadingManager.CustomLoadingToken.CustomTokenLoader();
				customTokenLoader2.tokenCallbacks.Add(onLoadTokenFinish);
				this.CustomTokens.Add(tokenSettings, customTokenLoader2);
				if (this.LoadingTokens.TryAddUnique(tokenSettings))
				{
					Singleton<CustomLoadingManager>.Instance.StartCoroutine(Singleton<CustomLoadingManager>.Instance.CoroutineLoadToken(tokenSettings, loadType, addLoading));
				}
			}
		}

		// Token: 0x060038BE RID: 14526 RVA: 0x0016E88C File Offset: 0x0016CA8C
		public void Finished(CustomTokenContainer customTokenContainer)
		{
			this.LoadingTokens.Remove(customTokenContainer.settings);
			if (this.CustomTokens.ContainsKey(customTokenContainer.settings))
			{
				CustomLoadingManager.CustomLoadingToken.CustomTokenLoader customTokenLoader = this.CustomTokens[customTokenContainer.settings];
				customTokenLoader.tokenContainer = customTokenContainer;
				for (int i = 0; i < customTokenLoader.tokenCallbacks.Count; i++)
				{
					Action<CustomTokenContainer> action = customTokenLoader.tokenCallbacks[i];
					if (action != null)
					{
						action(customTokenContainer);
						CustomObject customObject;
						if (customTokenContainer.mesh == null && (customObject = (action.Target as CustomObject)) != null)
						{
							if (i == 0)
							{
								customObject.bCustomUI = true;
							}
							if (customObject.FreezeDuringLoad && customObject.GetComponent<NetworkPhysicsObject>())
							{
								customObject.GetComponent<NetworkPhysicsObject>().IsLocked = true;
							}
						}
					}
					else
					{
						Debug.LogError("Callback function destroyed, but not cleaned up something went wrong");
					}
				}
				this.CheckCleanupToken(customTokenContainer.settings);
				if (customTokenContainer.mesh == null)
				{
					this.CustomTokens.Remove(customTokenContainer.settings);
					return;
				}
			}
			else
			{
				customTokenContainer.Cleanup(false);
			}
		}

		// Token: 0x060038BF RID: 14527 RVA: 0x0016E990 File Offset: 0x0016CB90
		public void Cleanup(TokenSettings settings, Action<CustomTokenContainer> onLoadTokenFinish)
		{
			if (this.CustomTokens.ContainsKey(settings))
			{
				CustomLoadingManager.CustomLoadingToken.CustomTokenLoader customTokenLoader = this.CustomTokens[settings];
				if (customTokenLoader.tokenCallbacks.Contains(onLoadTokenFinish))
				{
					customTokenLoader.tokenCallbacks.Remove(onLoadTokenFinish);
				}
				if (customTokenLoader.tokenCallbacks.Count == 0)
				{
					Wait.Frames(delegate
					{
						this.CheckCleanupToken(settings);
					}, 2);
					return;
				}
			}
			else
			{
				Debug.Log("Trying to cleanup Token that doesn't exist");
			}
		}

		// Token: 0x060038C0 RID: 14528 RVA: 0x0016EA1C File Offset: 0x0016CC1C
		private void CheckCleanupToken(TokenSettings settings)
		{
			if (this.CustomTokens.ContainsKey(settings))
			{
				CustomLoadingManager.CustomLoadingToken.CustomTokenLoader customTokenLoader = this.CustomTokens[settings];
				if (customTokenLoader.tokenCallbacks.Count == 0)
				{
					customTokenLoader.Cleanup();
					this.CustomTokens.Remove(settings);
				}
			}
		}

		// Token: 0x060038C1 RID: 14529 RVA: 0x0016EA64 File Offset: 0x0016CC64
		public void CheckCleanupLeaks()
		{
			using (Dictionary<TokenSettings, CustomLoadingManager.CustomLoadingToken.CustomTokenLoader>.Enumerator enumerator = this.CustomTokens.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<TokenSettings, CustomLoadingManager.CustomLoadingToken.CustomTokenLoader> pair = enumerator.Current;
					if (pair.Value != null)
					{
						List<Action<CustomTokenContainer>> tokenCallbacks = pair.Value.tokenCallbacks;
						for (int i = tokenCallbacks.Count - 1; i >= 0; i--)
						{
							Action<CustomTokenContainer> action = tokenCallbacks[i];
							if (action.Target == null || action.Target.Equals(null))
							{
								tokenCallbacks.RemoveAt(i);
							}
						}
						if (tokenCallbacks.Count == 0)
						{
							Wait.Frames(delegate
							{
								this.CheckCleanupToken(pair.Key);
							}, 1);
						}
					}
				}
			}
		}

		// Token: 0x060038C2 RID: 14530 RVA: 0x0016EB3C File Offset: 0x0016CD3C
		public void ForceDestroy()
		{
			foreach (KeyValuePair<TokenSettings, CustomLoadingManager.CustomLoadingToken.CustomTokenLoader> keyValuePair in this.CustomTokens)
			{
				if (keyValuePair.Value != null)
				{
					keyValuePair.Value.Cleanup();
				}
			}
			this.CustomTokens.Clear();
		}

		// Token: 0x060038C3 RID: 14531 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public bool IsDLCLoaded(string DLCName)
		{
			return false;
		}

		// Token: 0x060038C4 RID: 14532 RVA: 0x0016EBA8 File Offset: 0x0016CDA8
		public List<string> GetKeys()
		{
			return new List<string>();
		}

		// Token: 0x060038C5 RID: 14533 RVA: 0x0016EBAF File Offset: 0x0016CDAF
		public int GetCount()
		{
			return this.CustomTokens.Count;
		}

		// Token: 0x040025DF RID: 9695
		private readonly Dictionary<TokenSettings, CustomLoadingManager.CustomLoadingToken.CustomTokenLoader> CustomTokens = new Dictionary<TokenSettings, CustomLoadingManager.CustomLoadingToken.CustomTokenLoader>();

		// Token: 0x040025E0 RID: 9696
		private readonly List<TokenSettings> LoadingTokens = new List<TokenSettings>();

		// Token: 0x0200089A RID: 2202
		private class CustomTokenLoader
		{
			// Token: 0x1700087D RID: 2173
			// (get) Token: 0x0600425B RID: 16987 RVA: 0x001850C2 File Offset: 0x001832C2
			public bool isDone
			{
				get
				{
					return this.tokenContainer != null;
				}
			}

			// Token: 0x0600425C RID: 16988 RVA: 0x001850CD File Offset: 0x001832CD
			public void Cleanup()
			{
				CustomTokenContainer customTokenContainer = this.tokenContainer;
				if (customTokenContainer == null)
				{
					return;
				}
				customTokenContainer.Cleanup(false);
			}

			// Token: 0x04002F61 RID: 12129
			public readonly List<Action<CustomTokenContainer>> tokenCallbacks = new List<Action<CustomTokenContainer>>();

			// Token: 0x04002F62 RID: 12130
			public CustomTokenContainer tokenContainer;
		}
	}

	// Token: 0x020005B0 RID: 1456
	public abstract class CustomLoadingURLAsset<TContainer, TSettings> : CustomLoadingManager.ICustomLoadingAsset where TContainer : CustomContainer where TSettings : CustomLoadingManager.CustomLoadingURLAsset<!0, !1>.Settings, new()
	{
		// Token: 0x060038C7 RID: 14535 RVA: 0x0016EBDC File Offset: 0x0016CDDC
		public void Load(string url, Action<TContainer> callback, TSettings settings = default(TSettings))
		{
			string text;
			bool flag;
			bool flag2;
			TextCode.CheckURLCodes(url, out text, out flag, out flag2);
			if (this.CustomAssets.ContainsKey(text))
			{
				CustomLoadingManager.CustomLoadingURLAsset<TContainer, TSettings>.CustomLoader customLoader = this.CustomAssets[text];
				if (!customLoader.Callbacks.Contains(callback))
				{
					customLoader.Callbacks.Add(callback);
				}
				if (customLoader.IsDone)
				{
					callback(customLoader.Container);
					return;
				}
			}
			else
			{
				if (flag || flag2)
				{
					this.codeStrippedToNonStripped[text] = url;
				}
				CustomLoadingManager.CustomLoadingURLAsset<TContainer, TSettings>.CustomLoader customLoader2 = new CustomLoadingManager.CustomLoadingURLAsset<!0, !1>.CustomLoader();
				customLoader2.Callbacks.Add(callback);
				this.CustomAssets.Add(text, customLoader2);
				if (settings == null)
				{
					settings = this.defaultSettings;
				}
				if (this.LoadingAssets.TryAddUnique(text))
				{
					this.OnLoad(text, flag, settings);
				}
			}
		}

		// Token: 0x060038C8 RID: 14536
		protected abstract void OnLoad(string url, bool isVerifyCache, TSettings settings);

		// Token: 0x060038C9 RID: 14537 RVA: 0x0016EC9C File Offset: 0x0016CE9C
		public void Finished(TContainer customContainer)
		{
			this.LoadingAssets.Remove(customContainer.url);
			if (this.CustomAssets.ContainsKey(customContainer.url))
			{
				CustomLoadingManager.CustomLoadingURLAsset<TContainer, TSettings>.CustomLoader customLoader = this.CustomAssets[customContainer.url];
				customLoader.Container = customContainer;
				for (int i = 0; i < customLoader.Callbacks.Count; i++)
				{
					Action<TContainer> action = customLoader.Callbacks[i];
					if (action != null)
					{
						action(customContainer);
						CustomObject customObject;
						if (customContainer.IsError() && (customObject = (action.Target as CustomObject)) != null)
						{
							if (i == 0)
							{
								customObject.bCustomUI = true;
							}
							if (customObject.FreezeDuringLoad)
							{
								NetworkPhysicsObject component = customObject.GetComponent<NetworkPhysicsObject>();
								if (component)
								{
									component.IsLocked = true;
								}
							}
						}
					}
					else
					{
						Debug.LogError("Callback function destroyed, but not cleaned up something went wrong");
					}
				}
				this.CheckCleanup(customContainer.url);
				if (customContainer.IsError())
				{
					this.CustomAssets.Remove(customContainer.url);
					return;
				}
			}
			else
			{
				customContainer.Cleanup(false);
			}
		}

		// Token: 0x060038CA RID: 14538 RVA: 0x0016EDBC File Offset: 0x0016CFBC
		public void Cleanup(string url, Action<TContainer> callback, bool delayCleanup = true)
		{
			if (string.IsNullOrEmpty(url))
			{
				return;
			}
			foreach (KeyValuePair<string, string> keyValuePair in this.codeStrippedToNonStripped)
			{
				if (keyValuePair.Value == url && this.InternalCleanup(keyValuePair.Key, callback, delayCleanup))
				{
					return;
				}
			}
			if (this.InternalCleanup(url, callback, delayCleanup))
			{
				return;
			}
			if (Application.isEditor)
			{
				Debug.Log("Trying to cleanup url that doesn't exist: " + url);
			}
		}

		// Token: 0x060038CB RID: 14539 RVA: 0x0016EE58 File Offset: 0x0016D058
		private bool InternalCleanup(string url, Action<TContainer> callback, bool delayCleanup = true)
		{
			if (this.CustomAssets.ContainsKey(url))
			{
				CustomLoadingManager.CustomLoadingURLAsset<TContainer, TSettings>.CustomLoader customLoader = this.CustomAssets[url];
				bool flag = customLoader.Callbacks.Remove(callback);
				if (flag && customLoader.Callbacks.Count == 0)
				{
					if (delayCleanup)
					{
						Wait.Frames(delegate
						{
							this.CheckCleanup(url);
						}, 2);
						return flag;
					}
					this.CheckCleanup(url);
				}
				return flag;
			}
			return false;
		}

		// Token: 0x060038CC RID: 14540 RVA: 0x0016EEE0 File Offset: 0x0016D0E0
		private void CheckCleanup(string url)
		{
			if (this.CustomAssets.ContainsKey(url))
			{
				CustomLoadingManager.CustomLoadingURLAsset<TContainer, TSettings>.CustomLoader customLoader = this.CustomAssets[url];
				if (customLoader.Callbacks.Count == 0)
				{
					customLoader.Cleanup();
					this.CustomAssets.Remove(url);
					this.codeStrippedToNonStripped.Remove(url);
					Singleton<CustomLoadingManager>.Instance.CheckCleanupDLC(url);
				}
			}
		}

		// Token: 0x060038CD RID: 14541 RVA: 0x0016EF40 File Offset: 0x0016D140
		public bool ContainsURL(string url)
		{
			return this.CustomAssets.ContainsKey(url);
		}

		// Token: 0x060038CE RID: 14542 RVA: 0x0016EF50 File Offset: 0x0016D150
		public string NonCodeStrippedFromURL(string url)
		{
			string result;
			if (!this.codeStrippedToNonStripped.TryGetValue(url, out result))
			{
				return url;
			}
			return result;
		}

		// Token: 0x060038CF RID: 14543 RVA: 0x0016EF70 File Offset: 0x0016D170
		public void CheckCleanupLeaks()
		{
			using (Dictionary<string, CustomLoadingManager.CustomLoadingURLAsset<TContainer, TSettings>.CustomLoader>.Enumerator enumerator = this.CustomAssets.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, CustomLoadingManager.CustomLoadingURLAsset<TContainer, TSettings>.CustomLoader> pair = enumerator.Current;
					if (pair.Value != null)
					{
						List<Action<TContainer>> callbacks = pair.Value.Callbacks;
						for (int i = callbacks.Count - 1; i >= 0; i--)
						{
							Action<TContainer> action = callbacks[i];
							if (action == null || action.Target == null || action.Target.Equals(null))
							{
								callbacks.RemoveAt(i);
							}
						}
						if (callbacks.Count == 0)
						{
							Wait.Frames(delegate
							{
								this.CheckCleanup(pair.Key);
							}, 1);
						}
					}
				}
			}
		}

		// Token: 0x060038D0 RID: 14544 RVA: 0x0016F04C File Offset: 0x0016D24C
		public void ForceDestroy()
		{
			foreach (KeyValuePair<string, CustomLoadingManager.CustomLoadingURLAsset<TContainer, TSettings>.CustomLoader> keyValuePair in this.CustomAssets)
			{
				CustomLoadingManager.CustomLoadingURLAsset<!0, !1>.CustomLoader value = keyValuePair.Value;
				if (value != null)
				{
					value.Cleanup();
				}
			}
			this.CustomAssets.Clear();
		}

		// Token: 0x060038D1 RID: 14545 RVA: 0x0016F0B8 File Offset: 0x0016D2B8
		public bool IsDLCLoaded(string DLCName)
		{
			foreach (KeyValuePair<string, CustomLoadingManager.CustomLoadingURLAsset<TContainer, TSettings>.CustomLoader> keyValuePair in this.CustomAssets)
			{
				string b;
				string text;
				if (DLCManager.URLisDLC(keyValuePair.Key, out b, out text) && DLCName == b)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060038D2 RID: 14546 RVA: 0x0016F128 File Offset: 0x0016D328
		public List<string> GetKeys()
		{
			return this.CustomAssets.Keys.ToList<string>();
		}

		// Token: 0x060038D3 RID: 14547 RVA: 0x0016F13A File Offset: 0x0016D33A
		public int GetCount()
		{
			return this.CustomAssets.Count;
		}

		// Token: 0x040025E1 RID: 9697
		private readonly TSettings defaultSettings = Activator.CreateInstance<TSettings>();

		// Token: 0x040025E2 RID: 9698
		protected readonly Dictionary<string, CustomLoadingManager.CustomLoadingURLAsset<TContainer, TSettings>.CustomLoader> CustomAssets = new Dictionary<string, CustomLoadingManager.CustomLoadingURLAsset<!0, !1>.CustomLoader>();

		// Token: 0x040025E3 RID: 9699
		protected readonly Dictionary<string, string> codeStrippedToNonStripped = new Dictionary<string, string>();

		// Token: 0x040025E4 RID: 9700
		protected readonly List<string> LoadingAssets = new List<string>();

		// Token: 0x0200089D RID: 2205
		protected class CustomLoader
		{
			// Token: 0x1700087E RID: 2174
			// (get) Token: 0x06004262 RID: 16994 RVA: 0x0018511E File Offset: 0x0018331E
			public bool IsDone
			{
				get
				{
					return this.Container != null;
				}
			}

			// Token: 0x06004263 RID: 16995 RVA: 0x0018512E File Offset: 0x0018332E
			public void Cleanup()
			{
				!0 ! = this.Container;
				if (! == null)
				{
					return;
				}
				!.Cleanup(false);
			}

			// Token: 0x04002F67 RID: 12135
			public TContainer Container;

			// Token: 0x04002F68 RID: 12136
			public readonly List<Action<TContainer>> Callbacks = new List<Action<!0>>();
		}

		// Token: 0x0200089E RID: 2206
		public class Settings
		{
			// Token: 0x04002F69 RID: 12137
			public CustomLoadingManager.LoadType loadType;

			// Token: 0x04002F6A RID: 12138
			public bool addLoading = true;
		}
	}

	// Token: 0x020005B1 RID: 1457
	public class CustomLoadingTexture : CustomLoadingManager.CustomLoadingURLAsset<CustomTextureContainer, CustomLoadingManager.CustomLoadingTexture.TextureSettings>
	{
		// Token: 0x060038D5 RID: 14549 RVA: 0x0016F17C File Offset: 0x0016D37C
		public void Load(string url, Action<CustomTextureContainer> callback, bool compress = true, bool normalMap = false, bool linear = false, bool mipMaps = true, bool addLoading = true, bool readable = false, int imageMaxSize = 8192, CustomLoadingManager.LoadType loadType = CustomLoadingManager.LoadType.Auto)
		{
			this.textureSettings.compress = compress;
			this.textureSettings.normalMap = normalMap;
			this.textureSettings.linear = linear;
			this.textureSettings.mipMaps = mipMaps;
			this.textureSettings.addLoading = addLoading;
			this.textureSettings.readable = readable;
			this.textureSettings.imageMaxSize = imageMaxSize;
			this.textureSettings.loadType = loadType;
			base.Load(url, callback, this.textureSettings);
		}

		// Token: 0x060038D6 RID: 14550 RVA: 0x0016F200 File Offset: 0x0016D400
		protected override void OnLoad(string url, bool isVerifyCache, CustomLoadingManager.CustomLoadingTexture.TextureSettings settings)
		{
			Singleton<CustomLoadingManager>.Instance.StartCoroutine(Singleton<CustomLoadingManager>.Instance.CoroutineLoadTexture(url, isVerifyCache, settings.loadType, settings.imageMaxSize, settings.mipMaps, settings.addLoading, settings.linear, settings.compress, settings.normalMap, settings.readable));
		}

		// Token: 0x040025E5 RID: 9701
		private readonly CustomLoadingManager.CustomLoadingTexture.TextureSettings textureSettings = new CustomLoadingManager.CustomLoadingTexture.TextureSettings();

		// Token: 0x020008A1 RID: 2209
		public class TextureSettings : CustomLoadingManager.CustomLoadingURLAsset<CustomTextureContainer, CustomLoadingManager.CustomLoadingTexture.TextureSettings>.Settings
		{
			// Token: 0x04002F6F RID: 12143
			public bool compress = true;

			// Token: 0x04002F70 RID: 12144
			public bool normalMap;

			// Token: 0x04002F71 RID: 12145
			public bool linear;

			// Token: 0x04002F72 RID: 12146
			public bool mipMaps = true;

			// Token: 0x04002F73 RID: 12147
			public bool readable;

			// Token: 0x04002F74 RID: 12148
			public int imageMaxSize = 8192;
		}
	}

	// Token: 0x020005B2 RID: 1458
	public class CustomLoadingMesh : CustomLoadingManager.CustomLoadingURLAsset<CustomMeshContainer, CustomLoadingManager.CustomLoadingMesh.MeshSettings>
	{
		// Token: 0x060038D8 RID: 14552 RVA: 0x0016F267 File Offset: 0x0016D467
		protected override void OnLoad(string url, bool isVerifyCache, CustomLoadingManager.CustomLoadingMesh.MeshSettings settings)
		{
			Singleton<CustomLoadingManager>.Instance.StartCoroutine(Singleton<CustomLoadingManager>.Instance.CoroutineLoadMesh(url, isVerifyCache, settings.loadType, settings.addLoading));
		}

		// Token: 0x020008A2 RID: 2210
		public class MeshSettings : CustomLoadingManager.CustomLoadingURLAsset<CustomMeshContainer, CustomLoadingManager.CustomLoadingMesh.MeshSettings>.Settings
		{
		}
	}

	// Token: 0x020005B3 RID: 1459
	public class CustomLoadingAssetbundle : CustomLoadingManager.CustomLoadingURLAsset<CustomAssetbundleContainer, CustomLoadingManager.CustomLoadingAssetbundle.AssetbundleSettings>
	{
		// Token: 0x060038DA RID: 14554 RVA: 0x0016F294 File Offset: 0x0016D494
		protected override void OnLoad(string url, bool isVerifyCache, CustomLoadingManager.CustomLoadingAssetbundle.AssetbundleSettings settings)
		{
			Singleton<CustomLoadingManager>.Instance.StartCoroutine(Singleton<CustomLoadingManager>.Instance.CoroutineLoadAssetbundle(url, isVerifyCache, settings.loadType, settings.addLoading));
		}

		// Token: 0x020008A3 RID: 2211
		public class AssetbundleSettings : CustomLoadingManager.CustomLoadingURLAsset<CustomAssetbundleContainer, CustomLoadingManager.CustomLoadingAssetbundle.AssetbundleSettings>.Settings
		{
		}
	}

	// Token: 0x020005B4 RID: 1460
	public class CustomLoadingAudio : CustomLoadingManager.CustomLoadingURLAsset<CustomAudioContainer, CustomLoadingManager.CustomLoadingAudio.AudioSettings>
	{
		// Token: 0x060038DC RID: 14556 RVA: 0x0016F2C1 File Offset: 0x0016D4C1
		public void Load(string url, Action<CustomAudioContainer> callback, string clipName, CustomLoadingManager.LoadType loadType = CustomLoadingManager.LoadType.Auto, bool addLoading = true)
		{
			this.audioSettings.audioName = clipName;
			this.audioSettings.loadType = loadType;
			this.audioSettings.addLoading = addLoading;
			base.Load(url, callback, this.audioSettings);
		}

		// Token: 0x060038DD RID: 14557 RVA: 0x0016F2F7 File Offset: 0x0016D4F7
		protected override void OnLoad(string url, bool isVerifyCache, CustomLoadingManager.CustomLoadingAudio.AudioSettings settings)
		{
			Singleton<CustomLoadingManager>.Instance.StartCoroutine(Singleton<CustomLoadingManager>.Instance.CoroutineLoadAudio(url, isVerifyCache, settings.loadType, settings.addLoading, settings.audioName));
		}

		// Token: 0x040025E6 RID: 9702
		private readonly CustomLoadingManager.CustomLoadingAudio.AudioSettings audioSettings = new CustomLoadingManager.CustomLoadingAudio.AudioSettings();

		// Token: 0x020008A4 RID: 2212
		public class AudioSettings : CustomLoadingManager.CustomLoadingURLAsset<CustomAudioContainer, CustomLoadingManager.CustomLoadingAudio.AudioSettings>.Settings
		{
			// Token: 0x04002F75 RID: 12149
			public string audioName;
		}
	}

	// Token: 0x020005B5 RID: 1461
	public class CustomLoadingPDF : CustomLoadingManager.CustomLoadingURLAsset<CustomPDFContainer, CustomLoadingManager.CustomLoadingPDF.PDFSettings>
	{
		// Token: 0x060038DF RID: 14559 RVA: 0x0016F335 File Offset: 0x0016D535
		public void Load(string url, Action<CustomPDFContainer> callback, string password = "", CustomLoadingManager.LoadType loadType = CustomLoadingManager.LoadType.Auto, bool addLoading = true)
		{
			this.pdfSettings.password = password;
			this.pdfSettings.loadType = loadType;
			this.pdfSettings.addLoading = addLoading;
			base.Load(url, callback, this.pdfSettings);
		}

		// Token: 0x060038E0 RID: 14560 RVA: 0x0016F36B File Offset: 0x0016D56B
		protected override void OnLoad(string url, bool isVerifyCache, CustomLoadingManager.CustomLoadingPDF.PDFSettings settings)
		{
			Singleton<CustomLoadingManager>.Instance.StartCoroutine(Singleton<CustomLoadingManager>.Instance.CoroutineLoadPDF(url, isVerifyCache, settings.loadType, settings.addLoading, settings.password));
		}

		// Token: 0x040025E7 RID: 9703
		private readonly CustomLoadingManager.CustomLoadingPDF.PDFSettings pdfSettings = new CustomLoadingManager.CustomLoadingPDF.PDFSettings();

		// Token: 0x020008A5 RID: 2213
		public class PDFSettings : CustomLoadingManager.CustomLoadingURLAsset<CustomPDFContainer, CustomLoadingManager.CustomLoadingPDF.PDFSettings>.Settings
		{
			// Token: 0x04002F76 RID: 12150
			public string password = "";
		}
	}

	// Token: 0x020005B6 RID: 1462
	public class CustomLoadingText : CustomLoadingManager.CustomLoadingURLAsset<CustomTextContainer, CustomLoadingManager.CustomLoadingText.TextSettings>
	{
		// Token: 0x060038E2 RID: 14562 RVA: 0x0016F3A9 File Offset: 0x0016D5A9
		public void Load(string url, Action<CustomTextContainer> callback, bool forceFetch = false, CustomLoadingManager.LoadType loadType = CustomLoadingManager.LoadType.Auto, bool addLoading = true)
		{
			this.textSettings.forceFetch = forceFetch;
			this.textSettings.loadType = loadType;
			this.textSettings.addLoading = addLoading;
			base.Load(url, callback, this.textSettings);
		}

		// Token: 0x060038E3 RID: 14563 RVA: 0x0016F3DF File Offset: 0x0016D5DF
		protected override void OnLoad(string url, bool isVerifyCache, CustomLoadingManager.CustomLoadingText.TextSettings settings)
		{
			Singleton<CustomLoadingManager>.Instance.StartCoroutine(Singleton<CustomLoadingManager>.Instance.CoroutineLoadText(url, isVerifyCache, settings.loadType, settings.addLoading, settings.forceFetch));
		}

		// Token: 0x040025E8 RID: 9704
		private readonly CustomLoadingManager.CustomLoadingText.TextSettings textSettings = new CustomLoadingManager.CustomLoadingText.TextSettings();

		// Token: 0x020008A6 RID: 2214
		public class TextSettings : CustomLoadingManager.CustomLoadingURLAsset<CustomTextContainer, CustomLoadingManager.CustomLoadingText.TextSettings>.Settings
		{
			// Token: 0x04002F77 RID: 12151
			public bool forceFetch;
		}
	}

	// Token: 0x020005B7 RID: 1463
	public class CustomLoadingUIAssetbundle : CustomLoadingManager.CustomLoadingURLAsset<CustomUIAssetbundleContainer, CustomLoadingManager.CustomLoadingUIAssetbundle.UIAssetbundleSettings>
	{
		// Token: 0x060038E5 RID: 14565 RVA: 0x0016F41D File Offset: 0x0016D61D
		protected override void OnLoad(string url, bool isVerifyCache, CustomLoadingManager.CustomLoadingUIAssetbundle.UIAssetbundleSettings settings)
		{
			Singleton<CustomLoadingManager>.Instance.StartCoroutine(Singleton<CustomLoadingManager>.Instance.CoroutineLoadUIAssetbundle(url, isVerifyCache, settings.loadType, settings.addLoading));
		}

		// Token: 0x020008A7 RID: 2215
		public class UIAssetbundleSettings : CustomLoadingManager.CustomLoadingURLAsset<CustomUIAssetbundleContainer, CustomLoadingManager.CustomLoadingUIAssetbundle.UIAssetbundleSettings>.Settings
		{
		}
	}
}
