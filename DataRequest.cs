using System;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x020000EF RID: 239
public class DataRequest
{
	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x06000BDD RID: 3037 RVA: 0x000515AB File Offset: 0x0004F7AB
	// (set) Token: 0x06000BDE RID: 3038 RVA: 0x000515B3 File Offset: 0x0004F7B3
	public string url { get; private set; }

	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x06000BDF RID: 3039 RVA: 0x000515BC File Offset: 0x0004F7BC
	// (set) Token: 0x06000BE0 RID: 3040 RVA: 0x000515C4 File Offset: 0x0004F7C4
	public string urlConverted { get; private set; }

	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x06000BE1 RID: 3041 RVA: 0x000515CD File Offset: 0x0004F7CD
	public float progress
	{
		get
		{
			if (this.asyncOperation != null)
			{
				return this.asyncOperation.progress;
			}
			if (this.www != null)
			{
				return this.www.progress;
			}
			return 0f;
		}
	}

	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x000515FC File Offset: 0x0004F7FC
	public ulong progressBytes
	{
		get
		{
			if (this.unityWebRequest != null)
			{
				return this.unityWebRequest.downloadedBytes;
			}
			if (this.www != null)
			{
				return (ulong)((long)this.www.bytesDownloaded);
			}
			return 0UL;
		}
	}

	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x0005162C File Offset: 0x0004F82C
	public byte[] data
	{
		get
		{
			if (this._data == null)
			{
				if (this.unityWebRequest != null)
				{
					this._data = this.unityWebRequest.downloadHandler.data;
				}
				else if (this.www != null)
				{
					this._data = this.www.bytes;
				}
			}
			return this._data;
		}
	}

	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x00051680 File Offset: 0x0004F880
	public string text
	{
		get
		{
			if (this._text == null)
			{
				if (this.unityWebRequest != null)
				{
					this._text = this.unityWebRequest.downloadHandler.text;
				}
				else if (this.www != null)
				{
					this._text = this.www.text;
				}
			}
			return this._text;
		}
	}

	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x06000BE5 RID: 3045 RVA: 0x000516D4 File Offset: 0x0004F8D4
	public bool isError
	{
		get
		{
			if (this.unityWebRequest != null)
			{
				return this.unityWebRequest.isNetworkError;
			}
			return this.www != null && !string.IsNullOrEmpty(this.www.error);
		}
	}

	// Token: 0x170001FA RID: 506
	// (get) Token: 0x06000BE6 RID: 3046 RVA: 0x00051707 File Offset: 0x0004F907
	public string error
	{
		get
		{
			if (this.unityWebRequest != null)
			{
				return this.unityWebRequest.error;
			}
			if (this.www != null)
			{
				return this.www.error;
			}
			return null;
		}
	}

	// Token: 0x170001FB RID: 507
	// (get) Token: 0x06000BE7 RID: 3047 RVA: 0x00051732 File Offset: 0x0004F932
	public bool isDone
	{
		get
		{
			if (this.asyncOperation != null)
			{
				return this.asyncOperation.isDone;
			}
			return this.www == null || this.www.isDone;
		}
	}

	// Token: 0x170001FC RID: 508
	// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x00051760 File Offset: 0x0004F960
	public AssetBundle assetBundle
	{
		get
		{
			if (this.unityWebRequest != null)
			{
				return ((DownloadHandlerAssetBundle)this.unityWebRequest.downloadHandler).assetBundle;
			}
			if (this.www != null)
			{
				return this.www.assetBundle;
			}
			if (this.assetBundleRequest != null)
			{
				return this.assetBundleRequest.assetBundle;
			}
			return null;
		}
	}

	// Token: 0x170001FD RID: 509
	// (get) Token: 0x06000BE9 RID: 3049 RVA: 0x000517B4 File Offset: 0x0004F9B4
	public AudioClip AudioClip
	{
		get
		{
			if (this.unityWebRequest != null)
			{
				return DownloadHandlerAudioClip.GetContent(this.unityWebRequest);
			}
			WWW www = this.www;
			if (www == null)
			{
				return null;
			}
			return www.GetAudioClip();
		}
	}

