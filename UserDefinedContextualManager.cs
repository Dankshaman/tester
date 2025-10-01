using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using NewNet;
using UnityEngine;

// Token: 0x02000360 RID: 864
public class UserDefinedContextualManager : NetworkSingleton<UserDefinedContextualManager>
{
	// Token: 0x060028D5 RID: 10453 RVA: 0x00120008 File Offset: 0x0011E208
	protected override void Awake()
	{
		base.Awake();
		NetworkEvents.OnPlayerConnected += this.OnPlayerConnect;
		NetworkEvents.OnDisconnectedFromServer += this.OnDisconnect;
		EventManager.OnResetTable += this.OnTableReset;
	}

	// Token: 0x060028D6 RID: 10454 RVA: 0x00120043 File Offset: 0x0011E243
	private void OnDestroy()
	{
		NetworkEvents.OnPlayerConnected -= this.OnPlayerConnect;
		NetworkEvents.OnDisconnectedFromServer -= this.OnDisconnect;
		EventManager.OnResetTable -= this.OnTableReset;
	}

	// Token: 0x060028D7 RID: 10455 RVA: 0x00120078 File Offset: 0x0011E278
	private void OnPlayerConnect(NetworkPlayer networkPlayer)
	{
		if (!Network.isServer)
		{
			return;
		}
		base.networkView.RPC(networkPlayer, new Action(this.RPCClearObjectItems));
		foreach (KeyValuePair<int, UserDefinedContextualManager.UDCIdentifier> keyValuePair in this.ObjectEntries)
		{
			base.networkView.RPC<int, UserDefinedContextualManager.UDCIdentifier>(networkPlayer, new Action<int, UserDefinedContextualManager.UDCIdentifier>(this.RPCAddObjectItem), keyValuePair.Key, keyValuePair.Value);
		}
		base.networkView.RPC(networkPlayer, new Action(this.RPCClearGlobalItems));
		foreach (KeyValuePair<int, UserDefinedContextualManager.UDCGlobalIdentifier> keyValuePair2 in this.GlobalEntries)
		{
			base.networkView.RPC<int, UserDefinedContextualManager.UDCGlobalIdentifier>(networkPlayer, new Action<int, UserDefinedContextualManager.UDCGlobalIdentifier>(this.RPCAddGlobalItem), keyValuePair2.Key, keyValuePair2.Value);
		}
	}

	// Token: 0x060028D8 RID: 10456 RVA: 0x00120184 File Offset: 0x0011E384
	private void OnDisconnect(DisconnectInfo info)
	{
		this.RPCClearObjectItems();
		this.RPCClearGlobalItems();
	}

	// Token: 0x060028D9 RID: 10457 RVA: 0x00120192 File Offset: 0x0011E392
	private void OnTableReset()
	{
		this.ClearObjectItems();
		this.ClearGlobalItems();
	}

	// Token: 0x060028DA RID: 10458 RVA: 0x001201A0 File Offset: 0x0011E3A0
	public int AddObjectItem(string label, Closure luaFunction, bool keepOpen)
	{
		if (!Network.isServer || luaFunction == null)
		{
			return -1;
		}
		string key = label + "=" + luaFunction.GetHashCode();
		int count;
		if (!this.ObjectHashes.TryGetValue(key, out count))
		{
			count = this.ObjectEntries.Count;
			this.ObjectHashes[key] = count;
			UserDefinedContextualManager.UDCIdentifier udcidentifier = new UserDefinedContextualManager.UDCIdentifier(label, keepOpen);
			this.ObjectEntries[count] = udcidentifier;
			this.ObjectMethods[count] = luaFunction;
			base.networkView.RPC<int, UserDefinedContextualManager.UDCIdentifier>(RPCTarget.Others, new Action<int, UserDefinedContextualManager.UDCIdentifier>(this.RPCAddObjectItem), count, udcidentifier);
		}
		return count;
	}

	// Token: 0x060028DB RID: 10459 RVA: 0x00120237 File Offset: 0x0011E437
	[Remote(Permission.Server)]
	public void RPCAddObjectItem(int index, UserDefinedContextualManager.UDCIdentifier id)
	{
		if (Network.isServer)
		{
			return;
		}
		this.ObjectEntries[index] = id;
	}

