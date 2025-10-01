using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NewNet;
using UnityEngine;

// Token: 0x020000EE RID: 238
public class DLCManager : Singleton<DLCManager>
{
	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x06000BBF RID: 3007 RVA: 0x00050B55 File Offset: 0x0004ED55
	public static List<DLCWebsiteInfo> DLCInfos
	{
		get
		{
			return Singleton<DLCManager>.Instance.DLCWebsites;
		}
	}

	// Token: 0x06000BC0 RID: 3008 RVA: 0x00050B61 File Offset: 0x0004ED61
	protected override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06000BC1 RID: 3009 RVA: 0x00050B69 File Offset: 0x0004ED69
	private void Start()
	{
		DLCManager.HostDLCs.Clear();
		this.CheckSales();
	}

	// Token: 0x06000BC2 RID: 3010 RVA: 0x00050B7C File Offset: 0x0004ED7C
	private void OnDestroy()
	{
		foreach (KeyValuePair<string, DLCManager.DLCBundleData> keyValuePair in this.LoadedDLCBundles)
		{
			this.CleanupDLC(keyValuePair.Key, false);
		}
	}

	// Token: 0x06000BC3 RID: 3011 RVA: 0x00050BD8 File Offset: 0x0004EDD8
	public static bool URLisDLC(string URL)
	{
		string text;
		string text2;
		return DLCManager.URLisDLC(URL, out text, out text2);
	}