	// Token: 0x06000BEB RID: 3051 RVA: 0x000517DC File Offset: 0x0004F9DC
	public static DataRequest Get(string url, ListBuffer<byte> downloadBuffer = null)
	{
		DataRequest dataRequest = new DataRequest
		{
			url = url
		};
		dataRequest.urlConverted = DataRequest.ConvertURLForUnityWebRequest(url);
		if (downloadBuffer != null)
		{
			dataRequest.unityWebRequest = new UnityWebRequest(dataRequest.urlConverted, "GET", DownloadHandlerPool.Create(downloadBuffer), null);
		}
		else
		{
			dataRequest.unityWebRequest = UnityWebRequest.Get(dataRequest.urlConverted);
		}
		dataRequest.asyncOperation = dataRequest.unityWebRequest.SendWebRequest();
		return dataRequest;
	}

	// Token: 0x06000BEC RID: 3052 RVA: 0x00051847 File Offset: 0x0004FA47
	public static DataRequest GetAudio(string url, AudioType audioType)
	{
		DataRequest dataRequest = new DataRequest();
		dataRequest.url = url;
		dataRequest.urlConverted = DataRequest.ConvertURLForUnityWebRequest(url);
		dataRequest.unityWebRequest = UnityWebRequestMultimedia.GetAudioClip(dataRequest.urlConverted, audioType);
		dataRequest.asyncOperation = dataRequest.unityWebRequest.SendWebRequest();
		return dataRequest;
	}

	// Token: 0x06000BED RID: 3053 RVA: 0x00051884 File Offset: 0x0004FA84
	public static DataRequest GetAssetBundle(string url, bool useWebRequest = false)
	{
		DataRequest dataRequest = new DataRequest
		{
			url = url
		};
		if (DataRequest.IsLocalFile(url))
		{
			dataRequest.assetBundleRequest = AssetBundle.LoadFromFileAsync(DataRequest.TryRemoveLocalFile(url));
			dataRequest.asyncOperation = dataRequest.assetBundleRequest;
		}
		else if (!useWebRequest)
		{
			dataRequest.www = new WWW(url);
		}
		else
		{
			dataRequest.urlConverted = DataRequest.ConvertURLForUnityWebRequest(url);
			dataRequest.unityWebRequest = UnityWebRequestAssetBundle.GetAssetBundle(dataRequest.urlConverted);
			dataRequest.asyncOperation = dataRequest.unityWebRequest.SendWebRequest();
		}
		return dataRequest;
	}

	// Token: 0x06000BEE RID: 3054 RVA: 0x00051908 File Offset: 0x0004FB08
	public void Dispose()
	{
		if (this.unityWebRequest != null)
		{
			DownloadHandlerPool downloadHandlerPool;
			if ((downloadHandlerPool = (this.unityWebRequest.downloadHandler as DownloadHandlerPool)) != null)
			{
				downloadHandlerPool.Cleanup();
			}
			this.unityWebRequest.Dispose();
		}
		else if (this.www != null)
		{
			this.www.Dispose();
		}
		this.www = null;
		this.unityWebRequest = null;
		this.asyncOperation = null;
		this.assetBundleRequest = null;
		this._data = null;
		this._text = null;
	}

	// Token: 0x06000BEF RID: 3055 RVA: 0x00051981 File Offset: 0x0004FB81
	public void Abort()
	{
		if (this.unityWebRequest != null)
		{
			this.unityWebRequest.Abort();
		}
	}

	// Token: 0x06000BF0 RID: 3056 RVA: 0x00051996 File Offset: 0x0004FB96
	private static string ConvertURLForUnityWebRequest(string url)
	{
		if (!DataRequest.IsLocalFile(url))
		{
			return url;
		}
		if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
		{
			return url.Replace("file:///", "");
		}
		return url.Replace("file:////", "file:///");
	}

	// Token: 0x06000BF1 RID: 3057 RVA: 0x000519D3 File Offset: 0x0004FBD3
	public static bool IsLocalFile(string URL)
	{
		return URL.StartsWith("file:", StringComparison.OrdinalIgnoreCase);
	}

	// Token: 0x06000BF2 RID: 3058 RVA: 0x000519E1 File Offset: 0x0004FBE1
	public static string TryRemoveLocalFile(string URL)
	{
		if (DataRequest.IsLocalFile(URL))
		{
			return URL.Substring(8, URL.Length - 8);
		}
		return URL;
	}

	// Token: 0x0400082C RID: 2092
	private WWW www;

	// Token: 0x0400082D RID: 2093
	private UnityWebRequest unityWebRequest;

	// Token: 0x0400082E RID: 2094
	private AsyncOperation asyncOperation;

	// Token: 0x0400082F RID: 2095
	private AssetBundleCreateRequest assetBundleRequest;

	// Token: 0x04000832 RID: 2098
	private byte[] _data;

	// Token: 0x04000833 RID: 2099
	private string _text;
}
