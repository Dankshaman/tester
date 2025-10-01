using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;

// Token: 0x0200017D RID: 381
public class LuaMaterialReference : LuaObjectReference
{
	// Token: 0x170002FB RID: 763
	// (get) Token: 0x060011BB RID: 4539 RVA: 0x00079514 File Offset: 0x00077714
	public LuaGameObjectReference game_object { get; }

	// Token: 0x170002FC RID: 764
	// (get) Token: 0x060011BC RID: 4540 RVA: 0x0007951C File Offset: 0x0007771C
	public string shader
	{
		get
		{
			return this.material.shader.name;
		}
	}

	// Token: 0x060011BD RID: 4541 RVA: 0x0007952E File Offset: 0x0007772E
	[MoonSharpHidden]
	public LuaMaterialReference(LuaGameObjectScript luaGameObjectScript, Material material, LuaGameObjectReference game_object)
	{
		this.luaGameObjectScript = luaGameObjectScript;
		this.material = material;
		base.name = material.name;
		this.game_object = game_object;
	}

	// Token: 0x060011BE RID: 4542 RVA: 0x00079557 File Offset: 0x00077757
	public DynValue get(string varName, Script script)
	{
		return this.luaGameObjectScript.getMaterialValue(this.material, varName, script);
	}

	// Token: 0x060011BF RID: 4543 RVA: 0x0007956C File Offset: 0x0007776C
	public bool set(string varName, object value)
	{
		return this.luaGameObjectScript.setMaterialValue(this.material, varName, value);
	}

	// Token: 0x060011C0 RID: 4544 RVA: 0x00079581 File Offset: 0x00077781
	public Dictionary<string, string> getVars()
	{
		return this.luaGameObjectScript.getMaterialVars(this.material);
	}

	// Token: 0x04000B6A RID: 2922
	[MoonSharpHidden]
	private Material material;
}
