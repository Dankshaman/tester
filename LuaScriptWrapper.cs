using System;
using MoonSharp.Interpreter;

// Token: 0x02000198 RID: 408
public class LuaScriptWrapper
{
	// Token: 0x06001416 RID: 5142 RVA: 0x00084B3F File Offset: 0x00082D3F
	public LuaScriptWrapper(LuaGameObjectScript script)
	{
		this.script = script;
	}

	// Token: 0x06001417 RID: 5143 RVA: 0x00084B4E File Offset: 0x00082D4E
	public DynValue init(params object[] param)
	{
		return this.script.Call("init", param);
	}

	// Token: 0x04000BC5 RID: 3013
	[MoonSharpHidden]
	private LuaGameObjectScript script;
}
