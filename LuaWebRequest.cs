using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine.Networking;

// Token: 0x020001A7 RID: 423
public class LuaWebRequest
{
	// Token: 0x1700037E RID: 894
	// (get) Token: 0x0600151D RID: 5405 RVA: 0x00089E5E File Offset: 0x0008805E
	// (set) Token: 0x0600151E RID: 5406 RVA: 0x00089E66 File Offset: 0x00088066
	[MoonSharpHidden]
	public UnityWebRequest webRequest { get; private set; }

	// Token: 0x1700037F RID: 895
	// (get) Token: 0x0600151F RID: 5407 RVA: 0x00089E6F File Offset: 0x0008806F
	// (set) Token: 0x06001520 RID: 5408 RVA: 0x00089E77 File Offset: 0x00088077
	public Closure callback_function { get; private set; }

	// Token: 0x17000380 RID: 896
	// (get) Token: 0x06001521 RID: 5409 RVA: 0x00089E80 File Offset: 0x00088080
	// (set) Token: 0x06001522 RID: 5410 RVA: 0x00089E88 File Offset: 0x00088088
	[Obsolete]
	public LuaGameObjectScript function_owner { get; private set; }

	// Token: 0x17000381 RID: 897
	// (get) Token: 0x06001523 RID: 5411 RVA: 0x00089E91 File Offset: 0x00088091
	// (set) Token: 0x06001524 RID: 5412 RVA: 0x00089E99 File Offset: 0x00088099
	[Obsolete]
	public string lua_function { get; private set; }

	// Token: 0x17000382 RID: 898
	// (get) Token: 0x06001525 RID: 5413 RVA: 0x00089EA2 File Offset: 0x000880A2
	public string url
	{
		get
		{
			return this.webRequest.url;
		}
	}

	// Token: 0x17000383 RID: 899
	// (get) Token: 0x06001526 RID: 5414 RVA: 0x00089EAF File Offset: 0x000880AF
	public float download_progress
	{
		get
		{
			return this.webRequest.downloadProgress;
		}
	}

	// Token: 0x17000384 RID: 900
	// (get) Token: 0x06001527 RID: 5415 RVA: 0x00089EBC File Offset: 0x000880BC
	public float upload_progress
	{
		get
		{
			return this.webRequest.uploadProgress;
		}
	}

	// Token: 0x17000385 RID: 901
	// (get) Token: 0x06001528 RID: 5416 RVA: 0x00089EC9 File Offset: 0x000880C9
	public bool is_done
	{
		get
		{
			return this.webRequest.isDone;
		}
	}

	// Token: 0x17000386 RID: 902
	// (get) Token: 0x06001529 RID: 5417 RVA: 0x00089ED6 File Offset: 0x000880D6
	public bool is_error
	{
		get
		{
			return this.webRequest.isNetworkError;
		}
	}

	// Token: 0x17000387 RID: 903
	// (get) Token: 0x0600152A RID: 5418 RVA: 0x00089EE3 File Offset: 0x000880E3
	public string error
	{
		get
		{
			return this.webRequest.error;
		}
	}

	// Token: 0x17000388 RID: 904
	// (get) Token: 0x0600152B RID: 5419 RVA: 0x00089EF0 File Offset: 0x000880F0
	public long response_code
	{
		get
		{
			return this.webRequest.responseCode;
		}
	}

	// Token: 0x17000389 RID: 905
	// (get) Token: 0x0600152C RID: 5420 RVA: 0x00089EFD File Offset: 0x000880FD
	public string text
	{
		get
		{
			DownloadHandler downloadHandler = this.webRequest.downloadHandler;
			if (downloadHandler == null)
			{
				return null;
			}
			return downloadHandler.text;
		}
	}

	// Token: 0x0600152D RID: 5421 RVA: 0x00089F15 File Offset: 0x00088115
	public string GetResponseHeader(string name)
	{
		return this.webRequest.GetResponseHeader(name);
	}

	// Token: 0x0600152E RID: 5422 RVA: 0x00089F23 File Offset: 0x00088123
	public Dictionary<string, string> GetResponseHeaders()
	{
		return this.webRequest.GetResponseHeaders();
	}

	// Token: 0x0600152F RID: 5423 RVA: 0x00089F30 File Offset: 0x00088130
	public LuaWebRequest(UnityWebRequest webRequest, Closure callbackFunction)
	{
		this.webRequest = webRequest;
		this.callback_function = callbackFunction;
	}

	// Token: 0x06001530 RID: 5424 RVA: 0x00089F46 File Offset: 0x00088146
	[Obsolete]
	public LuaWebRequest(UnityWebRequest webRequest, LuaGameObjectScript functionOwner, string luaFunction)
	{
		this.webRequest = webRequest;
		this.function_owner = functionOwner;
		this.lua_function = luaFunction;
	}

	// Token: 0x06001531 RID: 5425 RVA: 0x00089F63 File Offset: 0x00088163
	public void Dispose()
	{
		this.webRequest.Dispose();
	}
}
