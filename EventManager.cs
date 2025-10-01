using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200010A RID: 266
public class EventManager : MonoBehaviour
{
	// Token: 0x14000005 RID: 5
	// (add) Token: 0x06000CE5 RID: 3301 RVA: 0x00056D84 File Offset: 0x00054F84
	// (remove) Token: 0x06000CE6 RID: 3302 RVA: 0x00056DB8 File Offset: 0x00054FB8
	public static event EventManager.UnityAnalytics OnUnityAnalytics;

	// Token: 0x06000CE7 RID: 3303 RVA: 0x00056DEB File Offset: 0x00054FEB
	public static void TriggerUnityAnalytic(string eventName, IDictionary<string, object> eventData = null, int limit = 0)
	{
		EventManager.UnityAnalytics onUnityAnalytics = EventManager.OnUnityAnalytics;
		if (onUnityAnalytics == null)
		{
			return;
		}
		onUnityAnalytics(eventName, eventData, limit);
	}

	// Token: 0x06000CE8 RID: 3304 RVA: 0x00056DFF File Offset: 0x00054FFF
	public static void TriggerUnityAnalytic(string eventName, string dataName, object data, int limit = 0)
	{
		EventManager.EventData.Clear();
		EventManager.EventData.Add(dataName, data);
		EventManager.UnityAnalytics onUnityAnalytics = EventManager.OnUnityAnalytics;
		if (onUnityAnalytics == null)
		{
			return;
		}
		onUnityAnalytics(eventName, EventManager.EventData, limit);
	}

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x06000CE9 RID: 3305 RVA: 0x00056E30 File Offset: 0x00055030
	// (remove) Token: 0x06000CEA RID: 3306 RVA: 0x00056E64 File Offset: 0x00055064
	public static event EventManager.UIThemeChange OnUIThemeChange;

	// Token: 0x06000CEB RID: 3307 RVA: 0x00056E97 File Offset: 0x00055097
	public static void TriggerUIThemeChange()
	{
		EventManager.UIThemeChange onUIThemeChange = EventManager.OnUIThemeChange;
		if (onUIThemeChange == null)
		{
			return;
		}
		onUIThemeChange();
	}

	// Token: 0x14000007 RID: 7
	// (add) Token: 0x06000CEC RID: 3308 RVA: 0x00056EA8 File Offset: 0x000550A8
	// (remove) Token: 0x06000CED RID: 3309 RVA: 0x00056EDC File Offset: 0x000550DC
	public static event EventManager.ChangePointerMode OnChangePointerMode;

	// Token: 0x06000CEE RID: 3310 RVA: 0x00056F0F File Offset: 0x0005510F
	public static void TriggerChangePointerMode(PointerMode newMode)
	{
		EventManager.ChangePointerMode onChangePointerMode = EventManager.OnChangePointerMode;
		if (onChangePointerMode == null)
		{
			return;
		}
		onChangePointerMode(newMode);
	}

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06000CEF RID: 3311 RVA: 0x00056F24 File Offset: 0x00055124
	// (remove) Token: 0x06000CF0 RID: 3312 RVA: 0x00056F58 File Offset: 0x00055158
	public static event EventManager.ChangePlayerColor OnChangePlayerColor;

	// Token: 0x06000CF1 RID: 3313 RVA: 0x00056F8B File Offset: 0x0005518B
	public static void TriggerChangePlayerColor(Color newColor, int id)
	{
		EventManager.ChangePlayerColor onChangePlayerColor = EventManager.OnChangePlayerColor;
		if (onChangePlayerColor == null)
		{
			return;
		}
		onChangePlayerColor(newColor, id);
	}

	// Token: 0x14000009 RID: 9
	// (add) Token: 0x06000CF2 RID: 3314 RVA: 0x00056FA0 File Offset: 0x000551A0
	// (remove) Token: 0x06000CF3 RID: 3315 RVA: 0x00056FD4 File Offset: 0x000551D4
	public static event EventManager.ChangePlayerTeam OnChangePlayerTeam;

	// Token: 0x06000CF4 RID: 3316 RVA: 0x00057007 File Offset: 0x00055207
	public static void TriggerChangePlayerTeam(bool join, int id)
	{
		EventManager.ChangePlayerTeam onChangePlayerTeam = EventManager.OnChangePlayerTeam;
		if (onChangePlayerTeam == null)
		{
			return;
		}
		onChangePlayerTeam(join, id);
	}

	// Token: 0x1400000A RID: 10
	// (add) Token: 0x06000CF5 RID: 3317 RVA: 0x0005701C File Offset: 0x0005521C
	// (remove) Token: 0x06000CF6 RID: 3318 RVA: 0x00057050 File Offset: 0x00055250
	public static event EventManager.ChatMessageTypeEvent OnChatMessage;

	// Token: 0x06000CF7 RID: 3319 RVA: 0x00057083 File Offset: 0x00055283
	public static void TriggerChatMessageType(ChatMessageType type)
	{
		EventManager.ChatMessageTypeEvent onChatMessage = EventManager.OnChatMessage;
		if (onChatMessage == null)
		{
			return;
		}
		onChatMessage(type);
	}

	// Token: 0x1400000B RID: 11
	// (add) Token: 0x06000CF8 RID: 3320 RVA: 0x00057098 File Offset: 0x00055298
	// (remove) Token: 0x06000CF9 RID: 3321 RVA: 0x000570CC File Offset: 0x000552CC
	public static event EventManager.ChatTyping OnChatTyping;

	// Token: 0x06000CFA RID: 3322 RVA: 0x000570FF File Offset: 0x000552FF
	public static void TriggerChatTyping(ChatMessageType type, bool typing)
	{
		EventManager.ChatTyping onChatTyping = EventManager.OnChatTyping;
		if (onChatTyping == null)
		{
			return;
		}
		onChatTyping(type, typing);
	}

	// Token: 0x1400000C RID: 12
	// (add) Token: 0x06000CFB RID: 3323 RVA: 0x00057114 File Offset: 0x00055314
	// (remove) Token: 0x06000CFC RID: 3324 RVA: 0x00057148 File Offset: 0x00055348
	public static event EventManager.PlayerChatTyping OnPlayerChatTyping;

	// Token: 0x06000CFD RID: 3325 RVA: 0x0005717B File Offset: 0x0005537B
	public static void TriggerPlayerChatTyping(PlayerState player, bool typing)
	{
		EventManager.PlayerChatTyping onPlayerChatTyping = EventManager.OnPlayerChatTyping;
		if (onPlayerChatTyping == null)
		{
			return;
		}
		onPlayerChatTyping(player, typing);
	}

	// Token: 0x1400000D RID: 13
	// (add) Token: 0x06000CFE RID: 3326 RVA: 0x00057190 File Offset: 0x00055390
	// (remove) Token: 0x06000CFF RID: 3327 RVA: 0x000571C4 File Offset: 0x000553C4
	public static event EventManager.AllowMessagesChanged OnAllowMessagesChanged;

	// Token: 0x06000D00 RID: 3328 RVA: 0x000571F7 File Offset: 0x000553F7
	public static void TriggerAllowMessages(bool allow, ChatMessageType type)
	{
		EventManager.AllowMessagesChanged onAllowMessagesChanged = EventManager.OnAllowMessagesChanged;
		if (onAllowMessagesChanged == null)
		{
			return;
		}
		onAllowMessagesChanged(allow, type);
	}

	// Token: 0x1400000E RID: 14
	// (add) Token: 0x06000D01 RID: 3329 RVA: 0x0005720C File Offset: 0x0005540C
	// (remove) Token: 0x06000D02 RID: 3330 RVA: 0x00057240 File Offset: 0x00055440
	public static event EventManager.AutoHideChatChanged OnAutoHideChat;

	// Token: 0x06000D03 RID: 3331 RVA: 0x00057273 File Offset: 0x00055473
	public static void TriggerAutoHideChat(bool allow)
	{
		EventManager.AutoHideChatChanged onAutoHideChat = EventManager.OnAutoHideChat;
		if (onAutoHideChat == null)
		{
			return;
		}
		onAutoHideChat(allow);
	}

	// Token: 0x1400000F RID: 15
	// (add) Token: 0x06000D04 RID: 3332 RVA: 0x00057288 File Offset: 0x00055488
	// (remove) Token: 0x06000D05 RID: 3333 RVA: 0x000572BC File Offset: 0x000554BC
	public static event EventManager.HandSelectModeEnd OnHandSelectModeEnd;

	// Token: 0x06000D06 RID: 3334 RVA: 0x000572EF File Offset: 0x000554EF
	public static void TriggerHandSelectModeEnd(string playerColor, string handSelectModeLabel, List<LuaGameObjectScript> chosenObjects, bool confirmed)
	{
		EventManager.HandSelectModeEnd onHandSelectModeEnd = EventManager.OnHandSelectModeEnd;
		if (onHandSelectModeEnd == null)
		{
			return;
		}
		onHandSelectModeEnd(playerColor, handSelectModeLabel, chosenObjects, confirmed);
	}

	// Token: 0x14000010 RID: 16
	// (add) Token: 0x06000D07 RID: 3335 RVA: 0x00057304 File Offset: 0x00055504
	// (remove) Token: 0x06000D08 RID: 3336 RVA: 0x00057338 File Offset: 0x00055538
	public static event EventManager.LoadingComplete OnLoadingComplete;

	// Token: 0x06000D09 RID: 3337 RVA: 0x0005736B File Offset: 0x0005556B
	public static void TriggerLoadingSaveComplete()
	{
		EventManager.LoadingComplete onLoadingComplete = EventManager.OnLoadingComplete;
		if (onLoadingComplete == null)
		{
			return;
		}
		onLoadingComplete();
	}