	// Token: 0x06000BC4 RID: 3012 RVA: 0x00050BF0 File Offset: 0x0004EDF0
	public static bool URLisDLC(string URL, out string DLCName, out string newURL)
	{
		DLCName = null;
		newURL = URL;
		if (!newURL.StartsWith("<"))
		{
			return false;
		}
		for (int i = 0; i < DLCManager.DLCInfos.Count; i++)
		{
			DLCWebsiteInfo dlcwebsiteInfo = DLCManager.DLCInfos[i];
			if (newURL.StartsWith(dlcwebsiteInfo.SaveTag))
			{
				DLCName = dlcwebsiteInfo.Name;
				newURL = newURL.Replace(dlcwebsiteInfo.SaveTag, "");
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000BC5 RID: 3013 RVA: 0x00050C64 File Offset: 0x0004EE64
	public static DLCWebsiteInfo NameToDLCInfo(string DLCName)
	{
		for (int i = 0; i < DLCManager.DLCInfos.Count; i++)
		{
			DLCWebsiteInfo dlcwebsiteInfo = DLCManager.DLCInfos[i];
			if (dlcwebsiteInfo.Name == DLCName)
			{
				return dlcwebsiteInfo;
			}
		}
		Debug.LogError("DLC not found: " + DLCName);
		return null;
	}

	// Token: 0x06000BC6 RID: 3014 RVA: 0x00050CB3 File Offset: 0x0004EEB3
	public static bool CanWeLoadThisDLC(string DLCName)
	{
		return SteamManager.IsSubscribedApp(DLCManager.NameToDLCInfo(DLCName).AppId) || DLCManager.HostDLCs.Contains(DLCName);
	}

	// Token: 0x06000BC7 RID: 3015 RVA: 0x00050CD4 File Offset: 0x0004EED4
	public static string GetAppId(string DLCName)
	{
		return DLCManager.NameToDLCInfo(DLCName).AppId.ToString();
	}

	// Token: 0x06000BC8 RID: 3016 RVA: 0x00050CE8 File Offset: 0x0004EEE8
	public static List<string> GetOwnedDLCs()
	{
		List<string> list = new List<string>();
		for (int i = 0; i < DLCManager.DLCInfos.Count; i++)
		{
			DLCWebsiteInfo dlcwebsiteInfo = DLCManager.DLCInfos[i];
			if (SteamManager.IsSubscribedApp(dlcwebsiteInfo.AppId))
			{
				list.Add(dlcwebsiteInfo.Name);
			}
		}
		return list;
	}

	// Token: 0x06000BC9 RID: 3017 RVA: 0x00050D38 File Offset: 0x0004EF38
	public static void SetHostOwnedDLCs(List<string> hostDLCs)
	{
		DLCManager.HostDLCs = hostDLCs;
		if (hostDLCs.Count == 0)
		{
			Chat.Log("Host owns no DLCs.", Colour.Green, ChatMessageType.Game, false);
			return;
		}
		string text = "Host owns DLC(s): " + DLCManager.HostDLCs[0];
		for (int i = 1; i < DLCManager.HostDLCs.Count; i++)
		{
			text = text + ", " + DLCManager.HostDLCs[i];
		}
		text += ".";
		Chat.Log(text, Colour.Green, ChatMessageType.Game, false);
	}

	// Token: 0x06000BCA RID: 3018 RVA: 0x00050DC0 File Offset: 0x0004EFC0
	public CustomTextureContainer LoadDLCTexture(string DLCName, string Name, string url)
	{
		DLCManager.DLCBundleData dlcbundleData;
		if (!this.LoadedDLCBundles.TryGetValue(DLCName, out dlcbundleData))
		{
			base.StartCoroutine(this.LoadDLCAssetBundle(DLCName));
			return null;
		}
		if (dlcbundleData.DLCBundle != null)
		{
			CustomTextureContainer result;
			if (dlcbundleData.CachedTextures.TryGetValue(Name, out result))
			{
				return result;
			}
			dlcbundleData.CachedTextures.Add(Name, null);
			base.StartCoroutine(this.LoadAsync(dlcbundleData, Name, url, ".jpg", new Action<DLCManager.DLCBundleData, string, string, object>(this.AddCacheTexture)));
		}
		return null;
	}

	// Token: 0x06000BCB RID: 3019 RVA: 0x00050E3C File Offset: 0x0004F03C
	private void AddCacheTexture(DLCManager.DLCBundleData dlcBundleData, string Name, string url, object obj)
	{
		Texture2D texture2D;
		if ((texture2D = (obj as Texture2D)) != null && texture2D != null)
		{
			TextureScript.ApplyTextSettings(texture2D);
			dlcBundleData.CachedTextures[Name] = new CustomTextureContainer(url, texture2D, (float)texture2D.width / (float)texture2D.height);
			return;
		}
		TextAsset textAsset;
		if ((textAsset = (obj as TextAsset)) != null && textAsset != null)
		{
			base.StartCoroutine(this.LoadAsyncMaterialTexture(dlcBundleData, Name, url, textAsset));
			return;
		}
		Debug.LogError(string.Concat(new object[]
		{
			"DLC Texture == Null: ",
			Name,
			" Type: ",
			obj.GetType()
		}));
	}

	// Token: 0x06000BCC RID: 3020 RVA: 0x00050EDA File Offset: 0x0004F0DA
	private IEnumerator LoadAsyncMaterialTexture(DLCManager.DLCBundleData dlcBundleData, string Name, string url, TextAsset textAsset)
	{
		AssetBundle assetBundle = null;
		AssetBundleCreateRequest bundleCreateRequest = AssetBundle.LoadFromMemoryAsync(textAsset.bytes);
		yield return bundleCreateRequest;
		try
		{
			assetBundle = bundleCreateRequest.assetBundle;
		}
		catch (Exception e)
		{
			Chat.LogException("Loading Image AssetBundle Material", e, true, false);
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
					Texture mainTexture = material.mainTexture;
					if (mainTexture != null)
					{
						if (mainTexture.filterMode != FilterMode.Point)
						{
							TextureScript.ApplyTextSettings(mainTexture);
						}
						dlcBundleData.CachedTextures[Name] = new CustomTextureContainer(url, mainTexture, (float)mainTexture.width / (float)mainTexture.height, assetBundle, material);
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
		yield break;
	}

	// Token: 0x06000BCD RID: 3021 RVA: 0x00050F00 File Offset: 0x0004F100
	public CustomMeshContainer LoadDLCMesh(string DLCName, string Name, string url)
	{
		DLCManager.DLCBundleData dlcbundleData;
		if (!this.LoadedDLCBundles.TryGetValue(DLCName, out dlcbundleData))
		{
			base.StartCoroutine(this.LoadDLCAssetBundle(DLCName));
			return null;
		}
		if (dlcbundleData.DLCBundle != null)
		{
			CustomMeshContainer result;
			if (dlcbundleData.CachedMeshes.TryGetValue(Name, out result))
			{
				return result;
			}
			dlcbundleData.CachedMeshes.Add(Name, null);
			if (Name == "null")
			{
				dlcbundleData.CachedMeshes[Name] = new CustomMeshContainer(url, new Mesh());
				return dlcbundleData.CachedMeshes[Name];
			}
			base.StartCoroutine(this.LoadAsync(dlcbundleData, Name, url, ".obj", new Action<DLCManager.DLCBundleData, string, string, object>(this.AddCacheMesh)));
		}
		return null;
	}

	// Token: 0x06000BCE RID: 3022 RVA: 0x00050FB0 File Offset: 0x0004F1B0
	private void AddCacheMesh(DLCManager.DLCBundleData dlcBundleData, string Name, string url, object obj)
	{
		GameObject gameObject = obj as GameObject;
		if (!(gameObject != null) || gameObject.transform.childCount <= 0)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"DLC Mesh == null: ",
				Name,
				" Type: ",
				obj.GetType()
			}));
			return;
		}
		MeshFilter[] componentsInChildren = gameObject.GetComponentsInChildren<MeshFilter>(true);
		if (componentsInChildren.Length == 1)
		{
			dlcBundleData.CachedMeshes[Name] = new CustomMeshContainer(url, componentsInChildren[0].sharedMesh);
			return;
		}
		CombineInstance[] array = new CombineInstance[componentsInChildren.Length];
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			array[i].mesh = componentsInChildren[i].sharedMesh;
			array[i].transform = componentsInChildren[i].transform.localToWorldMatrix;
		}
		Mesh mesh = new Mesh();
		mesh.CombineMeshes(array, true);
		dlcBundleData.CachedMeshes[Name] = new CustomMeshContainer(url, mesh);
	}

