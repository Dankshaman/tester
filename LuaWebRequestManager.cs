using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x020001A6 RID: 422
[MoonSharpHideMember("Invoke")]
[MoonSharpHideMember("InvokeRepeating")]
[MoonSharpHideMember("BroadcastMessage")]
[MoonSharpHideMember("SendMessage")]
[MoonSharpHideMember("SendMessageUpwards")]
[MoonSharpHideMember("networkView")]
public class LuaWebRequestManager : MonoBehaviour
{
	// Token: 0x0600150E RID: 5390 RVA: 0x00089C91 File Offset: 0x00087E91
	public LuaWebRequest Get(string url, Closure callbackFunction)
	{
		return this.StartSend(UnityWebRequest.Get(url), callbackFunction);
	}

	// Token: 0x0600150F RID: 5391 RVA: 0x00089CA0 File Offset: 0x00087EA0
	public LuaWebRequest Post(string url, string data, Closure callbackFunction)
	{
		return this.StartSend(UnityWebRequest.Post(url, data), callbackFunction);
	}

	// Token: 0x06001510 RID: 5392 RVA: 0x00089CB0 File Offset: 0x00087EB0
	public LuaWebRequest Post(string url, Dictionary<string, string> form, Closure callbackFunction)
	{
		return this.StartSend(UnityWebRequest.Post(url, form), callbackFunction);
	}

	// Token: 0x06001511 RID: 5393 RVA: 0x00089CC0 File Offset: 0x00087EC0
	public LuaWebRequest Put(string url, string data, Closure callbackFunction)
	{
		return this.StartSend(UnityWebRequest.Put(url, data), callbackFunction);
	}

	// Token: 0x06001512 RID: 5394 RVA: 0x00089CD0 File Offset: 0x00087ED0
	public LuaWebRequest Head(string url, Closure callbackFunction)
	{
		return this.StartSend(UnityWebRequest.Head(url), callbackFunction);
	}

	// Token: 0x06001513 RID: 5395 RVA: 0x00089CDF File Offset: 0x00087EDF
	public LuaWebRequest Delete(string url, Closure callbackFunction)
	{
		return this.StartSend(UnityWebRequest.Delete(url), callbackFunction);
	}

	// Token: 0x06001514 RID: 5396 RVA: 0x00089CF0 File Offset: 0x00087EF0
	public LuaWebRequest Custom(string url, string method, bool download, string data, Dictionary<string, string> headers, Closure callbackFunction)
	{
		UnityWebRequest unityWebRequest = new UnityWebRequest(url, method, download ? new DownloadHandlerBuffer() : null, (data != null) ? new UploadHandlerRaw(Encoding.UTF8.GetBytes(data)) : null);
		if (headers != null)
		{
			foreach (KeyValuePair<string, string> keyValuePair in headers)
			{
				unityWebRequest.SetRequestHeader(keyValuePair.Key, keyValuePair.Value);
			}
		}
		return this.StartSend(unityWebRequest, callbackFunction);
	}

	// Token: 0x06001515 RID: 5397 RVA: 0x00089D84 File Offset: 0x00087F84
	private LuaWebRequest StartSend(UnityWebRequest webRequest, Closure callbackFunction)
	{
		if (webRequest.url.StartsWith("file:"))
		{
			webRequest.Dispose();
			return null;
		}
		LuaWebRequest luaWebRequest = new LuaWebRequest(webRequest, callbackFunction);
		base.StartCoroutine(this.Send(luaWebRequest));
		return luaWebRequest;
	}

	// Token: 0x06001516 RID: 5398 RVA: 0x00089DC2 File Offset: 0x00087FC2
	private IEnumerator Send(LuaWebRequest luaWebRequest)
	{
		yield return luaWebRequest.webRequest.SendWebRequest();
		if (luaWebRequest.callback_function != null)
		{
			LuaScript.TryCall(luaWebRequest.callback_function, new object[]
			{
				luaWebRequest
			});
		}
		yield return null;
		luaWebRequest.webRequest.Dispose();
		yield break;
	}

	// Token: 0x06001517 RID: 5399 RVA: 0x00089DD1 File Offset: 0x00087FD1
	[Obsolete]
	public LuaWebRequest Get(string url, LuaGameObjectScript FunctionOwner, string LuaFunction)
	{
		return this.StartSend(UnityWebRequest.Get(url), FunctionOwner, LuaFunction);
	}

	// Token: 0x06001518 RID: 5400 RVA: 0x00089DE1 File Offset: 0x00087FE1
	[Obsolete]
	public LuaWebRequest Post(string url, Dictionary<string, string> form, LuaGameObjectScript FunctionOwner, string LuaFunction)
	{
		return this.StartSend(UnityWebRequest.Post(url, form), FunctionOwner, LuaFunction);
	}

	// Token: 0x06001519 RID: 5401 RVA: 0x00089DF3 File Offset: 0x00087FF3
	[Obsolete]
	public LuaWebRequest Put(string url, string data, LuaGameObjectScript FunctionOwner, string LuaFunction)
	{
		return this.StartSend(UnityWebRequest.Put(url, data), FunctionOwner, LuaFunction);
	}

	// Token: 0x0600151A RID: 5402 RVA: 0x00089E08 File Offset: 0x00088008
	[Obsolete]
	private LuaWebRequest StartSend(UnityWebRequest webRequest, LuaGameObjectScript FunctionOwner, string LuaFunction)
	{
		if (webRequest.url.StartsWith("file:"))
		{
			webRequest.Dispose();
			return null;
		}
		LuaGlobalScriptManager.FunctionOwnerToScript(ref FunctionOwner);
		LuaWebRequest luaWebRequest = new LuaWebRequest(webRequest, FunctionOwner, LuaFunction);
		base.StartCoroutine(this.SendObsolete(luaWebRequest));
		return luaWebRequest;
	}

	// Token: 0x0600151B RID: 5403 RVA: 0x00089E4F File Offset: 0x0008804F
	[Obsolete]
	private IEnumerator SendObsolete(LuaWebRequest luaWebRequest)
	{
		yield return luaWebRequest.webRequest.Send();
		LuaGameObjectScript function_owner = luaWebRequest.function_owner;
		if (function_owner && !string.IsNullOrEmpty(luaWebRequest.lua_function))
		{
			Script lua = function_owner.lua;
			try
			{
				lua.Call(lua.Globals[luaWebRequest.lua_function], new object[]
				{
					luaWebRequest
				});
			}
			catch (Exception e)
			{
				function_owner.LogError(luaWebRequest.lua_function, e, null);
			}
		}
		yield return null;
		luaWebRequest.webRequest.Dispose();
		yield break;
	}
}
