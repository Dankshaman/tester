using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using NewNet;
using UnityEngine;

// Token: 0x02000361 RID: 865
public class UserDefinedHotkeyManager : NetworkSingleton<UserDefinedHotkeyManager>
{
	// Token: 0x060028EA RID: 10474 RVA: 0x001205AE File Offset: 0x0011E7AE
	protected override void Awake()
	{
		base.Awake();
		NetworkEvents.OnPlayerConnected += this.OnPlayerConnect;
		NetworkEvents.OnDisconnectedFromServer += this.OnDisconnect;
		EventManager.OnResetTable += this.OnTableReset;
	}

	// Token: 0x060028EB RID: 10475 RVA: 0x001205E9 File Offset: 0x0011E7E9
	private void OnDestroy()
	{
		NetworkEvents.OnPlayerConnected -= this.OnPlayerConnect;
		NetworkEvents.OnDisconnectedFromServer -= this.OnDisconnect;
		EventManager.OnResetTable -= this.OnTableReset;
	}

	// Token: 0x060028EC RID: 10476 RVA: 0x00120620 File Offset: 0x0011E820
	private void OnPlayerConnect(NetworkPlayer networkPlayer)
	{
		if (!Network.isServer)
		{
			return;
		}
		base.networkView.RPC(networkPlayer, new Action(this.RPCClearHotkeys));
		for (int i = 0; i < this.Hotkeys.Count; i++)
		{
			base.networkView.RPC<UserDefinedHotkeyManager.HotkeyIdentifier>(networkPlayer, new Action<UserDefinedHotkeyManager.HotkeyIdentifier>(this.RPCAddHotkey), this.Hotkeys[i]);
		}
	}

	// Token: 0x060028ED RID: 10477 RVA: 0x00120687 File Offset: 0x0011E887
	private void OnDisconnect(DisconnectInfo info)
	{
		this.RPCClearHotkeys();
	}

	// Token: 0x060028EE RID: 10478 RVA: 0x0012068F File Offset: 0x0011E88F
	private void OnTableReset()
	{
		this.ClearHotkeys();
	}

	// Token: 0x060028EF RID: 10479 RVA: 0x00120698 File Offset: 0x0011E898
	public bool cInputIDUsed(string cInputID)
	{
		try
		{
			cInput.GetText(cInputID);
		}
		catch
		{
			return false;
		}
		return true;
	}

	// Token: 0x060028F0 RID: 10480 RVA: 0x001206C8 File Offset: 0x0011E8C8
	public void cInputSet(string cInputID, string primary, string secondary)
	{
		if (this.cInputIDUsed(cInputID))
		{
			cInput.ChangeKey(cInputID, primary, secondary);
		}
		else
		{
			cInput.SetKey(cInputID, primary, secondary);
		}
		if (primary == "")
		{
			cInput.TTSClearPrimary(cInputID);
		}
		if (secondary == "")
		{
			cInput.TTSClearSecondary(cInputID);
		}
	}

	// Token: 0x060028F1 RID: 10481 RVA: 0x00120717 File Offset: 0x0011E917
	public void cInputSetPrimary(string cInputID, string key)
	{
		if (this.cInputIDUsed(cInputID))
		{
			cInput.ChangeKey(cInputID, key);
		}
		else
		{
			cInput.SetKey(cInputID, key);
		}
		if (key == "")
		{
			cInput.TTSClearPrimary(cInputID);
		}
	}

	// Token: 0x060028F2 RID: 10482 RVA: 0x00120748 File Offset: 0x0011E948
	public void cInputSetSecondary(string cInputID, string key)
	{
		if (this.cInputIDUsed(cInputID))
		{
			string text = cInput.GetText(cInputID, 1);
			cInput.ChangeKey(cInputID, text, key);
		}
		else
		{
			cInput.SetKey(cInputID, "", key);
		}
		if (key == "")
		{
			cInput.TTSClearPrimary(cInputID);
		}
	}

