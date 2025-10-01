using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using NewNet;
using UnityEngine;

// Token: 0x020001A3 RID: 419
public class LuaScript : NetworkBehavior
{
	// Token: 0x1700037A RID: 890
	// (get) Token: 0x060014C7 RID: 5319 RVA: 0x0008885A File Offset: 0x00086A5A
	// (set) Token: 0x060014C8 RID: 5320 RVA: 0x00088862 File Offset: 0x00086A62
	[MoonSharpHidden]
	public XmlUIScript XmlUI { get; protected set; }

	// Token: 0x1700037B RID: 891
	// (get) Token: 0x060014C9 RID: 5321 RVA: 0x0008886B File Offset: 0x00086A6B
	public virtual string guid
	{
		get
		{
			return "-1";
		}
	}

	// Token: 0x060014CA RID: 5322 RVA: 0x000025B8 File Offset: 0x000007B8
	protected virtual void Awake()
	{
	}

	// Token: 0x060014CB RID: 5323 RVA: 0x00088874 File Offset: 0x00086A74
	protected virtual void Start()
	{
		if (this.IsGlobalDummyObject)
		{
			return;
		}
		this.startHappened = true;
		EventManager.OnLoadingComplete += this.OnLoad;
		EventManager.OnGameSave += this.OnSave;
		EventManager.OnLuaObjectSpawn += this.OnObjectSpawn;
		EventManager.OnLuaObjectDestroy += this.OnObjectDestroy;
		EventManager.OnObjectHover += this.OnObjectHover;
		EventManager.OnObjectPickUp += this.OnObjectPickUp;
		EventManager.OnObjectDrop += this.OnObjectDrop;
		EventManager.OnObjectEnterContainer += this.OnObjectEnterContainer;
		EventManager.OnObjectLeaveContainer += this.OnObjectLeaveContainer;
		EventManager.OnScriptingButtonDown += this.OnScriptingButtonDown;
		EventManager.OnScriptingButtonUp += this.OnScriptingButtonUp;
		EventManager.OnObjectTriggerEffect += this.OnObjectTriggerEffect;
		EventManager.OnObjectLoopingEffect += this.OnObjectLoopingEffect;
		EventManager.OnObjectSearchStart += this.OnObjectSearchStart;
		EventManager.OnObjectSearchEnd += this.OnObjectSearchEnd;
		EventManager.OnObjectPeek += this.OnObjectPeek;
		EventManager.OnObjectRandomize += this.OnObjectRandomize;
		EventManager.OnObjectFlick += this.OnObjectFlick;
		EventManager.OnObjectPageChange += this.OnObjectPageChange;
		EventManager.OnObjectRotate += this.OnObjectRotate;
		EventManager.OnZoneAdd += this.OnObjectEnterZone;
		EventManager.OnZoneRemove += this.OnObjectLeaveZone;
		EventManager.OnChat += this.OnChat;
		EventManager.OnBlindfold += this.OnBlindfold;
		EventManager.OnPlayerAction += this.OnPlayerAction;
		EventManager.OnPlayersAdd += this.OnPlayersConnect;
		EventManager.OnPlayersRemove += this.OnPlayersDisconnect;
		EventManager.OnPlayerPing += this.OnPlayerPing;
		EventManager.OnPlayerTurnStart += this.OnPlayerTurnStart;
		EventManager.OnPlayerTurnEnd += this.OnPlayerTurnEnd;
		EventManager.OnChangePlayerColor += this.OnPlayerChangeColor;
		EventManager.OnChangePlayerTeam += this.OnPlayerChangeTeam;
		EventManager.OnPlayerChatTyping += this.OnPlayerChatTyping;
		EventManager.OnHandSelectModeEnd += this.OnHandSelectModeEnd;
		LuaGlobalScriptManager.OnExternalCustomMessage += this.OnExternalMessage;
	}

