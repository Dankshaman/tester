using System;
using MoonSharp.Interpreter;

// Token: 0x02000169 RID: 361
public class LuaComponent
{
	// Token: 0x06001170 RID: 4464 RVA: 0x00078C98 File Offset: 0x00076E98
	[MoonSharpHidden]
	public LuaComponent(LuaGameObjectScript LGOS)
	{
		this.LGOS = LGOS;
	}

	// Token: 0x04000B47 RID: 2887
	[MoonSharpHidden]
	public LuaGameObjectScript LGOS;
}