	// Token: 0x060028F3 RID: 10483 RVA: 0x00120790 File Offset: 0x0011E990
	public int HotkeyIndex(string label)
	{
		for (int i = 0; i < this.Hotkeys.Count; i++)
		{
			if (this.Hotkeys[i].label == label)
			{
				return this.Hotkeys[i].index;
			}
		}
		return -1;
	}

	// Token: 0x060028F4 RID: 10484 RVA: 0x001207E0 File Offset: 0x0011E9E0
	public int AddHotkey(string label, Closure luaFunction, bool triggerOnKeyUp)
	{
		if (!Network.isServer)
		{
			return -1;
		}
		int num = this.HotkeyIndex(label);
		if (num == -1)
		{
			num = this.Hotkeys.Count;
			this.Hotkeys.Add(new UserDefinedHotkeyManager.HotkeyIdentifier(num, label, triggerOnKeyUp));
			this.HotkeyMethods[num] = luaFunction;
			base.networkView.RPC<UserDefinedHotkeyManager.HotkeyIdentifier>(RPCTarget.Others, new Action<UserDefinedHotkeyManager.HotkeyIdentifier>(this.RPCAddHotkey), this.Hotkeys[num]);
		}
		this.cInputSet(this.Hotkeys[num].cInputID, this.ReadPref(this.Hotkeys[num].label, true), this.ReadPref(this.Hotkeys[num].label, false));
		return num;
	}

	// Token: 0x060028F5 RID: 10485 RVA: 0x0012089C File Offset: 0x0011EA9C
	[Remote(Permission.Server)]
	public void RPCAddHotkey(UserDefinedHotkeyManager.HotkeyIdentifier hotkey)
	{
		if (Network.isServer)
		{
			return;
		}
		while (this.Hotkeys.Count <= hotkey.index)
		{
			this.Hotkeys.Add(new UserDefinedHotkeyManager.HotkeyIdentifier(this.Hotkeys.Count, "", false));
		}
		this.Hotkeys[hotkey.index] = hotkey;
		this.cInputSet(hotkey.cInputID, this.ReadPref(hotkey.label, true), this.ReadPref(hotkey.label, false));
	}

	// Token: 0x060028F6 RID: 10486 RVA: 0x00120920 File Offset: 0x0011EB20
	public void ClearHotkeys()
	{
		if (!Network.isServer)
		{
			return;
		}
		for (int i = 0; i < this.Hotkeys.Count; i++)
		{
			this.cInputSet(this.Hotkeys[i].cInputID, "", "");
		}
		this.Hotkeys.Clear();
		this.HotkeyMethods.Clear();
		base.networkView.RPC(RPCTarget.Others, new Action(this.RPCClearHotkeys));
	}

	// Token: 0x060028F7 RID: 10487 RVA: 0x0012099C File Offset: 0x0011EB9C
	[Remote(Permission.Server)]
	public void RPCClearHotkeys()
	{
		if (Network.isServer)
		{
			return;
		}
		for (int i = 0; i < this.Hotkeys.Count; i++)
		{
			this.cInputSet(this.Hotkeys[i].cInputID, "", "");
		}
		this.Hotkeys.Clear();
	}

	// Token: 0x060028F8 RID: 10488 RVA: 0x001209F3 File Offset: 0x0011EBF3
	public void DoHotkey(int playerID, int hotkeyID, NetworkPhysicsObject npo, Vector3? position, bool isKeyUp)
	{
		if (Network.isServer)
		{
			this.DoHotkeyRPC(playerID, hotkeyID, npo, position, isKeyUp);
			return;
		}
		base.networkView.RPC<int, int, NetworkPhysicsObject, Vector3?, bool>(RPCTarget.Server, new NetworkView.Action<int, int, NetworkPhysicsObject, Vector3?, bool>(this.DoHotkeyRPC), playerID, hotkeyID, npo, position, isKeyUp);
	}