	// Token: 0x060014CC RID: 5324 RVA: 0x00088B04 File Offset: 0x00086D04
	protected virtual void OnDestroy()
	{
		if (this.IsGlobalDummyObject)
		{
			return;
		}
		if (!this.startHappened)
		{
			return;
		}
		EventManager.OnLoadingComplete -= this.OnLoad;
		EventManager.OnGameSave -= this.OnSave;
		EventManager.OnLuaObjectSpawn -= this.OnObjectSpawn;
		EventManager.OnLuaObjectDestroy -= this.OnObjectDestroy;
		EventManager.OnObjectHover -= this.OnObjectHover;
		EventManager.OnObjectPickUp -= this.OnObjectPickUp;
		EventManager.OnObjectDrop -= this.OnObjectDrop;
		EventManager.OnObjectEnterContainer -= this.OnObjectEnterContainer;
		EventManager.OnObjectLeaveContainer -= this.OnObjectLeaveContainer;
		EventManager.OnScriptingButtonDown -= this.OnScriptingButtonDown;
		EventManager.OnScriptingButtonUp -= this.OnScriptingButtonUp;
		EventManager.OnObjectTriggerEffect -= this.OnObjectTriggerEffect;
		EventManager.OnObjectLoopingEffect -= this.OnObjectLoopingEffect;
		EventManager.OnObjectSearchStart -= this.OnObjectSearchStart;
		EventManager.OnObjectSearchEnd -= this.OnObjectSearchEnd;
		EventManager.OnObjectPeek -= this.OnObjectPeek;
		EventManager.OnObjectRandomize -= this.OnObjectRandomize;
		EventManager.OnObjectFlick -= this.OnObjectFlick;
		EventManager.OnObjectPageChange -= this.OnObjectPageChange;
		EventManager.OnObjectRotate -= this.OnObjectRotate;
		EventManager.OnZoneAdd -= this.OnObjectEnterZone;
		EventManager.OnZoneRemove -= this.OnObjectLeaveZone;
		EventManager.OnChat -= this.OnChat;
		EventManager.OnBlindfold -= this.OnBlindfold;
		EventManager.OnPlayerAction -= this.OnPlayerAction;
		EventManager.OnPlayersAdd -= this.OnPlayersConnect;
		EventManager.OnPlayersRemove -= this.OnPlayersDisconnect;
		EventManager.OnPlayerPing -= this.OnPlayerPing;
		EventManager.OnPlayerTurnStart -= this.OnPlayerTurnStart;
		EventManager.OnPlayerTurnEnd -= this.OnPlayerTurnEnd;
		EventManager.OnChangePlayerColor -= this.OnPlayerChangeColor;
		EventManager.OnChangePlayerTeam -= this.OnPlayerChangeTeam;
		EventManager.OnPlayerChatTyping -= this.OnPlayerChatTyping;
		EventManager.OnHandSelectModeEnd -= this.OnHandSelectModeEnd;
		LuaGlobalScriptManager.OnExternalCustomMessage -= this.OnExternalMessage;
	}

	// Token: 0x060014CD RID: 5325 RVA: 0x00088D96 File Offset: 0x00086F96
	protected virtual void Update()
	{
		this.TryCall("onUpdate");
		this.TryCall("update");
	}

	// Token: 0x060014CE RID: 5326 RVA: 0x00088DB0 File Offset: 0x00086FB0
	protected virtual void FixedUpdate()
	{
		this.TryCall("onFixedUpdate");
		this.TryCall("fixedUpdate");
	}

	// Token: 0x060014CF RID: 5327 RVA: 0x00088DCA File Offset: 0x00086FCA
	[MoonSharpHidden]
	public void DoString()
	{
		if (string.IsNullOrEmpty(this.script_code))
		{
			return;
		}
		this.lua.DoString(this.script_code, null, null);
		if (this.loaded)
		{
			this.OnLoad();
		}
	}

	// Token: 0x060014D0 RID: 5328 RVA: 0x00088DFC File Offset: 0x00086FFC
	public virtual string GetScriptName()
	{
		return "Global";
	}

	// Token: 0x060014D1 RID: 5329 RVA: 0x00088E04 File Offset: 0x00087004
	[MoonSharpHidden]
	public static string ExceptionToMessage(Exception e)
	{
		InterpreterException ex;
		string text;
		if ((ex = (e as InterpreterException)) != null)
		{
			text = ex.DecoratedMessage;
		}
		else
		{
			text = e.Message;
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "<Unknown Error>";
		}
		return text;
	}

	// Token: 0x060014D2 RID: 5330 RVA: 0x00088E3C File Offset: 0x0008703C
	[MoonSharpHidden]
	public void LogError(Exception e)
	{
		string text = LuaScript.ExceptionToMessage(e);
		string text2 = string.Format("Error in Script ({0}): {1}", this.GetScriptName(), text);
		if (NetworkSingleton<Chat>.Instance)
		{
			Chat.LogError(text2, true);
		}
		LuaGlobalScriptManager.Instance.PushLuaErrorMessage(text, this.guid, text2);
	}

