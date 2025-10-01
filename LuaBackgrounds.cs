using System;
using NewNet;
using UnityEngine;

// Token: 0x0200019C RID: 412
public class LuaBackgrounds
{
	// Token: 0x0600142E RID: 5166 RVA: 0x00084D93 File Offset: 0x00082F93
	public string GetBackground()
	{
		return NetworkSingleton<ManagerPhysicsObject>.Instance.Sky.InternalName;
	}

	// Token: 0x0600142F RID: 5167 RVA: 0x00084DA4 File Offset: 0x00082FA4
	public bool SetBackground(string name)
	{
		NetworkSingleton<NetworkUI>.Instance.GUIChangeBackground(name);
		return true;
	}

	// Token: 0x06001430 RID: 5168 RVA: 0x00084DB2 File Offset: 0x00082FB2
	public string GetCustomURL()
	{
		if (!CustomSky.ActiveCustomSky)
		{
			return null;
		}
		return CustomSky.ActiveCustomSky.CustomSkyURL;
	}

	// Token: 0x06001431 RID: 5169 RVA: 0x00084DCC File Offset: 0x00082FCC
	public bool SetCustomURL(string url)
	{
		NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyCustomSky();
		if (!string.IsNullOrEmpty(url))
		{
			Network.Instantiate<CustomSky>(NetworkSingleton<GameMode>.Instance.Sky_Custom, default(Vector3), default(Vector3), default(NetworkPlayer)).CustomSkyURL = url;
		}
		return true;
	}
}