	// Token: 0x060028F9 RID: 10489 RVA: 0x00120A2C File Offset: 0x0011EC2C
	[Remote(SendType.ReliableNoDelay)]
	public void DoHotkeyRPC(int playerID, int hotkeyID, NetworkPhysicsObject npo, Vector3? position, bool isKeyUp)
	{
		if (!Network.isServer)
		{
			return;
		}
		Closure function;
		if (this.HotkeyMethods.TryGetValue(hotkeyID, out function))
		{
			string text = Colour.LabelFromColour(NetworkSingleton<PlayerManager>.Instance.ColourFromID(playerID));
			LuaGameObjectScript luaGameObjectScript = null;
			if (npo)
			{
				luaGameObjectScript = npo.luaGameObjectScript;
			}
			if (position == null)
			{
				LuaScript.TryCall(function, new object[]
				{
					text,
					luaGameObjectScript,
					null,
					isKeyUp
				});
				return;
			}
			LuaScript.TryCall(function, new object[]
			{
				text,
				luaGameObjectScript,
				position.Value,
				isKeyUp
			});
		}
	}

	// Token: 0x060028FA RID: 10490 RVA: 0x00120AD0 File Offset: 0x0011ECD0
	public void ClearBindings()
	{
		for (int i = 0; i < this.Hotkeys.Count; i++)
		{
			this.cInputSet(this.Hotkeys[i].cInputID, "", "");
		}
		this.WritePrefs();
	}

	// Token: 0x060028FB RID: 10491 RVA: 0x00120B1A File Offset: 0x0011ED1A
	public void ShowSettings(bool fromLua = false)
	{
		if (fromLua && UserDefinedHotkeyManager.PROHIBIT_USER_HOTKEY_POPUP)
		{
			return;
		}
		this.HotkeySettingsUI.SetActive(true);
	}

	// Token: 0x060028FC RID: 10492 RVA: 0x00120B34 File Offset: 0x0011ED34
	public void WritePrefs()
	{
		if (Network.gameName == "" || Network.gameName == "None")
		{
			return;
		}
		for (int i = 0; i < this.Hotkeys.Count; i++)
		{
			UserDefinedHotkeyManager.HotkeyIdentifier hotkeyIdentifier = this.Hotkeys[i];
			PlayerPrefs.SetString(hotkeyIdentifier.label + ":1:" + Network.gameName, cInput.GetText(hotkeyIdentifier.cInputID, 1));
			PlayerPrefs.SetString(hotkeyIdentifier.label + ":2:" + Network.gameName, cInput.GetText(hotkeyIdentifier.cInputID, 2));
		}
	}

	// Token: 0x060028FD RID: 10493 RVA: 0x00120BD4 File Offset: 0x0011EDD4
	public string ReadPref(string label, bool primary = true)
	{
		if (Network.gameName == "" || Network.gameName == "None")
		{
			return "";
		}
		label = label.Replace(":", "");
		string str = primary ? ":1:" : ":2:";
		return PlayerPrefs.GetString(label + str + Network.gameName, "");
	}

	// Token: 0x04001AE3 RID: 6883
	public static bool PROHIBIT_USER_HOTKEY_POPUP;

	// Token: 0x04001AE4 RID: 6884
	public GameObject HotkeySettingsUI;

	// Token: 0x04001AE5 RID: 6885
	public List<UserDefinedHotkeyManager.HotkeyIdentifier> Hotkeys = new List<UserDefinedHotkeyManager.HotkeyIdentifier>();

	// Token: 0x04001AE6 RID: 6886
	public Dictionary<int, Closure> HotkeyMethods = new Dictionary<int, Closure>();

	// Token: 0x020007A0 RID: 1952
	public struct HotkeyIdentifier
	{
		// Token: 0x06003F64 RID: 16228 RVA: 0x0018148E File Offset: 0x0017F68E
		public HotkeyIdentifier(int index, string label, bool triggerOnKeyUp)
		{
			this.index = index;
			this.label = label.Replace(":", "");
			this.cInputID = "user:" + index;
			this.triggerOnKeyUp = triggerOnKeyUp;
		}

		// Token: 0x04002CCB RID: 11467
		public int index;

		// Token: 0x04002CCC RID: 11468
		public string label;

		// Token: 0x04002CCD RID: 11469
		public string cInputID;

		// Token: 0x04002CCE RID: 11470
		public bool triggerOnKeyUp;
	}
}
