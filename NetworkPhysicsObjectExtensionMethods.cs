using System;
using System.Collections.Generic;

// Token: 0x020001C0 RID: 448
public static class NetworkPhysicsObjectExtensionMethods
{
	// Token: 0x06001774 RID: 6004 RVA: 0x000A0658 File Offset: 0x0009E858
	public static List<LuaGameObjectScript> ToLGOS(this List<NetworkPhysicsObject> npos)
	{
		List<LuaGameObjectScript> list = new List<LuaGameObjectScript>(npos.Count);
		for (int i = 0; i < npos.Count; i++)
		{
			list.Add(npos[i].luaGameObjectScript);
		}
		return list;
	}

	// Token: 0x06001775 RID: 6005 RVA: 0x000A0698 File Offset: 0x0009E898
	public static List<LuaGameObjectScript> ToLGOS(this List<NetworkPhysicsObject> npos, Func<NetworkPhysicsObject, bool> condition)
	{
		List<LuaGameObjectScript> list = new List<LuaGameObjectScript>(npos.Count);
		for (int i = 0; i < npos.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = npos[i];
			if (condition(networkPhysicsObject))
			{
				list.Add(networkPhysicsObject.luaGameObjectScript);
			}
		}
		return list;
	}
}
