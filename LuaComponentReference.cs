using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;

// Token: 0x0200017C RID: 380
public class LuaComponentReference : LuaObjectReference
{
	// Token: 0x170002FA RID: 762
	// (get) Token: 0x060011B5 RID: 4533 RVA: 0x00079481 File Offset: 0x00077681
	public LuaGameObjectReference game_object { get; }

	// Token: 0x060011B6 RID: 4534 RVA: 0x00079489 File Offset: 0x00077689
	[MoonSharpHidden]
	public LuaComponentReference(LuaGameObjectScript luaGameObjectScript, Component component)
	{
		this.luaGameObjectScript = luaGameObjectScript;
		this.component = component;
		base.name = component.GetType().Name;
		this.game_object = new LuaGameObjectReference(luaGameObjectScript, component.gameObject);
	}

	// Token: 0x060011B7 RID: 4535 RVA: 0x000794C2 File Offset: 0x000776C2
	public DynValue get(string varName, Script script)
	{
		return this.luaGameObjectScript.getComponentVar(this.component, varName, script);
	}

	// Token: 0x060011B8 RID: 4536 RVA: 0x000794D7 File Offset: 0x000776D7
	public bool set(string varName, object value)
	{
		return this.luaGameObjectScript.setComponentVar(this.component, varName, value);
	}

	// Token: 0x060011B9 RID: 4537 RVA: 0x000794EC File Offset: 0x000776EC
	public Dictionary<string, string> getVars()
	{
		return this.luaGameObjectScript.getComponentVars(this.component);
	}

	// Token: 0x060011BA RID: 4538 RVA: 0x000794FF File Offset: 0x000776FF
	public DynValue call(string funcName, params object[] parameters)
	{
		return this.luaGameObjectScript.callComponentFunc(base.name, funcName, parameters);
	}

	// Token: 0x04000B68 RID: 2920
	[MoonSharpHidden]
	private Component component;
}