	// Token: 0x14000011 RID: 17
	// (add) Token: 0x06000D0A RID: 3338 RVA: 0x0005737C File Offset: 0x0005557C
	// (remove) Token: 0x06000D0B RID: 3339 RVA: 0x000573B0 File Offset: 0x000555B0
	public static event EventManager.LoadingChange OnLoadingChange;

	// Token: 0x06000D0C RID: 3340 RVA: 0x000573E3 File Offset: 0x000555E3
	public static void TriggerLoadingChange(int NumDownloads, int NumComplete)
	{
		EventManager.LoadingChange onLoadingChange = EventManager.OnLoadingChange;
		if (onLoadingChange == null)
		{
			return;
		}
		onLoadingChange(NumDownloads, NumComplete);
	}

	// Token: 0x14000012 RID: 18
	// (add) Token: 0x06000D0D RID: 3341 RVA: 0x000573F8 File Offset: 0x000555F8
	// (remove) Token: 0x06000D0E RID: 3342 RVA: 0x0005742C File Offset: 0x0005562C
	public static event EventManager.ZoneChangeAdd OnZoneAdd;

	// Token: 0x06000D0F RID: 3343 RVA: 0x0005745F File Offset: 0x0005565F
	public static void TriggerZoneAdd(NetworkPhysicsObject Zone, NetworkPhysicsObject ChangeObject)
	{
		EventManager.ZoneChangeAdd onZoneAdd = EventManager.OnZoneAdd;
		if (onZoneAdd == null)
		{
			return;
		}
		onZoneAdd(Zone, ChangeObject);
	}

	// Token: 0x14000013 RID: 19
	// (add) Token: 0x06000D10 RID: 3344 RVA: 0x00057474 File Offset: 0x00055674
	// (remove) Token: 0x06000D11 RID: 3345 RVA: 0x000574A8 File Offset: 0x000556A8
	public static event EventManager.ZoneChangeRemove OnZoneRemove;

	// Token: 0x06000D12 RID: 3346 RVA: 0x000574DB File Offset: 0x000556DB
	public static void TriggerZoneRemove(NetworkPhysicsObject Zone, NetworkPhysicsObject ChangeObject)
	{
		EventManager.ZoneChangeRemove onZoneRemove = EventManager.OnZoneRemove;
		if (onZoneRemove == null)
		{
			return;
		}
		onZoneRemove(Zone, ChangeObject);
	}

	// Token: 0x14000014 RID: 20
	// (add) Token: 0x06000D13 RID: 3347 RVA: 0x000574F0 File Offset: 0x000556F0
	// (remove) Token: 0x06000D14 RID: 3348 RVA: 0x00057524 File Offset: 0x00055724
	public static event EventManager.NetworkObjectSpawn OnNetworkObjectSpawn;

	// Token: 0x06000D15 RID: 3349 RVA: 0x00057557 File Offset: 0x00055757
	public static void TriggerNetworkObjectSpawn(NetworkPhysicsObject NPO)
	{
		EventManager.NetworkObjectSpawn onNetworkObjectSpawn = EventManager.OnNetworkObjectSpawn;
		if (onNetworkObjectSpawn == null)
		{
			return;
		}
		onNetworkObjectSpawn(NPO);
	}

	// Token: 0x14000015 RID: 21
	// (add) Token: 0x06000D16 RID: 3350 RVA: 0x0005756C File Offset: 0x0005576C
	// (remove) Token: 0x06000D17 RID: 3351 RVA: 0x000575A0 File Offset: 0x000557A0
	public static event EventManager.NetworkObjectSpawnFromUI OnNetworkObjectSpawnFromUI;

	// Token: 0x06000D18 RID: 3352 RVA: 0x000575D3 File Offset: 0x000557D3
	public static void TriggerNetworkObjectSpawnFromUI(NetworkPhysicsObject NPO)
	{
		EventManager.NetworkObjectSpawnFromUI onNetworkObjectSpawnFromUI = EventManager.OnNetworkObjectSpawnFromUI;
		if (onNetworkObjectSpawnFromUI == null)
		{
			return;
		}
		onNetworkObjectSpawnFromUI(NPO);
	}

	// Token: 0x14000016 RID: 22
	// (add) Token: 0x06000D19 RID: 3353 RVA: 0x000575E8 File Offset: 0x000557E8
	// (remove) Token: 0x06000D1A RID: 3354 RVA: 0x0005761C File Offset: 0x0005581C
	public static event EventManager.NetworkObjectDestroy OnNetworkObjectDestroy;

	// Token: 0x06000D1B RID: 3355 RVA: 0x0005764F File Offset: 0x0005584F
	public static void TriggerNetworkObjectDestroy(NetworkPhysicsObject NPO)
	{
		EventManager.NetworkObjectDestroy onNetworkObjectDestroy = EventManager.OnNetworkObjectDestroy;
		if (onNetworkObjectDestroy == null)
		{
			return;
		}
		onNetworkObjectDestroy(NPO);
	}

	// Token: 0x14000017 RID: 23
	// (add) Token: 0x06000D1C RID: 3356 RVA: 0x00057664 File Offset: 0x00055864
	// (remove) Token: 0x06000D1D RID: 3357 RVA: 0x00057698 File Offset: 0x00055898
	public static event EventManager.NetworkObjectHide OnNetworkObjectHide;

	// Token: 0x06000D1E RID: 3358 RVA: 0x000576CB File Offset: 0x000558CB
	public static void TriggerNetworkObjectHide(NetworkPhysicsObject NPO, bool bHide)
	{
		EventManager.NetworkObjectHide onNetworkObjectHide = EventManager.OnNetworkObjectHide;
		if (onNetworkObjectHide == null)
		{
			return;
		}
		onNetworkObjectHide(NPO, bHide);
	}

	// Token: 0x14000018 RID: 24
	// (add) Token: 0x06000D1F RID: 3359 RVA: 0x000576E0 File Offset: 0x000558E0
	// (remove) Token: 0x06000D20 RID: 3360 RVA: 0x00057714 File Offset: 0x00055914
	public static event EventManager.LuaObjectSpawn OnLuaObjectSpawn;

	// Token: 0x06000D21 RID: 3361 RVA: 0x00057747 File Offset: 0x00055947
	public static void TriggerLuaObjectSpawn(LuaGameObjectScript LGOS)
	{
		if (EventManager.OnLuaObjectSpawn != null)
		{
			LGOS.Spawned();
			EventManager.OnLuaObjectSpawn(LGOS);
		}
	}

	// Token: 0x14000019 RID: 25
	// (add) Token: 0x06000D22 RID: 3362 RVA: 0x00057764 File Offset: 0x00055964
	// (remove) Token: 0x06000D23 RID: 3363 RVA: 0x00057798 File Offset: 0x00055998
	public static event EventManager.LuaObjectDestroy OnLuaObjectDestroy;

	// Token: 0x06000D24 RID: 3364 RVA: 0x000577CB File Offset: 0x000559CB
	public static void TriggerLuaObjectDestroy(LuaGameObjectScript LGOS)
	{
		EventManager.LuaObjectDestroy onLuaObjectDestroy = EventManager.OnLuaObjectDestroy;
		if (onLuaObjectDestroy == null)
		{
			return;
		}
		onLuaObjectDestroy(LGOS);
	}

	// Token: 0x1400001A RID: 26
	// (add) Token: 0x06000D25 RID: 3365 RVA: 0x000577E0 File Offset: 0x000559E0
	// (remove) Token: 0x06000D26 RID: 3366 RVA: 0x00057814 File Offset: 0x00055A14
	public static event EventManager.FogOfWarRevealerAdd OnFogOfWarRevealerAdd;

	// Token: 0x06000D27 RID: 3367 RVA: 0x00057847 File Offset: 0x00055A47
	public static void TriggerFogOfWarRevealerAdd(GameObject fogOfWarRevealer)
	{
		EventManager.FogOfWarRevealerAdd onFogOfWarRevealerAdd = EventManager.OnFogOfWarRevealerAdd;
		if (onFogOfWarRevealerAdd == null)
		{
			return;
		}
		onFogOfWarRevealerAdd(fogOfWarRevealer);
	}

	// Token: 0x1400001B RID: 27
	// (add) Token: 0x06000D28 RID: 3368 RVA: 0x0005785C File Offset: 0x00055A5C
	// (remove) Token: 0x06000D29 RID: 3369 RVA: 0x00057890 File Offset: 0x00055A90
	public static event EventManager.FogOfWarRevealerDestroy OnFogOfWarRevealerDestroy;

	// Token: 0x06000D2A RID: 3370 RVA: 0x000578C3 File Offset: 0x00055AC3
	public static void TriggerFogOfWarRevealerDestroy(GameObject fogOfWarRevealer)
	{
		EventManager.FogOfWarRevealerDestroy onFogOfWarRevealerDestroy = EventManager.OnFogOfWarRevealerDestroy;
		if (onFogOfWarRevealerDestroy == null)
		{
			return;
		}
		onFogOfWarRevealerDestroy(fogOfWarRevealer);
	}

	// Token: 0x1400001C RID: 28
	// (add) Token: 0x06000D2B RID: 3371 RVA: 0x000578D8 File Offset: 0x00055AD8
	// (remove) Token: 0x06000D2C RID: 3372 RVA: 0x0005790C File Offset: 0x00055B0C
	public static event EventManager.PlayerTurnStart OnPlayerTurnStart;

	// Token: 0x06000D2D RID: 3373 RVA: 0x0005793F File Offset: 0x00055B3F
	public static void TriggerPlayerTurnStart(string EndColor, string StartColor)
	{
		EventManager.PlayerTurnStart onPlayerTurnStart = EventManager.OnPlayerTurnStart;
		if (onPlayerTurnStart == null)
		{
			return;
		}
		onPlayerTurnStart(EndColor, StartColor);
	}