	// Token: 0x060014D3 RID: 5331 RVA: 0x00088E87 File Offset: 0x00087087
	[MoonSharpHidden]
	public void LogError(string FunctionName, Exception e, LuaScript owner = null)
	{
		Debug.LogException(e);
		this.LogError(FunctionName, LuaScript.ExceptionToMessage(e), owner);
	}

	// Token: 0x060014D4 RID: 5332 RVA: 0x00088EA0 File Offset: 0x000870A0
	[MoonSharpHidden]
	public void LogError(string FunctionName, string Error, LuaScript owner = null)
	{
		if (owner == null)
		{
			owner = this;
		}
		string text = string.Format("Error in Script ({0}) function <{1}>: {2}", owner.GetScriptName(), FunctionName, Error);
		if (NetworkSingleton<Chat>.Instance)
		{
			Chat.LogError(text, true);
		}
		LuaGlobalScriptManager.Instance.PushLuaErrorMessage(Error, owner.guid, text);
	}

	// Token: 0x060014D5 RID: 5333 RVA: 0x00088EF1 File Offset: 0x000870F1
	[MoonSharpHidden]
	public DynValue TryCall(string FunctionName)
	{
		if (this.CanCall(FunctionName, false))
		{
			return this.Call(FunctionName);
		}
		return null;
	}

	// Token: 0x060014D6 RID: 5334 RVA: 0x00088F08 File Offset: 0x00087108
	public DynValue Call(string FunctionName)
	{
		try
		{
			return this.lua.Call(this.lua.Globals[FunctionName]);
		}
		catch (Exception e)
		{
			this.LogError(FunctionName, e, null);
		}
		return null;
	}

	// Token: 0x060014D7 RID: 5335 RVA: 0x00088F54 File Offset: 0x00087154
	[MoonSharpHidden]
	public DynValue TryCall(string FunctionName, params object[] args)
	{
		if (this.CanCall(FunctionName, false))
		{
			return this.Call(FunctionName, args);
		}
		return null;
	}

	// Token: 0x060014D8 RID: 5336 RVA: 0x00088F6C File Offset: 0x0008716C
	public DynValue Call(string FunctionName, params object[] args)
	{
		try
		{
			return this.lua.Call(this.lua.Globals[FunctionName], args);
		}
		catch (Exception e)
		{
			this.LogError(FunctionName, e, null);
		}
		return null;
	}

	// Token: 0x060014D9 RID: 5337 RVA: 0x00088FB8 File Offset: 0x000871B8
	[MoonSharpHidden]
	public bool CanCall(string FunctionName, bool BeforeLoaded = false)
	{
		return !this.IsGlobalDummyObject && !this.BlockEvents && (BeforeLoaded || this.loaded) && this.lua != null && this.lua.Globals[FunctionName] != null;
	}

	// Token: 0x060014DA RID: 5338 RVA: 0x00088FF4 File Offset: 0x000871F4
	[MoonSharpHidden]
	public virtual void OnLoad()
	{
		this.loaded = true;
		if (this.CanCall("onLoad", false))
		{
			this.Call("onLoad", new object[]
			{
				this.script_state
			});
		}
		if (this.CanCall("onload", false))
		{
			this.Call("onload", new object[]
			{
				this.script_state
			});
		}
	}

	// Token: 0x060014DB RID: 5339 RVA: 0x0008905C File Offset: 0x0008725C
	[MoonSharpHidden]
	public virtual void OnPlayerChangeColor(Color newColor, int id)
	{
		if (this.CanCall("onPlayerChangeColor", false))
		{
			this.Call("onPlayerChangeColor", new object[]
			{
				Colour.LabelFromColour(newColor)
			});
		}
		if (this.CanCall("onPlayerChangedColor", false))
		{
			this.Call("onPlayerChangedColor", new object[]
			{
				Colour.LabelFromColour(newColor)
			});
		}
	}