	// Token: 0x060028DC RID: 10460 RVA: 0x00120250 File Offset: 0x0011E450
	public void ClearObjectItems()
	{
		if (!Network.isServer)
		{
			return;
		}
		this.ObjectEntries.Clear();
		this.ObjectMethods.Clear();
		this.ObjectHashes.Clear();
		base.networkView.RPC(RPCTarget.Others, new Action(this.RPCClearObjectItems));
	}

	// Token: 0x060028DD RID: 10461 RVA: 0x0012029E File Offset: 0x0011E49E
	[Remote(Permission.Server)]
	public void RPCClearObjectItems()
	{
		if (Network.isServer)
		{
			return;
		}
		this.ObjectEntries.Clear();
	}

	// Token: 0x060028DE RID: 10462 RVA: 0x001202B4 File Offset: 0x0011E4B4
	public int AddGlobalItem(string label, Closure luaFunction, bool keepOpen, bool requireTable)
	{
		if (!Network.isServer || luaFunction == null)
		{
			return -1;
		}
		string key = label + "=" + luaFunction.GetHashCode();
		int count;
		if (!this.GlobalHashes.TryGetValue(key, out count))
		{
			count = this.GlobalEntries.Count;
			this.GlobalHashes[key] = count;
			UserDefinedContextualManager.UDCGlobalIdentifier udcglobalIdentifier = new UserDefinedContextualManager.UDCGlobalIdentifier(label, keepOpen, requireTable);
			this.GlobalEntries[count] = udcglobalIdentifier;
			this.GlobalMethods[count] = luaFunction;
			this.GlobalItems.Add(count);
			base.networkView.RPC<int, UserDefinedContextualManager.UDCGlobalIdentifier>(RPCTarget.Others, new Action<int, UserDefinedContextualManager.UDCGlobalIdentifier>(this.RPCAddGlobalItem), count, udcglobalIdentifier);
		}
		return count;
	}

	// Token: 0x060028DF RID: 10463 RVA: 0x00120359 File Offset: 0x0011E559
	[Remote(Permission.Server)]
	public void RPCAddGlobalItem(int index, UserDefinedContextualManager.UDCGlobalIdentifier id)
	{
		if (Network.isServer)
		{
			return;
		}
		this.GlobalEntries[index] = id;
		this.GlobalItems.Add(index);
	}

	// Token: 0x060028E0 RID: 10464 RVA: 0x0012037C File Offset: 0x0011E57C
	public void ClearGlobalItems()
	{
		if (!Network.isServer)
		{
			return;
		}
		this.GlobalEntries.Clear();
		this.GlobalMethods.Clear();
		this.GlobalHashes.Clear();
		this.GlobalItems.Clear();
		base.networkView.RPC(RPCTarget.Others, new Action(this.RPCClearGlobalItems));
	}

	// Token: 0x060028E1 RID: 10465 RVA: 0x001203D5 File Offset: 0x0011E5D5
	[Remote(Permission.Server)]
	public void RPCClearGlobalItems()
	{
		if (Network.isServer)
		{
			return;
		}
		this.GlobalEntries.Clear();
		this.GlobalItems.Clear();
	}

	// Token: 0x060028E2 RID: 10466 RVA: 0x001203F5 File Offset: 0x0011E5F5
	public void ContextualMenu(int playerID, int menuID, Vector3? position)
	{
		if (Network.isServer)
		{
			this.ContextualMenuRPC(playerID, menuID, position);
			return;
		}
		base.networkView.RPC<int, int, Vector3?>(RPCTarget.Server, new Action<int, int, Vector3?>(this.ContextualMenuRPC), playerID, menuID, position);
	}

	// Token: 0x060028E3 RID: 10467 RVA: 0x00120424 File Offset: 0x0011E624
	[Remote(SendType.ReliableNoDelay)]
	public void ContextualMenuRPC(int playerID, int menuID, Vector3? position)
	{
		if (!Network.isServer)
		{
			return;
		}
		Closure function;
		if (this.GlobalMethods.TryGetValue(menuID, out function))
		{
			string text = Colour.LabelFromColour(NetworkSingleton<PlayerManager>.Instance.ColourFromID(playerID));
			if (position == null)
			{
				LuaScript.TryCall(function, new object[]
				{
					text
				});
				return;
			}
			LuaScript.TryCall(function, new object[]
			{
				text,
				position.Value
			});
		}
	}