	// Token: 0x06000BCF RID: 3023 RVA: 0x000510A8 File Offset: 0x0004F2A8
	public CustomAssetbundleContainer LoadDLCAssetBundle(string DLCName, string Name, string url)
	{
		DLCManager.DLCBundleData dlcbundleData;
		if (!this.LoadedDLCBundles.TryGetValue(DLCName, out dlcbundleData))
		{
			base.StartCoroutine(this.LoadDLCAssetBundle(DLCName));
			return null;
		}
		if (dlcbundleData.DLCBundle != null)
		{
			CustomAssetbundleContainer result;
			if (dlcbundleData.CachedAssetBundles.TryGetValue(Name, out result))
			{
				return result;
			}
			dlcbundleData.CachedAssetBundles.Add(Name, null);
			base.StartCoroutine(this.LoadAsyncAssetBundle(dlcbundleData, Name, url));
		}
		return null;
	}

	// Token: 0x06000BD0 RID: 3024 RVA: 0x00051114 File Offset: 0x0004F314
	public CustomAudioContainer LoadDLCAudio(string DLCName, string Name, string url)
	{
		DLCManager.DLCBundleData dlcbundleData;
		if (!this.LoadedDLCBundles.TryGetValue(DLCName, out dlcbundleData))
		{
			base.StartCoroutine(this.LoadDLCAssetBundle(DLCName));
			return null;
		}
		if (dlcbundleData.DLCBundle != null)
		{
			CustomAudioContainer result;
			if (dlcbundleData.CachedAudioclips.TryGetValue(Name, out result))
			{
				return result;
			}
			dlcbundleData.CachedAudioclips.Add(Name, null);
			base.StartCoroutine(this.LoadAsync(dlcbundleData, Name, url, ".mp3", new Action<DLCManager.DLCBundleData, string, string, object>(this.AddCacheAudio)));
		}
		return null;
	}