	// Token: 0x060014DC RID: 5340 RVA: 0x000890C8 File Offset: 0x000872C8
	private void OnPlayerChangeTeam(bool join, int id)
	{
		PlayerState playerState = NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(id);
		if (this.CanCall("onPlayerChangeTeam", false))
		{
			this.Call("onPlayerChangeTeam", new object[]
			{
				playerState.stringColor,
				TeamScript.StringFromTeam(playerState.team)
			});
		}
	}

	// Token: 0x060014DD RID: 5341 RVA: 0x00089118 File Offset: 0x00087318
	private void OnPlayerChatTyping(PlayerState player, bool typing)
	{
		if (this.CanCall("onPlayerChatTyping", false))
		{
			this.Call("onPlayerChatTyping", new object[]
			{
				LuaGlobalScriptManager.Instance.GlobalPlayer.GetPlayer(player.id),
				typing
			});
		}
	}

	// Token: 0x060014DE RID: 5342 RVA: 0x00089168 File Offset: 0x00087368
	[MoonSharpHidden]
	public virtual void OnObjectEnterZone(NetworkPhysicsObject Zone, NetworkPhysicsObject ChangeObject)
	{
		if (Zone == null || ChangeObject == null)
		{
			return;
		}
		if (this.CanCall("onObjectEnterZone", false))
		{
			this.Call("onObjectEnterZone", new object[]
			{
				Zone.luaGameObjectScript,
				ChangeObject.luaGameObjectScript
			});
		}
		if (Zone.scriptingZone && this.CanCall("onObjectEnterScriptingZone", false))
		{
			this.Call("onObjectEnterScriptingZone", new object[]
			{
				Zone.luaGameObjectScript,
				ChangeObject.luaGameObjectScript
			});
		}
	}

	// Token: 0x060014DF RID: 5343 RVA: 0x000891FC File Offset: 0x000873FC
	[MoonSharpHidden]
	public virtual void OnObjectLeaveZone(NetworkPhysicsObject Zone, NetworkPhysicsObject ChangeObject)
	{
		if (Zone == null || ChangeObject == null)
		{
			return;
		}
		if (this.CanCall("onObjectLeaveZone", false))
		{
			this.Call("onObjectLeaveZone", new object[]
			{
				Zone.luaGameObjectScript,
				ChangeObject.luaGameObjectScript
			});
		}
		if (Zone.scriptingZone && this.CanCall("onObjectLeaveScriptingZone", false))
		{
			this.Call("onObjectLeaveScriptingZone", new object[]
			{
				Zone.luaGameObjectScript,
				ChangeObject.luaGameObjectScript
			});
		}
	}

	// Token: 0x060014E0 RID: 5344 RVA: 0x00089290 File Offset: 0x00087490
	[MoonSharpHidden]
	public virtual void OnObjectSpawn(LuaGameObjectScript LGOS)
	{
		if (LGOS == null)
		{
			return;
		}
		if (this.CanCall("onObjectSpawn", false))
		{
			this.Call("onObjectSpawn", new object[]
			{
				LGOS
			});
		}
		if (this.CanCall("onObjectSpawned", false))
		{
			this.Call("onObjectSpawned", new object[]
			{
				LGOS
			});
		}
	}

	// Token: 0x060014E1 RID: 5345 RVA: 0x000892F0 File Offset: 0x000874F0
	[MoonSharpHidden]
	public virtual void OnObjectDestroy(LuaGameObjectScript LGOS)
	{
		if (LGOS == null)
		{
			return;
		}
		if (this.CanCall("onObjectDestroy", false))
		{
			this.Call("onObjectDestroy", new object[]
			{
				LGOS
			});
		}
		if (this.CanCall("onObjectDestroyed", false))
		{
			this.Call("onObjectDestroyed", new object[]
			{
				LGOS
			});
		}
	}

	// Token: 0x060014E2 RID: 5346 RVA: 0x00089350 File Offset: 0x00087550
	[MoonSharpHidden]
	public virtual void OnPlayerAction(string playerColor, PlayerAction playerAction, List<LuaGameObjectScript> objects)
	{
		if (this.CanCall("onPlayerAction", true))
		{
			DynValue dynValue = this.TryCall("onPlayerAction", new object[]
			{
				LuaPlayer.GetHandPlayer(playerColor),
				playerAction,
				objects
			});
			if (dynValue != null && dynValue.Type == DataType.Boolean && !dynValue.Boolean)
			{
				EventManager.CancelPlayerAction();
			}
		}
	}