	// Token: 0x1400001D RID: 29
	// (add) Token: 0x06000D2E RID: 3374 RVA: 0x00057954 File Offset: 0x00055B54
	// (remove) Token: 0x06000D2F RID: 3375 RVA: 0x00057988 File Offset: 0x00055B88
	public static event EventManager.PlayerTurnEnd OnPlayerTurnEnd;

	// Token: 0x06000D30 RID: 3376 RVA: 0x000579BB File Offset: 0x00055BBB
	public static void TriggerPlayerTurnEnd(string EndColor, string StartColor)
	{
		EventManager.PlayerTurnEnd onPlayerTurnEnd = EventManager.OnPlayerTurnEnd;
		if (onPlayerTurnEnd == null)
		{
			return;
		}
		onPlayerTurnEnd(EndColor, StartColor);
	}

	// Token: 0x1400001E RID: 30
	// (add) Token: 0x06000D31 RID: 3377 RVA: 0x000579D0 File Offset: 0x00055BD0
	// (remove) Token: 0x06000D32 RID: 3378 RVA: 0x00057A04 File Offset: 0x00055C04
	public static event EventManager.PlayerPing OnPlayerPing;

	// Token: 0x06000D33 RID: 3379 RVA: 0x00057A37 File Offset: 0x00055C37
	public static void TriggerPlayerPing(string playerColor, Vector3 position, LuaGameObjectScript hoveredObject)
	{
		EventManager.PlayerPing onPlayerPing = EventManager.OnPlayerPing;
		if (onPlayerPing == null)
		{
			return;
		}
		onPlayerPing(playerColor, position, hoveredObject);
	}

	// Token: 0x1400001F RID: 31
	// (add) Token: 0x06000D34 RID: 3380 RVA: 0x00057A4C File Offset: 0x00055C4C
	// (remove) Token: 0x06000D35 RID: 3381 RVA: 0x00057A80 File Offset: 0x00055C80
	public static event EventManager.GameRoundStart OnGameRoundStart;

	// Token: 0x06000D36 RID: 3382 RVA: 0x00057AB3 File Offset: 0x00055CB3
	public static void TriggerGameRoundStart()
	{
		EventManager.GameRoundStart onGameRoundStart = EventManager.OnGameRoundStart;
		if (onGameRoundStart == null)
		{
			return;
		}
		onGameRoundStart();
	}

	// Token: 0x14000020 RID: 32
	// (add) Token: 0x06000D37 RID: 3383 RVA: 0x00057AC4 File Offset: 0x00055CC4
	// (remove) Token: 0x06000D38 RID: 3384 RVA: 0x00057AF8 File Offset: 0x00055CF8
	public static event EventManager.GameRoundEnd OnGameRoundEnd;

	// Token: 0x06000D39 RID: 3385 RVA: 0x00057B2B File Offset: 0x00055D2B
	public static void TriggerGameRoundEnd()
	{
		EventManager.GameRoundEnd onGameRoundEnd = EventManager.OnGameRoundEnd;
		if (onGameRoundEnd == null)
		{
			return;
		}
		onGameRoundEnd();
	}

	// Token: 0x14000021 RID: 33
	// (add) Token: 0x06000D3A RID: 3386 RVA: 0x00057B3C File Offset: 0x00055D3C
	// (remove) Token: 0x06000D3B RID: 3387 RVA: 0x00057B70 File Offset: 0x00055D70
	public static event EventManager.ObjectHover OnObjectHover;

	// Token: 0x06000D3C RID: 3388 RVA: 0x00057BA3 File Offset: 0x00055DA3
	public static void TriggerObjectHover(GameObject HoverObject, Color Player)
	{
		EventManager.ObjectHover onObjectHover = EventManager.OnObjectHover;
		if (onObjectHover == null)
		{
			return;
		}
		onObjectHover(HoverObject, Player);
	}

	// Token: 0x14000022 RID: 34
	// (add) Token: 0x06000D3D RID: 3389 RVA: 0x00057BB8 File Offset: 0x00055DB8
	// (remove) Token: 0x06000D3E RID: 3390 RVA: 0x00057BEC File Offset: 0x00055DEC
	public static event EventManager.ObjectPickUp OnObjectPickUp;

	// Token: 0x06000D3F RID: 3391 RVA: 0x00057C1F File Offset: 0x00055E1F
	public static void TriggerObjectPickUp(NetworkPhysicsObject PickUpObject, PlayerState Player)
	{
		if (!PickUpObject || !Player.IsValid())
		{
			return;
		}
		EventManager.ObjectPickUp onObjectPickUp = EventManager.OnObjectPickUp;
		if (onObjectPickUp == null)
		{
			return;
		}
		onObjectPickUp(PickUpObject, Player);
	}

	// Token: 0x14000023 RID: 35
	// (add) Token: 0x06000D40 RID: 3392 RVA: 0x00057C44 File Offset: 0x00055E44
	// (remove) Token: 0x06000D41 RID: 3393 RVA: 0x00057C78 File Offset: 0x00055E78
	public static event EventManager.ObjectDrop OnObjectDrop;

	// Token: 0x06000D42 RID: 3394 RVA: 0x00057CAB File Offset: 0x00055EAB
	public static void TriggerObjectDrop(NetworkPhysicsObject DropObject, PlayerState LastPlayerToHold)
	{
		if (!DropObject || !LastPlayerToHold.IsValid())
		{
			return;
		}
		EventManager.ObjectDrop onObjectDrop = EventManager.OnObjectDrop;
		if (onObjectDrop == null)
		{
			return;
		}
		onObjectDrop(DropObject, LastPlayerToHold);
	}

	// Token: 0x14000024 RID: 36
	// (add) Token: 0x06000D43 RID: 3395 RVA: 0x00057CD0 File Offset: 0x00055ED0
	// (remove) Token: 0x06000D44 RID: 3396 RVA: 0x00057D04 File Offset: 0x00055F04
	public static event EventManager.ObjectFinishSmoothMove OnObjectFinishSmoothMove;

	// Token: 0x06000D45 RID: 3397 RVA: 0x00057D37 File Offset: 0x00055F37
	public static void TriggerObjectFinishSmoothMove(NetworkPhysicsObject NPO)
	{
		EventManager.ObjectFinishSmoothMove onObjectFinishSmoothMove = EventManager.OnObjectFinishSmoothMove;
		if (onObjectFinishSmoothMove == null)
		{
			return;
		}
		onObjectFinishSmoothMove(NPO);
	}

	// Token: 0x14000025 RID: 37
	// (add) Token: 0x06000D46 RID: 3398 RVA: 0x00057D4C File Offset: 0x00055F4C
	// (remove) Token: 0x06000D47 RID: 3399 RVA: 0x00057D80 File Offset: 0x00055F80
	public static event EventManager.CloudUploadFinish OnCloudUploadFinish;

	// Token: 0x06000D48 RID: 3400 RVA: 0x00057DB3 File Offset: 0x00055FB3
	public static void TriggerCloudUploadFinish(string name, string url)
	{
		EventManager.CloudUploadFinish onCloudUploadFinish = EventManager.OnCloudUploadFinish;
		if (onCloudUploadFinish == null)
		{
			return;
		}
		onCloudUploadFinish(name, url);
	}

	// Token: 0x14000026 RID: 38
	// (add) Token: 0x06000D49 RID: 3401 RVA: 0x00057DC8 File Offset: 0x00055FC8
	// (remove) Token: 0x06000D4A RID: 3402 RVA: 0x00057DFC File Offset: 0x00055FFC
	public static event EventManager.GameSave OnGameSave;

	// Token: 0x06000D4B RID: 3403 RVA: 0x00057E2F File Offset: 0x0005602F
	public static void TriggerGameSave()
	{
		EventManager.GameSave onGameSave = EventManager.OnGameSave;
		if (onGameSave == null)
		{
			return;
		}
		onGameSave();
	}

	// Token: 0x14000027 RID: 39
	// (add) Token: 0x06000D4C RID: 3404 RVA: 0x00057E40 File Offset: 0x00056040
	// (remove) Token: 0x06000D4D RID: 3405 RVA: 0x00057E74 File Offset: 0x00056074
	public static event EventManager.Blindfold OnBlindfold;

	// Token: 0x06000D4E RID: 3406 RVA: 0x00057EA7 File Offset: 0x000560A7
	public static void TriggerBlindfold(bool bBlind, int id)
	{
		EventManager.Blindfold onBlindfold = EventManager.OnBlindfold;
		if (onBlindfold == null)
		{
			return;
		}
		onBlindfold(bBlind, id);
	}

	// Token: 0x14000028 RID: 40
	// (add) Token: 0x06000D4F RID: 3407 RVA: 0x00057EBC File Offset: 0x000560BC
	// (remove) Token: 0x06000D50 RID: 3408 RVA: 0x00057EF0 File Offset: 0x000560F0
	public static event EventManager.DummyObjectFinish OnDummyObjectFinish;

	// Token: 0x06000D51 RID: 3409 RVA: 0x00057F23 File Offset: 0x00056123
	public static void TriggerDummyObjectFinish(GameObject dummyGameObject)
	{
		EventManager.DummyObjectFinish onDummyObjectFinish = EventManager.OnDummyObjectFinish;
		if (onDummyObjectFinish == null)
		{
			return;
		}
		onDummyObjectFinish(dummyGameObject);
	}

	// Token: 0x14000029 RID: 41
	// (add) Token: 0x06000D52 RID: 3410 RVA: 0x00057F38 File Offset: 0x00056138
	// (remove) Token: 0x06000D53 RID: 3411 RVA: 0x00057F6C File Offset: 0x0005616C
	public static event EventManager.PlayerPromoted OnPlayerPromoted;

	// Token: 0x06000D54 RID: 3412 RVA: 0x00057F9F File Offset: 0x0005619F
	public static void TriggerPlayerPromote(bool isPromoted, int id)
	{
		EventManager.PlayerPromoted onPlayerPromoted = EventManager.OnPlayerPromoted;
		if (onPlayerPromoted == null)
		{
			return;
		}
		onPlayerPromoted(isPromoted, id);
	}

