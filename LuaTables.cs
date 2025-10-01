using System;

// Token: 0x0200019B RID: 411
public class LuaTables
{
	// Token: 0x06001428 RID: 5160 RVA: 0x00084CE0 File Offset: 0x00082EE0
	public LuaGameObjectScript GetTableObject()
	{
		return NetworkSingleton<ManagerPhysicsObject>.Instance.TableNPO.luaGameObjectScript;
	}

	// Token: 0x06001429 RID: 5161 RVA: 0x00084CF1 File Offset: 0x00082EF1
	public string GetTable()
	{
		return NetworkSingleton<ManagerPhysicsObject>.Instance.TableNPO.InternalName;
	}

	// Token: 0x0600142A RID: 5162 RVA: 0x00084D02 File Offset: 0x00082F02
	public bool SetTable(string name)
	{
		NetworkSingleton<NetworkUI>.Instance.GUIChangeTable(name);
		return true;
	}

	// Token: 0x0600142B RID: 5163 RVA: 0x00084D10 File Offset: 0x00082F10
	public string GetCustomURL()
	{
		if (!NetworkSingleton<ManagerPhysicsObject>.Instance.TableNPO.customImage)
		{
			return null;
		}
		return NetworkSingleton<ManagerPhysicsObject>.Instance.TableNPO.customImage.CustomImageURL;
	}

	// Token: 0x0600142C RID: 5164 RVA: 0x00084D40 File Offset: 0x00082F40
	public bool SetCustomURL(string url)
	{
		if (!NetworkSingleton<ManagerPhysicsObject>.Instance.TableNPO.customImage)
		{
			return false;
		}
		NetworkSingleton<NetworkUI>.Instance.GUIChangeTable(this.GetTable());
		if (!string.IsNullOrEmpty(url))
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.TableNPO.customImage.CustomImageURL = url;
		}
		return true;
	}
}
