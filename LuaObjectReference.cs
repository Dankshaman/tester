using System;
using MoonSharp.Interpreter;

// Token: 0x0200017A RID: 378
public class LuaObjectReference
{
	// Token: 0x170002F9 RID: 761
	// (get) Token: 0x060011A8 RID: 4520 RVA: 0x000793A9 File Offset: 0x000775A9
	// (set) Token: 0x060011A9 RID: 4521 RVA: 0x000793B1 File Offset: 0x000775B1
	public string name { get; protected set; }

	// Token: 0x060011AA RID: 4522 RVA: 0x000793BA File Offset: 0x000775BA
	public override string ToString()
	{
		return this.name;
	}

	// Token: 0x04000B65 RID: 2917
	[MoonSharpHidden]
	protected LuaGameObjectScript luaGameObjectScript;
}
