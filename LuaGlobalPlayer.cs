using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

// Token: 0x0200018B RID: 395
public class LuaGlobalPlayer : IUserDataType
{
	// Token: 0x0600137C RID: 4988 RVA: 0x0008270C File Offset: 0x0008090C
	[MoonSharpHidden]
	public LuaGlobalPlayer()
	{
		foreach (string text in Colour.HandPlayerLabels)
		{
			DynValue value = UserData.Create(LuaPlayer.GetHandPlayer(text));
			string key = char.ToLowerInvariant(text[0]).ToString() + text.Substring(1);
			this.handPlayers[text] = value;
			this.handPlayers[key] = value;
		}
	}

	// Token: 0x0600137D RID: 4989 RVA: 0x00082797 File Offset: 0x00080997
	[MoonSharpHidden]
	public LuaPlayer GetPlayer(int id)
	{
		return new LuaPlayer(NetworkSingleton<PlayerManager>.Instance.PlayerStateFromID(id).stringColor, id);
	}

	// Token: 0x0600137E RID: 4990 RVA: 0x000827B0 File Offset: 0x000809B0
	public List<LuaPlayer> GetPlayers()
	{
		List<LuaPlayer> list = new List<LuaPlayer>();
		List<PlayerState> playersList = NetworkSingleton<PlayerManager>.Instance.PlayersList;
		for (int i = 0; i < playersList.Count; i++)
		{
			list.Add(new LuaPlayer(playersList[i].stringColor, playersList[i].id));
		}
		return list;
	}

	// Token: 0x0600137F RID: 4991 RVA: 0x00082804 File Offset: 0x00080A04
	public List<LuaPlayer> GetSpectators()
	{
		List<LuaPlayer> list = new List<LuaPlayer>();
		List<PlayerState> playersList = NetworkSingleton<PlayerManager>.Instance.PlayersList;
		for (int i = 0; i < playersList.Count; i++)
		{
			if (playersList[i].stringColor == "Grey")
			{
				list.Add(new LuaPlayer(playersList[i].stringColor, playersList[i].id));
			}
		}
		return list;
	}

	// Token: 0x06001380 RID: 4992 RVA: 0x00082870 File Offset: 0x00080A70
	public List<string> GetAvailableColors()
	{
		List<string> list = new List<string>();
		List<HandZone> handZones = HandZone.GetHandZones();
		for (int i = 0; i < Colour.HandPlayerLabels.Length; i++)
		{
			foreach (HandZone handZone in handZones)
			{
				if (handZone.TriggerLabel == Colour.HandPlayerLabels[i])
				{
					list.Add(handZone.TriggerLabel);
					break;
				}
			}
		}
		return list;
	}

	// Token: 0x06001381 RID: 4993 RVA: 0x000828FC File Offset: 0x00080AFC
	public string[] GetColors()
	{
		return Colour.AllPlayerLabels;
	}

	// Token: 0x06001382 RID: 4994 RVA: 0x00082904 File Offset: 0x00080B04
	[MoonSharpHidden]
	public DynValue Index(Script script, DynValue index, bool isDirectIndexing)
	{
		if (index.Type != DataType.String)
		{
			return null;
		}
		DynValue result;
		if (!this.handPlayers.TryGetValue(index.String, out result))
		{
			return null;
		}
		return result;
	}

	// Token: 0x06001383 RID: 4995 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
	[MoonSharpHidden]
	public bool SetIndex(Script script, DynValue index, DynValue value, bool isDirectIndexing)
	{
		return false;
	}

	// Token: 0x06001384 RID: 4996 RVA: 0x00079594 File Offset: 0x00077794
	[MoonSharpHidden]
	public DynValue MetaIndex(Script script, string metaname)
	{
		return null;
	}

	// Token: 0x04000BA7 RID: 2983
	public static IUserDataDescriptor UserDataDescriptor = new CompositeUserDataDescriptor(new List<IUserDataDescriptor>
	{
		new StandardUserDataDescriptor(typeof(LuaGlobalPlayer), InteropAccessMode.Default, null),
		new AutoDescribingUserDataDescriptor(typeof(LuaGlobalPlayer), null)
	}, typeof(LuaGlobalPlayer));

	// Token: 0x04000BA8 RID: 2984
	public readonly LuaGlobalPlayer.LuaAction Action = new LuaGlobalPlayer.LuaAction();

	// Token: 0x04000BA9 RID: 2985
	private readonly Dictionary<string, DynValue> handPlayers = new Dictionary<string, DynValue>();

	// Token: 0x0200066D RID: 1645
	public class LuaAction : LuaEnum
	{
		// Token: 0x06003B75 RID: 15221 RVA: 0x001771D4 File Offset: 0x001753D4
		internal LuaAction() : base(typeof(PlayerAction))
		{
		}
	}
}