	// Token: 0x060014E3 RID: 5347 RVA: 0x000893AC File Offset: 0x000875AC
	[MoonSharpHidden]
	public virtual void OnPlayerPing(string playerColor, Vector3 position, LuaGameObjectScript LGOS)
	{
		if (this.CanCall("onPlayerPing", false))
		{
			this.Call("onPlayerPing", new object[]
			{
				LuaPlayer.GetHandPlayer(playerColor),
				position,
				LGOS
			});
		}
	}

	// Token: 0x060014E4 RID: 5348 RVA: 0x000893E4 File Offset: 0x000875E4
	[MoonSharpHidden]
	public virtual void OnPlayerTurnStart(string EndColor, string StartColor)
	{
		if (this.CanCall("onPlayerTurn", false))
		{
			this.Call("onPlayerTurn", new object[]
			{
				LuaPlayer.GetHandPlayer(StartColor),
				(EndColor != null) ? LuaPlayer.GetHandPlayer(EndColor) : null
			});
		}
		if (this.CanCall("onPlayerTurnStart", false))
		{
			this.Call("onPlayerTurnStart", new object[]
			{
				StartColor,
				EndColor
			});
		}
	}

	// Token: 0x060014E5 RID: 5349 RVA: 0x00089451 File Offset: 0x00087651
	[MoonSharpHidden]
	public virtual void OnPlayerTurnEnd(string EndColor, string StartColor)
	{
		if (this.CanCall("onPlayerTurnEnd", false))
		{
			this.Call("onPlayerTurnEnd", new object[]
			{
				EndColor,
				StartColor
			});
		}
	}

	// Token: 0x060014E6 RID: 5350 RVA: 0x0008947B File Offset: 0x0008767B
	[MoonSharpHidden]
	public virtual void OnGameRoundStart()
	{
		this.TryCall("onGameRoundStart");
	}

	// Token: 0x060014E7 RID: 5351 RVA: 0x00089489 File Offset: 0x00087689
	[MoonSharpHidden]
	public virtual void OnGameRoundEnd()
	{
		this.TryCall("onGameRoundEnd");
	}

	// Token: 0x060014E8 RID: 5352 RVA: 0x00089498 File Offset: 0x00087698
	[MoonSharpHidden]
	public virtual void OnObjectHover(GameObject HoverObject, Color Player)
	{
		if (this.CanCall("onObjectHover", false))
		{
			if (HoverObject)
			{
				this.Call("onObjectHover", new object[]
				{
					Colour.LabelFromColour(Player),
					HoverObject.GetComponent<LuaGameObjectScript>()
				});
				return;
			}
			this.Call("onObjectHover", new object[]
			{
				Colour.LabelFromColour(Player)
			});
		}
	}

	// Token: 0x060014E9 RID: 5353 RVA: 0x00089508 File Offset: 0x00087708
	[MoonSharpHidden]
	public virtual void OnObjectPickUp(NetworkPhysicsObject PickUpObject, PlayerState Player)
	{
		if (this.CanCall("onObjectPickUp", false))
		{
			this.Call("onObjectPickUp", new object[]
			{
				Player.stringColor,
				PickUpObject.luaGameObjectScript
			});
		}
		if (this.CanCall("onObjectPickedUp", false))
		{
			this.Call("onObjectPickedUp", new object[]
			{
				Player.stringColor,
				PickUpObject.luaGameObjectScript
			});
		}
	}

	// Token: 0x060014EA RID: 5354 RVA: 0x0008957C File Offset: 0x0008777C
	[MoonSharpHidden]
	public virtual void OnObjectDrop(NetworkPhysicsObject DroppedObject, PlayerState LastPlayerToHold)
	{
		if (this.CanCall("onObjectDrop", false))
		{
			this.Call("onObjectDrop", new object[]
			{
				LastPlayerToHold.stringColor,
				DroppedObject.luaGameObjectScript
			});
		}
		if (this.CanCall("onObjectDropped", false))
		{
			this.Call("onObjectDropped", new object[]
			{
				LastPlayerToHold.stringColor,
				DroppedObject.luaGameObjectScript
			});
		}
	}