	// Token: 0x1400002A RID: 42
	// (add) Token: 0x06000D55 RID: 3413 RVA: 0x00057FB4 File Offset: 0x000561B4
	// (remove) Token: 0x06000D56 RID: 3414 RVA: 0x00057FE8 File Offset: 0x000561E8
	public static event EventManager.CanFlipTable OnCanFlipTable;

	// Token: 0x06000D57 RID: 3415 RVA: 0x0005801B File Offset: 0x0005621B
	public static void TriggerCanFlipTable(bool bCanFlipTable)
	{
		EventManager.CanFlipTable onCanFlipTable = EventManager.OnCanFlipTable;
		if (onCanFlipTable == null)
		{
			return;
		}
		onCanFlipTable(bCanFlipTable);
	}

	// Token: 0x1400002B RID: 43
	// (add) Token: 0x06000D58 RID: 3416 RVA: 0x00058030 File Offset: 0x00056230
	// (remove) Token: 0x06000D59 RID: 3417 RVA: 0x00058064 File Offset: 0x00056264
	public static event EventManager.ChangeTable OnChangeTable;

	// Token: 0x06000D5A RID: 3418 RVA: 0x00058097 File Offset: 0x00056297
	public static void TriggerChangeTable(GameObject Table)
	{
		EventManager.ChangeTable onChangeTable = EventManager.OnChangeTable;
		if (onChangeTable == null)
		{
			return;
		}
		onChangeTable(Table);
	}

	// Token: 0x1400002C RID: 44
	// (add) Token: 0x06000D5B RID: 3419 RVA: 0x000580AC File Offset: 0x000562AC
	// (remove) Token: 0x06000D5C RID: 3420 RVA: 0x000580E0 File Offset: 0x000562E0
	public static event EventManager.WorkshopUpToDate OnWorkshopUpToDate;

	// Token: 0x06000D5D RID: 3421 RVA: 0x00058113 File Offset: 0x00056313
	public static void TriggerWorkshopUpToDate()
	{
		EventManager.WorkshopUpToDate onWorkshopUpToDate = EventManager.OnWorkshopUpToDate;
		if (onWorkshopUpToDate == null)
		{
			return;
		}
		onWorkshopUpToDate();
	}

	// Token: 0x1400002D RID: 45
	// (add) Token: 0x06000D5E RID: 3422 RVA: 0x00058124 File Offset: 0x00056324
	// (remove) Token: 0x06000D5F RID: 3423 RVA: 0x00058158 File Offset: 0x00056358
	public static event EventManager.FileSave OnFileSave;

	// Token: 0x06000D60 RID: 3424 RVA: 0x0005818B File Offset: 0x0005638B
	public static void TriggerFileSave(string Path)
	{
		EventManager.FileSave onFileSave = EventManager.OnFileSave;
		if (onFileSave == null)
		{
			return;
		}
		onFileSave(Path);
	}

	// Token: 0x1400002E RID: 46
	// (add) Token: 0x06000D61 RID: 3425 RVA: 0x000581A0 File Offset: 0x000563A0
	// (remove) Token: 0x06000D62 RID: 3426 RVA: 0x000581D4 File Offset: 0x000563D4
	public static event EventManager.FileDelete OnFileDelete;

	// Token: 0x06000D63 RID: 3427 RVA: 0x00058207 File Offset: 0x00056407
	public static void TriggerFileDelete(string Path)
	{
		EventManager.FileDelete onFileDelete = EventManager.OnFileDelete;
		if (onFileDelete == null)
		{
			return;
		}
		onFileDelete(Path);
	}

	// Token: 0x1400002F RID: 47
	// (add) Token: 0x06000D64 RID: 3428 RVA: 0x0005821C File Offset: 0x0005641C
	// (remove) Token: 0x06000D65 RID: 3429 RVA: 0x00058250 File Offset: 0x00056450
	public static event EventManager.FocusUI OnFocusUI;

	// Token: 0x06000D66 RID: 3430 RVA: 0x00058283 File Offset: 0x00056483
	public static void TriggerOnFocusUI(UIDragObject FocusDragObject)
	{
		EventManager.FocusUI onFocusUI = EventManager.OnFocusUI;
		if (onFocusUI == null)
		{
			return;
		}
		onFocusUI(FocusDragObject);
	}

	// Token: 0x14000030 RID: 48
	// (add) Token: 0x06000D67 RID: 3431 RVA: 0x00058298 File Offset: 0x00056498
	// (remove) Token: 0x06000D68 RID: 3432 RVA: 0x000582CC File Offset: 0x000564CC
	public static event EventManager.ObjectEnterContainer OnObjectEnterContainer;

	// Token: 0x06000D69 RID: 3433 RVA: 0x000582FF File Offset: 0x000564FF
	public static void TriggerObjectEnterContainer(NetworkPhysicsObject Container, NetworkPhysicsObject Object)
	{
		EventManager.ObjectEnterContainer onObjectEnterContainer = EventManager.OnObjectEnterContainer;
		if (onObjectEnterContainer == null)
		{
			return;
		}
		onObjectEnterContainer(Container, Object);
	}

	// Token: 0x14000031 RID: 49
	// (add) Token: 0x06000D6A RID: 3434 RVA: 0x00058314 File Offset: 0x00056514
	// (remove) Token: 0x06000D6B RID: 3435 RVA: 0x00058348 File Offset: 0x00056548
	public static event EventManager.ObjectLeaveContainer OnObjectLeaveContainer;

	// Token: 0x06000D6C RID: 3436 RVA: 0x0005837B File Offset: 0x0005657B
	public static void TriggerObjectLeaveContainer(NetworkPhysicsObject Container, NetworkPhysicsObject Object)
	{
		EventManager.ObjectLeaveContainer onObjectLeaveContainer = EventManager.OnObjectLeaveContainer;
		if (onObjectLeaveContainer == null)
		{
			return;
		}
		onObjectLeaveContainer(Container, Object);
	}

	// Token: 0x14000032 RID: 50
	// (add) Token: 0x06000D6D RID: 3437 RVA: 0x00058390 File Offset: 0x00056590
	// (remove) Token: 0x06000D6E RID: 3438 RVA: 0x000583C4 File Offset: 0x000565C4
	public static event EventManager.ScriptingButtonDown OnScriptingButtonDown;

	// Token: 0x06000D6F RID: 3439 RVA: 0x000583F7 File Offset: 0x000565F7
	public static void TriggerScriptingButtonDown(int index, string playerColor)
	{
		EventManager.ScriptingButtonDown onScriptingButtonDown = EventManager.OnScriptingButtonDown;
		if (onScriptingButtonDown == null)
		{
			return;
		}
		onScriptingButtonDown(index, playerColor);
	}

	// Token: 0x14000033 RID: 51
	// (add) Token: 0x06000D70 RID: 3440 RVA: 0x0005840C File Offset: 0x0005660C
	// (remove) Token: 0x06000D71 RID: 3441 RVA: 0x00058440 File Offset: 0x00056640
	public static event EventManager.ScriptingButtonUp OnScriptingButtonUp;

	// Token: 0x06000D72 RID: 3442 RVA: 0x00058473 File Offset: 0x00056673
	public static void TriggerScriptingButtonUp(int index, string playerColor)
	{
		EventManager.ScriptingButtonUp onScriptingButtonUp = EventManager.OnScriptingButtonUp;
		if (onScriptingButtonUp == null)
		{
			return;
		}
		onScriptingButtonUp(index, playerColor);
	}

	// Token: 0x14000034 RID: 52
	// (add) Token: 0x06000D73 RID: 3443 RVA: 0x00058488 File Offset: 0x00056688
	// (remove) Token: 0x06000D74 RID: 3444 RVA: 0x000584BC File Offset: 0x000566BC
	public static event EventManager.ObjectTriggerEffect OnObjectTriggerEffect;

	// Token: 0x06000D75 RID: 3445 RVA: 0x000584EF File Offset: 0x000566EF
	public static void TriggerObjectTriggerEffect(NetworkPhysicsObject NPO, int index)
	{
		EventManager.ObjectTriggerEffect onObjectTriggerEffect = EventManager.OnObjectTriggerEffect;
		if (onObjectTriggerEffect == null)
		{
			return;
		}
		onObjectTriggerEffect(NPO, index);
	}

	// Token: 0x14000035 RID: 53
	// (add) Token: 0x06000D76 RID: 3446 RVA: 0x00058504 File Offset: 0x00056704
	// (remove) Token: 0x06000D77 RID: 3447 RVA: 0x00058538 File Offset: 0x00056738
	public static event EventManager.ObjectLoopingEffect OnObjectLoopingEffect;

	// Token: 0x06000D78 RID: 3448 RVA: 0x0005856B File Offset: 0x0005676B
	public static void TriggerObjectLoopingEffect(NetworkPhysicsObject NPO, int index)
	{
		EventManager.ObjectLoopingEffect onObjectLoopingEffect = EventManager.OnObjectLoopingEffect;
		if (onObjectLoopingEffect == null)
		{
			return;
		}
		onObjectLoopingEffect(NPO, index);
	}

	// Token: 0x14000036 RID: 54
	// (add) Token: 0x06000D79 RID: 3449 RVA: 0x00058580 File Offset: 0x00056780
	// (remove) Token: 0x06000D7A RID: 3450 RVA: 0x000585B4 File Offset: 0x000567B4
	public static event EventManager.VR OnVR;

	// Token: 0x06000D7B RID: 3451 RVA: 0x000585E7 File Offset: 0x000567E7
	public static void TriggerVR(bool bEnabled)
	{
		EventManager.VR onVR = EventManager.OnVR;
		if (onVR == null)
		{
			return;
		}
		onVR(bEnabled);
	}

