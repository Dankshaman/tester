using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using NewNet;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020001C5 RID: 453
public class NetworkUI : NetworkSingleton<NetworkUI>
{
	// Token: 0x170003DA RID: 986
	// (get) Token: 0x0600178E RID: 6030 RVA: 0x000A0B4C File Offset: 0x0009ED4C
	// (set) Token: 0x0600178F RID: 6031 RVA: 0x000A0B77 File Offset: 0x0009ED77
	public string VersionNumber
	{
		get
		{
			string result;
			if ((result = this._VersionNumber) == null)
			{
				result = (this._VersionNumber = this.Version.VersionNumber);
			}
			return result;
		}
		set
		{
			this._VersionNumber = value;
		}
	}

	// Token: 0x170003DB RID: 987
	// (get) Token: 0x06001790 RID: 6032 RVA: 0x000A0B80 File Offset: 0x0009ED80
	// (set) Token: 0x06001791 RID: 6033 RVA: 0x000A0BAB File Offset: 0x0009EDAB
	public string VersionHotfix
	{
		get
		{
			string result;
			if ((result = this._VersionHotfix) == null)
			{
				result = (this._VersionHotfix = this.Version.VersionHotFix);
			}
			return result;
		}
		set
		{
			this._VersionHotfix = value;
		}
	}

	// Token: 0x170003DC RID: 988
	// (get) Token: 0x06001792 RID: 6034 RVA: 0x000A0BB4 File Offset: 0x0009EDB4
	// (set) Token: 0x06001793 RID: 6035 RVA: 0x000A0BD0 File Offset: 0x0009EDD0
	public bool bCanFlipTable
	{
		get
		{
			return !(this.playerLabel == "Grey") && this._bCanFlipTable;
		}
		set
		{
			if (value != this._bCanFlipTable)
			{
				this._bCanFlipTable = value;
				EventManager.TriggerCanFlipTable(value);
			}
		}
	}

	// Token: 0x170003DD RID: 989
	// (get) Token: 0x06001794 RID: 6036 RVA: 0x000A0BE8 File Offset: 0x0009EDE8
	public static Vector3 DefaultGravity
	{
		get
		{
			if (VRHMD.isVR)
			{
				return new Vector3(0f, -100f, 0f);
			}
			return new Vector3(0f, -25f, 0f);
		}
	}

	// Token: 0x06001795 RID: 6037 RVA: 0x000A0C1C File Offset: 0x0009EE1C
	protected override void Awake()
	{
		base.Awake();
		this.SetCultureUS();
		this.CheckReset();
		this.Init();
		this.Resets();
		Network.version = this.VersionNumber;
		NetworkEvents.OnServerInitializing += this.ServerInitializing;
		NetworkEvents.OnServerInitialized += this.ServerInitialized;
		NetworkEvents.OnConnectingToServer += this.ConnectingToServer;
		NetworkEvents.OnConnectedToServer += this.ConnectedToServer;
		NetworkEvents.OnDisconnectedFromServer += this.DisconnectedFromServer;
		NetworkEvents.OnPlayerConnected += this.OnPlayerConnect;
		NetworkEvents.OnPlayerDisconnected += this.OnPlayerDisconnect;
		NetworkEvents.OnFailedToConnect += this.FailedToConnect;
		SceneManager.sceneLoaded += this.SceneManager_sceneLoaded;
		EventManager.OnLanguageChange += this.OnLanguageChange;
	}

	// Token: 0x06001796 RID: 6038 RVA: 0x000A0CFC File Offset: 0x0009EEFC
	private void OnDestroy()
	{
		NetworkEvents.OnServerInitializing -= this.ServerInitializing;
		NetworkEvents.OnServerInitialized -= this.ServerInitialized;
		NetworkEvents.OnConnectingToServer -= this.ConnectingToServer;
		NetworkEvents.OnConnectedToServer -= this.ConnectedToServer;
		NetworkEvents.OnDisconnectedFromServer -= this.DisconnectedFromServer;
		NetworkEvents.OnPlayerConnected -= this.OnPlayerConnect;
		NetworkEvents.OnPlayerDisconnected -= this.OnPlayerDisconnect;
		NetworkEvents.OnFailedToConnect -= this.FailedToConnect;
		SceneManager.sceneLoaded -= this.SceneManager_sceneLoaded;
		EventManager.OnLanguageChange -= this.OnLanguageChange;
	}

	// Token: 0x06001797 RID: 6039 RVA: 0x000A0DB3 File Offset: 0x0009EFB3
	private void SetCultureUS()
	{
		CultureInfo.DefaultThreadCurrentCulture = (CultureInfo.CurrentCulture = new CultureInfo("en-US"));
		LibString.CreateForeignLookup();
	}

	// Token: 0x06001798 RID: 6040 RVA: 0x000A0DD0 File Offset: 0x0009EFD0
	private void CheckReset()
	{
		if (!PlayerPrefs.HasKey("Reset"))
		{
			PlayerPrefs.SetInt("Reset", 25);
			UIDialog.Show("Would you like to play the quick Tutorial?", "Yes", "No", new Action(this.TutorialConfirm), new Action(this.TutorialCancel));
		}
		else if (PlayerPrefs.GetInt("Reset") != 25)
		{
			if (24 > PlayerPrefs.GetInt("Reset"))
			{
				Chat.LogWarning("RAW Mod Cache has been deleted due to update.", true);
				CustomCache.DeleteRAWCache();
			}
			Chat.LogWarning("Key bindings have been reset to default due to update.", true);
			cInput.ResetInputs();
			PlayerPrefs.SetInt("Reset", 25);
		}
		Wait.Frames(delegate
		{
			string @string = PlayerPrefs.GetString("VersionNumber");
			if (@string != this.VersionNumber)
			{
				EventManager.TriggerVersionNumberChange(@string, this.VersionNumber);
				PlayerPrefs.SetString("VersionNumber", this.VersionNumber);
			}
		}, 3);
	}

	// Token: 0x06001799 RID: 6041 RVA: 0x000A0E80 File Offset: 0x0009F080
	private void Init()
	{
		Utilities.SetCursor(this.WhiteCursorTexture, NetworkUI.HardwareCursorOffest, CursorMode.Auto);
		Physics.gravity = NetworkUI.DefaultGravity;
		for (int i = 0; i < this.GUIPasswordInputs.Length; i++)
		{
			this.GUIPasswordInputs[i].characterLimit = 40;
		}
		this.escapeMenus.Add(this.GUIControlScheme);
		this.escapeMenus.Add(this.GUIControlSchemeController);
		this.escapeMenus.Add(this.GUIControlSchemeControllerSteam);
		this.escapeMenus.Add(this.GUIBackgrounds);
		this.escapeMenus.Add(this.GUIGames);
		this.escapeMenus.Add(this.GUIObjects);
		this.escapeMenus.Add(this.GUITables);
		this.escapeMenus.Add(this.GUIWorkshopUpload);
		this.escapeMenus.Add(this.GUIConfiguration);
		this.escapeMenus.Add(this.GUIBackgrounds);
		this.escapeMenus.Add(this.GUIComponents);
		this.escapeMenus.Add(this.GUITurns);
		this.escapeMenus.Add(this.GUIServerOptions);
		this.escapeMenus.Add(this.GUIPermissions);
		this.escapeMenus.Add(this.GUIInfoOptions);
		this.escapeMenus.Add(this.GUIPhysicsOptions);
		this.escapeMenus.Add(this.GUIGrid);
		this.escapeMenus.Add(this.GUILighting);
		this.escapeMenus.Add(this.GUIRules);
		this.escapeMenus.Add(this.GUINotepad);
		this.escapeMenus.Add(Singleton<UICustomMusicPlayer>.Instance.gameObject);
		this.escapeMenus.Add(Singleton<UIAudioImport>.Instance.gameObject);
		this.escapeMenus.Add(this.GUIHands);
		this.escapeMenus.Add(this.GUITeams);
		this.escapeMenus.Add(this.GUIInventory.gameObject);
		this.escapeMenus.Add(this.GUISavedObjects);
		this.escapeMenus.Add(this.GUICloud);
		this.escapeMenus.Add(this.GUILuaNotepad.gameObject);
		this.escapeMenus.Add(Singleton<UIPDFPopout>.Instance.gameObject);
		this.escapeMenus.Add(Singleton<UIFinder>.Instance.finder);
		this.escapeMenus.Add(this.GUIServerBrowser);
	}

	// Token: 0x0600179A RID: 6042 RVA: 0x000A10F4 File Offset: 0x0009F2F4
	private void Resets()
	{
		TabletScript.WhiteListId.Clear();
		TabletScript.BlackListId.Clear();
		SerializationScript.LastLoadedJsonFilePath = "";
	}

	// Token: 0x0600179B RID: 6043 RVA: 0x000A1114 File Offset: 0x0009F314
	private void PrintAllResourcesFolder()
	{
		string text = Application.dataPath + "//Resources";
		string[] files = Directory.GetFiles(text, "*.prefab");
		string text2 = "";
		for (int i = 0; i < files.Length; i++)
		{
			text2 = text2 + files[i].Substring(text.Length + 1, files[i].Length - text.Length - 8) + "\n";
		}
		Debug.Log(text2);
	}

	// Token: 0x0600179C RID: 6044 RVA: 0x000A1184 File Offset: 0x0009F384
	private void Start()
	{
		this.SetPlayerName(SteamManager.SteamName);
	}

	// Token: 0x0600179D RID: 6045 RVA: 0x000A1191 File Offset: 0x0009F391
	public void ResetAllSavedData()
	{
		Chat.Log("All settings have been reset. Might have to restart for all changes to take effect.", ChatMessageType.All);
		cInput.ResetInputs();
		PlayerPrefs.DeleteAll();
	}

	// Token: 0x0600179E RID: 6046 RVA: 0x000A11A8 File Offset: 0x0009F3A8
	public GameObject NetworkSpawn(GameObject GO, Vector3 pos = default(Vector3))
	{
		Vector3 vector = pos;
		if (vector == Vector3.zero)
		{
			vector = GO.transform.position;
		}
		return Network.Instantiate(GO, vector, GO.transform.rotation, default(NetworkPlayer));
	}

	// Token: 0x0600179F RID: 6047 RVA: 0x000A11EC File Offset: 0x0009F3EC
	public void EscapeMenu(NetworkUI.EscapeMenuActivation activation = NetworkUI.EscapeMenuActivation.AUTO)
	{
		if (Input.GetKeyDown(KeyCode.Escape) && PlayerScript.PointerScript && PlayerScript.PointerScript.AcceptingEscape)
		{
			PlayerScript.PointerScript.HandleEscape();
			return;
		}
		if (Singleton<UIChatInput>.Instance.InputText.isSelected)
		{
			Singleton<UIChatInput>.Instance.InputText.isSelected = false;
			return;
		}
		bool flag = false;
		foreach (GameObject gameObject in this.escapeMenus)
		{
			if (gameObject.activeSelf)
			{
				flag = true;
			}
			gameObject.SetActive(false);
		}
		if ((activation == NetworkUI.EscapeMenuActivation.AUTO && !flag) || activation == NetworkUI.EscapeMenuActivation.FORCED)
		{
			bool active = !this.GUIMenu.activeSelf;
			this.GUIMenu.SetActive(active);
		}
	}

	// Token: 0x060017A0 RID: 6048 RVA: 0x000A12BC File Offset: 0x0009F4BC
	private void Update()
	{
		if (TTSInput.GetKeyDown(KeyCode.Escape))
		{
			NetworkUI.bHideGUI = false;
		}
		if (this.notepadstring != this.prevnotepadstring && this.notepadtimeholder + 0.25f < Time.time)
		{
			this.notepadtimeholder = Time.time;
			if (!PermissionsOptions.CheckAllow(PermissionsOptions.options.Notes, -1))
			{
				this.notepadstring = this.prevnotepadstring;
			}
			else
			{
				base.networkView.RPC<string>(RPCTarget.Others, new Action<string>(this.NotepadRPC), this.notepadstring);
				this.prevnotepadstring = this.notepadstring;
			}
		}
		if (Network.peerType != NetworkPeerMode.Disconnected)
		{
			if (zInput.GetButtonDown("Hide GUI", ControlType.All) && !UICamera.SelectIsInput() && NetworkSingleton<GameOptions>.Instance.GameName != this.GameTutorial && !NetworkSingleton<PlayerManager>.Instance.IsBlinded() && !zInput.GetButton("Shift", ControlType.All))
			{
				NetworkUI.bHideGUI = !NetworkUI.bHideGUI;
			}
			if (zInput.GetButtonDown("Hide GUI", ControlType.All) && zInput.GetButton("Shift", ControlType.All))
			{
				ScreenshotScript.SaveScreenshot();
			}
			if (zInput.GetButtonDown("Blindfold", ControlType.All) && !UICamera.SelectIsInput() && !NetworkUI.bHideGUI)
			{
				NetworkSingleton<PlayerManager>.Instance.ToggleBlindfold();
			}
			if (zInput.GetButtonDown("Help", ControlType.Keyboard) && !UICamera.SelectIsInput())
			{
				this.GUIControlScheme.SetActive(!this.GUIControlScheme.activeSelf);
			}
			if (zInput.GetButtonDown("Help", ControlType.Controller) && !UICamera.SelectIsInput() && !TTSInput.GetKey(KeyCode.RightControl))
			{
				this.GUIControlSchemeController.SetActive(!this.GUIControlSchemeController.activeSelf);
			}
			if (zInput.GetButtonDown("Help", ControlType.Controller) && !UICamera.SelectIsInput() && TTSInput.GetKey(KeyCode.RightControl))
			{
				this.GUIControlSchemeControllerSteam.SetActive(!this.GUIControlSchemeControllerSteam.activeSelf);
			}
			if ((TTSInput.GetKeyDown(KeyCode.Escape) || zInput.GetButtonDown("Main Menu", ControlType.Controller)) && !cInput.scanning)
			{
				this.EscapeMenu(NetworkUI.EscapeMenuActivation.AUTO);
			}
			if (this.GUIColor.activeSelf != this.bNeedToPickColour)
			{
				this.GUIColor.SetActive(this.bNeedToPickColour);
			}
			if (this.GUIUIRoot.activeSelf != !NetworkUI.bHideGUI)
			{
				this.GUIUIRoot.SetActive(!NetworkUI.bHideGUI);
			}
		}
	}

	// Token: 0x060017A1 RID: 6049 RVA: 0x000A1501 File Offset: 0x0009F701
	public void SetCurrentGame(string SGM)
	{
		if (string.IsNullOrEmpty(SGM))
		{
			SGM = "None";
		}
		base.networkView.RPC<string>(RPCTarget.All, new Action<string>(this.RPCSetCurrentGame), SGM);
	}

	// Token: 0x060017A2 RID: 6050 RVA: 0x000A152B File Offset: 0x0009F72B
	[Remote(Permission.Server)]
	private void RPCSetHostOwnedDLCs(byte[] HostOwnedBytes)
	{
		DLCManager.SetHostOwnedDLCs(Json.Load<List<string>>(HostOwnedBytes, true));
	}

	// Token: 0x060017A3 RID: 6051 RVA: 0x000A153C File Offset: 0x0009F73C
	private void CheckSetClassicRules()
	{
		string tabFromRulesLegacy;
		if (GameRules.Rules.TryGetValue(NetworkSingleton<GameOptions>.Instance.GameName, out tabFromRulesLegacy))
		{
			this.GUIRules.GetComponent<UINotebook>().SetTabFromRulesLegacy(tabFromRulesLegacy);
		}
	}