	// Token: 0x060014EB RID: 5355 RVA: 0x000895ED File Offset: 0x000877ED
	[MoonSharpHidden]
	public virtual void OnObjectEnterContainer(NetworkPhysicsObject Container, NetworkPhysicsObject Object)
	{
		if (this.CanCall("onObjectEnterContainer", false))
		{
			this.Call("onObjectEnterContainer", new object[]
			{
				Container.luaGameObjectScript,
				Object.luaGameObjectScript
			});
		}
	}

	// Token: 0x060014EC RID: 5356 RVA: 0x00089621 File Offset: 0x00087821
	[MoonSharpHidden]
	public virtual void OnObjectLeaveContainer(NetworkPhysicsObject Container, NetworkPhysicsObject Object)
	{
		if (this.CanCall("onObjectLeaveContainer", false))
		{
			this.Call("onObjectLeaveContainer", new object[]
			{
				Container.luaGameObjectScript,
				Object.luaGameObjectScript
			});
		}
	}

	// Token: 0x060014ED RID: 5357 RVA: 0x00089655 File Offset: 0x00087855
	[MoonSharpHidden]
	public virtual void OnScriptingButtonDown(int index, string playerColor)
	{
		if (this.CanCall("onScriptingButtonDown", false))
		{
			this.Call("onScriptingButtonDown", new object[]
			{
				index,
				playerColor
			});
		}
	}

	// Token: 0x060014EE RID: 5358 RVA: 0x00089684 File Offset: 0x00087884
	[MoonSharpHidden]
	public virtual void OnScriptingButtonUp(int index, string playerColor)
	{
		if (this.CanCall("onScriptingButtonUp", false))
		{
			this.Call("onScriptingButtonUp", new object[]
			{
				index,
				playerColor
			});
		}
	}

	// Token: 0x060014EF RID: 5359 RVA: 0x000896B3 File Offset: 0x000878B3
	[MoonSharpHidden]
	public virtual void OnObjectTriggerEffect(NetworkPhysicsObject NPO, int index)
	{
		if (this.CanCall("onObjectTriggerEffect", false))
		{
			this.Call("onObjectTriggerEffect", new object[]
			{
				NPO.luaGameObjectScript,
				index
			});
		}
	}

	// Token: 0x060014F0 RID: 5360 RVA: 0x000896E7 File Offset: 0x000878E7
	[MoonSharpHidden]
	public virtual void OnObjectLoopingEffect(NetworkPhysicsObject NPO, int index)
	{
		if (this.CanCall("onObjectLoopingEffect", false))
		{
			this.Call("onObjectLoopingEffect", new object[]
			{
				NPO.luaGameObjectScript,
				index
			});
		}
	}

	// Token: 0x060014F1 RID: 5361 RVA: 0x0008971B File Offset: 0x0008791B
	[MoonSharpHidden]
	public virtual void OnObjectSearchStart(NetworkPhysicsObject NPO, string playerColor)
	{
		if (this.CanCall("onObjectSearchStart", false))
		{
			this.Call("onObjectSearchStart", new object[]
			{
				NPO.luaGameObjectScript,
				playerColor
			});
		}
	}

	// Token: 0x060014F2 RID: 5362 RVA: 0x0008974A File Offset: 0x0008794A
	[MoonSharpHidden]
	public virtual void OnObjectSearchEnd(NetworkPhysicsObject NPO, string playerColor)
	{
		if (this.CanCall("onObjectSearchEnd", false))
		{
			this.Call("onObjectSearchEnd", new object[]
			{
				NPO.luaGameObjectScript,
				playerColor
			});
		}
	}

	// Token: 0x060014F3 RID: 5363 RVA: 0x00089779 File Offset: 0x00087979
	[MoonSharpHidden]
	public virtual void OnObjectPeek(NetworkPhysicsObject NPO, string playerColor)
	{
		if (this.CanCall("onObjectPeek", false))
		{
			this.Call("onObjectPeek", new object[]
			{
				NPO.luaGameObjectScript,
				playerColor
			});
		}
	}

	// Token: 0x060014F4 RID: 5364 RVA: 0x000897A8 File Offset: 0x000879A8
	[MoonSharpHidden]
	public virtual void OnObjectRandomize(NetworkPhysicsObject NPO, string playerColor)
	{
		if (this.CanCall("onObjectRandomize", false))
		{
			this.Call("onObjectRandomize", new object[]
			{
				NPO.luaGameObjectScript,
				playerColor
			});
		}
	}

