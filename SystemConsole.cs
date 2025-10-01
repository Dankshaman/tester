using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Discord;
using Dissonance;
using HighlightingSystem;
using I2.Loc;
using MoonSharp.Interpreter;
using NewNet;
using RTEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.XR;
using Valve.VR;

// Token: 0x0200024A RID: 586
public class SystemConsole : Singleton<SystemConsole>
{
	// Token: 0x06001D35 RID: 7477 RVA: 0x000C8E9C File Offset: 0x000C709C
	public SystemConsole.ConsoleCommand NewDeveloperCommand(string name, string help, string documentation, SystemConsole.ConsoleCommandFunction function, string defaults = "")
	{
		return new SystemConsole.ConsoleCommand(name, SystemConsole.CommandType.Command, SystemConsole.CommandPermission.Developer, help, documentation, function, defaults, null, SystemConsole.StoreAs.DoNotStore, SystemConsole.CommandOrigin.Internal, null);
	}

	// Token: 0x06001D36 RID: 7478 RVA: 0x000C8EBC File Offset: 0x000C70BC
	public SystemConsole.ConsoleCommand NewDeveloperVariable<T>(string name, SystemConsole.StoreAs storeAs, string help, string documentation, SystemConsole.ConsoleCommandFunction function, string defaults = "")
	{
		return new SystemConsole.ConsoleCommand(name, SystemConsole.CommandType.Variable, SystemConsole.CommandPermission.Developer, help, documentation, function, defaults, typeof(!!0), storeAs, SystemConsole.CommandOrigin.Internal, null);
	}

	// Token: 0x06001D37 RID: 7479 RVA: 0x000C8EE5 File Offset: 0x000C70E5
	public SystemConsole.ConsoleCommand NewDeveloperVariable<T>(string name, string help, string documentation, SystemConsole.ConsoleCommandFunction function, string defaults = "")
	{
		return this.NewDeveloperVariable<T>(name, SystemConsole.StoreAs.DoNotStore, help, documentation, function, defaults);
	}

	// Token: 0x06001D38 RID: 7480 RVA: 0x000C8EF5 File Offset: 0x000C70F5
	public SystemConsole.ConsoleCommand NewDeveloperVariable<T>(string name, SystemConsole.StoreAs storeAs, string help, SystemConsole.ConsoleCommandFunction function, string defaults = "")
	{
		return this.NewDeveloperVariable<T>(name, storeAs, help, this.DocsForVariable<T>(name), function, defaults);
	}

	// Token: 0x06001D39 RID: 7481 RVA: 0x000C8F0B File Offset: 0x000C710B
	public SystemConsole.ConsoleCommand NewDeveloperVariable<T>(string name, string help, SystemConsole.ConsoleCommandFunction function, string defaults = "")
	{
		return this.NewDeveloperVariable<T>(name, SystemConsole.StoreAs.DoNotStore, help, this.DocsForVariable<T>(name), function, defaults);
	}

	// Token: 0x06001D3A RID: 7482 RVA: 0x000C8F20 File Offset: 0x000C7120
	public SystemConsole.ConsoleCommand NewAdminCommand(string name, string help, string documentation, SystemConsole.ConsoleCommandFunction function, string defaults = "")
	{
		return new SystemConsole.ConsoleCommand(name, SystemConsole.CommandType.Command, SystemConsole.CommandPermission.Admin, help, documentation, function, defaults, null, SystemConsole.StoreAs.DoNotStore, SystemConsole.CommandOrigin.Internal, null);
	}

	// Token: 0x06001D3B RID: 7483 RVA: 0x000C8F40 File Offset: 0x000C7140
	public SystemConsole.ConsoleCommand NewAdminVariable<T>(string name, SystemConsole.StoreAs storeAs, string help, string documentation, SystemConsole.ConsoleCommandFunction function, string defaults = "")
	{
		return new SystemConsole.ConsoleCommand(name, SystemConsole.CommandType.Variable, SystemConsole.CommandPermission.Admin, help, documentation, function, defaults, typeof(!!0), storeAs, SystemConsole.CommandOrigin.Internal, null);
	}

	// Token: 0x06001D3C RID: 7484 RVA: 0x000C8F69 File Offset: 0x000C7169
	public SystemConsole.ConsoleCommand NewAdminVariable<T>(string name, string help, string documentation, SystemConsole.ConsoleCommandFunction function, string defaults = "")
	{
		return this.NewAdminVariable<T>(name, SystemConsole.StoreAs.DoNotStore, help, documentation, function, defaults);
	}

	// Token: 0x06001D3D RID: 7485 RVA: 0x000C8F79 File Offset: 0x000C7179
	public SystemConsole.ConsoleCommand NewAdminVariable<T>(string name, SystemConsole.StoreAs storeAs, string help, SystemConsole.ConsoleCommandFunction function, string defaults = "")
	{
		return this.NewAdminVariable<T>(name, storeAs, help, this.DocsForVariable<T>(name), function, defaults);
	}

	// Token: 0x06001D3E RID: 7486 RVA: 0x000C8F8F File Offset: 0x000C718F
	public SystemConsole.ConsoleCommand NewAdminVariable<T>(string name, string help, SystemConsole.ConsoleCommandFunction function, string defaults = "")
	{
		return this.NewAdminVariable<T>(name, SystemConsole.StoreAs.DoNotStore, help, this.DocsForVariable<T>(name), function, defaults);
	}

	// Token: 0x06001D3F RID: 7487 RVA: 0x000C8FA4 File Offset: 0x000C71A4
	public SystemConsole.ConsoleCommand NewPlayerCommand(string name, string help, string documentation, SystemConsole.ConsoleCommandFunction function, string defaults = "")
	{
		return new SystemConsole.ConsoleCommand(name, SystemConsole.CommandType.Command, SystemConsole.CommandPermission.Player, help, documentation, function, defaults, null, SystemConsole.StoreAs.DoNotStore, SystemConsole.CommandOrigin.Internal, null);
	}

	// Token: 0x06001D40 RID: 7488 RVA: 0x000C8FC4 File Offset: 0x000C71C4
	public SystemConsole.ConsoleCommand NewPlayerVariable<T>(string name, SystemConsole.StoreAs storeAs, string help, string documentation, SystemConsole.ConsoleCommandFunction function, string defaults = "")
	{
		return new SystemConsole.ConsoleCommand(name, SystemConsole.CommandType.Variable, SystemConsole.CommandPermission.Player, help, documentation, function, defaults, typeof(!!0), storeAs, SystemConsole.CommandOrigin.Internal, null);
	}

	// Token: 0x06001D41 RID: 7489 RVA: 0x000C8FED File Offset: 0x000C71ED
	public SystemConsole.ConsoleCommand NewPlayerVariable<T>(string name, string help, string documentation, SystemConsole.ConsoleCommandFunction function, string defaults = "")
	{
		return this.NewPlayerVariable<T>(name, SystemConsole.StoreAs.DoNotStore, help, documentation, function, defaults);
	}

	// Token: 0x06001D42 RID: 7490 RVA: 0x000C8FFD File Offset: 0x000C71FD
	public SystemConsole.ConsoleCommand NewPlayerVariable<T>(string name, SystemConsole.StoreAs storeAs, string help, SystemConsole.ConsoleCommandFunction function, string defaults = "")
	{
		return this.NewPlayerVariable<T>(name, storeAs, help, this.DocsForVariable<T>(name), function, defaults);
	}

	// Token: 0x06001D43 RID: 7491 RVA: 0x000C9013 File Offset: 0x000C7213
	public SystemConsole.ConsoleCommand NewPlayerVariable<T>(string name, string help, SystemConsole.ConsoleCommandFunction function, string defaults = "")
	{
		return this.NewPlayerVariable<T>(name, SystemConsole.StoreAs.DoNotStore, help, this.DocsForVariable<T>(name), function, defaults);
	}

	// Token: 0x06001D44 RID: 7492 RVA: 0x000C9028 File Offset: 0x000C7228
	public void OnEnable()
	{
		EventManager.OnLoadingComplete += this.CheckCommandsAwaitingOnLoad;
	}

	// Token: 0x06001D45 RID: 7493 RVA: 0x000C903B File Offset: 0x000C723B
	public void OnDestroy()
	{
		EventManager.OnLoadingComplete -= this.CheckCommandsAwaitingOnLoad;
	}

	// Token: 0x06001D46 RID: 7494 RVA: 0x000C904E File Offset: 0x000C724E
	protected override void Awake()
	{
		base.Awake();
		this.PureModePrimaryMaterial = new Material(this.PureModePrimaryMaterial);
		this.PureModeSecondaryMaterial = new Material(this.PureModeSecondaryMaterial);
		this.PureModeSplashMaterial = new Material(this.PureModeSplashMaterial);
	}

	// Token: 0x06001D47 RID: 7495 RVA: 0x000C9089 File Offset: 0x000C7289
	private IEnumerator Start()
	{
		this.isDeveloper = (Developer.HasSteamID(SteamManager.StringSteamID) && !SystemConsole.plebMode);
		this.DisplayConsoleTab(true);
		this.CreateCommands();
		this.CreateAliases();
		this.logFormat = SystemConsole.LogFormat.Expansive;
		this.logTablePrefix = "  ";
		this.logTableDepth = 3;
		this.logDisplayTag = false;
		this.LogTags["warning"] = new SystemConsole.LogTag("warning", Colour.Yellow, "", "");
		this.LogTags["error"] = new SystemConsole.LogTag("error", Colour.Red, "", "");
		string initialBatch = "";
		string BatchToExecute = "@bind ctrl+o @!ui_objects_window\n";
		SystemConsole.TimesStarted++;
		if (SystemConsole.TimesStarted == 1)
		{
			initialBatch = SerializationScript.LoadPlayerPref("SystemConsoleAutoexecBoot", DirectoryScript.bootexecFilePath);
		}
		string toAppend = SerializationScript.LoadPlayerPref("SystemConsoleAutoexecStart", DirectoryScript.autoexecFilePath);
		this.appendString(ref BatchToExecute, toAppend);
		if (SystemConsole.TimesStarted > 1)
		{
			toAppend = PlayerPrefs.GetString("SystemConsoleAutoexecOnce", "");
			this.appendString(ref BatchToExecute, toAppend);
		}
		PlayerPrefs.SetString("SystemConsoleAutoexecOnce", "");
		BatchToExecute = BatchToExecute.Replace("\t", "  ");
		yield return null;
		for (int i = 0; i < this.ConsoleCommandList.Count; i++)
		{
			string text = this.ConsoleCommandList[i];
			SystemConsole.ConsoleCommand consoleCommand = this.ConsoleCommands[text];
			if (this.CommandAvailable(consoleCommand) && consoleCommand.type == SystemConsole.CommandType.Variable && consoleCommand.storeAs != SystemConsole.StoreAs.DoNotStore && consoleCommand.storeAs != SystemConsole.StoreAs.Manual)
			{
				string text2 = " ";
				SystemConsole.CommandStatus commandStatus = new SystemConsole.CommandStatus(SystemConsole.CommandEcho.Loud);
				this.ExecuteCommand(text + " -return", ref commandStatus, SystemConsole.CommandEcho.Silent);
				if (SystemConsole.TimesStarted == 1)
				{
					SystemConsole.Defaults[text] = commandStatus.value;
				}
				switch (consoleCommand.storeAs)
				{
				case SystemConsole.StoreAs.Bool:
					if (PlayerPrefs.GetInt(text, ((bool)SystemConsole.Defaults[text]) ? 1 : 0) != 0)
					{
						text2 += true.ToString();
					}
					else
					{
						text2 += false.ToString();
					}
					break;
				case SystemConsole.StoreAs.Int:
					text2 += PlayerPrefs.GetInt(text, (int)SystemConsole.Defaults[text]);
					break;
				case SystemConsole.StoreAs.Float:
					text2 += PlayerPrefs.GetFloat(text, (float)SystemConsole.Defaults[text]);
					break;
				case SystemConsole.StoreAs.String:
					text2 += PlayerPrefs.GetString(text, (string)SystemConsole.Defaults[text]);
					break;
				}
				this.ExecuteCommand(text + text2, ref commandStatus, SystemConsole.CommandEcho.Silent);
			}
		}
		yield return null;
		if (initialBatch != "")
		{
			this.ProcessBatch(initialBatch, SystemConsole.CommandEcho.Quiet);
		}
		if (BatchToExecute != "")
		{
			this.ProcessBatch(BatchToExecute, SystemConsole.CommandEcho.Quiet);
		}
		SystemConsole.doneInitializing = true;
		yield break;
	}

	// Token: 0x06001D48 RID: 7496 RVA: 0x000C9098 File Offset: 0x000C7298
	private void appendString(ref string s, string toAppend)
	{
		if (toAppend != "")
		{
			if (s == "")
			{
				s = toAppend;
				return;
			}
			s = s + "\n" + toAppend;
		}
	}

	// Token: 0x06001D49 RID: 7497 RVA: 0x000C90C8 File Offset: 0x000C72C8
	public void Update()
	{
		if (zInput.GetButtonDown("System Console", ControlType.All) && !UICamera.GetKey(KeyCode.LeftShift) && !UICamera.GetKey(KeyCode.RightShift) && !NetworkSingleton<Chat>.Instance.isConsoleSwitching && (this.lockHotkey || !UICamera.SelectIsInput()))
		{
			NetworkSingleton<Chat>.Instance.ChatWindow.SetActive(true);
			if (NetworkSingleton<Chat>.Instance.selectedTab.chatType == ChatMessageType.System)
			{
				NetworkSingleton<Chat>.Instance.previousTab.SendMessage("OnClick");
			}
			else
			{
				this.SystemTab.SendMessage("OnClick");
			}
		}
		while (SystemConsole.DebugMessageHead != SystemConsole.DebugMessageTail)
		{
			SystemConsole.DebugMessageHead++;
			SystemConsole.DebugMessageHead %= 100;
			Chat.LogSystem(SystemConsole.DebugMessages[SystemConsole.DebugMessageHead], Colour.Orange, false);
		}
		if (UICamera.SelectIsInput())
		{
			foreach (KeyValuePair<ModifiedKeyCode, string> keyValuePair in this.KeyPressBinds)
			{
				if (TTSInput.GetKeyDown(keyValuePair.Key) && (keyValuePair.Key.keyCode >= KeyCode.F1 || keyValuePair.Key.ctrl || keyValuePair.Key.alt))
				{
					this.ProcessCommand(keyValuePair.Value, true, SystemConsole.CommandEcho.Quiet);
				}
			}
			using (Dictionary<ModifiedKeyCode, string>.Enumerator enumerator = this.KeyReleaseBinds.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<ModifiedKeyCode, string> keyValuePair2 = enumerator.Current;
					if (TTSInput.GetKeyUp(keyValuePair2.Key) && (keyValuePair2.Key.keyCode >= KeyCode.F1 || keyValuePair2.Key.ctrl || keyValuePair2.Key.alt))
					{
						this.ProcessCommand(keyValuePair2.Value, true, SystemConsole.CommandEcho.Quiet);
					}
				}
				return;
			}
		}
		foreach (KeyValuePair<ModifiedKeyCode, string> keyValuePair3 in this.KeyPressBinds)
		{
			if (TTSInput.GetKeyDown(keyValuePair3.Key))
			{
				this.ProcessCommand(keyValuePair3.Value, true, SystemConsole.CommandEcho.Quiet);
			}
		}
		foreach (KeyValuePair<ModifiedKeyCode, string> keyValuePair4 in this.KeyReleaseBinds)
		{
			if (TTSInput.GetKeyUp(keyValuePair4.Key))
			{
				this.ProcessCommand(keyValuePair4.Value, true, SystemConsole.CommandEcho.Quiet);
			}
		}
	}

	// Token: 0x06001D4A RID: 7498 RVA: 0x000C9388 File Offset: 0x000C7588
	public bool VRPadEvent(VRTrackedController.VRKeyCode keycode, VRTrackedController.VRKeyEvent padEvent)
	{
		if (padEvent == VRTrackedController.VRKeyEvent.Pressed)
		{
			if (this.VRKeyPressBinds.ContainsKey(keycode))
			{
				this.ProcessCommand(this.VRKeyPressBinds[keycode], true, SystemConsole.CommandEcho.Silent);
				return true;
			}
		}
		else if (padEvent == VRTrackedController.VRKeyEvent.Released)
		{
			if (this.VRKeyReleaseBinds.ContainsKey(keycode))
			{
				this.ProcessCommand(this.VRKeyReleaseBinds[keycode], true, SystemConsole.CommandEcho.Silent);
				return true;
			}
		}
		else if (padEvent == VRTrackedController.VRKeyEvent.LongPress && this.VRKeyLongPressBinds.ContainsKey(keycode))
		{
			this.ProcessCommand(this.VRKeyLongPressBinds[keycode], true, SystemConsole.CommandEcho.Silent);
			return true;
		}
		return false;
	}

	// Token: 0x06001D4B RID: 7499 RVA: 0x000C9410 File Offset: 0x000C7610
	private void CreateCommands()
	{
		this.CreateCommandsAM();
		this.CreateCommandsNZ();
		this.ConsoleCommandList.Clear();
		foreach (KeyValuePair<string, SystemConsole.ConsoleCommand> keyValuePair in this.ConsoleCommands)
		{
			this.ConsoleCommandList.Add(keyValuePair.Key);
		}
		this.ConsoleCommandList.Sort();
	}

	// Token: 0x06001D4C RID: 7500 RVA: 0x000C9490 File Offset: 0x000C7690
	private void CreateAliases()
	{
		List<string> list = new List<string>
		{
			"bool store_toggle",
			"string store_text",
			"float store_number",
			"int store_number",
			"run exec -v"
		};
		SystemConsole.CommandStatus commandStatus = new SystemConsole.CommandStatus(SystemConsole.CommandEcho.Loud);
		commandStatus.Batch();
		for (int i = 0; i < list.Count; i++)
		{
			this.ExecuteCommand("alias " + list[i], ref commandStatus, SystemConsole.CommandEcho.Silent);
		}
	}

	// Token: 0x06001D4D RID: 7501 RVA: 0x000C9512 File Offset: 0x000C7712
	public void RemoveCommand(string cmd)
	{
		if (this.ConsoleCommands.Remove(cmd))
		{
			this.ConsoleCommandList.Remove(cmd);
		}
	}

	// Token: 0x06001D4E RID: 7502 RVA: 0x000C9530 File Offset: 0x000C7730
	public void RemoveCommands(List<string> cmds)
	{
		for (int i = 0; i < cmds.Count; i++)
		{
			if (this.ConsoleCommands.Remove(cmds[i]))
			{
				this.ConsoleCommandList.Remove(cmds[i]);
			}
		}
	}

	// Token: 0x06001D4F RID: 7503 RVA: 0x000C9578 File Offset: 0x000C7778
	public void CreateThemeCommand(string name)
	{
		string text = "ui_theme_is_" + name.Trim().ToLower().Replace(" ", "_");
		if (this.ConsoleCommands.ContainsKey(text))
		{
			return;
		}
		string themeName = name.ToLower();
		string text2 = "Activate " + name + " Theme";
		this.ConsoleCommands.Add(text, this.NewPlayerCommand(text, text2, text2, delegate(SystemConsole.CommandStatus status, string parameters)
		{
			for (int i = 0; i < Singleton<UIPalette>.Instance.Themes.Count; i++)
			{
				if (Singleton<UIPalette>.Instance.Themes[i].name.ToLower() == themeName)
				{
					Singleton<UIThemeEditor>.Instance.LoadTheme(i, "");
					return;
				}
			}
		}, ""));
		this.ConsoleCommandList.Add(text);
	}

	// Token: 0x06001D50 RID: 7504 RVA: 0x000C9610 File Offset: 0x000C7810
	public void RemoveThemeCommand(string name)
	{
		string cmd = "ui_theme_is_" + name.Trim().ToLower().Replace(" ", "_");
		this.RemoveCommand(cmd);
	}

	// Token: 0x06001D51 RID: 7505 RVA: 0x000C964C File Offset: 0x000C784C
	private List<NetworkPhysicsObject> ComponentHelper(SystemConsole.CommandStatus status, string parameters)
	{
		bool flag;
		return this.ComponentHelper(status, parameters, out flag);
	}

	// Token: 0x06001D52 RID: 7506 RVA: 0x000C9664 File Offset: 0x000C7864
	private List<NetworkPhysicsObject> ComponentHelper(SystemConsole.CommandStatus status, string parameters, out bool held)
	{
		NetworkPhysicsObject networkPhysicsObject = null;
		List<NetworkPhysicsObject> list = new List<NetworkPhysicsObject>();
		string text = LibString.bite(ref parameters, false, ' ', false, false, '\0');
		held = false;
		if (text == null)
		{
			if (PlayerScript.PointerScript.FirstGrabbedObject)
			{
				networkPhysicsObject = PlayerScript.PointerScript.FirstGrabbedObject.GetComponent<NetworkPhysicsObject>();
			}
			if (networkPhysicsObject)
			{
				held = true;
				list.Add(networkPhysicsObject);
				foreach (NetworkPhysicsObject networkPhysicsObject2 in NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs)
				{
					if (networkPhysicsObject2.HeldByPlayerID == networkPhysicsObject.ID && !list.Contains(networkPhysicsObject2))
					{
						list.Add(networkPhysicsObject2);
					}
				}
			}
			NetworkPhysicsObject networkPhysicsObject3 = null;
			if (HoverScript.HoverObject && list.Count == 0)
			{
				networkPhysicsObject3 = HoverScript.HoverObject.GetComponent<NetworkPhysicsObject>();
				if (networkPhysicsObject3 && (networkPhysicsObject3.HeldByPlayerID == NetworkID.ID || networkPhysicsObject3.IsHeldByNobody))
				{
					list.Add(networkPhysicsObject3);
				}
			}
			using (List<GameObject>.Enumerator enumerator2 = PlayerScript.PointerScript.GetSelectedObjects(networkPhysicsObject3 ? networkPhysicsObject3.ID : -1, true, true).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					GameObject gameObject = enumerator2.Current;
					networkPhysicsObject = gameObject.GetComponent<NetworkPhysicsObject>();
					if (networkPhysicsObject && (networkPhysicsObject.HeldByPlayerID == NetworkID.ID || networkPhysicsObject.IsHeldByNobody) && !list.Contains(networkPhysicsObject))
					{
						list.Add(networkPhysicsObject);
					}
				}
				return list;
			}
		}
		networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGUID(text);
		if (networkPhysicsObject == null)
		{
			this.Log("Invalid Component: " + text, false);
		}
		while (networkPhysicsObject)
		{
			if (networkPhysicsObject.HeldByPlayerID == NetworkID.ID)
			{
				held = true;
			}
			list.Add(networkPhysicsObject);
			text = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGUID(text);
			if (text != null && networkPhysicsObject == null)
			{
				this.Log("Invalid Component: " + text, false);
			}
		}
		return list;
	}

	// Token: 0x06001D53 RID: 7507 RVA: 0x000C9878 File Offset: 0x000C7A78
	private void ChatTabHelper(SystemConsole.CommandStatus status, string parameters, ChatMessageType type)
	{
		if (type == ChatMessageType.System)
		{
			SystemConsole.dontFocusNextSwitch = true;
			if (LibString.bite(ref parameters, true, ' ', false, false, '\0') == "-f")
			{
				SystemConsole.dontFocusNextSwitch = false;
			}
		}
		for (int i = 0; i < NetworkSingleton<Chat>.Instance.chatTabs.Length; i++)
		{
			ChatTab chatTab = NetworkSingleton<Chat>.Instance.chatTabs[i];
			if (chatTab.chatType == type && chatTab.gameObject.activeInHierarchy)
			{
				chatTab.gameObject.SendMessage("OnClick");
				return;
			}
		}
	}

	// Token: 0x06001D54 RID: 7508 RVA: 0x000C98FC File Offset: 0x000C7AFC
	private void UIButtonHelper(string cmd, SystemConsole.CommandStatus status, string parameters, string path)
	{
		GameObject gameObject = NetworkSingleton<NetworkUI>.Instance.GUITopBar.transform.Find("Anchor/Grid").gameObject;
		GameObject gameObject2 = gameObject.transform.Find(path).gameObject;
		bool activeInHierarchy = gameObject2.activeInHierarchy;
		this.Variable(cmd, ref activeInHierarchy, ref status, parameters);
		if (status.isDirty)
		{
			gameObject2.SetActive(activeInHierarchy);
			gameObject.GetComponent<UIGrid>().enabled = true;
		}
	}

	// Token: 0x06001D55 RID: 7509 RVA: 0x000C996C File Offset: 0x000C7B6C
	private bool TestHelper(SystemConsole.CommandStatus status, string parameters, out GameObject gameObject, out int count)
	{
		gameObject = null;
		count = 1000000;
		string text = LibString.bite(ref parameters, false, ' ', false, false, '\0');
		if (text == null)
		{
			this.Log("You must provide a string <guid>.", false);
			return false;
		}
		string text2 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
		if (text2 != null && (!int.TryParse(text2, out count) || count < 1))
		{
			this.Log("Could not parse <count>", false);
			return false;
		}
		gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromGUID(text);
		if (gameObject == null)
		{
			this.Log("Object not found.", false);
			return false;
		}
		return true;
	}

	// Token: 0x06001D56 RID: 7510 RVA: 0x000C99F8 File Offset: 0x000C7BF8
	private void PlayerPrefHelper(SystemConsole.CommandStatus status, string parameters, Type type)
	{
		string text = LibString.bite(ref parameters, false, ' ', false, false, '\0');
		if (text == null)
		{
			this.Log("You must specify a PlayerPref entry.", false);
			return;
		}
		if (!PlayerPrefs.HasKey(text))
		{
			this.Log("PlayerPref \"" + text + "\" not found.", false);
			return;
		}
		int num = 0;
		float num2 = 0f;
		string text2 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
		if (text2 != null)
		{
			if (type == typeof(int))
			{
				if (!int.TryParse(text2, out num))
				{
					this.Log("Unable to parse int: " + text2, false);
					return;
				}
				PlayerPrefs.SetInt(text, num);
				PlayerPrefs.SetInt(text, num);
			}
			else if (type == typeof(float))
			{
				if (!float.TryParse(text2, out num2))
				{
					this.Log("Unable to parse float: " + text2, false);
					return;
				}
				PlayerPrefs.SetFloat(text, num2);
			}
			else
			{
				PlayerPrefs.SetString(text, text2);
			}
		}
		if (!status.isSilent)
		{
			if (type == typeof(int))
			{
				num = PlayerPrefs.GetInt(text);
				this.Log(text + ": [B8B601]" + num, status.inBatch);
			}
			else if (type == typeof(float))
			{
				num2 = PlayerPrefs.GetFloat(text);
				this.Log(text + ": [B8B601]" + num2, status.inBatch);
			}
			else
			{
				text2 = PlayerPrefs.GetString(text);
				if (text2 == "")
				{
					this.Log(text + ": [B8B601]\"\"", status.inBatch);
				}
				else if (text2.Contains("\n"))
				{
					this.Log(text + " is:\n[B8B601]" + text2, status.inBatch);
				}
				else
				{
					this.Log(text + ": [B8B601]" + text2, status.inBatch);
				}
			}
		}
		SystemConsole.lastReturnedValue = text2;
	}

	// Token: 0x06001D57 RID: 7511 RVA: 0x000C9BD0 File Offset: 0x000C7DD0
	public void LogTagsHelper(ref List<string> list, string input)
	{
		list.Clear();
		input = input.Replace('\n', ' ');
		for (string item = LibString.bite(ref input, true, ' ', false, false, '\0'); item != null; item = LibString.bite(ref input, true, ' ', false, false, '\0'))
		{
			list.Add(item);
		}
	}

	// Token: 0x06001D58 RID: 7512 RVA: 0x000C9C1C File Offset: 0x000C7E1C
	public void LogFormatHelper(string command, ref SystemConsole.LogTag style, ref SystemConsole.CommandStatus status, ref string parameters)
	{
		status.value = null;
		string text = LibString.bite(ref parameters, false, ' ', false, false, '\0');
		if (text == null)
		{
			if (!status.isSilent)
			{
				this.Log(string.Format("Colour: {0}\nPrefix: {1}\nPostfix: {2}", style.colour.Hex.Replace("[", "#").Replace("]", ""), style.prefix, style.postfix), style.colour, false);
			}
			return;
		}
		if (text == "-return")
		{
			status.value = style;
			return;
		}
		if (text == "-variable")
		{
			if (!status.isSilent)
			{
				this.LogVariable(command, string.Format("{3}Colour={0} Prefix={1} Postfix={2}", new object[]
				{
					style.colour.Hex.Replace("[", "#").Replace("]", ""),
					style.prefix,
					style.postfix,
					style.colour.RGBHex
				}), 37);
			}
			return;
		}
		if (text.Substring(0, 1) == "#")
		{
			try
			{
				style.colour = Colour.ColourFromRGBHex(text);
				goto IL_16F;
			}
			catch
			{
				this.Log("You must specify RGB colour as #RRGGBB", false);
				return;
			}
		}
		text = text.Substring(0, 1).ToUpper() + text.Substring(1).ToLower();
		style.colour = Colour.ColourFromLabel(text);
		IL_16F:
		status.value = style;
		text = LibString.bite(ref parameters, false, ' ', false, false, '\0');
		if (text != null)
		{
			style.prefix = SystemConsole.ConvertEscapesToCharacters(text);
			text = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text != null)
			{
				style.postfix = SystemConsole.ConvertEscapesToCharacters(text);
			}
		}
		if (!status.isSilent)
		{
			this.Log(string.Format("Colour: {0}\nPrefix: {1}\nPostfix: {2}", style.colour.Hex.Replace("[", "#").Replace("]", ""), style.prefix, style.postfix), style.colour, false);
		}
	}

	// Token: 0x06001D59 RID: 7513 RVA: 0x000C9E50 File Offset: 0x000C8050
	public void ComponentTransformHelper(string cmd, SystemConsole.CommandStatus status, string parameters, SystemConsole.TransformType type)
	{
		string text = LibString.bite(ref parameters, false, ' ', false, false, '\0');
		if (text == null)
		{
			this.Log("Specify component", false);
			return;
		}
		GameObject gameObject = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromGUID(text);
		if (gameObject == null)
		{
			this.Log("No such component.", false);
			return;
		}
		bool fastSpeed = false;
		if (LibString.lookAhead(parameters, false, ' ', 0, false, false) == "-f")
		{
			LibString.bite(ref parameters, false, ' ', false, false, '\0');
			fastSpeed = true;
		}
		bool flag = true;
		float x = 0f;
		bool flag2 = true;
		float y = 0f;
		bool flag3 = true;
		float z = 0f;
		parameters = parameters.Replace("(", "");
		parameters = parameters.Replace(")", "");
		parameters = parameters.Replace(",", " ");
		string text2 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
		if (text2 == "-")
		{
			flag = false;
		}
		else if (!float.TryParse(text2, out x))
		{
			this.Log("Could not parse x value", false);
			return;
		}
		text2 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
		if (text2 == "-")
		{
			flag2 = false;
		}
		else if (!float.TryParse(text2, out y))
		{
			this.Log("Could not parse y value", false);
			return;
		}
		text2 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
		if (text2 == "-")
		{
			flag3 = false;
		}
		else if (!float.TryParse(text2, out z))
		{
			this.Log("Could not parse z value", false);
			return;
		}
		Vector3 vector = new Vector3(x, y, z);
		NetworkPhysicsObject component = gameObject.GetComponent<NetworkPhysicsObject>();
		switch (type)
		{
		case SystemConsole.TransformType.MoveRelative:
			if (!component.currentSmoothPosition.Moving)
			{
				Vector3 position = gameObject.transform.position;
				if (flag)
				{
					position.x += vector.x;
				}
				if (flag2)
				{
					position.y += vector.y;
				}
				if (flag3)
				{
					position.z += vector.z;
				}
				component.RPCSetSmoothPosition(position, false, fastSpeed);
				return;
			}
			break;
		case SystemConsole.TransformType.MoveAbsolute:
			if (!component.currentSmoothPosition.Moving)
			{
				Vector3 position2 = gameObject.transform.position;
				if (flag)
				{
					position2.x = vector.x;
				}
				if (flag2)
				{
					position2.y = vector.y;
				}
				if (flag3)
				{
					position2.z = vector.z;
				}
				component.RPCSetSmoothPosition(position2, false, fastSpeed);
				return;
			}
			break;
		case SystemConsole.TransformType.RotateRelative:
			if (!component.currentSmoothRotation.Moving)
			{
				Vector3 eulerAngles = gameObject.transform.rotation.eulerAngles;
				if (flag)
				{
					eulerAngles.x += vector.x;
				}
				if (flag2)
				{
					eulerAngles.y += vector.y;
				}
				if (flag3)
				{
					eulerAngles.z += vector.z;
				}
				component.RPCSetSmoothRotation(eulerAngles, false, fastSpeed);
			}
			break;
		case SystemConsole.TransformType.RotateAbsolute:
			if (!component.currentSmoothRotation.Moving)
			{
				Vector3 eulerAngles2 = gameObject.transform.rotation.eulerAngles;
				if (flag)
				{
					eulerAngles2.x = vector.x;
				}
				if (flag2)
				{
					eulerAngles2.y = vector.y;
				}
				if (flag3)
				{
					eulerAngles2.z = vector.z;
				}
				component.RPCSetSmoothRotation(eulerAngles2, false, fastSpeed);
				return;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06001D5A RID: 7514 RVA: 0x000CA194 File Offset: 0x000C8394
	public void Variable(string label, ref bool var, ref SystemConsole.CommandStatus status, string parameters)
	{
		status.value = var;
		if (status.isReturn)
		{
			return;
		}
		string text = LibString.bite(ref parameters, false, ' ', false, false, '\0');
		if (text != null)
		{
			if (text == "-return")
			{
				return;
			}
			if (text == "-variable")
			{
				if (!status.isSilent)
				{
					if (var)
					{
						this.LogVariable(label, "ON", Colour.Blue, 37);
						return;
					}
					this.LogVariable(label, "OFF", Colour.Red, 37);
				}
				return;
			}
			if (!status.isReadOnly)
			{
				if (LibString.StringIsTrue(text))
				{
					var = true;
				}
				else if (LibString.StringIsFalse(text))
				{
					var = false;
				}
				else
				{
					if (!LibString.StringIsToggle(text))
					{
						this.Log("Boolean can only be set to ON, OFF, or TOGGLE.", false);
						return;
					}
					var = !var;
				}
				status.Dirty();
			}
		}
		if (var)
		{
			if (!status.isSilent)
			{
				this.Log(label + ": ON", Colour.Blue, status.inBatch);
			}
			SystemConsole.lastReturnedValue = "true";
		}
		else
		{
			if (!status.isSilent)
			{
				this.Log(label + ": OFF", Colour.Red, status.inBatch);
			}
			SystemConsole.lastReturnedValue = "false";
		}
		status.value = var;
	}

	// Token: 0x06001D5B RID: 7515 RVA: 0x000CA2E4 File Offset: 0x000C84E4
	public void Variable(string label, ref int var, ref SystemConsole.CommandStatus status, string parameters)
	{
		status.value = var;
		if (status.isReturn)
		{
			return;
		}
		string text = LibString.lookAhead(parameters, true, ' ', 0, false, false);
		if (text == "-return")
		{
			return;
		}
		if (text == "-variable")
		{
			if (!status.isSilent)
			{
				this.LogVariable(label, var.ToString(), Colour.Pink, 37);
			}
			return;
		}
		if (text != null && !status.isReadOnly)
		{
			int num = var;
			if (!int.TryParse(text, out num))
			{
				this.Log("Could not parse int value", false);
				return;
			}
			var = num;
			status.Dirty();
		}
		SystemConsole.lastReturnedValue = var.ToString();
		if (!status.isSilent)
		{
			this.Log(label + ": [F570CE]" + SystemConsole.lastReturnedValue, status.inBatch);
		}
		status.value = var;
	}

	// Token: 0x06001D5C RID: 7516 RVA: 0x000CA3C4 File Offset: 0x000C85C4
	public void Variable(string label, ref float var, ref SystemConsole.CommandStatus status, string parameters)
	{
		status.value = var;
		if (status.isReturn)
		{
			return;
		}
		string text = LibString.lookAhead(parameters, true, ' ', 0, false, false);
		if (text == "-return")
		{
			return;
		}
		if (text == "-variable")
		{
			if (!status.isSilent)
			{
				this.LogVariable(label, LibString.StringFromFloat(var), Colour.Pink, 37);
			}
			return;
		}
		if (text != null && !status.isReadOnly)
		{
			float num = var;
			if (!float.TryParse(text, out num))
			{
				this.Log("Could not parse float value", false);
				return;
			}
			var = num;
			status.Dirty();
		}
		SystemConsole.lastReturnedValue = LibString.StringFromFloat(var);
		if (!status.isSilent)
		{
			this.Log(label + ": [F570CE]" + SystemConsole.lastReturnedValue, status.inBatch);
		}
		status.value = var;
	}

	// Token: 0x06001D5D RID: 7517 RVA: 0x000CA4A4 File Offset: 0x000C86A4
	public void Variable(string label, ref Vector3 var, ref SystemConsole.CommandStatus status, string parameters)
	{
		status.value = var;
		if (status.isReturn)
		{
			return;
		}
		string text = LibString.lookAhead(parameters, true, ' ', 0, false, false);
		if (text == "-return")
		{
			return;
		}
		if (text == "-variable")
		{
			if (!status.isSilent)
			{
				this.LogVariable(label, var.ToString(), Colour.Pink, 37);
			}
			return;
		}
		if (text != null && !status.isReadOnly)
		{
			parameters = parameters.Replace("(", "");
			parameters = parameters.Replace(")", "");
			parameters = parameters.Replace(",", " ");
			text = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			float x;
			if (!float.TryParse(text, out x))
			{
				this.Log("Could not parse x value", false);
				return;
			}
			text = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			float y;
			if (!float.TryParse(text, out y))
			{
				this.Log("Could not parse y value", false);
				return;
			}
			text = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			float z;
			if (!float.TryParse(text, out z))
			{
				this.Log("Could not parse z value", false);
				return;
			}
			var = new Vector3(x, y, z);
			status.Dirty();
		}
		SystemConsole.lastReturnedValue = var.ToString();
		SystemConsole.lastReturnedValue = SystemConsole.lastReturnedValue.Replace("(", "");
		SystemConsole.lastReturnedValue = SystemConsole.lastReturnedValue.Replace(")", "");
		SystemConsole.lastReturnedValue = SystemConsole.lastReturnedValue.Replace(",", " ");
		SystemConsole.lastReturnedValue = SystemConsole.lastReturnedValue.Replace("  ", " ");
		if (!status.isSilent)
		{
			this.Log(label + ": [F570CE]" + SystemConsole.lastReturnedValue, status.inBatch);
		}
		status.value = var;
	}

	// Token: 0x06001D5E RID: 7518 RVA: 0x000CA698 File Offset: 0x000C8898
	public void Variable(string label, ref Vector2 var, ref SystemConsole.CommandStatus status, string parameters)
	{
		status.value = var;
		if (status.isReturn)
		{
			return;
		}
		string text = LibString.lookAhead(parameters, true, ' ', 0, false, false);
		if (text == "-return")
		{
			return;
		}
		if (text == "-variable")
		{
			if (!status.isSilent)
			{
				this.LogVariable(label, var.ToString(), Colour.Pink, 37);
			}
			return;
		}
		if (text != null && !status.isReadOnly)
		{
			parameters = parameters.Replace("(", "");
			parameters = parameters.Replace(")", "");
			parameters = parameters.Replace(",", " ");
			text = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			float x;
			if (!float.TryParse(text, out x))
			{
				this.Log("Could not parse x value", false);
				return;
			}
			text = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			float y;
			if (!float.TryParse(text, out y))
			{
				this.Log("Could not parse y value", false);
				return;
			}
			var = new Vector2(x, y);
			status.Dirty();
		}
		SystemConsole.lastReturnedValue = var.ToString();
		SystemConsole.lastReturnedValue = SystemConsole.lastReturnedValue.Replace("(", "");
		SystemConsole.lastReturnedValue = SystemConsole.lastReturnedValue.Replace(")", "");
		SystemConsole.lastReturnedValue = SystemConsole.lastReturnedValue.Replace(",", " ");
		SystemConsole.lastReturnedValue = SystemConsole.lastReturnedValue.Replace("  ", " ");
		if (!status.isSilent)
		{
			this.Log(label + ": [F570CE]" + SystemConsole.lastReturnedValue, status.inBatch);
		}
		status.value = var;
	}

	// Token: 0x06001D5F RID: 7519 RVA: 0x000CA864 File Offset: 0x000C8A64
	public void Variable(string label, ref string var, ref SystemConsole.CommandStatus status, string parameters)
	{
		status.value = var;
		string text = LibString.lookAhead(parameters, true, ' ', 0, false, false);
		if (text == "-return")
		{
			return;
		}
		if (text == "-variable")
		{
			if (!status.isSilent)
			{
				this.LogVariable(label, var, Colour.YellowDark, 37);
			}
			return;
		}
		if (!status.isReadOnly)
		{
			if (text == "-e")
			{
				UIDialog.ShowMemoInput(label, "Ok", "Cancel", new Action<string, string>(this.SetStringVariable), null, var, "Edit Variable");
				return;
			}
			if (text == "-clear")
			{
				var = "";
			}
			else if (text != null)
			{
				if (status.isRaw)
				{
					var = parameters;
				}
				else
				{
					string a = parameters;
					text = LibString.bite(ref a, false, ' ', false, false, '\0');
					if (a == "")
					{
						var = text;
					}
					else
					{
						int num = 0;
						while (num < parameters.Length && parameters[num] == ' ')
						{
							num++;
						}
						var = parameters.Substring(num);
					}
				}
				status.Dirty();
			}
		}
		SystemConsole.lastReturnedValue = var;
		if (!status.isSilent)
		{
			if (var == "")
			{
				this.Log(label + ": [B8B601]\"\"", status.inBatch);
			}
			else if (var != null && var.Contains("\n"))
			{
				if (status.inBatch)
				{
					this.Log("[B8B601]" + var, true);
				}
				else
				{
					this.Log(label + " is:\n[B8B601]" + var, false);
				}
			}
			else
			{
				this.Log(label + ": [B8B601]" + var, status.inBatch);
			}
		}
		status.value = var;
	}

	// Token: 0x06001D60 RID: 7520 RVA: 0x000CAA1C File Offset: 0x000C8C1C
	public void Variable(string label, ref PointerMode var, ref SystemConsole.CommandStatus status, string parameters)
	{
		status.value = var;
		string text = LibString.lookAhead(parameters, true, ' ', 0, false, false);
		if (text == "-return")
		{
			return;
		}
		if (text == "-variable")
		{
			if (!status.isSilent)
			{
				this.LogVariable(label, var.ToString(), Colour.Pink, 37);
			}
			return;
		}
		text = LibString.bite(ref parameters, false, ' ', false, false, '\0');
		if (text != null && !status.isReadOnly)
		{
			int num = -1;
			if (int.TryParse(text, out num))
			{
				if (!Enum.IsDefined(typeof(PointerMode), num))
				{
					this.Log("Could not parse tool mode " + text, false);
					return;
				}
				var = (PointerMode)num;
				status.Dirty();
			}
			else
			{
				string value = LibString.CamelCaseFromUnderscore(text, true, false);
				PointerMode pointerMode;
				try
				{
					pointerMode = (PointerMode)Enum.Parse(typeof(PointerMode), value);
				}
				catch (ArgumentException)
				{
					this.Log("Could not parse tool mode " + text, false);
					return;
				}
				var = pointerMode;
				status.Dirty();
			}
		}
		SystemConsole.lastReturnedValue = LibString.UnderscoreFromCamelCase(var.ToString());
		if (!status.isSilent)
		{
			this.Log(label + ": [F570CE]" + SystemConsole.lastReturnedValue, status.inBatch);
		}
		status.value = var;
	}

	// Token: 0x06001D61 RID: 7521 RVA: 0x000CAB84 File Offset: 0x000C8D84
	public void SetStringVariable(string text, string variable)
	{
		string text2 = LibString.lookAhead(text, false, ' ', 0, false, false);
		if (text2 == null || text2 == "")
		{
			this.ProcessCommand(variable + " -clear", false, SystemConsole.CommandEcho.Quiet);
			return;
		}
		SystemConsole.CommandStatus commandStatus = new SystemConsole.CommandStatus(SystemConsole.CommandEcho.Loud);
		commandStatus.Raw();
		commandStatus.Quiet();
		this.ExecuteCommand(variable + " " + text, ref commandStatus);
	}

	// Token: 0x06001D62 RID: 7522 RVA: 0x000CABE9 File Offset: 0x000C8DE9
	public void LogVariable(string label, string value, int padding = 37)
	{
		this.Log(string.Format("{0,-" + padding + "} {1}", label, value), true);
	}

	// Token: 0x06001D63 RID: 7523 RVA: 0x000CAC0E File Offset: 0x000C8E0E
	public void LogVariable(string label, string value, Colour colour, int padding = 37)
	{
		this.Log(string.Format("{0,-" + padding + "} {1}{2}", label, colour.RGBHex, value.Replace("\n", "; ")), true);
	}

	// Token: 0x06001D64 RID: 7524 RVA: 0x000CAC4C File Offset: 0x000C8E4C
	public string DocsForVariable<T>(string label)
	{
		if (typeof(!!0) == typeof(bool))
		{
			return string.Format("USAGE: {0} {{ON|OFF|TOGGLE}}\nDisplays value of {1}{0}[-].  If value parameter provided then sets {1}{0}[-] to value specified (if {3}TOGGLE[-] then its value is inverted).\nShortcut by prefixing with: {2}+ - ![-]\nFor example, {1}!{0}[-] = '{1}{0} TOGGLE[-]'", new object[]
			{
				label,
				"[21B19B]",
				"[A64B00]",
				"[B8B601]"
			});
		}
		if (typeof(!!0) == typeof(string))
		{
			return string.Format("USAGE: {0} {{value}} OR {0} -e\nDisplays value of {1}{0}[-].  If value parameter provided then sets {1}{0}[-] to value specified.\n   -e = Edit in UI editor", label, "[21B19B]");
		}
		if (typeof(!!0) == typeof(int))
		{
			return string.Format("USAGE: {0} {{value}}\nDisplays value of {1}{0}[-].  If value parameter provided then sets {1}{0}[-] to value specified.\n", label, "[F570CE]");
		}
		if (typeof(!!0) == typeof(float))
		{
			return string.Format("USAGE: {0} {{value}}\nDisplays value of {1}{0}[-].  If value parameter provided then sets {1}{0}[-] to value specified.\n", label, "[F570CE]");
		}
		if (typeof(!!0) == typeof(Vector3))
		{
			return string.Format("USAGE: {0} {{x y z}}\nDisplays value of {1}{0}[-].\nIf {{x y z}} parameters provided then sets {1}{0}[-] to value specified.\n", label, "[F570CE]");
		}
		if (typeof(!!0) == typeof(Colour))
		{
			return string.Format("USAGE: {0} {{#RRGGBB}} OR {0} {{#RRGGBBAA}}\nDisplays value of {1}{0}[-].\nIf {{RRGGBB{{AA}}}} parameters provided then sets {1}{0}[-] to value specified.\n", label, "[F570CE]");
		}
		return string.Format("USAGE: {0} {{value}}\nDisplay value of {0}.  If value parameter provided then sets {0} to value specified.", label);
	}

	// Token: 0x06001D65 RID: 7525 RVA: 0x000CAD88 File Offset: 0x000C8F88
	private void copyHelpToClipboard()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("# Console Commands\n");
		for (int i = 0; i < this.ConsoleCommandList.Count; i++)
		{
			string text = this.ConsoleCommandList[i];
			SystemConsole.ConsoleCommand consoleCommand = this.ConsoleCommands[text];
			if (consoleCommand.permission != SystemConsole.CommandPermission.Developer)
			{
				stringBuilder.Append("\n\n\n## `");
				stringBuilder.Append(text);
				if (consoleCommand.permission == SystemConsole.CommandPermission.Admin)
				{
					stringBuilder.Append("` (admin)");
				}
				else
				{
					stringBuilder.Append("`");
				}
				stringBuilder.Append("\n\n");
				stringBuilder.Append(consoleCommand.help);
				stringBuilder.Append("\n\n");
				stringBuilder.Append(consoleCommand.documentation.Replace("\n", "\n\n"));
			}
		}
		string text2 = stringBuilder.ToString();
		text2 = Regex.Replace(text2, "\\[[0-9a-zA-Z]{6}\\]", "`");
		text2 = text2.Replace("[-]", "`");
		text2 = text2.Replace("{", "[");
		text2 = text2.Replace("}", "]");
		text2 = Regex.Replace(text2, "USAGE:\\s*(.*)", "USAGE: `$1`");
		bool flag = false;
		StringBuilder stringBuilder2 = new StringBuilder();
		foreach (char c in text2)
		{
			if (flag)
			{
				if (c == '`')
				{
					flag = false;
				}
				stringBuilder2.Append(c);
			}
			else if (c == '`')
			{
				flag = true;
				stringBuilder2.Append(c);
			}
			else if (c == '<')
			{
				stringBuilder2.Append("&lt;");
			}
			else if (c == '>')
			{
				stringBuilder2.Append("&gt;");
			}
			else
			{
				stringBuilder2.Append(c);
			}
		}
		GUIUtility.systemCopyBuffer = stringBuilder2.ToString();
	}

	// Token: 0x06001D66 RID: 7526 RVA: 0x000CAF53 File Offset: 0x000C9153
	public bool CommandAvailable(string commandName)
	{
		return this.ConsoleCommands.ContainsKey(commandName) && this.CommandAvailable(this.ConsoleCommands[commandName]);
	}

	// Token: 0x06001D67 RID: 7527 RVA: 0x000CAF77 File Offset: 0x000C9177
	public bool CommandAvailable(SystemConsole.ConsoleCommand command)
	{
		return command != null && this.CommandAvailable(command.permission);
	}

	// Token: 0x06001D68 RID: 7528 RVA: 0x000CAF8A File Offset: 0x000C918A
	public bool CommandAvailable(SystemConsole.CommandPermission permission)
	{
		if (permission == SystemConsole.CommandPermission.Player)
		{
			return true;
		}
		if (permission == SystemConsole.CommandPermission.Developer)
		{
			return this.isDeveloper;
		}
		return Network.peerType == NetworkPeerMode.Disconnected || Network.peerType == NetworkPeerMode.Connecting || Network.maxConnections == 0 || Network.isAdmin;
	}

	// Token: 0x06001D69 RID: 7529 RVA: 0x000CAFBC File Offset: 0x000C91BC
	public void ProcessCommand(string command, bool inBatch, SystemConsole.CommandEcho echo)
	{
		SystemConsole.CommandStatus commandStatus = new SystemConsole.CommandStatus(SystemConsole.CommandEcho.Loud);
		if (inBatch)
		{
			commandStatus.Batch();
		}
		base.StartCoroutine(this.ProcessCommand(command, commandStatus, echo));
	}

	// Token: 0x06001D6A RID: 7530 RVA: 0x000CAFE9 File Offset: 0x000C91E9
	public void ProcessCommand(string command, bool inBatch)
	{
		this.ProcessCommand(command, inBatch, this.echoMode);
	}

	// Token: 0x06001D6B RID: 7531 RVA: 0x000CAFF9 File Offset: 0x000C91F9
	private IEnumerator ProcessCommand(string command, SystemConsole.CommandStatus status, SystemConsole.CommandEcho echo = SystemConsole.CommandEcho.Loud)
	{
		this.ExecuteCommand(command, ref status, echo);
		float timeoutAt = Time.time + 15f;
		while (status.isOpen && Time.time < timeoutAt)
		{
			yield return new WaitForSeconds(0.5f);
		}
		if (Time.time >= timeoutAt)
		{
			Chat.LogError("Timed out waiting for command to complete: " + command, true);
		}
		yield break;
	}

	// Token: 0x06001D6C RID: 7532 RVA: 0x000CB01D File Offset: 0x000C921D
	private void ExecuteCommand(string commandToParse, ref SystemConsole.CommandStatus status)
	{
		this.ExecuteCommand(commandToParse, ref status, this.echoMode);
	}

	// Token: 0x06001D6D RID: 7533 RVA: 0x000CB030 File Offset: 0x000C9230
	private void ExecuteCommand(string commandToParse, ref SystemConsole.CommandStatus status, SystemConsole.CommandEcho echo)
	{
		string text = string.Copy(commandToParse);
		string text2 = LibString.bite(ref text, true, ' ', false, false, '\0');
		if (text2 == "" || text2 == null)
		{
			return;
		}
		SystemConsole.CommandEcho echo2 = status.echo;
		if (text2.StartsWith("@"))
		{
			echo = SystemConsole.CommandEcho.Silent;
			text2 = text2.Substring(1);
		}
		if (echo == SystemConsole.CommandEcho.Quiet && status.isLoud)
		{
			status.Quiet();
		}
		else if (echo == SystemConsole.CommandEcho.Silent)
		{
			status.Silent();
		}
		if (text2 == "" || text2 == null)
		{
			return;
		}
		text2 = text2.ToLower();
		if (!status.inBatch && echo != SystemConsole.CommandEcho.Silent)
		{
			this.Newline();
		}
		if (echo == SystemConsole.CommandEcho.Loud)
		{
			this.Log(">" + commandToParse, Color.grey, false);
		}
		if (!text2.StartsWith("/"))
		{
			if (text2[0] == '+' || text2[0] == '-' || text2[0] == '!')
			{
				text = text + " " + text2[0].ToString();
				text2 = text2.Substring(1);
			}
			SystemConsole.ConsoleCommand consoleCommand;
			this.ConsoleCommands.TryGetValue(text2, out consoleCommand);
			if (this.CommandAvailable(consoleCommand))
			{
				if (!status.isRaw)
				{
					text = this.InsertVariablesOrReduce(text);
				}
				consoleCommand.code(status, consoleCommand.defaults + " " + text);
				if (consoleCommand.type == SystemConsole.CommandType.Variable && status.isDirty)
				{
					object obj;
					if (consoleCommand.storeAs != SystemConsole.StoreAs.DoNotStore && consoleCommand.storeAs != SystemConsole.StoreAs.Manual && SystemConsole.Defaults.TryGetValue(text2, out obj))
					{
						switch (consoleCommand.storeAs)
						{
						case SystemConsole.StoreAs.Bool:
							if ((bool)status.value != (bool)obj)
							{
								PlayerPrefs.SetInt(text2, ((bool)status.value) ? 1 : 0);
							}
							else
							{
								PlayerPrefs.DeleteKey(text2);
							}
							break;
						case SystemConsole.StoreAs.Int:
							if ((int)status.value != (int)obj)
							{
								PlayerPrefs.SetInt(text2, (int)status.value);
							}
							else
							{
								PlayerPrefs.DeleteKey(text2);
							}
							break;
						case SystemConsole.StoreAs.Float:
							if ((float)status.value != (float)obj)
							{
								PlayerPrefs.SetFloat(text2, (float)status.value);
							}
							else
							{
								PlayerPrefs.DeleteKey(text2);
							}
							break;
						case SystemConsole.StoreAs.String:
							if ((string)status.value != (string)obj)
							{
								PlayerPrefs.SetString(text2, (string)status.value);
							}
							else
							{
								PlayerPrefs.DeleteKey(text2);
							}
							break;
						}
					}
					if (consoleCommand.variableType == typeof(bool) && !status.isReadOnly)
					{
						bool flag = (bool)status.value;
						UIToggle uitoggle;
						if (this.uiToggleToUpdate.TryGetValue(text2, out uitoggle) && uitoggle)
						{
							uitoggle.Set(flag, false);
						}
						string command;
						if (!this.currentlyRunning.Contains(consoleCommand) && (flag ? this.toggleOnExecute : this.toggleOffExecute).TryGetValue(text2, out command))
						{
							this.currentlyRunning.Add(consoleCommand);
							this.ProcessCommand(command, true, SystemConsole.CommandEcho.Quiet);
							this.currentlyRunning.Remove(consoleCommand);
						}
					}
				}
			}
			else
			{
				if (!status.inBatch)
				{
					this.Newline();
				}
				this.Log("Unknown command: " + text2, false);
			}
			status.echo = echo2;
			return;
		}
		if (echo != SystemConsole.CommandEcho.Silent)
		{
			echo = SystemConsole.CommandEcho.Quiet;
		}
		if (text2 == "/nick" || text2 == "/msg" || text2 == "/rules")
		{
			this.ExecuteCommand("say_global " + commandToParse, ref status, echo);
			return;
		}
		this.ExecuteCommand("say_game " + commandToParse, ref status, echo);
	}

	// Token: 0x06001D6E RID: 7534 RVA: 0x000CB3E4 File Offset: 0x000C95E4
	public string InsertVariablesOrReduce(string remainder)
	{
		SystemConsole.CommandStatus commandStatus = new SystemConsole.CommandStatus(SystemConsole.CommandEcho.Loud);
		commandStatus.Batch();
		commandStatus.ReadOnly();
		string text = "";
		int i = 0;
		while (i < remainder.Length)
		{
			while (i < remainder.Length && remainder[i] != '{')
			{
				text += remainder[i].ToString();
				i++;
			}
			if (i >= remainder.Length)
			{
				break;
			}
			int num = 0;
			while (i + num < remainder.Length && remainder[i + num] == '{')
			{
				num++;
			}
			if (i + num >= remainder.Length)
			{
				text += 123 * num;
				break;
			}
			int num2 = i + num;
			i = num2;
			while (i < remainder.Length && remainder[i] != '}')
			{
				i++;
			}
			if (i >= remainder.Length)
			{
				text += remainder.Substring(num2);
				break;
			}
			string text2 = remainder.Substring(num2, i - num2);
			if (num == 1)
			{
				SystemConsole.ConsoleCommand consoleCommand;
				if (this.ConsoleCommands.TryGetValue(text2, out consoleCommand) && this.CommandAvailable(consoleCommand) && consoleCommand.type == SystemConsole.CommandType.Variable)
				{
					this.ExecuteCommand(text2 + " -return", ref commandStatus, SystemConsole.CommandEcho.Silent);
					if (consoleCommand.variableType == typeof(float))
					{
						float f = (float)commandStatus.value;
						text += LibString.StringFromFloat(f);
					}
					else if (consoleCommand.variableType == typeof(Vector3))
					{
						Vector3 v = (Vector3)commandStatus.value;
						text += LibString.StringFromVector3(v);
					}
					else
					{
						text += commandStatus.value.ToString();
					}
				}
				else
				{
					text = text + "{" + text2 + "}";
				}
			}
			else
			{
				num--;
				for (int j = 0; j < num; j++)
				{
					text += "{";
				}
				text += text2;
				for (int k = 0; k < num; k++)
				{
					text += "}";
				}
			}
			while (i < remainder.Length && remainder[i] == '}')
			{
				i++;
			}
		}
		return text;
	}

	// Token: 0x06001D6F RID: 7535 RVA: 0x000CB630 File Offset: 0x000C9830
	public static void Concat(ref string s, char c, int count)
	{
		for (int i = 0; i < count; i++)
		{
			s += c.ToString();
		}
	}

	// Token: 0x06001D70 RID: 7536 RVA: 0x000CB65C File Offset: 0x000C985C
	public object GetVariableValue(string command)
	{
		SystemConsole.CommandStatus commandStatus = new SystemConsole.CommandStatus(SystemConsole.CommandEcho.Loud);
		this.ExecuteCommand(command + " -return", ref commandStatus, SystemConsole.CommandEcho.Silent);
		return commandStatus.value;
	}

	// Token: 0x06001D71 RID: 7537 RVA: 0x000CB68A File Offset: 0x000C988A
	public void ProcessBatch(string batch)
	{
		this.ProcessBatch(batch, this.echoMode);
	}

	// Token: 0x06001D72 RID: 7538 RVA: 0x000CB69C File Offset: 0x000C989C
	public void ProcessBatch(string batch, SystemConsole.CommandEcho echo)
	{
		SystemConsole.CommandStatus commandStatus = new SystemConsole.CommandStatus(SystemConsole.CommandEcho.Loud);
		commandStatus.Open();
		base.StartCoroutine(this.ProcessBatch(batch, commandStatus, echo));
	}

	// Token: 0x06001D73 RID: 7539 RVA: 0x000CB6C6 File Offset: 0x000C98C6
	private IEnumerator ProcessBatch(string batch, SystemConsole.CommandStatus status, SystemConsole.CommandEcho echo = SystemConsole.CommandEcho.Loud)
	{
		string line = LibString.bite(ref batch, false, '\n', false, true, '\n');
		bool silenceEntireBatch = false;
		SystemConsole.CommandEcho echoOverride = echo;
		SystemConsole.CommandStatus commandStatus = new SystemConsole.CommandStatus(SystemConsole.CommandEcho.Loud);
		commandStatus.Batch();
		SystemConsole.CommandStatus textStatus = new SystemConsole.CommandStatus(SystemConsole.CommandEcho.Loud);
		textStatus.Batch();
		textStatus.Silent();
		status.skippingToLabel = "";
		while (line != null)
		{
			string text = LibString.lookAhead(line, true, ' ', 0, false, true);
			if (this.StoreTextAliases.Contains(text) && LibString.paramCount(line, 2) == 1)
			{
				string text2 = LibString.lookAhead(line, false, ' ', 1, false, false);
				string b = "end " + text2;
				string text3 = "";
				line = LibString.bite(ref batch, false, '\n', true, true, '\n');
				while (line != null && line != b)
				{
					text3 = text3 + this.InsertVariablesOrReduce(line) + "\n";
					line = LibString.bite(ref batch, false, '\n', true, true, '\n');
				}
				this.ExecuteCommand("store_text " + text2, ref textStatus, SystemConsole.CommandEcho.Silent);
				this.UserStringVars[text2] = text3;
				this.ProcessCommand(text2, true, echoOverride);
			}
			else
			{
				for (string text4 = LibString.bite(ref line, false, ';', false, true, '\0'); text4 != null; text4 = LibString.bite(ref line, false, ';', false, true, '\0'))
				{
					echoOverride = echo;
					if (text4.StartsWith("@"))
					{
						if (text4.StartsWith("@@"))
						{
							silenceEntireBatch = !silenceEntireBatch;
							text4 = text4.Substring(2);
						}
						else
						{
							echoOverride = SystemConsole.CommandEcho.Silent;
							text4 = text4.Substring(1);
						}
						if (text4 == "")
						{
							continue;
						}
					}
					if (silenceEntireBatch)
					{
						echoOverride = SystemConsole.CommandEcho.Silent;
					}
					text = LibString.lookAhead(text4, true, ' ', 0, false, false);
					if (status.skippingToLabel != "")
					{
						if (text == status.skippingToLabel)
						{
							status.skippingToLabel = "";
						}
					}
					else if (text != null && !text4.StartsWith("#") && !text4.StartsWith(":"))
					{
						if (text == "exit")
						{
							LibString.bite(ref text4, false, ' ', false, false, '\0');
							status.value = LibString.bite(ref text4, false, ' ', false, false, '\0');
							if (status.value != null)
							{
								SystemConsole.lastReturnedValue = (string)status.value;
							}
							status.Done();
							line = "";
							batch = "";
						}
						else if (text == "disconnect")
						{
							this.DeferredExecuteCommand(line);
							this.DeferredExecuteCommand(batch);
							status.Done();
							line = "";
							batch = "";
							Network.Disconnect();
						}
						else
						{
							string text5 = "";
							char c = '\0';
							bool flag = false;
							for (int i = 0; i < text4.Length; i++)
							{
								text5 += text4[i].ToString();
								if (flag)
								{
									if (text4[i] == c)
									{
										flag = false;
									}
								}
								else if (text4[i] == '"' || text4[i] == '\'')
								{
									c = text4[i];
									flag = true;
								}
							}
							text4 = text5;
							commandStatus.Done();
							commandStatus.echo = echoOverride;
							this.ExecuteCommand(text4, ref commandStatus, echoOverride);
							float timeoutAt = Time.time + 15f;
							while (commandStatus.isOpen && Time.time < timeoutAt)
							{
								yield return new WaitForSeconds(0.5f);
							}
							if (Time.time >= timeoutAt)
							{
								Chat.LogError("Timed out waiting for batch to complete.", true);
								batch = "";
							}
							if (commandStatus.skippingToLabel != "")
							{
								status.skippingToLabel = commandStatus.skippingToLabel;
								commandStatus.skippingToLabel = "";
							}
						}
					}
				}
			}
			line = LibString.bite(ref batch, false, '\n', false, true, '\n');
		}
		yield break;
	}

	// Token: 0x06001D74 RID: 7540 RVA: 0x000CB6EC File Offset: 0x000C98EC
	public void DeferredExecuteCommand(string command)
	{
		if (command == "")
		{
			return;
		}
		string text = PlayerPrefs.GetString("SystemConsoleAutoexecOnce", "");
		if (text != "")
		{
			text += "\n";
		}
		text += command;
		PlayerPrefs.SetString("SystemConsoleAutoexecOnce", text);
		Debug.Log("Deferring: " + PlayerPrefs.GetString("SystemConsoleAutoexecOnce"));
		PlayerPrefs.Save();
	}

	// Token: 0x06001D75 RID: 7541 RVA: 0x000CB761 File Offset: 0x000C9961
	private IEnumerator HostOrConnect(SystemConsole.CommandStatus status, bool hosting, int seats, bool hotseat, string hostname, string password)
	{
		float timeoutAt = Time.time + 15f;
		if (Time.time < timeoutAt)
		{
			if (!hosting)
			{
				NetworkSingleton<NetworkUI>.Instance.GUIStartSingleplayer();
			}
			else if (seats == 1)
			{
				NetworkSingleton<NetworkUI>.Instance.GUIStartSingleplayer();
			}
			else if (hotseat)
			{
				NetworkSingleton<NetworkUI>.Instance.GUIStartHotseat(seats.ToString());
			}
			else
			{
				NetworkSingleton<NetworkUI>.Instance.GUIHostServer(hostname, password, seats.ToString());
			}
			timeoutAt = Time.time + 15f;
			while ((Network.peerType == NetworkPeerMode.Disconnected || Network.peerType == NetworkPeerMode.Connecting) && Time.time < timeoutAt)
			{
				yield return new WaitForSeconds(0.5f);
			}
			if (Time.time >= timeoutAt)
			{
				if (hosting)
				{
					Chat.LogError("Timed out waiting to host.", true);
				}
				else
				{
					Chat.LogError("Timed out waiting to connect.", true);
				}
			}
		}
		status.Done();
		yield break;
	}

	// Token: 0x06001D76 RID: 7542 RVA: 0x000CB798 File Offset: 0x000C9998
	public void CheckCommandsAwaitingOnLoad()
	{
		for (int i = this.CommandsAwaitingOnLoad.Count - 1; i >= 0; i--)
		{
			this.CommandsAwaitingOnLoad[i].Done();
			this.CommandsAwaitingOnLoad.RemoveAt(i);
		}
	}

	// Token: 0x06001D77 RID: 7543 RVA: 0x000CB7DA File Offset: 0x000C99DA
	private IEnumerator WaitUntilDisconnected(SystemConsole.CommandStatus status)
	{
		float timeoutAt = Time.time + 15f;
		while (Network.peerType != NetworkPeerMode.Disconnected && Time.time < timeoutAt)
		{
			yield return new WaitForSeconds(0.5f);
		}
		if (Time.time >= timeoutAt)
		{
			Chat.LogError("Timed out waiting to disconnect", true);
		}
		status.Done();
		yield break;
	}

	// Token: 0x06001D78 RID: 7544 RVA: 0x000CB7E9 File Offset: 0x000C99E9
	public IEnumerator DelayedDone(float delay, SystemConsole.CommandStatus status)
	{
		yield return new WaitForSeconds(delay);
		status.Done();
		yield break;
	}

	// Token: 0x06001D79 RID: 7545 RVA: 0x000CB7FF File Offset: 0x000C99FF
	public IEnumerator DelayedClick(float delay, GameObject gameObject)
	{
		yield return new WaitForSeconds(delay);
		gameObject.GetComponent<UIButton>().SendMessage("OnClick");
		yield break;
	}

	// Token: 0x06001D7A RID: 7546 RVA: 0x000CB815 File Offset: 0x000C9A15
	private void Newline()
	{
		this.Log("", false);
	}

	// Token: 0x06001D7B RID: 7547 RVA: 0x000CB824 File Offset: 0x000C9A24
	public void Log(string msg, Color color, bool truncate = false)
	{
		if (truncate)
		{
			UILabel component = NetworkSingleton<Chat>.Instance.ChatOutputText.GetComponent<UILabel>();
			NGUIText.regionWidth = component.width;
			NGUIText.regionHeight = (int)((float)component.fontSize * 1.5f);
			NGUIText.bitmapFont = component.bitmapFont;
			NGUIText.fontSize = component.fontSize;
			NGUIText.fontScale = 1f;
			if (NGUIText.bitmapFont.isDynamic)
			{
				NGUIText.dynamicFont = NGUIText.bitmapFont.dynamicFont;
				NGUIText.bitmapFont = null;
			}
			else
			{
				NGUIText.dynamicFont = null;
			}
			NGUIText.Update(false);
			string text;
			if (!NGUIText.WrapText(msg, out text, false))
			{
				string text2 = "";
				int num = 0;
				for (bool flag = true; flag && num < msg.Length; flag = NGUIText.WrapText(msg.Substring(0, num), out text, false))
				{
					num++;
					text2 = text;
				}
				msg = text2.Substring(0, text2.Length - 4) + "...";
			}
		}
		Chat.Log(msg, color, ChatMessageType.System, false);
	}

	// Token: 0x06001D7C RID: 7548 RVA: 0x000CB917 File Offset: 0x000C9B17
	public void Log(string msg, bool truncate = false)
	{
		this.Log(msg, SystemConsole.OutputColour, truncate);
	}

	// Token: 0x06001D7D RID: 7549 RVA: 0x000CB92C File Offset: 0x000C9B2C
	public static bool intFromArg(string arg, out int i, string label)
	{
		if (arg == null)
		{
			Singleton<SystemConsole>.Instance.Log("Missing " + label, false);
			i = 0;
			return false;
		}
		if (!int.TryParse(arg, out i))
		{
			Singleton<SystemConsole>.Instance.Log("Bad " + label, false);
			return false;
		}
		return true;
	}

	// Token: 0x06001D7E RID: 7550 RVA: 0x000CB97C File Offset: 0x000C9B7C
	public static string biteToken(ref string remainder, out SystemConsole.EvalTokenType tokenType, out string component)
	{
		tokenType = SystemConsole.EvalTokenType.None;
		component = "";
		if (remainder == "")
		{
			return "";
		}
		string result;
		if ("abcdefghijkjlmnopqrstuvwxyz_".Contains(remainder[0].ToString()))
		{
			tokenType = SystemConsole.EvalTokenType.Token;
			int num = 1;
			int num2 = -1;
			while (num < remainder.Length && "abcdefghijkjlmnopqrstuvwxyz_1234567890.".Contains(remainder[num].ToString()))
			{
				if (remainder[num] == '.')
				{
					if (num2 != -1)
					{
						tokenType = SystemConsole.EvalTokenType.Error;
					}
					else
					{
						num2 = num;
					}
				}
				num++;
			}
			if (num2 != -1)
			{
				result = remainder.Substring(0, num2);
				component = remainder.Substring(num2 + 1, num - num2 - 1);
				if (component != "x" && component != "y" && component != "z")
				{
					tokenType = SystemConsole.EvalTokenType.Error;
				}
				else
				{
					tokenType = SystemConsole.EvalTokenType.TokenWithComponent;
				}
			}
			else
			{
				result = remainder.Substring(0, num);
			}
			remainder = remainder.Substring(num);
		}
		else
		{
			tokenType = SystemConsole.EvalTokenType.Arithmetic;
			int num3 = 1;
			while (num3 < remainder.Length && !"abcdefghijkjlmnopqrstuvwxyz_".Contains(remainder[num3].ToString()))
			{
				num3++;
			}
			result = remainder.Substring(0, num3);
			remainder = remainder.Substring(num3);
		}
		return result;
	}

	// Token: 0x06001D7F RID: 7551 RVA: 0x000CBACC File Offset: 0x000C9CCC
	public static bool IsValidCommandCharacter(char s)
	{
		return (s >= '0' && s <= '9') || (s >= 'a' && s <= 'z') || s == '_';
	}

	// Token: 0x06001D80 RID: 7552 RVA: 0x000CBAEC File Offset: 0x000C9CEC
	public static string ConvertEscapesToCharacters(string s)
	{
		string text = "";
		for (int i = 0; i < s.Length; i++)
		{
			if (s[i] == '\\')
			{
				i++;
				char c = s[i];
				if (c != '\\')
				{
					if (c != 'n')
					{
						if (c != 't')
						{
							text += s[i - 1].ToString();
							text += s[i].ToString();
						}
						else
						{
							text += "\t";
						}
					}
					else
					{
						text += "\n";
					}
				}
				else
				{
					text += "\\";
				}
			}
			else
			{
				text += s[i].ToString();
			}
		}
		return text;
	}

	// Token: 0x06001D81 RID: 7553 RVA: 0x000CBBB0 File Offset: 0x000C9DB0
	public void UpdateFog()
	{
		bool flag = SystemConsole.Fog || (TableScript.PURE_MODE && TableScript.PURE_FOG);
		this.DisableForFog.enabled = !flag;
		this.EnableForFog.SetActive(flag);
	}

	// Token: 0x06001D82 RID: 7554 RVA: 0x000CBBF4 File Offset: 0x000C9DF4
	private void DisplayConsoleTab(bool visible = true)
	{
		ChatTab[] chatTabs = NetworkSingleton<Chat>.Instance.chatTabs;
		for (int i = 0; i < chatTabs.Length; i++)
		{
			if (chatTabs[i].chatType == ChatMessageType.System)
			{
				chatTabs[i].gameObject.SetActive(visible);
				this.SystemTab = chatTabs[i];
			}
			else if (chatTabs[i].chatType != ChatMessageType.All)
			{
				chatTabs[i].GetComponent<UIWidget>().leftAnchor.absolute += 34;
				chatTabs[i].GetComponent<UIWidget>().rightAnchor.absolute += 34;
			}
		}
		NetworkSingleton<Chat>.Instance.ChatResizer.minWidth += 34;
	}

	// Token: 0x06001D83 RID: 7555 RVA: 0x000CBC98 File Offset: 0x000C9E98
	public bool LogTagShouldDisplay(string tagName)
	{
		bool flag = false;
		if (this.LogTagsIncluded.Count == 0)
		{
			flag = true;
		}
		else
		{
			for (int i = 0; i < this.LogTagsIncluded.Count; i++)
			{
				if (this.LogTagsIncluded[i] == tagName)
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			return false;
		}
		if (this.LogTagsExcluded.Count > 0)
		{
			for (int j = 0; j < this.LogTagsExcluded.Count; j++)
			{
				if (this.LogTagsExcluded[j] == tagName)
				{
					flag = false;
					break;
				}
			}
		}
		return flag;
	}

	// Token: 0x06001D84 RID: 7556 RVA: 0x000CBD28 File Offset: 0x000C9F28
	public bool LogTagShouldHighlight(string tagName)
	{
		for (int i = 0; i < this.LogTagsHighlight.Count; i++)
		{
			if (this.LogTagsHighlight[i] == tagName)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001D85 RID: 7557 RVA: 0x000CBD64 File Offset: 0x000C9F64
	public void SetValuesFromLogTag(string tagName, ref Colour colour, ref string prefix, ref string postfix)
	{
		SystemConsole.LogTag logTag;
		if (this.LogTags.TryGetValue(tagName, out logTag))
		{
			colour = logTag.colour;
			prefix = logTag.prefix;
			postfix = logTag.postfix;
		}
	}

	// Token: 0x06001D86 RID: 7558 RVA: 0x000CBD9E File Offset: 0x000C9F9E
	public void SetValuesFromHighlight(ref Colour colour, ref string prefix, ref string postfix)
	{
		colour = this.HighlightTag.colour;
		prefix = this.HighlightTag.prefix;
		postfix = this.HighlightTag.postfix;
	}

	// Token: 0x06001D87 RID: 7559 RVA: 0x000CBDCC File Offset: 0x000C9FCC
	public void RecheckTouchpadBindings()
	{
		foreach (VRTrackedController.VRKeyCode vrkeyCode in this.VRKeyLongPressBinds.Keys)
		{
			VRTrackedController.ProcessTouchpadBinding(vrkeyCode, this.VRKeyLongPressBinds[vrkeyCode]);
		}
		foreach (VRTrackedController.VRKeyCode vrkeyCode2 in this.VRKeyReleaseBinds.Keys)
		{
			VRTrackedController.ProcessTouchpadBinding(vrkeyCode2, this.VRKeyReleaseBinds[vrkeyCode2]);
		}
		foreach (VRTrackedController.VRKeyCode vrkeyCode3 in this.VRKeyPressBinds.Keys)
		{
			VRTrackedController.ProcessTouchpadBinding(vrkeyCode3, this.VRKeyPressBinds[vrkeyCode3]);
		}
	}

	// Token: 0x06001D88 RID: 7560 RVA: 0x000CBED4 File Offset: 0x000CA0D4
	private void makePath(StringBuilder builder, Transform t)
	{
		builder.Append(t.name);
		if (t.parent != null)
		{
			builder.Append(" <- ");
			this.makePath(builder, t.parent);
		}
	}

	// Token: 0x06001D89 RID: 7561 RVA: 0x000CBF0C File Offset: 0x000CA10C
	public string GetPath(Transform t)
	{
		StringBuilder stringBuilder = new StringBuilder();
		this.makePath(stringBuilder, t);
		return stringBuilder.ToString();
	}

	// Token: 0x06001D8A RID: 7562 RVA: 0x000CBF30 File Offset: 0x000CA130
	public static void UserDebug(DebugType type, string msg)
	{
		string str = "";
		bool flag = false;
		if (type == DebugType.External_API)
		{
			flag = SystemConsole.DEBUG_EXTERNAL_API;
			str = "EXT: ";
		}
		Debug.Log(str + msg);
		if (flag)
		{
			SystemConsole.DebugMessageTail++;
			SystemConsole.DebugMessageTail %= 100;
			SystemConsole.DebugMessages[SystemConsole.DebugMessageTail] = str + msg;
		}
	}

	// Token: 0x06001D8B RID: 7563 RVA: 0x000CBF90 File Offset: 0x000CA190
	public bool GetToggleValue(string toggle)
	{
		SystemConsole.CommandStatus commandStatus = new SystemConsole.CommandStatus(SystemConsole.CommandEcho.Loud);
		this.ExecuteCommand(toggle + " -return", ref commandStatus, SystemConsole.CommandEcho.Silent);
		return (bool)commandStatus.value;
	}

	// Token: 0x06001D8C RID: 7564 RVA: 0x000CBFC4 File Offset: 0x000CA1C4
	public static bool TryGetColour(string arg, out Colour colour, out string message)
	{
		message = null;
		colour = Colour.White;
		if (arg == null)
		{
			message = "Specify color";
			return false;
		}
		if (arg.Substring(0, 1) == "#")
		{
			try
			{
				colour = Colour.ColourFromRGBHex(arg);
				return true;
			}
			catch
			{
				message = "You must specify RGB colour as #RRGGBB";
				return false;
			}
		}
		arg = arg.Substring(0, 1).ToUpper() + arg.Substring(1).ToLower();
		if (Colour.TryColourFromLabel(arg, out colour))
		{
			return true;
		}
		message = "Invalid color";
		return false;
	}

	// Token: 0x06001D8D RID: 7565 RVA: 0x000CC060 File Offset: 0x000CA260
	public bool AddLuaConsoleCommand(UserDefinedConsoleCommandManager.LuaConsoleCommandIdentifier luaConsoleCommand)
	{
		string text = luaConsoleCommand.label.ToLower();
		SystemConsole.ConsoleCommand consoleCommand;
		if (this.ConsoleCommands.TryGetValue(text, out consoleCommand))
		{
			return false;
		}
		SystemConsole.ConsoleCommand consoleCommand2 = new SystemConsole.ConsoleCommand(text, SystemConsole.CommandType.Command, luaConsoleCommand.adminOnly ? SystemConsole.CommandPermission.Admin : SystemConsole.CommandPermission.Player, "", "", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			List<string> list = new List<string>();
			for (string item = LibString.bite(ref parameters, false, ' ', false, false, '\0'); item != null; item = LibString.bite(ref parameters, false, ' ', false, false, '\0'))
			{
				list.Add(item);
			}
			NetworkSingleton<UserDefinedConsoleCommandManager>.Instance.DoConsoleCommand(NetworkID.ID, luaConsoleCommand.index, list);
		}, "", null, SystemConsole.StoreAs.DoNotStore, SystemConsole.CommandOrigin.Internal, null);
		consoleCommand2.origin = SystemConsole.CommandOrigin.Lua;
		this.ConsoleCommands[text] = consoleCommand2;
		this.ConsoleCommandList.Add(text);
		this.ConsoleCommandList.Sort();
		return true;
	}

	// Token: 0x06001D8E RID: 7566 RVA: 0x000CC104 File Offset: 0x000CA304
	public void RemoveLuaConsoleCommand(string commandName)
	{
		SystemConsole.ConsoleCommand consoleCommand;
		if (this.ConsoleCommands.TryGetValue(commandName, out consoleCommand) && consoleCommand.origin == SystemConsole.CommandOrigin.Lua)
		{
			this.RemoveCommand(commandName);
		}
	}

	// Token: 0x06001D8F RID: 7567 RVA: 0x000CC134 File Offset: 0x000CA334
	private void CreateCommandsAM()
	{
		this.ConsoleCommands.Add("action_cut", this.NewPlayerCommand("action_cut", "Cut specified component at specified point.", "USAGE: $CMD$ {-p <point>|-c <count>} {<guid>...}\nCut deck/stack in half. Cut point defaults to splitting in half, or use one of:\n-p = between 0.0 and 1.0, a ratio over container size.\n-c = count of items.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float num = 0.5f;
			bool flag = true;
			string a = LibString.lookAhead(parameters, false, ' ', 0, false, false);
			if (a == "-p" || a == "-c")
			{
				LibString.bite(ref parameters, false, ' ', false, false, '\0');
				if (!float.TryParse(LibString.bite(ref parameters, false, ' ', false, false, '\0'), out num))
				{
					this.Log("Invalid cutting point.", false);
					return;
				}
				flag = (a == "-p");
				if (a == "-p")
				{
					if (num < 0f || num > 1f)
					{
						this.Log("Invalid cutting point.", false);
						return;
					}
				}
				else
				{
					flag = false;
				}
			}
			List<NetworkPhysicsObject> list = this.ComponentHelper(status, parameters);
			for (int k = 0; k < list.Count; k++)
			{
				NetworkPhysicsObject networkPhysicsObject = list[k];
				if (networkPhysicsObject.CompareTag("Deck"))
				{
					int count = networkPhysicsObject.deckScript.GetDeck().Count;
					int num2;
					if (flag)
					{
						num2 = (int)((1f - num) * (float)count + 0.5f);
					}
					else
					{
						num2 = (int)num;
					}
					num2 = Mathf.Max(2, Mathf.Min(count - 2, num2));
					NetworkSingleton<ManagerPhysicsObject>.Instance.CutDeck(networkPhysicsObject.ID, count - num2);
				}
				else if (networkPhysicsObject.GetComponent<StackObject>() && !networkPhysicsObject.GetComponent<StackObject>().bBag && !networkPhysicsObject.GetComponent<StackObject>().IsInfiniteStack)
				{
					int num_objects_ = networkPhysicsObject.GetComponent<StackObject>().num_objects_;
					int num2;
					if (flag)
					{
						num2 = (int)((1f - num) * (float)num_objects_ + 0.5f);
					}
					else
					{
						num2 = (int)num;
					}
					num2 = Mathf.Max(2, Mathf.Min(num_objects_ - 2, num2));
					NetworkSingleton<ManagerPhysicsObject>.Instance.CutStack(networkPhysicsObject.ID, num_objects_ - num2);
				}
			}
		}, ""));
		this.ConsoleCommands.Add("action_deal", this.NewPlayerCommand("action_deal", "Deal from specified component.", "USAGE: $CMD$ {-c <count>} <guid> {<player>...}\nDeal <count> cards (default 1) from component specified by <guid> to each <player>. If no <player> provided then deal to each seated player.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			float num = 1f;
			if (text3 == "-c")
			{
				text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
				if (text3 == null || !float.TryParse(text3, out num) || (int)num < 1)
				{
					this.Log("Invalid count.", false);
					return;
				}
				text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			}
			NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGUID(text3);
			if (networkPhysicsObject == null)
			{
				this.Log("Invalid Component: " + text3, false);
				return;
			}
			List<string> list = new List<string>();
			for (text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0'); text3 != null; text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0'))
			{
				string text4 = LibString.CamelCaseFromUnderscore(text3.ToLower(), true, false);
				if (!Colour.IsColourLabel(text4) && text4 != "All" && text4 != "Seated")
				{
					this.Log("Invalid player colour: " + text3, false);
					return;
				}
				list.Add(text4);
			}
			if (list.Count == 0)
			{
				list.Add("Seated");
			}
			foreach (string colorLabel in list)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.DealObjectToColor(networkPhysicsObject.ID, colorLabel, (int)num, 0);
			}
		}, ""));
		this.ConsoleCommands.Add("action_draw", this.NewPlayerCommand("action_draw", "Draw from specified component.", "USAGE: $CMD$ {-c <count>} {<guid>...}\nDraw from component specified by <guid>. If no <guid> provided then the mouse/selection will be used.\n -c = specify number of cards to draw", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float num = 1f;
			if (LibString.lookAhead(parameters, false, ' ', 0, false, false) == "-c")
			{
				LibString.bite(ref parameters, false, ' ', false, false, '\0');
				if (!float.TryParse(LibString.bite(ref parameters, false, ' ', false, false, '\0'), out num))
				{
					this.Log("Invalid count.", false);
					return;
				}
				if ((int)num <= 0)
				{
					this.Log("Invalid count.", false);
					return;
				}
			}
			foreach (NetworkPhysicsObject networkPhysicsObject in this.ComponentHelper(status, parameters))
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.DealObjectToColor(networkPhysicsObject.ID, Colour.MyColorLabel(), (int)num, 0);
			}
		}, ""));
		this.ConsoleCommands.Add("action_flip", this.NewPlayerCommand("action_flip", "Flip specified component.", "USAGE: $CMD$ {<guid>...}\nFlip component specified by <guid>. If no <guid> provided then the mouse/selection will be used.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool flag;
			List<NetworkPhysicsObject> list = this.ComponentHelper(status, parameters, out flag);
			if (list.Count > 0)
			{
				if (flag)
				{
					PlayerScript.PointerScript.ChangeHeldFlipRotationIndex(12, -1);
					return;
				}
				foreach (NetworkPhysicsObject networkPhysicsObject in list)
				{
					PlayerScript.PointerScript.Rotation(networkPhysicsObject.ID, 0, 12, false);
				}
			}
		}, ""));
		this.ConsoleCommands.Add("action_group", this.NewPlayerCommand("action_group", "Group specified components.", "USAGE: $CMD$ {<guid>...}\nGroup components specified by <guid>. If no <guid> provided then the mouse/selection will be used.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			List<NetworkPhysicsObject> list = this.ComponentHelper(status, parameters);
			if (list.Count > 0)
			{
				PlayerScript.PointerScript.Group(list);
			}
		}, ""));
		this.ConsoleCommands.Add("action_layout", this.NewPlayerCommand("action_layout", "Layout specified component.", "USAGE: $CMD$ {<guid>...}\nLayout component specified by <guid> if its in (or is) a valid Layout Zone. If no <guid> provided then the mouse/selection will be used.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			List<NetworkPhysicsObject> list = this.ComponentHelper(status, parameters);
			if (list.Count > 0)
			{
				List<LayoutZone> list2 = new List<LayoutZone>();
				for (int k = 0; k < list.Count; k++)
				{
					NetworkPhysicsObject networkPhysicsObject = list[k];
					LayoutZone layoutZone = networkPhysicsObject.layoutZone;
					int num;
					if (layoutZone)
					{
						list2.TryAddUnique(layoutZone);
					}
					else if (LayoutZone.TryNPOInLayoutZone(networkPhysicsObject, out layoutZone, out num, LayoutZone.PotentialZoneCheck.Both))
					{
						list2.TryAddUnique(layoutZone);
					}
				}
				for (int l = 0; l < list2.Count; l++)
				{
					list2[l].ManualLayoutZone();
				}
			}
		}, ""));
		this.ConsoleCommands.Add("action_lock", this.NewPlayerCommand("action_lock", "Lock specified component.", "USAGE: $CMD$ {<guid>...}\nLock/unlock component specified by <guid>. If no <guid> provided then the mouse/selection will be used.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			List<NetworkPhysicsObject> list = this.ComponentHelper(status, parameters);
			if (list.Count > 0)
			{
				bool flag = false;
				using (List<NetworkPhysicsObject>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.IsLocked)
						{
							flag = true;
							break;
						}
					}
				}
				PlayerScript.PointerScript.Lock(list, !flag);
			}
		}, ""));
		this.ConsoleCommands.Add("action_popout", this.NewPlayerCommand("action_popout", "Pop-out to screen specified component.", "USAGE: $CMD$ {<guid>...}\nPop-out to screen component specified by <guid>. If no <guid> provided then the mouse/selection will be used.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			foreach (NetworkPhysicsObject networkPhysicsObject in this.ComponentHelper(status, parameters))
			{
				CustomPDF component = networkPhysicsObject.GetComponent<CustomPDF>();
				if (component)
				{
					component.PopoutToScreen();
					break;
				}
			}
		}, ""));
		this.ConsoleCommands.Add("action_randomize", this.NewPlayerCommand("action_randomize", "Randomize (shuffle/roll/etc.) specified component.", "USAGE: $CMD$ {<guid>...}\nRandomize component specified by <guid>. If no <guid> provided then the mouse/selection will be used.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			foreach (NetworkPhysicsObject networkPhysicsObject in this.ComponentHelper(status, parameters))
			{
				PlayerScript.PointerScript.Randomize(networkPhysicsObject.ID, PlayerScript.PointerScript.ID);
			}
		}, ""));
		this.ConsoleCommands.Add("action_rotate", this.NewPlayerCommand("action_rotate", "Rotate specified component.", "USAGE: $CMD$ > {-z} <angle> {<guid>...}\nRotate component specified by <guid>. <angle> is a multiple of 15.\nIf no <guid> provided then the mouse/selection will be used.\n -z = rotate around z axis instead.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float num = 0f;
			bool flag = false;
			string text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text3 == "-z")
			{
				flag = true;
				text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			}
			if (text3 == null || !float.TryParse(text3, out num))
			{
				this.Log("Invalid angle.", false);
				return;
			}
			if (num > 0f)
			{
				num = Mathf.Ceil(num / 15f);
			}
			else
			{
				num = -num;
				num = Mathf.Ceil(num / 15f);
				num = -num;
			}
			int num2 = (int)num;
			bool flag2;
			List<NetworkPhysicsObject> list = this.ComponentHelper(status, parameters, out flag2);
			if (flag2)
			{
				if (flag)
				{
					PlayerScript.PointerScript.ChangeHeldFlipRotationIndex((int)num, -1);
				}
				else
				{
					PlayerScript.PointerScript.ChangeHeldSpinRotationIndex((int)num, -1);
				}
			}
			foreach (NetworkPhysicsObject networkPhysicsObject in list)
			{
				if (networkPhysicsObject.IsHeldByNobody)
				{
					PlayerScript.PointerScript.Rotation(networkPhysicsObject.ID, flag ? 0 : num2, flag ? num2 : 0, false);
				}
			}
		}, ""));
		this.ConsoleCommands.Add("action_search", this.NewPlayerCommand("action_search", "Search specified component.", "USAGE: $CMD$ {-s <search text>} {-c <count>} {<guid>}\nSearch component specified by <guid>. If no <guid> provided then the mouse/selection will be used.\n -s <text> = specific search text\n -c <count> = max cards when searching deck", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text3 = "";
			int maxCards = -1;
			bool flag = true;
			while (flag)
			{
				flag = false;
				if (LibString.lookAhead(parameters, false, ' ', 0, false, false) == "-s")
				{
					LibString.bite(ref parameters, false, ' ', false, false, '\0');
					text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
					if (text3 == null)
					{
						this.Log("Invalid search text.", false);
						return;
					}
					flag = true;
				}
				else if (LibString.lookAhead(parameters, false, ' ', 0, false, false) == "-c")
				{
					LibString.bite(ref parameters, false, ' ', false, false, '\0');
					if (!int.TryParse(LibString.bite(ref parameters, false, ' ', false, false, '\0'), out maxCards))
					{
						this.Log("Invalid count.", false);
						return;
					}
					flag = true;
				}
			}
			foreach (NetworkPhysicsObject networkPhysicsObject in this.ComponentHelper(status, parameters))
			{
				if ((networkPhysicsObject.GetComponent<StackObject>() && networkPhysicsObject.GetComponent<StackObject>().bBag) || networkPhysicsObject.CompareTag("Deck"))
				{
					UIInventory.NextSearch = text3;
					NetworkSingleton<ManagerPhysicsObject>.Instance.SearchInventory(networkPhysicsObject.ID, NetworkID.ID, maxCards);
					PlayerScript.PointerScript.ResetHighlight();
					break;
				}
				if (networkPhysicsObject.customPDF)
				{
					if (text3 != "")
					{
						networkPhysicsObject.customPDF.Search(text3);
					}
					else
					{
						networkPhysicsObject.customPDF.Search();
					}
				}
			}
		}, ""));
		this.ConsoleCommands.Add("action_split", this.NewPlayerCommand("action_split", "Split specified component into piles.", "USAGE: $CMD$ <piles> {<guid>...}\nSplit container specified by <guid> into <piles>.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float num;
			if (!float.TryParse(LibString.bite(ref parameters, false, ' ', false, false, '\0'), out num))
			{
				this.Log("Invalid number of piles.", false);
				return;
			}
			if (num <= 1f)
			{
				this.Log("Invalid number of piles.", false);
				return;
			}
			foreach (NetworkPhysicsObject networkPhysicsObject in this.ComponentHelper(status, parameters))
			{
				if (networkPhysicsObject.CompareTag("Deck"))
				{
					NetworkSingleton<ManagerPhysicsObject>.Instance.SplitDeck(networkPhysicsObject.ID, (int)num);
				}
				else if (networkPhysicsObject.GetComponent<StackObject>() && !networkPhysicsObject.GetComponent<StackObject>().bBag && !networkPhysicsObject.GetComponent<StackObject>().IsInfiniteStack)
				{
					NetworkSingleton<ManagerPhysicsObject>.Instance.SplitStack(networkPhysicsObject.ID, (int)num);
				}
			}
		}, ""));
		this.ConsoleCommands.Add("action_spread", this.NewPlayerCommand("action_spread", "Spread specified deck component face-up across table.", "USAGE: $CMD$ {-d <distance>} {<guid>}\nSplit container specified by <guid> into <piles>.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float spread_ACTION_DISTANCE = Pointer.SPREAD_ACTION_DISTANCE;
			if (LibString.lookAhead(parameters, false, ' ', 0, false, false) == "-d")
			{
				LibString.bite(ref parameters, false, ' ', false, false, '\0');
				if (!float.TryParse(LibString.bite(ref parameters, false, ' ', false, false, '\0'), out spread_ACTION_DISTANCE))
				{
					this.Log("Invalid distance.", false);
					return;
				}
				if (spread_ACTION_DISTANCE <= 0f)
				{
					this.Log("Invalid distance.", false);
					return;
				}
			}
			foreach (NetworkPhysicsObject networkPhysicsObject in this.ComponentHelper(status, parameters))
			{
				if (networkPhysicsObject.CompareTag("Deck"))
				{
					NetworkSingleton<ManagerPhysicsObject>.Instance.SpreadDeck(networkPhysicsObject.ID, spread_ACTION_DISTANCE, 0f, 0);
				}
			}
		}, ""));
		this.ConsoleCommands.Add("action_state_next", this.NewPlayerCommand("action_state_next", "Increment state of specified component.", "USAGE: $CMD$ {<guid>}\nIncrement state of component specified by <guid>. If no <guid> provided then the mouse/selection will be used.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			foreach (NetworkPhysicsObject networkPhysicsObject in this.ComponentHelper(status, parameters))
			{
				if (networkPhysicsObject.HasStates())
				{
					networkPhysicsObject.NextState();
				}
				else if (networkPhysicsObject.customPDF != null)
				{
					networkPhysicsObject.customPDF.NextPage();
				}
			}
		}, ""));
		this.ConsoleCommands.Add("action_state_prev", this.NewPlayerCommand("action_state_prev", "Decrement state of specified component.", "USAGE: $CMD$ {<guid>}\nDecrement state of component specified by <guid>. If no <guid> provided then the mouse/selection will be used.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			foreach (NetworkPhysicsObject networkPhysicsObject in this.ComponentHelper(status, parameters))
			{
				if (networkPhysicsObject.HasStates())
				{
					networkPhysicsObject.PrevState();
				}
				else if (networkPhysicsObject.customPDF != null)
				{
					networkPhysicsObject.customPDF.PrevPage();
				}
			}
		}, ""));
		this.ConsoleCommands.Add("add", this.NewPlayerCommand("add", "Add a value to a numerical variable.", "USAGE: $CMD$ <variable> <value> {<modulus>}\nSets <variable> to its current value + <value>, modulo the optional parameter if present.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text3 == null)
			{
				this.Log("You must provide a numeric <variable>.", false);
				return;
			}
			SystemConsole.ConsoleCommand consoleCommand;
			this.ConsoleCommands.TryGetValue(text3, out consoleCommand);
			if (consoleCommand == null || consoleCommand.type != SystemConsole.CommandType.Variable || (consoleCommand.variableType != typeof(float) && consoleCommand.variableType != typeof(int)))
			{
				this.Log("You must provide a numeric <variable>.", false);
				return;
			}
			float num;
			if (!float.TryParse(LibString.bite(ref parameters, false, ' ', false, false, '\0'), out num))
			{
				this.Log("Could not parse numeric <value>", false);
				return;
			}
			string text4 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			int num2 = 0;
			if (text4 != null && !int.TryParse(text4, out num2))
			{
				this.Log("Could not parse integer <modulo>", false);
				return;
			}
			SystemConsole.CommandStatus commandStatus = new SystemConsole.CommandStatus(SystemConsole.CommandEcho.Loud);
			commandStatus.Batch();
			this.ExecuteCommand(text3 + " -return", ref commandStatus, SystemConsole.CommandEcho.Silent);
			float num3;
			if (commandStatus.value.GetType() == typeof(int))
			{
				num3 = (float)((int)commandStatus.value);
			}
			else
			{
				num3 = (float)commandStatus.value;
			}
			num3 += num;
			if (text4 != null)
			{
				num3 %= (float)num2;
				if (num3 < 0f)
				{
					num3 += (float)num2;
				}
			}
			status.Batch();
			this.ExecuteCommand(text3 + " " + num3, ref status, SystemConsole.CommandEcho.Quiet);
		}, ""));
		this.ConsoleCommands.Add("alias", this.NewPlayerCommand("alias", "Creates an alias for another command using specified parameters as defaults.", "USAGE: $CMD$ <label> <command> {<parameters} OR $CMD$ <label> -d OR $CMD$ <new_prefix>* <old_prefix>*\nCreates an alternate way to call <command>.  If <parameters> are provided they will automatically be applied when using that alias.\nUse * create an alias for all commands with specified prefix.\n-d = deletes an already existing alias.\n\nIf <label> is a toggle variable you may prefix with + or - to attach <command> to it; when it changes to that value the command will execute.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			string text4 = LibString.bite(ref parameters, true, ' ', false, false, '\0');
			parameters = parameters.Trim();
			if (text4 == null || text4 == "-d")
			{
				if (text3 == null)
				{
					this.Log("You must provide a <label> and <command>.", false);
					return;
				}
				SystemConsole.ConsoleCommand consoleCommand = null;
				if (text3[0] == '+' || text3[0] == '-')
				{
					Dictionary<string, string> dictionary = (text3[0] == '+') ? this.toggleOnExecute : this.toggleOffExecute;
					string str;
					if (!dictionary.TryGetValue(text3.Substring(1), out str))
					{
						this.Log("No such attachment: " + text3, false);
						return;
					}
					if (text4 == "-d")
					{
						dictionary.Remove(text3.Substring(1));
						if (!status.isSilent)
						{
							this.Log(text3 + " deleted.", false);
							return;
						}
					}
					else if (!status.isSilent)
					{
						this.Log(text3 + " -> " + str, status.inBatch);
						return;
					}
				}
				else
				{
					this.ConsoleCommands.TryGetValue(text3, out consoleCommand);
					if (!this.CommandAvailable(consoleCommand))
					{
						this.Log("Unknown command: " + text4, false);
						return;
					}
					if (consoleCommand.aliasOf == "")
					{
						this.Log(text3 + " is an in-built command, not an alias.", false);
						return;
					}
					if (text4 == "-d")
					{
						this.ConsoleCommandList.Remove(text3);
						this.ConsoleCommands.Remove(text3);
						this.StoreTextAliases.Remove(text3);
						if (!status.isSilent)
						{
							this.Log(text3 + " deleted.", false);
							return;
						}
					}
					else if (!status.isSilent)
					{
						this.Log(consoleCommand.aliasOf + consoleCommand.defaults, status.inBatch);
						return;
					}
				}
			}
			else if (text3.EndsWith("*"))
			{
				if (!text4.EndsWith("*"))
				{
					this.Log("Both new and old prefixes must end with *", false);
					return;
				}
				text3 = text3.Substring(0, text3.Length - 1);
				text4 = text4.Substring(0, text4.Length - 1);
				List<string> list = new List<string>();
				for (int k = 0; k < this.ConsoleCommandList.Count; k++)
				{
					string text5 = this.ConsoleCommandList[k];
					if (text5.StartsWith(text4))
					{
						list.Add(text5);
					}
				}
				SystemConsole.CommandStatus commandStatus = new SystemConsole.CommandStatus(status.echo);
				commandStatus.Batch();
				if (commandStatus.isLoud)
				{
					commandStatus.Quiet();
				}
				for (int l = 0; l < list.Count; l++)
				{
					this.ExecuteCommand(string.Concat(new string[]
					{
						"alias ",
						text3,
						list[l].Substring(text4.Length),
						" ",
						list[l]
					}), ref commandStatus, commandStatus.echo);
				}
				return;
			}
			else if (text3[0] == '+' || text3[0] == '-')
			{
				bool flag = text3[0] == '+';
				text3 = text3.Substring(1);
				SystemConsole.ConsoleCommand consoleCommand2;
				this.ConsoleCommands.TryGetValue(text3, out consoleCommand2);
				if (!this.CommandAvailable(consoleCommand2))
				{
					this.Log("Unknown command: " + text3, false);
					return;
				}
				if (consoleCommand2.type != SystemConsole.CommandType.Variable || !(consoleCommand2.variableType == typeof(bool)) || !(consoleCommand2.aliasOf == ""))
				{
					this.Log("+/- prefix is used to attach a command to a toggle variable changing value", false);
					return;
				}
				if (parameters != null)
				{
					text4 = text4 + " " + parameters;
				}
				if (flag)
				{
					this.toggleOnExecute[text3] = text4;
				}
				else
				{
					this.toggleOffExecute[text3] = text4;
				}
				if (!status.isSilent)
				{
					this.Log(string.Format("Attached: {0}{1} -> {2}", flag ? '+' : '-', text3, text4), status.inBatch);
					return;
				}
			}
			else
			{
				SystemConsole.ConsoleCommand consoleCommand3;
				this.ConsoleCommands.TryGetValue(text4, out consoleCommand3);
				if (!this.CommandAvailable(consoleCommand3))
				{
					this.Log("Unknown command: " + text4, false);
					return;
				}
				SystemConsole.ConsoleCommand consoleCommand4;
				bool flag2 = this.ConsoleCommands.TryGetValue(text3, out consoleCommand4);
				consoleCommand4 = new SystemConsole.ConsoleCommand(text3, consoleCommand3.type, consoleCommand3.permission, consoleCommand3.help, consoleCommand3.documentation, consoleCommand3.code, consoleCommand3.defaults.Trim() + " " + parameters, consoleCommand3.variableType, SystemConsole.StoreAs.DoNotStore, SystemConsole.CommandOrigin.Internal, null);
				consoleCommand4.aliasOf = text4;
				consoleCommand4.origin = SystemConsole.CommandOrigin.UserVariable;
				this.ConsoleCommands[text3] = consoleCommand4;
				if (!flag2)
				{
					this.ConsoleCommandList.Add(text3);
					this.ConsoleCommandList.Sort();
					if (this.StoreTextAliases.Contains(text4))
					{
						this.StoreTextAliases.Add(text3);
					}
				}
				if (!status.isSilent)
				{
					this.Log(string.Format("Added: {0} -> {1} {2}", text3, text4, consoleCommand4.defaults), status.inBatch);
				}
			}
		}, ""));
		this.ConsoleCommands.Add("append", this.NewPlayerCommand("append", "Add text to a text variable.", "USAGE: $CMD$ {-n} <variable> {<text>}\nAppends <text> to <variable>.  If no <text> specified then appends last entered command (before this one).\n-n = do not insert newline before appending.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool flag = true;
			string text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			while (text3 == null || text3 == "-n")
			{
				if (text3 == null)
				{
					this.Log("You must provide a text <variable>.", false);
					return;
				}
				flag = false;
				text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			}
			SystemConsole.ConsoleCommand consoleCommand;
			this.ConsoleCommands.TryGetValue(text3, out consoleCommand);
			if (consoleCommand == null || consoleCommand.type != SystemConsole.CommandType.Variable || consoleCommand.variableType != typeof(string))
			{
				this.Log("You must provide a text <variable>.", false);
				return;
			}
			string text4 = LibString.lookAhead(parameters, false, ' ', 0, false, false);
			if (text4 != null)
			{
				text4 = parameters;
			}
			else
			{
				text4 = Singleton<UIChatInput>.Instance.LastEnteredCommand;
			}
			SystemConsole.CommandStatus commandStatus = new SystemConsole.CommandStatus(SystemConsole.CommandEcho.Loud);
			commandStatus.Batch();
			this.ExecuteCommand(text3 + " -return", ref commandStatus, SystemConsole.CommandEcho.Silent);
			string text5 = (string)commandStatus.value;
			if (flag)
			{
				text5 += "\n";
			}
			text5 += text4;
			status.Batch();
			status.Raw();
			this.ExecuteCommand(text3 + " " + text5, ref status, SystemConsole.CommandEcho.Quiet);
			status.Default();
		}, ""));
		this.ConsoleCommands.Add("autoexec", this.NewPlayerVariable<string>("autoexec", "Batch which automatically runs every time the game restarts.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string value = SerializationScript.LoadPlayerPref("SystemConsoleAutoexecStart", DirectoryScript.autoexecFilePath);
			this.Variable("autoexec", ref value, ref status, parameters);
			if (status.isDirty)
			{
				PlayerPrefs.SetString("SystemConsoleAutoexecStart", value);
				SerializationScript.SavePlayerPref("SystemConsoleAutoexecStart", DirectoryScript.autoexecFilePath);
			}
		}, ""));
		this.ConsoleCommands.Add("autosave_games_window_count", this.NewPlayerVariable<int>("autosave_games_window_count", SystemConsole.StoreAs.Int, "The number of auto save files that can appear on the front page of the Games window. (They will all appear when you go into the Save & Load section)", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("autosave_games_window_count", ref UIGridMenuGames.MaxAutosavesOnFeatureRow, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("autosave_interval", this.NewPlayerVariable<float>("autosave_interval", SystemConsole.StoreAs.Float, "Time in seconds between each auto save. If set to 0 then auto save is disabled.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("autosave_interval", ref NetworkSingleton<SaveManager>.Instance.AutoSaveInterval, ref status, parameters);
			if (NetworkSingleton<SaveManager>.Instance.AutoSaveInterval != 0f && NetworkSingleton<SaveManager>.Instance.AutoSaveInterval < 1f)
			{
				NetworkSingleton<SaveManager>.Instance.AutoSaveInterval = 1f;
			}
		}, ""));
		this.ConsoleCommands.Add("autosave_log", this.NewPlayerVariable<bool>("autosave_log", SystemConsole.StoreAs.Bool, "If ON then when an auto save happens it will be logged in the system console.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("autosave_log", ref NetworkSingleton<SaveManager>.Instance.AutoSaveLog, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("autosave_slots", this.NewPlayerVariable<int>("autosave_slots", SystemConsole.StoreAs.Int, "The number of auto save slots. If set to 0 then auto save is disabled.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("autosave_slots", ref NetworkSingleton<SaveManager>.Instance.AutoSaveCount, ref status, parameters);
			if (NetworkSingleton<SaveManager>.Instance.AutoSaveCount != 0 && NetworkSingleton<SaveManager>.Instance.AutoSaveCount < 1)
			{
				NetworkSingleton<SaveManager>.Instance.AutoSaveCount = 1;
			}
		}, ""));
		this.ConsoleCommands.Add("bootexec", this.NewPlayerVariable<string>("bootexec", "Batch which automatically runs when the game first starts up.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string value = SerializationScript.LoadPlayerPref("SystemConsoleAutoexecBoot", DirectoryScript.bootexecFilePath);
			this.Variable("bootexec", ref value, ref status, parameters);
			if (status.isDirty)
			{
				PlayerPrefs.SetString("SystemConsoleAutoexecBoot", value);
				SerializationScript.SavePlayerPref("SystemConsoleAutoexecBoot", DirectoryScript.bootexecFilePath);
			}
		}, ""));
		this.ConsoleCommands.Add("bind", this.NewPlayerCommand("bind", "Bind a command to a key.", "USAGE: $CMD$ {{+|-|!}<key> {<command>}}\nIf <command> is specified then it is bound to trigger whenever <key> is pressed.\nOptional prefixes on <key>:\n   + = trigger on key press (same as no prefix)\n   - = trigger on key release\n   ! = trigger on key long press\nIf no <command> specified will display whatever <key> is currently bound to, or all bindings if no <key> specified.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text3 == null)
			{
				if (!status.isSilent)
				{
					if (this.KeyPressBinds.Count == 0 && this.KeyReleaseBinds.Count == 0 && this.VRKeyLongPressBinds.Count == 0 && this.VRKeyPressBinds.Count == 0 && this.VRKeyReleaseBinds.Count == 0)
					{
						this.Log("No current bindings.", false);
						return;
					}
					foreach (ModifiedKeyCode key in this.KeyPressBinds.Keys)
					{
						if (this.KeyReleaseBinds.ContainsKey(key))
						{
							this.Log("+" + key.ToString() + " -> " + this.KeyPressBinds[key], Colour.Orange, false);
							this.Log("-" + key.ToString() + " -> " + this.KeyReleaseBinds[key], Colour.Orange, false);
						}
						else
						{
							this.Log(key.ToString() + " -> " + this.KeyPressBinds[key], Colour.Orange, false);
						}
					}
					foreach (ModifiedKeyCode key2 in this.KeyReleaseBinds.Keys)
					{
						if (!this.KeyPressBinds.ContainsKey(key2))
						{
							this.Log("-" + key2.ToString() + " -> " + this.KeyReleaseBinds[key2], Colour.Orange, false);
						}
					}
					foreach (VRTrackedController.VRKeyCode key3 in this.VRKeyPressBinds.Keys)
					{
						if (this.VRKeyReleaseBinds.ContainsKey(key3) || this.VRKeyLongPressBinds.ContainsKey(key3))
						{
							this.Log("+" + key3.ToString() + " -> " + this.VRKeyPressBinds[key3], Colour.Orange, false);
							if (this.VRKeyReleaseBinds.ContainsKey(key3))
							{
								this.Log("-" + key3.ToString() + " -> " + this.VRKeyReleaseBinds[key3], Colour.Orange, false);
							}
							if (this.VRKeyLongPressBinds.ContainsKey(key3))
							{
								this.Log("!" + key3.ToString() + " -> " + this.VRKeyLongPressBinds[key3], Colour.Orange, false);
							}
						}
						else
						{
							this.Log(key3.ToString() + " -> " + this.VRKeyPressBinds[key3], Colour.Orange, false);
						}
					}
					foreach (VRTrackedController.VRKeyCode key4 in this.VRKeyReleaseBinds.Keys)
					{
						if (!this.VRKeyPressBinds.ContainsKey(key4))
						{
							this.Log("-" + key4.ToString() + " -> " + this.VRKeyReleaseBinds[key4], Colour.Orange, false);
							if (this.VRKeyLongPressBinds.ContainsKey(key4))
							{
								this.Log("!" + key4.ToString() + " -> " + this.VRKeyLongPressBinds[key4], Colour.Orange, false);
							}
						}
					}
					foreach (VRTrackedController.VRKeyCode key5 in this.VRKeyLongPressBinds.Keys)
					{
						if (!this.VRKeyPressBinds.ContainsKey(key5) && !this.VRKeyReleaseBinds.ContainsKey(key5))
						{
							this.Log("!" + key5.ToString() + " -> " + this.VRKeyLongPressBinds[key5], Colour.Orange, false);
						}
					}
				}
				return;
			}
			bool flag = false;
			if (Regex.IsMatch(text3, "^[-+!]?vr", RegexOptions.IgnoreCase))
			{
				flag = true;
			}
			ModifiedKeyCode key6 = new ModifiedKeyCode(KeyCode.None, false, false, false);
			Dictionary<ModifiedKeyCode, string> dictionary = this.KeyPressBinds;
			VRTrackedController.VRKeyCode vrkeyCode = VRTrackedController.VRKeyCode.None;
			Dictionary<VRTrackedController.VRKeyCode, string> dictionary2 = this.VRKeyPressBinds;
			string str = "";
			if (text3.StartsWith("-"))
			{
				if (flag)
				{
					dictionary2 = this.VRKeyReleaseBinds;
				}
				else
				{
					dictionary = this.KeyReleaseBinds;
				}
				str = "-";
				text3 = text3.Substring(1);
			}
			else if (text3.StartsWith("+"))
			{
				str = "+";
				text3 = text3.Substring(1);
			}
			else if (text3.StartsWith("!"))
			{
				if (!flag)
				{
					this.Log("Keyboard binds do not support long press events", false);
					return;
				}
				dictionary2 = this.VRKeyLongPressBinds;
				str = "!";
				text3 = text3.Substring(1);
			}
			if (flag)
			{
				text3 = "VR" + LibString.CamelCaseFromUnderscore(text3, true, false).Substring(2);
			}
			try
			{
				if (flag)
				{
					vrkeyCode = (VRTrackedController.VRKeyCode)Enum.Parse(typeof(VRTrackedController.VRKeyCode), text3);
					if (vrkeyCode < (VRTrackedController.VRKeyCode)32)
					{
						this.Log("<key> not recognized.", false);
						return;
					}
				}
				else
				{
					key6 = LibKeyCode.ModifiedKeyCodeFromString(text3);
				}
			}
			catch
			{
				this.Log("<key> not recognized.", false);
				return;
			}
			if (parameters == "")
			{
				if (!status.isSilent)
				{
					if (flag)
					{
						if (dictionary2.ContainsKey(vrkeyCode))
						{
							this.Log(str + text3 + " -> " + dictionary2[vrkeyCode], Colour.Orange, false);
						}
						else
						{
							this.Log(str + text3 + " -> <nothing>", Colour.Orange, false);
						}
					}
					else if (dictionary.ContainsKey(key6))
					{
						this.Log(str + text3 + " -> " + dictionary[key6], Colour.Orange, false);
					}
					else
					{
						this.Log(str + text3 + " -> <nothing>", Colour.Orange, false);
					}
				}
				if (flag)
				{
					if (dictionary2.ContainsKey(vrkeyCode))
					{
						SystemConsole.lastReturnedValue = dictionary2[vrkeyCode];
						return;
					}
					SystemConsole.lastReturnedValue = "";
					return;
				}
				else
				{
					if (dictionary.ContainsKey(key6))
					{
						SystemConsole.lastReturnedValue = dictionary[key6];
						return;
					}
					SystemConsole.lastReturnedValue = "";
					return;
				}
			}
			else
			{
				if (flag)
				{
					dictionary2[vrkeyCode] = parameters;
					VRTrackedController.ProcessTouchpadBinding(vrkeyCode, parameters);
				}
				else
				{
					dictionary[key6] = parameters;
				}
				if (!status.isSilent)
				{
					this.Log(str + text3 + " -> " + parameters, Colour.Orange, false);
				}
			}
		}, ""));
		this.ConsoleCommands.Add("bitstream_pool_debug", this.NewDeveloperCommand("bitstream_pool_debug", "Prints out the current bitstream pool.", "USAGE: $CMD$ <text>\nPrints out the current bitstream pool.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			List<BitStream.PoolStream> pool = BitStream.GetPool();
			long num = 0L;
			for (int k = 0; k < pool.Count; k++)
			{
				BitStream.PoolStream poolStream = pool[k];
				num += (long)poolStream.stream.Buffer.Length;
				this.Log(string.Format("Pooled Stream index: {0} inUse: {1}", k, poolStream.inUse), false);
			}
			this.Log(string.Format("Pool Count: {0} Pool Size: {1}", pool.Count, Utilities.BytesToFileSizeString(num)), false);
		}, ""));
		this.ConsoleCommands.Add("broadcast", this.NewPlayerCommand("broadcast", "Broadcast text.", "USAGE: $CMD$ <text>\nBroadcasts the specified <text>.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (!status.isSilent)
			{
				UIBroadcast.Log(parameters, Colour.White, 2f, 0f);
			}
		}, ""));
		this.ConsoleCommands.Add("camera_clear_saved_positions", this.NewPlayerCommand("camera_clear_saved_positions", "Resets camera saved positions.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			Singleton<CameraController>.Instance.CameraStates = new CameraState[10];
		}, ""));
		this.ConsoleCommands.Add("camera_load", this.NewPlayerVariable<int>("camera_load", "Set camera position to saved position.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			int lastLoadedCamera = Singleton<CameraController>.Instance.lastLoadedCamera;
			this.Variable("camera_load", ref lastLoadedCamera, ref status, parameters);
			if (status.isDirty)
			{
				if (Singleton<CameraController>.Instance.CameraStateInUse(lastLoadedCamera))
				{
					Singleton<CameraController>.Instance.LoadCamera(lastLoadedCamera);
					return;
				}
				this.Log("No camera save in slot " + lastLoadedCamera, false);
			}
		}, ""));
		this.ConsoleCommands.Add("camera_load_zero", this.NewPlayerVariable<int>("camera_load_zero", "Set camera position to saved position.  Position is zero-indexed (so one less than displayed in UI)", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			int num = Singleton<CameraController>.Instance.lastLoadedCamera - 1;
			this.Variable("camera_load_zero", ref num, ref status, parameters);
			if (status.isDirty)
			{
				num++;
				if (Singleton<CameraController>.Instance.CameraStateInUse(num))
				{
					Singleton<CameraController>.Instance.LoadCamera(num);
					return;
				}
				this.Log("No camera save in slot " + num, false);
			}
		}, ""));
		this.ConsoleCommands.Add("camera_rotation_rate", this.NewPlayerVariable<float>("camera_rotation_rate", SystemConsole.StoreAs.Float, "Sets how fast the camera rotates to the saved direction.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("camera_rotation_rate", ref Singleton<CameraController>.Instance.LOAD_ROTATION_RATE, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("camera_reset_on_load", this.NewPlayerVariable<bool>("camera_reset_on_load", SystemConsole.StoreAs.Bool, "When ON the camera will reset its position when you load a game.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("camera_reset_on_load", ref TableScript.RESET_CAMERA_ON_LOAD, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("camera_restore_saved_positions", this.NewPlayerCommand("camera_restore_saved_positions", "Restore camera saved positions.", "USAGE: $CMD$ {<label>}\nRestore camera saved positions.  Read from default store, or <label> if specified.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text3 != null)
			{
				text3 = "saved_cameras_" + text3.Replace(" ", "");
			}
			else
			{
				text3 = "default_saved_cameras";
			}
			string @string = PlayerPrefs.GetString(text3, "");
			if (@string == "")
			{
				this.Log("No saved positions found.", false);
				return;
			}
			Singleton<CameraController>.Instance.CameraStates = Json.Load<CameraState[]>(@string);
			if (!status.isSilent)
			{
				int num = 0;
				for (int k = 0; k < 10; k++)
				{
					if (Singleton<CameraController>.Instance.CameraStateInUse(k))
					{
						num++;
					}
				}
				if (!status.isSilent)
				{
					this.Log("Restored " + num + " saved cameras.", status.inBatch);
				}
			}
		}, ""));
		this.ConsoleCommands.Add("camera_save", this.NewPlayerVariable<int>("camera_save", "Save camera position.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			int lastSavedCamera = Singleton<CameraController>.Instance.lastSavedCamera;
			this.Variable("camera_save", ref lastSavedCamera, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<CameraController>.Instance.SaveCamera(lastSavedCamera, true);
			}
		}, ""));
		this.ConsoleCommands.Add("camera_save_zero", this.NewPlayerVariable<int>("camera_save_zero", "Save camera position.  Position is zero-indexed (so one less than displayed in UI)", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			int num = Singleton<CameraController>.Instance.lastSavedCamera - 1;
			this.Variable("camera_save_zero", ref num, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<CameraController>.Instance.SaveCamera(num + 1, true);
			}
		}, ""));
		this.ConsoleCommands.Add("camera_store_saved_positions", this.NewPlayerCommand("camera_store_saved_positions", "Store camera saved positions.", "USAGE: $CMD$ {<label>}\nStore camera saved positions.  Stores as default, or as <label> if specified.  If no saved cameras exist then <label> will be removed.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text3 != null)
			{
				text3 = "saved_cameras_" + text3.Replace(" ", "");
			}
			else
			{
				text3 = "default_saved_cameras";
			}
			int num = 0;
			for (int k = 0; k < 10; k++)
			{
				if (Singleton<CameraController>.Instance.CameraStateInUse(k))
				{
					num++;
				}
			}
			if (num > 0)
			{
				string json = Json.GetJson(Singleton<CameraController>.Instance.CameraStates, true);
				PlayerPrefs.SetString(text3, json);
				if (!status.isSilent)
				{
					this.Log("Stored " + num + " saved cameras.", status.inBatch);
					return;
				}
			}
			else
			{
				PlayerPrefs.DeleteKey(text3);
				if (!status.isSilent)
				{
					this.Log("Removed saved cameras.", status.inBatch);
				}
			}
		}, ""));
		this.ConsoleCommands.Add("card_is_a_deck_for_hotkeys", this.NewPlayerVariable<bool>("card_is_a_deck_for_hotkeys", SystemConsole.StoreAs.Bool, "When ON pressing a number on a card will draw it.  When OFF card will be treated like any other component (changing rotation value / state / etc.).", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("card_is_a_deck_for_hotkeys", ref CardScript.CARD_IS_A_DECK_FOR_HOTKEYS, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("chat_copy", this.NewPlayerCommand("chat_copy", "Copy text from current chat window to clipboard.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			Regex regex = new Regex("(\\[[a-zA-Z0-9]{1,6}\\])|(\\[-\\])", RegexOptions.Compiled, new TimeSpan(0, 0, 1));
			MatchEvaluator evaluator = (Match x) => "";
			string text3 = NetworkSingleton<Chat>.Instance.ChatList.GetText();
			try
			{
				text3 = regex.Replace(text3, evaluator);
			}
			catch (RegexMatchTimeoutException)
			{
			}
			GUIUtility.systemCopyBuffer = text3;
		}, ""));
		this.ConsoleCommands.Add("chat_filter", this.NewPlayerVariable<bool>("chat_filter", SystemConsole.StoreAs.Manual, "When ON chat messages will be filtered.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool filterChatMessages = Singleton<ChatSettings>.Instance.FilterChatMessages;
			this.Variable("chat_filter", ref filterChatMessages, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<ChatSettings>.Instance.FilterChatMessages = filterChatMessages;
			}
		}, ""));
		this.ConsoleCommands.Add("chat_font_size", this.NewPlayerVariable<int>("chat_font_size", SystemConsole.StoreAs.Int, "Sets size of chat font.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			int chatFontSize = NetworkSingleton<Chat>.Instance.GetChatFontSize();
			this.Variable("chat_font_size", ref chatFontSize, ref status, parameters);
			if (status.isDirty)
			{
				if (chatFontSize >= 8 && chatFontSize <= 72)
				{
					NetworkSingleton<Chat>.Instance.SetChatFontSize(chatFontSize);
					return;
				}
				this.Log("Invalid font size : resetting to previous value.", false);
			}
		}, ""));
		this.ConsoleCommands.Add("chat_input", this.NewPlayerCommand("chat_input", "Activate chat input.", "USAGE: $CMD$\nActivate chat input box.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			Singleton<UIChatInput>.Instance.Activate();
		}, ""));
		this.ConsoleCommands.Add("chat_input_clear_on_dismiss", this.NewPlayerVariable<bool>("chat_input_clear_on_dismiss", "When ON chat input is cleared when chat is dismissed.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("chat_input_clear_on_dismiss", ref UIChatInput.ClearOnDismiss, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("chat_refresh_filter", this.NewPlayerCommand("chat_refresh_filter", "Refresh the IRC chat filter.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			Singleton<ChatIRC>.Instance.RefreshWordlist(true);
		}, ""));
		this.ConsoleCommands.Add("chat_tab_game", this.NewPlayerCommand("chat_tab_game", "Switch to GAME tab.", "USAGE: $CMD$\nSwitch chat tab to GAME tab, if available.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.ChatTabHelper(status, parameters, ChatMessageType.Game);
		}, ""));
		this.ConsoleCommands.Add("chat_tab_global", this.NewPlayerCommand("chat_tab_global", "Switch to GLOBAL tab.", "USAGE: $CMD$\nSwitch chat tab to GLOBAL tab, if available.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.ChatTabHelper(status, parameters, ChatMessageType.Global);
		}, ""));
		this.ConsoleCommands.Add("chat_tab_system", this.NewPlayerCommand("chat_tab_system", "Switch to SYSTEM tab.", "USAGE: $CMD$ [-f]\nSwitch chat tab to SYSTEM tab, if available.\n-f = focus input", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.ChatTabHelper(status, parameters, ChatMessageType.System);
		}, ""));
		this.ConsoleCommands.Add("chat_tab_team", this.NewPlayerCommand("chat_tab_team", "Switch to TEAM tab.", "USAGE: $CMD$\nSwitch chat tab to TEAM tab, if available.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.ChatTabHelper(status, parameters, ChatMessageType.Team);
		}, ""));
		this.ConsoleCommands.Add("chat_visible", this.NewPlayerVariable<bool>("chat_visible", "Whether the chat window is currently visible.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool chatActive = NetworkSingleton<Chat>.Instance.ChatActive;
			this.Variable("chat_visible", ref chatActive, ref status, parameters);
			if (status.isDirty)
			{
				NetworkSingleton<Chat>.Instance.ChatActive = chatActive;
			}
		}, ""));
		this.ConsoleCommands.Add("choose", this.NewPlayerCommand("choose", "Allow the user to make a choice from a drop-down.", "USAGE: $CMD$ <variable> {-t <title>} <option> {<option>...}\nIf <variable> is a number, set it to index of selected option.\nIf <variable> is text, set it to the selected option.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string variableName = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (variableName == null)
			{
				this.Log("You must provide a text <variable>.", false);
				return;
			}
			SystemConsole.ConsoleCommand command;
			this.ConsoleCommands.TryGetValue(variableName, out command);
			if (command == null || command.type != SystemConsole.CommandType.Variable || (command.variableType != typeof(string) && command.variableType != typeof(float)))
			{
				this.Log("You must provide a number or text <variable>.", false);
				return;
			}
			string description = "Choose";
			List<string> options = new List<string>();
			for (string text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0'); text3 != null; text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0'))
			{
				if (text3 == "-t")
				{
					description = LibString.bite(ref parameters, false, ' ', false, false, '\0');
				}
				else
				{
					options.Add(text3);
				}
			}
			if (options.Count == 0)
			{
				this.Log("You must provide at least one option.", false);
				return;
			}
			UIDialog.ShowDropDown(description, "OK", "Cancel", options, delegate(string choice)
			{
				if (choice != "")
				{
					if (command.variableType == typeof(string))
					{
						this.ProcessCommand(variableName + " " + choice, false, SystemConsole.CommandEcho.Silent);
						return;
					}
					this.ProcessCommand(variableName + " " + options.IndexOf(choice), false, SystemConsole.CommandEcho.Silent);
				}
			}, null, "");
		}, ""));
		this.ConsoleCommands.Add("clear", this.NewPlayerCommand("clear", "Clears a text variable.", "USAGE: $CMD$ <variable>\nClears specified text variable.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text3 == null)
			{
				this.Log("You must provide a text <variable>.", false);
				return;
			}
			SystemConsole.ConsoleCommand consoleCommand;
			this.ConsoleCommands.TryGetValue(text3, out consoleCommand);
			if (consoleCommand == null || consoleCommand.type != SystemConsole.CommandType.Variable || consoleCommand.variableType != typeof(string))
			{
				this.Log("You must provide a text <variable>.", false);
				return;
			}
			this.ProcessCommand(text3 + " -clear", false, SystemConsole.CommandEcho.Silent);
		}, ""));
		this.ConsoleCommands.Add("color", this.NewPlayerVariable<string>("color", "Your current player color.  Changing it will change your seat.", "USAGE: $CMD$ {<color>}\nReturns your current color.  If <color> provided will attempt to swap you to that color.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text3 = Colour.MyColorLabel();
			Colour colour = Colour.ColourFromLabel(text3);
			string text4 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			status.value = text3;
			if (text4 == "-return")
			{
				return;
			}
			if (text4 == "-variable")
			{
				if (!status.isSilent)
				{
					this.LogVariable("color", text3, colour, 37);
				}
				return;
			}
			if (text4 != null)
			{
				if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.ChangeColor, -1))
				{
					this.Log("You may not change color.", false);
					return;
				}
				text4 = LibString.CamelCaseFromUnderscore(text4.ToLower(), true, false);
				Colour colour2;
				if (!Colour.TryColourFromLabel(text4, out colour2))
				{
					this.Log(text4 + " is not a color.", false);
					return;
				}
				if (colour2 == Colour.Black && !this.CommandAvailable(SystemConsole.CommandPermission.Admin))
				{
					this.Log(text4 + " is not available.", false);
					return;
				}
				if (!NetworkSingleton<PlayerManager>.Instance.ColourInUse(colour2))
				{
					colour = colour2;
					text3 = text4;
				}
				NetworkSingleton<NetworkUI>.Instance.ClientRequestColor(text4);
			}
			SystemConsole.lastReturnedValue = text3;
			status.value = text3;
			if (!status.isSilent)
			{
				this.Log("color: " + colour.Hex + text3, status.inBatch);
			}
		}, ""));
		this.ConsoleCommands.Add("commands", this.NewPlayerCommand("commands", "Lists all commands (not including variables).", "USAGE: $CMD$ {-a} {prefix}\nDisplays all command roots which are not variables.\nIf prefix is supplied then only commands which start with it will be displayed.\n-a = display all commands, not just command roots.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (status.isSilent)
			{
				return;
			}
			bool flag = false;
			string text3 = LibString.bite(ref parameters, true, ' ', false, false, '\0');
			if (text3 == "-a")
			{
				flag = true;
				text3 = LibString.bite(ref parameters, true, ' ', false, false, '\0');
			}
			if (text3 != null)
			{
				flag = true;
			}
			string b = "";
			for (int k = 0; k < this.ConsoleCommandList.Count; k++)
			{
				string text4 = this.ConsoleCommandList[k];
				SystemConsole.ConsoleCommand consoleCommand = this.ConsoleCommands[text4];
				if (text4.Contains("_") && !flag)
				{
					text4 = "[A020F0]" + LibString.lookAhead(text4, '_') + "_";
				}
				if (this.CommandAvailable(consoleCommand) && consoleCommand.type == SystemConsole.CommandType.Command && (text3 == null || text4.StartsWith(text3)) && text4 != b)
				{
					b = text4;
					if (consoleCommand.aliasOf != "")
					{
						text4 = string.Concat(new string[]
						{
							"[006D5C]",
							text4,
							"[AAAAAA] (",
							consoleCommand.aliasOf,
							consoleCommand.defaults
						});
						text4 = text4.TrimEnd(new char[]
						{
							' '
						}) + ")";
					}
					this.Log(text4, Colour.Teal, true);
				}
				else
				{
					b = text4;
				}
			}
		}, ""));
		this.ConsoleCommands.Add("component_default_autoraise", this.NewPlayerVariable<bool>("component_default_autoraise", "Default state of component toggle for auto-raise.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("component_default_autoraise", ref NetworkPhysicsObject.AutoRaiseOverride, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("component_default_grid", this.NewPlayerVariable<bool>("component_default_grid", "Default state of component toggle for grid.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("component_default_grid", ref NetworkPhysicsObject.GridOverride, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("component_default_hands", this.NewPlayerVariable<bool>("component_default_hands", "Default state of component toggle for hands.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("component_default_hands", ref NetworkPhysicsObject.UseHandsOverride, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("component_default_ignore_fow", this.NewPlayerVariable<bool>("component_default_ignore_fow", "Default state of component toggle for ignore fog-of-war.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("component_default_ignore_fow", ref NetworkPhysicsObject.IgnoreFOWOverride, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("component_default_reveal_fow", this.NewPlayerVariable<bool>("component_default_reveal_fow", "Default state of component toggle for reveal fog-of-war.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("component_default_reveal_fow", ref NetworkPhysicsObject.RevealFOROverride, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("component_default_snap", this.NewPlayerVariable<bool>("component_default_snap", "Default state of component toggle for snap.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("component_default_snap", ref NetworkPhysicsObject.SnapOverride, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("component_default_sticky", this.NewPlayerVariable<bool>("component_default_sticky", "Default state of component toggle for sticky.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("component_default_sticky", ref NetworkPhysicsObject.StickyOverride, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("component_default_tooltip", this.NewPlayerVariable<bool>("component_default_tooltip", "Default state of component toggle for tooltip.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("component_default_tooltip", ref NetworkPhysicsObject.TooltipOverride, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("component_examine", this.NewPlayerVariable<string>("component_examine", "Currently examined component.", "USAGE: $CMD$ {<GUID>|<color>}\nIf <GUID> specified then start examining that component. If <color> specified then start examining that player's hand zone.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text3 = "";
			if (this.examinedObject != null)
			{
				text3 = this.examinedObjectID;
			}
			this.Variable("component_examine", ref text3, ref status, parameters);
			if (status.isDirty)
			{
				GameObject x = HandZone.GetHand(LibString.CamelCaseFromUnderscore(text3, true, false), 0);
				if (x == null)
				{
					x = NetworkSingleton<ManagerPhysicsObject>.Instance.GOFromGUID(text3);
				}
				if (x == null)
				{
					this.Log("No such component.", false);
					return;
				}
				this.examinedObject = x;
				this.examinedObjectID = text3;
			}
		}, ""));
		this.ConsoleCommands.Add("component_hotkey_state_change", this.NewPlayerVariable<bool>("component_hotkey_state_change", SystemConsole.StoreAs.Bool, "When ON pressing a number on a component which has multiple states will select the state.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("component_hotkey_state_change", ref NetworkPhysicsObject.ChangeStateByTypingNumbers, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("component_hiders", this.NewAdminCommand("component_hiders", "Displays any hiders attached to component.", "USAGE: $CMD$ {<guid>}\nDisplays any hiders attached to component specified by <guid>. If no <guid> provided then that last held component will be used.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (status.isSilent)
			{
				return;
			}
			string text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			NetworkPhysicsObject networkPhysicsObject;
			if (text3 == null)
			{
				networkPhysicsObject = NetworkPhysicsObject.LastNPOHeldByMe;
			}
			else
			{
				networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGUID(text3);
			}
			if (networkPhysicsObject)
			{
				this.Log(string.Format("\n{0} : {1}", networkPhysicsObject.GUID, networkPhysicsObject.tag), Colour.Grey, false);
				if (networkPhysicsObject.ObscuredHiders.Count == 0 && networkPhysicsObject.InvisibleHiders.Count == 0)
				{
					this.Log("No hiders attached.", false);
					return;
				}
				if (networkPhysicsObject.ObscuredHiders.Count > 0)
				{
					foreach (KeyValuePair<string, uint> keyValuePair in networkPhysicsObject.ObscuredHiders)
					{
						this.LogVariable(keyValuePair.Key, "Face", Colour.Blue, 37);
					}
				}
				if (networkPhysicsObject.InvisibleHiders.Count > 0)
				{
					foreach (KeyValuePair<string, uint> keyValuePair2 in networkPhysicsObject.InvisibleHiders)
					{
						this.LogVariable(keyValuePair2.Key, "Invisible", Colour.Purple, 37);
					}
				}
			}
		}, ""));
		this.ConsoleCommands.Add("component_locked", this.NewPlayerVariable<bool>("component_locked", "Locked state of specified component.", "USAGE: $CMD$ <guid> {<locked>}\nLocked state of component specified by <guid>.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text3 == "-variable")
			{
				return;
			}
			NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGUID(text3);
			if (!networkPhysicsObject)
			{
				this.Log("Component not found.", false);
				return;
			}
			bool isLocked = networkPhysicsObject.IsLocked;
			this.Variable("component_locked<" + text3 + ">", ref isLocked, ref status, parameters);
			if (status.isDirty)
			{
				PlayerScript.PointerScript.Lock(networkPhysicsObject, isLocked);
			}
		}, ""));
		this.ConsoleCommands.Add("component_move", this.NewPlayerCommand("component_move", "Move a component.", "USAGE: $CMD$ <GUID> {-f} <translation>\nMove specified component.  <translation> is a vector (x,y,z).  Use '-' for any axis to leave it as is.-f = fast", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.ComponentTransformHelper("component_move", status, parameters, SystemConsole.TransformType.MoveRelative);
		}, ""));
		this.ConsoleCommands.Add("component_override_defaults", this.NewPlayerVariable<bool>("component_override_defaults", "When ON components spawned from UI will use defaults set by `component_default_...` commands.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("component_override_defaults", ref NetworkPhysicsObject.OverrideDefaultsWhenSpawning, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("component_position", this.NewPlayerCommand("component_position", "Set position of a component.", "USAGE: $CMD$ <GUID> {-f} <position>\nMove specified component to specified <position>.  <position> is a vector (x,y,z).  Use '-' for any axis to leave it as is.-f = fast", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.ComponentTransformHelper("component_position", status, parameters, SystemConsole.TransformType.MoveAbsolute);
		}, ""));
		this.ConsoleCommands.Add("component_rotate", this.NewPlayerCommand("component_rotate", "Rotate a component.", "USAGE: $CMD$ <GUID> {-f} <rotation>\nRotate specified component.  <rotation> is a vector (x,y,z).  Use '-' for any axis to leave it as is.-f = fast", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.ComponentTransformHelper("component_rotate", status, parameters, SystemConsole.TransformType.RotateRelative);
		}, ""));
		this.ConsoleCommands.Add("component_rotation", this.NewPlayerCommand("component_rotation", "Set rotation of a component.", "USAGE: $CMD$ <GUID> {-f} <rotation>\nRotate specified component to specified <rotation>.  <rotation> is a vector (x,y,z).  Use '-' for any axis to leave it as is.-f = fast", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.ComponentTransformHelper("component_rotation", status, parameters, SystemConsole.TransformType.RotateAbsolute);
		}, ""));
		this.ConsoleCommands.Add("component_spread_distance", this.NewPlayerVariable<float>("component_spread_distance", SystemConsole.StoreAs.Float, "Distance cards are spread apart when the Spread action is used.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float spread_ACTION_DISTANCE = Pointer.SPREAD_ACTION_DISTANCE;
			this.Variable("component_spread_distance", ref spread_ACTION_DISTANCE, ref status, parameters);
			if (status.isDirty)
			{
				Pointer.SPREAD_ACTION_DISTANCE = Mathf.Max(0.6f, spread_ACTION_DISTANCE);
			}
		}, ""));
		this.ConsoleCommands.Add("component_spread_cards_per_row", this.NewPlayerVariable<int>("component_spread_cards_per_row", SystemConsole.StoreAs.Int, "maximum number of cards per row when the Spread action is used.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			int spread_ACTION_CARDS_PER_ROW = Pointer.SPREAD_ACTION_CARDS_PER_ROW;
			this.Variable("component_spread_cards_per_row", ref spread_ACTION_CARDS_PER_ROW, ref status, parameters);
			if (status.isDirty)
			{
				Pointer.SPREAD_ACTION_CARDS_PER_ROW = Mathf.Max(1, spread_ACTION_CARDS_PER_ROW);
			}
		}, ""));
		this.ConsoleCommands.Add("component_spread_row_distance", this.NewPlayerVariable<float>("component_spread_row_distance", SystemConsole.StoreAs.Float, "Distance between rows when cards are spread apart when the Spread action is used.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float spread_ACTION_ROW_DISTANCE = Pointer.SPREAD_ACTION_ROW_DISTANCE;
			this.Variable("component_spread_row_distance", ref spread_ACTION_ROW_DISTANCE, ref status, parameters);
			if (status.isDirty)
			{
				Pointer.SPREAD_ACTION_ROW_DISTANCE = Mathf.Max(3.2f, spread_ACTION_ROW_DISTANCE);
			}
		}, ""));
		this.ConsoleCommands.Add("component_state", this.NewPlayerVariable<int>("component_state", "State of specified component.", "USAGE: $CMD$ <guid> {<state>}\nState of component specified by <guid>.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text3 == "-variable")
			{
				return;
			}
			NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGUID(text3);
			if (!networkPhysicsObject)
			{
				this.Log("Component not found.", false);
				return;
			}
			if (networkPhysicsObject.HasStates())
			{
				int selectedStateId = networkPhysicsObject.GetSelectedStateId();
				this.Variable("component_state<" + text3 + ">", ref selectedStateId, ref status, parameters);
				if (status.isDirty)
				{
					networkPhysicsObject.ChangeState(selectedStateId);
					return;
				}
			}
			else if (networkPhysicsObject.customPDF != null)
			{
				int currentPDFPage = networkPhysicsObject.customPDF.CurrentPDFPage;
				this.Variable("component_state<" + text3 + ">", ref currentPDFPage, ref status, parameters);
				if (status.isDirty)
				{
					networkPhysicsObject.customPDF.CurrentPDFPage = currentPDFPage;
				}
			}
		}, ""));
		this.ConsoleCommands.Add("component_tooltip_delay", this.NewPlayerVariable<float>("component_tooltip_delay", SystemConsole.StoreAs.Float, "Length of time you must hover over a game component before the description tooltip is displayed.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("component_tooltip_delay", ref UIHoverText.NPODelayTooltipTime, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("component_update_visibility", this.NewPlayerCommand("component_update_visibility", "Force component visibility calculation to run.", "USAGE: $CMD$ {<GUID>}\n", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			foreach (NetworkPhysicsObject networkPhysicsObject in this.ComponentHelper(status, parameters))
			{
				networkPhysicsObject.UpdateVisiblity(false);
			}
		}, ""));
		this.ConsoleCommands.Add("component_wrap_states", this.NewPlayerVariable<bool>("component_wrap_states", SystemConsole.StoreAs.Bool, "When ON the Next State and Prev State commands will wrap around available states.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("component_wrap_states", ref NetworkPhysicsObject.WrapStates, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("console_hotkey_lock", this.NewPlayerVariable<bool>("console_hotkey_lock", SystemConsole.StoreAs.Bool, "When ON disables typing of the System Console keyboard shortcut; it will then *always* toggle the System Console", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("console_hotkey_lock", ref this.lockHotkey, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("container_logging", this.NewPlayerVariable<bool>("container_logging", SystemConsole.StoreAs.Bool, "When ON all player container search changes are logged.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("container_logging", ref Pointer.LOG_INVENTORY, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("debug_component", this.NewDeveloperCommand("debug_component", "Displays debug info for component.", "USAGE: $CMD$ {<GUID>}\nDisplays debug info on specified component. Leave GUID blank to use grabbed/hovered object.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (status.isSilent)
			{
				return;
			}
			string text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			NetworkPhysicsObject networkPhysicsObject = null;
			if (text3 != null)
			{
				networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGUID(text3);
			}
			else
			{
				if (PlayerScript.PointerScript && PlayerScript.PointerScript.FirstGrabbedObject)
				{
					networkPhysicsObject = PlayerScript.PointerScript.FirstGrabbedObject.GetComponent<NetworkPhysicsObject>();
				}
				if (networkPhysicsObject == null && HoverScript.HoverObject)
				{
					networkPhysicsObject = HoverScript.HoverObject.GetComponent<NetworkPhysicsObject>();
				}
			}
			if (networkPhysicsObject == null)
			{
				this.Log("Component not found", false);
				return;
			}
			Chat.LogObject(networkPhysicsObject);
		}, ""));
		this.ConsoleCommands.Add("debug_external_api", this.NewPlayerVariable<bool>("debug_external_api", SystemConsole.StoreAs.Bool, "When ON activates debug logging for extrnal api.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("debug_external_api", ref SystemConsole.DEBUG_EXTERNAL_API, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("debug_smooth_move", this.NewDeveloperVariable<bool>("debug_smooth_move", "When ON activates debug display for smooth moves.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("debug_smooth_move", ref NetworkPhysicsObject.DebugSmoothMove, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("debug_list_resources", this.NewPlayerCommand("debug_list_resources", "Lists number of currently loaded resources.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.LogVariable("All", Resources.FindObjectsOfTypeAll(typeof(UnityEngine.Object)).Length.ToString(), Colour.YellowDark, 37);
			this.LogVariable("Textures", Resources.FindObjectsOfTypeAll(typeof(Texture)).Length.ToString(), Colour.OrangeDark, 37);
			this.LogVariable("AudioClips", Resources.FindObjectsOfTypeAll(typeof(AudioClip)).Length.ToString(), Colour.OrangeDark, 37);
			this.LogVariable("Meshes", Resources.FindObjectsOfTypeAll(typeof(Mesh)).Length.ToString(), Colour.OrangeDark, 37);
			this.LogVariable("Materials", Resources.FindObjectsOfTypeAll(typeof(Material)).Length.ToString(), Colour.OrangeDark, 37);
			this.LogVariable("GameObjects", Resources.FindObjectsOfTypeAll(typeof(GameObject)).Length.ToString(), Colour.OrangeDark, 37);
			this.LogVariable(" NPOs", Resources.FindObjectsOfTypeAll(typeof(NetworkPhysicsObject)).Length.ToString(), Colour.OrangeDark, 37);
			this.LogVariable("Unity Components", Resources.FindObjectsOfTypeAll(typeof(Component)).Length.ToString(), Colour.BrownDark, 37);
		}, ""));
		this.ConsoleCommands.Add("deck_can_spread_facedown", this.NewPlayerVariable<bool>("deck_can_spread_facedown", SystemConsole.StoreAs.Bool, "When ON and you use the Spread action on a deck which is face-down, its cards will remain face-down after spreading.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("deck_can_spread_facedown", ref ManagerPhysicsObject.SpreadDeckRespectsCurrentOrientation, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("default_host_name", this.NewPlayerVariable<string>("default_host_name", SystemConsole.StoreAs.Manual, "Default server name when hosting.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string @string = PlayerPrefs.GetString("HostServerName", "");
			this.Variable("default_host_name", ref @string, ref status, parameters);
			if (status.isDirty)
			{
				PlayerPrefs.SetString("HostServerName", @string);
			}
		}, ""));
		this.ConsoleCommands.Add("default_host_password", this.NewPlayerVariable<string>("default_host_password", SystemConsole.StoreAs.Manual, "Default server password when hosting.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string @string = PlayerPrefs.GetString("HostPassword", "");
			this.Variable("default_host_password", ref @string, ref status, parameters);
			if (status.isDirty)
			{
				PlayerPrefs.SetString("HostPassword", @string);
			}
		}, ""));
		this.ConsoleCommands.Add("delete", this.NewPlayerCommand("delete", "Deletes a user-created variable.", "USAGE: $CMD$ <variable>\nDeletes a variable.  May only delete variables created with 'store_toggle', 'store_number', and 'store_text'.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text3 == null)
			{
				this.Log("You must provide a <variable>.", false);
				return;
			}
			SystemConsole.ConsoleCommand consoleCommand;
			this.ConsoleCommands.TryGetValue(text3, out consoleCommand);
			if (!this.CommandAvailable(consoleCommand))
			{
				this.Log(text3 + " does not exist.", false);
				return;
			}
			if (consoleCommand.origin != SystemConsole.CommandOrigin.UserVariable)
			{
				this.Log(text3 + " is not user-created!", false);
				return;
			}
			for (int k = this.ConsoleCommandList.Count - 1; k >= 0; k--)
			{
				if (this.ConsoleCommandList[k] == text3)
				{
					this.ConsoleCommandList.RemoveAt(k);
					break;
				}
			}
			this.ConsoleCommands.Remove(text3);
			this.UserBoolVars.Remove(text3);
			this.UserStringVars.Remove(text3);
		}, ""));
		this.ConsoleCommands.Add("double_click_container_grab", this.NewPlayerVariable<bool>("double_click_container_grab", SystemConsole.StoreAs.Bool, "When ON you must double-click to pick up a container (deck, stack, etc), rather than long-press.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("double_click_container_grab", ref Pointer.DOUBLE_CLICK_CONTAINER_PICKUP, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("dev_autoconfirm_browser_url_change", this.NewPlayerVariable<bool>("dev_autoconfirm_browser_url_change", "Browser objects will load all URLs without prompting.  Danger!", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("dev_autoconfirm_browser_url_change", ref TabletScript.AutoconfirmURLChange, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("dev_highlight_3d", this.NewAdminVariable<bool>("dev_highlight_3d", SystemConsole.StoreAs.Bool, "Use 3D object highlight system.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("dev_highlight_3d", ref NetworkPhysicsObject.UseParticleHighlights, ref status, parameters);
			if (status.isDirty)
			{
				HighlightingRenderer[] array = UnityEngine.Object.FindObjectsOfType<HighlightingRenderer>();
				for (int k = 0; k < array.Length; k++)
				{
					array[k].enabled = !NetworkPhysicsObject.UseParticleHighlights;
				}
				foreach (NetworkPhysicsObject networkPhysicsObject in NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs)
				{
					networkPhysicsObject.EnableParticleHighlights(NetworkPhysicsObject.UseParticleHighlights);
				}
			}
		}, ""));
		this.ConsoleCommands.Add("dev_highlight_opacity", this.NewPlayerVariable<float>("dev_highlight_opacity", SystemConsole.StoreAs.Float, "Opacity of highlighter when using 3D highlighting.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("dev_highlight_opacity", ref ParticleHighlight.HIGHLIGHT_3D_OPACITY, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("dev_highlight_scalar", this.NewPlayerVariable<float>("dev_highlight_scalar", SystemConsole.StoreAs.Float, "Scale multiplier of highlighter when using 3D highlighting.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("dev_highlight_scalar", ref ParticleHighlight.HIGHLIGHT_3D_SCALAR, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("dev_version_override", this.NewDeveloperVariable<string>("dev_version_override", "Game version string", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string versionNumber = NetworkSingleton<NetworkUI>.Instance.VersionNumber;
			this.Variable("dev_version_override", ref versionNumber, ref status, parameters);
			if (status.isDirty)
			{
				NetworkSingleton<NetworkUI>.Instance.VersionNumber = versionNumber;
				Network.version = versionNumber;
				this.DeferredExecuteCommand("dev_version_override");
			}
		}, ""));
		this.ConsoleCommands.Add("dev_start_hand_select_mode", this.NewDeveloperCommand("dev_start_hand_select_mode", "Start Hand Select Mode", "USAGE: $CMD$ {MAX_COUNT} {X}\nIf X is set then the cancel button will be displayed too.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			HandZone handZone = HandZone.GetHandZone(NetworkSingleton<PlayerManager>.Instance.MyPlayerState().stringColor, 0, true);
			if (!handZone)
			{
				return;
			}
			string text3;
			int maxCount;
			if (int.TryParse(text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0'), out maxCount))
			{
				text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			}
			else
			{
				maxCount = 0;
			}
			bool showCancel = text3.ToUpper() == "X";
			handZone.PushHandSelectMode(new HandSelectModeSettings("dev_test", showCancel, 0, maxCount, ""));
		}, ""));
		this.ConsoleCommands.Add("dev_stash_toggle", this.NewPlayerCommand("dev_stash_toggle", "On card in hand: move to stash. On stash: move to hand.", "USAGE: $CMD$ <guid>...\nIf <guid> is a card in hand, move it to stash. If <guid> is the stash, move it to hand.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool flag;
			List<NetworkPhysicsObject> list = this.ComponentHelper(status, parameters, out flag);
			if (list.Count == 0)
			{
				HandZone.GetHandZone(PlayerScript.PointerScript.PointerColorLabel, 0, true).MoveStashToHand();
				return;
			}
			foreach (NetworkPhysicsObject networkPhysicsObject in list)
			{
				if (networkPhysicsObject.CurrentPlayerHand)
				{
					networkPhysicsObject.CurrentPlayerHand.MoveToStash(networkPhysicsObject);
				}
			}
		}, ""));
		this.ConsoleCommands.Add("dev_toast", this.NewDeveloperCommand("dev_toast", "Show toast with provided message", "USAGE: $CMD$\n <message>\n", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			Singleton<UIToast>.Instance.AddJoinRequest(new User
			{
				Username = parameters
			});
		}, ""));
		this.ConsoleCommands.Add("dice_roll_height_multiplier", this.NewDeveloperVariable<float>("dice_roll_height_multiplier", SystemConsole.StoreAs.Float, "Sets how high the dice is thrown in the air when it is randomized.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("dice_roll_height_multiplier", ref ManagerPhysicsObject.DiceRollForceMultiplier, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("disconnect", this.NewPlayerCommand("disconnect", "Disconnect from current game.", "USAGE: $CMD$\nCease to host or be connected to table and return to main menu.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (Network.peerType == NetworkPeerMode.Disconnected)
			{
				this.Log("Not connected to a game.", false);
				return;
			}
			Network.Disconnect();
		}, ""));
		this.ConsoleCommands.Add("displays", this.NewPlayerCommand("displays", "Display information on currently connected displays.", "USAGE: $CMD$\nDisplay information on currently connected displays.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (!status.isSilent)
			{
				for (int k = 0; k < Display.displays.Length; k++)
				{
					this.Log(string.Format("Display {0}    {1}x{2}", k, Display.displays[k].renderingWidth, Display.displays[k].renderingHeight), false);
				}
			}
		}, ""));
		this.ConsoleCommands.Add("drawing_erase_all", this.NewAdminCommand("drawing_erase_all", "Erases all drawings.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			NetworkSingleton<ToolVector>.Instance.RemoveAll();
		}, ""));
		this.ConsoleCommands.Add("drawing_render_fully_visible", this.NewPlayerVariable<bool>("drawing_render_fully_visible", SystemConsole.StoreAs.Bool, "Render drawn lines without limiting against UI etc.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool renderMode = ToolVector.RENDER_QUEUE == 3000;
			this.Variable("drawing_render_fully_visible", ref renderMode, ref status, parameters);
			if (status.isDirty)
			{
				NetworkSingleton<ToolVector>.Instance.SetRenderMode(renderMode);
			}
		}, ""));
		this.ConsoleCommands.Add("echo", this.NewPlayerCommand("echo", "Echo text to the system console.", "USAGE: $CMD$ <text>\nDisplay the specified <text> in the System Console.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (!status.isSilent)
			{
				Chat.LogSystem(parameters, false);
			}
		}, ""));
		this.ConsoleCommands.Add("edit", this.NewPlayerCommand("edit", "Edit a text variable.", "USAGE: $CMD$ <variable>\nOpens specified text variable in GUI editor.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text3 == null)
			{
				this.Log("You must provide a text <variable>.", false);
				return;
			}
			SystemConsole.ConsoleCommand consoleCommand;
			this.ConsoleCommands.TryGetValue(text3, out consoleCommand);
			if (consoleCommand == null || consoleCommand.type != SystemConsole.CommandType.Variable || consoleCommand.variableType != typeof(string))
			{
				this.Log("You must provide a text <variable>.", false);
				return;
			}
			this.ProcessCommand(text3 + " -e", false, SystemConsole.CommandEcho.Silent);
		}, ""));
		this.ConsoleCommands.Add("end_turn", this.NewPlayerCommand("end_turn", "End your turn.", "USAGE: $CMD$\n", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (NetworkSingleton<Turns>.Instance.turnsState.Enable || NetworkSingleton<NetworkUI>.Instance.bHotseat)
			{
				NetworkSingleton<Turns>.Instance.GUIEndTurn();
			}
		}, ""));
		this.ConsoleCommands.Add("enhanced_base_precision", this.NewPlayerVariable<bool>("enhanced_base_precision", SystemConsole.StoreAs.Bool, "When ON the built-in models with circular bases will have more accurate colliders for the bases.\nThis will have a higher demand on performance.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("enhanced_base_precision", ref SystemConsole.EnhancedFigurinePrecision, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("errors_restrict_to_console", this.NewPlayerVariable<bool>("errors_restrict_to_console", "When ON global errors and warnings will only display in the system console.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("errors_restrict_to_console", ref SystemConsole.RestrictErrorsToConsole, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("errors_disable_broadcast", this.NewPlayerVariable<bool>("errors_disable_broadcast", "When ON global errors and warnings will not be broadcast.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("errors_disable_broadcast", ref SystemConsole.DisableErrorBroadcast, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("escape", this.NewPlayerCommand("escape", "Echo text variable to the system console escaped to avoid '[' formatting.", "USAGE: $CMD$ <variable>\nDisplay the specified <variable> in the System Console without applying any '[' format codes it contains.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (!status.isSilent)
			{
				string text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
				SystemConsole.ConsoleCommand consoleCommand;
				if (this.ConsoleCommands.TryGetValue(text3, out consoleCommand))
				{
					if (!this.CommandAvailable(consoleCommand) || consoleCommand.type != SystemConsole.CommandType.Variable)
					{
						this.Log("Unknown variable: " + text3, false);
						return;
					}
					SystemConsole.CommandStatus commandStatus = new SystemConsole.CommandStatus(SystemConsole.CommandEcho.Loud);
					commandStatus.Batch();
					this.ExecuteCommand(text3 + " -return", ref commandStatus, SystemConsole.CommandEcho.Silent);
					string text4 = (string)commandStatus.value;
					if (text4 != null)
					{
						Chat.LogSystem(Chat.EscapeFormatting(text4), false);
						return;
					}
				}
				else
				{
					this.Log("Unknown variable: " + text3, false);
				}
			}
		}, ""));
		this.ConsoleCommands.Add("eval", this.NewPlayerCommand("eval", "Evaluate statement.", "USAGE: $CMD$ <variable> <statement>\nEvaluate <statement> and store result in <variable>.  <statement> can include numeric variables, vector components, arithmetic operators, and these functions:\nabs, acos, asin, atan, atan2, ceil, cos, cosh, deg, exp, floor, fmod, frexp, ldexp, log, max, min, modf, pow, rad, random, sin, sinh, sqrt, tan, tanh, pi", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text3 == null)
			{
				this.Log("Specifiy <variable>", false);
				return;
			}
			SystemConsole.ConsoleCommand consoleCommand;
			if (!this.ConsoleCommands.TryGetValue(text3, out consoleCommand) || !this.CommandAvailable(consoleCommand) || consoleCommand.type != SystemConsole.CommandType.Variable || (!(consoleCommand.variableType == typeof(float)) && !(consoleCommand.variableType == typeof(int)) && !(consoleCommand.variableType == typeof(bool)) && !(consoleCommand.variableType == typeof(Vector2)) && !(consoleCommand.variableType == typeof(Vector3))))
			{
				this.Log("Specify toggle, number, or vector <variable>", false);
				return;
			}
			SystemConsole.CommandStatus commandStatus = new SystemConsole.CommandStatus(SystemConsole.CommandEcho.Loud);
			commandStatus.Batch();
			parameters = parameters.ToLower();
			string text4 = "";
			SystemConsole.EvalTokenType evalTokenType;
			string a;
			string text5 = SystemConsole.biteToken(ref parameters, out evalTokenType, out a);
			while (text5 != "")
			{
				if (evalTokenType == SystemConsole.EvalTokenType.Arithmetic)
				{
					text5 = text5.Replace("<>", "~=");
					text5 = text5.Replace("!=", "~=");
					text5 = text5.Replace("!", " not ");
					text5 = text5.Replace("&&", "&");
					text5 = text5.Replace("&", " and ");
					text5 = text5.Replace("||", "|");
					text5 = text5.Replace("|", " or ");
				}
				else if (evalTokenType == SystemConsole.EvalTokenType.Token)
				{
					if (this.EvalAllowedFunctions.Contains(text5))
					{
						text5 = "math." + text5;
					}
					else
					{
						if (!this.ConsoleCommands.TryGetValue(text5, out consoleCommand) || !this.CommandAvailable(consoleCommand) || consoleCommand.type != SystemConsole.CommandType.Variable || (!(consoleCommand.variableType == typeof(float)) && !(consoleCommand.variableType == typeof(int)) && !(consoleCommand.variableType == typeof(bool)) && !(consoleCommand.variableType == typeof(Vector2)) && !(consoleCommand.variableType == typeof(Vector3))))
						{
							this.Log("Invalid statement", false);
							return;
						}
						this.ExecuteCommand(text5 + " -return", ref commandStatus, SystemConsole.CommandEcho.Silent);
						if (consoleCommand.variableType == typeof(float))
						{
							text5 = LibString.StringFromFloat((float)commandStatus.value);
						}
						else if (consoleCommand.variableType == typeof(int))
						{
							text5 = ((int)commandStatus.value).ToString();
						}
						else if (consoleCommand.variableType == typeof(bool))
						{
							text5 = ((bool)commandStatus.value).ToString();
						}
						else if (consoleCommand.variableType == typeof(Vector2))
						{
							text5 = ((Vector2)commandStatus.value).ToString();
						}
						else
						{
							text5 = ((Vector3)commandStatus.value).ToString();
						}
					}
				}
				else
				{
					if (evalTokenType != SystemConsole.EvalTokenType.TokenWithComponent)
					{
						this.Log("Invalid statement", false);
						return;
					}
					if (!this.ConsoleCommands.TryGetValue(text5, out consoleCommand) || !this.CommandAvailable(consoleCommand) || consoleCommand.type != SystemConsole.CommandType.Variable || (!(consoleCommand.variableType == typeof(Vector2)) && !(consoleCommand.variableType == typeof(Vector3))) || (consoleCommand.variableType == typeof(Vector2) && a == "z"))
					{
						this.Log("Invalid statement", false);
						return;
					}
					this.ExecuteCommand(text5 + " -return", ref commandStatus, SystemConsole.CommandEcho.Silent);
					if (consoleCommand.variableType == typeof(Vector2))
					{
						if (a == "x")
						{
							text5 = LibString.StringFromFloat(((Vector2)commandStatus.value).x);
						}
						else
						{
							text5 = LibString.StringFromFloat(((Vector2)commandStatus.value).y);
						}
					}
					else if (a == "x")
					{
						text5 = LibString.StringFromFloat(((Vector3)commandStatus.value).x);
					}
					else if (a == "y")
					{
						text5 = LibString.StringFromFloat(((Vector3)commandStatus.value).y);
					}
					else
					{
						text5 = LibString.StringFromFloat(((Vector3)commandStatus.value).z);
					}
				}
				text4 += text5;
				text5 = SystemConsole.biteToken(ref parameters, out evalTokenType, out a);
			}
			DynValue dynValue;
			try
			{
				dynValue = LuaGlobalScriptManager.Instance.ExecuteScript("return (" + text4 + ")", true);
			}
			catch
			{
				this.Log("Invalid statement", false);
				return;
			}
			status.Batch();
			this.ExecuteCommand(text3 + " " + dynValue.ToString(), ref status, SystemConsole.CommandEcho.Quiet);
		}, ""));
		this.ConsoleCommands.Add("examine_position", this.NewPlayerVariable<Vector3>("examine_position", "Get position of examined component.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			Vector3 vector = Vector3.zero;
			if (this.examinedObject != null)
			{
				vector = this.examinedObject.transform.position;
			}
			status.ReadOnly();
			this.Variable("examine_position", ref vector, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("examine_rotation", this.NewPlayerVariable<Vector3>("examine_rotation", "Get rotation of examined component.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			Vector3 vector = Vector3.zero;
			if (this.examinedObject != null)
			{
				vector = this.examinedObject.transform.rotation.eulerAngles;
			}
			status.ReadOnly();
			this.Variable("examine_rotation", ref vector, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("exec", this.NewPlayerCommand("exec", "Execute a series of commands.", "USAGE: $CMD$ {-q} <commands> OR $CMD$ {-q} -v <string_variable>\nExecute a string of commands separated by ';', or each line in <string_variable>, one after another.\n   -q = quiet mode\n   -v = variable mode\nExtra commands available in a batch script:\n   exit = stop processing script\n   @    = use as prefix to command to silence it\n   @@   = toggle every subsequent command being silent\n   #    = comment\n   :    = prefix to assign a label\n   skip = see 'skip' command.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string remainder = string.Copy(parameters);
			string text3 = LibString.bite(ref remainder, true, ' ', false, false, '\0');
			SystemConsole.CommandEcho echo = this.echoMode;
			bool flag = false;
			while (text3 == "-q" || text3 == "-v")
			{
				if (text3 == "-q")
				{
					if (this.echoMode == SystemConsole.CommandEcho.Loud)
					{
						echo = SystemConsole.CommandEcho.Quiet;
					}
					else
					{
						echo = SystemConsole.CommandEcho.Loud;
					}
				}
				else
				{
					flag = !flag;
				}
				LibString.bite(ref parameters, false, ' ', false, false, '\0');
				text3 = LibString.bite(ref remainder, true, ' ', false, false, '\0');
			}
			if (status.isSilent)
			{
				echo = SystemConsole.CommandEcho.Silent;
			}
			if (text3 == null)
			{
				this.Log("Required argument missing.", false);
			}
			if (flag)
			{
				SystemConsole.ConsoleCommand consoleCommand;
				this.ConsoleCommands.TryGetValue(text3, out consoleCommand);
				string a = LibString.lookAhead(text3, false, ';', 0, false, false);
				string text4 = LibString.bite(ref remainder, true, ' ', false, false, '\0');
				if (a == text3 && text4 == null)
				{
					if (this.CommandAvailable(consoleCommand) && consoleCommand.type == SystemConsole.CommandType.Variable)
					{
						SystemConsole.CommandStatus commandStatus = new SystemConsole.CommandStatus(SystemConsole.CommandEcho.Loud);
						commandStatus.Batch();
						this.ExecuteCommand(text3 + " -return", ref commandStatus, SystemConsole.CommandEcho.Silent);
						this.ProcessBatch((string)commandStatus.value, echo);
						return;
					}
					this.Log("Unknown variable: " + text3, false);
					return;
				}
			}
			else
			{
				if (LibString.lookAhead(remainder, false, ' ', 0, false, false) == null)
				{
					text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
				}
				else
				{
					text3 = parameters;
				}
				this.ProcessBatch(text3, echo);
			}
		}, ""));
		this.ConsoleCommands.Add("exit", this.NewPlayerCommand("exit", "Exit batch script.", "USAGE: $CMD$ {<return value>}\nOnly applicable within a script.  Stops the script execution.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
		}, ""));
		this.ConsoleCommands.Add("fast_drag", this.NewPlayerVariable<bool>("fast_drag", "When ON activates code which attempts to reduce feeling of input lag when moving objects as client.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool enabled = FastDrag.Enabled;
			this.Variable("fast_drag", ref enabled, ref status, parameters);
			if (status.isDirty && enabled != FastDrag.Enabled)
			{
				FastDrag.Enabled = enabled;
				List<NetworkPhysicsObject> grabbableNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs;
				for (int k = 0; k < grabbableNPOs.Count; k++)
				{
					NetworkPhysicsObject networkPhysicsObject = grabbableNPOs[k];
					if (enabled)
					{
						if (networkPhysicsObject.fastDrag == null)
						{
							networkPhysicsObject.fastDrag = networkPhysicsObject.gameObject.AddComponent<FastDrag>();
						}
					}
					else if (networkPhysicsObject.fastDrag != null)
					{
						UnityEngine.Object.Destroy(networkPhysicsObject.fastDrag);
						networkPhysicsObject.fastDrag = null;
					}
				}
			}
		}, ""));
		this.ConsoleCommands.Add("find", this.NewAdminVariable<GameObject>("find", "Find a component.", "USAGE: $CMD$ OR $CMD$ {-c} {-name <text>} {-type <text>} {-desc <text>}\nWithout parameters will return last found component (use as <<find>> in the parameter of another command).\nWith parameters, sets itself to first component which matches all selectors.\n-c = case sensitive", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text3 = LibString.bite(ref parameters, true, ' ', false, false, '\0');
			string text4 = null;
			string text5 = null;
			string text6 = null;
			bool flag = false;
			while (text3 != null)
			{
				if (text3 == "-name")
				{
					text4 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
				}
				else if (text3 == "-type")
				{
					text5 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
				}
				else if (text3 == "-desc")
				{
					text6 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
				}
				else if (text3 == "-c")
				{
					flag = !flag;
				}
				else
				{
					if (text3 == "-return" || text3 == "-variable")
					{
						parameters = text3;
						break;
					}
					this.Log("Invalid argument: " + text3, false);
					return;
				}
				text3 = LibString.bite(ref parameters, true, ' ', false, false, '\0');
			}
			if (!flag)
			{
				if (text4 != null)
				{
					text4 = text4.ToLower();
				}
				if (text5 != null)
				{
					text5 = text5.ToLower();
				}
				if (text6 != null)
				{
					text6 = text6.ToLower();
				}
			}
			if (text4 != null || text5 != null || text6 != null)
			{
				string text7 = Colour.MyColorLabel() + " is running find:";
				if (text4 != null)
				{
					text7 = text7 + " Name=" + text4;
				}
				if (text5 != null)
				{
					text7 = text7 + " Type=" + text5;
				}
				if (text6 != null)
				{
					text7 = text7 + " Desc=" + text6;
				}
				LuaGlobalScriptManager.Instance.PrintToAll(text7, new Color?(Colour.Orange));
				List<NetworkPhysicsObject> grabbableNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs;
				this.foundObject = null;
				int k = 0;
				while (k < grabbableNPOs.Count)
				{
					NetworkPhysicsObject networkPhysicsObject = grabbableNPOs[k];
					if (flag)
					{
						if ((text4 == null || networkPhysicsObject.Name.Contains(text4)) && (text5 == null || networkPhysicsObject.tag.Contains(text5)))
						{
							if (text6 == null || networkPhysicsObject.Description.Contains(text6))
							{
								goto IL_20A;
							}
						}
					}
					else if ((text4 == null || networkPhysicsObject.Name.ToLower().Contains(text4)) && (text5 == null || networkPhysicsObject.tag.ToLower().Contains(text5)) && (text6 == null || networkPhysicsObject.Description.ToLower().Contains(text6)))
					{
						goto IL_20A;
					}
					k++;
					continue;
					IL_20A:
					this.foundObject = networkPhysicsObject;
					break;
				}
			}
			status.ReadOnly();
			string text8;
			if (this.foundObject)
			{
				text8 = this.foundObject.GUID;
			}
			else
			{
				text8 = "";
			}
			this.Variable("find", ref text8, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("file_browser_native", this.NewPlayerVariable<bool>("file_browser_native", SystemConsole.StoreAs.Bool, "When ON uses your OS's native file browser instead of the built in one. (VR will always use the built in file browser)", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("file_browser_native", ref UIFileBrowser.UseNativeFileBrowser, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("finder", this.NewDeveloperCommand("finder", "Searches using Finder for provided text.", "USAGE: $CMD$ <text>.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			int num = 0;
			foreach (FinderEntry finderEntry in Singleton<Finder>.Instance.Search(parameters))
			{
				this.Log("- " + ++num, false);
				this.LogVariable("Type", finderEntry.category.ToString(), Colour.Purple, 6);
				this.LogVariable("Target", this.GetPath(finderEntry.targetItem), Colour.Pink, 6);
				this.LogVariable("Window", this.GetPath(finderEntry.windowItem), Colour.Pink, 6);
				this.Log(finderEntry.keyText, Colour.UnityCyan, false);
			}
		}, ""));
		this.ConsoleCommands.Add("fog", this.NewPlayerVariable<bool>("fog", SystemConsole.StoreAs.Bool, "When ON activates fog effect.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("fog", ref SystemConsole.Fog, ref status, parameters);
			if (status.isDirty)
			{
				this.UpdateFog();
			}
		}, ""));
		this.ConsoleCommands.Add("framerate_custom_cap", this.NewPlayerVariable<int>("framerate_custom_cap", "When >0 will be used to cap the framerate, instead of the monitor's refresh rate (if Cap Framerate is enabled in Graphics config).", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("framerate_custom_cap", ref UIConfigGraphics.CurrentGraphics.CustomCapFPSRate, ref status, parameters);
			if (status.isDirty)
			{
				UIConfigGraphics.CurrentGraphics.ApplyFramerateCap();
			}
		}, ""));
		this.ConsoleCommands.Add("game_hotkey_bind", this.NewPlayerCommand("game_hotkey_bind", "Assign key to game-defined hotkey index.", "USAGE: $CMD$ <index> <key> {<key2>}\nAssign <key> to the game-defined hotkey specified by <index> (starts at 0).", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			int num;
			if (int.TryParse(LibString.bite(ref parameters, false, ' ', false, false, '\0'), out num))
			{
				string text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
				string text4 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
				if (text4 == null)
				{
					text4 = "";
				}
				text3 = LibString.CamelCaseFromUnderscore(text3, true, false);
				text4 = LibString.CamelCaseFromUnderscore(text4, true, false);
				UserDefinedHotkeyManager instance = NetworkSingleton<UserDefinedHotkeyManager>.Instance;
				if (instance.Hotkeys.Count > num)
				{
					UserDefinedHotkeyManager.HotkeyIdentifier hotkeyIdentifier = instance.Hotkeys[num];
					NetworkSingleton<UserDefinedHotkeyManager>.Instance.cInputSet(hotkeyIdentifier.cInputID, text3, text4);
					if (!status.isSilent)
					{
						Chat.LogSystem(string.Concat(new string[]
						{
							hotkeyIdentifier.label,
							" -> ",
							text3,
							" ",
							text4
						}), false);
					}
					NetworkSingleton<UserDefinedHotkeyManager>.Instance.WritePrefs();
					return;
				}
			}
			Chat.LogSystem("Invalid <index>", false);
		}, ""));
		this.ConsoleCommands.Add("game_hotkey_config_can_open", this.NewPlayerVariable<bool>("game_hotkey_config_can_open", SystemConsole.StoreAs.Bool, "When ON games may cause the [Options->Game Keys] settings window to show.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool flag = !UserDefinedHotkeyManager.PROHIBIT_USER_HOTKEY_POPUP;
			this.Variable("game_hotkey_config_can_open", ref flag, ref status, parameters);
			if (status.isDirty)
			{
				UserDefinedHotkeyManager.PROHIBIT_USER_HOTKEY_POPUP = !flag;
			}
		}, ""));
		this.ConsoleCommands.Add("game_hotkey_list", this.NewPlayerCommand("game_hotkey_list", "List game-defined hotkeys.", "USAGE: $CMD$\n", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (status.isSilent)
			{
				return;
			}
			UserDefinedHotkeyManager instance = NetworkSingleton<UserDefinedHotkeyManager>.Instance;
			if (instance.Hotkeys.Count == 0)
			{
				Chat.LogSystem("No game hotkeys.", false);
				return;
			}
			for (int k = 0; k < instance.Hotkeys.Count; k++)
			{
				UserDefinedHotkeyManager.HotkeyIdentifier hotkeyIdentifier = instance.Hotkeys[k];
				string text3 = cInput.GetText(hotkeyIdentifier.cInputID);
				this.LogVariable(hotkeyIdentifier.index + ": " + hotkeyIdentifier.label, text3, 37);
				string text4 = cInput.GetText(hotkeyIdentifier.cInputID, 2);
				if (text3 != "None" && text4 != "None")
				{
					this.LogVariable(hotkeyIdentifier.index + ": " + hotkeyIdentifier.label, text4, 37);
				}
			}
		}, ""));
		this.ConsoleCommands.Add("grabbed", this.NewPlayerVariable<string>("grabbed", "GUID of first grabbed object.", "Returns the GUID of the object grabbed by your pointer.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text3 = "";
			if (PlayerScript.PointerScript && PlayerScript.PointerScript.FirstGrabbedObject)
			{
				NetworkPhysicsObject component = PlayerScript.PointerScript.FirstGrabbedObject.GetComponent<NetworkPhysicsObject>();
				if (component && component.HeldByPlayerID == NetworkID.ID)
				{
					text3 = component.GUID;
				}
			}
			status.ReadOnly();
			this.Variable("grabbed", ref text3, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("group_into_bag_first", this.NewPlayerVariable<bool>("group_into_bag_first", SystemConsole.StoreAs.Bool, "When ON the group action will prioritize putting everything into a bag, if a single bag is present in the objects.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("group_into_bag_first", ref ManagerPhysicsObject.PrioritizeBagWhenGrouping, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("hand_component_hotkey_draw", this.NewPlayerVariable<bool>("hand_component_hotkey_draw", SystemConsole.StoreAs.Bool, "When ON pressing a number on a component which can be held in hand will draw it.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("hand_component_hotkey_draw", ref NetworkPhysicsObject.DrawByTypingNumbers, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("help", this.NewPlayerCommand("help", "Lists all available commands or use 'help <command>' for help on a specific command.", "USAGE: $CMD$ {<command> | -c}\nIf <command> is provided detailed help for it is displayed, otherwise all available commands are listed.\n -c = copy all help text to clipboard", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text3 = LibString.bite(ref parameters, true, ' ', false, false, '\0');
			if (text3 == "-c")
			{
				this.copyHelpToClipboard();
				this.Log("Help text copied to clipboard!", false);
				return;
			}
			if (status.isSilent)
			{
				return;
			}
			bool flag = false;
			string text4 = null;
			SystemConsole.ConsoleCommand consoleCommand = null;
			if (text3 == "all")
			{
				flag = true;
				text3 = null;
			}
			else if (text3 != null && (!this.ConsoleCommands.TryGetValue(text3, out consoleCommand) || !this.CommandAvailable(consoleCommand)))
			{
				text4 = text3;
				flag = true;
				text3 = null;
			}
			if (text3 == null)
			{
				for (int k = 0; k < this.ConsoleCommandList.Count; k++)
				{
					string text5 = this.ConsoleCommandList[k];
					if (text4 == null || text5.StartsWith(text4))
					{
						consoleCommand = this.ConsoleCommands[text5];
						if (this.CommandAvailable(consoleCommand))
						{
							if (flag)
							{
								this.Newline();
							}
							if (consoleCommand.storeAs != SystemConsole.StoreAs.DoNotStore)
							{
								this.Log(this.ConsoleCommandList[k] + "[98076D] [persistent]", Colour.Teal, false);
							}
							else
							{
								this.Log(this.ConsoleCommandList[k], Colour.Teal, false);
							}
							if (flag)
							{
								if (consoleCommand.defaults != "")
								{
									this.Log(string.Concat(new string[]
									{
										text5,
										" = ",
										consoleCommand.aliasOf,
										consoleCommand.defaults,
										"[-]"
									}), Colour.Grey, status.inBatch);
								}
								this.Log(consoleCommand.help, false);
							}
						}
					}
				}
				if (!flag)
				{
					this.Log("Use 'help all' for details on each command.", false);
					return;
				}
			}
			else
			{
				this.ConsoleCommands.TryGetValue(text3, out consoleCommand);
				if (!this.CommandAvailable(consoleCommand))
				{
					this.Log("Unknown command: " + text3, false);
					return;
				}
				this.Newline();
				if (consoleCommand.storeAs != SystemConsole.StoreAs.DoNotStore)
				{
					this.Log(text3 + "[98076D] [persistent]", Colour.Teal, false);
				}
				else
				{
					this.Log(text3, Colour.Teal, false);
				}
				if (consoleCommand.defaults != "")
				{
					this.Log(string.Concat(new string[]
					{
						text3,
						" = ",
						consoleCommand.aliasOf,
						consoleCommand.defaults,
						"[-]"
					}), Colour.Grey, status.inBatch);
				}
				this.Log(consoleCommand.help, false);
				this.Newline();
				string documentation = consoleCommand.documentation;
				string text6 = LibString.bite(ref documentation, false, ' ', false, false, '\0');
				text6 = LibString.bite(ref documentation, false, ' ', false, false, '\0');
				string text7 = LibString.bite(ref documentation, '\n');
				text7 = text7.Replace(" OR ", SystemConsole.OutputColour.RGBHex + " OR[-] ").Replace(text6, "[21B19B]" + text6 + "[-]");
				this.Log(string.Format("{0}USAGE: {1}{2} {3}{4}\n{5}{6}", new object[]
				{
					"[A64B00]",
					"[21B19B]",
					text6,
					"[B8B601]",
					text7,
					SystemConsole.OutputColour.RGBHex,
					documentation
				}), false);
			}
		}, ""));
		this.ConsoleCommands.Add("hidden_zone_hiding_opacity", this.NewPlayerVariable<float>("hidden_zone_hiding_opacity", SystemConsole.StoreAs.Float, "The opacity of hidden zones when you cannot see the objects inside them.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("hidden_zone_hiding_opacity", ref HiddenZone.OpacityWhenHiding, ref status, parameters);
			if (status.isDirty)
			{
				HiddenZone.OpacityWhenHiding = Mathf.Clamp01(HiddenZone.OpacityWhenHiding);
				HiddenZone.UpdateAllVisuals();
			}
		}, ""));
		this.ConsoleCommands.Add("hidden_zone_showing_opacity", this.NewPlayerVariable<float>("hidden_zone_showing_opacity", SystemConsole.StoreAs.Float, "The opacity of hidden zones when you can see the objects inside them.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("hidden_zone_showing_opacity", ref HiddenZone.OpacityWhenNotHiding, ref status, parameters);
			if (status.isDirty)
			{
				HiddenZone.OpacityWhenNotHiding = Mathf.Clamp01(HiddenZone.OpacityWhenNotHiding);
				HiddenZone.UpdateAllVisuals();
			}
		}, ""));
		this.ConsoleCommands.Add("highlight", this.NewAdminCommand("highlight", "Highlights specified component.", "USAGE: $CMD$ <GUID> {duration} {<color>}\nHighlights specified component.  You may specify a <duration> in seconds, and a <color>.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float duration = 3f;
			Colour pink = Colour.Pink;
			string text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text3 == null)
			{
				this.Log("Must specify <GUID>", false);
				return;
			}
			NetworkPhysicsObject networkPhysicsObject = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGUID(text3);
			if (networkPhysicsObject == null)
			{
				this.Log("Component not found.", false);
				return;
			}
			while (parameters != "")
			{
				string text4 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
				float num;
				if (float.TryParse(text4, out num))
				{
					duration = num;
				}
				else if (!Colour.TryColourFromLabel(LibString.CamelCaseFromUnderscore(text4.ToLower(), true, false), out pink))
				{
					this.Log("Could not parse argument: " + text4, false);
					return;
				}
			}
			networkPhysicsObject.LuaHighlightOn(pink, duration);
			LuaGlobalScriptManager.Instance.PrintToAll(Colour.MyColorLabel() + " highlighted 〔" + text3 + "〕", new Color?(Colour.Orange));
		}, ""));
		this.ConsoleCommands.Add("host_game", this.NewPlayerCommand("host_game", "Hosts a table.", "USAGE: $CMD$ {<seats> {<name> <password>}|{-h}} {-f}\nIf <seats> is one or missing then start a singleplayer server.\n\n<name> and <password> default to 'default_host_name' and 'default_host_password'.\n-f = force (disconnect if currently hosting or connected)\n-h = hotseat mode instead of multiplayer server.\n", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool hosting = true;
			int num = 0;
			string hostname = PlayerPrefs.GetString("HostServerName", "default server name");
			string password = PlayerPrefs.GetString("HostPassword", "");
			bool hotseat = false;
			bool flag = false;
			int num2 = 0;
			string text3 = "host";
			for (string text4 = LibString.bite(ref parameters, true, ' ', false, false, '\0'); text4 != null; text4 = LibString.bite(ref parameters, true, ' ', false, false, '\0'))
			{
				if (text4 == "-f")
				{
					if (status.inBatch)
					{
						this.Log("Cannot use -f inside batch script.", false);
						return;
					}
					flag = true;
				}
				else
				{
					if (text4.Contains(" "))
					{
						text3 = text3 + " \"" + text4 + "\"";
					}
					else
					{
						text3 = text3 + " " + text4;
					}
					if (text4 == "-h")
					{
						hotseat = true;
					}
					else
					{
						num2++;
						if (num2 == 1)
						{
							int.TryParse(text4, out num);
						}
						else if (num2 == 2)
						{
							hostname = text4;
						}
						else
						{
							if (num2 != 3)
							{
								this.Log("Wrong number of arguments.", false);
								return;
							}
							password = text4;
						}
					}
				}
			}
			if (num2 == 0)
			{
				num = 1;
			}
			if (num < 1 || num > 10)
			{
				this.Log("Seats must be 1-10", false);
				return;
			}
			if (Network.peerType == NetworkPeerMode.Disconnected)
			{
				status.Open();
				base.StartCoroutine(this.HostOrConnect(status, hosting, num, hotseat, hostname, password));
				return;
			}
			if (!flag)
			{
				this.Log("Already hosting or connected to a server.", false);
				return;
			}
			this.DeferredExecuteCommand(text3);
			Network.Disconnect();
		}, ""));
		this.ConsoleCommands.Add("host_max_players", this.NewAdminVariable<int>("host_max_players", "Current server max number of players.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			int maxConnections = NetworkSingleton<ServerOptions>.Instance.MaxConnections;
			this.Variable("host_max_players", ref maxConnections, ref status, parameters);
			if (status.isDirty && maxConnections > 0)
			{
				NetworkSingleton<ServerOptions>.Instance.MaxConnections = maxConnections;
			}
		}, ""));
		this.ConsoleCommands.Add("host_name", this.NewAdminVariable<string>("host_name", "Current server name.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text3 = NetworkSingleton<ServerOptions>.Instance.ServerName;
			if (text3 == null)
			{
				text3 = "";
			}
			this.Variable("host_name", ref text3, ref status, parameters);
			if (status.isDirty)
			{
				NetworkSingleton<ServerOptions>.Instance.ServerName = text3;
			}
		}, ""));
		this.ConsoleCommands.Add("host_password", this.NewAdminVariable<string>("host_password", "Current server password.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text3 = NetworkSingleton<ServerOptions>.Instance.Password;
			if (text3 == null)
			{
				text3 = "";
			}
			this.Variable("host_password", ref text3, ref status, parameters);
			if (status.isDirty)
			{
				NetworkSingleton<ServerOptions>.Instance.Password = text3;
			}
		}, ""));
		this.ConsoleCommands.Add("hotseat_ask_for_names", this.NewPlayerVariable<bool>("hotseat_ask_for_names", SystemConsole.StoreAs.Bool, "Ask for player names when Hotseat mode begins.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("hotseat_ask_for_names", ref Turns.HotseatAskForNames, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("hotseat_camera_reset", this.NewPlayerVariable<bool>("hotseat_camera_reset", SystemConsole.StoreAs.Bool, "Automatically reset the camera to the player's hand when the turn changes in Hotseat mode.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("hotseat_camera_reset", ref Turns.HotseatCameraReset, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("hotseat_end_turn", this.NewPlayerCommand("hotseat_end_turn", "End turn in Hotseat mode.", "USAGE: $CMD$\n", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (NetworkSingleton<NetworkUI>.Instance.bHotseat)
			{
				NetworkSingleton<Turns>.Instance.GUIEndTurn();
			}
		}, ""));
		this.ConsoleCommands.Add("hotseat_start_turn", this.NewPlayerCommand("hotseat_start_turn", "Confirm start of turn in Hotseat mode.", "USAGE: $CMD$\n", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			NetworkSingleton<NetworkUI>.Instance.GUIHotseatOK();
		}, ""));
		this.ConsoleCommands.Add("hotseat_turn_button", this.NewPlayerVariable<bool>("hotseat_turn_button", SystemConsole.StoreAs.Bool, "When ON the button showing the current player in Hotseat mode will be displayed.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("hotseat_turn_button", ref Turns.ShowHotseatTurnButton, ref status, parameters);
			if (status.isDirty)
			{
				NetworkSingleton<NetworkUI>.Instance.GUIEndTurn.SetActive(NetworkSingleton<NetworkUI>.Instance.bHotseat && Turns.ShowHotseatTurnButton);
			}
		}, ""));
		this.ConsoleCommands.Add("hotseat_turn_confirmation", this.NewPlayerVariable<bool>("hotseat_turn_confirmation", SystemConsole.StoreAs.Bool, "When ON you must confirm the start of your turn by clicking OK when playing in hotseat mode.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("hotseat_turn_confirmation", ref Turns.ConfirmHotseatTurnStart, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("hotseat_turn_delay", this.NewPlayerVariable<float>("hotseat_turn_delay", SystemConsole.StoreAs.Float, "Length of time in seconds after hitting End Turn before next player is activated in Hotseat mode.\nDisabled if hotseat_turn_confirmation is ON.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("hotseat_turn_delay", ref Turns.HotseatTurnInterval, ref status, parameters);
			if (status.isDirty && Turns.HotseatTurnInterval < 0f)
			{
				Turns.HotseatTurnInterval = 0f;
			}
		}, ""));
		for (int i = 1; i <= 8; i++)
		{
			int playerID = i;
			string cmd = "hotseat_name_" + i;
			string help = string.Format("Player {0}'s name in hotseat mode.", i);
			this.ConsoleCommands.Add(cmd, this.NewPlayerVariable<string>(cmd, help, delegate(SystemConsole.CommandStatus status, string parameters)
			{
				string text3 = "N/A";
				PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(playerID);
				if (NetworkSingleton<NetworkUI>.Instance.bHotseat && playerState != null)
				{
					text3 = playerState.name;
				}
				this.Variable(cmd, ref text3, ref status, parameters);
				if (status.isDirty)
				{
					text3 = text3.Trim();
					if (!NetworkSingleton<NetworkUI>.Instance.bHotseat)
					{
						this.Log("Not in Hotseat mode.", false);
						return;
					}
					if (playerState == null)
					{
						this.Log("No such player.", false);
						return;
					}
					if (text3 == "")
					{
						this.Log("Name cannot be blank.", false);
						return;
					}
					playerState.name = text3.Trim();
				}
			}, ""));
		}
		this.ConsoleCommands.Add("hovered", this.NewPlayerVariable<string>("hovered", "GUID of first object hovered over.", "Returns the GUID of the first object hovered over by your pointer.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text3 = "";
			if (HoverScript.HoverObject)
			{
				NetworkPhysicsObject component = HoverScript.HoverObject.GetComponent<NetworkPhysicsObject>();
				if (component)
				{
					text3 = component.GUID;
				}
			}
			status.ReadOnly();
			this.Variable("hovered", ref text3, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("irc_send", this.NewDeveloperVariable<bool>("irc_send", "When ON chat messages will be sent to IRC (disable when testing filters).", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("irc_send", ref ChatIRC.ENABLE_IRC_SEND, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("jigsaw_animate_box", this.NewPlayerVariable<bool>("jigsaw_animate_box", SystemConsole.StoreAs.Bool, "When ON jigsaw puzzle box image will be animated when appropriate.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("jigsaw_animate_box", ref CustomJigsawPuzzle.JIGSAW_ANIMATE_BOX, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("jigsaw_randomize", this.NewAdminCommand("jigsaw_randomize", "Randomizes the jigsaw puzzle.", "USAGE: $CMD$\nMoves all pieces to a random position.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			CustomJigsawPuzzle.Instance.Randomize(false);
		}, ""));
		this.ConsoleCommands.Add("jigsaw_solve", this.NewDeveloperCommand("jigsaw_solve", "Solves the jigsaw puzzle.", "USAGE: $CMD$\nSolves the jigsaw puzzle.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			CustomJigsawPuzzle.Instance.Solve();
		}, ""));
		this.ConsoleCommands.Add("jigsaw_validate", this.NewPlayerCommand("jigsaw_validate", "Validates the jigsaw puzzle.", "USAGE: $CMD$\nValidates the jigsaw puzzle.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			CustomJigsawPuzzle.Instance.Randomize(true);
		}, ""));
		this.ConsoleCommands.Add("joystick_names", this.NewPlayerCommand("joystick_names", "Lists joysticks.", "USAGE: $CMD$\nLists joysticks.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string[] joystickNames = TTSInput.GetJoystickNames();
			for (int k = 0; k < joystickNames.Length; k++)
			{
				Chat.LogSystem(joystickNames[k], false);
			}
		}, ""));
		this.ConsoleCommands.Add("keyboard_single_digit_by_default", this.NewPlayerVariable<bool>("keyboard_single_digit_by_default", SystemConsole.StoreAs.Bool, "When ON typing numbers on components will trigger the relevant action on the first digit typed, unless prefixed by a '0'", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("keyboard_single_digit_by_default", ref Pointer.DEFAULT_TYPED_NUMBER_IS_SINGLE_DIGIT, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("language", this.NewPlayerVariable<string>("language", SystemConsole.StoreAs.Manual, "Currently used language.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text3 = Language.CurrentLanguage;
			string a = LibString.lookAhead(parameters, true, ' ', 0, false, false);
			bool flag = a == "-l";
			if (!flag)
			{
				this.Variable("language", ref text3, ref status, parameters);
				if (a == "-return" || a == "-variable")
				{
					return;
				}
				if (status.isDirty)
				{
					float num;
					if (float.TryParse(text3, out num))
					{
						text3 = Language.OrderedLanguageCodes[(int)num % Language.OrderedLanguageCodes.Count];
					}
					if (Singleton<Language>.Instance.SetLanguage(text3, false))
					{
						if (LocalizationManager.CurrentLanguage != text3)
						{
							this.Log("-> " + LocalizationManager.CurrentLanguage, false);
							if (LocalizationManager.CurrentLanguage != "English")
							{
								this.Log("   " + "localized_language".ToString(), false);
							}
						}
					}
					else
					{
						this.Log("Unrecognized, reverting to: " + Language.CurrentLanguage, false);
						flag = true;
					}
				}
				else
				{
					this.Log("\nUse `language -l` to list all available languages, or instead type `language` and hit <tab> twice to see available commands.", false);
				}
			}
			if (flag)
			{
				this.Log("\nCurrently supported languages:\n", false);
				foreach (string text4 in LocalizationManager.GetAllLanguages(true))
				{
					this.Log(text4 + " (" + LocalizationManager.GetLanguageCode(text4) + ")", false);
				}
			}
		}, ""));
		for (int j = 0; j < Language.OrderedLanguageCodes.Count; j++)
		{
			string code = Language.OrderedLanguageCodes[j];
			string text = Language.LanguageFromCode[code];
			string text2 = "language_" + code.Replace("-", "_");
			string help2 = string.Concat(new string[]
			{
				"Use language: ",
				text,
				". (",
				code,
				"))"
			});
			this.ConsoleCommands.Add(text2, this.NewPlayerCommand(text2, help2, "Use `language` command variable to check current language.", delegate(SystemConsole.CommandStatus status, string parameters)
			{
				status.Batch();
				this.ExecuteCommand("language " + code, ref status, SystemConsole.CommandEcho.Quiet);
			}, ""));
		}
		this.ConsoleCommands.Add("last", this.NewPlayerVariable<string>("last", "Cannot be set; displays the last returned value.", "USAGE: $CMD$\nDisplays the last returned value. Usually used by reference (i.e. <<$CMD$>>)", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			status.value = SystemConsole.lastReturnedValue;
			string a = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (a == "-return")
			{
				return;
			}
			if (a == "-variable")
			{
				if (!status.isSilent)
				{
					this.LogVariable("last", SystemConsole.lastReturnedValue, Colour.YellowDark, 37);
				}
				return;
			}
			if (!status.isSilent)
			{
				if (SystemConsole.lastReturnedValue == "")
				{
					this.Log("last: [B8B601]\"\"", status.inBatch);
					return;
				}
				if (SystemConsole.lastReturnedValue.Contains("\n"))
				{
					this.Log("last is:\n[B8B601]" + SystemConsole.lastReturnedValue, status.inBatch);
					return;
				}
				this.Log("last: [B8B601]" + SystemConsole.lastReturnedValue, status.inBatch);
			}
		}, ""));
		this.ConsoleCommands.Add("lift_height", this.NewPlayerVariable<float>("lift_height", SystemConsole.StoreAs.Manual, "Lift Height when grabbing components.", "USAGE: $CMD$ <height>", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float raiseHeight = PlayerScript.PointerScript.RaiseHeight;
			this.Variable("lift_height", ref raiseHeight, ref status, parameters);
			if (status.isDirty)
			{
				PlayerScript.PointerScript.RaiseHeight = raiseHeight;
			}
		}, ""));
		this.ConsoleCommands.Add("load", this.NewAdminCommand("load", "Load a game.", "USAGE: $CMD$ <filename>\nLoad game stored as <filename>.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (!Network.isServer && !NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
			{
				return;
			}
			string text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			text3 = text3.Replace("..", "").Replace(":", "");
			if (text3 == "")
			{
				this.Log("Bad filename, could not load", false);
				return;
			}
			if (!text3.ToLower().EndsWith(".json"))
			{
				text3 += ".json";
			}
			text3 = DirectoryScript.saveFilePath + "//" + text3;
			this.Log(text3, false);
			if (SerializationScript.LoadJson(text3, true) == null)
			{
				this.Log("Could not load", false);
			}
		}, ""));
		this.ConsoleCommands.Add("log_display_tag", this.NewPlayerVariable<bool>("log_display_tag", "When ON displays the tag label before the log output.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("log_display_tag", ref this.logDisplayTag, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("log_format_concise", this.NewPlayerCommand("log_format_concise", "Sets the log formatting to concise.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			status.Batch();
			this.ExecuteCommand("log_output_format 1", ref status, SystemConsole.CommandEcho.Quiet);
		}, ""));
		this.ConsoleCommands.Add("log_format_expansive", this.NewPlayerCommand("log_format_expansive", "Sets the log formatting to expansive.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			status.Batch();
			this.ExecuteCommand("log_output_format 2", ref status, SystemConsole.CommandEcho.Quiet);
		}, ""));
		this.ConsoleCommands.Add("log_format_truncated", this.NewPlayerCommand("log_format_truncated", "Sets the log formatting to truncated.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			status.Batch();
			this.ExecuteCommand("log_output_format 0", ref status, SystemConsole.CommandEcho.Quiet);
		}, ""));
		this.ConsoleCommands.Add("log_output_format", this.NewPlayerVariable<int>("log_output_format", "Controls how verbose logging is", "USAGE: $CMD$ <format>\n<format> may be one of:\n 0 = Truncated\n 1 = Concise\n 2 = Expansive", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			int num;
			if (this.logFormat == SystemConsole.LogFormat.Truncated)
			{
				num = 0;
			}
			else if (this.logFormat == SystemConsole.LogFormat.Concise)
			{
				num = 1;
			}
			else
			{
				num = 2;
			}
			this.Variable("log_output_format", ref num, ref status, parameters);
			if (status.isDirty)
			{
				if (num == 0)
				{
					this.logFormat = SystemConsole.LogFormat.Truncated;
					return;
				}
				if (num == 1)
				{
					this.logFormat = SystemConsole.LogFormat.Concise;
					return;
				}
				if (num == 2)
				{
					this.logFormat = SystemConsole.LogFormat.Expansive;
					return;
				}
				this.Log("Must be 0, 1 or 2: resetting to previous value.", false);
			}
		}, ""));
		this.ConsoleCommands.Add("log_max_table_depth", this.NewPlayerVariable<int>("log_max_table_depth", "Sets maximum depth a table in a log entry will display to.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("log_max_table_depth", ref this.logTableDepth, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("log_style_default", this.NewPlayerVariable<SystemConsole.LogTag>("log_style_default", "Sets default styling of log entries.", "USAGE: $CMD$ <color> <prefix> <postfix>\n<color> can be a named color or #RRGGBB.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.LogFormatHelper("log_style_default", ref this.DefaultLogTag, ref status, ref parameters);
		}, ""));
		this.ConsoleCommands.Add("log_style_highlight", this.NewPlayerVariable<SystemConsole.LogTag>("log_style_highlight", "Sets highlight styling of log entries.", "USAGE: $CMD$ <color> <prefix> <postfix>\n<color> can be a named color or #RRGGBB.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.LogFormatHelper("log_style_highlight", ref this.HighlightTag, ref status, ref parameters);
		}, ""));
		this.ConsoleCommands.Add("log_style_tag", this.NewPlayerCommand("log_style_tag", "Sets the style of a log tag.  Can also be done in Lua with logStyle.", "USAGE: $CMD$ <name> <color> <prefix> <postfix>\n<color> can be a named color or #RRGGBB.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text3 = LibString.bite(ref parameters, true, ' ', false, false, '\0');
			if (text3 == null)
			{
				this.Log("Must specify a tag name.", false);
				return;
			}
			if (LibString.lookAhead(parameters, false, ' ', 0, false, false) == null)
			{
				this.Log("Must specify a color.", false);
				return;
			}
			SystemConsole.LogTag value = new SystemConsole.LogTag(text3, this.DefaultLogTag.colour, this.DefaultLogTag.prefix, this.DefaultLogTag.postfix);
			this.LogFormatHelper("log_style_tag", ref value, ref status, ref parameters);
			if (status.value != null)
			{
				this.LogTags[text3] = value;
			}
		}, ""));
		this.ConsoleCommands.Add("log_tags_clear", this.NewPlayerCommand("log_tags_clear", "Remove all defined log tags.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.LogTags.Clear();
			if (!status.isSilent)
			{
				this.Log("All log tags removed.", false);
			}
		}, ""));
		this.ConsoleCommands.Add("log_tags_display", this.NewPlayerCommand("log_tags_display", "Display all defined log tags.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			SystemConsole.CommandStatus commandStatus = new SystemConsole.CommandStatus(SystemConsole.CommandEcho.Loud);
			commandStatus.Batch();
			commandStatus.echo = status.echo;
			string text3 = "-variable";
			this.LogFormatHelper("<default>", ref this.DefaultLogTag, ref commandStatus, ref text3);
			text3 = "-variable";
			this.LogFormatHelper("<highlight>", ref this.HighlightTag, ref commandStatus, ref text3);
			foreach (string key in this.LogTags.Keys)
			{
				SystemConsole.LogTag logTag = this.LogTags[key];
				text3 = "-variable";
				this.LogFormatHelper(logTag.name, ref logTag, ref commandStatus, ref text3);
			}
		}, ""));
		this.ConsoleCommands.Add("log_tags_exclude", this.NewPlayerVariable<string>("log_tags_exclude", "When non-empty any log entries which match an excluded tag will not be displayed.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string input = string.Join(" ", this.LogTagsExcluded.ToArray()).ToLower();
			this.Variable("log_tags_exclude", ref input, ref status, parameters);
			if (status.isDirty)
			{
				this.LogTagsHelper(ref this.LogTagsExcluded, input);
			}
		}, ""));
		this.ConsoleCommands.Add("log_tags_highlight", this.NewPlayerVariable<string>("log_tags_highlight", "When non-empty any log entry being displayed which matches a hilighted tag will be styled as such.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string input = string.Join(" ", this.LogTagsHighlight.ToArray()).ToLower();
			this.Variable("log_tags_highlight", ref input, ref status, parameters);
			if (status.isDirty)
			{
				this.LogTagsHelper(ref this.LogTagsHighlight, input);
			}
		}, ""));
		this.ConsoleCommands.Add("log_tags_include", this.NewPlayerVariable<string>("log_tags_include", "When non-empty only log entries which match an included tag will be displayed.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string input = string.Join(" ", this.LogTagsIncluded.ToArray()).ToLower();
			this.Variable("log_tags_include", ref input, ref status, parameters);
			if (status.isDirty)
			{
				this.LogTagsHelper(ref this.LogTagsIncluded, input);
			}
		}, ""));
		this.ConsoleCommands.Add("lua", this.NewAdminCommand("lua", "Execute lua statement.", "USAGE: $CMD$ <statement>", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			LuaGlobalScriptManager.Instance.RPCExecuteScript(parameters);
		}, ""));
		this.ConsoleCommands.Add("magnify_zoom_size", this.NewPlayerVariable<float>("magnify_zoom_size", SystemConsole.StoreAs.Float, "Size of the magnify zoom based on the screen percent of the resolution height.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("magnify_zoom_size", ref MagnifyCamera.SCREEN_PERCENT, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<MagnifyCamera>.Instance.Reset();
			}
		}, ""));
		this.ConsoleCommands.Add("measure_arrows_angle", this.NewPlayerVariable<float>("measure_arrows_angle", SystemConsole.StoreAs.Float, "Angle of arrowhead prongs on Line Tool.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("measure_arrows_angle", ref Pointer.MeasureToolArrowHeadAngle, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("measure_arrows_length", this.NewPlayerVariable<float>("measure_arrows_length", SystemConsole.StoreAs.Float, "Lengh of arrowhead prongs on Line Tool.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("measure_arrows_length", ref Pointer.LineToolArrowHeadLength, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("measure_components", this.NewPlayerVariable<bool>("measure_components", SystemConsole.StoreAs.Bool, "When ON moving a component with the Measure Movement toggle will automatically begin a measurement.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("measure_components", ref LineScript.MEASURE_OBJECTS, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<LineScript>.Instance.UpdateModeButton();
			}
		}, ""));
		this.ConsoleCommands.Add("measure_flat", this.NewPlayerVariable<bool>("measure_flat", SystemConsole.StoreAs.Bool, "When ON measurements are flattened to the horizontal plane.  When OFF height above the table is included.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("measure_flat", ref LineScript.MEASURE_FLAT, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<LineScript>.Instance.UpdateFlatButton();
			}
		}, ""));
		this.ConsoleCommands.Add("measure_font_size", this.NewPlayerVariable<float>("measure_font_size", SystemConsole.StoreAs.Float, "Size of displayed measurement when using Line Tool.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float num = (float)Pointer.LineToolFontSize;
			this.Variable("measure_font_size", ref num, ref status, parameters);
			if (status.isDirty)
			{
				Pointer.LineToolFontSize = Mathf.Min(256, Mathf.Max(14, (int)num));
			}
		}, ""));
		this.ConsoleCommands.Add("measure_inch_multiplier", this.NewPlayerVariable<float>("measure_inch_multiplier", "Global multiplier applied when measuring in inches.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("measure_inch_multiplier", ref LineScript.MEASURE_MULTIPLIER, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("measure_using_grid", this.NewPlayerVariable<bool>("measure_using_grid", SystemConsole.StoreAs.Bool, "When ON measurements are done using grid size, rather than inches or cm.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("measure_using_grid", ref LineScript.GRID_MEASUREMENTS, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<LineScript>.Instance.UpdateUnitButton();
			}
		}, ""));
		this.ConsoleCommands.Add("measure_hovered_from_edge", this.NewPlayerVariable<bool>("measure_hovered_from_edge", SystemConsole.StoreAs.Bool, "When ON and you start measuring over a hovered object, the measurement will be from its edge (instead of its center).", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("measure_hovered_from_edge", ref LineScript.MEASURE_HOVERED_FROM_EDGE, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<LineScript>.Instance.UpdateHoverButton();
			}
		}, ""));
		this.ConsoleCommands.Add("measure_in_metric", this.NewPlayerVariable<bool>("measure_in_metric", SystemConsole.StoreAs.Bool, "When ON measurements are converted to cm.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("measure_in_metric", ref LineScript.METRIC_MEASUREMENTS, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<LineScript>.Instance.UpdateUnitButton();
			}
		}, ""));
		this.ConsoleCommands.Add("measure_logging", this.NewPlayerVariable<bool>("measure_logging", SystemConsole.StoreAs.Bool, "When ON all player measurements are logged.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("measure_logging", ref LineScript.LOG_MEASUREMENTS, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<LineScript>.Instance.UpdateLogButton();
			}
		}, ""));
		this.ConsoleCommands.Add("mouse_shake_threshold", this.NewPlayerVariable<int>("mouse_shake_threshold", SystemConsole.StoreAs.Int, "Determines amount of jostling mouse requires to 'shake' held component.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("mouse_shake_threshold", ref ManagerPhysicsObject.ShakeThreshold, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("mouse_wheel_zoom_and_center", this.NewPlayerVariable<bool>("mouse_wheel_zoom_and_center", SystemConsole.StoreAs.Bool, "When ON the camera will center on the mouse pointer as you use it to zoom.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("mouse_wheel_zoom_and_center", ref CameraController.WHEEL_CENTER_ON_ZOOM, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("mirror_all", this.NewPlayerVariable<bool>("mirror_all", "When ON mirrors to the system console any messages sent to ALL tabs.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool value = this.mirrorTab[ChatMessageType.All];
			this.Variable("mirror_all", ref value, ref status, parameters);
			if (status.isDirty)
			{
				this.mirrorTab[ChatMessageType.All] = value;
			}
		}, ""));
		this.ConsoleCommands.Add("mirror_game", this.NewPlayerVariable<bool>("mirror_game", "When ON mirrors to the system console any messages sent to the GAME tab.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool value = this.mirrorTab[ChatMessageType.Game];
			this.Variable("mirror_game", ref value, ref status, parameters);
			if (status.isDirty)
			{
				this.mirrorTab[ChatMessageType.Game] = value;
			}
		}, ""));
		this.ConsoleCommands.Add("mirror_global", this.NewPlayerVariable<bool>("mirror_global", "When ON mirrors to the system console any messages sent to the GLOBAL tab.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool value = this.mirrorTab[ChatMessageType.Global];
			this.Variable("mirror_global", ref value, ref status, parameters);
			if (status.isDirty)
			{
				this.mirrorTab[ChatMessageType.Global] = value;
			}
		}, ""));
		this.ConsoleCommands.Add("mirror_team", this.NewPlayerVariable<bool>("mirror_team", "When ON mirrors to the system console any messages sent to the TEAM tab.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool value = this.mirrorTab[ChatMessageType.Team];
			this.Variable("mirror_team", ref value, ref status, parameters);
			if (status.isDirty)
			{
				this.mirrorTab[ChatMessageType.Team] = value;
			}
		}, ""));
		this.ConsoleCommands.Add("multiply", this.NewPlayerCommand("multiply", "Multiplies a numerical variable.", "USAGE: $CMD$ <variable> <value>\nSets <variable> to its current value * <value>.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text3 == null)
			{
				this.Log("You must provide a numeric <variable>.", false);
				return;
			}
			SystemConsole.ConsoleCommand consoleCommand;
			this.ConsoleCommands.TryGetValue(text3, out consoleCommand);
			if (consoleCommand == null || consoleCommand.type != SystemConsole.CommandType.Variable || (consoleCommand.variableType != typeof(float) && consoleCommand.variableType != typeof(int)))
			{
				this.Log("You must provide a numeric <variable>.", false);
				return;
			}
			float num;
			if (!float.TryParse(LibString.bite(ref parameters, false, ' ', false, false, '\0'), out num))
			{
				this.Log("Could not parse numeric <value>", false);
				return;
			}
			SystemConsole.CommandStatus commandStatus = new SystemConsole.CommandStatus(SystemConsole.CommandEcho.Loud);
			commandStatus.Batch();
			this.ExecuteCommand(text3 + " -return", ref commandStatus, SystemConsole.CommandEcho.Silent);
			float num2;
			if (commandStatus.value.GetType() == typeof(int))
			{
				num2 = (float)((int)commandStatus.value);
			}
			else
			{
				num2 = (float)commandStatus.value;
			}
			num2 *= num;
			status.Batch();
			this.ExecuteCommand(text3 + " " + num2, ref status, SystemConsole.CommandEcho.Quiet);
		}, ""));
		this.ConsoleCommands.Add("music_add", this.NewPlayerCommand("music_add", "Add song to playlist.", "USAGE: $CMD$ <url> {<name>}\nImport music file at <url> and add it to playlist.\n-p = Play it.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text3 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text3 == null)
			{
				this.Log("Specify URL", false);
				return;
			}
			NetworkSingleton<CustomMusicPlayer>.Instance.AddAudio(parameters, text3);
		}, ""));
		this.ConsoleCommands.Add("music_mute", this.NewPlayerVariable<bool>("music_mute", "Mute music.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool muteAudio = Singleton<UICustomMusicPlayer>.Instance.MuteAudio;
			this.Variable("music_mute", ref muteAudio, ref status, parameters);
			if (status.isDirty && muteAudio != Singleton<UICustomMusicPlayer>.Instance.MuteAudio)
			{
				Singleton<UICustomMusicPlayer>.Instance.ToggleMuteAudio();
			}
		}, ""));
		this.ConsoleCommands.Add("music_next", this.NewPlayerCommand("music_next", "Play next track.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			NetworkSingleton<CustomMusicPlayer>.Instance.SkipEnd();
		}, ""));
		this.ConsoleCommands.Add("music_pause", this.NewPlayerCommand("music_pause", "Pause music.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (NetworkSingleton<CustomMusicPlayer>.Instance.PlayStatus == CustomMusicPlayer.MusicPlayerStatus.Play)
			{
				NetworkSingleton<CustomMusicPlayer>.Instance.PlayPause();
			}
		}, ""));
		this.ConsoleCommands.Add("music_play", this.NewPlayerCommand("music_play", "Play music.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (NetworkSingleton<CustomMusicPlayer>.Instance.PlayStatus == CustomMusicPlayer.MusicPlayerStatus.Pause || NetworkSingleton<CustomMusicPlayer>.Instance.PlayStatus == CustomMusicPlayer.MusicPlayerStatus.Ready)
			{
				NetworkSingleton<CustomMusicPlayer>.Instance.PlayPause();
			}
		}, ""));
		this.ConsoleCommands.Add("music_prev", this.NewPlayerCommand("music_prev", "Play previous track.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			NetworkSingleton<CustomMusicPlayer>.Instance.SkipStart();
		}, ""));
		this.ConsoleCommands.Add("music_repeat", this.NewPlayerVariable<bool>("music_repeat", "Music player song repeat.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool repeatSong = NetworkSingleton<CustomMusicPlayer>.Instance.RepeatSong;
			this.Variable("music_repeat", ref repeatSong, ref status, parameters);
			if (status.isDirty)
			{
				NetworkSingleton<CustomMusicPlayer>.Instance.RepeatSong = repeatSong;
			}
		}, ""));
		this.ConsoleCommands.Add("music_shuffle", this.NewPlayerVariable<bool>("music_shuffle", "Music player playlist shuffle.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool shuffleAudio = NetworkSingleton<CustomMusicPlayer>.Instance.ShuffleAudio;
			this.Variable("music_shuffle", ref shuffleAudio, ref status, parameters);
			if (status.isDirty)
			{
				NetworkSingleton<CustomMusicPlayer>.Instance.ShuffleAudio = shuffleAudio;
			}
		}, ""));
		this.ConsoleCommands.Add("music_timecode", this.NewPlayerVariable<float>("music_timecode", "Current timecode of music (seconds).", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float time = NetworkSingleton<CustomMusicPlayer>.Instance.AudioSource.time;
			this.Variable("music_timecode", ref time, ref status, parameters);
			if (status.isDirty)
			{
				NetworkSingleton<CustomMusicPlayer>.Instance.SkipTo(time);
			}
		}, ""));
		this.ConsoleCommands.Add("music_volume", this.NewPlayerVariable<float>("music_volume", "The volume of the music player.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float volume = NetworkSingleton<CustomMusicPlayer>.Instance.Volume;
			this.Variable("music_volume", ref volume, ref status, parameters);
			if (status.isDirty)
			{
				NetworkSingleton<CustomMusicPlayer>.Instance.SetVolume(Mathf.Clamp(volume, 0f, 1f));
				Singleton<UICustomMusicPlayer>.Instance.UpdateVolumeSlider();
			}
		}, ""));
	}

	// Token: 0x06001D90 RID: 7568 RVA: 0x000CEB70 File Offset: 0x000CCD70
	private void CreateCommandsNZ()
	{
		this.ConsoleCommands.Add("negative_typed_numbers", this.NewPlayerVariable<bool>("negative_typed_numbers", SystemConsole.StoreAs.Bool, "When ON pressing `Minus` before typing a number on component will enter a negative number.  Remember to unbind the `Minus` key from Scale Down!", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("negative_typed_numbers", ref NetworkPhysicsObject.AllowTypingNegativeNumbers, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("playerpref_float", this.NewDeveloperCommand("playerpref_float", "Display or set float PlayerPref.", "USAGE: $CMD$ <key> [<value>]\nDisplays playerpref matching <key>.  If <value> specified then sets it to that value as a float first.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.PlayerPrefHelper(status, parameters, typeof(float));
		}, ""));
		this.ConsoleCommands.Add("playerpref_int", this.NewDeveloperCommand("playerpref_int", "Display or set int PlayerPref.", "USAGE: $CMD$ <key> {<value>}\nDisplays playerpref matching <key>.  If <value> specified then sets it to that value as an int first.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.PlayerPrefHelper(status, parameters, typeof(int));
		}, ""));
		this.ConsoleCommands.Add("playerpref_string", this.NewDeveloperCommand("playerpref_string", "Display or set string PlayerPref.", "USAGE: $CMD$ <key> {<value>}\nDisplays playerpref matching <key>.  If <value> specified then sets it to that value as a string first.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.PlayerPrefHelper(status, parameters, typeof(string));
		}, ""));
		this.ConsoleCommands.Add("pleb_mode", this.NewDeveloperVariable<bool>("pleb_mode", "When turned ON removes your DEVELOPER status.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("pleb_mode", ref SystemConsole.plebMode, ref status, parameters);
			if (SystemConsole.plebMode)
			{
				this.isDeveloper = false;
				for (int k = this.ConsoleCommandList.Count - 1; k >= 0; k--)
				{
					string key = this.ConsoleCommandList[k];
					if (this.ConsoleCommands[key].permission == SystemConsole.CommandPermission.Developer)
					{
						this.ConsoleCommands.Remove(key);
						this.ConsoleCommandList.RemoveAt(k);
					}
				}
				if (!status.isSilent)
				{
					this.Log("Developer status removed!", false);
				}
			}
		}, ""));
		this.ConsoleCommands.Add("pointer_position", this.NewPlayerVariable<Vector3>("pointer_position", "Get position of player pointer.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			Vector3 position = PlayerScript.PointerScript.transform.position;
			position.y += 1.5f;
			status.ReadOnly();
			this.Variable("pointer_position", ref position, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("pure_fog", this.NewPlayerVariable<bool>("pure_fog", SystemConsole.StoreAs.Bool, "When ON display floor mist when in Pure Mode.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("pure_fog", ref TableScript.PURE_FOG, ref status, parameters);
			if (status.isDirty)
			{
				this.UpdateFog();
			}
		}, ""));
		this.ConsoleCommands.Add("pure_mode", this.NewPlayerVariable<bool>("pure_mode", SystemConsole.StoreAs.Bool, "When ON use Pure Mode visual style, colours for which can be set in the Theme Editor.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("pure_mode", ref TableScript.PURE_MODE, ref status, parameters);
			if (status.isDirty)
			{
				this.UpdateFog();
			}
		}, ""));
		this.ConsoleCommands.Add("pure_override_custom_table", this.NewPlayerVariable<bool>("pure_override_custom_table", SystemConsole.StoreAs.Bool, "When ON images on custom tables will be hidden while in Pure Mode.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("pure_override_custom_table", ref TableScript.PURE_HIDE_CUSTOM, ref status, parameters);
			if (status.isDirty)
			{
				TableScript.UpdatePureMode();
			}
		}, ""));
		this.ConsoleCommands.Add("pure_specular_intensity_a", this.NewPlayerVariable<float>("pure_specular_intensity_a", SystemConsole.StoreAs.Float, "The specular intensity of the primary table color when in Pure Mode (0.0 - 1.0).", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float pure_PRIMARY_SPECULAR_INTENSITY = TableScript.PURE_PRIMARY_SPECULAR_INTENSITY;
			this.Variable("pure_specular_intensity_a", ref pure_PRIMARY_SPECULAR_INTENSITY, ref status, parameters);
			if (status.isDirty)
			{
				if (pure_PRIMARY_SPECULAR_INTENSITY < 0f || pure_PRIMARY_SPECULAR_INTENSITY > 1f)
				{
					this.Log("Must be in range 0.0 - 1.0", false);
					return;
				}
				TableScript.PURE_PRIMARY_SPECULAR_INTENSITY = pure_PRIMARY_SPECULAR_INTENSITY;
				this.PureModePrimaryMaterial.SetFloat("_SpecInt", pure_PRIMARY_SPECULAR_INTENSITY);
				TableScript.UpdatePureMode();
			}
		}, ""));
		this.ConsoleCommands.Add("pure_specular_intensity_b", this.NewPlayerVariable<float>("pure_specular_intensity_b", SystemConsole.StoreAs.Float, "The specular intensity of the secondary table color when in Pure Mode (0.0 - 1.0).", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float pure_SECONDARY_SPECULAR_INTENSITY = TableScript.PURE_SECONDARY_SPECULAR_INTENSITY;
			this.Variable("pure_specular_intensity_b", ref pure_SECONDARY_SPECULAR_INTENSITY, ref status, parameters);
			if (status.isDirty)
			{
				if (pure_SECONDARY_SPECULAR_INTENSITY < 0f || pure_SECONDARY_SPECULAR_INTENSITY > 1f)
				{
					this.Log("Must be in range 0.0 - 1.0", false);
					return;
				}
				TableScript.PURE_SECONDARY_SPECULAR_INTENSITY = pure_SECONDARY_SPECULAR_INTENSITY;
				this.PureModeSecondaryMaterial.SetFloat("_SpecInt", pure_SECONDARY_SPECULAR_INTENSITY);
				TableScript.UpdatePureMode();
			}
		}, ""));
		this.ConsoleCommands.Add("pure_specular_intensity_splash", this.NewPlayerVariable<float>("pure_specular_intensity_splash", SystemConsole.StoreAs.Float, "The specular intensity of the splash table color when in Pure Mode (0.0 - 1.0).", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float pure_SPLASH_SPECULAR_INTENSITY = TableScript.PURE_SPLASH_SPECULAR_INTENSITY;
			this.Variable("pure_specular_intensity_splash", ref pure_SPLASH_SPECULAR_INTENSITY, ref status, parameters);
			if (status.isDirty)
			{
				if (pure_SPLASH_SPECULAR_INTENSITY < 0f || pure_SPLASH_SPECULAR_INTENSITY > 1f)
				{
					this.Log("Must be in range 0.0 - 1.0", false);
					return;
				}
				TableScript.PURE_SPLASH_SPECULAR_INTENSITY = pure_SPLASH_SPECULAR_INTENSITY;
				this.PureModeSecondaryMaterial.SetFloat("_SpecInt", pure_SPLASH_SPECULAR_INTENSITY);
				TableScript.UpdatePureMode();
			}
		}, ""));
		this.ConsoleCommands.Add("pure_specular_sharpness_a", this.NewPlayerVariable<float>("pure_specular_sharpness_a", SystemConsole.StoreAs.Float, "The specular sharpness of the primary table color when in Pure Mode (2.0 - 8.0).", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float pure_PRIMARY_SPECULAR_SHARPNESS = TableScript.PURE_PRIMARY_SPECULAR_SHARPNESS;
			this.Variable("pure_specular_sharpness_a", ref pure_PRIMARY_SPECULAR_SHARPNESS, ref status, parameters);
			if (status.isDirty)
			{
				if (pure_PRIMARY_SPECULAR_SHARPNESS < 2f || pure_PRIMARY_SPECULAR_SHARPNESS > 8f)
				{
					this.Log("Must be in range 2.0 - 8.0", false);
					return;
				}
				TableScript.PURE_PRIMARY_SPECULAR_SHARPNESS = pure_PRIMARY_SPECULAR_SHARPNESS;
				this.PureModePrimaryMaterial.SetFloat("_Shininess", pure_PRIMARY_SPECULAR_SHARPNESS);
				TableScript.UpdatePureMode();
			}
		}, ""));
		this.ConsoleCommands.Add("pure_specular_sharpness_b", this.NewPlayerVariable<float>("pure_specular_sharpness_b", SystemConsole.StoreAs.Float, "The specular sharness of the secondary table color when in Pure Mode (2.0 - 8.0).", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float pure_SECONDARY_SPECULAR_SHARPNESS = TableScript.PURE_SECONDARY_SPECULAR_SHARPNESS;
			this.Variable("pure_specular_sharpness_b", ref pure_SECONDARY_SPECULAR_SHARPNESS, ref status, parameters);
			if (status.isDirty)
			{
				if (pure_SECONDARY_SPECULAR_SHARPNESS < 2f || pure_SECONDARY_SPECULAR_SHARPNESS > 8f)
				{
					this.Log("Must be in range 2.0 - 8.0", false);
					return;
				}
				TableScript.PURE_SECONDARY_SPECULAR_SHARPNESS = pure_SECONDARY_SPECULAR_SHARPNESS;
				this.PureModeSecondaryMaterial.SetFloat("_Shininess", pure_SECONDARY_SPECULAR_SHARPNESS);
				TableScript.UpdatePureMode();
			}
		}, ""));
		this.ConsoleCommands.Add("pure_specular_sharpness_splash", this.NewPlayerVariable<float>("pure_specular_sharpness_splash", SystemConsole.StoreAs.Float, "The specular sharness of the splash table color when in Pure Mode (2.0 - 8.0).", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float pure_SPLASH_SPECULAR_SHARPNESS = TableScript.PURE_SPLASH_SPECULAR_SHARPNESS;
			this.Variable("pure_specular_sharpness_splash", ref pure_SPLASH_SPECULAR_SHARPNESS, ref status, parameters);
			if (status.isDirty)
			{
				if (pure_SPLASH_SPECULAR_SHARPNESS < 2f || pure_SPLASH_SPECULAR_SHARPNESS > 8f)
				{
					this.Log("Must be in range 2.0 - 8.0", false);
					return;
				}
				TableScript.PURE_SPLASH_SPECULAR_SHARPNESS = pure_SPLASH_SPECULAR_SHARPNESS;
				this.PureModeSecondaryMaterial.SetFloat("_Shininess", pure_SPLASH_SPECULAR_SHARPNESS);
				TableScript.UpdatePureMode();
			}
		}, ""));
		this.ConsoleCommands.Add("mod_caching", this.NewPlayerVariable<bool>("mod_caching", "Globally control if the game should use mod caching to speed up loading. This controls regular and raw caching.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("mod_caching", ref ConfigGame.Settings.ConfigMods.Caching, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<ConfigGame>.Instance.TriggerSettingChanged();
			}
		}, ""));
		this.ConsoleCommands.Add("mod_caching_raw", this.NewPlayerVariable<bool>("mod_caching_raw", "This is an even faster cache than regular caching that stores the raw data for an assets, but using more disk space.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("mod_caching_raw", ref ConfigGame.Settings.ConfigMods.RawCaching, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<ConfigGame>.Instance.TriggerSettingChanged();
			}
		}, ""));
		this.ConsoleCommands.Add("mod_threading", this.NewPlayerVariable<bool>("mod_threading", "Controls if the game will use multiple threads to speed and smooth out loading of custom content. Only turn off if you have loading problems.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("mod_threading", ref ConfigGame.Settings.ConfigMods.Threading, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<ConfigGame>.Instance.TriggerSettingChanged();
			}
		}, ""));
		this.ConsoleCommands.Add("mod_thread_count", this.NewPlayerVariable<int>("mod_thread_count", SystemConsole.StoreAs.Manual, "Controls the number of threads that will be used to load up assets. Default is CPU threads - 1.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			int maxNumberJobs = ThreadedJob.MaxNumberJobs;
			this.Variable("mod_thread_count", ref maxNumberJobs, ref status, parameters);
			if (status.isDirty)
			{
				ThreadedJob.MaxNumberJobs = maxNumberJobs;
			}
		}, ""));
		this.ConsoleCommands.Add("randomize_zone_prompt_on_load", this.NewPlayerVariable<bool>("randomize_zone_prompt_on_load", SystemConsole.StoreAs.Bool, "When ON and you load a game with a Randomize Zone, it will ask if you want to activate it.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("randomize_zone_prompt_on_load", ref RandomizeZone.ShowPromptOnLoad, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("reset", this.NewPlayerCommand("reset", "Resets a persistent variable to its default value and removes its stored setting (playerpref).", "USAGE: $CMD$ <variable>\nResets a variable.  May only reset variables which are persistent.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text4 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text4 == null)
			{
				this.Log("You must provide a <variable>.", false);
				return;
			}
			SystemConsole.ConsoleCommand consoleCommand;
			this.ConsoleCommands.TryGetValue(text4, out consoleCommand);
			if (!this.CommandAvailable(consoleCommand))
			{
				this.Log(text4 + " does not exist.", false);
				return;
			}
			if (consoleCommand.storeAs == SystemConsole.StoreAs.DoNotStore && !status.isSilent)
			{
				this.Log(text4 + " is not persistent!", false);
				return;
			}
			status.Batch();
			switch (consoleCommand.storeAs)
			{
			case SystemConsole.StoreAs.Bool:
				this.ExecuteCommand(text4 + " " + ((bool)SystemConsole.Defaults[text4]).ToString(), ref status, SystemConsole.CommandEcho.Quiet);
				break;
			case SystemConsole.StoreAs.Int:
				this.ExecuteCommand(text4 + " " + (int)SystemConsole.Defaults[text4], ref status, SystemConsole.CommandEcho.Quiet);
				break;
			case SystemConsole.StoreAs.Float:
				this.ExecuteCommand(text4 + " " + (float)SystemConsole.Defaults[text4], ref status, SystemConsole.CommandEcho.Quiet);
				break;
			case SystemConsole.StoreAs.String:
				this.ExecuteCommand(text4 + " " + (string)SystemConsole.Defaults[text4], ref status, SystemConsole.CommandEcho.Quiet);
				break;
			case SystemConsole.StoreAs.Manual:
			{
				Func<string> func;
				if (this.ManualDefaultValues.TryGetValue(text4, out func))
				{
					this.ExecuteCommand(text4 + " " + func(), ref status, SystemConsole.CommandEcho.Quiet);
					return;
				}
				this.Log(text4 + " has no default value!", false);
				return;
			}
			}
			PlayerPrefs.DeleteKey(text4);
		}, ""));
		this.ConsoleCommands.Add("quiet_mode", this.NewPlayerVariable<bool>("quiet_mode", "When ON commands display their outputs only.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool flag = this.echoMode > SystemConsole.CommandEcho.Loud;
			this.Variable("quiet_mode", ref flag, ref status, parameters);
			if (status.isDirty)
			{
				if (flag)
				{
					this.echoMode = SystemConsole.CommandEcho.Quiet;
					return;
				}
				this.echoMode = SystemConsole.CommandEcho.Loud;
			}
		}, ""));
		this.ConsoleCommands.Add("quit", this.NewPlayerCommand("quit", "Exit TTS entirely.", "USAGE: $CMD$ {-f}\nExit to desktop.  Shows a confirm prompt.\n -f = Don't show prompt: force exit.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (LibString.bite(ref parameters, false, ' ', false, false, '\0') == "-f")
			{
				Application.Quit();
				return;
			}
			NetworkSingleton<NetworkUI>.Instance.GUIExitGame();
		}, ""));
		this.ConsoleCommands.Add("rewind_interval", this.NewPlayerVariable<float>("rewind_interval", SystemConsole.StoreAs.Float, "Time in seconds between each rewind.  If set to 0 then rewind is disabled.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("rewind_interval", ref NetworkSingleton<SaveManager>.Instance.RewindSaveInterval, ref status, parameters);
			if (NetworkSingleton<SaveManager>.Instance.RewindSaveInterval != 0f && NetworkSingleton<SaveManager>.Instance.RewindSaveInterval < 1f)
			{
				NetworkSingleton<SaveManager>.Instance.RewindSaveInterval = 1f;
			}
		}, ""));
		this.ConsoleCommands.Add("rotation_degrees", this.NewPlayerVariable<int>("rotation_degrees", SystemConsole.StoreAs.Manual, "Degrees component is rotate through when held.", "USAGE: $CMD$ <degrees>\nMust be a mulitple of 15", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			int num = PlayerScript.PointerScript.RotationSnap;
			this.Variable("rotation_degrees", ref num, ref status, parameters);
			if (status.isDirty)
			{
				num = (int)Mathf.Max(1f, Mathf.Floor((float)num / 15f)) * 15;
				PlayerScript.PointerScript.RotationSnap = num;
			}
		}, ""));
		this.ConsoleCommands.Add("save", this.NewAdminCommand("save", "Save current game.", "USAGE: $CMD$ <filename> {<gamename>}\nSaves game as <filename>.  Optionally provide a <gamename>.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text4 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			text4 = text4.Replace("..", "").Replace(":", "");
			if (text4 == "")
			{
				this.Log("Bad filename, could not save", false);
				return;
			}
			if (!text4.ToLower().EndsWith(".json"))
			{
				text4 += ".json";
			}
			string text5 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text5 == null)
			{
				text5 = (NetworkSingleton<GameOptions>.Instance.GameName ?? "");
			}
			text4 = DirectoryScript.saveFilePath + "//" + text4;
			SerializationScript.Save(text4, text5, true);
			SerializationScript.UpdateAccessTime(text4);
		}, ""));
		this.ConsoleCommands.Add("say_game", this.NewPlayerCommand("say_game", "Send text as if typed into Game tab.", "USAGE: $CMD$ <message>\nSends <message> to Game tab as if you had typed it in.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			LibString.stripLead(ref parameters, ' ');
			Chat.InputChat(parameters, false, "", ChatMessageType.Game);
			if (!this.mirrorTab[ChatMessageType.All] && !this.mirrorTab[ChatMessageType.Game])
			{
				this.Log("[888888]Output on Game tab, use [1F87FF]+mirror_game[-] to see it here too.", Colour.GreyDark, false);
			}
		}, ""));
		this.ConsoleCommands.Add("say_global", this.NewPlayerCommand("say_global", "Send text as if typed into Global tab.", "USAGE: $CMD$ <message>\nSends <message> to Global tab as if you had typed it in.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			LibString.stripLead(ref parameters, ' ');
			Chat.InputChat(parameters, false, "", ChatMessageType.Global);
			if (!this.mirrorTab[ChatMessageType.All] && !this.mirrorTab[ChatMessageType.Global])
			{
				this.Log("[888888]Output on Global tab, use [1F87FF]+mirror_global[-] to see it here too.", Colour.GreyDark, false);
			}
		}, ""));
		this.ConsoleCommands.Add("say_team", this.NewPlayerCommand("say_team", "Send text as if typed into Team tab.", "USAGE: $CMD$ <message>\nSends <message> to Team tab as if you had typed it in.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			LibString.stripLead(ref parameters, ' ');
			Chat.InputChat(parameters, false, "", ChatMessageType.Team);
			if (!this.mirrorTab[ChatMessageType.All] && !this.mirrorTab[ChatMessageType.Team])
			{
				this.Log("[888888]Output on Team tab, use [1F87FF]+mirror_team[-] to see it here too.", Colour.GreyDark, false);
			}
		}, ""));
		this.ConsoleCommands.Add("search_close_after_take", this.NewPlayerVariable<bool>("search_close_after_take", SystemConsole.StoreAs.Bool, "If ON then the search interface will close as soon as you take any object from it.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("search_close_after_take", ref UIInventory.CLOSE_SEARCH_AFTER_TAKE, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("search_specularity_threshold", this.NewPlayerVariable<float>("search_specularity_threshold", SystemConsole.StoreAs.Float, "Sets the threshold below which specularity is disabled for objects in containers, so as not to appear faded.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("search_specularity_threshold", ref UIInventoryItem.SPECULAR_INTENSITY_TO_KEEP_COLOR, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("sendkey", this.NewPlayerCommand("sendkey", "Emulates a keypress (or other input).", "USAGE: $CMD$ <key>\nEmulates Unity KeyCode <key> as if it were pressed by the user.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text4 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text4 == null)
			{
				this.Log("Specify a <key>.", false);
				return;
			}
			if (text4.Length == 1)
			{
				text4 = text4.ToUpper();
			}
			KeyCode key;
			try
			{
				key = (KeyCode)Enum.Parse(typeof(KeyCode), text4);
			}
			catch
			{
				this.Log("KeyCode <" + text4 + "> not recognized.", false);
				return;
			}
			TTSInput.Override(key);
		}, ""));
		this.ConsoleCommands.Add("skip", this.NewPlayerCommand("skip", "Skip forward in batch script.", "USAGE: $CMD$ <label> {<variable> {<comparison> <value> {threshold}}}\nOnly applicable within a script.  If only <label> specified, or <variable> (or its <comparison> to <value>) is positive, then skip ahead to <label>.\nIf <threshold> specified then = and <> will use it.Comparisons: = < > <= >= <>", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text4 = LibString.bite(ref parameters, true, ' ', false, false, '\0');
			if (text4 == null)
			{
				this.Log("You must provide a <label>.", false);
				return;
			}
			if (!text4.StartsWith(":"))
			{
				text4 = ":" + text4;
			}
			string text5 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text5 == null)
			{
				status.skippingToLabel = text4;
				return;
			}
			SystemConsole.ConsoleCommand consoleCommand;
			this.ConsoleCommands.TryGetValue(text5, out consoleCommand);
			if (!this.CommandAvailable(consoleCommand))
			{
				this.Log(text5 + " does not exist.", false);
				return;
			}
			if (consoleCommand.type != SystemConsole.CommandType.Variable)
			{
				this.Log(text5 + " is not a variable!", false);
				return;
			}
			string text6 = LibString.bite(ref parameters, true, ' ', false, false, '\0');
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text6);
			if (num <= 957132539U)
			{
				if (num <= 284975636U)
				{
					if (num != 0U)
					{
						if (num == 284975636U)
						{
							if (text6 == ">=")
							{
								goto IL_1D1;
							}
						}
					}
					else if (text6 == null)
					{
						goto IL_1D1;
					}
				}
				else if (num != 940354920U)
				{
					if (num == 957132539U)
					{
						if (text6 == "<")
						{
							goto IL_1D1;
						}
					}
				}
				else if (text6 == "=")
				{
					text6 = "==";
					goto IL_1D1;
				}
			}
			else if (num <= 2428715011U)
			{
				if (num != 990687777U)
				{
					if (num == 2428715011U)
					{
						if (text6 == "!=")
						{
							goto IL_1D1;
						}
					}
				}
				else if (text6 == ">")
				{
					goto IL_1D1;
				}
			}
			else if (num != 2431966415U)
			{
				if (num != 2482446367U)
				{
					if (num == 2499223986U)
					{
						if (text6 == "<=")
						{
							goto IL_1D1;
						}
					}
				}
				else if (text6 == "<>")
				{
					text6 = "!=";
					goto IL_1D1;
				}
			}
			else if (text6 == "==")
			{
				goto IL_1D1;
			}
			this.Log("Illegal comparison: " + text6, false);
			return;
			IL_1D1:
			string text7 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			string text8 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			float num2 = 0f;
			if (text8 != null && !float.TryParse(text8, out num2))
			{
				this.Log("Invalid thershold", false);
				return;
			}
			bool flag = false;
			this.ExecuteCommand(text5 + " -return", ref status, SystemConsole.CommandEcho.Silent);
			if (consoleCommand.variableType == typeof(bool))
			{
				bool flag2 = (bool)status.value;
				if (text6 == null)
				{
					flag = flag2;
				}
				else if (text7 == null)
				{
					this.Log("Missing value", false);
				}
				else if (text6 == "==")
				{
					flag = LibString.StringIsTrue(text7);
				}
				else if (text6 == "!=")
				{
					flag = LibString.StringIsFalse(text7);
				}
				else
				{
					this.Log("Invalid comparison for toggle variable: " + text6, false);
				}
			}
			else if (consoleCommand.variableType == typeof(string))
			{
				string a = (string)status.value;
				if (text6 == null)
				{
					flag = (a != "");
				}
				else if (text7 == null)
				{
					this.Log("Missing value", false);
				}
				else if (text6 == "==")
				{
					flag = (a == text7);
				}
				else if (text6 == "!=")
				{
					flag = (a != text7);
				}
				else
				{
					this.Log("Invalid comparison for text variable: " + text6, false);
				}
			}
			else if (consoleCommand.variableType == typeof(int))
			{
				int num3 = (int)status.value;
				int num4;
				if (text6 == null)
				{
					flag = (num3 != 0);
				}
				else if (text7 == null)
				{
					this.Log("Missing value", false);
				}
				else if (int.TryParse(text7, out num4))
				{
					if (text6 == "==")
					{
						flag = ((float)num3 >= (float)num4 - num2 && (float)num3 <= (float)num4 + num2);
					}
					else if (text6 == "!=")
					{
						flag = ((float)num3 < (float)num4 - num2 || (float)num3 > (float)num4 + num2);
					}
					else if (text6 == "<")
					{
						flag = (num3 < num4);
					}
					else if (text6 == ">")
					{
						flag = (num3 > num4);
					}
					else if (text6 == "<=")
					{
						flag = (num3 <= num4);
					}
					else if (text6 == ">=")
					{
						flag = (num3 >= num4);
					}
				}
			}
			else if (consoleCommand.variableType == typeof(float))
			{
				float num5 = (float)status.value;
				float num6;
				if (text6 == null)
				{
					flag = (num5 != 0f);
				}
				else if (text7 == null)
				{
					this.Log("Missing value", false);
				}
				else if (float.TryParse(text7, out num6))
				{
					if (text6 == "==")
					{
						flag = (num5 >= num6 - num2 && num5 <= num6 + num2);
					}
					else if (text6 == "!=")
					{
						flag = (num5 < num6 - num2 || num5 > num6 + num2);
					}
					else if (text6 == "<")
					{
						flag = (num5 < num6);
					}
					else if (text6 == ">")
					{
						flag = (num5 > num6);
					}
					else if (text6 == "<=")
					{
						flag = (num5 <= num6);
					}
					else if (text6 == ">=")
					{
						flag = (num5 >= num6);
					}
				}
			}
			if (flag)
			{
				status.skippingToLabel = text4;
			}
		}, ""));
		this.ConsoleCommands.Add("smooth_move_force", this.NewDeveloperVariable<float>("smooth_move_force", "Force used to propel objects during smooth move.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("smooth_move_force", ref ManagerPhysicsObject.SmoothMoveBaseForce, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("spectator_activate_with_resolution", this.NewPlayerCommand("spectator_activate_with_resolution", "Activates spectator window with specified resolution.", "USAGE: $CMD$ <width> <height> {-d <display>} {-r <rate>} {-p <x> <y>}\nActivates spectator window with specified resolution.\n-d = specify display number (use `displays` command to list displays)\n-p = display in a panel inside TTS instead of on a separate display\n-o = no overlay buttons\n-s = no sizing corners\n-l = locked in place (not draggable with mouse)\n", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (Singleton<SpectatorCamera>.Instance.active)
			{
				if (!status.isSilent)
				{
					this.Log("Spectator window already active.", false);
				}
				return;
			}
			string command = "spectator_activate_with_resolution " + parameters;
			string text4 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			int width;
			if (!SystemConsole.intFromArg(text4, out width, "<width>"))
			{
				return;
			}
			text4 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			int height;
			if (!SystemConsole.intFromArg(text4, out height, "<height>"))
			{
				return;
			}
			int rate = 60;
			int display = 1;
			bool flag = false;
			int x = 20;
			int y = 20;
			bool overlayButtons = true;
			bool sizingCorners = true;
			bool movable = true;
			bool hideOnHover = false;
			while (parameters != "")
			{
				text4 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
				if (text4 == "-d")
				{
					text4 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
					if (!SystemConsole.intFromArg(text4, out display, "<display>"))
					{
						return;
					}
				}
				else if (text4 == "-r")
				{
					text4 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
					if (!SystemConsole.intFromArg(text4, out rate, "<rate>"))
					{
						return;
					}
				}
				else if (text4 == "-p")
				{
					flag = true;
					text4 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
					if (!SystemConsole.intFromArg(text4, out x, "<x>"))
					{
						return;
					}
					text4 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
					if (!SystemConsole.intFromArg(text4, out y, "<y>"))
					{
						return;
					}
				}
				else if (text4 == "-o")
				{
					overlayButtons = false;
				}
				else if (text4 == "-s")
				{
					sizingCorners = false;
				}
				else if (text4 == "-h")
				{
					hideOnHover = true;
				}
				else
				{
					if (!(text4 == "-l"))
					{
						this.Log("Unknown parameter.", false);
						return;
					}
					movable = false;
				}
			}
			Singleton<SpectatorCamera>.Instance.TurnOn(width, height, rate, display, flag, x, y, overlayButtons, sizingCorners, movable, hideOnHover);
			if (!flag)
			{
				this.DeferredExecuteCommand(command);
			}
		}, ""));
		this.ConsoleCommands.Add("spectator_camera_attachment", this.NewPlayerVariable<string>("spectator_camera_attachment", "<GUID> of component or <PLAYER COLOR> of pointer which spectator camera will follow when spectator_camera_follow_attachment is ON.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("spectator_camera_attachment", ref Singleton<SpectatorCamera>.Instance.lockObjectIdentifier, ref status, parameters);
			if (status.isDirty)
			{
				string text4 = LibString.CamelCaseFromUnderscore(Singleton<SpectatorCamera>.Instance.lockObjectIdentifier, true, false);
				if (Colour.IsColourLabel(text4))
				{
					Singleton<SpectatorCamera>.Instance.lockObjectIdentifier = text4;
				}
				Singleton<SpectatorCamera>.Instance.lockObject = null;
			}
		}, ""));
		this.ConsoleCommands.Add("spectator_camera_dolly", this.NewPlayerVariable<float>("spectator_camera_dolly", "Set distance camera is offset when following a component, along its facing direction.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("spectator_camera_dolly", ref Singleton<SpectatorCamera>.Instance.lockObjectOffset, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("spectator_camera_follow_attachment", this.NewPlayerVariable<bool>("spectator_camera_follow_attachment", "Set spectator camera to follow component or player pointer specified with spectator_camera_attachment.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("spectator_camera_follow_attachment", ref Singleton<SpectatorCamera>.Instance.lockedToObject, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("spectator_camera_follow_player", this.NewPlayerVariable<bool>("spectator_camera_follow_player", "Set spectator camera to follow player camera.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("spectator_camera_follow_player", ref Singleton<SpectatorCamera>.Instance.lockedToPlayer, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("spectator_camera_load", this.NewPlayerVariable<int>("spectator_camera_load", "Set spectator camera position to saved position.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			int lastSetPosition = Singleton<SpectatorCamera>.Instance.lastSetPosition;
			this.Variable("spectator_camera_load", ref lastSetPosition, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<SpectatorCamera>.Instance.lastSetPosition = lastSetPosition;
				if (Singleton<CameraController>.Instance.CameraStateInUse(lastSetPosition))
				{
					Singleton<SpectatorCamera>.Instance.LoadState(Singleton<CameraController>.Instance.CameraStates[lastSetPosition]);
					return;
				}
				this.Log("No camera save in slot " + lastSetPosition, false);
			}
		}, ""));
		this.ConsoleCommands.Add("spectator_camera_load_zero", this.NewPlayerVariable<int>("spectator_camera_load_zero", "Set spectator camera position to saved position.  Position is zero-indexed (so one less than displayed in UI)", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			int num = Singleton<SpectatorCamera>.Instance.lastSetPosition - 1;
			this.Variable("spectator_camera_load_zero", ref num, ref status, parameters);
			if (status.isDirty)
			{
				num++;
				Singleton<SpectatorCamera>.Instance.lastSetPosition = num;
				if (Singleton<CameraController>.Instance.CameraStateInUse(num))
				{
					Singleton<SpectatorCamera>.Instance.LoadState(Singleton<CameraController>.Instance.CameraStates[num]);
					return;
				}
				this.Log("No camera save in slot " + num, false);
			}
		}, ""));
		this.ConsoleCommands.Add("spectator_camera_look_at", this.NewPlayerCommand("spectator_camera_look_at", "Make spectator camera look at a component or player pointer.", "USAGE: $CMD$ <GUID> OR $CMD$ <COLOR>", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text4 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text4 == null)
			{
				this.Log("Specifiy component GUID or player color.", false);
				return;
			}
			string text5 = LibString.CamelCaseFromUnderscore(text4, true, false);
			if (Colour.IsColourLabel(text5))
			{
				text4 = text5;
			}
			Singleton<SpectatorCamera>.Instance.glanceIdentifier = text4;
		}, ""));
		this.ConsoleCommands.Add("spectator_camera_offset_position", this.NewPlayerVariable<Vector3>("spectator_camera_offset_position", "Set camera positional offset from component.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("spectator_camera_offset_position", ref Singleton<SpectatorCamera>.Instance.lockObjectOffsetPosition, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("spectator_camera_offset_rotation", this.NewPlayerVariable<Vector3>("spectator_camera_offset_rotation", "Set camera rotational offset from component.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("spectator_camera_offset_rotation", ref Singleton<SpectatorCamera>.Instance.lockObjectOffsetRotation, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("spectator_camera_override_player_with_look", this.NewPlayerVariable<bool>("spectator_camera_override_player_with_look", "When ON the look and track commands will work when spectator_camera_follow_player is ON.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("spectator_camera_override_player_with_look", ref Singleton<SpectatorCamera>.Instance.lockedToObject, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("spectator_camera_smooth_position", this.NewPlayerVariable<float>("spectator_camera_smooth_position", "The factor used to smooth the spectator camera follow position.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float lerp_RATE = SpectatorCamera.LERP_RATE;
			this.Variable("spectator_camera_smooth_position", ref lerp_RATE, ref status, parameters);
			if (!status.isDirty)
			{
				return;
			}
			if (lerp_RATE > 0f && lerp_RATE <= 1f)
			{
				SpectatorCamera.LERP_RATE = lerp_RATE;
				return;
			}
			this.Log("Must be in range (0, 1] : resetting to previous value.", false);
		}, ""));
		this.ConsoleCommands.Add("spectator_camera_smooth_rotation", this.NewPlayerVariable<float>("spectator_camera_smooth_rotation", "The factor used to smooth the spectator camera follow rotation.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float slerp_RATE = SpectatorCamera.SLERP_RATE;
			this.Variable("spectator_camera_smooth_rotation", ref slerp_RATE, ref status, parameters);
			if (!status.isDirty)
			{
				return;
			}
			if (slerp_RATE > 0f && slerp_RATE <= 1f)
			{
				SpectatorCamera.SLERP_RATE = slerp_RATE;
				return;
			}
			this.Log("Must be in range (0, 1] : resetting to previous value.", false);
		}, ""));
		this.ConsoleCommands.Add("spectator_camera_smooth_on_load", this.NewPlayerVariable<bool>("spectator_camera_smooth_on_load", "Set spectator camera to follow player camera.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("spectator_camera_smooth_on_load", ref SpectatorCamera.SMOOTH_SET_POSITION, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("spectator_camera_stay_upright", this.NewPlayerVariable<bool>("spectator_camera_stay_upright", "When ON the camera will not rotate upside-down.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("spectator_camera_stay_upright", ref Singleton<SpectatorCamera>.Instance.stayUpright, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("spectator_camera_target", this.NewPlayerVariable<string>("spectator_camera_target", "<GUID> or <PLAYER COLOR> for spectator camera to look at.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("spectator_camera_target", ref Singleton<SpectatorCamera>.Instance.trackedIdentifier, ref status, parameters);
			if (status.isDirty)
			{
				string text4 = LibString.CamelCaseFromUnderscore(Singleton<SpectatorCamera>.Instance.trackedIdentifier.ToLower(), true, false);
				if (Colour.IsColourLabel(text4))
				{
					Singleton<SpectatorCamera>.Instance.trackedIdentifier = text4;
				}
				Singleton<SpectatorCamera>.Instance.trackedObject = null;
			}
		}, ""));
		this.ConsoleCommands.Add("spectator_camera_tracking", this.NewPlayerVariable<bool>("spectator_camera_tracking", "When ON spectator camera willo track target specified with spectator_camera_target.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("spectator_camera_tracking", ref Singleton<SpectatorCamera>.Instance.tracking, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("spectator_panel_buttons", this.NewPlayerVariable<bool>("spectator_panel_buttons", "When ON the spectator panel will have overlay buttons (copy player, lock to player, restrict view, close).", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool overlayButtonsActive = Singleton<UISpectatorView>.Instance.OverlayButtonsActive;
			this.Variable("spectator_panel_buttons", ref overlayButtonsActive, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<UISpectatorView>.Instance.OverlayButtonsActive = overlayButtonsActive;
			}
		}, ""));
		this.ConsoleCommands.Add("spectator_panel_corners", this.NewPlayerVariable<bool>("spectator_panel_corners", "When ON the spectator panel will have draggable corners (allows resizing).", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool sizingCornersActive = Singleton<UISpectatorView>.Instance.SizingCornersActive;
			this.Variable("spectator_panel_corners", ref sizingCornersActive, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<UISpectatorView>.Instance.SizingCornersActive = sizingCornersActive;
			}
		}, ""));
		this.ConsoleCommands.Add("spectator_panel_locked", this.NewPlayerVariable<bool>("spectator_panel_locked", "When ON the spectaor panel will not be draggable with the mouse.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool flag = !Singleton<UISpectatorView>.Instance.Movable;
			this.Variable("spectator_panel_locked", ref flag, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<UISpectatorView>.Instance.Movable = !flag;
			}
		}, ""));
		this.ConsoleCommands.Add("spectator_post_processing", this.NewPlayerVariable<bool>("spectator_post_processing", "When ON the spectator window will apply post-processing effects.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool spectatorPostProcessing = SystemConsole.SpectatorPostProcessing;
			this.Variable("spectator_post_processing", ref spectatorPostProcessing, ref status, parameters);
			if (status.isDirty)
			{
				SystemConsole.SpectatorPostProcessing = spectatorPostProcessing;
				Singleton<SpectatorCamera>.Instance.GetComponent<PostProcessLayer>().enabled = (spectatorPostProcessing && !TableScript.PURE_MODE);
			}
		}, ""));
		this.ConsoleCommands.Add("spectator_restrict_view", this.NewPlayerVariable<bool>("spectator_restrict_view", "When ON the spectator window will only see what attached spectators see.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool restrictView = Singleton<SpectatorCamera>.Instance.RestrictView;
			this.Variable("spectator_restrict_view", ref restrictView, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<SpectatorCamera>.Instance.RestrictView = restrictView;
			}
		}, ""));
		this.ConsoleCommands.Add("spectator_restrict_zoom", this.NewPlayerVariable<bool>("spectator_restrict_zoom", "When ON the spectator zoom display will be restricted if the view is restricted.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("spectator_restrict_zoom", ref SpectatorAltZoomCamera.SpectatorAltZoomRestricted, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("spectator_show_grid", this.NewPlayerVariable<bool>("spectator_show_grid", "When ON the spectator window will display grid lines.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("spectator_show_grid", ref Singleton<SpectatorCamera>.Instance.showGrid, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<SpectatorCamera>.Instance.UpdateGrid();
			}
		}, ""));
		this.ConsoleCommands.Add("spectator_show_ui", this.NewPlayerVariable<bool>("spectator_show_ui", "When ON the spectator window will display UI elements.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("spectator_show_ui", ref Singleton<SpectatorCamera>.Instance.showUI, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<SpectatorCamera>.Instance.UpdateUICamera();
			}
		}, ""));
		this.ConsoleCommands.Add("spectator_show_game_ui", this.NewPlayerVariable<bool>("spectator_show_game_ui", "When ON the spectator window will display UI elements created by the currently loaded game.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool showUI3D = Singleton<SpectatorCamera>.Instance.ShowUI3D;
			this.Variable("spectator_show_game_ui", ref showUI3D, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<SpectatorCamera>.Instance.ShowUI3D = showUI3D;
			}
		}, ""));
		this.ConsoleCommands.Add("spectator_show_zoom", this.NewPlayerVariable<bool>("spectator_show_zoom", "When ON the spectator window will show the alt zoom display.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("spectator_show_zoom", ref SpectatorAltZoomCamera.SpectatorAltZoomActive, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("spectator_screen", this.NewPlayerVariable<bool>("spectator_screen", "When ON the spectator view will be displayed on a screen.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool flag = Singleton<SpectatorCamera>.Instance.active && !Singleton<SpectatorCamera>.Instance.displayedInPanel;
			this.Variable("spectator_screen", ref flag, ref status, parameters);
			if (status.isDirty)
			{
				if (Singleton<SpectatorCamera>.Instance.active && Singleton<SpectatorCamera>.Instance.displayedInPanel)
				{
					this.Log("Spectator view is currently being displayed in a window.  Use spectator_window to control it.", false);
					return;
				}
				if (flag)
				{
					if (!Singleton<SpectatorCamera>.Instance.active)
					{
						Singleton<SpectatorCamera>.Instance.TurnOn(0, 0, 0, 1, false, 0, 0, true, true, true, false);
						this.DeferredExecuteCommand("+spectator_screen");
						return;
					}
					if (!status.isSilent)
					{
						this.Log("Spectator screen already active.", false);
						return;
					}
				}
				else
				{
					this.Log("Spectator view displayed on screen cannot be disabled.", false);
				}
			}
		}, ""));
		this.ConsoleCommands.Add("spectator_window", this.NewPlayerVariable<bool>("spectator_window", "When ON the spectator view will be displayed in a window inside TTS.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool flag = Singleton<SpectatorCamera>.Instance.active && Singleton<SpectatorCamera>.Instance.displayedInPanel;
			this.Variable("spectator_window", ref flag, ref status, parameters);
			if (status.isDirty)
			{
				if (Singleton<SpectatorCamera>.Instance.active && !Singleton<SpectatorCamera>.Instance.displayedInPanel)
				{
					this.Log("Spectator view displayed on screen cannot be disabled.", false);
					return;
				}
				if (flag != Singleton<SpectatorCamera>.Instance.active)
				{
					if (flag)
					{
						int[] panelPrefs = Singleton<UISpectatorView>.Instance.GetPanelPrefs();
						Singleton<SpectatorCamera>.Instance.TurnOn(panelPrefs[2], panelPrefs[3], 0, 0, true, panelPrefs[0], panelPrefs[1], true, true, true, false);
						return;
					}
					Singleton<UISpectatorView>.Instance.OnClose();
				}
			}
		}, ""));
		this.ConsoleCommands.Add("spectator_zoom_follows_pointer", this.NewPlayerVariable<bool>("spectator_zoom_follows_pointer", "When ON the spectator zoom display will appear at the pointer location. When OFF it will appear at spectator_zoom_position.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("spectator_zoom_follows_pointer", ref SpectatorAltZoomCamera.SpectatorAltZoomFollowsPointer, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("spectator_zoom_position", this.NewPlayerVariable<Vector2>("spectator_zoom_position", "The location the zoom display will appear when spectator_zoom_follows_pointer is off.  0,0 = bottom left, 1,1 = top right.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			Vector2 spectatorAltZoomLocation = SpectatorAltZoomCamera.SpectatorAltZoomLocation;
			this.Variable("spectator_zoom_position", ref spectatorAltZoomLocation, ref status, parameters);
			if (status.isDirty)
			{
				if (spectatorAltZoomLocation.x < 0f || spectatorAltZoomLocation.x > 1f || spectatorAltZoomLocation.y < 0f || spectatorAltZoomLocation.y > 1f)
				{
					this.Log("Must be in range 0.0 - 1.0", false);
					return;
				}
				SpectatorAltZoomCamera.SpectatorAltZoomLocation = spectatorAltZoomLocation;
			}
		}, ""));
		this.ConsoleCommands.Add("stats_monitor", this.NewPlayerVariable<bool>("stats_monitor", "When ON activates stats monitor.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("stats_monitor", ref SystemConsole.ShowGraphy, ref status, parameters);
			if (status.isDirty)
			{
				if (this.Graphy == null)
				{
					this.Graphy = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("[Graphy]"));
				}
				this.Graphy.SetActive(SystemConsole.ShowGraphy);
			}
		}, ""));
		this.ConsoleCommands.Add("status", this.NewPlayerCommand("status", "Displays current status.", "USAGE: $CMD$\nDisplays information on current game state.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (status.isSilent)
			{
				return;
			}
			this.LogVariable("Peer Type", Network.peerType.ToString(), Colour.OrangeDark, 37);
			this.LogVariable("Times Started", SystemConsole.TimesStarted.ToString(), Colour.OrangeDark, 37);
			this.LogVariable("Admin", NetworkSingleton<PlayerManager>.Instance.IsAdmin(NetworkID.ID).ToString(), Colour.OrangeDark, 37);
			this.LogVariable("Host", NetworkSingleton<PlayerManager>.Instance.IsHost(NetworkID.ID).ToString(), Colour.OrangeDark, 37);
		}, ""));
		this.ConsoleCommands.Add("store_number", this.NewPlayerCommand("store_number", "Creates a numeric variable.", "USAGE: $CMD$ <variable> {<value>}\nCreates a numeric variable.  It will be set to <value> if it is provided, else 0.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string variableName = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (variableName == null)
			{
				this.Log("You must provide a <variable>.", false);
				return;
			}
			SystemConsole.ConsoleCommand consoleCommand;
			this.ConsoleCommands.TryGetValue(variableName, out consoleCommand);
			if (this.CommandAvailable(consoleCommand))
			{
				this.Log(variableName + " already exists!", false);
				return;
			}
			string str2 = LibString.lookAhead(parameters, false, ' ', 0, false, false);
			this.UserFloatVars[variableName] = 0f;
			consoleCommand = this.NewPlayerVariable<float>(variableName, "numeric variable", this.DocsForVariable<float>(variableName), delegate(SystemConsole.CommandStatus variableStatus, string variableParameters)
			{
				float value = this.UserFloatVars[variableName];
				this.Variable(variableName, ref value, ref variableStatus, variableParameters);
				if (variableStatus.isDirty)
				{
					this.UserFloatVars[variableName] = value;
				}
			}, "");
			consoleCommand.origin = SystemConsole.CommandOrigin.UserVariable;
			consoleCommand.variableType = typeof(float);
			this.ConsoleCommands[variableName] = consoleCommand;
			this.ConsoleCommandList.Add(variableName);
			this.ConsoleCommandList.Sort();
			this.ExecuteCommand(variableName + " " + str2, ref status, status.echo);
		}, ""));
		this.ConsoleCommands.Add("store_text", this.NewPlayerCommand("store_text", "Creates a text variable.", "USAGE: $CMD$ <variable> {<value>}\nCreates a text variable.  It will be set to <value> if it is provided.\nIf in a multiline batch (such as autoexec), leaving value blank will set\nit to the block of text which follows, until 'end <variable>'", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string variableName = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (variableName == null)
			{
				this.Log("You must provide a <variable>.", false);
				return;
			}
			SystemConsole.ConsoleCommand consoleCommand;
			this.ConsoleCommands.TryGetValue(variableName, out consoleCommand);
			if (this.CommandAvailable(consoleCommand))
			{
				this.Log(variableName + " already exists!", false);
				return;
			}
			string str2;
			if (LibString.lookAhead(parameters, false, ' ', 0, false, false) == null)
			{
				str2 = "";
			}
			else
			{
				int num = 0;
				while (num < parameters.Length && parameters[num] == ' ')
				{
					num++;
				}
				str2 = parameters.Substring(num);
			}
			this.UserStringVars[variableName] = "";
			consoleCommand = this.NewPlayerVariable<string>(variableName, "text variable", this.DocsForVariable<string>(variableName), delegate(SystemConsole.CommandStatus variableStatus, string variableParameters)
			{
				string value = this.UserStringVars[variableName];
				this.Variable(variableName, ref value, ref variableStatus, variableParameters);
				if (variableStatus.isDirty)
				{
					this.UserStringVars[variableName] = value;
				}
			}, "");
			consoleCommand.origin = SystemConsole.CommandOrigin.UserVariable;
			consoleCommand.variableType = typeof(string);
			this.ConsoleCommands[variableName] = consoleCommand;
			this.ConsoleCommandList.Add(variableName);
			this.ConsoleCommandList.Sort();
			this.ExecuteCommand(variableName + " " + str2, ref status, status.echo);
		}, ""));
		this.ConsoleCommands.Add("store_toggle", this.NewPlayerCommand("store_toggle", "Creates a variable which can be ON or OFF.", "USAGE: $CMD$ <variable> {<value>}\nCreates a variable which can be ON or OFF.  It will be set to <value> if it is provided, else OFF.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string variableName = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (variableName == null)
			{
				this.Log("You must provide a <variable>.", false);
				return;
			}
			SystemConsole.ConsoleCommand consoleCommand;
			this.ConsoleCommands.TryGetValue(variableName, out consoleCommand);
			if (this.CommandAvailable(consoleCommand))
			{
				this.Log(variableName + " already exists!", false);
				return;
			}
			string str2 = LibString.lookAhead(parameters, false, ' ', 0, false, false);
			this.UserBoolVars[variableName] = false;
			consoleCommand = this.NewPlayerVariable<bool>(variableName, "toggle variable", this.DocsForVariable<bool>(variableName), delegate(SystemConsole.CommandStatus variableStatus, string variableParameters)
			{
				bool value = this.UserBoolVars[variableName];
				this.Variable(variableName, ref value, ref variableStatus, variableParameters);
				if (variableStatus.isDirty)
				{
					this.UserBoolVars[variableName] = value;
				}
			}, "");
			consoleCommand.origin = SystemConsole.CommandOrigin.UserVariable;
			consoleCommand.variableType = typeof(bool);
			this.ConsoleCommands[variableName] = consoleCommand;
			this.ConsoleCommandList.Add(variableName);
			this.ConsoleCommandList.Sort();
			this.ExecuteCommand(variableName + " " + str2, ref status, status.echo);
		}, ""));
		this.ConsoleCommands.Add("subtract", this.NewPlayerCommand("subtract", "Set a numerical variable to itself subtracted from a value.", "USAGE: $CMD$ <variable> <value>\nSets <variable> to <value> - current value.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text4 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text4 == null)
			{
				this.Log("You must provide a numeric <variable>.", false);
				return;
			}
			SystemConsole.ConsoleCommand consoleCommand;
			this.ConsoleCommands.TryGetValue(text4, out consoleCommand);
			if (consoleCommand == null || consoleCommand.type != SystemConsole.CommandType.Variable || (consoleCommand.variableType != typeof(float) && consoleCommand.variableType != typeof(int)))
			{
				this.Log("You must provide a numeric <variable>.", false);
				return;
			}
			float num;
			if (!float.TryParse(LibString.bite(ref parameters, false, ' ', false, false, '\0'), out num))
			{
				this.Log("Could not parse numeric <value>", false);
				return;
			}
			SystemConsole.CommandStatus commandStatus = new SystemConsole.CommandStatus(SystemConsole.CommandEcho.Loud);
			commandStatus.Batch();
			this.ExecuteCommand(text4 + " -return", ref commandStatus, SystemConsole.CommandEcho.Silent);
			float num2;
			if (commandStatus.value.GetType() == typeof(int))
			{
				num2 = (float)((int)commandStatus.value);
			}
			else
			{
				num2 = (float)commandStatus.value;
			}
			num2 = num - num2;
			status.Batch();
			this.ExecuteCommand(text4 + " " + num2, ref status, SystemConsole.CommandEcho.Quiet);
		}, ""));
		this.ConsoleCommands.Add("team", this.NewPlayerVariable<string>("team", "Your current team.", "USAGE: $CMD$ {<team>}\nReturns your current team.  If <team> provided will attempt to swap you to that team.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			PlayerState playerState = null;
			if (NetworkSingleton<PlayerManager>.Instance)
			{
				playerState = NetworkSingleton<PlayerManager>.Instance.MyPlayerState();
			}
			if (playerState == null)
			{
				return;
			}
			Team team = playerState.team;
			string text4 = TeamScript.StringFromTeam(team);
			status.value = text4;
			string text5 = LibString.bite(ref parameters, true, ' ', false, false, '\0');
			if (text5 == "-return")
			{
				return;
			}
			if (text5 == "-variable")
			{
				if (!status.isSilent)
				{
					this.LogVariable("team", text4, 37);
				}
				return;
			}
			if (text5 != null)
			{
				if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.ChangeTeam, -1))
				{
					this.Log("You may not change team.", false);
					return;
				}
				team = TeamScript.TeamFromString(LibString.CamelCaseFromUnderscore(text5, true, false));
				text4 = TeamScript.StringFromTeam(team);
				status.value = text4;
				NetworkSingleton<PlayerManager>.Instance.ChangeTeam(playerState.id, team);
			}
			SystemConsole.lastReturnedValue = text4;
			if (!status.isSilent)
			{
				string str2;
				if (team > Team.Diamonds)
				{
					if (team - Team.Clubs <= 2)
					{
						str2 = "[6A6A6A]";
					}
					else
					{
						text4 = "No team";
						str2 = "[FFFFFF]";
					}
				}
				else
				{
					str2 = "[960100]";
				}
				this.Log("team: " + str2 + text4, status.inBatch);
			}
		}, ""));
		this.ConsoleCommands.Add("test_npo_from_go_with_get_component", this.NewDeveloperCommand("test_npo_from_go_with_get_component", "Tests accessing NPO from gameObject using GetComponent.", "USAGE: $CMD$ <guid> <count=1000>\nGets the NPO for specified game object <count> times.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			GameObject gameObject;
			int num;
			if (this.TestHelper(status, parameters, out gameObject, out num))
			{
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				for (int k = 0; k < num; k++)
				{
					int id = gameObject.GetComponent<NetworkPhysicsObject>().ID;
				}
				float realtimeSinceStartup2 = Time.realtimeSinceStartup;
				this.LogVariable("Start Time", realtimeSinceStartup.ToString(), Colour.OrangeDark, 37);
				this.LogVariable("End Time", realtimeSinceStartup2.ToString(), Colour.OrangeDark, 37);
				this.LogVariable("Total Time", (realtimeSinceStartup2 - realtimeSinceStartup).ToString(), Colour.Pink, 37);
			}
		}, ""));
		this.ConsoleCommands.Add("test_npo_from_go_with_local", this.NewDeveloperCommand("test_npo_from_go_with_local", "Tests accessing NPO from gameObject using a local cached NPO.", "USAGE: $CMD$ <guid> <count=1000>\nGets the NPO for specified game object <count> times.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			GameObject gameObject;
			int num;
			if (this.TestHelper(status, parameters, out gameObject, out num))
			{
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				NetworkPhysicsObject component = gameObject.GetComponent<NetworkPhysicsObject>();
				for (int k = 0; k < num; k++)
				{
					int id = component.ID;
				}
				float realtimeSinceStartup2 = Time.realtimeSinceStartup;
				this.LogVariable("Start Time", realtimeSinceStartup.ToString(), Colour.OrangeDark, 37);
				this.LogVariable("End Time", realtimeSinceStartup2.ToString(), Colour.OrangeDark, 37);
				this.LogVariable("Total Time", (realtimeSinceStartup2 - realtimeSinceStartup).ToString(), Colour.Pink, 37);
			}
		}, ""));
		this.ConsoleCommands.Add("test_npo_from_go_with_manager", this.NewDeveloperCommand("test_npo_from_go_with_manager", "Tests accessing NPO from gameObject using ManagerPhysicsObject.GameObjectToNetworkPhysicsObject.", "USAGE: $CMD$ <guid> <count=1000>\nGets the NPO for specified game object <count> times.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			GameObject grabbable;
			int num;
			if (this.TestHelper(status, parameters, out grabbable, out num))
			{
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				for (int k = 0; k < num; k++)
				{
					int id = NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGO(grabbable).ID;
				}
				float realtimeSinceStartup2 = Time.realtimeSinceStartup;
				this.LogVariable("Start Time", realtimeSinceStartup.ToString(), Colour.OrangeDark, 37);
				this.LogVariable("End Time", realtimeSinceStartup2.ToString(), Colour.OrangeDark, 37);
				this.LogVariable("Total Time", (realtimeSinceStartup2 - realtimeSinceStartup).ToString(), Colour.Pink, 37);
			}
		}, ""));
		this.ConsoleCommands.Add("timestamp_all", this.NewPlayerVariable<bool>("timestamp_all", "When ON timestamps will be displayed for ALL messages.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool value;
			Singleton<ChatSettings>.Instance.chatDisplayTimestamp.TryGetValue(ChatMessageType.All, out value);
			this.Variable("timestamp_all", ref value, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<ChatSettings>.Instance.chatDisplayTimestamp[ChatMessageType.All] = value;
				PlayerPrefs.SetInt("chatDisplayTimestampAll", Singleton<ChatSettings>.Instance.chatDisplayTimestamp[ChatMessageType.All] ? 1 : 0);
			}
		}, ""));
		this.ConsoleCommands.Add("timestamp_format", this.NewPlayerVariable<string>("timestamp_format", "Format string which controls how message timestamps are displayed.  See tinyurl.com/ycwh45af for details.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("timestamp_format", ref Chat.TIMESTAMP_FORMAT, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("timestamp_game", this.NewPlayerVariable<bool>("timestamp_game", SystemConsole.StoreAs.Bool, "When ON timestamps will be displayed for GAME chat messages.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool value;
			Singleton<ChatSettings>.Instance.chatDisplayTimestamp.TryGetValue(ChatMessageType.Game, out value);
			this.Variable("timestamp_game", ref value, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<ChatSettings>.Instance.chatDisplayTimestamp[ChatMessageType.Game] = value;
			}
		}, ""));
		this.ConsoleCommands.Add("timestamp_global", this.NewPlayerVariable<bool>("timestamp_global", SystemConsole.StoreAs.Bool, "When ON timestamps will be displayed for GLOBAL chat messages.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool value;
			Singleton<ChatSettings>.Instance.chatDisplayTimestamp.TryGetValue(ChatMessageType.Global, out value);
			this.Variable("timestamp_global", ref value, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<ChatSettings>.Instance.chatDisplayTimestamp[ChatMessageType.Global] = value;
			}
		}, ""));
		this.ConsoleCommands.Add("timestamp_system", this.NewPlayerVariable<bool>("timestamp_system", SystemConsole.StoreAs.Bool, "When ON timestamps will be displayed for SYSTEM messages.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool value;
			Singleton<ChatSettings>.Instance.chatDisplayTimestamp.TryGetValue(ChatMessageType.System, out value);
			this.Variable("timestamp_system", ref value, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<ChatSettings>.Instance.chatDisplayTimestamp[ChatMessageType.System] = value;
			}
		}, ""));
		this.ConsoleCommands.Add("timestamp_team", this.NewPlayerVariable<bool>("timestamp_team", SystemConsole.StoreAs.Bool, "When ON timestamps will be displayed for TEAM chat messages.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool value;
			Singleton<ChatSettings>.Instance.chatDisplayTimestamp.TryGetValue(ChatMessageType.Team, out value);
			this.Variable("timestamp_team", ref value, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<ChatSettings>.Instance.chatDisplayTimestamp[ChatMessageType.Team] = value;
			}
		}, ""));
		this.ConsoleCommands.Add("tool_current", this.NewPlayerVariable<PointerMode>("tool_current", "Change or displays the current tool mode.", "USAGE: $CMD$ {mode}\nDisplays index of current tool mode.  If mode specified then changes to it.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (LibString.lookAhead(parameters, false, ' ', 0, false, false) == null && !status.inBatch && !status.isSilent)
			{
				this.Newline();
				this.Log("Tool modes:", false);
				foreach (object obj in Enum.GetValues(typeof(PointerMode)))
				{
					PointerMode pointerMode = (PointerMode)obj;
					if (pointerMode != PointerMode.None)
					{
						this.Log(LibString.UnderscoreFromCamelCase(pointerMode.ToString()), Colour.Purple, false);
					}
				}
				this.Newline();
			}
			if (PlayerScript.PointerScript == null)
			{
				return;
			}
			PointerMode currentPointerMode = PlayerScript.PointerScript.CurrentPointerMode;
			this.Variable("tool_current", ref currentPointerMode, ref status, parameters);
			if (status.isDirty)
			{
				PlayerScript.PointerScript.CurrentPointerMode = currentPointerMode;
			}
		}, ""));
		string text;
		using (IEnumerator enumerator = Enum.GetValues(typeof(PointerMode)).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PointerMode pm = (PointerMode)enumerator.Current;
				if (pm != PointerMode.None)
				{
					string str = LibString.UnderscoreFromCamelCase(pm.ToString());
					text = "tool_" + str;
					this.ConsoleCommands.Add(text, this.NewPlayerCommand(text, "Change tool mode to " + str + ".", "USAGE: $CMD$\nChanges current tool mode to " + str + ".", delegate(SystemConsole.CommandStatus status, string parameters)
					{
						if (PlayerScript.PointerScript == null)
						{
							return;
						}
						PlayerScript.PointerScript.CurrentPointerMode = pm;
					}, ""));
				}
			}
		}
		this.ConsoleCommands.Add("tool_revert", this.NewPlayerCommand("tool_revert", "Change tool mode.", "USAGE: $CMD$ {-d}\nChanges current tool mode to that which was in use prior to last tool switch.  -d = If last switch was to the same tool then don't change", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (PlayerScript.PointerScript == null)
			{
				return;
			}
			if (LibString.bite(ref parameters, false, ' ', false, false, '\0') == "-d")
			{
				PlayerScript.PointerScript.CurrentPointerMode = PlayerScript.PointerScript.PreviousPointerModeWithDup;
				return;
			}
			PlayerScript.PointerScript.CurrentPointerMode = PlayerScript.PointerScript.PreviousPointerMode;
		}, ""));
		this.ConsoleCommands.Add("translate", this.NewPlayerCommand("translate", "Translate supplied English text into current language if it exist in TTS localization dictionary.", "USAGE: $CMD$ <text>", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (status.isSilent)
			{
				return;
			}
			string text4 = Language.Translate(parameters.Trim());
			if (text4 == null)
			{
				this.Log("<Not found>", false);
				return;
			}
			this.Log(text4, false);
		}, ""));
		this.ConsoleCommands.Add("translation_export", this.NewPlayerCommand("translation_export", "Export language translation as CSV.", "USAGE: $CMD$ <language> <filename>\nSave language translations for specified <language> in CSV file <filename>.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text4 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			string languageCode;
			if (Language.TryGetLanguageName(text4, out languageCode))
			{
				string text5 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
				string text6 = Language.ExportCSVFile(text5, languageCode);
				if (text6 == null)
				{
					this.Log("Error writing to: " + text5, false);
					return;
				}
				if (!status.isSilent)
				{
					this.Log("Wrote languages to: " + text6, false);
					return;
				}
			}
			else
			{
				this.Log("Not a language: " + text4, false);
			}
		}, ""));
		for (int i = 0; i < Language.OrderedLanguageCodes.Count; i++)
		{
			string code = Language.OrderedLanguageCodes[i];
			string text2 = Language.LanguageFromCode[code];
			string cmd = "translation_for_" + code.Replace("-", "_");
			string help = string.Concat(new string[]
			{
				"Filename of translation CSV for language: ",
				text2,
				". (",
				code,
				"))"
			});
			this.ManualDefaultValues[cmd] = (() => "");
			this.ConsoleCommands.Add(cmd, this.NewPlayerVariable<string>(cmd, SystemConsole.StoreAs.Manual, help, delegate(SystemConsole.CommandStatus status, string parameters)
			{
				string text4 = Language.CSVFilenameFromCode[code];
				if (text4 == "")
				{
					text4 = Language.DefaultFileName(code);
				}
				string b = text4;
				this.Variable(cmd, ref text4, ref status, parameters);
				if (status.isDirty && text4 != b)
				{
					if (code == Language.CurrentLanguageCode)
					{
						Singleton<Language>.Instance.SetLanguageAndTranslation(code, text4);
						return;
					}
					if (text4 == "")
					{
						Language.SetCSVFilenameFromCode(code, "");
						return;
					}
					string filePath = Path.Combine(DirectoryScript.translationsPath, text4);
					CSV csv;
					if (Language.TryReadValidCSVFile(code, filePath, out csv))
					{
						Language.SetCSVFilenameFromCode(code, text4);
						return;
					}
					Language.SetCSVFilenameFromCode(code, "");
					this.Log("Bad CSV, resetting to default.", false);
				}
			}, ""));
		}
		this.ConsoleCommands.Add("translation_import", this.NewPlayerCommand("translation_import", "Import language translations in CSV.", "USAGE: $CMD$ <filename>\nLoad all language translations in CSV file <filename>.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string filePath = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			CSV csv;
			global::Result result = Language.TryReadValidCSVFile("", filePath, out csv);
			if (!result)
			{
				this.Log(result, false);
				return;
			}
			csv.RemoveColumn(3);
			result = Singleton<Language>.Instance.ImportCSV(csv.ToString());
			if (!result)
			{
				this.Log(result, false);
				return;
			}
			if (!status.isSilent)
			{
				this.Log("Imported!", false);
			}
		}, ""));
		this.ConsoleCommands.Add("ui_anchor", this.NewPlayerVariable<Vector2>("ui_anchor", "Set position on screen UI elements will be created relative to.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_anchor", ref this.UIAnchor, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("ui_autofocus_search", this.NewPlayerVariable<bool>("ui_autofocus_search", SystemConsole.StoreAs.Bool, "Automatically focus search input boxes when windows containing them are displayed.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_autofocus_search", ref SystemConsole.AutofocusSearch, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("ui_book_buttons_on_hover", this.NewPlayerVariable<bool>("ui_book_buttons_on_hover", SystemConsole.StoreAs.Bool, "Whether books only show their attached buttons when hovered (if OFF then they show them all the time).", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_book_buttons_on_hover", ref CustomPDF.ONLY_SHOW_UI_WHEN_HOVERED, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("ui_book_highlight_color", this.NewPlayerVariable<string>("ui_book_highlight_color", SystemConsole.StoreAs.String, "Color of highlighter on book pop-out.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text4 = "#" + ColorUtility.ToHtmlStringRGBA(UIPDFPopout.HIGHLIGHT_COLOUR);
			status.value = text4;
			string text5 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text5 == "-return")
			{
				return;
			}
			if (text5 == "-variable")
			{
				if (!status.isSilent)
				{
					this.LogVariable("ui_book_highlight_color", text4, 37);
				}
				return;
			}
			if (text5 != null)
			{
				Colour colour;
				string msg;
				if (!SystemConsole.TryGetColour(text5, out colour, out msg))
				{
					this.Log(msg, false);
					return;
				}
				text4 = "#" + ColorUtility.ToHtmlStringRGBA(colour);
				UIPDFPopout.HIGHLIGHT_COLOUR = colour;
				status.value = text4;
				status.Dirty();
			}
			SystemConsole.lastReturnedValue = text4;
			if (!status.isSilent)
			{
				this.Log(text4, false);
			}
		}, ""));
		this.ConsoleCommands.Add("ui_book_navigation_bookmark_level", this.NewPlayerVariable<int>("ui_book_navigation_bookmark_level", SystemConsole.StoreAs.Int, "Level of bookmarks which navigation buttons will jump between.  If 0 then ui_book_navigation_step will always be used.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_book_navigation_bookmark_level", ref CustomPDF.BOOKMARK_LEVEL, ref status, parameters);
			if (status.isDirty)
			{
				CustomPDF[] array = UnityEngine.Object.FindObjectsOfType<CustomPDF>();
				for (int k = 0; k < array.Length; k++)
				{
					array[k].CalculateBookmarkLevel();
				}
			}
		}, ""));
		this.ConsoleCommands.Add("ui_book_navigation_step", this.NewPlayerVariable<int>("ui_book_navigation_step", SystemConsole.StoreAs.Int, "Number of pages the back and forward buttons on the book reader will jump.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_book_navigation_step", ref CustomPDF.BIG_PAGE_STEP, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("ui_book_panel_opacity", this.NewPlayerVariable<float>("ui_book_panel_opacity", SystemConsole.StoreAs.Float, "Opacity of book pop-out panel and buttons when you are not hovering over it.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_book_panel_opacity", ref UIPDFPopout.PANEL_ALPHA, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<UIPDFPopout>.Instance.SetPanelOutAlpha(UIPDFPopout.PANEL_ALPHA);
			}
		}, ""));
		this.ConsoleCommands.Add("ui_book_page_opacity", this.NewPlayerVariable<float>("ui_book_page_opacity", SystemConsole.StoreAs.Float, "Opacity of book pop-out pages when you are not hovering over them.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_book_page_opacity", ref UIPDFPopout.PDF_ALPHA, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<UIPDFPopout>.Instance.SetPDFOutAlpha(UIPDFPopout.PDF_ALPHA);
			}
		}, ""));
		this.ConsoleCommands.Add("ui_book_window", this.NewPlayerVariable<bool>("ui_book_window", "When ON the book pop-out window is active.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool activeSelf = Singleton<UIPDFPopout>.Instance.gameObject.activeSelf;
			this.Variable("ui_book_window", ref activeSelf, ref status, parameters);
			if (status.isDirty)
			{
				if (activeSelf)
				{
					this.Log("Use `action_popout` on the book you wish to view.", false);
					return;
				}
				Singleton<UIPDFPopout>.Instance.gameObject.SetActive(false);
			}
		}, ""));
		this.ConsoleCommands.Add("ui_button", this.NewPlayerCommand("ui_button", "Add an on-screen button which performs a command.", "USAGE: $CMD$ <label> <x> <y> {-f <fontsize>} {-w <width}} {-h <height>} {-s|-l} <command>\nAdd a button with specified position which will perform <command> when clicked.\n-s = silent\n-l = loud", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text4 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			int num;
			if (!int.TryParse(LibString.bite(ref parameters, false, ' ', false, false, '\0'), out num))
			{
				this.Log("Could not parse X", false);
				return;
			}
			int num2;
			if (!int.TryParse(LibString.bite(ref parameters, false, ' ', false, false, '\0'), out num2))
			{
				this.Log("Could not parse Y", false);
				return;
			}
			string text5 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			int volume = 1;
			int num3 = 20;
			int num4 = (int)((float)(text4.Length * num3) * 0.5f + 20f);
			int num5 = (int)((float)num3 * 0.5f + 20f);
			bool flag = false;
			bool flag2 = false;
			while (text5 != null && text5.StartsWith("-"))
			{
				if (text5 == "-f")
				{
					if (!int.TryParse(LibString.bite(ref parameters, false, ' ', false, false, '\0'), out num3))
					{
						this.Log("Could not parse font size", false);
						return;
					}
					if (!flag)
					{
						num4 = (int)((float)(text4.Length * num3) * 0.5f + 20f);
					}
					if (!flag2)
					{
						num5 = (int)((float)num3 * 0.5f + 20f);
					}
				}
				else if (text5 == "-w")
				{
					if (!int.TryParse(LibString.bite(ref parameters, false, ' ', false, false, '\0'), out num4))
					{
						this.Log("Could not parse width", false);
						return;
					}
					flag = true;
				}
				else if (text5 == "-h")
				{
					if (!int.TryParse(LibString.bite(ref parameters, false, ' ', false, false, '\0'), out num5))
					{
						this.Log("Could not parse width", false);
						return;
					}
					flag2 = true;
				}
				else if (text5 == "-s")
				{
					volume = 2;
				}
				else
				{
					if (!(text5 == "-l"))
					{
						this.Log("Unknown parameter: " + text5, false);
						return;
					}
					volume = 0;
				}
				text5 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			}
			text5 = text5 + " " + parameters;
			CustomUI.Instance.AddButton(text4, text5, new Vector2((float)num + this.UIAnchor.x, (float)num2 + this.UIAnchor.y), new Vector2((float)num4, (float)num5), num3, volume);
		}, ""));
		this.ConsoleCommands.Add("ui_clear", this.NewPlayerCommand("ui_clear", "Clear all console script-generated UI elements.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			CustomUI.Instance.Clear();
		}, ""));
		this.ConsoleCommands.Add("ui_collapsing_context_menus", this.NewPlayerVariable<bool>("ui_collapsing_context_menus", SystemConsole.StoreAs.Bool, "Whether right-click context menu sections can be collapsed by clicking the line above them.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool flag = !UIContextMenuSpacer.ALWAYS_OPEN;
			this.Variable("ui_collapsing_context_menus", ref flag, ref status, parameters);
			if (status.isDirty)
			{
				UIContextMenuSpacer.ALWAYS_OPEN = !flag;
				UIContextMenuSpacer.UpdateEnabledDisabledBehaviour();
			}
		}, ""));
		this.ConsoleCommands.Add("ui_config_game_hotkeys", this.NewPlayerCommand("ui_config_game_hotkeys", "Shows game-defined hotkeys config UI.", "USAGE: $CMD$\nShows the game-defined hotkeys config user interface.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			NetworkSingleton<UserDefinedHotkeyManager>.Instance.ShowSettings(false);
		}, ""));
		this.ConsoleCommands.Add("ui_config_language", this.NewPlayerCommand("ui_config_language", "Shows Language Selection UI.", "USAGE: $CMD$\nShows the language selection user interface.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			Singleton<UILanguageSettings>.Instance.Display();
		}, ""));
		this.ConsoleCommands.Add("ui_config_misc", this.NewPlayerCommand("ui_config_misc", "Shows Misc Config UI.", "USAGE: $CMD$\nShows the Misc config user interface.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			UIMiscSettings.Instance.Display();
		}, ""));
		this.ConsoleCommands.Add("ui_config_theme", this.NewPlayerCommand("ui_config_theme", "Shows Theme Editor UI.", "USAGE: $CMD$\nShows the theme editor user interface.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			Singleton<UIThemeEditor>.Instance.Display();
		}, ""));
		this.ConsoleCommands.Add("ui_config_vr", this.NewPlayerCommand("ui_config_vr", "Shows VR Config UI.", "USAGE: $CMD$\nShows the VR config user interface.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			UIVRSettings.Instance.Display();
		}, ""));
		this.ConsoleCommands.Add("ui_container_enter_indicator", this.NewPlayerVariable<bool>("ui_container_enter_indicator", SystemConsole.StoreAs.Bool, "When ON an icon will be briefly displayed over a container when a player drops another object into it.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_container_enter_indicator", ref UIObjectIndicatorManager.ShowObjectEnterContainerIndicator, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("ui_context_menus_collapsed_height", this.NewPlayerVariable<int>("ui_context_menus_collapsed_height", "How large the line showing the collapsed context menu section is (8-32).", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			int collapsed_SPACER_HEIGHT = UIContextMenuSpacer.COLLAPSED_SPACER_HEIGHT;
			this.Variable("ui_context_menus_collapsed_height", ref collapsed_SPACER_HEIGHT, ref status, parameters);
			if (status.isDirty && collapsed_SPACER_HEIGHT >= 8 && collapsed_SPACER_HEIGHT <= 32)
			{
				UIContextMenuSpacer.COLLAPSED_SPACER_HEIGHT = collapsed_SPACER_HEIGHT;
			}
		}, ""));
		this.ConsoleCommands.Add("ui_context_menus_from_games", this.NewPlayerVariable<bool>("ui_context_menus_from_games", SystemConsole.StoreAs.Bool, "When ON games may create extra entries in the right-click context menus.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool flag = !UserDefinedContextualManager.PROHIBIT_USER_CONTEXT_MENUS;
			this.Variable("ui_context_menus_from_games", ref flag, ref status, parameters);
			if (status.isDirty)
			{
				UserDefinedContextualManager.PROHIBIT_USER_CONTEXT_MENUS = !flag;
			}
		}, ""));
		this.ConsoleCommands.Add("ui_context_menus_show_gm_notes", this.NewPlayerVariable<bool>("ui_context_menus_show_gm_notes", SystemConsole.StoreAs.Bool, "When ON and if you are Black, will display GM notes in context menu.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_context_menus_show_gm_notes", ref Pointer.SHOW_GM_NOTES, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("ui_custom_object_check", this.NewPlayerVariable<bool>("ui_custom_object_check", SystemConsole.StoreAs.Bool, "When ON, whenever you edit a custom object a dialog will appear if there are one or more identical custom objects which you may wish to update to match.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_custom_object_check", ref SystemConsole.CheckForMatchingCustomObjects, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("ui_dialog_color", this.NewPlayerCommand("ui_dialog_color", "Shows UIDialog:Color.", "USAGE: $CMD$ {<default>}}\nShows the COLOR version of the UIDialog and then displays it's output.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			SystemConsole.<>c__DisplayClass180_5 CS$<>8__locals5 = new SystemConsole.<>c__DisplayClass180_5();
			CS$<>8__locals5.<>4__this = this;
			CS$<>8__locals5.status = status;
			string text4 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			Color? color = null;
			try
			{
				if (text4 != null)
				{
					color = new Color?(Colour.ColourFromRGBHex(text4));
				}
			}
			catch
			{
			}
			if (color != null)
			{
				Singleton<UIColorPickerStandaloneDialog>.Instance.Show(new Action<Color>(CS$<>8__locals5.<CreateCommandsNZ>g__callback|115), color.Value);
				return;
			}
			Singleton<UIColorPickerStandaloneDialog>.Instance.Show(new Action<Color>(CS$<>8__locals5.<CreateCommandsNZ>g__callback|115));
		}, ""));
		this.ConsoleCommands.Add("ui_dialog_input", this.NewPlayerCommand("ui_dialog_input", "Shows UIDialog:Input.", "USAGE: $CMD$ {<label> {<default>}}\nShows the INPUT version of the UIDialog and then displays it's output.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text4 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			string text5 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text4 == null)
			{
				text5 = "Enter value:";
			}
			if (text5 == null)
			{
				text5 = "";
			}
			UIDialog.ShowInput(text4, "Ok", "Cancel", delegate(string text)
			{
				SystemConsole.lastReturnedValue = text;
				if (!status.isSilent)
				{
					this.Log(text, false);
				}
			}, null, text5, text4);
		}, ""));
		this.ConsoleCommands.Add("ui_games_click", this.NewPlayerCommand("ui_games_click", "Sends a click event to the Games dialog window.", "USAGE: $CMD$ <row> <column>\nSends a click event to the specified button on the Games dialog window if it is currently visible.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (NetworkSingleton<NetworkUI>.Instance.GUIGames.activeInHierarchy)
			{
				string s = LibString.bite(ref parameters, false, ' ', false, false, '\0');
				string s2 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
				int num;
				int.TryParse(s, out num);
				int num2;
				int.TryParse(s2, out num2);
				if (num >= 1 && num <= 4 && num2 >= 1 && num2 <= 5)
				{
					num--;
					num2--;
					this.confirmOveride = true;
					this.UIGamesRows[num].transform.GetChild(num2).SendMessage("OnClick");
					status.Open();
					this.CommandsAwaitingOnLoad.Add(status);
					return;
				}
				this.Log("You must specify a pertinent <row> and <column>.", false);
			}
		}, ""));
		this.ConsoleCommands.Add("ui_games_window", this.NewPlayerVariable<bool>("ui_games_window", "The state of the Games window.  Set to ON to show it.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool activeSelf = NetworkSingleton<NetworkUI>.Instance.GUIGames.activeSelf;
			this.Variable("ui_games_window", ref activeSelf, ref status, parameters);
			if (status.isDirty)
			{
				NetworkSingleton<NetworkUI>.Instance.GUIGames.SetActive(activeSelf);
			}
		}, ""));
		this.ConsoleCommands.Add("ui_gizmo_scale", this.NewPlayerVariable<float>("ui_gizmo_scale", SystemConsole.StoreAs.Float, "The size of the gizmo tool.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_gizmo_scale", ref SystemConsole.GizmoScaleMultiplier, ref status, parameters);
			if (status.isDirty)
			{
				SystemConsole.GizmoScaleMultiplier = Mathf.Max(0.5f, SystemConsole.GizmoScaleMultiplier);
				this.TranslationGizmo.GizmoBaseScale = 0.77f * SystemConsole.GizmoScaleMultiplier;
				this.ScaleGizmo.GizmoBaseScale = 0.77f * SystemConsole.GizmoScaleMultiplier;
				this.VolumeGizmo.DragHandleSizeInPixels = (int)(8f * SystemConsole.GizmoScaleMultiplier);
			}
		}, ""));
		this.ConsoleCommands.Add("ui_hand_minimized_size", this.NewPlayerVariable<float>("ui_hand_minimized_size", SystemConsole.StoreAs.Float, "Proportion of the hand view visible while minimized.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float minimized_SIZE = HandCamera.MINIMIZED_SIZE;
			this.Variable("ui_hand_minimized_size", ref minimized_SIZE, ref status, parameters);
			if (status.isDirty)
			{
				HandCamera.MINIMIZED_SIZE = Mathf.Clamp(minimized_SIZE, 0f, 1f);
			}
		}, ""));
		this.ConsoleCommands.Add("ui_hand_view", this.NewPlayerVariable<bool>("ui_hand_view", SystemConsole.StoreAs.Bool, "The on-screen hand view.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_hand_view", ref HandCamera.Instance.Enabled, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("ui_keyboard_default_state", this.NewPlayerVariable<int>("ui_keyboard_default_state", SystemConsole.StoreAs.Int, "Whether the on-screen keyboard is shown automatically.", "USAGE: $CMD$ <mode>\n<mode> may be one of:\n 0 = Disabled\n 1 = Enabled if in VR\n 2 = Enabled", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			int default_KEYBOARD_STATE = (int)UIOnScreenKeyboard.DEFAULT_KEYBOARD_STATE;
			this.Variable("ui_keyboard_default_state", ref default_KEYBOARD_STATE, ref status, parameters);
			if (status.isDirty)
			{
				UIOnScreenKeyboard.DEFAULT_KEYBOARD_STATE = (UIOnScreenKeyboard.KeyboardDefaultState)default_KEYBOARD_STATE;
				UIOnScreenKeyboard.ON_SCREEN_KEYBOARD = false;
				if (UIOnScreenKeyboard.DEFAULT_KEYBOARD_STATE == UIOnScreenKeyboard.KeyboardDefaultState.Enabled || (UIOnScreenKeyboard.DEFAULT_KEYBOARD_STATE == UIOnScreenKeyboard.KeyboardDefaultState.EnabledInVR && VRHMD.isVR))
				{
					UIOnScreenKeyboard.ON_SCREEN_KEYBOARD = true;
				}
			}
		}, ""));
		this.ConsoleCommands.Add("ui_keyboard_echo_duration", this.NewPlayerVariable<float>("ui_keyboard_echo_duration", SystemConsole.StoreAs.Float, "How long the echo is displayed above the keyboard.  Set to 0 to turn on permanently.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_keyboard_echo_duration", ref UIOnScreenKeyboard.READOUT_DISPLAY_DURATION, ref status, parameters);
			if (status.isDirty && UIOnScreenKeyboard.READOUT_DISPLAY_DURATION == 0f)
			{
				Singleton<UIOnScreenKeyboard>.Instance.DisplayReadout();
			}
		}, ""));
		this.ConsoleCommands.Add("ui_keyboard_scale", this.NewPlayerVariable<float>("ui_keyboard_scale", SystemConsole.StoreAs.Float, "Size of the on-screen keyboard.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float scale = UIOnScreenKeyboard.SCALE;
			this.Variable("ui_keyboard_scale", ref scale, ref status, parameters);
			if (status.isDirty)
			{
				UIOnScreenKeyboard.SCALE = scale;
			}
		}, ""));
		this.ConsoleCommands.Add("ui_keyboard_show", this.NewPlayerVariable<bool>("ui_keyboard_show", "The state of the on-screen keyboard.  Set to ON to show it.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool on_SCREEN_KEYBOARD = UIOnScreenKeyboard.ON_SCREEN_KEYBOARD;
			this.Variable("ui_keyboard_show", ref on_SCREEN_KEYBOARD, ref status, parameters);
			if (status.isDirty)
			{
				UIOnScreenKeyboard.ON_SCREEN_KEYBOARD = on_SCREEN_KEYBOARD;
			}
		}, ""));
		this.ConsoleCommands.Add("ui_label", this.NewPlayerCommand("ui_label", "Add an on-screen label (non-interactive).", "USAGE: $CMD$ <label> <x> <y> {-f <fontsize>} {-c <color>} {-o <outline>} {-d <dropshadow>}\nAdd a label at specified position.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text7 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			int num;
			if (!int.TryParse(LibString.bite(ref parameters, false, ' ', false, false, '\0'), out num))
			{
				this.Log("Could not parse X", false);
				return;
			}
			int num2;
			if (!int.TryParse(LibString.bite(ref parameters, false, ' ', false, false, '\0'), out num2))
			{
				this.Log("Could not parse Y", false);
				return;
			}
			string text8 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			int fontSize = 20;
			Colour black = Colour.Black;
			Colour? dropShadow = null;
			Colour? outline = null;
			while (text8 != null && text8.StartsWith("-"))
			{
				if (text8 == "-f")
				{
					if (!int.TryParse(LibString.bite(ref parameters, false, ' ', false, false, '\0'), out fontSize))
					{
						this.Log("Could not parse font size", false);
						return;
					}
				}
				else if (text8 == "-c")
				{
					text8 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
					string msg;
					if (!SystemConsole.TryGetColour(text8, out black, out msg))
					{
						this.Log(msg, false);
						return;
					}
				}
				else if (text8 == "-d")
				{
					text8 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
					Colour value;
					string msg2;
					if (!SystemConsole.TryGetColour(text8, out value, out msg2))
					{
						this.Log(msg2, false);
						return;
					}
					dropShadow = new Colour?(value);
				}
				else
				{
					if (!(text8 == "-o"))
					{
						this.Log("Unknown parameter: " + text8, false);
						return;
					}
					text8 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
					Colour value2;
					string msg3;
					if (!SystemConsole.TryGetColour(text8, out value2, out msg3))
					{
						this.Log(msg3, false);
						return;
					}
					outline = new Colour?(value2);
				}
				text8 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			}
			CustomUI.Instance.AddLabel(text7, new Vector2((float)num + this.UIAnchor.x, (float)num2 + this.UIAnchor.y), black, dropShadow, outline, fontSize);
		}, ""));
		foreach (KeyValuePair<string, string> keyValuePair in new Dictionary<string, string>
		{
			{
				"objects",
				"02 Objects"
			},
			{
				"music",
				"03 Music"
			},
			{
				"notebook",
				"04 Notebook"
			},
			{
				"options",
				"05 Options"
			},
			{
				"modding",
				"07 Modding"
			},
			{
				"flip",
				"08 Flip Table"
			}
		})
		{
			string cmd = "ui_main_" + keyValuePair.Key;
			string help2 = "When ON the " + keyValuePair.Key + " button on the main panel is visible.";
			string path = keyValuePair.Value;
			this.ConsoleCommands.Add(cmd, this.NewPlayerVariable<bool>(cmd, help2, delegate(SystemConsole.CommandStatus status, string parameters)
			{
				this.UIButtonHelper(cmd, status, parameters, path);
			}, ""));
		}
		this.ConsoleCommands.Add("ui_music_player", this.NewPlayerVariable<bool>("ui_music_player", "The state of the music player.  Set to ON to show it.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool activeSelf = Singleton<UICustomMusicPlayer>.Instance.gameObject.activeSelf;
			this.Variable("ui_music_player", ref activeSelf, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<UICustomMusicPlayer>.Instance.gameObject.SetActive(activeSelf);
			}
		}, ""));
		this.ConsoleCommands.Add("ui_objects_window", this.NewPlayerVariable<bool>("ui_objects_window", "The state of the Objects window.  Set to ON to show it.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool activeSelf = NetworkSingleton<NetworkUI>.Instance.GUIObjects.activeSelf;
			this.Variable("ui_objects_window", ref activeSelf, ref status, parameters);
			if (status.isDirty)
			{
				NetworkSingleton<NetworkUI>.Instance.GUIObjects.SetActive(activeSelf);
			}
		}, ""));
		this.ConsoleCommands.Add("ui_panel_chat", this.NewPlayerVariable<bool>("ui_panel_chat", "The state of the main panel.  Set to ON to show it.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool value = Singleton<UISettings>.Instance.chat.value;
			this.Variable("ui_panel_chat", ref value, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<UISettings>.Instance.chat.value = value;
				PlayerPrefs.SetInt("showChatUI", value ? 1 : 0);
				Singleton<UISettings>.Instance.chatGroup.SetActive(value);
				Singleton<UISettings>.Instance.chatInputIcon.SetActive(value);
			}
		}, ""));
		this.ConsoleCommands.Add("ui_panel_main", this.NewPlayerVariable<bool>("ui_panel_main", "The state of the main panel.  Set to ON to show it.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool value = Singleton<UISettings>.Instance.topMenu.value;
			this.Variable("ui_panel_main", ref value, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<UISettings>.Instance.topMenu.value = value;
				PlayerPrefs.SetInt("showTopMenuUI", value ? 1 : 0);
				Singleton<UISettings>.Instance.topMenuGroup.SetActive(value);
			}
		}, ""));
		this.ConsoleCommands.Add("ui_panel_notepad", this.NewPlayerVariable<bool>("ui_panel_notepad", "The state of the player panel.  Set to ON to show it.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool value = Singleton<UISettings>.Instance.notepad.value;
			this.Variable("ui_panel_notepad", ref value, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<UISettings>.Instance.notepad.value = value;
				PlayerPrefs.SetInt("showNotepadUI", value ? 1 : 0);
				Singleton<UISettings>.Instance.notepadGroup.SetActive(value);
			}
		}, ""));
		this.ConsoleCommands.Add("ui_panel_player", this.NewPlayerVariable<bool>("ui_panel_player", "The state of the player panel.  Set to ON to show it.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool value = Singleton<UISettings>.Instance.players.value;
			this.Variable("ui_panel_player", ref value, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<UISettings>.Instance.players.value = value;
				PlayerPrefs.SetInt("showPlayerUI", value ? 1 : 0);
				Singleton<UISettings>.Instance.playersGroup.SetActive(value);
			}
		}, ""));
		this.ConsoleCommands.Add("ui_panel_tools", this.NewPlayerVariable<bool>("ui_panel_tools", "The state of tools panel.  Set to ON to show it.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool value = Singleton<UISettings>.Instance.tools.value;
			this.Variable("ui_panel_tools", ref value, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<UISettings>.Instance.tools.value = value;
				PlayerPrefs.SetInt("showToolUI", value ? 1 : 0);
				Singleton<UISettings>.Instance.toolsGroup.SetActive(value);
			}
		}, ""));
		this.ConsoleCommands.Add("ui_server_browser_search", this.NewPlayerVariable<string>("ui_server_browser_search", SystemConsole.StoreAs.Manual, "Current Server Browser search text.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string @string = PlayerPrefs.GetString("ServerBrowserSearch", "");
			this.Variable("ui_server_browser_search", ref @string, ref status, parameters);
			if (status.isDirty)
			{
				NetworkSingleton<NetworkUI>.Instance.GUIServerBrowser.GetComponent<UIServerBrowser>().SearchScript.GetComponent<UIInput>().value = @string;
				PlayerPrefs.SetString("ServerBrowserSearch", @string);
			}
		}, ""));
		this.ConsoleCommands.Add("ui_server_browser_window", this.NewPlayerVariable<bool>("ui_server_browser_window", "The state of the server browser.  Set to ON to show it.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool activeSelf = NetworkSingleton<NetworkUI>.Instance.GUIServerBrowser.activeSelf;
			this.Variable("ui_server_browser_window", ref activeSelf, ref status, parameters);
			if (status.isDirty)
			{
				NetworkSingleton<NetworkUI>.Instance.GUIServerBrowser.SetActive(activeSelf);
			}
		}, ""));
		this.ConsoleCommands.Add("ui_tag_editor_window", this.NewPlayerVariable<bool>("ui_tag_editor_window", "The state of the tag editor window.  Set to ON to show it.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool activeSelf = NetworkSingleton<NetworkUI>.Instance.GUITagEditor.gameObject.activeSelf;
			this.Variable("ui_tag_editor_window", ref activeSelf, ref status, parameters);
			if (status.isDirty)
			{
				NetworkSingleton<NetworkUI>.Instance.GUITagEditor.gameObject.SetActive(activeSelf);
			}
		}, ""));
		this.ConsoleCommands.Add("ui_theme_from_game_auto", this.NewPlayerVariable<bool>("ui_theme_from_game_auto", SystemConsole.StoreAs.Bool, "When ON games you join or load may set the theme colours for you.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_theme_from_game_auto", ref UIPalette.ALLOW_GAME_TO_THEME, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("ui_theme_batch_start", this.NewPlayerCommand("ui_theme_batch_start", "Start a batch update of the UI Theme.", "When in a batch update, ui_theme_color_ commands will not refresh the UI until you run ui_theme_batch_end", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			UIPalette.InBatchThemeUpdate = true;
		}, ""));
		this.ConsoleCommands.Add("ui_theme_batch_end", this.NewPlayerCommand("ui_theme_batch_end", "End a batch update of the UI Theme.", "Run after ui_theme_batch_start + some ui_theme_color_ commands to refresh the UI all at once.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (UIPalette.InBatchThemeUpdate)
			{
				Singleton<UIPalette>.Instance.RefreshThemeColours(null, true);
			}
			UIPalette.InBatchThemeUpdate = false;
		}, ""));
		this.ConsoleCommands.Add("ui_theme_count", this.NewPlayerVariable<int>("ui_theme_count", "Number of currently available UI themes.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			status.ReadOnly();
			int count = Singleton<UIPalette>.Instance.Themes.Count;
			this.Variable("ui_theme_count", ref count, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("ui_theme_index", this.NewPlayerVariable<int>("ui_theme_index", SystemConsole.StoreAs.Manual, "Index of currently selected UI theme.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			int currentThemeID = Singleton<UIThemeEditor>.Instance.CurrentThemeID;
			this.Variable("ui_theme_index", ref currentThemeID, ref status, parameters);
			if (status.isDirty && ((currentThemeID == -2 && UIPalette.GameTheme.colours != null) || (currentThemeID >= 0 && currentThemeID < Singleton<UIPalette>.Instance.Themes.Count && Singleton<UIPalette>.Instance.Themes[currentThemeID].id != -1)))
			{
				Singleton<UIThemeEditor>.Instance.LoadTheme(currentThemeID, "");
			}
		}, ""));
		this.ConsoleCommands.Add("ui_theme_is_from_game", this.NewPlayerCommand("ui_theme_is_from_game", "Set theme to the one provided by the current game", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (UIPalette.GameTheme.colours != null)
			{
				Singleton<UIThemeEditor>.Instance.LoadTheme(-2, "");
				return;
			}
			this.Log("No game-provided theme available", false);
		}, ""));
		this.ConsoleCommands.Add("ui_theme_minimum_opacity", this.NewPlayerVariable<float>("ui_theme_minimum_opacity", SystemConsole.StoreAs.Float, "When ui_theme_restrict_transparency is ON this set the minimum value of alpha.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float min_ALPHA = UIPalette.MIN_ALPHA;
			this.Variable("ui_theme_minimum_opacity", ref UIPalette.MIN_ALPHA, ref status, parameters);
			if (status.isDirty && UIPalette.RESTRICT_TRANSPARENCY && UIPalette.MIN_ALPHA > min_ALPHA)
			{
				Singleton<UIPalette>.Instance.RefreshTransparency();
			}
		}, ""));
		this.ConsoleCommands.Add("ui_theme_name", this.NewPlayerVariable<string>("ui_theme_name", SystemConsole.StoreAs.Manual, "Name of currently selected UI theme.  \nSetting will load the specified theme, if it exists (unless currently within a batch theme update).", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string rawCurrentThemeName = Singleton<UIThemeEditor>.Instance.rawCurrentThemeName;
			this.Variable("ui_theme_name", ref rawCurrentThemeName, ref status, parameters);
			if (status.isDirty)
			{
				if (rawCurrentThemeName.ToLower() == "from game" && UIPalette.GameTheme.colours != null)
				{
					Singleton<UIThemeEditor>.Instance.LoadTheme(-2, "");
					if (!status.isSilent)
					{
						this.Log("Loaded theme From Game", false);
					}
					return;
				}
				if (!UIPalette.InBatchThemeUpdate)
				{
					for (int k = 0; k < Singleton<UIPalette>.Instance.Themes.Count; k++)
					{
						if (Singleton<UIPalette>.Instance.Themes[k].name.ToLower() == rawCurrentThemeName)
						{
							Singleton<UIThemeEditor>.Instance.LoadTheme(k, "");
							if (!status.isSilent)
							{
								this.Log("Loaded theme " + Singleton<UIPalette>.Instance.Themes[k].name, false);
							}
							return;
						}
					}
				}
				Singleton<UIThemeEditor>.Instance.CurrentThemeName = rawCurrentThemeName;
				if (!status.isSilent)
				{
					this.Log("Set theme name to " + rawCurrentThemeName, false);
				}
			}
		}, ""));
		this.ConsoleCommands.Add("ui_theme_restrict_transparency", this.NewPlayerVariable<bool>("ui_theme_restrict_transparency", SystemConsole.StoreAs.Bool, "When ON the alpha value of colours will always be at least 50%", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool restrict_TRANSPARENCY = UIPalette.RESTRICT_TRANSPARENCY;
			this.Variable("ui_theme_restrict_transparency", ref UIPalette.RESTRICT_TRANSPARENCY, ref status, parameters);
			if (status.isDirty && UIPalette.RESTRICT_TRANSPARENCY && !restrict_TRANSPARENCY)
			{
				Singleton<UIPalette>.Instance.RefreshTransparency();
			}
		}, ""));
		string[] names = Enum.GetNames(typeof(UIPalette.UI));
		for (int j = 0; j < names.Length; j++)
		{
			string text3 = names[j];
			UIPalette.UI item = (UIPalette.UI)Enum.Parse(typeof(UIPalette.UI), text3);
			if (item != UIPalette.UI.Auto && item != UIPalette.UI.DoNotTheme)
			{
				string cmd = UIPalette.CommandFromThemeString(LibString.UnderscoreFromCamelCase(text3));
				string help3 = "The color of \"" + LibString.SpacedCamelCase(text3) + "\" UI elements.";
				this.ConsoleCommands.Add(cmd, this.NewPlayerVariable<string>(cmd, help3, delegate(SystemConsole.CommandStatus status, string parameters)
				{
					string text7 = "#" + ColorUtility.ToHtmlStringRGBA(Singleton<UIPalette>.Instance.CurrentThemeColours[item]);
					status.value = text7;
					string text8 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
					if (text8 != null)
					{
						text8 = text8.Trim();
					}
					if (text8 == "-return")
					{
						return;
					}
					if (text8 == "-variable")
					{
						if (!status.isSilent)
						{
							this.LogVariable(cmd, text7, 37);
						}
						return;
					}
					if (text8 != null && item != UIPalette.UI.PlayerWhite && item != UIPalette.UI.PlayerBrown && item != UIPalette.UI.PlayerRed && item != UIPalette.UI.PlayerOrange && item != UIPalette.UI.PlayerYellow && item != UIPalette.UI.PlayerGreen && item != UIPalette.UI.PlayerTeal && item != UIPalette.UI.PlayerBlue && item != UIPalette.UI.PlayerPurple && item != UIPalette.UI.PlayerPink && item != UIPalette.UI.PlayerGrey && item != UIPalette.UI.PlayerBlack)
					{
						Colour colour;
						string msg;
						if (!SystemConsole.TryGetColour(text8, out colour, out msg))
						{
							this.Log(msg, false);
							return;
						}
						text7 = "#" + ColorUtility.ToHtmlStringRGBA(colour);
						if (UIPalette.RestrictTransparency(item) && colour.a < UIPalette.MIN_ALPHA)
						{
							colour.a = UIPalette.MIN_ALPHA;
						}
						if (UIPalette.InGameThemeUpdate)
						{
							UIPalette.GameTheme.colours[item] = colour;
							return;
						}
						Singleton<UIPalette>.Instance.CurrentThemeColours[item] = colour;
						if (!UIPalette.InBatchThemeUpdate)
						{
							if (item == UIPalette.UI.PureTableA || item == UIPalette.UI.PureTableASpecular || item == UIPalette.UI.PureTableB || item == UIPalette.UI.PureTableBSpecular || item == UIPalette.UI.PureSplash || item == UIPalette.UI.PureSplashSpecular || item == UIPalette.UI.PureSkyAbove || item == UIPalette.UI.PureSkyHorizon || item == UIPalette.UI.PureSkyBelow)
							{
								TableScript.UpdatePureMode();
							}
							Singleton<UIPalette>.Instance.RefreshThemeColours(null, true);
						}
						status.value = text7;
						status.Dirty();
					}
					SystemConsole.lastReturnedValue = text7;
					if (!status.isSilent)
					{
						this.Log(text7, false);
					}
				}, ""));
			}
		}
		this.ConsoleCommands.Add("ui_toast_duration", this.NewDeveloperVariable<float>("ui_toast_duration", SystemConsole.StoreAs.Float, "Sets how long toasts (for instance, the Discord join request) remain on screen before timing out.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_toast_duration", ref UIToast.DiscordRequestDuration, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("ui_toggle", this.NewPlayerCommand("ui_toggle", "Add an on-screen checkbox attached to a toggle variable.", "USAGE: $CMD$ <label> <x> <y> {-r} {-f <fontsize>} {-c <color>} {-o <outline>} {-d <dropshadow>} {-w <width>} {-s|-l} <variable>\nAdd a checkbox with specified position which will be attatched to <variable>.\n-r = label on right\n-s = silent\n-l = loud\n<outline> and <dropshadow> specify color.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string label = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			int num;
			if (!int.TryParse(LibString.bite(ref parameters, false, ' ', false, false, '\0'), out num))
			{
				this.Log("Could not parse X", false);
				return;
			}
			int num2;
			if (!int.TryParse(LibString.bite(ref parameters, false, ' ', false, false, '\0'), out num2))
			{
				this.Log("Could not parse Y", false);
				return;
			}
			string text7 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			int volume = 1;
			int num3 = 20;
			int size = (int)((float)num3 * 0.5f + 20f);
			bool flag = false;
			bool labelOnRight = false;
			Colour black = Colour.Black;
			Colour? dropShadow = null;
			Colour? outline = null;
			while (text7 != null && text7.StartsWith("-"))
			{
				if (text7 == "-f")
				{
					if (!int.TryParse(LibString.bite(ref parameters, false, ' ', false, false, '\0'), out num3))
					{
						this.Log("Could not parse font size", false);
						return;
					}
					if (!flag)
					{
						size = (int)((float)num3 * 0.5f + 20f);
					}
				}
				else if (text7 == "-w")
				{
					if (!int.TryParse(LibString.bite(ref parameters, false, ' ', false, false, '\0'), out size))
					{
						this.Log("Could not parse width", false);
						return;
					}
					flag = true;
				}
				else if (text7 == "-c")
				{
					text7 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
					string msg;
					if (!SystemConsole.TryGetColour(text7, out black, out msg))
					{
						this.Log(msg, false);
						return;
					}
				}
				else if (text7 == "-d")
				{
					text7 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
					Colour value;
					string msg2;
					if (!SystemConsole.TryGetColour(text7, out value, out msg2))
					{
						this.Log(msg2, false);
						return;
					}
					dropShadow = new Colour?(value);
				}
				else if (text7 == "-o")
				{
					text7 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
					Colour value2;
					string msg3;
					if (!SystemConsole.TryGetColour(text7, out value2, out msg3))
					{
						this.Log(msg3, false);
						return;
					}
					outline = new Colour?(value2);
				}
				else if (text7 == "-s")
				{
					volume = 2;
				}
				else if (text7 == "-l")
				{
					volume = 0;
				}
				else
				{
					if (!(text7 == "-r"))
					{
						this.Log("Unknown parameter: " + text7, false);
						return;
					}
					labelOnRight = true;
				}
				text7 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			}
			SystemConsole.ConsoleCommand command;
			this.ConsoleCommands.TryGetValue(text7, out command);
			if (!this.CommandAvailable(command))
			{
				this.Log(text7 + " does not exist.", false);
				return;
			}
			text7 = text7.ToLower();
			CustomUI.Instance.AddCheckbox(label, text7, new Vector2((float)num + this.UIAnchor.x, (float)num2 + this.UIAnchor.y), size, black, dropShadow, outline, num3, volume, labelOnRight);
		}, ""));
		this.ConsoleCommands.Add("ui_tooltip_delay", this.NewPlayerVariable<float>("ui_tooltip_delay", SystemConsole.StoreAs.Float, "Length of time you must hover over a UI element before the verbose tooltip is displayed.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_tooltip_delay", ref UIHoverText.UIDelayTooltipTime, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("ui_tooltip_indicator", this.NewPlayerVariable<bool>("ui_tooltip_indicator", SystemConsole.StoreAs.Bool, "When ON tooltips are indicated with a (?) postfix.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_tooltip_indicator", ref UITooltipScript.SHOW_QUESTION_MARK, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("ui_visualize_drops", this.NewPlayerVariable<bool>("ui_visualize_drops", SystemConsole.StoreAs.Bool, "Enable/disable drop location indicators.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_visualize_drops", ref ObjectPositioningVisualizer.VisualizeGrabbedObjects, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("ui_visualize_drop_free", this.NewPlayerVariable<bool>("ui_visualize_drop_free", SystemConsole.StoreAs.Bool, "Display drop location indicator when holding object which are not going to snap to a point.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_visualize_drop_free", ref ObjectPositioningVisualizer.VisualizeDropFree, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("ui_visualize_drop_grid", this.NewPlayerVariable<bool>("ui_visualize_drop_grid", SystemConsole.StoreAs.Bool, "Display drop location indicator when holding object which will snap to a grid point.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_visualize_drop_grid", ref ObjectPositioningVisualizer.VisualizeDropGrid, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("ui_visualize_drop_opacity", this.NewPlayerVariable<float>("ui_visualize_drop_opacity", SystemConsole.StoreAs.Float, "Opacity of drop location indicator.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_visualize_drop_opacity", ref ObjectPositioningVisualizer.VisualizerDropAlpha, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("ui_visualize_drop_snap", this.NewPlayerVariable<bool>("ui_visualize_drop_snap", SystemConsole.StoreAs.Bool, "Display drop location indicator when holding object which will snap to a snap point.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_visualize_drop_snap", ref ObjectPositioningVisualizer.VisualizeDropSnap, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("ui_visualize_spawn_hide_window", this.NewPlayerVariable<bool>("ui_visualize_spawn_hide_window", SystemConsole.StoreAs.Bool, "Hide components window while spawning objects.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_visualize_spawn_hide_window", ref ObjectPositioningVisualizer.VisualizerSpawnHideWindow, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("ui_visualize_spawn_in_air", this.NewPlayerVariable<bool>("ui_visualize_spawn_in_air", SystemConsole.StoreAs.Bool, "When ON objects will spawn above the table instead of directly on the table. Holding <CTRL> flips this behaviour.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_visualize_spawn_in_air", ref ObjectPositioningVisualizer.VisualizerSpawnAboveTable, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("ui_visualize_spawn_object_snap", this.NewPlayerVariable<bool>("ui_visualize_spawn_object_snap", SystemConsole.StoreAs.Bool, "When spawning objects while hovering over an object, do they snap to it.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_visualize_spawn_object_snap", ref ObjectPositioningVisualizer.VisualizerSpawnObjectSnap, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("ui_visualize_spawn_opacity", this.NewPlayerVariable<float>("ui_visualize_spawn_opacity", SystemConsole.StoreAs.Float, "Opacity of spawn location indicator.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_visualize_spawn_opacity", ref ObjectPositioningVisualizer.VisualizerSpawnAlpha, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("ui_visualize_spawn_right_click_ends", this.NewPlayerVariable<bool>("ui_visualize_spawn_right_click_ends", SystemConsole.StoreAs.Bool, "Exit Spawn mode by hitting right-click.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("ui_visualize_spawn_right_click_ends", ref ObjectPositioningVisualizer.VisualizerSpawnEndOnRightClick, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("unbind", this.NewPlayerCommand("unbind", "Unbinds a key.", "USAGE: $CMD$ {-a} <key>\nRemoves any command bound to <key>. If '-a' specified then unbinds all versions of <key> (+-!).", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text7 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text7 == null)
			{
				this.Log("You must provide a <key>.", false);
				return;
			}
			if (text7 == "-a")
			{
				text7 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
				this.ExecuteCommand("unbind +" + text7, ref status, SystemConsole.CommandEcho.Quiet);
				this.ExecuteCommand("unbind -" + text7, ref status, SystemConsole.CommandEcho.Quiet);
				this.ExecuteCommand("unbind !" + text7, ref status, SystemConsole.CommandEcho.Quiet);
				return;
			}
			bool flag = false;
			if (Regex.IsMatch(text7, "^[-+!]?vr", RegexOptions.IgnoreCase))
			{
				flag = true;
			}
			ModifiedKeyCode key = new ModifiedKeyCode(KeyCode.None, false, false, false);
			Dictionary<ModifiedKeyCode, string> dictionary = this.KeyPressBinds;
			VRTrackedController.VRKeyCode key2 = VRTrackedController.VRKeyCode.None;
			Dictionary<VRTrackedController.VRKeyCode, string> dictionary2 = this.VRKeyPressBinds;
			string str2 = "";
			if (text7.StartsWith("-"))
			{
				if (flag)
				{
					dictionary2 = this.VRKeyReleaseBinds;
				}
				else
				{
					dictionary = this.KeyReleaseBinds;
				}
				str2 = "-";
				text7 = text7.Substring(1);
			}
			else if (text7.StartsWith("+"))
			{
				str2 = "+";
				text7 = text7.Substring(1);
			}
			else if (text7.StartsWith("!"))
			{
				if (!flag)
				{
					return;
				}
				dictionary2 = this.VRKeyLongPressBinds;
				str2 = "!";
				text7 = text7.Substring(1);
			}
			if (flag)
			{
				text7 = "VR" + LibString.CamelCaseFromUnderscore(text7, true, false).Substring(2);
			}
			try
			{
				if (flag)
				{
					key2 = (VRTrackedController.VRKeyCode)Enum.Parse(typeof(VRTrackedController.VRKeyCode), text7);
				}
				else
				{
					key = LibKeyCode.ModifiedKeyCodeFromString(text7);
				}
			}
			catch
			{
				this.Log("Key <" + text7 + "> not recognized.", false);
				return;
			}
			if (flag)
			{
				dictionary2.Remove(key2);
			}
			else
			{
				dictionary.Remove(key);
			}
			if (!status.isSilent)
			{
				this.Log(str2 + text7 + " -> <nothing>", Colour.Orange, false);
			}
		}, ""));
		this.ConsoleCommands.Add("unload_interval", this.NewPlayerVariable<float>("unload_interval", SystemConsole.StoreAs.Float, "Time in seconds between each unload (cleanup the game can cause stutter).  If set to 0 then unload timer is disabled.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("unload_interval", ref NetworkSingleton<ManagerPhysicsObject>.Instance.UnloadInterval, ref status, parameters);
			if (status.isDirty)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.UnloadInterval = Mathf.Max(0f, NetworkSingleton<ManagerPhysicsObject>.Instance.UnloadInterval);
				if (NetworkSingleton<ManagerPhysicsObject>.Instance.UnloadInterval != 0f && NetworkSingleton<ManagerPhysicsObject>.Instance.UnloadInterval < 1f)
				{
					NetworkSingleton<ManagerPhysicsObject>.Instance.UnloadInterval = 1f;
				}
			}
		}, ""));
		this.ConsoleCommands.Add("variables", this.NewPlayerCommand("variables", "Lists all variables and their values.", "USAGE: $CMD$ <prefix>\nDisplays all available commands which are variables and what they are currently set to.\nIf prefix is supplied then only commands which start with it will be displayed.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (status.isSilent)
			{
				return;
			}
			string text7 = LibString.bite(ref parameters, true, ' ', false, false, '\0');
			for (int k = 0; k < this.ConsoleCommandList.Count; k++)
			{
				string text8 = this.ConsoleCommandList[k];
				SystemConsole.ConsoleCommand consoleCommand = this.ConsoleCommands[text8];
				status.Batch();
				if (this.CommandAvailable(consoleCommand) && consoleCommand.type == SystemConsole.CommandType.Variable && (text7 == null || text8.StartsWith(text7)))
				{
					this.ExecuteCommand(text8 + " -variable", ref status, SystemConsole.CommandEcho.Quiet);
				}
			}
		}, ""));
		this.ConsoleCommands.Add("vector_x", this.NewPlayerVariable<float>("vector_x", "Get the X component of a vector variable.", "USAGE: $CMD$ {<variable>}\nIf <variable> specified then set $CMD$ to be its X component.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text7 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text7 == null)
			{
				if (!status.isSilent)
				{
					this.Log("X is " + this.vector.x, false);
				}
				SystemConsole.lastReturnedValue = LibString.StringFromFloat(this.vector.x);
				return;
			}
			if (text7 == "-return")
			{
				status.value = this.vector.x;
				return;
			}
			if (text7 == "-variable")
			{
				if (!status.isSilent)
				{
					this.LogVariable("vector_x", LibString.StringFromFloat(this.vector.x), Colour.Pink, 37);
				}
				return;
			}
			SystemConsole.ConsoleCommand consoleCommand;
			this.ConsoleCommands.TryGetValue(text7, out consoleCommand);
			if (!this.CommandAvailable(consoleCommand))
			{
				this.Log(text7 + " does not exist.", false);
				return;
			}
			if (consoleCommand.type != SystemConsole.CommandType.Variable || (consoleCommand.variableType != typeof(Vector2) && consoleCommand.variableType != typeof(Vector3)))
			{
				this.Log(text7 + " is not a vector variable!", false);
				return;
			}
			this.ExecuteCommand(text7 + " -return", ref status, SystemConsole.CommandEcho.Silent);
			if (consoleCommand.variableType == typeof(Vector2))
			{
				this.vector.x = ((Vector2)status.value).x;
			}
			else
			{
				this.vector.x = ((Vector3)status.value).x;
			}
			status.value = this.vector.x;
			SystemConsole.lastReturnedValue = LibString.StringFromFloat(this.vector.x);
		}, ""));
		this.ConsoleCommands.Add("vector_y", this.NewPlayerVariable<float>("vector_y", "Get the Y component of a vector variable.", "USAGE: $CMD$ {<variable>}\nIf <variable> specified then set $CMD$ to be its Y component.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text7 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text7 == null)
			{
				if (!status.isSilent)
				{
					this.Log("Y is " + this.vector.y, false);
				}
				SystemConsole.lastReturnedValue = LibString.StringFromFloat(this.vector.y);
				return;
			}
			if (text7 == "-return")
			{
				status.value = this.vector.y;
				return;
			}
			if (text7 == "-variable")
			{
				if (!status.isSilent)
				{
					this.LogVariable("vector_y", LibString.StringFromFloat(this.vector.y), Colour.Pink, 37);
				}
				return;
			}
			SystemConsole.ConsoleCommand consoleCommand;
			this.ConsoleCommands.TryGetValue(text7, out consoleCommand);
			if (!this.CommandAvailable(consoleCommand))
			{
				this.Log(text7 + " does not exist.", false);
				return;
			}
			if (consoleCommand.type != SystemConsole.CommandType.Variable || (consoleCommand.variableType != typeof(Vector2) && consoleCommand.variableType != typeof(Vector3)))
			{
				this.Log(text7 + " is not a vector variable!", false);
				return;
			}
			this.ExecuteCommand(text7 + " -return", ref status, SystemConsole.CommandEcho.Silent);
			if (consoleCommand.variableType == typeof(Vector2))
			{
				this.vector.y = ((Vector2)status.value).y;
			}
			else
			{
				this.vector.y = ((Vector3)status.value).y;
			}
			status.value = this.vector.y;
			SystemConsole.lastReturnedValue = LibString.StringFromFloat(this.vector.y);
		}, ""));
		this.ConsoleCommands.Add("vector_z", this.NewPlayerVariable<float>("vector_z", "Get the Z component of a vector variable.", "USAGE: $CMD$ {<variable>}\nIf <variable> specified then set $CMD$ to be its Z component.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text7 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text7 == null)
			{
				if (!status.isSilent)
				{
					this.Log("Z is " + this.vector.z, false);
				}
				SystemConsole.lastReturnedValue = LibString.StringFromFloat(this.vector.z);
				return;
			}
			if (text7 == "-return")
			{
				status.value = this.vector.z;
				return;
			}
			if (text7 == "-variable")
			{
				if (!status.isSilent)
				{
					this.LogVariable("vector_z", LibString.StringFromFloat(this.vector.z), Colour.Pink, 37);
				}
				return;
			}
			SystemConsole.ConsoleCommand consoleCommand;
			this.ConsoleCommands.TryGetValue(text7, out consoleCommand);
			if (!this.CommandAvailable(consoleCommand))
			{
				this.Log(text7 + " does not exist.", false);
				return;
			}
			if (consoleCommand.type != SystemConsole.CommandType.Variable || consoleCommand.variableType != typeof(Vector3))
			{
				this.Log(text7 + " is not a valid vector variable!", false);
				return;
			}
			this.ExecuteCommand(text7 + " -return", ref status, SystemConsole.CommandEcho.Silent);
			this.vector.z = ((Vector3)status.value).z;
			status.value = this.vector.z;
			SystemConsole.lastReturnedValue = LibString.StringFromFloat(this.vector.z);
		}, ""));
		this.ConsoleCommands.Add("version", this.NewPlayerCommand("version", "Displays version information.", "USAGE: $CMD$\nDisplays information on current game version.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (status.isSilent)
			{
				return;
			}
			this.LogVariable("Tabletop Simulator", NetworkSingleton<NetworkUI>.Instance.VersionNumber + ((NetworkSingleton<NetworkUI>.Instance.VersionHotfix == "") ? "" : (" " + NetworkSingleton<NetworkUI>.Instance.VersionHotfix)), Colour.Purple, 37);
			this.LogVariable("Unity", Application.unityVersion, Colour.PinkDark, 37);
			this.LogVariable("Dissonance", DissonanceComms.Version.ToString(), Colour.BlueDark, 37);
			this.LogVariable("SteamVR", "2.2RC5", Colour.OrangeDark, 37);
			this.LogVariable("Steam.NET", "14.0.0", Colour.YellowDark, 37);
			this.LogVariable("Steam.SDK", "1.48", Colour.YellowDark, 37);
			this.LogVariable("Steam.API", "05.69.73.98", Colour.YellowDark, 37);
		}, ""));
		this.ConsoleCommands.Add("vr_active", this.NewPlayerVariable<bool>("vr_active", "Is VR currently active?", "Reports whether game is currently running in VR mode (READ ONLY).", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool enabled = XRSettings.enabled;
			status.ReadOnly();
			this.Variable("vr_active", ref enabled, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("vr_controller_alpha_body", this.NewPlayerVariable<float>("vr_controller_alpha_body", "Sets the opacity of the VR controllers: 0.0 = transparent, 1.0 = opaque.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_controller_alpha_body", ref VRTrackedController.RenderAlpha, ref status, parameters);
			if (status.isDirty)
			{
				if (VRTrackedController.leftVRTrackedController)
				{
					VRTrackedController.leftVRTrackedController.Model.GetComponent<VRReplaceShader>().SetAlpha(VRTrackedController.RenderAlpha);
				}
				if (VRTrackedController.rightVRTrackedController)
				{
					VRTrackedController.rightVRTrackedController.Model.GetComponent<VRReplaceShader>().SetAlpha(VRTrackedController.RenderAlpha);
				}
			}
		}, ""));
		this.ConsoleCommands.Add("vr_controller_alpha_gem", this.NewPlayerVariable<float>("vr_controller_alpha_gem", "Sets the opacity of the VR controller gem: 0.0 = transparent, 1.0 = opaque.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_controller_alpha_gem", ref VRTrackedController.GrabSphereAlpha, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("vr_controller_alpha_icons", this.NewPlayerVariable<float>("vr_controller_alpha_icons", "Sets the opacity of the VR controller icons: 0.0 = transparent, 1.0 = opaque.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_controller_alpha_icons", ref VRTrackedController.IconAlpha, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("vr_controls_original", this.NewPlayerVariable<bool>("vr_controls_original", SystemConsole.StoreAs.Bool, "If ON then controllers use the original control scheme.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool flag = VRTrackedController.ControllerStyle == TrackedControllerStyle.Old;
			this.Variable("vr_controls_original", ref flag, ref status, parameters);
			if (status.isDirty)
			{
				if (flag)
				{
					VRTrackedController.ControllerStyle = TrackedControllerStyle.Old;
					return;
				}
				VRTrackedController.ControllerStyle = TrackedControllerStyle.New;
			}
		}, ""));
		this.ConsoleCommands.Add("vr_display_network_players_off", this.NewPlayerCommand("vr_display_network_players_off", "Turn off displaying other VR players.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			status.Batch();
			this.ExecuteCommand("vr_mode_display_network_players 0", ref status, SystemConsole.CommandEcho.Quiet);
		}, ""));
		this.ConsoleCommands.Add("vr_display_network_players_hands", this.NewPlayerCommand("vr_display_network_players_hands", "Display controllers for other VR players.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			status.Batch();
			this.ExecuteCommand("vr_mode_display_network_players 1", ref status, SystemConsole.CommandEcho.Quiet);
		}, ""));
		this.ConsoleCommands.Add("vr_display_network_players_all", this.NewPlayerCommand("vr_display_network_players_all", "Display controllers and headsets for other VR players.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			status.Batch();
			this.ExecuteCommand("vr_mode_display_network_players 2", ref status, SystemConsole.CommandEcho.Quiet);
		}, ""));
		this.ConsoleCommands.Add("vr_joypad_emulation", this.NewPlayerVariable<bool>("vr_joypad_emulation", SystemConsole.StoreAs.Bool, "If ON then joypad bindings on VR controllers will be enabled, allowing you to use them for in-game actions.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_joypad_emulation", ref VRSteamControllerDevice.ENABLE_JOYPAD_EMULATION, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("vr_grabbing_hides_gem", this.NewPlayerVariable<bool>("vr_grabbing_hides_gem", SystemConsole.StoreAs.Bool, "If ON then the interaction gem will be hidden when you grab an object..", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_grabbing_hides_gem", ref VRTrackedController.HIDE_GEM_ON_GRAB, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("vr_hand_view_angle", this.NewPlayerVariable<float>("vr_hand_view_angle", SystemConsole.StoreAs.Float, "The angle the hand view is offset from the controller.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_hand_view_angle", ref VRVirtualHand.HAND_VIEW_ANGLE, ref status, parameters);
			if (status.isDirty)
			{
				Singleton<VRVirtualHand>.Instance.Refresh();
			}
		}, ""));
		this.ConsoleCommands.Add("vr_hand_view_detach", this.NewPlayerCommand("vr_hand_view_detach", "Detach (turn off) the virtual hand view.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			status.Batch();
			this.ExecuteCommand("vr_mode_hand_view 0", ref status, SystemConsole.CommandEcho.Quiet);
		}, ""));
		this.ConsoleCommands.Add("vr_hand_view_hide_on_grab", this.NewPlayerVariable<bool>("vr_hand_view_hide_on_grab", SystemConsole.StoreAs.Bool, "If ON then the virtual hand will be hidden when its controller is used to grab something.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_hand_view_hide_on_grab", ref VRTrackedController.HIDE_VIRTUAL_HAND_WHEN_GRABBING, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("vr_hand_view_scale", this.NewPlayerVariable<float>("vr_hand_view_scale", SystemConsole.StoreAs.Float, "Scale applied to virtual hand. Must be > 0.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float virtualHandScale = VRVirtualHandObject.VirtualHandScale;
			this.Variable("vr_hand_view_scale", ref virtualHandScale, ref status, parameters);
			if (status.isDirty)
			{
				if (virtualHandScale > 0f)
				{
					VRVirtualHandObject.VirtualHandScale = virtualHandScale;
					Singleton<VRVirtualHandObject>.Instance.Refresh();
					return;
				}
				this.Log("Must be grater than 0", false);
			}
		}, ""));
		this.ConsoleCommands.Add("vr_haptic_test", this.NewPlayerCommand("vr_haptic_test", "Triggers the haptic feedback on the VR controllers.", "USAGE: $CMD$ {<intensity>}", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float num = 1f;
			string text7 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			if (text7 != null && float.TryParse(text7, out num))
			{
				if (num > 100f)
				{
					num *= 0.001f;
				}
				else if (num > 1f)
				{
					num *= 0.01f;
				}
			}
			if (VRTrackedController.leftVRTrackedController)
			{
				VRTrackedController.leftVRTrackedController.controller.TriggerHapticPulse(num);
			}
			if (VRTrackedController.rightVRTrackedController)
			{
				VRTrackedController.rightVRTrackedController.controller.TriggerHapticPulse(num);
			}
		}, ""));
		this.ConsoleCommands.Add("vr_hover_tooltips", this.NewPlayerVariable<bool>("vr_hover_tooltips", SystemConsole.StoreAs.Bool, "When ON tooltips displayed when hovering over UI items will be displayed above the touchpad icons.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_hover_tooltips", ref VRTrackedController.UI_HOVER_TOOLTIPS, ref status, parameters);
			if (status.isDirty)
			{
				if (VRTrackedController.leftVRTrackedController)
				{
					VRTrackedController.leftVRTrackedController.UpdateHoverText(false);
				}
				if (VRTrackedController.rightVRTrackedController)
				{
					VRTrackedController.rightVRTrackedController.UpdateHoverText(false);
				}
			}
		}, ""));
		this.ConsoleCommands.Add("vr_interface_click_threshold", this.NewPlayerVariable<float>("vr_interface_click_threshold", SystemConsole.StoreAs.Float, "When interface click is bound to an analog input it obeys this threshold.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float interface_CLICK_THRESHOLD = VRSteamControllerDevice.INTERFACE_CLICK_THRESHOLD;
			this.Variable("vr_interface_click_threshold", ref interface_CLICK_THRESHOLD, ref status, parameters);
			if (status.isDirty && interface_CLICK_THRESHOLD >= 0f && interface_CLICK_THRESHOLD <= 1f)
			{
				VRSteamControllerDevice.INTERFACE_CLICK_THRESHOLD = interface_CLICK_THRESHOLD;
			}
		}, ""));
		this.ConsoleCommands.Add("vr_interface_repeat_duration", this.NewPlayerVariable<float>("vr_interface_repeat_duration", SystemConsole.StoreAs.Float, "Sets how long you have to hold a button for it to repeat a repeatable action.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float repeat_DURATION = VRTrackedController.REPEAT_DURATION;
			this.Variable("vr_interface_repeat_duration", ref repeat_DURATION, ref status, parameters);
			if (status.isDirty)
			{
				VRTrackedController.REPEAT_DURATION = repeat_DURATION;
			}
		}, ""));
		this.ConsoleCommands.Add("vr_laser_activation_threshold", this.NewPlayerVariable<float>("vr_laser_activation_threshold", SystemConsole.StoreAs.Float, "When laser activation is bound to an analog input it obeys this threshold.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float activate_LASER_THRESHOLD = VRSteamControllerDevice.ACTIVATE_LASER_THRESHOLD;
			this.Variable("vr_laser_activation_threshold", ref activate_LASER_THRESHOLD, ref status, parameters);
			if (status.isDirty && activate_LASER_THRESHOLD >= 0f && activate_LASER_THRESHOLD <= 1f)
			{
				VRSteamControllerDevice.ACTIVATE_LASER_THRESHOLD = activate_LASER_THRESHOLD;
			}
		}, ""));
		this.ConsoleCommands.Add("vr_laser_angled", this.NewPlayerVariable<bool>("vr_laser_angled", SystemConsole.StoreAs.Bool, "If ON then controller laser pointer will always be angled like under original controls.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool flag = VRTrackedController.LASER_ANGLE != 0f;
			this.Variable("vr_laser_angled", ref flag, ref status, parameters);
			if (status.isDirty)
			{
				if (flag || VRTrackedController.ALWAYS_ANGLE_LASER)
				{
					VRTrackedController.LASER_ANGLE = VRTrackedController.LASER_ANGLE_ORIGINAL;
					return;
				}
				VRTrackedController.LASER_ANGLE = 0f;
			}
		}, ""));
		this.ConsoleCommands.Add("vr_laser_beam_opacity", this.NewPlayerVariable<float>("vr_laser_beam_opacity", SystemConsole.StoreAs.Float, "Sets opacity of the laser beam: 0.0 = transparent, 1.0 = opaque.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float laser_BEAM_ALPHA = VRTrackedController.LASER_BEAM_ALPHA;
			this.Variable("vr_laser_beam_opacity", ref laser_BEAM_ALPHA, ref status, parameters);
			if (status.isDirty && laser_BEAM_ALPHA >= 0f && laser_BEAM_ALPHA <= 1f)
			{
				VRTrackedController.LASER_BEAM_ALPHA = laser_BEAM_ALPHA;
			}
		}, ""));
		this.ConsoleCommands.Add("vr_laser_beam_thickness", this.NewPlayerVariable<float>("vr_laser_beam_thickness", SystemConsole.StoreAs.Float, "Sets how thick the laser beam is.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float num = VRLaserPointer.LASER_BEAM_THICKNESS * 100f;
			this.Variable("vr_laser_beam_thickness", ref num, ref status, parameters);
			if (status.isDirty && num > 0f)
			{
				VRLaserPointer.LASER_BEAM_THICKNESS = num * 0.01f;
			}
		}, ""));
		this.ConsoleCommands.Add("vr_laser_beam_visible", this.NewPlayerVariable<bool>("vr_laser_beam_visible", SystemConsole.StoreAs.Bool, "If ON then controller laser pointer beam will be visible. Laser dot is always visible when laser is on.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			bool laser_BEAM_VISIBLE = VRTrackedController.LASER_BEAM_VISIBLE;
			this.Variable("vr_laser_beam_visible", ref laser_BEAM_VISIBLE, ref status, parameters);
			if (status.isDirty)
			{
				VRTrackedController.LaserBeamVisible(laser_BEAM_VISIBLE);
			}
		}, ""));
		this.ConsoleCommands.Add("vr_laser_constant", this.NewPlayerVariable<bool>("vr_laser_constant", SystemConsole.StoreAs.Bool, "If ON then controller laser pointer will always be on like under original controls.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_laser_constant", ref VRTrackedController.LASER_ALWAYS_ON, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("vr_laser_dot_size", this.NewPlayerVariable<float>("vr_laser_dot_size", "Sets how large the laser hit dot is.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float num = VRLaserPointer.LASER_DOT_SIZE * 100f;
			this.Variable("vr_laser_dot_size", ref num, ref status, parameters);
			if (status.isDirty && num > 0f)
			{
				VRLaserPointer.LASER_DOT_SIZE = num * 0.01f;
			}
		}, ""));
		this.ConsoleCommands.Add("vr_left_controller_attach_hand_view", this.NewPlayerCommand("vr_left_controller_attach_hand_view", "Attached the virtual hand view to the left controller.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			status.Batch();
			this.ExecuteCommand("vr_mode_hand_view 1", ref status, SystemConsole.CommandEcho.Quiet);
		}, ""));
		this.ConsoleCommands.Add("vr_left_controller_bind_tool_hotkeys", this.NewPlayerVariable<bool>("vr_left_controller_bind_tool_hotkeys", SystemConsole.StoreAs.Bool, "When ON the left controller's default tool selection hotkeys will be bound to its pad.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_left_controller_bind_tool_hotkeys", ref VRTrackedController.LEFT_CONTROLLER_HOTKEYS, ref status, parameters);
			if (status.isDirty && VRTrackedController.leftVRTrackedController)
			{
				VRTrackedController.leftVRTrackedController.BindPadHotkeys(true);
			}
		}, ""));
		this.ConsoleCommands.Add("vr_left_controller_mode_pad_down", this.NewPlayerVariable<int>("vr_left_controller_mode_pad_down", SystemConsole.StoreAs.Int, "Controls whether pad down on the left controller is bindable, displays the tool selecter, or zooms the active object.", "USAGE: $CMD$ <mode>\n<mode> may be one of:\n 0 = Bindable\n 1 = Tool Select\n 2 = Zoom", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			int num = 2;
			if (VRTrackedController.leftVRTrackedController)
			{
				if (VRTrackedController.leftVRTrackedController.touchpadDownMode == VRTrackedController.TouchpadDownMode.ToolSelect)
				{
					num = 1;
				}
				else if (VRTrackedController.leftVRTrackedController.touchpadDownMode == VRTrackedController.TouchpadDownMode.Zoom)
				{
					num = 2;
				}
				else
				{
					num = 0;
				}
			}
			else
			{
				num = PlayerPrefs.GetInt("vr_left_controller_mode_pad_down", num);
			}
			this.Variable("vr_left_controller_mode_pad_down", ref num, ref status, parameters);
			if (!status.isDirty)
			{
				return;
			}
			if (VRTrackedController.leftVRTrackedController)
			{
				if (num == 0)
				{
					VRTrackedController.leftVRTrackedController.touchpadDownMode = VRTrackedController.TouchpadDownMode.Bindable;
				}
				else if (num == 1)
				{
					VRTrackedController.leftVRTrackedController.touchpadDownMode = VRTrackedController.TouchpadDownMode.ToolSelect;
				}
				else
				{
					if (num != 2)
					{
						this.Log("Must be 0, 1 or 2: resetting to previous value.", false);
						status.Clean();
						return;
					}
					VRTrackedController.leftVRTrackedController.touchpadDownMode = VRTrackedController.TouchpadDownMode.Zoom;
				}
				VRTrackedController.RefreshTouchpadIcons();
				return;
			}
			status.Clean();
			if (num >= 0 && num <= 2)
			{
				PlayerPrefs.SetInt("vr_left_controller_mode_pad_down", num);
				return;
			}
			this.Log("Must be 0, 1 or 2: resetting to previous value.", false);
		}, ""));
		this.ConsoleCommands.Add("vr_left_controller_pad_down_bindable", this.NewPlayerCommand("vr_left_controller_pad_down_bindable", "Sets the left controller pad down mode to Bindable.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			status.Batch();
			this.ExecuteCommand("vr_left_controller_mode_pad_down 0", ref status, SystemConsole.CommandEcho.Quiet);
		}, ""));
		this.ConsoleCommands.Add("vr_left_controller_pad_down_tool_select", this.NewPlayerCommand("vr_left_controller_pad_down_tool_select", "Sets the left controller pad down mode to Tool Select.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			status.Batch();
			this.ExecuteCommand("vr_left_controller_mode_pad_down 1", ref status, SystemConsole.CommandEcho.Quiet);
		}, ""));
		this.ConsoleCommands.Add("vr_left_controller_pad_down_zoom", this.NewPlayerCommand("vr_left_controller_pad_down_zoom", "Sets the left controller pad down mode to Zoom.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			status.Batch();
			this.ExecuteCommand("vr_left_controller_mode_pad_down 2", ref status, SystemConsole.CommandEcho.Quiet);
		}, ""));
		this.ConsoleCommands.Add("vr_left_controller_trigger_click_effect", this.NewPlayerCommand("vr_left_controller_trigger_click_effect", "Emulates the trigger click effect for the left controller.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			VRTrackedController.leftVRTrackedController.EmulateTriggerClick = true;
		}, ""));
		this.ConsoleCommands.Add("vr_left_controller_zoom_scale", this.NewPlayerVariable<float>("vr_left_controller_zoom_scale", SystemConsole.StoreAs.Float, "Controls the size of the zoomed object.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float zoomScale = 1f;
			if (VRTrackedController.leftVRTrackedController)
			{
				zoomScale = VRTrackedController.leftVRTrackedController.GetZoomScale();
			}
			this.Variable("vr_left_controller_zoom_scale", ref zoomScale, ref status, parameters);
			if (status.isDirty && VRTrackedController.leftVRTrackedController)
			{
				VRTrackedController.leftVRTrackedController.SetZoomScale(zoomScale);
			}
		}, ""));
		this.ConsoleCommands.Add("vr_mode_display_network_players", this.NewPlayerVariable<int>("vr_mode_display_network_players", SystemConsole.StoreAs.Int, "Determines whether other VR players will be rendered (0 = None, 1 = Controllers, 2 = Head + Controllers).", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			int display_VR_PERIPHERALS = (int)VREnable.DISPLAY_VR_PERIPHERALS;
			this.Variable("vr_mode_display_network_players", ref display_VR_PERIPHERALS, ref status, parameters);
			if (status.isDirty)
			{
				if (display_VR_PERIPHERALS == 0)
				{
					VREnable.DISPLAY_VR_PERIPHERALS = VREnable.VRAvatarDisplay.None;
				}
				else if (display_VR_PERIPHERALS == 1)
				{
					VREnable.DISPLAY_VR_PERIPHERALS = VREnable.VRAvatarDisplay.Hands;
				}
				else
				{
					if (display_VR_PERIPHERALS != 2)
					{
						this.Log("Must be 0, 1 or 2: resetting to previous value.", false);
						status.Clean();
						return;
					}
					VREnable.DISPLAY_VR_PERIPHERALS = VREnable.VRAvatarDisplay.All;
				}
				foreach (VRAvatarDevice vravatarDevice in UnityEngine.Object.FindObjectsOfType<VRAvatarDevice>())
				{
					if (VREnable.DISPLAY_VR_PERIPHERALS == VREnable.VRAvatarDisplay.All || (VREnable.DISPLAY_VR_PERIPHERALS == VREnable.VRAvatarDisplay.Hands && vravatarDevice.IsController))
					{
						vravatarDevice.SetAlpha(0.33f);
					}
					else
					{
						vravatarDevice.SetAlpha(0f);
					}
				}
			}
		}, ""));
		this.ConsoleCommands.Add("vr_mode_hand_view", this.NewPlayerVariable<int>("vr_mode_hand_view", SystemConsole.StoreAs.Int, "Controls whether the virtual hand view is disabled, or attached to the left or right controller.", "USAGE: $CMD$ <mode>\n<mode> may be one of:\n 0 = Detached (turned off)\n 1 = Attached to Left controller\n 2 = Attached to Right controller", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			int attach = (int)Singleton<VRVirtualHand>.Instance.Attach;
			this.Variable("vr_mode_hand_view", ref attach, ref status, parameters);
			if (status.isDirty)
			{
				VRControllerAttachment virtualHandAttachment;
				try
				{
					virtualHandAttachment = (VRControllerAttachment)attach;
				}
				catch
				{
					this.Log("Must be 0, 1 or 2: resetting to previous value.", false);
					return;
				}
				Singleton<VRVirtualHand>.Instance.SetVirtualHandAttachment(virtualHandAttachment);
			}
		}, ""));
		this.ConsoleCommands.Add("vr_mode_icon_colored", this.NewPlayerVariable<bool>("vr_mode_icon_colored", SystemConsole.StoreAs.Bool, "If ON then the icon showing the current tool mode will be colored instead of black.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_mode_icon_colored", ref VRTrackedController.TOOL_ICON_COLORED, ref status, parameters);
			if (status.isDirty)
			{
				VRTrackedController.UpdateToolIconColor();
			}
		}, ""));
		this.ConsoleCommands.Add("vr_mode_selection_style", this.NewPlayerVariable<int>("vr_mode_selection_style", SystemConsole.StoreAs.Int, "Chooses how drag-selection works in VR: a box of fixed height, a box drawn by the player, or a box anchored to the table.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			int vrselectionMode = (int)VRTrackedController.VRSelectionMode;
			this.Variable("vr_mode_selection_style", ref vrselectionMode, ref status, parameters);
			if (status.isDirty)
			{
				VRTrackedController.VRSelectionStyle vrselectionMode2;
				try
				{
					vrselectionMode2 = (VRTrackedController.VRSelectionStyle)vrselectionMode;
				}
				catch
				{
					this.Log("Must be 0, 1 or 2: resetting to previous value.", false);
					return;
				}
				VRTrackedController.VRSelectionMode = vrselectionMode2;
			}
		}, ""));
		this.ConsoleCommands.Add("vr_mode_ui_attachment", this.NewPlayerVariable<int>("vr_mode_ui_attachment", SystemConsole.StoreAs.Int, "Controls whether the screen UI is detached, or attached to the left or right controller.", "USAGE: $CMD$ <mode>\n<mode> may be one of:\n 0 = Detached\n 1 = Attached to Left controller\n 2 = Attached to Right controller", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			VRUI component = Singleton<VRHMD>.Instance.VRUI.GetComponent<VRUI>();
			int vruiattached = (int)component.VRUIAttached;
			this.Variable("vr_mode_ui_attachment", ref vruiattached, ref status, parameters);
			if (status.isDirty)
			{
				VRControllerAttachment attachment;
				try
				{
					attachment = (VRControllerAttachment)vruiattached;
				}
				catch
				{
					this.Log("Must be 0, 1 or 2: resetting to previous value.", false);
					return;
				}
				component.SetAttachment(attachment);
			}
		}, ""));
		this.ConsoleCommands.Add("vr_move_with_inertia", this.NewPlayerVariable<bool>("vr_move_with_inertia", SystemConsole.StoreAs.Bool, "When ON you will be able to throw yourself around when moving.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_move_with_inertia", ref VRTrackedController.THROW_MOVE, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("vr_move_friction", this.NewPlayerVariable<float>("vr_move_friction", SystemConsole.StoreAs.Float, "Amount of friction applied to inertia when vr_move_with_inertia is ON.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float num = 1f - VRTrackedController.THROW_FRICTION;
			this.Variable("vr_move_friction", ref num, ref status, parameters);
			if (status.isDirty)
			{
				if (num >= 0f && num <= 1f)
				{
					VRTrackedController.THROW_FRICTION = 1f - num;
					return;
				}
				this.Log("Must be beteen 0 and 1", false);
			}
		}, ""));
		this.ConsoleCommands.Add("vr_orient_object_delay", this.NewPlayerVariable<float>("vr_orient_object_delay", SystemConsole.StoreAs.Float, "Delay in seconds after pickup in which the click effect will not register.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_orient_object_delay", ref VRTrackedController.ORIENT_OBJECT_DELAY, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("vr_right_controller_attach_hand_view", this.NewPlayerCommand("vr_right_controller_attach_hand_view", "Attached the virtual hand view to the right controller.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			status.Batch();
			this.ExecuteCommand("vr_mode_hand_view 2", ref status, SystemConsole.CommandEcho.Quiet);
		}, ""));
		this.ConsoleCommands.Add("vr_right_controller_bind_tool_hotkeys", this.NewPlayerVariable<bool>("vr_right_controller_bind_tool_hotkeys", SystemConsole.StoreAs.Bool, "When ON the right controller's default tool selection hotkeys will be bound to its pad.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_right_controller_bind_tool_hotkeys", ref VRTrackedController.RIGHT_CONTROLLER_HOTKEYS, ref status, parameters);
			if (status.isDirty && VRTrackedController.rightVRTrackedController)
			{
				VRTrackedController.rightVRTrackedController.BindPadHotkeys(true);
			}
		}, ""));
		this.ConsoleCommands.Add("vr_right_controller_mode_pad_down", this.NewPlayerVariable<int>("vr_right_controller_mode_pad_down", SystemConsole.StoreAs.Int, "Controls whether pad down on the right controller is bindable, displays the tool selecter, or zooms the active object.", "USAGE: $CMD$ <mode>\n<mode> may be one of:\n 0 = Bindable\n 1 = Tool Select\n 2 = Zoom", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			int num = 2;
			if (VRTrackedController.rightVRTrackedController)
			{
				if (VRTrackedController.rightVRTrackedController.touchpadDownMode == VRTrackedController.TouchpadDownMode.ToolSelect)
				{
					num = 1;
				}
				else if (VRTrackedController.rightVRTrackedController.touchpadDownMode == VRTrackedController.TouchpadDownMode.Zoom)
				{
					num = 2;
				}
				else
				{
					num = 0;
				}
			}
			else
			{
				num = PlayerPrefs.GetInt("vr_right_controller_mode_pad_down", num);
			}
			this.Variable("vr_right_controller_mode_pad_down", ref num, ref status, parameters);
			if (!status.isDirty)
			{
				return;
			}
			if (VRTrackedController.rightVRTrackedController)
			{
				if (num == 0)
				{
					VRTrackedController.rightVRTrackedController.touchpadDownMode = VRTrackedController.TouchpadDownMode.Bindable;
				}
				else if (num == 1)
				{
					VRTrackedController.rightVRTrackedController.touchpadDownMode = VRTrackedController.TouchpadDownMode.ToolSelect;
				}
				else
				{
					if (num != 2)
					{
						this.Log("Must be 0, 1 or 2: resetting to previous value.", false);
						status.Clean();
						return;
					}
					VRTrackedController.rightVRTrackedController.touchpadDownMode = VRTrackedController.TouchpadDownMode.Zoom;
				}
				VRTrackedController.RefreshTouchpadIcons();
				return;
			}
			status.Clean();
			if (num >= 0 && num <= 2)
			{
				PlayerPrefs.SetInt("vr_right_controller_mode_pad_down", num);
				return;
			}
			this.Log("Must be 0, 1 or 2: resetting to previous value.", false);
		}, ""));
		this.ConsoleCommands.Add("vr_right_controller_pad_down_bindable", this.NewPlayerCommand("vr_right_controller_pad_down_bindable", "Sets the right controller pad down mode to Bindable.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			status.Batch();
			this.ExecuteCommand("vr_right_controller_mode_pad_down 0", ref status, SystemConsole.CommandEcho.Quiet);
		}, ""));
		this.ConsoleCommands.Add("vr_right_controller_pad_down_tool_select", this.NewPlayerCommand("vr_right_controller_pad_down_tool_select", "Sets the right controller pad down mode to Tool Select.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			status.Batch();
			this.ExecuteCommand("vr_right_controller_mode_pad_down 1", ref status, SystemConsole.CommandEcho.Quiet);
		}, ""));
		this.ConsoleCommands.Add("vr_right_controller_pad_down_zoom", this.NewPlayerCommand("vr_right_controller_pad_down_zoom", "Sets the right controller pad down mode to Zoom.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			status.Batch();
			this.ExecuteCommand("vr_right_controller_mode_pad_down 2", ref status, SystemConsole.CommandEcho.Quiet);
		}, ""));
		this.ConsoleCommands.Add("vr_right_controller_trigger_click_effect", this.NewPlayerCommand("vr_right_controller_trigger_click_effect", "Emulates the trigger click effect for the right controller.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			VRTrackedController.rightVRTrackedController.EmulateTriggerClick = true;
		}, ""));
		this.ConsoleCommands.Add("vr_right_controller_zoom_scale", this.NewPlayerVariable<float>("vr_right_controller_zoom_scale", SystemConsole.StoreAs.Float, "Controls the size of the zoomed object.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float zoomScale = 1f;
			if (VRTrackedController.rightVRTrackedController)
			{
				zoomScale = VRTrackedController.rightVRTrackedController.GetZoomScale();
			}
			this.Variable("vr_right_controller_zoom_scale", ref zoomScale, ref status, parameters);
			if (status.isDirty && VRTrackedController.rightVRTrackedController)
			{
				VRTrackedController.rightVRTrackedController.SetZoomScale(zoomScale);
			}
		}, ""));
		this.ConsoleCommands.Add("vr_selection_style_anchored", this.NewPlayerCommand("vr_selection_style_anchored", "Sets VR selection mode to a box which is anchored to the table.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			status.Batch();
			this.ExecuteCommand("vr_mode_selection_style 2", ref status, SystemConsole.CommandEcho.Quiet);
		}, ""));
		this.ConsoleCommands.Add("vr_selection_style_exact", this.NewPlayerCommand("vr_selection_style_exact", "Sets VR selection mode to a box drawn by the player.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			status.Batch();
			this.ExecuteCommand("vr_mode_selection_style 1", ref status, SystemConsole.CommandEcho.Quiet);
		}, ""));
		this.ConsoleCommands.Add("vr_selection_style_fixed", this.NewPlayerCommand("vr_selection_style_fixed", "Sets VR selection mode to a box of fixed height (which is set with vr_selection_height).", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			status.Batch();
			this.ExecuteCommand("vr_mode_selection_style 0", ref status, SystemConsole.CommandEcho.Quiet);
		}, ""));
		this.ConsoleCommands.Add("vr_selection_height", this.NewPlayerVariable<float>("vr_selection_height", SystemConsole.StoreAs.Float, "Sets height of VR selection box when not using exact selection.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_selection_height", ref VRTrackedController.SELECTION_HEIGHT, ref status, parameters);
			if (status.isDirty)
			{
				VRTrackedController.SELECTION_HEIGHT = Mathf.Abs(VRTrackedController.SELECTION_HEIGHT);
			}
		}, ""));
		this.ConsoleCommands.Add("vr_show_missing_binding_warning", this.NewPlayerVariable<bool>("vr_show_missing_binding_warning", SystemConsole.StoreAs.Bool, "When ON, and when the GRAB and MAIN MENU actions are not bound, displays a warning advising how to set bindings.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_show_missing_binding_warning", ref VREnable.SHOW_MISSING_BINDING_WARNING, ref status, parameters);
			if (status.isDirty && !VREnable.SHOW_MISSING_BINDING_WARNING)
			{
				VREnable.Instance.HideMissingBindingsWarning();
			}
		}, ""));
		this.ConsoleCommands.Add("vr_spectator_replaces_main_window", this.NewPlayerVariable<bool>("vr_spectator_replaces_main_window", SystemConsole.StoreAs.Bool, "If ON then the spectator view will replace the normal display mirror.  If OFF then a second screen will open (requires second monitor).", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_spectator_replaces_main_window", ref SpectatorCamera.VR_OVERRIDES_MAIN_WINDOW, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("vr_steamvr_bindings", this.NewPlayerCommand("vr_steamvr_bindings", "Lists all current SteamVR bindings.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			if (!VRHMD.isVR)
			{
				this.Log("Not in VR!", false);
				return;
			}
			for (int k = 0; k < SteamVR_Input.actions.Length; k++)
			{
				SteamVR_Action_Boolean steamVR_Action_Boolean = SteamVR_Input.actionsBoolean[k];
				if (steamVR_Action_Boolean.active)
				{
					string localizedOriginPart = steamVR_Action_Boolean.GetLocalizedOriginPart(SteamVR_Input_Sources.LeftHand, new EVRInputStringBits[]
					{
						EVRInputStringBits.VRInputString_InputSource
					});
					if (localizedOriginPart != "")
					{
						this.LogVariable(steamVR_Action_Boolean.GetShortName(), "Left " + localizedOriginPart, Colour.YellowDark, 37);
					}
					localizedOriginPart = steamVR_Action_Boolean.GetLocalizedOriginPart(SteamVR_Input_Sources.RightHand, new EVRInputStringBits[]
					{
						EVRInputStringBits.VRInputString_InputSource
					});
					if (localizedOriginPart != "")
					{
						this.LogVariable(steamVR_Action_Boolean.GetShortName(), "Right " + localizedOriginPart, Colour.YellowDark, 37);
					}
				}
			}
		}, ""));
		this.ConsoleCommands.Add("vr_scale_rotate_rate", this.NewPlayerVariable<Vector3>("vr_scale_rotate_rate", "When you scale/rotate in VR your movement is smoothed.  This sets how fast you approach the desired position.\nThree values: <scale> <rotation> <position>", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			Vector3 vector = new Vector3(VRTransform.ScaleLerp, VRTransform.RotateLerp, VRTransform.TranslateLerp);
			this.Variable("vr_scale_rotate_rate", ref vector, ref status, parameters);
			if (status.isDirty)
			{
				if (vector.x > 0f && vector.y > 0f && vector.z > 0f)
				{
					VRTransform.ScaleLerp = vector.x;
					VRTransform.RotateLerp = vector.y;
					VRTransform.TranslateLerp = vector.z;
					return;
				}
				this.Log("Must be > 0", false);
			}
		}, ""));
		this.ConsoleCommands.Add("vr_sticky_grab", this.NewPlayerVariable<bool>("vr_sticky_grab", SystemConsole.StoreAs.Bool, "If ON then grabbing control becomes sticky: press to grab and press again to release.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_sticky_grab", ref VRTrackedController.STICKY_GRAB, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("vr_teleport_with_pad", this.NewPlayerVariable<bool>("vr_teleport_with_pad", SystemConsole.StoreAs.Bool, "If ON then pushing up on the pad will let you teleport, if OFF you may 'bind' it.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_teleport_with_pad", ref VRTrackedController.UP_IS_TELEPORT, ref status, parameters);
			if (status.isDirty)
			{
				VRTrackedController.RefreshTouchpadIcons();
			}
		}, ""));
		this.ConsoleCommands.Add("vr_thumbstick_icons_constant", this.NewPlayerVariable<bool>("vr_thumbstick_icons_constant", SystemConsole.StoreAs.Bool, "If ON then VR thumbstick icons are always displayed.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_thumbstick_icons_constant", ref VRTrackedController.TOUCHPAD_ICONS_ALWAYS_ON, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("vr_tooltips_action_enabled", this.NewPlayerVariable<bool>("vr_tooltips_action_enabled", SystemConsole.StoreAs.Bool, "If ON then VR controller tooltips are displayed whenever the Display Tooltips action is activated.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_tooltips_action_enabled", ref VRTrackedController.ENABLE_TOOLTIP_ACTION, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("vr_tooltips_initial_duration", this.NewPlayerVariable<float>("vr_tooltips_initial_duration", SystemConsole.StoreAs.Float, "Duration VR controller tooltip displays for at startup (when not set to be always-on).", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_tooltips_initial_duration", ref VRTrackedController.HIDE_TOOLTIPS_DELAY, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("vr_tooltips_constant", this.NewPlayerVariable<bool>("vr_tooltips_constant", SystemConsole.StoreAs.Bool, "If ON then VR controller tooltips are always displayed.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_tooltips_constant", ref VRTrackedController.ALWAYS_DISPLAY_TOOLTIPS, ref status, parameters);
			if (status.isDirty)
			{
				if (VRTrackedController.leftVRTrackedController)
				{
					VRTrackedController.leftVRTrackedController.DisplayTooltips(VRTrackedController.ALWAYS_DISPLAY_TOOLTIPS, true);
				}
				if (VRTrackedController.rightVRTrackedController)
				{
					VRTrackedController.rightVRTrackedController.DisplayTooltips(VRTrackedController.ALWAYS_DISPLAY_TOOLTIPS, true);
				}
			}
		}, ""));
		this.ConsoleCommands.Add("vr_tooltips_for_click_when_on_menu", this.NewPlayerVariable<bool>("vr_tooltips_for_click_when_on_menu", SystemConsole.StoreAs.Bool, "If ON then VR controller tooltips are displayed for click action when on menu.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_tooltips_for_click_when_on_menu", ref VRTrackedController.DISPLAY_CLICK_TOOLTIP_ON_MENU, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("vr_ui_attach_left", this.NewPlayerCommand("vr_ui_attach_left", "Attach VR screen UI to left controller.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			status.Batch();
			this.ExecuteCommand("vr_mode_ui_attachment 1", ref status, SystemConsole.CommandEcho.Quiet);
		}, ""));
		this.ConsoleCommands.Add("vr_ui_attach_right", this.NewPlayerCommand("vr_ui_attach_right", "Attach VR screen UI to right controller.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			status.Batch();
			this.ExecuteCommand("vr_mode_ui_attachment 2", ref status, SystemConsole.CommandEcho.Quiet);
		}, ""));
		this.ConsoleCommands.Add("vr_ui_detached", this.NewPlayerCommand("vr_ui_detached", "Detach VR screen UI.", "USAGE: $CMD$", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			status.Batch();
			this.ExecuteCommand("vr_mode_ui_attachment 0", ref status, SystemConsole.CommandEcho.Quiet);
		}, ""));
		this.ConsoleCommands.Add("vr_ui_floating", this.NewPlayerVariable<bool>("vr_ui_floating", SystemConsole.StoreAs.Bool, "When ON the VR UI screen will float in world space, or be attached to a controller (When OFF it will surround the room).", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_ui_floating", ref VRHMD.FLOATING, ref status, parameters);
			if (status.isDirty && VRHMD.FLOATING != Singleton<VRHMD>.Instance.floatingUI && Singleton<VRHMD>.Instance.Initialized)
			{
				Singleton<VRHMD>.Instance.ToggleUIMode();
			}
		}, ""));
		this.ConsoleCommands.Add("vr_ui_scale", this.NewPlayerVariable<float>("vr_ui_scale", SystemConsole.StoreAs.Float, "Sets size of VR UI screen when attached to controller.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			VRUI component = Singleton<VRHMD>.Instance.VRUI.GetComponent<VRUI>();
			float num = component.ActiveScale * 1000f;
			this.Variable("vr_ui_scale", ref num, ref status, parameters);
			if (status.isDirty)
			{
				component.ActiveScale = num / 1000f;
			}
		}, ""));
		this.ConsoleCommands.Add("vr_ui_suppressed", this.NewPlayerVariable<bool>("vr_ui_suppressed", "When ON the VR UI screen will be hidden (but only if attached to a controller).", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_ui_suppressed", ref VRUI.SUPPRESS_SCREEN, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("vr_unbind_all", this.NewPlayerCommand("vr_unbind_all", "Removes all VR pad bindings.", "Removes all commands bound to the VR controller pad.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.VRKeyPressBinds.Clear();
			this.VRKeyReleaseBinds.Clear();
			this.VRKeyLongPressBinds.Clear();
		}, ""));
		this.ConsoleCommands.Add("vr_tilt_angle", this.NewPlayerVariable<float>("vr_tilt_angle", SystemConsole.StoreAs.Float, "Angle to use when tilting world.  It is advisable to turn tilt mode off while changing this setting!", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			float wall_ANGLE = VRHMD.WALL_ANGLE;
			this.Variable("vr_tilt_angle", ref wall_ANGLE, ref status, parameters);
			if (status.isDirty)
			{
				if (wall_ANGLE < 10f || wall_ANGLE > 90f)
				{
					this.Log("vr_wall_angle must be between 10 and 90", false);
					return;
				}
				if (VRHMD.WALL_MODE)
				{
					Singleton<VRHMD>.Instance.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
					VRTrackedController.leftVRTrackedController.VRCameraRig.transform.position = Quaternion.Euler(-VRHMD.WALL_ANGLE, 0f, 0f) * VRTrackedController.leftVRTrackedController.VRCameraRig.transform.position;
					VRHMD.WALL_ANGLE = wall_ANGLE;
					Singleton<VRHMD>.Instance.transform.rotation = Quaternion.Euler(VRHMD.WALL_ANGLE, 0f, 0f);
					VRTrackedController.leftVRTrackedController.VRCameraRig.transform.position = Quaternion.Euler(VRHMD.WALL_ANGLE, 0f, 0f) * VRTrackedController.leftVRTrackedController.VRCameraRig.transform.position;
				}
			}
		}, ""));
		this.ConsoleCommands.Add("vr_tilt_mode", this.NewPlayerVariable<bool>("vr_tilt_mode", "Rotate the world around the Z axis.  Caution!", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_tilt_mode", ref VRHMD.WALL_MODE, ref status, parameters);
			if (status.isDirty && VRHMD.isVR)
			{
				if (VRHMD.WALL_MODE)
				{
					if (Mathf.Abs(Singleton<VRHMD>.Instance.transform.rotation.eulerAngles.x) < 5f)
					{
						Singleton<VRHMD>.Instance.transform.rotation = Quaternion.Euler(VRHMD.WALL_ANGLE, 0f, 0f);
						VRTrackedController.leftVRTrackedController.VRCameraRig.transform.position = Quaternion.Euler(VRHMD.WALL_ANGLE, 0f, 0f) * VRTrackedController.leftVRTrackedController.VRCameraRig.transform.position;
						return;
					}
				}
				else if (Mathf.Abs(Singleton<VRHMD>.Instance.transform.rotation.eulerAngles.x) >= 5f)
				{
					Singleton<VRHMD>.Instance.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
					VRTrackedController.leftVRTrackedController.VRCameraRig.transform.position = Quaternion.Euler(-VRHMD.WALL_ANGLE, 0f, 0f) * VRTrackedController.leftVRTrackedController.VRCameraRig.transform.position;
				}
			}
		}, ""));
		this.ConsoleCommands.Add("vr_zoom_object_aligned", this.NewPlayerVariable<bool>("vr_zoom_object_aligned", SystemConsole.StoreAs.Bool, "If ON then when you active the object zoom on a controller, the magnified object will appear with the same orientation as the game object.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("vr_zoom_object_aligned", ref VRTrackedController.ALIGN_ZOOMED_OBJECT, ref status, parameters);
			if (status.isDirty && !VRTrackedController.ALIGN_ZOOMED_OBJECT)
			{
				if (VRTrackedController.leftVRTrackedController)
				{
					VRTrackedController.leftVRTrackedController.ZoomObject.transform.rotation = Quaternion.identity;
				}
				if (VRTrackedController.rightVRTrackedController)
				{
					VRTrackedController.rightVRTrackedController.ZoomObject.transform.rotation = Quaternion.identity;
				}
			}
		}, ""));
		this.ConsoleCommands.Add("wait", this.NewPlayerCommand("wait", "Waits for the specified amount of time.  Useful in scripts.", "USAGE: $CMD$ <delay>\nWaits for <delay> seconds before executing the next command in a script.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			string text7 = LibString.bite(ref parameters, false, ' ', false, false, '\0');
			float delay;
			if (float.TryParse(text7, out delay))
			{
				status.Open();
				base.StartCoroutine(this.DelayedDone(delay, status));
				return;
			}
			this.Log("Unable to parse float: " + text7, false);
		}, ""));
		this.ConsoleCommands.Add("zoom_always", this.NewPlayerVariable<bool>("zoom_always", SystemConsole.StoreAs.Bool, "When ON (and when zoom_follows_pointer is OFF) the zoom display will always be displayed.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("zoom_always", ref AltZoomCamera.AltZoomAlwaysOn, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("zoom_follow_pointer", this.NewPlayerVariable<bool>("zoom_follow_pointer", SystemConsole.StoreAs.Bool, "When ON the zoom display will appear at the pointer location. When OFF it will appear at zoom_position.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			this.Variable("zoom_follow_pointer", ref AltZoomCamera.AltZoomFollowsPointer, ref status, parameters);
		}, ""));
		this.ConsoleCommands.Add("zoom_position", this.NewPlayerVariable<Vector2>("zoom_position", "The location the zoom display will appear when zoom_follows_pointer is off.  0,0 = bottom left, 1,1 = top right.", delegate(SystemConsole.CommandStatus status, string parameters)
		{
			Vector2 altZoomLocation = AltZoomCamera.AltZoomLocation;
			this.Variable("zoom_position", ref altZoomLocation, ref status, parameters);
			if (status.isDirty)
			{
				if (altZoomLocation.x < 0f || altZoomLocation.x > 1f || altZoomLocation.y < 0f || altZoomLocation.y > 1f)
				{
					this.Log("Must be in range 0.0 - 1.0", false);
					return;
				}
				AltZoomCamera.AltZoomLocation = altZoomLocation;
			}
		}, ""));
	}

	// Token: 0x06001D91 RID: 7569 RVA: 0x000D1FCC File Offset: 0x000D01CC
	public SystemConsole()
	{
		Dictionary<string, Func<string>> dictionary = new Dictionary<string, Func<string>>();
		dictionary.Add("chat_filter", () => "true");
		dictionary.Add("default_host_name", () => "\"\"");
		dictionary.Add("default_host_password", () => "\"\"");
		dictionary.Add("language", () => "English");
		dictionary.Add("lift_height", () => "0.2");
		dictionary.Add("mod_thread_count", () => Mathf.Max(SystemInfo.processorCount - 1, 1).ToString());
		dictionary.Add("rotation_degrees", () => "15");
		dictionary.Add("ui_server_browser_search", () => "");
		dictionary.Add("ui_theme_index", () => "0");
		dictionary.Add("ui_theme_name", () => "Light");
		this.ManualDefaultValues = dictionary;
		this.EvalAllowedFunctions = new List<string>
		{
			"abs",
			"acos",
			"asin",
			"atan",
			"atan2",
			"ceil",
			"cos",
			"cosh",
			"deg",
			"exp",
			"floor",
			"fmod",
			"frexp",
			"ldexp",
			"log",
			"max",
			"min",
			"modf",
			"pow",
			"rad",
			"random",
			"sin",
			"sinh",
			"sqrt",
			"tan",
			"tanh",
			"pi"
		};
		this.ConsoleCommands = new Dictionary<string, SystemConsole.ConsoleCommand>();
		this.ConsoleCommandList = new List<string>();
		this.StoreTextAliases = new List<string>
		{
			"store_text"
		};
		this.UserStringVars = new Dictionary<string, string>();
		this.UserBoolVars = new Dictionary<string, bool>();
		this.UserFloatVars = new Dictionary<string, float>();
		this.KeyPressBinds = new Dictionary<ModifiedKeyCode, string>();
		this.KeyReleaseBinds = new Dictionary<ModifiedKeyCode, string>();
		this.VRKeyPressBinds = new Dictionary<VRTrackedController.VRKeyCode, string>();
		this.VRKeyReleaseBinds = new Dictionary<VRTrackedController.VRKeyCode, string>();
		this.VRKeyLongPressBinds = new Dictionary<VRTrackedController.VRKeyCode, string>();
		this.currentlyRunning = new List<SystemConsole.ConsoleCommand>();
		this.toggleOnExecute = new Dictionary<string, string>();
		this.toggleOffExecute = new Dictionary<string, string>();
		this.uiToggleToUpdate = new Dictionary<string, UIToggle>();
		this.UIAnchor = Vector2.zero;
		this.LogTags = new Dictionary<string, SystemConsole.LogTag>();
		this.LogTagsIncluded = new List<string>();
		this.LogTagsExcluded = new List<string>();
		this.LogTagsHighlight = new List<string>();
		this.DefaultLogTag = new SystemConsole.LogTag("", Colour.White, "", "");
		this.HighlightTag = new SystemConsole.LogTag("", Colour.Orange, "\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\", "////////////////////");
		this.CommandsAwaitingOnLoad = new List<SystemConsole.CommandStatus>();
		base..ctor();
	}

	// Token: 0x040012B2 RID: 4786
	public Material PureModePrimaryMaterial;

	// Token: 0x040012B3 RID: 4787
	public Material PureModeSecondaryMaterial;

	// Token: 0x040012B4 RID: 4788
	public Material PureModeSplashMaterial;

	// Token: 0x040012B5 RID: 4789
	public static bool RestrictErrorsToConsole = false;

	// Token: 0x040012B6 RID: 4790
	public static bool DisableErrorBroadcast = false;

	// Token: 0x040012B7 RID: 4791
	public static bool doneInitializing = false;

	// Token: 0x040012B8 RID: 4792
	public static bool SpectatorPostProcessing = true;

	// Token: 0x040012B9 RID: 4793
	public static bool AutofocusSearch = false;

	// Token: 0x040012BA RID: 4794
	public static bool CheckForMatchingCustomObjects = true;

	// Token: 0x040012BB RID: 4795
	public static bool EnhancedFigurinePrecision = false;

	// Token: 0x040012BC RID: 4796
	public static float GizmoScaleMultiplier = 1f;

	// Token: 0x040012BD RID: 4797
	public TranslationGizmo TranslationGizmo;

	// Token: 0x040012BE RID: 4798
	public ScaleGizmo ScaleGizmo;

	// Token: 0x040012BF RID: 4799
	public VolumeScaleGizmo VolumeGizmo;

	// Token: 0x040012C0 RID: 4800
	public RotationGizmo RotationGizmo;

	// Token: 0x040012C1 RID: 4801
	private const int SYSTEM_CONSOLE_OFFSET = 34;

	// Token: 0x040012C2 RID: 4802
	private const int VARIABLE_COMMAND_PADDING = 37;

	// Token: 0x040012C3 RID: 4803
	private const float WAIT_ITERATION_DELAY = 0.5f;

	// Token: 0x040012C4 RID: 4804
	private const float WAIT_ITERATION_TIMEOUT = 15f;

	// Token: 0x040012C5 RID: 4805
	public static bool DEBUG_EXTERNAL_API = false;

	// Token: 0x040012C6 RID: 4806
	public const int DEBUG_MESSAGES_MAX = 100;

	// Token: 0x040012C7 RID: 4807
	public static string[] DebugMessages = new string[100];

	// Token: 0x040012C8 RID: 4808
	public static int DebugMessageHead = 0;

	// Token: 0x040012C9 RID: 4809
	public static int DebugMessageTail = 0;

	// Token: 0x040012CA RID: 4810
	public GameObject[] UIGamesRows;

	// Token: 0x040012CB RID: 4811
	public Renderer DisableForFog;

	// Token: 0x040012CC RID: 4812
	public GameObject EnableForFog;

	// Token: 0x040012CD RID: 4813
	public static bool Fog = false;

	// Token: 0x040012CE RID: 4814
	private NetworkPhysicsObject foundObject;

	// Token: 0x040012CF RID: 4815
	private GameObject examinedObject;

	// Token: 0x040012D0 RID: 4816
	private string examinedObjectID;

	// Token: 0x040012D1 RID: 4817
	private Vector3 vector;

	// Token: 0x040012D2 RID: 4818
	private GameObject Graphy;

	// Token: 0x040012D3 RID: 4819
	public static bool ShowGraphy = false;

	// Token: 0x040012D4 RID: 4820
	[HideInInspector]
	public static Colour OutputColour = Colour.White;

	// Token: 0x040012D5 RID: 4821
	[HideInInspector]
	public SystemConsole.CommandEcho echoMode;

	// Token: 0x040012D6 RID: 4822
	[HideInInspector]
	public bool isDeveloper;

	// Token: 0x040012D7 RID: 4823
	[HideInInspector]
	public bool confirmOveride;

	// Token: 0x040012D8 RID: 4824
	[HideInInspector]
	public bool lockHotkey;

	// Token: 0x040012D9 RID: 4825
	public static int TimesStarted = 0;

	// Token: 0x040012DA RID: 4826
	public static string lastReturnedValue = "";

	// Token: 0x040012DB RID: 4827
	public static bool plebMode = false;

	// Token: 0x040012DC RID: 4828
	public static bool dontFocusNextSwitch = false;

	// Token: 0x040012DD RID: 4829
	[HideInInspector]
	public Dictionary<ChatMessageType, bool> mirrorTab = new Dictionary<ChatMessageType, bool>
	{
		{
			ChatMessageType.All,
			false
		},
		{
			ChatMessageType.Game,
			false
		},
		{
			ChatMessageType.Global,
			false
		},
		{
			ChatMessageType.System,
			false
		},
		{
			ChatMessageType.Team,
			false
		}
	};

	// Token: 0x040012DE RID: 4830
	public ChatTab SystemTab;

	// Token: 0x040012DF RID: 4831
	private readonly Dictionary<string, Func<string>> ManualDefaultValues;

	// Token: 0x040012E0 RID: 4832
	private readonly List<string> EvalAllowedFunctions;

	// Token: 0x040012E1 RID: 4833
	private const string EvalTokenCharacters = "abcdefghijkjlmnopqrstuvwxyz_";

	// Token: 0x040012E2 RID: 4834
	private const string EvalTokenExtendCharacters = "abcdefghijkjlmnopqrstuvwxyz_1234567890.";

	// Token: 0x040012E3 RID: 4835
	private static Dictionary<string, object> Defaults = new Dictionary<string, object>();

	// Token: 0x040012E4 RID: 4836
	[HideInInspector]
	public Dictionary<string, SystemConsole.ConsoleCommand> ConsoleCommands;

	// Token: 0x040012E5 RID: 4837
	[HideInInspector]
	public List<string> ConsoleCommandList;

	// Token: 0x040012E6 RID: 4838
	[HideInInspector]
	public List<string> StoreTextAliases;

	// Token: 0x040012E7 RID: 4839
	[HideInInspector]
	public Dictionary<string, string> UserStringVars;

	// Token: 0x040012E8 RID: 4840
	[HideInInspector]
	public Dictionary<string, bool> UserBoolVars;

	// Token: 0x040012E9 RID: 4841
	[HideInInspector]
	public Dictionary<string, float> UserFloatVars;

	// Token: 0x040012EA RID: 4842
	[HideInInspector]
	public Dictionary<ModifiedKeyCode, string> KeyPressBinds;

	// Token: 0x040012EB RID: 4843
	[HideInInspector]
	public Dictionary<ModifiedKeyCode, string> KeyReleaseBinds;

	// Token: 0x040012EC RID: 4844
	[HideInInspector]
	public Dictionary<VRTrackedController.VRKeyCode, string> VRKeyPressBinds;

	// Token: 0x040012ED RID: 4845
	[HideInInspector]
	public Dictionary<VRTrackedController.VRKeyCode, string> VRKeyReleaseBinds;

	// Token: 0x040012EE RID: 4846
	[HideInInspector]
	public Dictionary<VRTrackedController.VRKeyCode, string> VRKeyLongPressBinds;

	// Token: 0x040012EF RID: 4847
	private List<SystemConsole.ConsoleCommand> currentlyRunning;

	// Token: 0x040012F0 RID: 4848
	private Dictionary<string, string> toggleOnExecute;

	// Token: 0x040012F1 RID: 4849
	private Dictionary<string, string> toggleOffExecute;

	// Token: 0x040012F2 RID: 4850
	public Dictionary<string, UIToggle> uiToggleToUpdate;

	// Token: 0x040012F3 RID: 4851
	private Vector2 UIAnchor;

	// Token: 0x040012F4 RID: 4852
	[HideInInspector]
	public Dictionary<string, SystemConsole.LogTag> LogTags;

	// Token: 0x040012F5 RID: 4853
	[HideInInspector]
	public List<string> LogTagsIncluded;

	// Token: 0x040012F6 RID: 4854
	[HideInInspector]
	public List<string> LogTagsExcluded;

	// Token: 0x040012F7 RID: 4855
	[HideInInspector]
	public List<string> LogTagsHighlight;

	// Token: 0x040012F8 RID: 4856
	[HideInInspector]
	public SystemConsole.LogTag DefaultLogTag;

	// Token: 0x040012F9 RID: 4857
	[HideInInspector]
	public SystemConsole.LogTag HighlightTag;

	// Token: 0x040012FA RID: 4858
	[HideInInspector]
	public SystemConsole.LogFormat logFormat;

	// Token: 0x040012FB RID: 4859
	[HideInInspector]
	public string logTablePrefix;

	// Token: 0x040012FC RID: 4860
	[HideInInspector]
	public int logTableDepth;

	// Token: 0x040012FD RID: 4861
	[HideInInspector]
	public bool logDisplayTag;

	// Token: 0x040012FE RID: 4862
	[HideInInspector]
	private List<SystemConsole.CommandStatus> CommandsAwaitingOnLoad;

	// Token: 0x020006D3 RID: 1747
	public enum CommandPermission
	{
		// Token: 0x04002978 RID: 10616
		Player,
		// Token: 0x04002979 RID: 10617
		Admin,
		// Token: 0x0400297A RID: 10618
		Developer
	}

	// Token: 0x020006D4 RID: 1748
	public enum CommandType
	{
		// Token: 0x0400297C RID: 10620
		Command,
		// Token: 0x0400297D RID: 10621
		Variable
	}

	// Token: 0x020006D5 RID: 1749
	public enum CommandEcho
	{
		// Token: 0x0400297F RID: 10623
		Loud,
		// Token: 0x04002980 RID: 10624
		Quiet,
		// Token: 0x04002981 RID: 10625
		Silent
	}

	// Token: 0x020006D6 RID: 1750
	public enum StoreAs
	{
		// Token: 0x04002983 RID: 10627
		DoNotStore,
		// Token: 0x04002984 RID: 10628
		Bool,
		// Token: 0x04002985 RID: 10629
		Int,
		// Token: 0x04002986 RID: 10630
		Float,
		// Token: 0x04002987 RID: 10631
		String,
		// Token: 0x04002988 RID: 10632
		Manual
	}

	// Token: 0x020006D7 RID: 1751
	public enum EvalTokenType
	{
		// Token: 0x0400298A RID: 10634
		None,
		// Token: 0x0400298B RID: 10635
		Token,
		// Token: 0x0400298C RID: 10636
		TokenWithComponent,
		// Token: 0x0400298D RID: 10637
		Arithmetic,
		// Token: 0x0400298E RID: 10638
		Error
	}

	// Token: 0x020006D8 RID: 1752
	public enum CommandOrigin
	{
		// Token: 0x04002990 RID: 10640
		Internal,
		// Token: 0x04002991 RID: 10641
		UserVariable,
		// Token: 0x04002992 RID: 10642
		Lua
	}

	// Token: 0x020006D9 RID: 1753
	public class CommandStatus
	{
		// Token: 0x06003CB5 RID: 15541 RVA: 0x00179C47 File Offset: 0x00177E47
		public CommandStatus(SystemConsole.CommandEcho echo = SystemConsole.CommandEcho.Loud)
		{
			this.echo = echo;
			this.done = true;
			this.batched = false;
			this.readOnly = false;
			this.dirty = false;
			this.raw = false;
		}

		// Token: 0x06003CB6 RID: 15542 RVA: 0x00179C84 File Offset: 0x00177E84
		public void Done()
		{
			this.done = true;
		}

		// Token: 0x06003CB7 RID: 15543 RVA: 0x00179C8D File Offset: 0x00177E8D
		public void Open()
		{
			this.done = false;
		}

		// Token: 0x06003CB8 RID: 15544 RVA: 0x00179C96 File Offset: 0x00177E96
		public void Batch()
		{
			this.batched = true;
		}

		// Token: 0x06003CB9 RID: 15545 RVA: 0x00179C9F File Offset: 0x00177E9F
		public void Dirty()
		{
			this.dirty = true;
		}

		// Token: 0x06003CBA RID: 15546 RVA: 0x00179CA8 File Offset: 0x00177EA8
		public void Clean()
		{
			this.dirty = false;
		}

		// Token: 0x06003CBB RID: 15547 RVA: 0x00179CB1 File Offset: 0x00177EB1
		public void Raw()
		{
			this.raw = true;
		}

		// Token: 0x06003CBC RID: 15548 RVA: 0x00179CBA File Offset: 0x00177EBA
		public void Default()
		{
			this.raw = false;
		}

		// Token: 0x06003CBD RID: 15549 RVA: 0x00179CC3 File Offset: 0x00177EC3
		public void ReadOnly()
		{
			this.readOnly = true;
		}

		// Token: 0x06003CBE RID: 15550 RVA: 0x00179CCC File Offset: 0x00177ECC
		public void Return()
		{
			this.doReturn = true;
		}

		// Token: 0x06003CBF RID: 15551 RVA: 0x00179CD5 File Offset: 0x00177ED5
		public void Loud()
		{
			this.echo = SystemConsole.CommandEcho.Loud;
		}

		// Token: 0x06003CC0 RID: 15552 RVA: 0x00179CDE File Offset: 0x00177EDE
		public void Quiet()
		{
			this.echo = SystemConsole.CommandEcho.Quiet;
		}

		// Token: 0x06003CC1 RID: 15553 RVA: 0x00179CE7 File Offset: 0x00177EE7
		public void Silent()
		{
			this.echo = SystemConsole.CommandEcho.Silent;
		}

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x06003CC2 RID: 15554 RVA: 0x00179CF0 File Offset: 0x00177EF0
		public bool isDone
		{
			get
			{
				return this.done;
			}
		}

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x06003CC3 RID: 15555 RVA: 0x00179CF8 File Offset: 0x00177EF8
		public bool isReturn
		{
			get
			{
				return this.doReturn;
			}
		}

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06003CC4 RID: 15556 RVA: 0x00179D00 File Offset: 0x00177F00
		public bool isOpen
		{
			get
			{
				return !this.done;
			}
		}

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x06003CC5 RID: 15557 RVA: 0x00179D0B File Offset: 0x00177F0B
		public bool inBatch
		{
			get
			{
				return this.batched;
			}
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06003CC6 RID: 15558 RVA: 0x00179D13 File Offset: 0x00177F13
		public bool isDirty
		{
			get
			{
				return this.dirty;
			}
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06003CC7 RID: 15559 RVA: 0x00179D1B File Offset: 0x00177F1B
		public bool isLoud
		{
			get
			{
				return this.echo == SystemConsole.CommandEcho.Loud;
			}
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06003CC8 RID: 15560 RVA: 0x00179D26 File Offset: 0x00177F26
		public bool isQuiet
		{
			get
			{
				return this.echo == SystemConsole.CommandEcho.Quiet || this.echo == SystemConsole.CommandEcho.Silent;
			}
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06003CC9 RID: 15561 RVA: 0x00179D3C File Offset: 0x00177F3C
		public bool isSilent
		{
			get
			{
				return this.echo == SystemConsole.CommandEcho.Silent;
			}
		}

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06003CCA RID: 15562 RVA: 0x00179D47 File Offset: 0x00177F47
		public bool isReadOnly
		{
			get
			{
				return this.readOnly;
			}
		}

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06003CCB RID: 15563 RVA: 0x00179D4F File Offset: 0x00177F4F
		public bool isRaw
		{
			get
			{
				return this.raw;
			}
		}

		// Token: 0x04002993 RID: 10643
		private bool done;

		// Token: 0x04002994 RID: 10644
		private bool batched;

		// Token: 0x04002995 RID: 10645
		private bool readOnly;

		// Token: 0x04002996 RID: 10646
		private bool raw;

		// Token: 0x04002997 RID: 10647
		private bool doReturn;

		// Token: 0x04002998 RID: 10648
		public bool dirty;

		// Token: 0x04002999 RID: 10649
		public object value;

		// Token: 0x0400299A RID: 10650
		public SystemConsole.CommandEcho echo;

		// Token: 0x0400299B RID: 10651
		public string skippingToLabel = "";
	}

	// Token: 0x020006DA RID: 1754
	// (Invoke) Token: 0x06003CCD RID: 15565
	public delegate void ConsoleCommandFunction(SystemConsole.CommandStatus status, string parameters);

	// Token: 0x020006DB RID: 1755
	public class ConsoleCommand
	{
		// Token: 0x06003CD0 RID: 15568 RVA: 0x00179D58 File Offset: 0x00177F58
		public ConsoleCommand(string name, SystemConsole.CommandType type, SystemConsole.CommandPermission permission, string help, string documentation, SystemConsole.ConsoleCommandFunction function, string defaults = "", Type variableType = null, SystemConsole.StoreAs storeAs = SystemConsole.StoreAs.DoNotStore, SystemConsole.CommandOrigin origin = SystemConsole.CommandOrigin.Internal, Closure luaFunction = null)
		{
			this.permission = permission;
			this.type = type;
			this.variableType = variableType;
			this.help = help;
			this.documentation = documentation.Replace("$CMD$", name);
			this.code = function;
			this.defaults = defaults;
			this.storeAs = storeAs;
			this.origin = origin;
			this.luaFunction = luaFunction;
		}

		// Token: 0x0400299C RID: 10652
		public SystemConsole.CommandPermission permission;

		// Token: 0x0400299D RID: 10653
		public SystemConsole.CommandType type;

		// Token: 0x0400299E RID: 10654
		public Type variableType;

		// Token: 0x0400299F RID: 10655
		public string help;

		// Token: 0x040029A0 RID: 10656
		public string documentation;

		// Token: 0x040029A1 RID: 10657
		public string aliasOf = "";

		// Token: 0x040029A2 RID: 10658
		public string defaults = "";

		// Token: 0x040029A3 RID: 10659
		public SystemConsole.ConsoleCommandFunction code;

		// Token: 0x040029A4 RID: 10660
		public SystemConsole.CommandOrigin origin;

		// Token: 0x040029A5 RID: 10661
		public Closure luaFunction;

		// Token: 0x040029A6 RID: 10662
		public SystemConsole.StoreAs storeAs;
	}

	// Token: 0x020006DC RID: 1756
	public struct LogTag
	{
		// Token: 0x06003CD1 RID: 15569 RVA: 0x00179DDA File Offset: 0x00177FDA
		public LogTag(string name, Colour colour, string prefix, string postfix)
		{
			this.name = name;
			this.colour = colour;
			this.prefix = prefix;
			this.postfix = postfix;
		}

		// Token: 0x040029A7 RID: 10663
		public string name;

		// Token: 0x040029A8 RID: 10664
		public Colour colour;

		// Token: 0x040029A9 RID: 10665
		public string prefix;

		// Token: 0x040029AA RID: 10666
		public string postfix;
	}

	// Token: 0x020006DD RID: 1757
	public enum LogFormat
	{
		// Token: 0x040029AC RID: 10668
		Expansive,
		// Token: 0x040029AD RID: 10669
		Concise,
		// Token: 0x040029AE RID: 10670
		Truncated
	}

	// Token: 0x020006DE RID: 1758
	public enum TransformType
	{
		// Token: 0x040029B0 RID: 10672
		MoveRelative,
		// Token: 0x040029B1 RID: 10673
		MoveAbsolute,
		// Token: 0x040029B2 RID: 10674
		RotateRelative,
		// Token: 0x040029B3 RID: 10675
		RotateAbsolute
	}
}