	// Token: 0x060017A4 RID: 6052 RVA: 0x000025B8 File Offset: 0x000007B8
	private void ServerInitializing()
	{
	}

	// Token: 0x060017A5 RID: 6053 RVA: 0x000A1572 File Offset: 0x0009F772
	private void ConnectingToServer()
	{
		this.GUIConnecting.SetActive(true);
	}

	// Token: 0x060017A6 RID: 6054 RVA: 0x000A1580 File Offset: 0x0009F780
	private void ServerInitialized()
	{
		if (Network.maxConnections > 0)
		{
			EventManager.TriggerUnityAnalytic("Multiplayer_Started", null, 0);
			NetworkSingleton<Chat>.Instance.MultiplayerGameStarted();
			NetworkSingleton<ServerOptions>.Instance.LookingForPlayers = true;
		}
		else
		{
			if (!this.IsInTutorial)
			{
				if (this.bHotseat)
				{
					EventManager.TriggerUnityAnalytic("Hotseat_Started", null, 0);
				}
				else
				{
					EventManager.TriggerUnityAnalytic("Singleplayer_Started", null, 0);
				}
			}
			NetworkSingleton<Chat>.Instance.SinglePlayerGameStarted();
			NetworkSingleton<ServerOptions>.Instance.LookingForPlayers = false;
		}
		if (NetworkSingleton<GameMode>.Instance.TestObject)
		{
			Network.Instantiate(NetworkSingleton<GameMode>.Instance.TestObject, this.SpawnPos, NetworkSingleton<GameMode>.Instance.TestObject.transform.rotation, default(NetworkPlayer));
		}
		this.GUIDisconnected.SetActive(false);
		this.GUIConnected.SetActive(true);
		this.GUIGames.SetActive(true);
		if (this.playerName == "")
		{
			this.playerName = "?";
		}
		if (!this.IsInTutorial)
		{
			int num = UnityEngine.Random.Range(1, 9);
			GameObject gameObject = null;
			switch (num)
			{
			case 1:
				gameObject = NetworkSingleton<GameMode>.Instance.Sky_Forest;
				break;
			case 2:
				gameObject = NetworkSingleton<GameMode>.Instance.Sky_Field;
				break;
			case 3:
				gameObject = NetworkSingleton<GameMode>.Instance.Sky_Tunnel;
				break;
			case 4:
				gameObject = NetworkSingleton<GameMode>.Instance.Sky_Cathedral;
				break;
			case 5:
				gameObject = NetworkSingleton<GameMode>.Instance.Sky_Downtown;
				break;
			case 6:
				gameObject = NetworkSingleton<GameMode>.Instance.Sky_Regal;
				break;
			case 7:
				gameObject = NetworkSingleton<GameMode>.Instance.Sky_Sunset;
				break;
			}
			if (gameObject != null)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.DestroySky();
				Network.Instantiate(gameObject, Vector3.zero, gameObject.transform.rotation, default(NetworkPlayer));
			}
		}
		GameObject gameObject2 = null;
		switch (UnityEngine.Random.Range(1, 6))
		{
		case 1:
			gameObject2 = NetworkSingleton<GameMode>.Instance.SquareTable;
			break;
		case 2:
			gameObject2 = NetworkSingleton<GameMode>.Instance.OctagonTable;
			break;
		case 3:
			gameObject2 = NetworkSingleton<GameMode>.Instance.HexagonTable;
			break;
		case 4:
			gameObject2 = NetworkSingleton<GameMode>.Instance.RPGTable;
			break;
		case 5:
			gameObject2 = NetworkSingleton<GameMode>.Instance.CircleTable;
			break;
		}
		if (this.IsInTutorial)
		{
			gameObject2 = NetworkSingleton<GameMode>.Instance.SquareTable;
			this.GUIGames.SetActive(false);
		}
		Network.Instantiate(gameObject2, gameObject2.transform.position, gameObject2.transform.rotation, default(NetworkPlayer));
		this.bCanFlipTable = true;
		Chat.Log("Press <?> or <Back> for keybinds. Press <Enter> then /help for chat commands.", Colour.Purple, ChatMessageType.Game, false);
		if (!this.bHotseat)
		{
			NetworkSingleton<PlayerManager>.Instance.AddPlayer(new PlayerManager.PlayerData(this.playerName, Network.player, SteamManager.StringSteamID, VRHMD.isVR, "Grey", false, Team.None, false));
			this.ClientRequestColor("White");
		}
		LuaGlobalScriptManager.Instance.Init();
		EventManager.TriggerUnityAnalytic("Control_Type", "Type", zInput.CurrentControlType.ToString(), 0);
	}

	// Token: 0x060017A7 RID: 6055 RVA: 0x000A1880 File Offset: 0x0009FA80
	private void CanFlipTable()
	{
		this.bCanFlipTable = true;
	}

	// Token: 0x170003DE RID: 990
	// (get) Token: 0x060017A8 RID: 6056 RVA: 0x000A1889 File Offset: 0x0009FA89
	// (set) Token: 0x060017A9 RID: 6057 RVA: 0x000A1891 File Offset: 0x0009FA91
	public bool IsInTutorial { get; private set; }

	// Token: 0x060017AA RID: 6058 RVA: 0x000A189C File Offset: 0x0009FA9C
	private void ConnectedToServer()
	{
		Debug.Log("ConnectedToServer");
		this.GUIDisconnected.SetActive(false);
		this.GUIConnected.SetActive(true);
		if (this.playerName == "")
		{
			this.playerName = "?";
		}
		EventManager.TriggerUnityAnalytic("Connected_To_Server", null, 0);
		Wait.Time(new Action(this.CanFlipTable), 120f, 1);
		Debug.Log("OnConnectedToServer");
		Chat.Log("Press <?> or <Back> for help. Press <Enter> then /help for console commands.", Colour.Purple, ChatMessageType.Game, false);
		this.playerLabel = "Grey";
		Utilities.SetCursor(this.StringColorToCursorTexture(this.playerLabel), NetworkUI.HardwareCursorOffest, CursorMode.Auto);
		base.networkView.RPC<string, string, string, bool>(RPCTarget.Server, new Action<string, string, string, bool>(this.Register), this.playerName, this.VersionNumber, SystemInfo.deviceUniqueIdentifier, VRHMD.isVR);
		NetworkSingleton<Chat>.Instance.MultiplayerGameStarted();
		UICamera.selectedObject = null;
		if (base.networkView.isMine)
		{
			this.GUIEndHandRevealButton.SetActive(false);
		}
		EventManager.TriggerChangePlayerColor(Colour.Grey, NetworkID.ID);
		EventManager.TriggerUnityAnalytic("Control_Type", "Type", zInput.CurrentControlType.ToString(), 0);
	}

	// Token: 0x060017AB RID: 6059 RVA: 0x000A19D8 File Offset: 0x0009FBD8
	private void OnPlayerConnect(NetworkPlayer player)
	{
		Debug.Log(string.Concat(new object[]
		{
			"OnPlayerConnect ",
			player.id,
			" : ",
			player.steamID
		}));
		if (Network.isServer)
		{
			Wait.Condition(null, delegate
			{
				PlayerState playerState2;
				return NetworkSingleton<PlayerManager>.Instance.PlayersDictionary.TryGetValue((int)player.id, out playerState2);
			}, 21f, delegate
			{
				if (Network.connections.Contains(player))
				{
					this.KickPlayer(player, "Didn't register in time.");
				}
			});
			List<PlayerState> playersList = NetworkSingleton<PlayerManager>.Instance.PlayersList;
			for (int i = 0; i < playersList.Count; i++)
			{
				PlayerState playerState = playersList[i];
				NetworkSingleton<PlayerManager>.Instance.AddPlayer(player, new PlayerManager.PlayerData(playerState.name, playerState.networkPlayer, playerState.steamId, playerState.isVR, playerState.stringColor, playerState.promoted, playerState.team, playerState.blind));
			}
			if (this.notepadstring != "")
			{
				base.networkView.RPC<string>(player, new Action<string>(this.NotepadRPC), this.notepadstring);
			}
			List<string> ownedDLCs = DLCManager.GetOwnedDLCs();
			if (ownedDLCs.Count > 0)
			{
				byte[] bson = Json.GetBson(ownedDLCs);
				base.networkView.RPC<byte[]>(player, new Action<byte[]>(this.RPCSetHostOwnedDLCs), bson);
			}
			this.GUIRules.GetComponent<UINotebook>().UpdateNotepadForPlayer(player);
		}
	}

	// Token: 0x060017AC RID: 6060 RVA: 0x000A1B60 File Offset: 0x0009FD60
	private void InitializePlayer(Colour colour, GameObject Pointer_Prefab)
	{
		this.playerLabel = PlayerScript.PointerScript.PointerColorLabel;
		this.bNeedToPickColour = false;
		if (!this.bHotseat)
		{
			Chat.SendChat(this.playerName + " is color " + this.playerLabel + ".", this.playerLabel);
			return;
		}
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromColour(colour);
		if (playerState != null)
		{
			this.CurrentHotseat = playerState.id;
			return;
		}
		this.CurrentHotseat++;
		if (this.CurrentHotseat > this.NumHotseat)
		{
			this.CurrentHotseat = 1;
			if (!this.askedForGame)
			{
				this.askedForGame = true;
				this.GUIGames.SetActive(true);
			}
			this.GUIStartConfirmHotseat(this.CurrentHotseat, false);
			return;
		}
		if (Turns.HotseatAskForNames)
		{
			this.GUIStartConfirmHotseatAndGetName(this.CurrentHotseat);
			return;
		}
		this.GUIStartConfirmHotseat(this.CurrentHotseat, true);
	}

	// Token: 0x060017AD RID: 6061 RVA: 0x000A1C3F File Offset: 0x0009FE3F
	[Remote(Permission.Admin)]
	private void RPCSetCurrentGame(string GameName)
	{
		if (Network.gameName != GameName)
		{
			EventManager.TriggerUnityAnalytic("Game_Loaded", "Name", GameName, 0);
		}
		Network.gameName = GameName;
		Chat.Log(GameName + " loading...", ChatMessageType.Game);
		this.CheckSetClassicRules();
	}

	// Token: 0x060017AE RID: 6062 RVA: 0x000A1C7C File Offset: 0x0009FE7C
	[Remote("Permissions/Notes")]
	private void NotepadRPC(string npstring)
	{
		this.notepadstring = npstring;
		this.prevnotepadstring = npstring;
		this.GUINotepadInput.value = npstring;
	}

	// Token: 0x060017AF RID: 6063 RVA: 0x000A1C98 File Offset: 0x0009FE98
	public void ClientRequestColor(string color)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<string>(RPCTarget.Server, new Action<string>(this.RequestColor), color);
			return;
		}
		this.CheckColor(color, NetworkID.ID);
	}

	// Token: 0x060017B0 RID: 6064 RVA: 0x000A1CC8 File Offset: 0x0009FEC8
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void RequestColor(string color)
	{
		if (color == "Grey" || PermissionsOptions.CheckAllowSender(PermissionsOptions.options.ChangeColor))
		{
			Debug.Log("RequestColor: " + color);
			int id = NetworkID.IDFromNetworkPlayer(Network.sender);
			if (color == "Black" && !NetworkSingleton<PlayerManager>.Instance.IsAdmin(id))
			{
				return;
			}
			this.CheckColor(color, id);
		}
	}

	// Token: 0x060017B1 RID: 6065 RVA: 0x000A1D34 File Offset: 0x0009FF34
	[Remote(Permission.Admin)]
	public void CheckColor(string color, int id)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<string, int>(RPCTarget.Server, new Action<string, int>(this.CheckColor), color, id);
			return;
		}
		if (!NetworkSingleton<PlayerManager>.Instance.ColourInUse(Colour.ColourFromLabel(color)))
		{
			NetworkPlayer networkPlayer = Network.IdToNetworkPlayer(id);
			if (this.bHotseat)
			{
				networkPlayer = NetworkPlayer.GetServerPlayer();
			}
			Pointer pointer = NetworkSingleton<ManagerPhysicsObject>.Instance.PointerFromID(id);
			if (pointer)
			{
				Network.Destroy(pointer.gameObject);
			}
			if (color != "Grey")
			{
				Network.Instantiate(this.StringColorToPrefab(color), Vector3.zero, Vector3.zero, networkPlayer);
			}
			base.networkView.RPC<string>(networkPlayer, new Action<string>(this.SetColorTo), color);
			NetworkSingleton<PlayerManager>.Instance.ChangeColor(id, color);
		}
	}

	// Token: 0x060017B2 RID: 6066 RVA: 0x000A1DF4 File Offset: 0x0009FFF4
	[Remote(Permission.Server)]
	private void SetColorTo(string color)
	{
		Debug.Log("SetColorTo: " + color);
		if (color == "Grey" && this.playerLabel == "Grey")
		{
			this.bNeedToPickColour = false;
		}
		Utilities.SetCursor(this.StringColorToCursorTexture(color), NetworkUI.HardwareCursorOffest, CursorMode.Auto);
		this.playerLabel = color;
		this.playerColour = Colour.ColourFromLabel(color);
		if (this.playerLabel != "Grey")
		{
			this.InitializePlayer(this.playerColour, this.StringColorToPrefab(color));
		}
	}

	// Token: 0x060017B3 RID: 6067 RVA: 0x000A1E84 File Offset: 0x000A0084
	[Remote(Permission.Client, SendType.ReliableBuffered, null, SerializationMethod.Default)]
	private void Register(string name, string versionnum, string deviceIdentifier, bool isVR)
	{
		NetworkPlayer sender = Network.sender;
		Debug.Log(string.Concat(new object[]
		{
			"Register steamID: ",
			sender.steamID,
			" deviceID: ",
			deviceIdentifier
		}));
		if (NetworkSingleton<PlayerManager>.Instance.ContainsSteamId(sender.steamID))
		{
			return;
		}
		bool flag = false;
		string text = NGUIText.StripSymbols(name);
		if (text != name)
		{
			name = text;
			flag = true;
		}
		string text2 = sender.steamID.ToString();
		if (Singleton<BlockList>.Instance.Contains(text2))
		{
			Chat.SendChat(name + " is blocked.", Colour.UnityRed);
			this.KickPlayer(sender, "Blocked by this host.");
			return;
		}
		if (GlobalBanList.IsBanned(name, text2, deviceIdentifier))
		{
			Debug.Log("Banned player!");
			this.KickPlayer(sender, "Steam authentication failed.");
			return;
		}
		while (NetworkSingleton<PlayerManager>.Instance.NameInUse(name))
		{
			name += "*";
			flag = true;
		}
		Chat.SendChat(name + " connected.", Colour.Grey);
		NetworkSingleton<PlayerManager>.Instance.AddPlayer(new PlayerManager.PlayerData(name, sender, text2, isVR, "Grey", false, Team.None, false));
		if (flag)
		{
			base.networkView.RPC<string>(sender, new Action<string>(this.UpdateName), name);
		}
		NetworkPlayer networkPlayer = sender;
		foreach (NetworkPhysicsObject networkPhysicsObject in NetworkSingleton<ManagerPhysicsObject>.Instance.GrabbableNPOs)
		{
			if (networkPhysicsObject.customObject)
			{
				networkPhysicsObject.customObject.CallCustomRPC(networkPlayer);
			}
		}
		if (CustomSky.ActiveCustomSky)
		{
			CustomSky.ActiveCustomSky.CallCustomRPC(networkPlayer);
		}
		NetworkSingleton<CardManagerScript>.Instance.SyncCustomDecksToPlayer(networkPlayer);
		if (NetworkSingleton<ManagerPhysicsObject>.Instance.Table && NetworkSingleton<ManagerPhysicsObject>.Instance.Table.GetComponent<CustomImage>())
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.Table.GetComponent<CustomImage>().CallCustomRPC(networkPlayer);
		}
		NetworkSingleton<ManagerPhysicsObject>.Instance.networkView.RPC(networkPlayer, new Action(NetworkSingleton<ManagerPhysicsObject>.Instance.RPCCheckLoadingSaveComplete));
	}

	// Token: 0x060017B4 RID: 6068 RVA: 0x000A20B8 File Offset: 0x000A02B8
	public void KickPlayer(NetworkPlayer player, string message)
	{
		base.StartCoroutine(this.DelayKick(player, message));
	}

	// Token: 0x060017B5 RID: 6069 RVA: 0x000A20C9 File Offset: 0x000A02C9
	private IEnumerator DelayKick(NetworkPlayer player, string message)
	{
		Debug.Log(string.Concat(new object[]
		{
			"Kick player: ",
			player.steamID,
			" : ",
			message
		}));
		base.networkView.RPC<string>(player, new Action<string>(this.SetErrorMessage), message);
		yield return new WaitForSeconds(0.5f);
		Network.CloseConnectionTo(player);
		yield break;
	}

	// Token: 0x060017B6 RID: 6070 RVA: 0x000A20E6 File Offset: 0x000A02E6
	[Remote(Permission.Server)]
	private void SetErrorMessage(string msg)
	{
		this.ErrorMessage = msg;
	}

	// Token: 0x060017B7 RID: 6071 RVA: 0x000A20EF File Offset: 0x000A02EF
	[Remote(Permission.Server)]
	private void UpdateName(string name)
	{
		Chat.Log("Name already in use, new name is " + name + ".", Colour.White, ChatMessageType.Game, false);
		this.playerName = name;
	}

	// Token: 0x060017B8 RID: 6072 RVA: 0x000A2114 File Offset: 0x000A0314
	private void OnPlayerDisconnect(NetworkPlayer player, DisconnectInfo info)
	{
		if (Network.isServer)
		{
			Debug.Log("OnPlayerDisconnect " + player);
			try
			{
				int num = NetworkID.IDFromNetworkPlayer(player);
				if (NetworkSingleton<PlayerManager>.Instance.PlayersDictionary.ContainsKey(num))
				{
					PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(num);
					Debug.Log(playerState.name + " has disconnected.");
					Chat.SendChat(playerState.name + " has disconnected.", playerState.color);
					NetworkSingleton<PlayerManager>.Instance.RemovePlayer(num);
				}
				else
				{
					Debug.Log("Player is not in player manager. " + num);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("OnPlayerDisconnect error: " + ex.Message);
			}
		}
	}

	// Token: 0x060017B9 RID: 6073 RVA: 0x000A21E4 File Offset: 0x000A03E4
	private void DisconnectedFromServer(DisconnectInfo info)
	{
		if ((Network.isServer && Network.maxConnections > 0) || Network.isClient)
		{
			EventManager.TriggerUnityAnalytic("Multiplayer_Ended", null, 0);
		}
		else if (!this.IsInTutorial)
		{
			if (this.bHotseat)
			{
				EventManager.TriggerUnityAnalytic("Hotseat_End", null, 0);
			}
			else
			{
				EventManager.TriggerUnityAnalytic("Singleplayer_End", null, 0);
			}
		}
		Chat.Log("On Disconnected From Server: " + info, ChatMessageType.Game);
		Debug.Log("On Disconnected From Server: " + info);
		if (string.IsNullOrEmpty(this.ErrorMessage) && info != DisconnectInfo.Successful)
		{
			this.ErrorMessage = "Disconnect from server: " + info;
		}
		NetworkUI.bHideGUI = false;
		this.GUIUIRoot.SetActive(true);
		if (string.IsNullOrEmpty(this.ErrorMessage))
		{
			this.GUIReloadGame();
			return;
		}
		if (PlayerScript.Pointer)
		{
			UnityEngine.Object.Destroy(PlayerScript.Pointer);
		}
		this.GUIConnected.SetActive(false);
		this.GUIDisconnected.SetActive(false);
		UIDialog.Show(this.ErrorMessage, "Ok", new Action(this.GUIReloadGame));
	}

	// Token: 0x060017BA RID: 6074 RVA: 0x000A2300 File Offset: 0x000A0500
	private void FailedToConnect(ConnectFailedInfo info)
	{
		this.ErrorMessage = "Failed to connect: " + info.ToString();
		this.GUIConnecting.SetActive(false);
	}

	// Token: 0x060017BB RID: 6075 RVA: 0x000A232B File Offset: 0x000A052B
	private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		NetworkSingleton<ManagerPhysicsObject>.Instance.GarbageCollection();
	}

	// Token: 0x060017BC RID: 6076 RVA: 0x000A2338 File Offset: 0x000A0538
	public int SkyToID(string SkyName)
	{
		if (SkyName == "Sky_Museum(Clone)")
		{
			return 10;
		}
		if (SkyName == "Sky_Forest(Clone)")
		{
			return 11;
		}
		if (SkyName == "Sky_Field(Clone)")
		{
			return 12;
		}
		if (SkyName == "Sky_Tunnel(Clone)")
		{
			return 13;
		}
		return 0;
	}

	// Token: 0x060017BD RID: 6077 RVA: 0x000A2388 File Offset: 0x000A0588
	public GameObject SkyFromID(int id)
	{
		if (id == 10)
		{
			return NetworkSingleton<GameMode>.Instance.Sky_Museum;
		}
		if (id == 11)
		{
			return NetworkSingleton<GameMode>.Instance.Sky_Forest;
		}
		if (id == 12)
		{
			return NetworkSingleton<GameMode>.Instance.Sky_Field;
		}
		if (id == 13)
		{
			return NetworkSingleton<GameMode>.Instance.Sky_Tunnel;
		}
		return null;
	}

	// Token: 0x060017BE RID: 6078 RVA: 0x000A23D8 File Offset: 0x000A05D8
	private GameObject StringColorToPrefab(string color)
	{
		if (color == "White")
		{
			return this.WhitePrefab;
		}
		if (color == "Brown")
		{
			return this.BrownPrefab;
		}
		if (color == "Red")
		{
			return this.RedPrefab;
		}
		if (color == "Orange")
		{
			return this.OrangePrefab;
		}
		if (color == "Yellow")
		{
			return this.YellowPrefab;
		}
		if (color == "Green")
		{
			return this.GreenPrefab;
		}
		if (color == "Teal")
		{
			return this.TealPrefab;
		}
		if (color == "Blue")
		{
			return this.BluePrefab;
		}
		if (color == "Purple")
		{
			return this.PurplePrefab;
		}
		if (color == "Pink")
		{
			return this.PinkPrefab;
		}
		if (color == "Black")
		{
			return this.BlackPrefab;
		}
		Debug.LogError("Color to prefab broke.");
		return this.WhitePrefab;
	}

	// Token: 0x060017BF RID: 6079 RVA: 0x000A24D4 File Offset: 0x000A06D4
	private Texture2D StringColorToCursorTexture(string color)
	{
		if (color == "White")
		{
			return this.WhiteCursorTexture;
		}
		if (color == "Brown")
		{
			return this.BrownCursorTexture;
		}
		if (color == "Red")
		{
			return this.RedCursorTexture;
		}
		if (color == "Orange")
		{
			return this.OrangeCursorTexture;
		}
		if (color == "Yellow")
		{
			return this.YellowCursorTexture;
		}
		if (color == "Green")
		{
			return this.GreenCursorTexture;
		}
		if (color == "Teal")
		{
			return this.TealCursorTexture;
		}
		if (color == "Blue")
		{
			return this.BlueCursorTexture;
		}
		if (color == "Purple")
		{
			return this.PurpleCursorTexture;
		}
		if (color == "Pink")
		{
			return this.PinkCursorTexture;
		}
		if (color == "Black")
		{
			return this.BlackCursorTexture;
		}
		if (color == "Grey")
		{
			return this.GreyCursorTexture;
		}
		Debug.LogError("Color to CursorTexture broke.");
		return this.WhiteCursorTexture;
	}

	// Token: 0x060017C0 RID: 6080 RVA: 0x000A25E1 File Offset: 0x000A07E1
	public string GetPlayerName()
	{
		return this.playerName;
	}

	// Token: 0x060017C1 RID: 6081 RVA: 0x000A25EC File Offset: 0x000A07EC
	public string GetPlayerNickIRC()
	{
		string @string = PlayerPrefs.GetString("NickIRC");
		if (string.IsNullOrEmpty(@string))
		{
			return this.GetPlayerName();
		}
		return @string;
	}

	// Token: 0x060017C2 RID: 6082 RVA: 0x000A2614 File Offset: 0x000A0814
	public void SetPlayerName(string name)
	{
		if (name.Length < 33)
		{
			this.playerName = name;
			return;
		}
		this.playerName = name.Substring(0, 32);
	}

	// Token: 0x060017C3 RID: 6083 RVA: 0x000A2637 File Offset: 0x000A0837
	public void SetSpecificPlayerName(string newName)
	{
		if (this.playerIDToSet == -1)
		{
			return;
		}
		NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(this.playerIDToSet).name = newName;
		this.playerIDToSet = -1;
	}

	// Token: 0x060017C4 RID: 6084 RVA: 0x000A2660 File Offset: 0x000A0860
	public void GUIMultiplayerStart()
	{
		if (SteamManager.bSteam)
		{
			this.SetPlayerName(SteamManager.SteamName);
			this.GUIMainMenu.SetActive(false);
			this.GUIMultiplayer.SetActive(true);
			return;
		}
		Chat.LogError("Could not connect to Steam. Please restart Steam.", true);
	}

	// Token: 0x060017C5 RID: 6085 RVA: 0x000A2698 File Offset: 0x000A0898
	public void GUISetConfirmWorkshop(string id)
	{
		this.GUIConfirmWorkshop.SetActive(true);
		this.GUIConfirmWorkshop.transform.Find("Confirm ID").gameObject.GetComponent<UIInput>().SetReadOnlyValue(id);
	}

	// Token: 0x060017C6 RID: 6086 RVA: 0x000A26CC File Offset: 0x000A08CC
	public void GUIReloadGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	// Token: 0x060017C7 RID: 6087 RVA: 0x000A26EB File Offset: 0x000A08EB
	public void GUIExitMainMenu()
	{
		UIDialog.Show("Are you sure you want to exit to the Main Menu?", "Yes", "No", new Action(this.GUIDisconnect), null);
	}

	// Token: 0x060017C8 RID: 6088 RVA: 0x000A270E File Offset: 0x000A090E
	public void GUIDisconnect()
	{
		if (this.IsInTutorial)
		{
			this.GUITutorial.GetComponent<TutorialScript>().EndTutorial();
		}
		NetworkSingleton<ManagerPhysicsObject>.Instance.DisableAllObjectLuaEvents();
		Network.Disconnect();
	}

	// Token: 0x060017C9 RID: 6089 RVA: 0x000A2737 File Offset: 0x000A0937
	public void GUISaveSlot(string filePath, string saveName)
	{
		if (Network.isServer)
		{
			SerializationScript.Save(filePath, saveName, true);
			return;
		}
		if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.RequestSaveState(filePath, saveName);
		}
	}

	// Token: 0x060017CA RID: 6090 RVA: 0x000A2764 File Offset: 0x000A0964
	public void GUIStartBlocked()
	{
		TTSUtilities.DestroyChildren(this.GUIBlockedGrid.transform);
		foreach (BlockedPlayer blockedPlayer in Singleton<BlockList>.Instance.BlockedPlayers)
		{
			GameObject gameObject = this.GUIBlockedGrid.AddChild(this.GUIButtonBlocked);
			gameObject.GetComponentsInChildren<UILabel>()[0].text = blockedPlayer.Name;
			gameObject.GetComponentsInChildren<UILabel>()[1].text = blockedPlayer.SteamID;
			gameObject.GetComponent<UIBlockIDHolder>().BlockedName = blockedPlayer.Name;
			gameObject.GetComponent<UIBlockIDHolder>().IDHolder = blockedPlayer.SteamID;
		}
		this.GUIBlockedGrid.GetComponent<UIGrid>().repositionNow = true;
	}

	// Token: 0x060017CB RID: 6091 RVA: 0x000A2830 File Offset: 0x000A0A30
	public void GUIRemoveBlocked(string name, string id)
	{
		Singleton<BlockList>.Instance.RemoveBlock(name, id);
		this.GUIStartBlocked();
	}

	// Token: 0x060017CC RID: 6092 RVA: 0x000A2844 File Offset: 0x000A0A44
	public List<PhysicsState> GUILoadDirectory(string Directory, bool bLoad = true)
	{
		if (Network.isServer)
		{
			return SerializationScript.LoadCJC(Directory, bLoad);
		}
		return null;
	}

	// Token: 0x060017CD RID: 6093 RVA: 0x000A2856 File Offset: 0x000A0A56
	public bool GUIDeleteDirectory(string Directory)
	{
		return SerializationScript.Delete(Directory);
	}

	// Token: 0x060017CE RID: 6094 RVA: 0x000A285E File Offset: 0x000A0A5E
	public void GUISocialBerserkGames()
	{
		TTSUtilities.OpenURL("http://www.berserk-games.com/");
		EventManager.TriggerUnityAnalytic("UI_Social_Website", null, 0);
	}

	// Token: 0x060017CF RID: 6095 RVA: 0x000A2876 File Offset: 0x000A0A76
	public void GUISocialFacebook()
	{
		TTSUtilities.OpenURL("https://www.facebook.com/tabletopsimulator");
		EventManager.TriggerUnityAnalytic("UI_Social_Facebook", null, 0);
	}

	// Token: 0x060017D0 RID: 6096 RVA: 0x000A288E File Offset: 0x000A0A8E
	public void GUISocialTwitter()
	{
		TTSUtilities.OpenURL("https://twitter.com/berserkgames");
		EventManager.TriggerUnityAnalytic("UI_Social_Twitter", null, 0);
	}

	// Token: 0x060017D1 RID: 6097 RVA: 0x000A28A6 File Offset: 0x000A0AA6
	public void GUISocialReddit()
	{
		TTSUtilities.OpenURL("http://www.reddit.com/r/tabletopsimulator");
		EventManager.TriggerUnityAnalytic("UI_Social_Reddit", null, 0);
	}

	// Token: 0x060017D2 RID: 6098 RVA: 0x000A28BE File Offset: 0x000A0ABE
	public void GUISocialWorkshop()
	{
		TTSUtilities.OpenURL("http://steamcommunity.com/app/286160/workshop/");
		EventManager.TriggerUnityAnalytic("UI_Social_Workshop", null, 0);
	}

	// Token: 0x060017D3 RID: 6099 RVA: 0x000A28D6 File Offset: 0x000A0AD6
	public void GUISocialCommunity()
	{
		TTSUtilities.OpenURL("http://steamcommunity.com/app/286160");
		EventManager.TriggerUnityAnalytic("UI_Social_Community", null, 0);
	}

	// Token: 0x060017D4 RID: 6100 RVA: 0x000A28EE File Offset: 0x000A0AEE
	public void GUISocalDiscord()
	{
		TTSUtilities.OpenURL("https://discord.gg/tabletopsimulator");
		EventManager.TriggerUnityAnalytic("UI_Social_Discord", null, 0);
	}

	// Token: 0x060017D5 RID: 6101 RVA: 0x000A2906 File Offset: 0x000A0B06
	public bool GUILayoutButton(Texture texture, params GUILayoutOption[] options)
	{
		bool flag = GUILayout.Button(texture, options);
		if (flag)
		{
			base.GetComponent<SoundScript>().PlayGUISound(this.ButtonSound, 0.3f, 1f);
		}
		return flag;
	}

	// Token: 0x060017D6 RID: 6102 RVA: 0x000A292D File Offset: 0x000A0B2D
	public bool GUILayoutButton(string text, params GUILayoutOption[] options)
	{
		bool flag = GUILayout.Button(text, options);
		if (flag)
		{
			base.GetComponent<SoundScript>().PlayGUISound(this.ButtonSound, 0.3f, 1f);
		}
		return flag;
	}

	// Token: 0x060017D7 RID: 6103 RVA: 0x000A2954 File Offset: 0x000A0B54
	public void GUIStartSingleplayer()
	{
		Network.serverName = Language.Translate("Singleplayer");
		this.StartServerWithZeroConnections();
	}

	// Token: 0x060017D8 RID: 6104 RVA: 0x000A296B File Offset: 0x000A0B6B
	private void StartServerWithZeroConnections()
	{
		Network.maxConnections = 0;
		Network.InitializeServer();
	}

	// Token: 0x060017D9 RID: 6105 RVA: 0x000A2978 File Offset: 0x000A0B78
	public void GUIHostServer(string serverName, string password, string numplayer)
	{
		int maxConnections = Mathf.Clamp(int.Parse(numplayer), 2, 10);
		Network.serverName = serverName;
		Network.password = password;
		Network.maxConnections = maxConnections;
		Network.InitializeServer();
	}

	// Token: 0x060017DA RID: 6106 RVA: 0x000A299E File Offset: 0x000A0B9E
	[Remote(Permission.Admin)]
	public void GUIChangeTable(string TableName)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<string>(RPCTarget.Server, new Action<string>(this.GUIChangeTable), TableName);
			return;
		}
		NetworkSingleton<ManagerPhysicsObject>.Instance.ChangeTable(TableScript.GetTablePrefabName(TableName));
	}

	// Token: 0x060017DB RID: 6107 RVA: 0x000A29D4 File Offset: 0x000A0BD4
	[Remote(Permission.Admin)]
	public void GUIChangeBackground(string SkyName)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<string>(RPCTarget.Server, new Action<string>(this.GUIChangeBackground), SkyName);
			return;
		}
		SkyName = "Sky_" + SkyName;
		if (SkyName != "Sky_Custom")
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.DestroySky();
		}
		NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyCustomSky();
		GameObject prefab = NetworkSingleton<GameMode>.Instance.GetPrefab(SkyName);
		Network.Instantiate(prefab, Vector3.zero, prefab.transform.rotation, default(NetworkPlayer));
	}

	// Token: 0x060017DC RID: 6108 RVA: 0x000A2A5C File Offset: 0x000A0C5C
	[Remote(Permission.Admin)]
	public void GUIChangeGame(string GameString)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<string>(RPCTarget.Server, new Action<string>(this.GUIChangeGame), GameString);
			return;
		}
		NetworkSingleton<ManagerPhysicsObject>.Instance.LoadClassGame(GameString);
	}

	// Token: 0x060017DD RID: 6109 RVA: 0x000A2A8C File Offset: 0x000A0C8C
	public void GUIChangeColor()
	{
		Stats.INT_CHANGE_COLOR_TIMES++;
		UIColorSelection.id = -1;
		if (this.playerLabel != "Grey")
		{
			this.ClientRequestColor("Grey");
		}
		Singleton<CameraController>.Instance.ResetCameraRotation();
		this.bNeedToPickColour = true;
	}

	// Token: 0x060017DE RID: 6110 RVA: 0x000A2ADC File Offset: 0x000A0CDC
	public void GUIMenuSelection(string Value)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(Value);
		if (num <= 2325700662U)
		{
			if (num <= 1116417408U)
			{
				if (num != 442426457U)
				{
					if (num != 1116417408U)
					{
						return;
					}
					if (!(Value == "Rules"))
					{
						return;
					}
					this.GUIRules.SetActive(!this.GUIRules.activeSelf);
				}
				else
				{
					if (!(Value == "Configuration"))
					{
						return;
					}
					this.GUIConfiguration.SetActive(!this.GUIConfiguration.activeSelf);
					return;
				}
			}
			else if (num != 2166136261U)
			{
				if (num != 2325700662U)
				{
					return;
				}
				if (!(Value == "Change Color"))
				{
					return;
				}
				this.GUIChangeColor();
				return;
			}
			else if (Value != null)
			{
				int length = Value.Length;
				return;
			}
			return;
		}
		if (num <= 2942152321U)
		{
			if (num != 2388541882U)
			{
				if (num != 2942152321U)
				{
					return;
				}
				if (!(Value == "Cloud Manager"))
				{
					return;
				}
				this.GUICloud.SetActive(!this.GUICloud.activeSelf);
				return;
			}
			else
			{
				if (!(Value == "Exit to Main Menu"))
				{
					return;
				}
				this.GUIDisconnect();
				return;
			}
		}
		else if (num != 3725030469U)
		{
			if (num != 3956685401U)
			{
				if (num != 4242608950U)
				{
					return;
				}
				if (!(Value == "Change Team"))
				{
					return;
				}
				NGUITools.SetActive(this.GUITeams, !this.GUITeams.activeSelf);
				return;
			}
			else
			{
				if (!(Value == "Control Scheme"))
				{
					return;
				}
				this.GUIControlScheme.SetActive(!this.GUIControlScheme.activeSelf);
				return;
			}
		}
		else
		{
			if (!(Value == "Workshop Upload"))
			{
				return;
			}
			this.GUIWorkshopUpload.GetComponent<UIUploadWorkshop>().ModType = SteamManager.ModInfo.ModType.Save;
			this.GUIWorkshopUpload.SetActive(!this.GUIWorkshopUpload.activeSelf);
			return;
		}
	}

	// Token: 0x060017DF RID: 6111 RVA: 0x000A2CAC File Offset: 0x000A0EAC
	public void GUIHostSelection(string Value)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(Value);
		if (num <= 1550582665U)
		{
			if (num <= 683999818U)
			{
				if (num != 231048258U)
				{
					if (num != 597150953U)
					{
						if (num != 683999818U)
						{
							return;
						}
						if (!(Value == "Backgrounds"))
						{
							return;
						}
						this.GUIBackgrounds.SetActive(!this.GUIBackgrounds.activeSelf);
						return;
					}
					else
					{
						if (!(Value == "Lighting"))
						{
							return;
						}
						this.GUILighting.SetActive(!this.GUILighting.activeSelf);
						return;
					}
				}
				else
				{
					if (!(Value == "Physics"))
					{
						return;
					}
					this.GUIPhysicsOptions.SetActive(!this.GUIPhysicsOptions.activeSelf);
					return;
				}
			}
			else if (num <= 1174571608U)
			{
				if (num != 800286385U)
				{
					if (num != 1174571608U)
					{
						return;
					}
					if (!(Value == "Saved Objects"))
					{
						return;
					}
					this.GUISavedObjects.SetActive(!this.GUISavedObjects.activeSelf);
					return;
				}
				else
				{
					if (!(Value == "Grid"))
					{
						return;
					}
					this.GUIGrid.SetActive(!this.GUIGrid.activeSelf);
					return;
				}
			}
			else if (num != 1338766283U)
			{
				if (num != 1550582665U)
				{
					return;
				}
				if (!(Value == "Components"))
				{
					return;
				}
				this.GUIComponents.SetActive(!this.GUIComponents.activeSelf);
				return;
			}
			else
			{
				if (!(Value == "Turns"))
				{
					return;
				}
				this.GUITurns.SetActive(!this.GUITurns.activeSelf);
				return;
			}
		}
		else if (num <= 2264989059U)
		{
			if (num <= 1945738556U)
			{
				if (num != 1836253938U)
				{
					if (num != 1945738556U)
					{
						return;
					}
					if (!(Value == "Games"))
					{
						return;
					}
					this.GUIGames.SetActive(!this.GUIGames.activeSelf);
					return;
				}
				else
				{
					if (!(Value == "Server"))
					{
						return;
					}
					this.GUIServerOptions.SetActive(!this.GUIServerOptions.activeSelf);
					return;
				}
			}
			else if (num != 1958160182U)
			{
				if (num != 2264989059U)
				{
					return;
				}
				if (!(Value == "Permissions"))
				{
					return;
				}
				this.GUIPermissions.SetActive(!this.GUIPermissions.activeSelf);
				return;
			}
			else
			{
				if (!(Value == "Scripting"))
				{
					return;
				}
				this.GUILuaNotepad.Init(null);
				return;
			}
		}
		else if (num <= 2505442276U)
		{
			if (num != 2413946405U)
			{
				if (num != 2505442276U)
				{
					return;
				}
				if (!(Value == "Tables"))
				{
					return;
				}
				this.GUITables.SetActive(!this.GUITables.activeSelf);
				return;
			}
			else
			{
				if (!(Value == "Info"))
				{
					return;
				}
				this.GUIInfoOptions.SetActive(!this.GUIInfoOptions.activeSelf);
				return;
			}
		}
		else if (num != 2849239369U)
		{
			if (num != 3714171437U)
			{
				return;
			}
			if (!(Value == "Game Keys"))
			{
				return;
			}
			NetworkSingleton<UserDefinedHotkeyManager>.Instance.ShowSettings(false);
			return;
		}
		else
		{
			if (!(Value == "Hands"))
			{
				return;
			}
			this.GUIHands.SetActive(!this.GUIHands.activeSelf);
			return;
		}
	}

	// Token: 0x060017E0 RID: 6112 RVA: 0x000A3000 File Offset: 0x000A1200
	public void GUIPlayerSelection(string Value, int playerID)
	{
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(playerID);
		string name = playerState.name;
		if (!NetworkSingleton<PlayerManager>.Instance.NameInUse(name))
		{
			Chat.Log("Player " + name + " does not exist.", ChatMessageType.Game);
			return;
		}
		uint num = <PrivateImplementationDetails>.ComputeStringHash(Value);
		if (num <= 1523018852U)
		{
			if (num <= 754690623U)
			{
				if (num != 15595271U)
				{
					if (num != 272321435U)
					{
						if (num != 754690623U)
						{
							return;
						}
						if (!(Value == "Kick"))
						{
							return;
						}
						NetworkSingleton<PlayerManager>.Instance.KickThisPlayer(name);
						return;
					}
					else if (!(Value == "Promote"))
					{
						return;
					}
				}
				else
				{
					if (!(Value == "Pass Turn"))
					{
						return;
					}
					goto IL_247;
				}
			}
			else if (num != 840728665U)
			{
				if (num != 1063296884U)
				{
					if (num != 1523018852U)
					{
						return;
					}
					if (!(Value == "Change Name"))
					{
						return;
					}
					this.playerIDToSet = playerID;
					UIDialog.ShowInput(Language.Translate("Enter name of player {0}", playerID.ToString()), "OK", "Cancel", new Action<string>(this.SetSpecificPlayerName), null, playerState.name, "Player Name");
					return;
				}
				else
				{
					if (!(Value == "Mute"))
					{
						return;
					}
					goto IL_1A4;
				}
			}
			else if (!(Value == "Demote"))
			{
				return;
			}
			NetworkSingleton<PlayerManager>.Instance.PromoteThisPlayer(name);
			return;
		}
		if (num <= 3144250043U)
		{
			if (num != 1545440182U)
			{
				if (num != 2325700662U)
				{
					if (num != 3144250043U)
					{
						return;
					}
					if (!(Value == "Unmute"))
					{
						return;
					}
				}
				else
				{
					if (!(Value == "Change Color"))
					{
						return;
					}
					if (playerID != NetworkID.ID)
					{
						UIColorSelection.id = playerID;
						this.bNeedToPickColour = true;
						return;
					}
					this.GUIChangeColor();
					return;
				}
			}
			else
			{
				if (!(Value == "Set Turn"))
				{
					return;
				}
				goto IL_247;
			}
		}
		else if (num != 3216789926U)
		{
			if (num != 4073006726U)
			{
				if (num != 4242608950U)
				{
					return;
				}
				if (!(Value == "Change Team"))
				{
					return;
				}
				NGUITools.SetActive(this.GUITeams, true);
				if (playerID != NetworkID.ID)
				{
					UITeamButton.OverrideId = playerID;
					return;
				}
				UITeamButton.OverrideId = -1;
				return;
			}
			else
			{
				if (!(Value == "Give Host"))
				{
					return;
				}
				NetworkSingleton<HostMigrationManager>.Instance.AskToMigrate(new NetworkPlayer((ushort)playerID));
				return;
			}
		}
		else
		{
			if (!(Value == "Ban"))
			{
				return;
			}
			NetworkSingleton<PlayerManager>.Instance.BanThisPlayer(name);
			return;
		}
		IL_1A4:
		NetworkSingleton<PlayerManager>.Instance.MuteThisPlayer(name);
		return;
		IL_247:
		NetworkSingleton<Turns>.Instance.RPCSetPlayerTurn(name, RPCTarget.All);
	}

	// Token: 0x060017E1 RID: 6113 RVA: 0x000A3272 File Offset: 0x000A1472
	public void GUIStartTutorial()
	{
		Network.serverName = (Network.gameName = Language.Translate(this.GameTutorial));
		this.IsInTutorial = true;
		this.StartServerWithZeroConnections();
		this.GUITutorial.SetActive(true);
	}

	// Token: 0x060017E2 RID: 6114 RVA: 0x000A32A3 File Offset: 0x000A14A3
	public void FlipTable()
	{
		if (this.bCanFlipTable)
		{
			Stats.INT_FLIP_TABLE_TIMES++;
			this.bCanFlipTable = false;
			base.Invoke("CanFlipTable", 2f);
			NetworkSingleton<ManagerPhysicsObject>.Instance.TableScript.FlipTable();
		}
	}

	// Token: 0x060017E3 RID: 6115 RVA: 0x000A32DF File Offset: 0x000A14DF
	private void TutorialConfirm()
	{
		EventManager.TriggerUnityAnalytic("Tutorial_Prompt", "Accept", true, 0);
		this.GUIStartTutorial();
	}

	// Token: 0x060017E4 RID: 6116 RVA: 0x000A32FD File Offset: 0x000A14FD
	private void TutorialCancel()
	{
		EventManager.TriggerUnityAnalytic("Tutorial_Prompt", "Accept", false, 0);
	}

	// Token: 0x060017E5 RID: 6117 RVA: 0x000A3315 File Offset: 0x000A1515
	public void GUIExitGame()
	{
		UIDialog.Show("Are you sure you want to Quit Tabletop Simulator?", "Yes", "No", new Action(this.ExitGameConfirm), null);
	}

	// Token: 0x060017E6 RID: 6118 RVA: 0x000A3338 File Offset: 0x000A1538
	public void ExitGameConfirm()
	{
		NetworkUI.bCleanApplicationQuit = true;
		Application.Quit();
	}

	// Token: 0x060017E7 RID: 6119 RVA: 0x000A3348 File Offset: 0x000A1548
	private void OnLanguageChange(string previousLanguageCode, string newLanguageCode)
	{
		if (Network.maxConnections == 0)
		{
			if (this.bHotseat)
			{
				Network.serverName = Language.Translate("Hotseat");
				return;
			}
			if (this.IsInTutorial)
			{
				Network.serverName = Language.Translate(this.GameTutorial);
				return;
			}
			Network.serverName = Language.Translate("Singleplayer");
		}
	}

	// Token: 0x060017E8 RID: 6120 RVA: 0x000A339C File Offset: 0x000A159C
	public void GUIStartHotseat(string Num)
	{
		this.bNeedToPickColour = false;
		this.bHotseat = true;
		this.NumHotseat = int.Parse(Num);
		this.askedForGame = false;
		this.GUIStartSingleplayer();
		Network.serverName = Language.Translate("Hotseat");
		this.GUIGames.SetActive(false);
		Singleton<CameraController>.Instance.ResetCameraRotation();
		for (int i = 1; i <= this.NumHotseat; i++)
		{
			NetworkSingleton<PlayerManager>.Instance.AddPlayer(new PlayerManager.PlayerData("Player " + i.ToString(), Network.player, SteamManager.StringSteamID, VRHMD.isVR, "Grey", false, Team.None, false));
		}
		this.CurrentHotseat = 1;
		if (Turns.HotseatAskForNames)
		{
			this.GUIStartConfirmHotseatAndGetName(1);
			return;
		}
		this.GUIStartConfirmHotseat(1, true);
	}

	// Token: 0x060017E9 RID: 6121 RVA: 0x000A345C File Offset: 0x000A165C
	public void GUIStartConfirmHotseat(int id, bool forceDialog = false)
	{
		GameObject pointer = PlayerScript.Pointer;
		if (pointer)
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(pointer);
		}
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(id);
		bool flag = Turns.ConfirmHotseatTurnStart || forceDialog;
		this.GUIConfirmHotseat.SetActive(flag);
		this.GUIConfirmHotseat.GetComponentInChildren<UILabel>().text = playerState.name + "'s turn";
		this.CurrentHotseat = id;
		Utilities.SetCursor(this.StringColorToCursorTexture("Grey"), NetworkUI.HardwareCursorOffest, CursorMode.Auto);
		this.GUIEndTurn.SetActive(false);
		this.HotseatBetweenTurns = true;
		NetworkSingleton<ManagerPhysicsObject>.Instance.UpdateVisibility();
		if (Turns.HotseatCameraReset)
		{
			Singleton<CameraController>.Instance.ResetCameraRotation(playerState.stringColor);
		}
		if (!flag)
		{
			Wait.Time(new Action(this.GUIHotseatOK), Turns.HotseatTurnInterval, 1);
		}
	}

	// Token: 0x060017EA RID: 6122 RVA: 0x000A3530 File Offset: 0x000A1730
	public void GUIStartConfirmHotseatAndGetName(int id)
	{
		GameObject pointer = PlayerScript.Pointer;
		if (pointer)
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(pointer);
		}
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(id);
		this.CurrentHotseat = id;
		Utilities.SetCursor(this.StringColorToCursorTexture("Grey"), NetworkUI.HardwareCursorOffest, CursorMode.Auto);
		this.GUIEndTurn.SetActive(false);
		NetworkSingleton<ManagerPhysicsObject>.Instance.UpdateVisibility();
		if (Turns.HotseatCameraReset)
		{
			Singleton<CameraController>.Instance.ResetCameraRotation(playerState.stringColor);
		}
		this.playerIDToSet = id;
		UIDialog.ShowInput(Language.Translate("Enter name of player {0}", id.ToString()), "OK", "Cancel", new Action<string>(this.SetSpecificPlayerNameThenOKHotseat), new Action<string>(this.GUIHotseatOKDummy), playerState.name, "Player Name");
	}

	// Token: 0x060017EB RID: 6123 RVA: 0x000A35F7 File Offset: 0x000A17F7
	private void GUIHotseatOKDummy(string _)
	{
		this.GUIHotseatOK();
	}

	// Token: 0x060017EC RID: 6124 RVA: 0x000A3600 File Offset: 0x000A1800
	public void GUIHotseatOK()
	{
		this.HotseatBetweenTurns = false;
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(this.CurrentHotseat);
		this.playerName = playerState.name;
		this.playerColour = playerState.color;
		NetworkID.OverrideID(playerState.id);
		if (this.playerColour == Colour.Grey)
		{
			this.bNeedToPickColour = true;
			return;
		}
		if (!PlayerScript.Pointer && playerState.stringColor != "Grey")
		{
			Network.Instantiate(this.StringColorToPrefab(playerState.stringColor), Vector3.zero, Vector3.zero, default(NetworkPlayer));
		}
		this.InitializePlayer(this.playerColour, this.StringColorToPrefab(playerState.stringColor));
		NetworkSingleton<Turns>.Instance.SetPlayerTurn(playerState.stringColor);
		NetworkSingleton<ManagerPhysicsObject>.Instance.UpdateVisibility();
		EventManager.TriggerPlayerTurnStart(this.HotseatPreviousColor, playerState.stringColor);
		this.HotseatPreviousColor = playerState.stringColor;
	}

	// Token: 0x060017ED RID: 6125 RVA: 0x000A36F9 File Offset: 0x000A18F9
	private void SetSpecificPlayerNameThenOKHotseat(string name)
	{
		this.SetSpecificPlayerName(name);
		this.GUIHotseatOK();
	}

	// Token: 0x060017EE RID: 6126 RVA: 0x000A3708 File Offset: 0x000A1908
	public static int HotseatNameToId(string name)
	{
		return NetworkUI.HotseatNameToNumber(name);
	}

	// Token: 0x060017EF RID: 6127 RVA: 0x000A3710 File Offset: 0x000A1910
	public static int HotseatNameToNumber(string name)
	{
		return int.Parse(name.Replace("Player ", ""));
	}

	// Token: 0x060017F0 RID: 6128 RVA: 0x000A3728 File Offset: 0x000A1928
	public void GUILockContextual(bool value)
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer)
		{
			return;
		}
		pointer.GetComponent<Pointer>().Lock(-1, value ? 1 : 0);
	}

	// Token: 0x060017F1 RID: 6129 RVA: 0x000A3758 File Offset: 0x000A1958
	public void GUIDragSelectableContextual(bool value)
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer)
		{
			return;
		}
		pointer.GetComponent<Pointer>().DragSelectable(value);
	}

	// Token: 0x060017F2 RID: 6130 RVA: 0x000A3780 File Offset: 0x000A1980
	public void GUIMeasureMovementContextual(bool value)
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer)
		{
			return;
		}
		pointer.GetComponent<Pointer>().MeasureMovement(value);
	}

	// Token: 0x060017F3 RID: 6131 RVA: 0x000A37A8 File Offset: 0x000A19A8
	public void GUIGridContextual(bool value)
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer)
		{
			return;
		}
		pointer.GetComponent<Pointer>().Grid(value);
	}

	// Token: 0x060017F4 RID: 6132 RVA: 0x000A37D0 File Offset: 0x000A19D0
	public void GUISnapContextual(bool value)
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer)
		{
			return;
		}
		pointer.GetComponent<Pointer>().Snap(value);
	}

	// Token: 0x060017F5 RID: 6133 RVA: 0x000A37F8 File Offset: 0x000A19F8
	public void GUIDestroyableContextual(bool value)
	{
		if (!NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
		{
			return;
		}
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer)
		{
			return;
		}
		pointer.GetComponent<Pointer>().Destroyable(!value);
	}

	// Token: 0x060017F6 RID: 6134 RVA: 0x000A3834 File Offset: 0x000A1A34
	public void GUIAutoraiseContextual(bool value)
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer)
		{
			return;
		}
		pointer.GetComponent<Pointer>().Autoraise(value);
	}

	// Token: 0x060017F7 RID: 6135 RVA: 0x000A385C File Offset: 0x000A1A5C
	public void GUIStickyContextual(bool value)
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer)
		{
			return;
		}
		pointer.GetComponent<Pointer>().Sticky(value);
	}

	// Token: 0x060017F8 RID: 6136 RVA: 0x000A3884 File Offset: 0x000A1A84
	public void GUITooltipContextual(bool value)
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer)
		{
			return;
		}
		pointer.GetComponent<Pointer>().Tooltip(value);
	}

	// Token: 0x060017F9 RID: 6137 RVA: 0x000A38AC File Offset: 0x000A1AAC
	public void GUIFogOfWarRevealContextual(bool value)
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer)
		{
			return;
		}
		Debug.Log("GUIFogOfWarRevealContextual");
		Debug.Log(value);
		pointer.GetComponent<Pointer>().FogOfWarReveal(value);
	}

	// Token: 0x060017FA RID: 6138 RVA: 0x000A38EC File Offset: 0x000A1AEC
	public void GUIGridProjectionContextual(bool value)
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer)
		{
			return;
		}
		pointer.GetComponent<Pointer>().GridProjection(value);
	}

	// Token: 0x060017FB RID: 6139 RVA: 0x000A3914 File Offset: 0x000A1B14
	public void GUIHandsContextual(bool value)
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer)
		{
			return;
		}
		pointer.GetComponent<Pointer>().Hands(value);
	}

	// Token: 0x060017FC RID: 6140 RVA: 0x000A393C File Offset: 0x000A1B3C
	public void GUIHideFaceDownContextual(bool value)
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer)
		{
			return;
		}
		pointer.GetComponent<Pointer>().HideFaceDown(value);
	}

	// Token: 0x060017FD RID: 6141 RVA: 0x000A3964 File Offset: 0x000A1B64
	public void GUIIgnoreFogOfWarContextual(bool value)
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer)
		{
			return;
		}
		pointer.GetComponent<Pointer>().IgnoreFogOfWar(value);
	}

	// Token: 0x060017FE RID: 6142 RVA: 0x000A398C File Offset: 0x000A1B8C
	public void GUIMaterialContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer)
		{
			return;
		}
		int matInt = -1;
		if (this.GUIContextualPlasticBool.GetComponent<UIToggle>().value || this.GUIContextualWoodBool.GetComponent<UIToggle>().value)
		{
			matInt = 0;
		}
		else if (this.GUIContextualMetalBool.GetComponent<UIToggle>().value)
		{
			matInt = 1;
		}
		else if (this.GUIContextualGoldBool.GetComponent<UIToggle>().value)
		{
			matInt = 2;
		}
		pointer.GetComponent<Pointer>().Material(matInt);
	}

	// Token: 0x060017FF RID: 6143 RVA: 0x000A3A08 File Offset: 0x000A1C08
	public void GUIInfoContextual(string Name, string Desc, string Value)
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer)
		{
			return;
		}
		int value = 0;
		try
		{
			value = Convert.ToInt32(Value);
		}
		catch (FormatException)
		{
		}
		pointer.GetComponent<Pointer>().Info(Name, Desc, value);
	}

	// Token: 0x06001800 RID: 6144 RVA: 0x000A3A50 File Offset: 0x000A1C50
	public void GUIGMContextual(string GMNotes)
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer)
		{
			return;
		}
		pointer.GetComponent<Pointer>().GMInfo(GMNotes);
	}

	// Token: 0x06001801 RID: 6145 RVA: 0x000A3A78 File Offset: 0x000A1C78
	public void GUIColorContextual(Color NewColor)
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer)
		{
			return;
		}
		pointer.GetComponent<Pointer>().DiffuseColor(NewColor);
	}

	// Token: 0x06001802 RID: 6146 RVA: 0x000A3AA0 File Offset: 0x000A1CA0
	public void GUICollapseContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (pointer)
		{
			pointer.GetComponent<Pointer>().InfoObject;
		}
	}

	// Token: 0x06001803 RID: 6147 RVA: 0x000A3ACC File Offset: 0x000A1CCC
	public void GUICustomContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject)
		{
			return;
		}
		if (PlayerScript.PointerScript.InfoObject.GetComponent<CustomObject>())
		{
			PlayerScript.PointerScript.InfoObject.GetComponent<CustomObject>().bCustomUI = true;
		}
	}

	// Token: 0x06001804 RID: 6148 RVA: 0x000A3B28 File Offset: 0x000A1D28
	public void GUIRollContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject)
		{
			return;
		}
		PlayerScript.PointerScript.Randomize(-1, PlayerScript.PointerScript.ID);
	}

	// Token: 0x06001805 RID: 6149 RVA: 0x000A3B6C File Offset: 0x000A1D6C
	public void GUIGroupContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject)
		{
			return;
		}
		PlayerScript.PointerScript.Group(-1);
	}

	// Token: 0x06001806 RID: 6150 RVA: 0x000A3BA5 File Offset: 0x000A1DA5
	public void GUICheckJigsawContextual()
	{
		CustomJigsawPuzzle.Instance.Check();
		PlayerScript.PointerScript.ResetInfoObject();
	}

	// Token: 0x06001807 RID: 6151 RVA: 0x000A3BBC File Offset: 0x000A1DBC
	public void GUISearchContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject)
		{
			return;
		}
		NetworkSingleton<ManagerPhysicsObject>.Instance.SearchInventory(PlayerScript.PointerScript.InfoObject.GetComponent<NetworkPhysicsObject>().ID, NetworkID.ID, -1);
		PlayerScript.PointerScript.ResetHighlight();
		PlayerScript.PointerScript.ResetInfoObject();
	}

	// Token: 0x06001808 RID: 6152 RVA: 0x000A3C24 File Offset: 0x000A1E24
	public void GUISearchPublicContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (pointer)
		{
			pointer.GetComponent<Pointer>().InfoObject;
		}
	}

	// Token: 0x06001809 RID: 6153 RVA: 0x000A3C50 File Offset: 0x000A1E50
	public void GUISpreadContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject)
		{
			return;
		}
		List<NetworkPhysicsObject> selectedNPOs = PlayerScript.PointerScript.GetSelectedNPOs(-1, true, false);
		for (int i = 0; i < selectedNPOs.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = selectedNPOs[i];
			if (networkPhysicsObject.deckScript != null)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.SpreadDeck(networkPhysicsObject.ID, 0f, 0f, 0);
			}
		}
	}

	// Token: 0x0600180A RID: 6154 RVA: 0x000A3CD0 File Offset: 0x000A1ED0
	public void GUILayoutContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject)
		{
			return;
		}
		List<NetworkPhysicsObject> selectedNPOs = PlayerScript.PointerScript.GetSelectedNPOs(-1, true, false);
		this.zonesToLayout.Clear();
		for (int i = 0; i < selectedNPOs.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = selectedNPOs[i];
			LayoutZone layoutZone = networkPhysicsObject.layoutZone;
			int num;
			if (layoutZone)
			{
				this.zonesToLayout.TryAddUnique(layoutZone);
			}
			else if (LayoutZone.TryNPOInLayoutZone(networkPhysicsObject, out layoutZone, out num, LayoutZone.PotentialZoneCheck.Both))
			{
				this.zonesToLayout.TryAddUnique(layoutZone);
			}
		}
		for (int j = 0; j < this.zonesToLayout.Count; j++)
		{
			this.zonesToLayout[j].ManualLayoutZone();
		}
	}

	// Token: 0x0600180B RID: 6155 RVA: 0x000A3D98 File Offset: 0x000A1F98
	public void GUIShuffleContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject)
		{
			return;
		}
		List<GameObject> selectedObjects = PlayerScript.PointerScript.GetSelectedObjects(-1, true, false);
		Dictionary<string, List<NetworkPhysicsObject>> dictionary = new Dictionary<string, List<NetworkPhysicsObject>>();
		foreach (string key in Colour.HandPlayerLabels)
		{
			dictionary[key] = new List<NetworkPhysicsObject>();
		}
		bool flag = false;
		foreach (GameObject gameObject in selectedObjects)
		{
			if (gameObject)
			{
				NetworkPhysicsObject component = gameObject.GetComponent<NetworkPhysicsObject>();
				if (NetworkSingleton<ManagerPhysicsObject>.Instance.CheckLuaTryRandomize(component, PlayerScript.PointerScript.ID))
				{
					if (gameObject.CompareTag("Jigsaw Box"))
					{
						if (Network.isAdmin)
						{
							CustomJigsawPuzzle.Instance.Randomize(false);
							flag = true;
						}
					}
					else
					{
						if (gameObject.GetComponent<StackObject>() || gameObject.GetComponent<DeckScript>() || component.GetSelectedStateId() != -1)
						{
							NetworkSingleton<ManagerPhysicsObject>.Instance.Shuffle(component.ID, NetworkID.ID);
						}
						if (component.CurrentPlayerHand)
						{
							dictionary[component.CurrentPlayerHand.TriggerLabel].Add(component);
						}
					}
				}
			}
		}
		foreach (KeyValuePair<string, List<NetworkPhysicsObject>> keyValuePair in dictionary)
		{
			if (keyValuePair.Value.Count > 1)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.RandomizeObjectsInHand(keyValuePair.Value);
			}
		}
		if (flag)
		{
			PlayerScript.PointerScript.ResetInfoObject();
		}
	}

	// Token: 0x0600180C RID: 6156 RVA: 0x000A3F6C File Offset: 0x000A216C
	public void GUIDealContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject)
		{
			return;
		}
		foreach (GameObject gameObject in PlayerScript.PointerScript.GetSelectedObjects(-1, true, false))
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.DealObjectToColor(gameObject.GetComponent<NetworkPhysicsObject>().ID, "Seated", 1, 0);
		}
	}

	// Token: 0x0600180D RID: 6157 RVA: 0x000A3FFC File Offset: 0x000A21FC
	public void GUIDrawContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject)
		{
			return;
		}
		foreach (GameObject gameObject in PlayerScript.PointerScript.GetSelectedObjects(-1, true, false))
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.DealObjectToColor(gameObject.GetComponent<NetworkPhysicsObject>().ID, PlayerScript.PointerScript.PointerColorLabel, 1, 0);
		}
	}

	// Token: 0x0600180E RID: 6158 RVA: 0x000A4094 File Offset: 0x000A2294
	public void GUIEditTags()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer)
		{
			return;
		}
		Pointer component = pointer.GetComponent<Pointer>();
		if (!component || !component.InfoObject)
		{
			return;
		}
		PointerMode currentPointerMode = component.CurrentPointerMode;
		if (currentPointerMode <= PointerMode.Hands)
		{
			if (currentPointerMode == PointerMode.Hidden)
			{
				Singleton<UIComponentTagDialog>.Instance.EditNPOs(PlayerScript.PointerScript.GetSelectedNPOs(-1, true, false), "Hidden Zone Tags", "Edit tags on hidden zone.");
				return;
			}
			if (currentPointerMode == PointerMode.Randomize)
			{
				Singleton<UIComponentTagDialog>.Instance.EditNPOs(PlayerScript.PointerScript.GetSelectedNPOs(-1, true, false), "Randomize Zone Tags", "Edit tags on randomize zone.");
				return;
			}
			if (currentPointerMode == PointerMode.Hands)
			{
				Singleton<UIComponentTagDialog>.Instance.EditNPOs(new List<NetworkPhysicsObject>
				{
					component.InfoObject.GetComponent<NetworkPhysicsObject>()
				}, "Hand Zone Tags", "Edit tags on hand zone.");
				return;
			}
		}
		else
		{
			if (currentPointerMode == PointerMode.Scripting)
			{
				Singleton<UIComponentTagDialog>.Instance.EditNPOs(PlayerScript.PointerScript.GetSelectedNPOs(-1, true, false), "Scripting Zone Tags", "Edit tags on scripting zone.");
				return;
			}
			if (currentPointerMode == PointerMode.FogOfWar)
			{
				Singleton<UIComponentTagDialog>.Instance.EditNPOs(PlayerScript.PointerScript.GetSelectedNPOs(-1, true, false), "Fog of War Tags", "Edit tags on fog of war zone.");
				return;
			}
			if (currentPointerMode == PointerMode.LayoutZone)
			{
				Singleton<UIComponentTagDialog>.Instance.EditNPOs(PlayerScript.PointerScript.GetSelectedNPOs(-1, true, false), "Layout Zone Tags", "Edit tags on layout zone.");
				return;
			}
		}
		Singleton<UIComponentTagDialog>.Instance.EditNPOs(PlayerScript.PointerScript.GetSelectedNPOs(-1, true, false), "Component Tags", "Edit tags on selected components.");
	}

	// Token: 0x0600180F RID: 6159 RVA: 0x000A41FC File Offset: 0x000A23FC
	public void GUICutContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject)
		{
			return;
		}
		int amount = Convert.ToInt32(NetworkSingleton<NetworkUI>.Instance.GUIContextualDeckCut.GetComponentInChildren<UISlider>(true).GetComponentInChildren<UILabel>().text);
		GameObject infoObject = PlayerScript.PointerScript.InfoObject;
		if (infoObject && infoObject.CompareTag("Deck"))
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.CutDeck(infoObject.GetComponent<NetworkPhysicsObject>().ID, amount);
		}
		if (infoObject && infoObject.GetComponent<StackObject>() && !infoObject.GetComponent<StackObject>().bBag && !infoObject.GetComponent<StackObject>().IsInfiniteStack)
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.CutStack(infoObject.GetComponent<NetworkPhysicsObject>().ID, amount);
		}
	}

	// Token: 0x06001810 RID: 6160 RVA: 0x000A42C8 File Offset: 0x000A24C8
	public void GUISplitContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject)
		{
			return;
		}
		foreach (GameObject gameObject in PlayerScript.PointerScript.GetSelectedObjects(-1, true, false))
		{
			if (gameObject && gameObject.CompareTag("Deck"))
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.SplitDeck(gameObject.GetComponent<NetworkPhysicsObject>().ID, 2);
			}
			if (gameObject && gameObject.GetComponent<StackObject>() && !gameObject.GetComponent<StackObject>().bBag && !gameObject.GetComponent<StackObject>().IsInfiniteStack)
			{
				NetworkSingleton<ManagerPhysicsObject>.Instance.SplitStack(gameObject.GetComponent<NetworkPhysicsObject>().ID, 2);
			}
		}
	}

	// Token: 0x06001811 RID: 6161 RVA: 0x000A43B4 File Offset: 0x000A25B4
	public void GUIResetContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject)
		{
			return;
		}
		foreach (GameObject gameObject in PlayerScript.PointerScript.GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				if (gameObject.CompareTag("Jigsaw Box"))
				{
					if (Network.isAdmin)
					{
						CustomJigsawPuzzle.Instance.Solve();
					}
				}
				else
				{
					if (gameObject.CompareTag("Deck"))
					{
						NetworkSingleton<ManagerPhysicsObject>.Instance.ResetDeck(gameObject.GetComponent<NetworkPhysicsObject>().ID);
					}
					if (gameObject.GetComponent<StackObject>() && gameObject.GetComponent<StackObject>().IsInfiniteStack && gameObject.GetComponent<StackObject>().ObjectsHolder.Count > 0)
					{
						gameObject.GetComponent<StackObject>().ResetObjectsContained();
					}
					if (gameObject.GetComponent<NetworkPhysicsObject>().GetSelectedStateId() != -1)
					{
						PlayerScript.PointerScript.ResetHighlight();
						PlayerScript.PointerScript.ResetInfoObject();
						gameObject.GetComponent<NetworkPhysicsObject>().CreateStates(PlayerScript.PointerScript.ID);
					}
				}
			}
		}
		PlayerScript.PointerScript.ResetInfoObject();
	}

	// Token: 0x06001812 RID: 6162 RVA: 0x000A44FC File Offset: 0x000A26FC
	public void GUIResetAllContextual()
	{
		NetworkSingleton<ManagerPhysicsObject>.Instance.ResetAllCards();
	}

	// Token: 0x06001813 RID: 6163 RVA: 0x000A450C File Offset: 0x000A270C
	public void GUICurrentTimeContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject)
		{
			return;
		}
		pointer.GetComponent<Pointer>().InfoObject.GetComponent<ClockScript>().StartCurrentTime();
	}

	// Token: 0x06001814 RID: 6164 RVA: 0x000A4550 File Offset: 0x000A2750
	public void GUIStopwatchContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject)
		{
			return;
		}
		pointer.GetComponent<Pointer>().InfoObject.GetComponent<ClockScript>().StartStopwatch();
	}

	// Token: 0x06001815 RID: 6165 RVA: 0x000A4594 File Offset: 0x000A2794
	public void GUIPauseContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject)
		{
			return;
		}
		pointer.GetComponent<Pointer>().InfoObject.GetComponent<ClockScript>().PauseStart();
	}

	// Token: 0x06001816 RID: 6166 RVA: 0x000A45D8 File Offset: 0x000A27D8
	public void GUITimerContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject)
		{
			return;
		}
		int num;
		if (int.TryParse(this.GUIContextualClockTimerInput.GetComponent<UIInput>().value, out num))
		{
			pointer.GetComponent<Pointer>().InfoObject.GetComponent<ClockScript>().StartTimer(++num, false);
			return;
		}
		Chat.Log("Must type in a seconds amount for the timer.", Colour.Red, ChatMessageType.Game, false);
	}

	// Token: 0x06001817 RID: 6167 RVA: 0x000A464C File Offset: 0x000A284C
	public void GUIFogOfWarToggleRevealActive(bool active)
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject)
		{
			return;
		}
		foreach (GameObject gameObject in PlayerScript.PointerScript.GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				base.networkView.RPC<NetworkPhysicsObject, bool>(RPCTarget.All, new Action<NetworkPhysicsObject, bool>(this.RPCToggleFogOfWarRevealer), NetworkSingleton<ManagerPhysicsObject>.Instance.NPOFromGO(gameObject), active);
			}
		}
		Wait.Time(delegate
		{
			pointer.GetComponent<Pointer>().ReStartContextual();
		}, 0.25f, 1);
	}

	// Token: 0x06001818 RID: 6168 RVA: 0x000A471C File Offset: 0x000A291C
	[Remote("Permissions/Contextual")]
	public void RPCToggleFogOfWarRevealer(NetworkPhysicsObject npo, bool active)
	{
		npo.fogOfWarRevealer.Active = active;
		npo.IgnoresFogOfWar = active;
		if (active)
		{
			npo.DoAutoRaise = false;
		}
	}

	// Token: 0x06001819 RID: 6169 RVA: 0x000A473C File Offset: 0x000A293C
	public void GUIPictureInPictureEnable()
	{
		int[] panelPrefs = Singleton<UISpectatorView>.Instance.GetPanelPrefs();
		Singleton<SpectatorCamera>.Instance.TurnOn(panelPrefs[2], panelPrefs[3], 0, 0, true, panelPrefs[0], panelPrefs[1], true, true, true, false);
	}

	// Token: 0x0600181A RID: 6170 RVA: 0x000A4771 File Offset: 0x000A2971
	public void GUIBlindfoldToggle()
	{
		NetworkSingleton<PlayerManager>.Instance.ToggleBlindfold();
	}

	// Token: 0x0600181B RID: 6171 RVA: 0x000A4780 File Offset: 0x000A2980
	public void GUIFogOfWarSetRevealRange()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject)
		{
			return;
		}
		float floatValue = NetworkSingleton<NetworkUI>.Instance.GUIContextualFogOfWarRevealRange.GetComponentInChildren<UISliderRange>(true).floatValue;
		foreach (GameObject gameObject in PlayerScript.PointerScript.GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				gameObject.GetComponent<FogOfWarRevealer>().Range = floatValue;
			}
		}
	}

	// Token: 0x0600181C RID: 6172 RVA: 0x000A4820 File Offset: 0x000A2A20
	public void GUIFogOfWarSetRevealHeight()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject)
		{
			return;
		}
		float floatValue = NetworkSingleton<NetworkUI>.Instance.GUIContextualFogOfWarRevealHeight.GetComponentInChildren<UISliderRange>(true).floatValue;
		foreach (GameObject gameObject in PlayerScript.PointerScript.GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				gameObject.GetComponent<FogOfWarRevealer>().Height = floatValue;
			}
		}
	}

	// Token: 0x0600181D RID: 6173 RVA: 0x000A48C0 File Offset: 0x000A2AC0
	public void GUIFogOfWarSetRevealFoV()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject)
		{
			return;
		}
		float floatValue = NetworkSingleton<NetworkUI>.Instance.GUIContextualFogOfWarRevealFoV.GetComponentInChildren<UISliderRange>(true).floatValue;
		foreach (GameObject gameObject in PlayerScript.PointerScript.GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				gameObject.GetComponent<FogOfWarRevealer>().FoV = floatValue;
			}
		}
	}

	// Token: 0x0600181E RID: 6174 RVA: 0x000A4960 File Offset: 0x000A2B60
	public void GUIFogOfWarSetRevealFoVOffset()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject)
		{
			return;
		}
		float floatValue = NetworkSingleton<NetworkUI>.Instance.GUIContextualFogOfWarRevealFoVOffset.GetComponentInChildren<UISliderRange>(true).floatValue;
		foreach (GameObject gameObject in PlayerScript.PointerScript.GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				gameObject.GetComponent<FogOfWarRevealer>().FoVOffset = floatValue;
			}
		}
	}

	// Token: 0x0600181F RID: 6175 RVA: 0x000A4A00 File Offset: 0x000A2C00
	public void GUIFogOfWarToggleOutline()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject)
		{
			return;
		}
		bool value = NetworkSingleton<NetworkUI>.Instance.GUIContextualFogOfWarRevealOutline.GetComponentInChildren<UIToggle>(true).value;
		foreach (GameObject gameObject in PlayerScript.PointerScript.GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				gameObject.GetComponent<FogOfWarRevealer>().ShowFoWOutline = value;
			}
		}
	}

	// Token: 0x06001820 RID: 6176 RVA: 0x000A4AA0 File Offset: 0x000A2CA0
	public void GUIFlipContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		Pointer pointer2 = (pointer != null) ? pointer.GetComponent<Pointer>() : null;
		if (!pointer2 || !pointer2.InfoObject)
		{
			return;
		}
		pointer2.Rotation(-1, 0, 12, false);
	}

	// Token: 0x06001821 RID: 6177 RVA: 0x000A4AE8 File Offset: 0x000A2CE8
	private void Rotate(bool left)
	{
		GameObject pointer = PlayerScript.Pointer;
		Pointer pointer2 = (pointer != null) ? pointer.GetComponent<Pointer>() : null;
		if (!pointer2 || !pointer2.InfoObject)
		{
			return;
		}
		int num = pointer2.RotationSnap / 15;
		if (left)
		{
			num = 24 - num;
		}
		pointer2.Rotation(-1, num, 0, false);
	}

	// Token: 0x06001822 RID: 6178 RVA: 0x000A4B40 File Offset: 0x000A2D40
	public void GUIRotateLeftContextual()
	{
		this.Rotate(true);
	}

	// Token: 0x06001823 RID: 6179 RVA: 0x000A4B49 File Offset: 0x000A2D49
	public void GUIRotateRightContextual()
	{
		this.Rotate(false);
	}

	// Token: 0x06001824 RID: 6180 RVA: 0x000A4B54 File Offset: 0x000A2D54
	public void GUIStatesNextContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject)
		{
			return;
		}
		foreach (GameObject gameObject in PlayerScript.PointerScript.GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				NetworkPhysicsObject component = gameObject.GetComponent<NetworkPhysicsObject>();
				if (component && component.HasStates())
				{
					component.NextState();
				}
			}
		}
	}

	// Token: 0x06001825 RID: 6181 RVA: 0x000A4BF0 File Offset: 0x000A2DF0
	public void GUIPDFPopoutContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject)
		{
			return;
		}
		foreach (GameObject gameObject in PlayerScript.PointerScript.GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				NetworkPhysicsObject component = gameObject.GetComponent<NetworkPhysicsObject>();
				if (component && component.customPDF)
				{
					component.customPDF.PopoutToScreen();
					PlayerScript.PointerScript.ResetInfoObject();
					break;
				}
			}
		}
	}

	// Token: 0x06001826 RID: 6182 RVA: 0x000A4CA0 File Offset: 0x000A2EA0
	public void GUIPDFNextPage()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject)
		{
			return;
		}
		foreach (GameObject gameObject in PlayerScript.PointerScript.GetSelectedObjects(-1, true, false))
		{
			if (gameObject)
			{
				NetworkPhysicsObject component = gameObject.GetComponent<NetworkPhysicsObject>();
				if (component && component.customPDF)
				{
					component.customPDF.NextPage();
				}
			}
		}
	}

	// Token: 0x06001827 RID: 6183 RVA: 0x000A4D44 File Offset: 0x000A2F44
	public void GUILuaEditorContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject || !NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
		{
			return;
		}
		LuaGlobalScriptManager.Instance.PushNewLuaObject(pointer.GetComponent<Pointer>().InfoObject);
		PlayerScript.PointerScript.ResetInfoObject();
	}

	// Token: 0x06001828 RID: 6184 RVA: 0x000A4DA0 File Offset: 0x000A2FA0
	public void GUICreateStatesContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject || !NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
		{
			return;
		}
		pointer.GetComponent<Pointer>().InfoObject.GetComponent<NetworkPhysicsObject>().CreateStates(pointer.GetComponent<Pointer>().ID);
		PlayerScript.PointerScript.ResetInfoObject();
	}

	// Token: 0x06001829 RID: 6185 RVA: 0x000A4E08 File Offset: 0x000A3008
	public void GUICloneContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject || !NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
		{
			return;
		}
		PlayerScript.PointerScript.StartCloning();
	}

	// Token: 0x0600182A RID: 6186 RVA: 0x000A4E50 File Offset: 0x000A3050
	public void GUICopyContextual()
	{
		GameObject pointer = PlayerScript.Pointer;
		if (!pointer || !pointer.GetComponent<Pointer>().InfoObject || !NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
		{
			return;
		}
		PlayerScript.PointerScript.Copy(-1, false, false);
	}

	// Token: 0x0600182B RID: 6187 RVA: 0x000A4E98 File Offset: 0x000A3098
	public void GUIPasteContextual()
	{
		if (!PlayerScript.Pointer || !NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
		{
			return;
		}
		PlayerScript.PointerScript.Paste(PlayerScript.PointerScript.StartGlobalPointerPos, false, true, true);
	}

	// Token: 0x0600182C RID: 6188 RVA: 0x000A4ECC File Offset: 0x000A30CC
	public void GUIDeleteContextual()
	{
		Pointer pointer = PlayerScript.Pointer ? PlayerScript.Pointer.GetComponent<Pointer>() : null;
		if (!pointer || !pointer.InfoObject)
		{
			return;
		}
		pointer.Delete(-1);
	}

	// Token: 0x0600182D RID: 6189 RVA: 0x000A4F10 File Offset: 0x000A3110
	public void DeleteAllTextObjects()
	{
		UIDialog.Show("Delete all 3D text objects?", "Yes", "No", delegate()
		{
			base.networkView.RPC(RPCTarget.All, new Action(this.RPCDeleteAllTextObjects));
		}, null);
	}

	// Token: 0x0600182E RID: 6190 RVA: 0x000A4F34 File Offset: 0x000A3134
	[Remote("Permissions/Notes")]
	private void RPCDeleteAllTextObjects()
	{
		Chat.NotifyFromNetworkSender("has deleted all 3D text objects");
		if (Network.isServer)
		{
			List<NetworkPhysicsObject> allNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.AllNPOs;
			for (int i = allNPOs.Count - 1; i >= 0; i--)
			{
				NetworkPhysicsObject networkPhysicsObject = allNPOs[i];
				if (networkPhysicsObject.textTool)
				{
					NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(networkPhysicsObject.gameObject);
				}
			}
		}
	}

	// Token: 0x0600182F RID: 6191 RVA: 0x000A4F95 File Offset: 0x000A3195
	public bool InventoryActive()
	{
		return this.GUIInventory && this.GUIInventory.gameObject.activeInHierarchy;
	}

	// Token: 0x06001830 RID: 6192 RVA: 0x000A4FB6 File Offset: 0x000A31B6
	public void EnableGUIEndHandRevealButton(Colour playerColour)
	{
		this.GUIEndHandRevealButton.transform.GetChild(0).GetComponent<UISprite>().color = playerColour;
		this.GUIEndHandRevealButton.SetActive(true);
	}

	// Token: 0x06001831 RID: 6193 RVA: 0x000A4FE5 File Offset: 0x000A31E5
	public void GUIEndHandZoneReveal()
	{
		this.GUIEndHandRevealButton.SetActive(false);
		while (this.handZoneToReveal)
		{
			this.handZoneToReveal.EndRevealHand();
			this.handZoneToReveal = HandZone.GetHandZoneBeingRevealedForColour(this.playerLabel);
		}
	}

	// Token: 0x06001832 RID: 6194 RVA: 0x000A5020 File Offset: 0x000A3220
	public void EnableGUIEndHandRevealButtonIfApplicableForColor(string playerLabel)
	{
		this.handZoneToReveal = HandZone.GetHandZoneBeingRevealedForColour(playerLabel);
		if (this.handZoneToReveal)
		{
			if (this.handZoneToReveal.RevealingToAllPlayers)
			{
				this.GUIEndHandRevealButton.transform.GetChild(0).GetComponent<UISprite>().color = Colour.Black;
			}
			else
			{
				this.GUIEndHandRevealButton.transform.GetChild(0).GetComponent<UISprite>().color = Colour.ColourFromLabel(playerLabel);
			}
			this.GUIEndHandRevealButton.SetActive(true);
		}
	}

	// Token: 0x04000D80 RID: 3456
	private string playerName = "NotConnectedToSteam";

	// Token: 0x04000D81 RID: 3457
	[Header("Version")]
	public TTSVersion Version;

	// Token: 0x04000D82 RID: 3458
	private string _VersionNumber;

	// Token: 0x04000D83 RID: 3459
	private string _VersionHotfix;

	// Token: 0x04000D84 RID: 3460
	[Header("VR")]
	public VRControllerManager VRControllerManager;

	// Token: 0x04000D85 RID: 3461
	[Header("NGUI")]
	public UIInventory GUIInventory;

	// Token: 0x04000D86 RID: 3462
	public GameObject GUIUIRoot;

	// Token: 0x04000D87 RID: 3463
	public GameObject GUIUIRoot3D;

	// Token: 0x04000D88 RID: 3464
	public GameObject GUIConnected;

	// Token: 0x04000D89 RID: 3465
	public GameObject GUIDisconnected;

	// Token: 0x04000D8A RID: 3466
	public GameObject GUIGames;

	// Token: 0x04000D8B RID: 3467
	public GameObject GUIObjects;

	// Token: 0x04000D8C RID: 3468
	public GameObject GUITables;

	// Token: 0x04000D8D RID: 3469
	public GameObject GUIBackgrounds;

	// Token: 0x04000D8E RID: 3470
	public GameObject GUIComponents;

	// Token: 0x04000D8F RID: 3471
	public GameObject GUIComponentTags;

	// Token: 0x04000D90 RID: 3472
	public GameObject GUISavedObjects;

	// Token: 0x04000D91 RID: 3473
	public GameObject GUITurns;

	// Token: 0x04000D92 RID: 3474
	public GameObject GUIServerOptions;

	// Token: 0x04000D93 RID: 3475
	public GameObject GUIPermissions;

	// Token: 0x04000D94 RID: 3476
	public GameObject GUIInfoOptions;

	// Token: 0x04000D95 RID: 3477
	public GameObject GUIPhysicsOptions;

	// Token: 0x04000D96 RID: 3478
	public GameObject GUIControlScheme;

	// Token: 0x04000D97 RID: 3479
	public GameObject GUIControlSchemeController;

	// Token: 0x04000D98 RID: 3480
	public GameObject GUIControlSchemeControllerSteam;

	// Token: 0x04000D99 RID: 3481
	public GameObject GUIConfiguration;

	// Token: 0x04000D9A RID: 3482
	public GameObject GUIWorkshopUpload;

	// Token: 0x04000D9B RID: 3483
	public GameObject GUICloud;

	// Token: 0x04000D9C RID: 3484
	public GameObject GUIColor;

	// Token: 0x04000D9D RID: 3485
	public GameObject GUIEndTurn;

	// Token: 0x04000D9E RID: 3486
	public UITopBar GUITopBar;

	// Token: 0x04000D9F RID: 3487
	public GameObject GUINotepad;

	// Token: 0x04000DA0 RID: 3488
	public GameObject GUIMenu;

	// Token: 0x04000DA1 RID: 3489
	public GameObject GUIDownload;

	// Token: 0x04000DA2 RID: 3490
	public GameObject GUIGrid;

	// Token: 0x04000DA3 RID: 3491
	public GameObject GUILighting;

	// Token: 0x04000DA4 RID: 3492
	public GameObject GUIRules;

	// Token: 0x04000DA5 RID: 3493
	public GameObject GUIMultiplayer;

	// Token: 0x04000DA6 RID: 3494
	public GameObject GUIMainMenu;

	// Token: 0x04000DA7 RID: 3495
	public GameObject GUITutorial;

	// Token: 0x04000DA8 RID: 3496
	public GameObject GUIFogColor;

	// Token: 0x04000DA9 RID: 3497
	public GameObject GUIFogOfWar;

	// Token: 0x04000DAA RID: 3498
	public GameObject GUILayoutZone;

	// Token: 0x04000DAB RID: 3499
	public UILayoutZoneSettings GUILayoutZoneSettings;

	// Token: 0x04000DAC RID: 3500
	public GameObject GUIRandomizeZone;

	// Token: 0x04000DAD RID: 3501
	public GameObject GUIHandColor;

	// Token: 0x04000DAE RID: 3502
	public GameObject GUIServerBrowser;

	// Token: 0x04000DAF RID: 3503
	public GameObject GUIConnecting;

	// Token: 0x04000DB0 RID: 3504
	public GameObject GUIPassword;

	// Token: 0x04000DB1 RID: 3505
	public GameObject GUIServerBrowserLoading;

	// Token: 0x04000DB2 RID: 3506
	public GameObject GUITabletWindow;

	// Token: 0x04000DB3 RID: 3507
	public GameObject GUIJoint;

	// Token: 0x04000DB4 RID: 3508
	public GameObject GUITeams;

	// Token: 0x04000DB5 RID: 3509
	public GameObject GUIBlindfold;

	// Token: 0x04000DB6 RID: 3510
	public GameObject GUIPictureInPicture;

	// Token: 0x04000DB7 RID: 3511
	public GameObject GUIUserDefinedGlobalTemplate;

	// Token: 0x04000DB8 RID: 3512
	public GameObject GUIUserDefinedGlobalSpacer;

	// Token: 0x04000DB9 RID: 3513
	public NGUIColorPicker GUIColorPickerScript;

	// Token: 0x04000DBA RID: 3514
	public UILuaNotepad GUILuaNotepad;

	// Token: 0x04000DBB RID: 3515
	public UITagEditor GUITagEditor;

	// Token: 0x04000DBC RID: 3516
	public UIStartScripts GUIStartScripts;

	// Token: 0x04000DBD RID: 3517
	public UITransform GUITransform;

	// Token: 0x04000DBE RID: 3518
	public UIGridMenuRotationValue GUIRotationValue;

	// Token: 0x04000DBF RID: 3519
	public GameObject GUIDecals;

	// Token: 0x04000DC0 RID: 3520
	public UIGridMenuUIAssets GUIUIAssets;

	// Token: 0x04000DC1 RID: 3521
	public GameObject GUIHands;

	// Token: 0x04000DC2 RID: 3522
	public GameObject GUISnapDotPanel;

	// Token: 0x04000DC3 RID: 3523
	public UIInput[] GUIPasswordInputs;

	// Token: 0x04000DC4 RID: 3524
	[Header("Confirm")]
	public GameObject GUIConfirmWorkshop;

	// Token: 0x04000DC5 RID: 3525
	public GameObject GUIConfirmUnsubscribe;

	// Token: 0x04000DC6 RID: 3526
	public GameObject GUIConfirmHotseat;

	// Token: 0x04000DC7 RID: 3527
	public GameObject GUIConfirmRandomize;

	// Token: 0x04000DC8 RID: 3528
	[Header("Prefabs")]
	public GameObject GUIButtonWorkshop;

	// Token: 0x04000DC9 RID: 3529
	public GameObject GUIButtonCustom;

	// Token: 0x04000DCA RID: 3530
	public GameObject GUIButtonCustomRoot;

	// Token: 0x04000DCB RID: 3531
	public GameObject GUIButtonBlocked;

	// Token: 0x04000DCC RID: 3532
	public GameObject GUIButtonServer;

	// Token: 0x04000DCD RID: 3533
	public GameObject GUISnapDot;

	// Token: 0x04000DCE RID: 3534
	public GameObject GUITooltipPrefab;

	// Token: 0x04000DCF RID: 3535
	[Header("Grids")]
	public GameObject GUIWorkshopGrid;

	// Token: 0x04000DD0 RID: 3536
	public GameObject GUIBlockedGrid;

	// Token: 0x04000DD1 RID: 3537
	public GameObject GUILoadGrid;

	// Token: 0x04000DD2 RID: 3538
	public GameObject GUISaveGrid;

	// Token: 0x04000DD3 RID: 3539
	public GameObject GUIChestSaveGrid;

	// Token: 0x04000DD4 RID: 3540
	public GameObject GUIBrowserGrid;

	// Token: 0x04000DD5 RID: 3541
	[Header("Contextual/Toggles")]
	public GameObject GUIContextualLockBool;

	// Token: 0x04000DD6 RID: 3542
	public GameObject GUIContextualDragSelectableBool;

	// Token: 0x04000DD7 RID: 3543
	public GameObject GUIContextualGridBool;

	// Token: 0x04000DD8 RID: 3544
	public GameObject GUIContextualSnapBool;

	// Token: 0x04000DD9 RID: 3545
	public GameObject GUIContextualDestroyableBool;

	// Token: 0x04000DDA RID: 3546
	public GameObject GUIContextualAutoraiseBool;

	// Token: 0x04000DDB RID: 3547
	public GameObject GUIContextualStickyBool;

	// Token: 0x04000DDC RID: 3548
	public GameObject GUIContextualRevealActive;

	// Token: 0x04000DDD RID: 3549
	public GameObject GUIContextualTooltipBool;

	// Token: 0x04000DDE RID: 3550
	public GameObject GUIContextualGridProjectionBool;

	// Token: 0x04000DDF RID: 3551
	public GameObject GUIContextualHandsBool;

	// Token: 0x04000DE0 RID: 3552
	public GameObject GUIContextualHideFaceDownBool;

	// Token: 0x04000DE1 RID: 3553
	public GameObject GUIContextualIgnoreFogOfWarBool;

	// Token: 0x04000DE2 RID: 3554
	public GameObject GUIContextualMeasureMovementBool;

	// Token: 0x04000DE3 RID: 3555
	[Header("Contextual")]
	public GameObject GUIContextualMenu;

	// Token: 0x04000DE4 RID: 3556
	public GameObject GUIContextualJoint;

	// Token: 0x04000DE5 RID: 3557
	public GameObject GUIContextualPhysics;

	// Token: 0x04000DE6 RID: 3558
	public GameObject GUIDropDown;

	// Token: 0x04000DE7 RID: 3559
	public GameObject GUIContextualDealColor;

	// Token: 0x04000DE8 RID: 3560
	public GameObject GUIContextualGrid;

	// Token: 0x04000DE9 RID: 3561
	public GameObject GUIContextualComponentTags;

	// Token: 0x04000DEA RID: 3562
	public GameObject GUIContextualSaveToChest;

	// Token: 0x04000DEB RID: 3563
	public GameObject GUIContextualColorTint;

	// Token: 0x04000DEC RID: 3564
	public GameObject GUIContextualToggles;

	// Token: 0x04000DED RID: 3565
	public GameObject GUIContextualMaterial;

	// Token: 0x04000DEE RID: 3566
	public GameObject GUIContextualWoodBool;

	// Token: 0x04000DEF RID: 3567
	public GameObject GUIContextualPlasticBool;

	// Token: 0x04000DF0 RID: 3568
	public GameObject GUIContextualMetalBool;

	// Token: 0x04000DF1 RID: 3569
	public GameObject GUIContextualGoldBool;

	// Token: 0x04000DF2 RID: 3570
	public GameObject GUIContextualOrder;

	// Token: 0x04000DF3 RID: 3571
	public UIToggle GUIContextualOrderLIFO;

	// Token: 0x04000DF4 RID: 3572
	public UIToggle GUIContextualOrderFIFO;

	// Token: 0x04000DF5 RID: 3573
	public UIToggle GUIContextualOrderRandom;

	// Token: 0x04000DF6 RID: 3574
	public GameObject GUIContextualName;

	// Token: 0x04000DF7 RID: 3575
	public GameObject GUIContextualDesc;

	// Token: 0x04000DF8 RID: 3576
	public GameObject GUIContextualValue;

	// Token: 0x04000DF9 RID: 3577
	public GameObject GUIContextualGMNotes;

	// Token: 0x04000DFA RID: 3578
	public GameObject GUIContextualRoll;

	// Token: 0x04000DFB RID: 3579
	public GameObject GUIContextualSetNumber;

	// Token: 0x04000DFC RID: 3580
	public GameObject GUIContextualSetState;

	// Token: 0x04000DFD RID: 3581
	public GameObject GUIContextualSetPDFPage;

	// Token: 0x04000DFE RID: 3582
	public GameObject GUIContextualPopoutPDFPage;

	// Token: 0x04000DFF RID: 3583
	public GameObject GUIContextualSplit;

	// Token: 0x04000E00 RID: 3584
	public GameObject GUIContextualDeckDeal;

	// Token: 0x04000E01 RID: 3585
	public GameObject GUIContextualDeckCut;

	// Token: 0x04000E02 RID: 3586
	public GameObject GUIContextualDeckReset;

	// Token: 0x04000E03 RID: 3587
	public GameObject GUIContextualDraw;

	// Token: 0x04000E04 RID: 3588
	public GameObject GUIContextualHandReveal;

	// Token: 0x04000E05 RID: 3589
	[NonSerialized]
	public HandZone handZoneToReveal;

	// Token: 0x04000E06 RID: 3590
	public GameObject GUIEndHandRevealButton;

	// Token: 0x04000E07 RID: 3591
	public GameObject GUIContextualHandRevealColors;

	// Token: 0x04000E08 RID: 3592
	public GameObject GUIContextualSearch;

	// Token: 0x04000E09 RID: 3593
	public GameObject GUIContextualStopSearch;

	// Token: 0x04000E0A RID: 3594
	public GameObject GUIContextualCheck;

	// Token: 0x04000E0B RID: 3595
	public GameObject GUIContextualSolve;

	// Token: 0x04000E0C RID: 3596
	public GameObject GUIContextualShuffle;

	// Token: 0x04000E0D RID: 3597
	public GameObject GUIContextualSpread;

	// Token: 0x04000E0E RID: 3598
	public GameObject GUIContextualLayout;

	// Token: 0x04000E0F RID: 3599
	public GameObject GUIContextualClockCurrentTime;

	// Token: 0x04000E10 RID: 3600
	public GameObject GUIContextualClockStopwatch;

	// Token: 0x04000E11 RID: 3601
	public GameObject GUIContextualClockTimer;

	// Token: 0x04000E12 RID: 3602
	public GameObject GUIContextualClockTimerInput;

	// Token: 0x04000E13 RID: 3603
	public GameObject GUIContextualCustom;

	// Token: 0x04000E14 RID: 3604
	public GameObject GUIContextualFogOfWarRevealPanel;

	// Token: 0x04000E15 RID: 3605
	public GameObject GUIContextualFogOfWarRevealColor;

	// Token: 0x04000E16 RID: 3606
	public UISprite GUIContextualFogOfWarRevealColorSprite;

	// Token: 0x04000E17 RID: 3607
	public GameObject GUIContextualFogOfWarRevealRange;

	// Token: 0x04000E18 RID: 3608
	public GameObject GUIContextualFogOfWarRevealHeight;

	// Token: 0x04000E19 RID: 3609
	public GameObject GUIContextualFogOfWarRevealFoV;

	// Token: 0x04000E1A RID: 3610
	public GameObject GUIContextualFogOfWarRevealFoVOffset;

	// Token: 0x04000E1B RID: 3611
	public GameObject GUIContextualFogOfWarRevealOutline;

	// Token: 0x04000E1C RID: 3612
	public GameObject GUIContextualRPGMode;

	// Token: 0x04000E1D RID: 3613
	public GameObject GUIContextualRPGAttack;

	// Token: 0x04000E1E RID: 3614
	public GameObject GUIContextualRPGDeath;

	// Token: 0x04000E1F RID: 3615
	public GameObject GUIContextualGroup;

	// Token: 0x04000E20 RID: 3616
	public GameObject GUIContextualScripting;

	// Token: 0x04000E21 RID: 3617
	public GameObject GUIContextualGUID;

	// Token: 0x04000E22 RID: 3618
	public GameObject GUIContextualCreateStates;

	// Token: 0x04000E23 RID: 3619
	public GameObject GUIContextualDelete;

	// Token: 0x04000E24 RID: 3620
	public GameObject GUIContextualClone;

	// Token: 0x04000E25 RID: 3621
	public GameObject GUIContextualCopy;

	// Token: 0x04000E26 RID: 3622
	public GameObject GUIContextualPaste;

	// Token: 0x04000E27 RID: 3623
	public GameObject GUIContextualLoopingEffect;

	// Token: 0x04000E28 RID: 3624
	public GameObject GUIContextualTriggerEffect;

	// Token: 0x04000E29 RID: 3625
	public GameObject GUIContextualFlip;

	// Token: 0x04000E2A RID: 3626
	public GameObject GUIContextualRotateMenu;

	// Token: 0x04000E2B RID: 3627
	public GameObject GUIContextualScaleMenu;

	// Token: 0x04000E2C RID: 3628
	public GameObject GUIContextualUserDefinedTemplate;

	// Token: 0x04000E2D RID: 3629
	public GameObject GUIContextualUserDefinedSpacer;

	// Token: 0x04000E2E RID: 3630
	public GameObject GUIContextualMovementSpacer;

	// Token: 0x04000E2F RID: 3631
	public GameObject GUIContextualGlobalMenu;

	// Token: 0x04000E30 RID: 3632
	public GameObject GUIGridSyncAxis;

	// Token: 0x04000E31 RID: 3633
	public UIInput GUINotepadInput;

	// Token: 0x04000E32 RID: 3634
	public SearchIcons deckIconsPrefab;

	// Token: 0x04000E33 RID: 3635
	[Header("Game Modes")]
	public string GameBackgammon = "Backgammon";

	// Token: 0x04000E34 RID: 3636
	public string GameCards = "Cards";

	// Token: 0x04000E35 RID: 3637
	public string GameCardBots = "CardBots";

	// Token: 0x04000E36 RID: 3638
	public string GameCheckers = "Checkers";

	// Token: 0x04000E37 RID: 3639
	public string GameChess = "Chess";

	// Token: 0x04000E38 RID: 3640
	public string GameChineseCheckers = "Chinese Checkers";

	// Token: 0x04000E39 RID: 3641
	public string GameCustom = "Custom";

	// Token: 0x04000E3A RID: 3642
	public string GameDice = "Dice";

	// Token: 0x04000E3B RID: 3643
	public string GameDominoes = "Dominoes";

	// Token: 0x04000E3C RID: 3644
	public string GameGo = "Go";

	// Token: 0x04000E3D RID: 3645
	public string GameMahjong = "Mahjong";

	// Token: 0x04000E3E RID: 3646
	public string GamePachisi = "Pachisi";

	// Token: 0x04000E3F RID: 3647
	public string GamePiecepack = "Piecepack";

	// Token: 0x04000E40 RID: 3648
	public string GamePoker = "Poker";

	// Token: 0x04000E41 RID: 3649
	public string GameReversi = "Reversi";

	// Token: 0x04000E42 RID: 3650
	public string GameRPG = "RPG";

	// Token: 0x04000E43 RID: 3651
	public string GameSandbox = "Sandbox";

	// Token: 0x04000E44 RID: 3652
	public string GameSolitaire = "Solitaire";

	// Token: 0x04000E45 RID: 3653
	public string GameNone = "None";

	// Token: 0x04000E46 RID: 3654
	public string GameFelt = "Felt";

	// Token: 0x04000E47 RID: 3655
	public string GameTutorial = "Tutorial";

	// Token: 0x04000E48 RID: 3656
	public string GameJigsaw = "Jigsaw";

	// Token: 0x04000E49 RID: 3657
	public string GameTool = "Tools";

	// Token: 0x04000E4A RID: 3658
	[Header("Audio")]
	public AudioClip ChatSound;

	// Token: 0x04000E4B RID: 3659
	public AudioClip YourTurnSound;

	// Token: 0x04000E4C RID: 3660
	public AudioClip ButtonSound;

	// Token: 0x04000E4D RID: 3661
	[Header("Pointers")]
	public GameObject WhitePrefab;

	// Token: 0x04000E4E RID: 3662
	public GameObject BrownPrefab;

	// Token: 0x04000E4F RID: 3663
	public GameObject RedPrefab;

	// Token: 0x04000E50 RID: 3664
	public GameObject OrangePrefab;

	// Token: 0x04000E51 RID: 3665
	public GameObject YellowPrefab;

	// Token: 0x04000E52 RID: 3666
	public GameObject GreenPrefab;

	// Token: 0x04000E53 RID: 3667
	public GameObject TealPrefab;

	// Token: 0x04000E54 RID: 3668
	public GameObject BluePrefab;

	// Token: 0x04000E55 RID: 3669
	public GameObject PurplePrefab;

	// Token: 0x04000E56 RID: 3670
	public GameObject PinkPrefab;

	// Token: 0x04000E57 RID: 3671
	public GameObject BlackPrefab;

	// Token: 0x04000E58 RID: 3672
	public Texture2D WhiteCursorTexture;

	// Token: 0x04000E59 RID: 3673
	public Texture2D BrownCursorTexture;

	// Token: 0x04000E5A RID: 3674
	public Texture2D RedCursorTexture;

	// Token: 0x04000E5B RID: 3675
	public Texture2D OrangeCursorTexture;

	// Token: 0x04000E5C RID: 3676
	public Texture2D YellowCursorTexture;

	// Token: 0x04000E5D RID: 3677
	public Texture2D GreenCursorTexture;

	// Token: 0x04000E5E RID: 3678
	public Texture2D TealCursorTexture;

	// Token: 0x04000E5F RID: 3679
	public Texture2D BlueCursorTexture;

	// Token: 0x04000E60 RID: 3680
	public Texture2D PurpleCursorTexture;

	// Token: 0x04000E61 RID: 3681
	public Texture2D PinkCursorTexture;

	// Token: 0x04000E62 RID: 3682
	public Texture2D BlackCursorTexture;

	// Token: 0x04000E63 RID: 3683
	public Texture2D GreyCursorTexture;

	// Token: 0x04000E64 RID: 3684
	public static Vector2 HardwareCursorOffest = new Vector2(6f, 3f);

	// Token: 0x04000E65 RID: 3685
	private string playerLabel = "";

	// Token: 0x04000E66 RID: 3686
	public Colour playerColour = Colour.UnityWhite;

	// Token: 0x04000E67 RID: 3687
	public bool bNeedToPickColour = true;

	// Token: 0x04000E68 RID: 3688
	private bool _bCanFlipTable;

	// Token: 0x04000E69 RID: 3689
	public string notepadstring = "";

	// Token: 0x04000E6A RID: 3690
	public string prevnotepadstring = "";

	// Token: 0x04000E6B RID: 3691
	private float notepadtimeholder;

	// Token: 0x04000E6C RID: 3692
	public static bool bHideGUI = false;

	// Token: 0x04000E6D RID: 3693
	public Vector3 SpawnPos = new Vector3(0f, 4f, 0f);

	// Token: 0x04000E6E RID: 3694
	public bool bAutoRunScripts = true;

	// Token: 0x04000E6F RID: 3695
	public string ErrorMessage = "";

	// Token: 0x04000E70 RID: 3696
	private Vector2 SaveScroll;

	// Token: 0x04000E71 RID: 3697
	private Vector2 LoadScroll;

	// Token: 0x04000E72 RID: 3698
	public bool bHotseat;

	// Token: 0x04000E73 RID: 3699
	private int NumHotseat;

	// Token: 0x04000E74 RID: 3700
	private bool askedForGame;

	// Token: 0x04000E75 RID: 3701
	[NonSerialized]
	public int CurrentHotseat;

	// Token: 0x04000E76 RID: 3702
	[NonSerialized]
	public bool HotseatBetweenTurns;

	// Token: 0x04000E77 RID: 3703
	public string HotseatPreviousColor;

	// Token: 0x04000E78 RID: 3704
	private const int ResetInt = 25;

	// Token: 0x04000E79 RID: 3705
	private const int ResetRawCache = 24;

	// Token: 0x04000E7A RID: 3706
	private readonly List<GameObject> escapeMenus = new List<GameObject>();

	// Token: 0x04000E7C RID: 3708
	public static bool bProxy = false;

	// Token: 0x04000E7D RID: 3709
	private int playerIDToSet = -1;

	// Token: 0x04000E7E RID: 3710
	public static bool bCleanApplicationQuit = false;

	// Token: 0x04000E7F RID: 3711
	private List<LayoutZone> zonesToLayout = new List<LayoutZone>();

	// Token: 0x020006AE RID: 1710
	public enum EscapeMenuActivation
	{
		// Token: 0x040028E9 RID: 10473
		AUTO,
		// Token: 0x040028EA RID: 10474
		FORCED,
		// Token: 0x040028EB RID: 10475
		NEVER
	}
}