	// Token: 0x060014F5 RID: 5365 RVA: 0x000897D8 File Offset: 0x000879D8
	[MoonSharpHidden]
	public virtual void OnObjectRotate(NetworkPhysicsObject NPO, int spinIndex, int flipIndex, string playerColor, int previousSpinIndex, int previousFlipIndex)
	{
		if (this.CanCall("onObjectRotate", false))
		{
			this.Call("onObjectRotate", new object[]
			{
				NPO.luaGameObjectScript,
				spinIndex * 15,
				flipIndex * 15,
				playerColor,
				previousSpinIndex * 15,
				previousFlipIndex * 15
			});
		}
	}

	// Token: 0x060014F6 RID: 5366 RVA: 0x00089845 File Offset: 0x00087A45
	[MoonSharpHidden]
	public virtual void OnObjectFlick(NetworkPhysicsObject NPO, string playerColor, Vector3 force)
	{
		if (this.CanCall("onObjectFlick", false))
		{
			this.Call("onObjectFlick", new object[]
			{
				NPO.luaGameObjectScript,
				playerColor,
				force
			});
		}
	}

	// Token: 0x060014F7 RID: 5367 RVA: 0x0008987D File Offset: 0x00087A7D
	[MoonSharpHidden]
	public virtual void OnObjectPageChange(NetworkPhysicsObject NPO)
	{
		if (this.CanCall("onObjectPageChange", false))
		{
			this.Call("onObjectPageChange", new object[]
			{
				NPO.luaGameObjectScript
			});
		}
	}

	// Token: 0x060014F8 RID: 5368 RVA: 0x000898A8 File Offset: 0x00087AA8
	[MoonSharpHidden]
	public virtual void OnChat(string message, int id)
	{
		if (this.CanCall("onChat", false))
		{
			DynValue dynValue = this.TryCall("onChat", new object[]
			{
				message,
				LuaGlobalScriptManager.Instance.GlobalPlayer.GetPlayer(id)
			});
			if (dynValue != null && !dynValue.IsNilOrNan() && !dynValue.Boolean)
			{
				NetworkSingleton<Chat>.Instance.LuaBlockChatMessage = true;
			}
		}
	}

	// Token: 0x060014F9 RID: 5369 RVA: 0x0008990A File Offset: 0x00087B0A
	[MoonSharpHidden]
	public virtual void OnBlindfold(bool bBlind, int id)
	{
		this.TryCall("onBlindfold", new object[]
		{
			LuaGlobalScriptManager.Instance.GlobalPlayer.GetPlayer(id),
			bBlind ? DynValue.True : DynValue.False
		});
	}

	// Token: 0x060014FA RID: 5370 RVA: 0x00089943 File Offset: 0x00087B43
	[MoonSharpHidden]
	public void OnHandSelectModeEnd(string playerColour, string handSelectModeLabel, List<LuaGameObjectScript> chosenObjects, bool confirmed)
	{
		this.TryCall("onPlayerHandChoice", new object[]
		{
			playerColour,
			handSelectModeLabel,
			chosenObjects,
			confirmed
		});
	}

	// Token: 0x060014FB RID: 5371 RVA: 0x0008996D File Offset: 0x00087B6D
	[MoonSharpHidden]
	public virtual void OnPlayersConnect(PlayerState playerState)
	{
		Wait.Frames(delegate
		{
			if (this.CanCall("onPlayerConnect", false))
			{
				this.Call("onPlayerConnect", new object[]
				{
					LuaGlobalScriptManager.Instance.GlobalPlayer.GetPlayer(playerState.id)
				});
			}
		}, 1);
	}

	// Token: 0x060014FC RID: 5372 RVA: 0x00089994 File Offset: 0x00087B94
	[MoonSharpHidden]
	public virtual void OnPlayersDisconnect(PlayerState playerState)
	{
		if (this.CanCall("onPlayerDisconnect", false))
		{
			this.Call("onPlayerDisconnect", new object[]
			{
				LuaGlobalScriptManager.Instance.GlobalPlayer.GetPlayer(playerState.id)
			});
		}
	}

	// Token: 0x060014FD RID: 5373 RVA: 0x000899D0 File Offset: 0x00087BD0
	[MoonSharpHidden]
	public virtual void OnSave()
	{
		if (this.CanCall("onSave", false))
		{
			DynValue dynValue = this.TryCall("onSave");
			if (dynValue != null && !dynValue.IsNilOrNan())
			{
				this.script_state = dynValue.String;
			}
		}
	}

