using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using NewNet;

// Token: 0x0200035E RID: 862
public class UserDefinedConsoleCommandManager : NetworkSingleton<UserDefinedConsoleCommandManager>
{
	// Token: 0x060028C5 RID: 10437 RVA: 0x0011FB90 File Offset: 0x0011DD90
	protected override void Awake()
	{
		base.Awake();
		NetworkEvents.OnPlayerConnected += this.OnPlayerConnect;
		NetworkEvents.OnDisconnectedFromServer += this.OnDisconnect;
		EventManager.OnResetTable += this.OnTableReset;
	}

	// Token: 0x060028C6 RID: 10438 RVA: 0x0011FBCB File Offset: 0x0011DDCB
	private void OnDestroy()
	{
		NetworkEvents.OnPlayerConnected -= this.OnPlayerConnect;
		NetworkEvents.OnDisconnectedFromServer -= this.OnDisconnect;
		EventManager.OnResetTable -= this.OnTableReset;
	}

	// Token: 0x060028C7 RID: 10439 RVA: 0x0011FC00 File Offset: 0x0011DE00
	private void OnPlayerConnect(NetworkPlayer networkPlayer)
	{
		if (!Network.isServer)
		{
			return;
		}
		base.networkView.RPC(networkPlayer, new Action(this.RPCClearLuaConsoleCommands));
		for (int i = 0; i < this.LuaConsoleCommands.Count; i++)
		{
			base.networkView.RPC<UserDefinedConsoleCommandManager.LuaConsoleCommandIdentifier>(networkPlayer, new Action<UserDefinedConsoleCommandManager.LuaConsoleCommandIdentifier>(this.RPCAddLuaConsoleCommand), this.LuaConsoleCommands[i]);
		}
	}

	// Token: 0x060028C8 RID: 10440 RVA: 0x0011FC67 File Offset: 0x0011DE67
	private void OnDisconnect(DisconnectInfo info)
	{
		this.RPCClearLuaConsoleCommands();
	}

	// Token: 0x060028C9 RID: 10441 RVA: 0x0011FC6F File Offset: 0x0011DE6F
	private void OnTableReset()
	{
		this.ClearLuaConsoleCommands();
	}

	// Token: 0x060028CA RID: 10442 RVA: 0x0011FC78 File Offset: 0x0011DE78
	public int ConsoleCommandIndex(string label)
	{
		for (int i = 0; i < this.LuaConsoleCommands.Count; i++)
		{
			if (this.LuaConsoleCommands[i].label == label)
			{
				return this.LuaConsoleCommands[i].index;
			}
		}
		return -1;
	}

	// Token: 0x060028CB RID: 10443 RVA: 0x0011FCC8 File Offset: 0x0011DEC8
	public int AddConsoleCommand(string label, Closure luaFunction, bool adminOnly)
	{
		if (!Network.isServer)
		{
			return -1;
		}
		int num = this.ConsoleCommandIndex(label);
		if (num == -1)
		{
			num = this.LuaConsoleCommands.Count;
			UserDefinedConsoleCommandManager.LuaConsoleCommandIdentifier luaConsoleCommandIdentifier = new UserDefinedConsoleCommandManager.LuaConsoleCommandIdentifier(num, label, adminOnly);
			if (Singleton<SystemConsole>.Instance.AddLuaConsoleCommand(luaConsoleCommandIdentifier))
			{
				this.LuaConsoleCommands.Add(luaConsoleCommandIdentifier);
				this.LuaConsoleCommandMethods[num] = luaFunction;
				base.networkView.RPC<UserDefinedConsoleCommandManager.LuaConsoleCommandIdentifier>(RPCTarget.Others, new Action<UserDefinedConsoleCommandManager.LuaConsoleCommandIdentifier>(this.RPCAddLuaConsoleCommand), this.LuaConsoleCommands[num]);
			}
		}
		return num;
	}

	// Token: 0x060028CC RID: 10444 RVA: 0x0011FD4C File Offset: 0x0011DF4C
	[Remote(Permission.Server)]
	public void RPCAddLuaConsoleCommand(UserDefinedConsoleCommandManager.LuaConsoleCommandIdentifier luaConsoleCommand)
	{
		if (Network.isServer)
		{
			return;
		}
		if (Singleton<SystemConsole>.Instance.AddLuaConsoleCommand(luaConsoleCommand))
		{
			while (this.LuaConsoleCommands.Count <= luaConsoleCommand.index)
			{
				this.LuaConsoleCommands.Add(new UserDefinedConsoleCommandManager.LuaConsoleCommandIdentifier(this.LuaConsoleCommands.Count, "", false));
			}
			this.LuaConsoleCommands[luaConsoleCommand.index] = luaConsoleCommand;
		}
	}

