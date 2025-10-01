using System;
using MoonSharp.Interpreter;

// Token: 0x0200016B RID: 363
public class LuaCounter : LuaComponent
{
	// Token: 0x06001179 RID: 4473 RVA: 0x00078CA7 File Offset: 0x00076EA7
	[MoonSharpHidden]
	public LuaCounter(LuaGameObjectScript LGOS) : base(LGOS)
	{
	}

	// Token: 0x0600117A RID: 4474 RVA: 0x00078CE8 File Offset: 0x00076EE8
	public DynValue getValue()
	{
		return this.LGOS.GetValue();
	}

	// Token: 0x0600117B RID: 4475 RVA: 0x00078CF5 File Offset: 0x00076EF5
	public bool setValue(DynValue Value)
	{
		return this.LGOS.SetValue(Value);
	}

	// Token: 0x0600117C RID: 4476 RVA: 0x00078D4B File Offset: 0x00076F4B
	public bool clear()
	{
		this.LGOS.NPO.counterScript.Clear();
		return true;
	}

	// Token: 0x0600117D RID: 4477 RVA: 0x00078D63 File Offset: 0x00076F63
	public bool increment()
	{
		this.LGOS.NPO.counterScript.Increment();
		return true;
	}

	// Token: 0x0600117E RID: 4478 RVA: 0x00078D7B File Offset: 0x00076F7B
	public bool decrement()
	{
		this.LGOS.NPO.counterScript.Decrement();
		return true;
	}
}