	// Token: 0x060014FE RID: 5374 RVA: 0x00089A0E File Offset: 0x00087C0E
	[MoonSharpHidden]
	public virtual void OnExternalMessage(Dictionary<object, object> message)
	{
		this.TryCall("onExternalMessage", new object[]
		{
			message
		});
	}

	// Token: 0x060014FF RID: 5375 RVA: 0x00089A28 File Offset: 0x00087C28
	public DynValue ExecuteScript(string script, bool doNotCatch = false)
	{
		if (doNotCatch)
		{
			return this.lua.DoString(script, null, null);
		}
		DynValue result;
		try
		{
			result = this.lua.DoString(script, null, null);
		}
		catch (Exception e)
		{
			this.LogError("executeScript", e, null);
			result = null;
		}
		return result;
	}

	// Token: 0x06001500 RID: 5376 RVA: 0x00089A7C File Offset: 0x00087C7C
	[MoonSharpHidden]
	[Remote(Permission.Admin)]
	public void RPCExecuteScript(string script)
	{
		if (Network.isClient)
		{
			base.networkView.RPC<string>(RPCTarget.Server, new Action<string>(this.RPCExecuteScript), script);
			return;
		}
		LuaGlobalScriptManager.Instance.ExecuteScript(script, false);
	}

	// Token: 0x06001501 RID: 5377 RVA: 0x00089AAC File Offset: 0x00087CAC
	[MoonSharpHidden]
	public static LuaScript ScriptToLuaScript(Script s)
	{
		if (s == null)
		{
			return null;
		}
		LuaGlobalScriptManager instance = LuaGlobalScriptManager.Instance;
		if (instance.lua == s)
		{
			return instance;
		}
		if (instance.GlobalDummyObject.lua == s)
		{
			return instance.GlobalDummyObject;
		}
		List<NetworkPhysicsObject> allNPOs = NetworkSingleton<ManagerPhysicsObject>.Instance.AllNPOs;
		for (int i = 0; i < allNPOs.Count; i++)
		{
			NetworkPhysicsObject networkPhysicsObject = allNPOs[i];
			if (networkPhysicsObject.luaGameObjectScript && networkPhysicsObject.luaGameObjectScript.lua == s)
			{
				return networkPhysicsObject.luaGameObjectScript;
			}
		}
		return null;
	}

	// Token: 0x06001502 RID: 5378 RVA: 0x00089B2C File Offset: 0x00087D2C
	[MoonSharpHidden]
	public static DynValue TryCall(Closure function)
	{
		if (function != null && function.OwnerScript != null)
		{
			try
			{
				return function.Call();
			}
			catch (Exception e)
			{
				LuaScript luaScript = LuaScript.ScriptToLuaScript(function.OwnerScript);
				if (luaScript)
				{
					luaScript.LogError(e);
				}
				return null;
			}
		}
		return null;
	}

	// Token: 0x06001503 RID: 5379 RVA: 0x00089B80 File Offset: 0x00087D80
	[MoonSharpHidden]
	public static DynValue TryCall(Closure function, params object[] args)
	{
		if (function != null && function.OwnerScript != null)
		{
			try
			{
				return function.Call(args);
			}
			catch (Exception e)
			{
				LuaScript luaScript = LuaScript.ScriptToLuaScript(function.OwnerScript);
				if (luaScript)
				{
					luaScript.LogError(e);
				}
				return null;
			}
		}
		return null;
	}

	// Token: 0x04000BEB RID: 3051
	[MoonSharpHidden]
	public Script lua;

	// Token: 0x04000BEC RID: 3052
	[TextArea(1, 40)]
	[NonSerialized]
	public string script_code = "";

	// Token: 0x04000BED RID: 3053
	[NonSerialized]
	public string script_state = "";

	// Token: 0x04000BEE RID: 3054
	[MoonSharpHidden]
	[NonSerialized]
	public bool loaded;

	// Token: 0x04000BEF RID: 3055
	[MoonSharpHidden]
	[NonSerialized]
	public bool IsGlobalDummyObject;

	// Token: 0x04000BF1 RID: 3057
	private bool startHappened;

	// Token: 0x04000BF2 RID: 3058
	[MoonSharpHidden]
	[NonSerialized]
	public bool BlockEvents;
}