	// Token: 0x06000BD1 RID: 3025 RVA: 0x00051190 File Offset: 0x0004F390
	private void AddCacheAudio(DLCManager.DLCBundleData dlcBundleData, string Name, string url, object obj)
	{
		AudioClip audioClip = obj as AudioClip;
		if (audioClip != null)
		{
			dlcBundleData.CachedAudioclips[Name] = new CustomAudioContainer(url, audioClip);
			return;
		}
		Debug.LogError(string.Concat(new object[]
		{
			"DLC AudioClip == Null: ",
			Name,
			" Type: ",
			obj.GetType()
		}));
	}

	// Token: 0x06000BD2 RID: 3026 RVA: 0x000511F0 File Offset: 0x0004F3F0
	private string CheckFileExtensionsAudio(DLCManager.DLCBundleData dlcBundleData, string LoadName)
	{
		foreach (string extension in CustomLoadingManager.SupportedAudioFormatExtensions)
		{
			if (dlcBundleData.DLCBundle.Contains(Path.ChangeExtension(LoadName, extension)))
			{
				LoadName = Path.ChangeExtension(LoadName, ".ogg");
			}
		}
		return LoadName;
	}

	// Token: 0x06000BD3 RID: 3027 RVA: 0x00051260 File Offset: 0x0004F460
	private IEnumerator LoadAsync(DLCManager.DLCBundleData dlcBundleData, string Name, string url, string Extension, Action<DLCManager.DLCBundleData, string, string, object> Function)
	{
		string text = Name;
		if (!Path.HasExtension(Name))
		{
			text += Extension;
		}
		if (!dlcBundleData.DLCBundle.Contains(text))
		{
			if (Extension == ".jpg" && dlcBundleData.DLCBundle.Contains(Path.ChangeExtension(text, ".png")))
			{
				text = Path.ChangeExtension(text, ".png");
			}
			else
			{
				if (!(Extension == ".mp3") || !(this.CheckFileExtensionsAudio(dlcBundleData, text) != text))
				{
					Debug.LogError("DLCBundle doesn't contain: " + Name);
					yield break;
				}
				text = this.CheckFileExtensionsAudio(dlcBundleData, text);
			}
		}
		AssetBundleRequest assetBundleRequest = dlcBundleData.DLCBundle.LoadAssetAsync<object>(text);
		yield return assetBundleRequest;
		if (!this.LoadedDLCBundles.ContainsValue(dlcBundleData))
		{
			UnityEngine.Object.Destroy(assetBundleRequest.asset);
			yield break;
		}
		if (assetBundleRequest.asset != null)
		{
			Function(dlcBundleData, Name, url, assetBundleRequest.asset);
		}
		else
		{
			Debug.LogError("DLC Loaded Asset == Null: " + Name);
		}
		yield break;
	}

