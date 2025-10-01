using System;
using MoonSharp.Interpreter;

// Token: 0x0200016A RID: 362
public class LuaClock : LuaComponent
{
	// Token: 0x06001171 RID: 4465 RVA: 0x00078CA7 File Offset: 0x00076EA7
	[MoonSharpHidden]
	public LuaClock(LuaGameObjectScript LGOS) : base(LGOS)
	{
	}

	// Token: 0x170002F4 RID: 756
	// (get) Token: 0x06001172 RID: 4466 RVA: 0x00078CB0 File Offset: 0x00076EB0
	// (set) Token: 0x06001173 RID: 4467 RVA: 0x00078CC7 File Offset: 0x00076EC7
	public bool paused
	{
		get
		{
			return this.LGOS.NPO.clockScript.bPaused;
		}
		set
		{
			if (value != this.LGOS.NPO.clockScript.bPaused)
			{
				this.pauseStart();
			}
		}
	}

	// Token: 0x06001174 RID: 4468 RVA: 0x00078CE8 File Offset: 0x00076EE8
	public DynValue getValue()
	{
		return this.LGOS.GetValue();
	}

	// Token: 0x06001175 RID: 4469 RVA: 0x00078CF5 File Offset: 0x00076EF5
	public bool setValue(DynValue Value)
	{
		return this.LGOS.SetValue(Value);
	}

	// Token: 0x06001176 RID: 4470 RVA: 0x00078D03 File Offset: 0x00076F03
	public bool showCurrentTime()
	{
		this.LGOS.NPO.clockScript.StartCurrentTime();
		return true;
	}

	// Token: 0x06001177 RID: 4471 RVA: 0x00078D1B File Offset: 0x00076F1B
	public bool pauseStart()
	{
		this.LGOS.NPO.clockScript.PauseStart();
		return true;
	}

	// Token: 0x06001178 RID: 4472 RVA: 0x00078D33 File Offset: 0x00076F33
	public bool startStopwatch()
	{
		this.LGOS.NPO.clockScript.StartStopwatch();
		return true;
	}
}
