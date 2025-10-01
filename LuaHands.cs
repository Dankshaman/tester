using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;

// Token: 0x0200019A RID: 410
public class LuaHands
{
	// Token: 0x1700036E RID: 878
	// (get) Token: 0x0600141E RID: 5150 RVA: 0x00084B84 File Offset: 0x00082D84
	// (set) Token: 0x0600141F RID: 5151 RVA: 0x00084B90 File Offset: 0x00082D90
	[MoonSharpHidden]
	private HandsState handsState
	{
		get
		{
			return NetworkSingleton<Hands>.Instance.handsState;
		}
		set
		{
			NetworkSingleton<Hands>.Instance.handsState = value;
		}
	}

	// Token: 0x1700036F RID: 879
	// (get) Token: 0x06001420 RID: 5152 RVA: 0x00084B9D File Offset: 0x00082D9D
	// (set) Token: 0x06001421 RID: 5153 RVA: 0x00084BAC File Offset: 0x00082DAC
	public bool enable
	{
		get
		{
			return this.handsState.Enable;
		}
		set
		{
			this.handsState = new HandsState(this.handsState, new bool?(value), null, null, null);
		}
	}

	// Token: 0x17000370 RID: 880
	// (get) Token: 0x06001422 RID: 5154 RVA: 0x00084BE3 File Offset: 0x00082DE3
	// (set) Token: 0x06001423 RID: 5155 RVA: 0x00084BF0 File Offset: 0x00082DF0
	public bool disable_unused
	{
		get
		{
			return this.handsState.DisableUnused;
		}
		set
		{
			this.handsState = new HandsState(this.handsState, null, new bool?(value), null, null);
		}
	}

	// Token: 0x17000371 RID: 881
	// (get) Token: 0x06001424 RID: 5156 RVA: 0x00084C27 File Offset: 0x00082E27
	// (set) Token: 0x06001425 RID: 5157 RVA: 0x00084C38 File Offset: 0x00082E38
	public int hiding
	{
		get
		{
			return (int)(this.handsState.Hiding + 1);
		}
		set
		{
			this.handsState = new HandsState(this.handsState, null, null, new HidingType?((HidingType)(value - 1)), null);
		}
	}

	// Token: 0x06001426 RID: 5158 RVA: 0x00084C74 File Offset: 0x00082E74
	public List<LuaGameObjectScript> GetHands()
	{
		List<HandZone> handZones = HandZone.GetHandZones();
		List<LuaGameObjectScript> list = new List<LuaGameObjectScript>(handZones.Count);
		foreach (HandZone handZone in handZones)
		{
			list.Add(handZone.NPO.luaGameObjectScript);
		}
		return list;
	}
}