	// Token: 0x14000037 RID: 55
	// (add) Token: 0x06000D7C RID: 3452 RVA: 0x000585FC File Offset: 0x000567FC
	// (remove) Token: 0x06000D7D RID: 3453 RVA: 0x00058630 File Offset: 0x00056830
	public static event EventManager.ObjectRandomize OnObjectRandomize;

	// Token: 0x06000D7E RID: 3454 RVA: 0x00058663 File Offset: 0x00056863
	public static void TriggerObjectRandomize(NetworkPhysicsObject NPO, string playerColor)
	{
		EventManager.ObjectRandomize onObjectRandomize = EventManager.OnObjectRandomize;
		if (onObjectRandomize == null)
		{
			return;
		}
		onObjectRandomize(NPO, playerColor);
	}

	// Token: 0x14000038 RID: 56
	// (add) Token: 0x06000D7F RID: 3455 RVA: 0x00058678 File Offset: 0x00056878
	// (remove) Token: 0x06000D80 RID: 3456 RVA: 0x000586AC File Offset: 0x000568AC
	public static event EventManager.ObjectRotate OnObjectRotate;

	// Token: 0x06000D81 RID: 3457 RVA: 0x000586DF File Offset: 0x000568DF
	public static void TriggerObjectRotate(NetworkPhysicsObject NPO, int spinIndex, int flipIndex, string playerColor, int previousSpinIndex, int previousFlipIndex)
	{
		EventManager.ObjectRotate onObjectRotate = EventManager.OnObjectRotate;
		if (onObjectRotate == null)
		{
			return;
		}
		onObjectRotate(NPO, spinIndex, flipIndex, playerColor, previousSpinIndex, previousFlipIndex);
	}

	// Token: 0x14000039 RID: 57
	// (add) Token: 0x06000D82 RID: 3458 RVA: 0x000586F8 File Offset: 0x000568F8
	// (remove) Token: 0x06000D83 RID: 3459 RVA: 0x0005872C File Offset: 0x0005692C
	public static event EventManager.ObjectSearchStart OnObjectSearchStart;

	// Token: 0x06000D84 RID: 3460 RVA: 0x0005875F File Offset: 0x0005695F
	public static void TriggerObjectSearchStart(NetworkPhysicsObject NPO, string playerColor)
	{
		EventManager.ObjectSearchStart onObjectSearchStart = EventManager.OnObjectSearchStart;
		if (onObjectSearchStart == null)
		{
			return;
		}
		onObjectSearchStart(NPO, playerColor);
	}

	// Token: 0x1400003A RID: 58
	// (add) Token: 0x06000D85 RID: 3461 RVA: 0x00058774 File Offset: 0x00056974
	// (remove) Token: 0x06000D86 RID: 3462 RVA: 0x000587A8 File Offset: 0x000569A8
	public static event EventManager.ObjectSearchEnd OnObjectSearchEnd;

	// Token: 0x06000D87 RID: 3463 RVA: 0x000587DB File Offset: 0x000569DB
	public static void TriggerObjectSearchEnd(NetworkPhysicsObject NPO, string playerColor)
	{
		EventManager.ObjectSearchEnd onObjectSearchEnd = EventManager.OnObjectSearchEnd;
		if (onObjectSearchEnd == null)
		{
			return;
		}
		onObjectSearchEnd(NPO, playerColor);
	}

	// Token: 0x1400003B RID: 59
	// (add) Token: 0x06000D88 RID: 3464 RVA: 0x000587F0 File Offset: 0x000569F0
	// (remove) Token: 0x06000D89 RID: 3465 RVA: 0x00058824 File Offset: 0x00056A24
	public static event EventManager.ObjectPeek OnObjectPeek;

	// Token: 0x06000D8A RID: 3466 RVA: 0x00058857 File Offset: 0x00056A57
	public static void TriggerObjectPeek(NetworkPhysicsObject NPO, string playerColor)
	{
		EventManager.ObjectPeek onObjectPeek = EventManager.OnObjectPeek;
		if (onObjectPeek == null)
		{
			return;
		}
		onObjectPeek(NPO, playerColor);
	}

	// Token: 0x1400003C RID: 60
	// (add) Token: 0x06000D8B RID: 3467 RVA: 0x0005886C File Offset: 0x00056A6C
	// (remove) Token: 0x06000D8C RID: 3468 RVA: 0x000588A0 File Offset: 0x00056AA0
	public static event EventManager.ObjectFlick OnObjectFlick;

	// Token: 0x06000D8D RID: 3469 RVA: 0x000588D3 File Offset: 0x00056AD3
	public static void TriggerObjectFlick(NetworkPhysicsObject NPO, string playerColor, Vector3 force)
	{
		EventManager.ObjectFlick onObjectFlick = EventManager.OnObjectFlick;
		if (onObjectFlick == null)
		{
			return;
		}
		onObjectFlick(NPO, playerColor, force);
	}

	// Token: 0x1400003D RID: 61
	// (add) Token: 0x06000D8E RID: 3470 RVA: 0x000588E8 File Offset: 0x00056AE8
	// (remove) Token: 0x06000D8F RID: 3471 RVA: 0x0005891C File Offset: 0x00056B1C
	public static event EventManager.ObjectStateChange OnObjectStateChange;

	// Token: 0x06000D90 RID: 3472 RVA: 0x0005894F File Offset: 0x00056B4F
	public static void TriggerObjectStateChange(NetworkPhysicsObject newNPO, string oldGUID)
	{
		EventManager.ObjectStateChange onObjectStateChange = EventManager.OnObjectStateChange;
		if (onObjectStateChange == null)
		{
			return;
		}
		onObjectStateChange(newNPO, oldGUID);
	}

	// Token: 0x1400003E RID: 62
	// (add) Token: 0x06000D91 RID: 3473 RVA: 0x00058964 File Offset: 0x00056B64
	// (remove) Token: 0x06000D92 RID: 3474 RVA: 0x00058998 File Offset: 0x00056B98
	public static event EventManager.ObjectPageChange OnObjectPageChange;

	// Token: 0x06000D93 RID: 3475 RVA: 0x000589CB File Offset: 0x00056BCB
	public static void TriggerObjectPageChange(NetworkPhysicsObject NPO)
	{
		EventManager.ObjectPageChange onObjectPageChange = EventManager.OnObjectPageChange;
		if (onObjectPageChange == null)
		{
			return;
		}
		onObjectPageChange(NPO);
	}

	// Token: 0x1400003F RID: 63
	// (add) Token: 0x06000D94 RID: 3476 RVA: 0x000589E0 File Offset: 0x00056BE0
	// (remove) Token: 0x06000D95 RID: 3477 RVA: 0x00058A14 File Offset: 0x00056C14
	public static event EventManager.ObjectTagsChange OnObjectTagsChange;

	// Token: 0x06000D96 RID: 3478 RVA: 0x00058A47 File Offset: 0x00056C47
	public static void TriggerObjectTagsChange(NetworkPhysicsObject NPO, List<ulong> oldTags, List<ulong> newTags)
	{
		EventManager.ObjectTagsChange onObjectTagsChange = EventManager.OnObjectTagsChange;
		if (onObjectTagsChange == null)
		{
			return;
		}
		onObjectTagsChange(NPO, oldTags, newTags);
	}

	// Token: 0x14000040 RID: 64
	// (add) Token: 0x06000D97 RID: 3479 RVA: 0x00058A5C File Offset: 0x00056C5C
	// (remove) Token: 0x06000D98 RID: 3480 RVA: 0x00058A90 File Offset: 0x00056C90
	public static event EventManager.TagsChange OnAvailableTagsChange;

	// Token: 0x06000D99 RID: 3481 RVA: 0x00058AC3 File Offset: 0x00056CC3
	public static void TriggerAvailableTagsChange()
	{
		EventManager.TagsChange onAvailableTagsChange = EventManager.OnAvailableTagsChange;
		if (onAvailableTagsChange == null)
		{
			return;
		}
		onAvailableTagsChange();
	}

	// Token: 0x14000041 RID: 65
	// (add) Token: 0x06000D9A RID: 3482 RVA: 0x00058AD4 File Offset: 0x00056CD4
	// (remove) Token: 0x06000D9B RID: 3483 RVA: 0x00058B08 File Offset: 0x00056D08
	public static event EventManager.PlayersUpdate OnPlayersUpdate;

	// Token: 0x06000D9C RID: 3484 RVA: 0x00058B3B File Offset: 0x00056D3B
	public static void TriggerPlayersUpdate()
	{
		EventManager.PlayersUpdate onPlayersUpdate = EventManager.OnPlayersUpdate;
		if (onPlayersUpdate == null)
		{
			return;
		}
		onPlayersUpdate();
	}

	// Token: 0x14000042 RID: 66
	// (add) Token: 0x06000D9D RID: 3485 RVA: 0x00058B4C File Offset: 0x00056D4C
	// (remove) Token: 0x06000D9E RID: 3486 RVA: 0x00058B80 File Offset: 0x00056D80
	public static event EventManager.PlayersAdd OnPlayersAdd;

	// Token: 0x06000D9F RID: 3487 RVA: 0x00058BB3 File Offset: 0x00056DB3
	public static void TriggerPlayersAdd(PlayerState playerState)
	{
		EventManager.PlayersAdd onPlayersAdd = EventManager.OnPlayersAdd;
		if (onPlayersAdd == null)
		{
			return;
		}
		onPlayersAdd(playerState);
	}

	// Token: 0x14000043 RID: 67
	// (add) Token: 0x06000DA0 RID: 3488 RVA: 0x00058BC8 File Offset: 0x00056DC8
	// (remove) Token: 0x06000DA1 RID: 3489 RVA: 0x00058BFC File Offset: 0x00056DFC
	public static event EventManager.PlayersRemove OnPlayersRemove;