	// Token: 0x06000BD4 RID: 3028 RVA: 0x00051294 File Offset: 0x0004F494
	private IEnumerator LoadAsyncAssetBundle(DLCManager.DLCBundleData dlcBundleData, string Name, string url)
	{
		string text = Name;
		if (!Path.HasExtension(Name))
		{
			text += ".unity3d";
		}
		if (!dlcBundleData.DLCBundle.Contains(text))
		{
			Debug.LogError("DLCBundle doesn't contain: " + Name);
			yield break;
		}
		AssetBundleRequest assetBundleTextRequest = dlcBundleData.DLCBundle.LoadAssetAsync<TextAsset>(text);
		yield return assetBundleTextRequest;
		TextAsset textAsset = assetBundleTextRequest.asset as TextAsset;
		if (textAsset == null)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"DLC TextAsset == null: ",
				Name,
				" Type: ",
				assetBundleTextRequest.asset.GetType()
			}));
			yield break;
		}
		AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromMemoryAsync(textAsset.bytes);
		yield return assetBundleCreateRequest;
		AssetBundle assetBundle = assetBundleCreateRequest.assetBundle;
		if (!this.LoadedDLCBundles.ContainsValue(dlcBundleData))
		{
			if (assetBundle != null)
			{
				assetBundle.Unload(true);
			}
			yield break;
		}
		AssetBundleRequest assetBundleRequest = assetBundle.LoadAllAssetsAsync<object>();
		yield return assetBundleRequest;
		UnityEngine.Object[] allAssets = assetBundleRequest.allAssets;
		List<GameObject> list = new List<GameObject>();
		List<Material> list2 = new List<Material>();
		foreach (UnityEngine.Object @object in allAssets)
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
		GameObject[] array = list.ToArray();
		CustomAssetbundle.CleanupAssetBundleGameObjects(array);
		CustomAssetbundle.CleanupAssetBundleMaterials(list2);
		if (!this.LoadedDLCBundles.ContainsValue(dlcBundleData))
		{
			if (assetBundle != null)
			{
				assetBundle.Unload(true);
			}
			if (array != null)
			{
				for (int j = 0; j < array.Length; j++)
				{
					UnityEngine.Object.Destroy(array[j]);
				}
			}
			yield break;
		}
		dlcBundleData.CachedAssetBundles[Name] = new CustomAssetbundleContainer(url, assetBundle, array);
		yield break;
	}

	// Token: 0x06000BD5 RID: 3029 RVA: 0x000512B8 File Offset: 0x0004F4B8
	private IEnumerator LoadDLCAssetBundle(string DLCName)
	{
		DLCManager.DLCBundleData dlcBundleData = new DLCManager.DLCBundleData
		{
			name = DLCName
		};
		this.LoadedDLCBundles.Add(DLCName, dlcBundleData);
		if (!DLCManager.CanWeLoadThisDLC(DLCName))
		{
			Chat.LogError("You do not own DLC " + DLCName + ". Cannot load.", true);
			TTSUtilities.OpenURL("http://store.steampowered.com/app/" + DLCManager.GetAppId(DLCName));
			yield break;
		}
		DLCWebsiteInfo dlcwebsiteInfo = DLCManager.NameToDLCInfo(DLCName);
		string url = string.IsNullOrEmpty(dlcwebsiteInfo.AssetBundleURL) ? (dlcwebsiteInfo.Name + ".unity3d") : dlcwebsiteInfo.AssetBundleURL;
		string ConvertedURL = CustomCache.CheckDLCAssetBundleExist(url, true, true);
		DataRequest dataRequest = DataRequest.GetAssetBundle(ConvertedURL, false);
		while (!dataRequest.isDone)
		{
			yield return null;
			Singleton<UILoading>.Instance.AddProgress("DLC " + DLCName, dataRequest.progress);
		}
		bool flag = false;
		if (dataRequest.isError)
		{
			Chat.LogError(DLCName + " loading error: " + dataRequest.error, true);
		}
		else
		{
			if (!this.LoadedDLCBundles.ContainsValue(dlcBundleData))
			{
				try
				{
					dataRequest.assetBundle.Unload(true);
					goto IL_1CA;
				}
				catch (Exception)
				{
					goto IL_1CA;
				}
			}
			try
			{
				dlcBundleData.DLCBundle = dataRequest.assetBundle;
				flag = true;
			}
			catch (Exception e)
			{
				Chat.LogException("loading DLC AssetBundle", e, true, false);
			}
		}
		IL_1CA:
		if (flag && !DataRequest.IsLocalFile(ConvertedURL))
		{
			Task task = CustomCache.SaveDLCAssetBundleAsync(dataRequest.data, url);
			while (!task.IsCompleted)
			{
				yield return null;
			}
			task = null;
		}
		dataRequest.Dispose();
		yield break;
	}

	// Token: 0x06000BD6 RID: 3030 RVA: 0x000512D0 File Offset: 0x0004F4D0
	public void CleanupDLC(string DLCName, bool removeDictionary = true)
	{
		DLCManager.DLCBundleData dlcbundleData;
		if (!this.LoadedDLCBundles.TryGetValue(DLCName, out dlcbundleData))
		{
			return;
		}
		Debug.Log("CleanupDLC: " + DLCName);
		if (removeDictionary)
		{
			this.LoadedDLCBundles.Remove(DLCName);
		}
		AssetBundle dlcbundle = dlcbundleData.DLCBundle;
		Dictionary<string, CustomTextureContainer> cachedTextures = dlcbundleData.CachedTextures;
		Dictionary<string, CustomMeshContainer> cachedMeshes = dlcbundleData.CachedMeshes;
		Dictionary<string, CustomAssetbundleContainer> cachedAssetBundles = dlcbundleData.CachedAssetBundles;
		Dictionary<string, CustomAudioContainer> cachedAudioclips = dlcbundleData.CachedAudioclips;
		if (dlcbundle != null)
		{
			dlcbundle.Unload(true);
		}
		foreach (KeyValuePair<string, CustomTextureContainer> keyValuePair in cachedTextures)
		{
			CustomTextureContainer value = keyValuePair.Value;
			if (value != null)
			{
				value.Cleanup(true);
			}
		}
		foreach (KeyValuePair<string, CustomMeshContainer> keyValuePair2 in cachedMeshes)
		{
			CustomMeshContainer value2 = keyValuePair2.Value;
			if (value2 != null)
			{
				value2.Cleanup(true);
			}
		}
		foreach (KeyValuePair<string, CustomAssetbundleContainer> keyValuePair3 in cachedAssetBundles)
		{
			CustomAssetbundleContainer value3 = keyValuePair3.Value;
			if (value3 != null)
			{
				value3.Cleanup(true);
			}
		}
		foreach (KeyValuePair<string, CustomAudioContainer> keyValuePair4 in cachedAudioclips)
		{
			CustomAudioContainer value4 = keyValuePair4.Value;
			if (value4 != null)
			{
				value4.Cleanup(true);
			}
		}
		cachedTextures.Clear();
		cachedMeshes.Clear();
		cachedAssetBundles.Clear();
		cachedAudioclips.Clear();
	}

	// Token: 0x06000BD7 RID: 3031 RVA: 0x00051494 File Offset: 0x0004F694
	public void Load(string DLCName)
	{
		DLCWebsiteInfo dlcwebsiteInfo = DLCManager.NameToDLCInfo(DLCName);
		if (!SteamManager.IsSubscribedApp(dlcwebsiteInfo.AppId))
		{
			Chat.Log("You do not own DLC " + dlcwebsiteInfo.Name + ". Cannot load.", Colour.Red, ChatMessageType.Game, false);
			TTSUtilities.OpenURL("http://store.steampowered.com/app/" + dlcwebsiteInfo.AppId.ToString());
			return;
		}
		base.StartCoroutine(this.LoadSaveFile(dlcwebsiteInfo));
	}

	// Token: 0x06000BD8 RID: 3032 RVA: 0x000514FF File Offset: 0x0004F6FF
	private IEnumerator LoadSaveFile(DLCWebsiteInfo dlcInfo)
	{
		string ConvertedURL = CustomCache.CheckDLCSaveExist(string.IsNullOrEmpty(dlcInfo.SaveURL) ? (dlcInfo.Name + ".json") : dlcInfo.SaveURL, true, true);
		Singleton<UILoading>.Instance.AddLoading();
		DataRequest request = DataRequest.Get(ConvertedURL, null);
		while (!request.isDone)
		{
			yield return null;
		}
		if (request.isError)
		{
			Chat.LogError("Error loading DLC save file " + dlcInfo.DisplayName + ": " + request.error, true);
		}
		else
		{
			string text = request.text;
			if (!DataRequest.IsLocalFile(ConvertedURL))
			{
				Task task = CustomCache.SaveDLCSaveAsync(text, dlcInfo.SaveURL);
				while (!task.IsCompleted)
				{
					yield return null;
				}
				task = null;
			}
			if (Network.isServer)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.LoadSaveState(Json.Load<SaveState>(text), false, true);
			}
			else if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.LoadPromotedSaveState(Json.Load<SaveState>(text));
			}
			int num;
			for (int i = 0; i < dlcInfo.Expansions.Count; i = num + 1)
			{
				DLCWebsiteBaseInfo expansion = dlcInfo.Expansions[i];
				if (!SteamManager.IsSubscribedApp(expansion.AppId))
				{
					Chat.Log("Expansion " + expansion.Name + " not owned.", ChatMessageType.Game);
				}
				else
				{
					Chat.Log("Expansion " + expansion.Name + " loading...", ChatMessageType.Game);
					string ConvertedURLExpansion = CustomCache.CheckDLCSaveExist(string.IsNullOrEmpty(expansion.SaveURL) ? (expansion.Name + ".json") : expansion.SaveURL, true, true);
					Singleton<UILoading>.Instance.AddLoading();
					DataRequest requestExpansion = DataRequest.Get(ConvertedURLExpansion, null);
					while (!requestExpansion.isDone)
					{
						yield return null;
					}
					if (requestExpansion.isError)
					{
						Chat.LogError("Error loading expansion save file " + expansion.Name + ": " + requestExpansion.error, true);
					}
					else
					{
						string textExpansion = requestExpansion.text;
						if (!DataRequest.IsLocalFile(ConvertedURLExpansion))
						{
							Task task = CustomCache.SaveDLCSaveAsync(textExpansion, expansion.SaveURL);
							while (!task.IsCompleted)
							{
								yield return null;
							}
							task = null;
						}
						if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
						{
							NetworkSingleton<ManagerPhysicsObject>.Instance.SpawnObjects(Json.Load<SaveState>(textExpansion).ObjectStates);
						}
						textExpansion = null;
					}
					Singleton<UILoading>.Instance.RemoveLoading();
					requestExpansion.Dispose();
					expansion = null;
					ConvertedURLExpansion = null;
					requestExpansion = null;
				}
				num = i;
			}
			text = null;
		}
		Singleton<UILoading>.Instance.RemoveLoading();
		request.Dispose();
		yield break;
	}

	// Token: 0x06000BD9 RID: 3033 RVA: 0x00051510 File Offset: 0x0004F710
	public void CheckSales()
	{
		foreach (DLCWebsiteInfo dlcInfo in DLCManager.DLCInfos)
		{
			base.StartCoroutine(this.CheckSale(dlcInfo));
		}
	}

	// Token: 0x06000BDA RID: 3034 RVA: 0x0005156C File Offset: 0x0004F76C
	private IEnumerator CheckSale(DLCWebsiteInfo dlcInfo)
	{
		DataRequest dataRequest = DataRequest.Get("http://store.steampowered.com/api/appdetails?appids=" + dlcInfo.AppId, null);
		while (!dataRequest.isDone)
		{
			yield return null;
		}
		if (!dataRequest.isError)
		{
			try
			{
				string @string = Encoding.UTF8.GetString(dataRequest.data);
				dlcInfo.DiscountPercent = Json.GetValueFromJson<int>(@string, "discount_percent", Json.SearchType.Exact);
			}
			catch (Exception)
			{
			}
		}
		dataRequest.Dispose();
		yield break;
	}

	// Token: 0x04000828 RID: 2088
	private static List<DLCWebsiteInfo> _DLCInfos = null;

	// Token: 0x04000829 RID: 2089
	public List<DLCWebsiteInfo> DLCWebsites = new List<DLCWebsiteInfo>();

	// Token: 0x0400082A RID: 2090
	private static List<string> HostDLCs = new List<string>();

	// Token: 0x0400082B RID: 2091
	public Dictionary<string, DLCManager.DLCBundleData> LoadedDLCBundles = new Dictionary<string, DLCManager.DLCBundleData>();

	// Token: 0x020005CD RID: 1485
	public class DLCBundleData
	{
		// Token: 0x040026C9 RID: 9929
		public string name;

		// Token: 0x040026CA RID: 9930
		public AssetBundle DLCBundle;

		// Token: 0x040026CB RID: 9931
		public Dictionary<string, CustomTextureContainer> CachedTextures = new Dictionary<string, CustomTextureContainer>();

		// Token: 0x040026CC RID: 9932
		public Dictionary<string, CustomMeshContainer> CachedMeshes = new Dictionary<string, CustomMeshContainer>();

		// Token: 0x040026CD RID: 9933
		public Dictionary<string, CustomAssetbundleContainer> CachedAssetBundles = new Dictionary<string, CustomAssetbundleContainer>();

		// Token: 0x040026CE RID: 9934
		public Dictionary<string, CustomAudioContainer> CachedAudioclips = new Dictionary<string, CustomAudioContainer>();
	}
}