	// Token: 0x060028CD RID: 10445 RVA: 0x0011FDB8 File Offset: 0x0011DFB8
	public void ClearLuaConsoleCommands()
	{
		if (!Network.isServer)
		{
			return;
		}
		for (int i = 0; i < this.LuaConsoleCommands.Count; i++)
		{
			Singleton<SystemConsole>.Instance.RemoveLuaConsoleCommand(this.LuaConsoleCommands[i].label);
		}
		this.LuaConsoleCommands.Clear();
		this.LuaConsoleCommandMethods.Clear();
		base.networkView.RPC(RPCTarget.Others, new Action(this.RPCClearLuaConsoleCommands));
	}

	// Token: 0x060028CE RID: 10446 RVA: 0x0011FE2C File Offset: 0x0011E02C
	[Remote(Permission.Server)]
	public void RPCClearLuaConsoleCommands()
	{
		if (Network.isServer)
		{
			return;
		}
		for (int i = 0; i < this.LuaConsoleCommands.Count; i++)
		{
			Singleton<SystemConsole>.Instance.RemoveLuaConsoleCommand(this.LuaConsoleCommands[i].label);
		}
		this.LuaConsoleCommands.Clear();
	}

	// Token: 0x060028CF RID: 10447 RVA: 0x0011FE7D File Offset: 0x0011E07D
	public void DoConsoleCommand(int playerID, int consoleCommandID, List<string> parameters)
	{
		if (Network.isServer)
		{
			this.DoConsoleCommandRPC(playerID, consoleCommandID, parameters);
			return;
		}
		base.networkView.RPC<int, int, List<string>>(RPCTarget.Server, new Action<int, int, List<string>>(this.DoConsoleCommandRPC), playerID, consoleCommandID, parameters);
	}

	// Token: 0x060028D0 RID: 10448 RVA: 0x0011FEAC File Offset: 0x0011E0AC
	[Remote(SendType.ReliableNoDelay)]
	public void DoConsoleCommandRPC(int playerID, int commandIndex, List<string> parameters)
	{
		if (!Network.isServer)
		{
			return;
		}
		if (this.LuaConsoleCommands[commandIndex].adminOnly && Network.peerType != NetworkPeerMode.Disconnected && Network.peerType != NetworkPeerMode.Connecting && Network.maxConnections != 0 && !NetworkSingleton<PlayerManager>.Instance.IsPromoted(playerID))
		{
			return;
		}
		Closure closure;
		if (this.LuaConsoleCommandMethods.TryGetValue(commandIndex, out closure))
		{
			string item = Colour.LabelFromColour(NetworkSingleton<PlayerManager>.Instance.ColourFromID(playerID));
			List<object> list = new List<object>(parameters);
			list.Insert(0, item);
			if (closure != null && closure.OwnerScript != null)
			{
				closure.Call(new object[]
				{
					list
				});
			}
		}
	}

	// Token: 0x04001AD1 RID: 6865
	public List<UserDefinedConsoleCommandManager.LuaConsoleCommandIdentifier> LuaConsoleCommands = new List<UserDefinedConsoleCommandManager.LuaConsoleCommandIdentifier>();

	// Token: 0x04001AD2 RID: 6866
	public Dictionary<int, Closure> LuaConsoleCommandMethods = new Dictionary<int, Closure>();

	// Token: 0x0200079D RID: 1949
	public struct LuaConsoleCommandIdentifier
	{
		// Token: 0x06003F61 RID: 16225 RVA: 0x00181450 File Offset: 0x0017F650
		public LuaConsoleCommandIdentifier(int index, string label, bool adminOnly)
		{
			this.index = index;
			this.label = label;
			this.adminOnly = adminOnly;
		}

		// Token: 0x04002CC3 RID: 11459
		public int index;

		// Token: 0x04002CC4 RID: 11460
		public string label;

		// Token: 0x04002CC5 RID: 11461
		public bool adminOnly;
	}
}