	// Token: 0x06000DA2 RID: 3490 RVA: 0x00058C2F File Offset: 0x00056E2F
	public static void TriggerPlayersRemove(PlayerState playerState)
	{
		EventManager.PlayersRemove onPlayersRemove = EventManager.OnPlayersRemove;
		if (onPlayersRemove == null)
		{
			return;
		}
		onPlayersRemove(playerState);
	}

	// Token: 0x14000044 RID: 68
	// (add) Token: 0x06000DA3 RID: 3491 RVA: 0x00058C44 File Offset: 0x00056E44
	// (remove) Token: 0x06000DA4 RID: 3492 RVA: 0x00058C78 File Offset: 0x00056E78
	public static event EventManager.PlayerChangeColor OnPlayerChangeColor;

	// Token: 0x06000DA5 RID: 3493 RVA: 0x00058CAB File Offset: 0x00056EAB
	public static void TriggerPlayerChangeColor(PlayerState playerState)
	{
		EventManager.PlayerChangeColor onPlayerChangeColor = EventManager.OnPlayerChangeColor;
		if (onPlayerChangeColor == null)
		{
			return;
		}
		onPlayerChangeColor(playerState);
	}

	// Token: 0x14000045 RID: 69
	// (add) Token: 0x06000DA6 RID: 3494 RVA: 0x00058CC0 File Offset: 0x00056EC0
	// (remove) Token: 0x06000DA7 RID: 3495 RVA: 0x00058CF4 File Offset: 0x00056EF4
	public static event EventManager.Chat OnChat;

	// Token: 0x06000DA8 RID: 3496 RVA: 0x00058D27 File Offset: 0x00056F27
	public static void TriggerChat(string message, int id)
	{
		EventManager.Chat onChat = EventManager.OnChat;
		if (onChat == null)
		{
			return;
		}
		onChat(message, id);
	}

	// Token: 0x14000046 RID: 70
	// (add) Token: 0x06000DA9 RID: 3497 RVA: 0x00058D3C File Offset: 0x00056F3C
	// (remove) Token: 0x06000DAA RID: 3498 RVA: 0x00058D70 File Offset: 0x00056F70
	public static event EventManager.ResetTable OnResetTable;

	// Token: 0x06000DAB RID: 3499 RVA: 0x00058DA3 File Offset: 0x00056FA3
	public static void TriggerResetTable()
	{
		EventManager.ResetTable onResetTable = EventManager.OnResetTable;
		if (onResetTable == null)
		{
			return;
		}
		onResetTable();
	}

	// Token: 0x14000047 RID: 71
	// (add) Token: 0x06000DAC RID: 3500 RVA: 0x00058DB4 File Offset: 0x00056FB4
	// (remove) Token: 0x06000DAD RID: 3501 RVA: 0x00058DE8 File Offset: 0x00056FE8
	public static event EventManager.VoiceTalk OnVoiceTalk;

	// Token: 0x06000DAE RID: 3502 RVA: 0x00058E1B File Offset: 0x0005701B
	public static void TriggerVoiceTalk(VoiceTalking talking)
	{
		EventManager.VoiceTalk onVoiceTalk = EventManager.OnVoiceTalk;
		if (onVoiceTalk == null)
		{
			return;
		}
		onVoiceTalk(talking);
	}

	// Token: 0x14000048 RID: 72
	// (add) Token: 0x06000DAF RID: 3503 RVA: 0x00058E30 File Offset: 0x00057030
	// (remove) Token: 0x06000DB0 RID: 3504 RVA: 0x00058E64 File Offset: 0x00057064
	public static event EventManager.PlayerMute OnPlayerMute;

	// Token: 0x06000DB1 RID: 3505 RVA: 0x00058E97 File Offset: 0x00057097
	public static void TriggerPlayerMute(bool muted, int id)
	{
		EventManager.PlayerMute onPlayerMute = EventManager.OnPlayerMute;
		if (onPlayerMute == null)
		{
			return;
		}
		onPlayerMute(muted, id);
	}

	// Token: 0x14000049 RID: 73
	// (add) Token: 0x06000DB2 RID: 3506 RVA: 0x00058EAC File Offset: 0x000570AC
	// (remove) Token: 0x06000DB3 RID: 3507 RVA: 0x00058EE0 File Offset: 0x000570E0
	public static event EventManager.EscapeMenu OnEscapeMenu;

	// Token: 0x06000DB4 RID: 3508 RVA: 0x00058F13 File Offset: 0x00057113
	public static void TriggerEscapeMenu(bool enable)
	{
		EventManager.EscapeMenu onEscapeMenu = EventManager.OnEscapeMenu;
		if (onEscapeMenu == null)
		{
			return;
		}
		onEscapeMenu(enable);
	}

	// Token: 0x1400004A RID: 74
	// (add) Token: 0x06000DB5 RID: 3509 RVA: 0x00058F28 File Offset: 0x00057128
	// (remove) Token: 0x06000DB6 RID: 3510 RVA: 0x00058F5C File Offset: 0x0005715C
	public static event EventManager.HideCustomUI OnHideCustomUI;

	// Token: 0x06000DB7 RID: 3511 RVA: 0x00058F8F File Offset: 0x0005718F
	public static void TriggerHideCustomUI(bool hide)
	{
		EventManager.HideCustomUI onHideCustomUI = EventManager.OnHideCustomUI;
		if (onHideCustomUI == null)
		{
			return;
		}
		onHideCustomUI(hide);
	}

	// Token: 0x1400004B RID: 75
	// (add) Token: 0x06000DB8 RID: 3512 RVA: 0x00058FA4 File Offset: 0x000571A4
	// (remove) Token: 0x06000DB9 RID: 3513 RVA: 0x00058FD8 File Offset: 0x000571D8
	public static event EventManager.LanguageChange OnLanguageChange;

	// Token: 0x06000DBA RID: 3514 RVA: 0x0005900B File Offset: 0x0005720B
	public static void TriggerLanguageChange(string previousLanguageCode, string newLanguageCode)
	{
		EventManager.LanguageChange onLanguageChange = EventManager.OnLanguageChange;
		if (onLanguageChange == null)
		{
			return;
		}
		onLanguageChange(previousLanguageCode, newLanguageCode);
	}

	// Token: 0x1400004C RID: 76
	// (add) Token: 0x06000DBB RID: 3515 RVA: 0x00059020 File Offset: 0x00057220
	// (remove) Token: 0x06000DBC RID: 3516 RVA: 0x00059054 File Offset: 0x00057254
	public static event EventManager.VersionNumberChange OnVersionNumberChange;

	// Token: 0x06000DBD RID: 3517 RVA: 0x00059087 File Offset: 0x00057287
	public static void TriggerVersionNumberChange(string oldVersion, string newVersion)
	{
		EventManager.VersionNumberChange onVersionNumberChange = EventManager.OnVersionNumberChange;
		if (onVersionNumberChange == null)
		{
			return;
		}
		onVersionNumberChange(oldVersion, newVersion);
	}

	// Token: 0x1400004D RID: 77
	// (add) Token: 0x06000DBE RID: 3518 RVA: 0x0005909C File Offset: 0x0005729C
	// (remove) Token: 0x06000DBF RID: 3519 RVA: 0x000590D0 File Offset: 0x000572D0
	public static event EventManager.ConfigSoundChange OnConfigSoundChange;

	// Token: 0x06000DC0 RID: 3520 RVA: 0x00059103 File Offset: 0x00057303
	public static void TriggerConfigSoundChange(ConfigSound.ConfigSoundState configSoundState)
	{
		EventManager.ConfigSoundChange onConfigSoundChange = EventManager.OnConfigSoundChange;
		if (onConfigSoundChange == null)
		{
			return;
		}
		onConfigSoundChange(configSoundState);
	}

	// Token: 0x1400004E RID: 78
	// (add) Token: 0x06000DC1 RID: 3521 RVA: 0x00059118 File Offset: 0x00057318
	// (remove) Token: 0x06000DC2 RID: 3522 RVA: 0x0005914C File Offset: 0x0005734C
	public static event EventManager.ZoomObjectChange OnZoomObjectChange;

	// Token: 0x06000DC3 RID: 3523 RVA: 0x0005917F File Offset: 0x0005737F
	public static void TriggerZoomObjectChange(GameObject zoomObject)
	{
		EventManager.ZoomObjectChange onZoomObjectChange = EventManager.OnZoomObjectChange;
		if (onZoomObjectChange == null)
		{
			return;
		}
		onZoomObjectChange(zoomObject);
	}

	// Token: 0x1400004F RID: 79
	// (add) Token: 0x06000DC4 RID: 3524 RVA: 0x00059194 File Offset: 0x00057394
	// (remove) Token: 0x06000DC5 RID: 3525 RVA: 0x000591C8 File Offset: 0x000573C8
	public static event EventManager.LateFixedUpdate OnLateFixedUpdate;

	// Token: 0x06000DC6 RID: 3526 RVA: 0x000591FB File Offset: 0x000573FB
	private static void TriggerLateFixedUpdate()
	{
		EventManager.LateFixedUpdate onLateFixedUpdate = EventManager.OnLateFixedUpdate;
		if (onLateFixedUpdate == null)
		{
			return;
		}
		onLateFixedUpdate();
	}

	// Token: 0x14000050 RID: 80
	// (add) Token: 0x06000DC7 RID: 3527 RVA: 0x0005920C File Offset: 0x0005740C
	// (remove) Token: 0x06000DC8 RID: 3528 RVA: 0x00059240 File Offset: 0x00057440
	public static event EventManager.ScreenDimensionsChange OnScreenDimensionsChange;

	// Token: 0x06000DC9 RID: 3529 RVA: 0x00059273 File Offset: 0x00057473
	private static void TriggerScreenDimensionsChange()
	{
		EventManager.ScreenDimensionsChange onScreenDimensionsChange = EventManager.OnScreenDimensionsChange;
		if (onScreenDimensionsChange == null)
		{
			return;
		}
		onScreenDimensionsChange();
	}