	// Token: 0x060028E4 RID: 10468 RVA: 0x0012049A File Offset: 0x0011E69A
	public void AddObjectMenuGO(GameObject go)
	{
		this.objectGameObjects.Add(go);
	}

	// Token: 0x060028E5 RID: 10469 RVA: 0x001204A8 File Offset: 0x0011E6A8
	public void CleanUpObjectMenuGOs()
	{
		for (int i = 0; i < this.objectGameObjects.Count; i++)
		{
			UnityEngine.Object.Destroy(this.objectGameObjects[i]);
		}
		this.objectGameObjects.Clear();
	}

	// Token: 0x060028E6 RID: 10470 RVA: 0x001204E7 File Offset: 0x0011E6E7
	public void AddGlobalMenuGO(GameObject go)
	{
		this.globalGameObjects.Add(go);
	}

	// Token: 0x060028E7 RID: 10471 RVA: 0x001204F8 File Offset: 0x0011E6F8
	public void CleanUpGlobalMenuGOs()
	{
		for (int i = 0; i < this.globalGameObjects.Count; i++)
		{
			UnityEngine.Object.Destroy(this.globalGameObjects[i]);
		}
		this.globalGameObjects.Clear();
	}

	// Token: 0x04001AD7 RID: 6871
	public static bool PROHIBIT_USER_CONTEXT_MENUS;

	// Token: 0x04001AD8 RID: 6872
	public Transform ObjectContextualMenu;

	// Token: 0x04001AD9 RID: 6873
	public Transform GlobalContextualMenu;

	// Token: 0x04001ADA RID: 6874
	public Dictionary<int, UserDefinedContextualManager.UDCIdentifier> ObjectEntries = new Dictionary<int, UserDefinedContextualManager.UDCIdentifier>();

	// Token: 0x04001ADB RID: 6875
	public Dictionary<string, int> ObjectHashes = new Dictionary<string, int>();

	// Token: 0x04001ADC RID: 6876
	public Dictionary<int, Closure> ObjectMethods = new Dictionary<int, Closure>();

	// Token: 0x04001ADD RID: 6877
	public Dictionary<int, UserDefinedContextualManager.UDCGlobalIdentifier> GlobalEntries = new Dictionary<int, UserDefinedContextualManager.UDCGlobalIdentifier>();

	// Token: 0x04001ADE RID: 6878
	public List<int> GlobalItems = new List<int>();

	// Token: 0x04001ADF RID: 6879
	public Dictionary<string, int> GlobalHashes = new Dictionary<string, int>();

	// Token: 0x04001AE0 RID: 6880
	public Dictionary<int, Closure> GlobalMethods = new Dictionary<int, Closure>();

	// Token: 0x04001AE1 RID: 6881
	private List<GameObject> objectGameObjects = new List<GameObject>();

	// Token: 0x04001AE2 RID: 6882
	private List<GameObject> globalGameObjects = new List<GameObject>();

	// Token: 0x0200079E RID: 1950
	public struct UDCIdentifier
	{
		// Token: 0x06003F62 RID: 16226 RVA: 0x00181467 File Offset: 0x0017F667
		public UDCIdentifier(string label, bool keepOpen)
		{
			this.label = label;
			this.keepOpen = keepOpen;
		}

		// Token: 0x04002CC6 RID: 11462
		public string label;

		// Token: 0x04002CC7 RID: 11463
		public bool keepOpen;
	}

	// Token: 0x0200079F RID: 1951
	public struct UDCGlobalIdentifier
	{
		// Token: 0x06003F63 RID: 16227 RVA: 0x00181477 File Offset: 0x0017F677
		public UDCGlobalIdentifier(string label, bool keepOpen, bool requireTable)
		{
			this.label = label;
			this.keepOpen = keepOpen;
			this.requireTable = requireTable;
		}

		// Token: 0x04002CC8 RID: 11464
		public string label;

		// Token: 0x04002CC9 RID: 11465
		public bool keepOpen;

		// Token: 0x04002CCA RID: 11466
		public bool requireTable;
	}
}
