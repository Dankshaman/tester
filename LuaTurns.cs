using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using NewNet;

// Token: 0x0200018F RID: 399
public class LuaTurns
{
	// Token: 0x17000357 RID: 855
	// (get) Token: 0x060013BB RID: 5051 RVA: 0x00083907 File Offset: 0x00081B07
	// (set) Token: 0x060013BC RID: 5052 RVA: 0x00083913 File Offset: 0x00081B13
	[MoonSharpHidden]
	private TurnsState turnsState
	{
		get
		{
			return NetworkSingleton<Turns>.Instance.turnsState;
		}
		set
		{
			NetworkSingleton<Turns>.Instance.turnsState = value;
		}
	}

	// Token: 0x17000358 RID: 856
	// (get) Token: 0x060013BD RID: 5053 RVA: 0x00083920 File Offset: 0x00081B20
	// (set) Token: 0x060013BE RID: 5054 RVA: 0x0008393C File Offset: 0x00081B3C
	public bool enable
	{
		get
		{
			return this.turnsState.Enable || NetworkSingleton<NetworkUI>.Instance.bHotseat;
		}
		set
		{
			this.turnsState = new TurnsState(this.turnsState, new bool?(value || NetworkSingleton<NetworkUI>.Instance.bHotseat), null, null, null, null, null, null, null);
		}
	}

	// Token: 0x17000359 RID: 857
	// (get) Token: 0x060013BF RID: 5055 RVA: 0x0008399E File Offset: 0x00081B9E
	// (set) Token: 0x060013C0 RID: 5056 RVA: 0x000839B0 File Offset: 0x00081BB0
	public int type
	{
		get
		{
			return (int)(this.turnsState.Type + 1);
		}
		set
		{
			this.turnsState = new TurnsState(this.turnsState, null, new TurnType?((TurnType)(value - 1)), null, null, null, null, null, null);
		}
	}

	// Token: 0x1700035A RID: 858
	// (get) Token: 0x060013C1 RID: 5057 RVA: 0x00083A05 File Offset: 0x00081C05
	// (set) Token: 0x060013C2 RID: 5058 RVA: 0x00083A14 File Offset: 0x00081C14
	public List<string> order
	{
		get
		{
			return this.turnsState.TurnOrder;
		}
		set
		{
			this.turnsState = new TurnsState(this.turnsState, null, new TurnType?(TurnType.Custom), value, null, null, null, null, null);
		}
	}

	// Token: 0x1700035B RID: 859
	// (get) Token: 0x060013C3 RID: 5059 RVA: 0x00083A69 File Offset: 0x00081C69
	// (set) Token: 0x060013C4 RID: 5060 RVA: 0x00083A78 File Offset: 0x00081C78
	public bool reverse_order
	{
		get
		{
			return this.turnsState.Reverse;
		}
		set
		{
			this.turnsState = new TurnsState(this.turnsState, null, null, null, new bool?(value), null, null, null, null);
		}
	}

	// Token: 0x1700035C RID: 860
	// (get) Token: 0x060013C5 RID: 5061 RVA: 0x00083ACB File Offset: 0x00081CCB
	// (set) Token: 0x060013C6 RID: 5062 RVA: 0x00083AD8 File Offset: 0x00081CD8
	public bool skip_empty_hands
	{
		get
		{
			return this.turnsState.SkipEmpty;
		}
		set
		{
			this.turnsState = new TurnsState(this.turnsState, null, null, null, null, new bool?(value), null, null, null);
		}
	}

	// Token: 0x1700035D RID: 861
	// (get) Token: 0x060013C7 RID: 5063 RVA: 0x00083B2B File Offset: 0x00081D2B
	// (set) Token: 0x060013C8 RID: 5064 RVA: 0x00083B38 File Offset: 0x00081D38
	public bool disable_interactations
	{
		get
		{
			return this.turnsState.DisableInteractions;
		}
		set
		{
			this.turnsState = new TurnsState(this.turnsState, null, null, null, null, null, new bool?(value), null, null);
		}
	}

	// Token: 0x1700035E RID: 862
	// (get) Token: 0x060013C9 RID: 5065 RVA: 0x00083B8B File Offset: 0x00081D8B
	// (set) Token: 0x060013CA RID: 5066 RVA: 0x00083B98 File Offset: 0x00081D98
	public bool pass_turns
	{
		get
		{
			return this.turnsState.PassTurns;
		}
		set
		{
			this.turnsState = new TurnsState(this.turnsState, null, null, null, null, null, null, new bool?(value), null);
		}
	}

	// Token: 0x1700035F RID: 863
	// (get) Token: 0x060013CB RID: 5067 RVA: 0x00083BEB File Offset: 0x00081DEB
	// (set) Token: 0x060013CC RID: 5068 RVA: 0x00083BF8 File Offset: 0x00081DF8
	public string turn_color
	{
		get
		{
			return this.turnsState.TurnColor;
		}
		set
		{
			NetworkSingleton<Turns>.Instance.SetPlayerTurnByColourLabel(value, RPCTarget.All);
		}
	}

	// Token: 0x060013CD RID: 5069 RVA: 0x00083C07 File Offset: 0x00081E07
	public string getNextTurnColor()
	{
		return NetworkSingleton<Turns>.Instance.GetNextTurnColor();
	}

	// Token: 0x060013CE RID: 5070 RVA: 0x00083C13 File Offset: 0x00081E13
	public string getPreviousTurnColor()
	{
		return NetworkSingleton<Turns>.Instance.GetPreviousTurnColor();
	}
}