	// Token: 0x14000051 RID: 81
	// (add) Token: 0x06000DCA RID: 3530 RVA: 0x00059284 File Offset: 0x00057484
	// (remove) Token: 0x06000DCB RID: 3531 RVA: 0x000592B8 File Offset: 0x000574B8
	public static event EventManager.DummyObjectDestroy OnDummyObjectDestroy;

	// Token: 0x06000DCC RID: 3532 RVA: 0x000592EB File Offset: 0x000574EB
	public static void TriggerDummyObjectDestroy(DummyObject dummy)
	{
		EventManager.DummyObjectDestroy onDummyObjectDestroy = EventManager.OnDummyObjectDestroy;
		if (onDummyObjectDestroy == null)
		{
			return;
		}
		onDummyObjectDestroy(dummy);
	}

	// Token: 0x14000052 RID: 82
	// (add) Token: 0x06000DCD RID: 3533 RVA: 0x00059300 File Offset: 0x00057500
	// (remove) Token: 0x06000DCE RID: 3534 RVA: 0x00059334 File Offset: 0x00057534
	public static event EventManager.HandZoneChange OnHandZoneChange;

	// Token: 0x06000DCF RID: 3535 RVA: 0x00059367 File Offset: 0x00057567
	public static void TriggerHandZoneChange(HandZone handZone)
	{
		EventManager.HandZoneChange onHandZoneChange = EventManager.OnHandZoneChange;
		if (onHandZoneChange == null)
		{
			return;
		}
		onHandZoneChange(handZone);
	}

	// Token: 0x14000053 RID: 83
	// (add) Token: 0x06000DD0 RID: 3536 RVA: 0x0005937C File Offset: 0x0005757C
	// (remove) Token: 0x06000DD1 RID: 3537 RVA: 0x000593B0 File Offset: 0x000575B0
	public static event EventManager.PlayerAction OnPlayerAction;

	// Token: 0x06000DD2 RID: 3538 RVA: 0x000593E3 File Offset: 0x000575E3
	public static void CancelPlayerAction()
	{
		EventManager.playerActionPermitted = false;
	}

	// Token: 0x06000DD3 RID: 3539 RVA: 0x000593EB File Offset: 0x000575EB
	public static bool CheckPlayerAction(string playerColor, global::PlayerAction action, List<LuaGameObjectScript> targets, bool allowZeroTargets = false)
	{
		if (!allowZeroTargets && targets.Count == 0)
		{
			return false;
		}
		EventManager.playerActionPermitted = true;
		EventManager.PlayerAction onPlayerAction = EventManager.OnPlayerAction;
		if (onPlayerAction != null)
		{
			onPlayerAction(playerColor, action, targets);
		}
		return EventManager.playerActionPermitted;
	}

	// Token: 0x06000DD4 RID: 3540 RVA: 0x00059418 File Offset: 0x00057618
	public static bool CheckPlayerAction(string playerColor, global::PlayerAction action, LuaGameObjectScript target)
	{
		EventManager.singleObjectList[0] = target;
		bool result = EventManager.CheckPlayerAction(playerColor, action, EventManager.singleObjectList, false);
		EventManager.singleObjectList[0] = null;
		return result;
	}

	// Token: 0x06000DD5 RID: 3541 RVA: 0x0005943F File Offset: 0x0005763F
	private void Awake()
	{
		base.StartCoroutine(this.WaitEndFixedUpdate());
	}

	// Token: 0x06000DD6 RID: 3542 RVA: 0x0005944E File Offset: 0x0005764E
	private IEnumerator WaitEndFixedUpdate()
	{
		for (;;)
		{
			yield return this.waitForFixedUpdate;
			EventManager.TriggerLateFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06000DD7 RID: 3543 RVA: 0x0001CDD5 File Offset: 0x0001AFD5
	private static Vector2 GetScreenDimension()
	{
		return new Vector2((float)Screen.width, (float)Screen.height);
	}

	// Token: 0x06000DD8 RID: 3544 RVA: 0x0005945D File Offset: 0x0005765D
	private void Update()
	{
		if (this.screenDimensions != EventManager.GetScreenDimension())
		{
			this.screenDimensions = EventManager.GetScreenDimension();
			EventManager.TriggerScreenDimensionsChange();
		}
	}

	// Token: 0x040008A8 RID: 2216
	private static readonly Dictionary<string, object> EventData = new Dictionary<string, object>();

	// Token: 0x040008F7 RID: 2295
	private static bool playerActionPermitted;

	// Token: 0x040008F8 RID: 2296
	private static readonly List<LuaGameObjectScript> singleObjectList = new List<LuaGameObjectScript>(1)
	{
		null
	};

	// Token: 0x040008F9 RID: 2297
	private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

	// Token: 0x040008FA RID: 2298
	private Vector2 screenDimensions = EventManager.GetScreenDimension();

	// Token: 0x020005D8 RID: 1496
	// (Invoke) Token: 0x0600397F RID: 14719
	public delegate void UnityAnalytics(string eventName, IDictionary<string, object> eventData, int limit);

	// Token: 0x020005D9 RID: 1497
	// (Invoke) Token: 0x06003983 RID: 14723
	public delegate void UIThemeChange();

	// Token: 0x020005DA RID: 1498
	// (Invoke) Token: 0x06003987 RID: 14727
	public delegate void ChangePointerMode(PointerMode newMode);

	// Token: 0x020005DB RID: 1499
	// (Invoke) Token: 0x0600398B RID: 14731
	public delegate void ChangePlayerColor(Color newColor, int id);

	// Token: 0x020005DC RID: 1500
	// (Invoke) Token: 0x0600398F RID: 14735
	public delegate void ChangePlayerTeam(bool join, int id);

	// Token: 0x020005DD RID: 1501
	// (Invoke) Token: 0x06003993 RID: 14739
	public delegate void ChatMessageTypeEvent(ChatMessageType type);

	// Token: 0x020005DE RID: 1502
	// (Invoke) Token: 0x06003997 RID: 14743
	public delegate void ChatTyping(ChatMessageType type, bool typing);

	// Token: 0x020005DF RID: 1503
	// (Invoke) Token: 0x0600399B RID: 14747
	public delegate void PlayerChatTyping(PlayerState player, bool typing);

	// Token: 0x020005E0 RID: 1504
	// (Invoke) Token: 0x0600399F RID: 14751
	public delegate void AllowMessagesChanged(bool allow, ChatMessageType type);

	// Token: 0x020005E1 RID: 1505
	// (Invoke) Token: 0x060039A3 RID: 14755
	public delegate void AutoHideChatChanged(bool allow);

	// Token: 0x020005E2 RID: 1506
	// (Invoke) Token: 0x060039A7 RID: 14759
	public delegate void HandSelectModeEnd(string playerColor, string handSelectModeLabel, List<LuaGameObjectScript> chosenObjects, bool confirmed);

	// Token: 0x020005E3 RID: 1507
	// (Invoke) Token: 0x060039AB RID: 14763
	public delegate void LoadingComplete();

	// Token: 0x020005E4 RID: 1508
	// (Invoke) Token: 0x060039AF RID: 14767
	public delegate void LoadingChange(int NumDownloads, int NumComplete);

	// Token: 0x020005E5 RID: 1509
	// (Invoke) Token: 0x060039B3 RID: 14771
	public delegate void ZoneChangeAdd(NetworkPhysicsObject Zone, NetworkPhysicsObject ChangeObject);

	// Token: 0x020005E6 RID: 1510
	// (Invoke) Token: 0x060039B7 RID: 14775
	public delegate void ZoneChangeRemove(NetworkPhysicsObject Zone, NetworkPhysicsObject ChangeObject);

	// Token: 0x020005E7 RID: 1511
	// (Invoke) Token: 0x060039BB RID: 14779
	public delegate void NetworkObjectSpawn(NetworkPhysicsObject NPO);

	// Token: 0x020005E8 RID: 1512
	// (Invoke) Token: 0x060039BF RID: 14783
	public delegate void NetworkObjectSpawnFromUI(NetworkPhysicsObject NPO);

	// Token: 0x020005E9 RID: 1513
	// (Invoke) Token: 0x060039C3 RID: 14787
	public delegate void NetworkObjectDestroy(NetworkPhysicsObject NPO);

	// Token: 0x020005EA RID: 1514
	// (Invoke) Token: 0x060039C7 RID: 14791
	public delegate void NetworkObjectHide(NetworkPhysicsObject NPO, bool bHide);

	// Token: 0x020005EB RID: 1515
	// (Invoke) Token: 0x060039CB RID: 14795
	public delegate void LuaObjectSpawn(LuaGameObjectScript LGOS);

	// Token: 0x020005EC RID: 1516
	// (Invoke) Token: 0x060039CF RID: 14799
	public delegate void LuaObjectDestroy(LuaGameObjectScript LGOS);

	// Token: 0x020005ED RID: 1517
	// (Invoke) Token: 0x060039D3 RID: 14803
	public delegate void FogOfWarRevealerAdd(GameObject fogOfWarRevealer);

	// Token: 0x020005EE RID: 1518
	// (Invoke) Token: 0x060039D7 RID: 14807
	public delegate void FogOfWarRevealerDestroy(GameObject fogOfWarRevealer);

	// Token: 0x020005EF RID: 1519
	// (Invoke) Token: 0x060039DB RID: 14811
	public delegate void PlayerTurnStart(string EndColor, string StartColor);

	// Token: 0x020005F0 RID: 1520
	// (Invoke) Token: 0x060039DF RID: 14815
	public delegate void PlayerTurnEnd(string EndColor, string StartColor);

	// Token: 0x020005F1 RID: 1521
	// (Invoke) Token: 0x060039E3 RID: 14819
	public delegate void PlayerPing(string playerColor, Vector3 position, LuaGameObjectScript hoveredObject);

	// Token: 0x020005F2 RID: 1522
	// (Invoke) Token: 0x060039E7 RID: 14823
	public delegate void GameRoundStart();

	// Token: 0x020005F3 RID: 1523
	// (Invoke) Token: 0x060039EB RID: 14827
	public delegate void GameRoundEnd();

	// Token: 0x020005F4 RID: 1524
	// (Invoke) Token: 0x060039EF RID: 14831
	public delegate void ObjectHover(GameObject HoverObject, Color Player);

	// Token: 0x020005F5 RID: 1525
	// (Invoke) Token: 0x060039F3 RID: 14835
	public delegate void ObjectPickUp(NetworkPhysicsObject PickUpObject, PlayerState Player);

	// Token: 0x020005F6 RID: 1526
	// (Invoke) Token: 0x060039F7 RID: 14839
	public delegate void ObjectDrop(NetworkPhysicsObject DropObject, PlayerState LastPlayerToHold);

	// Token: 0x020005F7 RID: 1527
	// (Invoke) Token: 0x060039FB RID: 14843
	public delegate void ObjectFinishSmoothMove(NetworkPhysicsObject NPO);

	// Token: 0x020005F8 RID: 1528
	// (Invoke) Token: 0x060039FF RID: 14847
	public delegate void CloudUploadFinish(string name, string url);

	// Token: 0x020005F9 RID: 1529
	// (Invoke) Token: 0x06003A03 RID: 14851
	public delegate void GameSave();

	// Token: 0x020005FA RID: 1530
	// (Invoke) Token: 0x06003A07 RID: 14855
	public delegate void Blindfold(bool bBlind, int id);

	// Token: 0x020005FB RID: 1531
	// (Invoke) Token: 0x06003A0B RID: 14859
	public delegate void DummyObjectFinish(GameObject dummyGameObject);

	// Token: 0x020005FC RID: 1532
	// (Invoke) Token: 0x06003A0F RID: 14863
	public delegate void PlayerPromoted(bool isPromoted, int id);

	// Token: 0x020005FD RID: 1533
	// (Invoke) Token: 0x06003A13 RID: 14867
	public delegate void CanFlipTable(bool bCanFlipTable);

	// Token: 0x020005FE RID: 1534
	// (Invoke) Token: 0x06003A17 RID: 14871
	public delegate void ChangeTable(GameObject Table);

	// Token: 0x020005FF RID: 1535
	// (Invoke) Token: 0x06003A1B RID: 14875
	public delegate void WorkshopUpToDate();

	// Token: 0x02000600 RID: 1536
	// (Invoke) Token: 0x06003A1F RID: 14879
	public delegate void FileSave(string Path);

	// Token: 0x02000601 RID: 1537
	// (Invoke) Token: 0x06003A23 RID: 14883
	public delegate void FileDelete(string Path);

	// Token: 0x02000602 RID: 1538
	// (Invoke) Token: 0x06003A27 RID: 14887
	public delegate void FocusUI(UIDragObject FocusDragObject);

	// Token: 0x02000603 RID: 1539
	// (Invoke) Token: 0x06003A2B RID: 14891
	public delegate void ObjectEnterContainer(NetworkPhysicsObject Container, NetworkPhysicsObject Object);

	// Token: 0x02000604 RID: 1540
	// (Invoke) Token: 0x06003A2F RID: 14895
	public delegate void ObjectLeaveContainer(NetworkPhysicsObject Container, NetworkPhysicsObject Object);

	// Token: 0x02000605 RID: 1541
	// (Invoke) Token: 0x06003A33 RID: 14899
	public delegate void ScriptingButtonDown(int index, string playerColor);

	// Token: 0x02000606 RID: 1542
	// (Invoke) Token: 0x06003A37 RID: 14903
	public delegate void ScriptingButtonUp(int index, string playerColor);

	// Token: 0x02000607 RID: 1543
	// (Invoke) Token: 0x06003A3B RID: 14907
	public delegate void ObjectTriggerEffect(NetworkPhysicsObject NPO, int index);

	// Token: 0x02000608 RID: 1544
	// (Invoke) Token: 0x06003A3F RID: 14911
	public delegate void ObjectLoopingEffect(NetworkPhysicsObject NPO, int index);

	// Token: 0x02000609 RID: 1545
	// (Invoke) Token: 0x06003A43 RID: 14915
	public delegate void VR(bool bEnabled);

	// Token: 0x0200060A RID: 1546
	// (Invoke) Token: 0x06003A47 RID: 14919
	public delegate void ObjectRandomize(NetworkPhysicsObject NPO, string playerColor);

	// Token: 0x0200060B RID: 1547
	// (Invoke) Token: 0x06003A4B RID: 14923
	public delegate void ObjectRotate(NetworkPhysicsObject NPO, int spinIndex, int flipIndex, string playerColor, int previousSpinIndex, int previousFlipIndex);

	// Token: 0x0200060C RID: 1548
	// (Invoke) Token: 0x06003A4F RID: 14927
	public delegate void ObjectSearchStart(NetworkPhysicsObject NPO, string playerColor);

	// Token: 0x0200060D RID: 1549
	// (Invoke) Token: 0x06003A53 RID: 14931
	public delegate void ObjectSearchEnd(NetworkPhysicsObject NPO, string playerColor);

	// Token: 0x0200060E RID: 1550
	// (Invoke) Token: 0x06003A57 RID: 14935
	public delegate void ObjectPeek(NetworkPhysicsObject NPO, string playerColor);

	// Token: 0x0200060F RID: 1551
	// (Invoke) Token: 0x06003A5B RID: 14939
	public delegate void ObjectFlick(NetworkPhysicsObject NPO, string playerColor, Vector3 force);

	// Token: 0x02000610 RID: 1552
	// (Invoke) Token: 0x06003A5F RID: 14943
	public delegate void ObjectStateChange(NetworkPhysicsObject newNPO, string oldGUID);

	// Token: 0x02000611 RID: 1553
	// (Invoke) Token: 0x06003A63 RID: 14947
	public delegate void ObjectPageChange(NetworkPhysicsObject NPO);

	// Token: 0x02000612 RID: 1554
	// (Invoke) Token: 0x06003A67 RID: 14951
	public delegate void ObjectTagsChange(NetworkPhysicsObject NPO, List<ulong> oldTags, List<ulong> newTags);

	// Token: 0x02000613 RID: 1555
	// (Invoke) Token: 0x06003A6B RID: 14955
	public delegate void TagsChange();

	// Token: 0x02000614 RID: 1556
	// (Invoke) Token: 0x06003A6F RID: 14959
	public delegate void PlayersUpdate();

	// Token: 0x02000615 RID: 1557
	// (Invoke) Token: 0x06003A73 RID: 14963
	public delegate void PlayersAdd(PlayerState playerState);

	// Token: 0x02000616 RID: 1558
	// (Invoke) Token: 0x06003A77 RID: 14967
	public delegate void PlayersRemove(PlayerState playerState);

	// Token: 0x02000617 RID: 1559
	// (Invoke) Token: 0x06003A7B RID: 14971
	public delegate void PlayerChangeColor(PlayerState playerState);

	// Token: 0x02000618 RID: 1560
	// (Invoke) Token: 0x06003A7F RID: 14975
	public delegate void Chat(string message, int id);

	// Token: 0x02000619 RID: 1561
	// (Invoke) Token: 0x06003A83 RID: 14979
	public delegate void ResetTable();

	// Token: 0x0200061A RID: 1562
	// (Invoke) Token: 0x06003A87 RID: 14983
	public delegate void VoiceTalk(VoiceTalking talking);

	// Token: 0x0200061B RID: 1563
	// (Invoke) Token: 0x06003A8B RID: 14987
	public delegate void PlayerMute(bool muted, int id);

	// Token: 0x0200061C RID: 1564
	// (Invoke) Token: 0x06003A8F RID: 14991
	public delegate void EscapeMenu(bool enable);

	// Token: 0x0200061D RID: 1565
	// (Invoke) Token: 0x06003A93 RID: 14995
	public delegate void HideCustomUI(bool hide);

	// Token: 0x0200061E RID: 1566
	// (Invoke) Token: 0x06003A97 RID: 14999
	public delegate void LanguageChange(string previousLanguageCode, string newLanguageCode);

	// Token: 0x0200061F RID: 1567
	// (Invoke) Token: 0x06003A9B RID: 15003
	public delegate void VersionNumberChange(string oldVersion, string newVersion);

	// Token: 0x02000620 RID: 1568
	// (Invoke) Token: 0x06003A9F RID: 15007
	public delegate void ConfigSoundChange(ConfigSound.ConfigSoundState configSoundState);

	// Token: 0x02000621 RID: 1569
	// (Invoke) Token: 0x06003AA3 RID: 15011
	public delegate void ZoomObjectChange(GameObject zoomObject);

	// Token: 0x02000622 RID: 1570
	// (Invoke) Token: 0x06003AA7 RID: 15015
	public delegate void LateFixedUpdate();

	// Token: 0x02000623 RID: 1571
	// (Invoke) Token: 0x06003AAB RID: 15019
	public delegate void ScreenDimensionsChange();

	// Token: 0x02000624 RID: 1572
	// (Invoke) Token: 0x06003AAF RID: 15023
	public delegate void DummyObjectDestroy(DummyObject dummy);

	// Token: 0x02000625 RID: 1573
	// (Invoke) Token: 0x06003AB3 RID: 15027
	public delegate void HandZoneChange(HandZone handZone);

	// Token: 0x02000626 RID: 1574
	// (Invoke) Token: 0x06003AB7 RID: 15031
	public delegate void PlayerAction(string playerColor, global::PlayerAction action, List<LuaGameObjectScript> targets);
}
