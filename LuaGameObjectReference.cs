using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;

// Token: 0x0200017B RID: 379
public class LuaGameObjectReference : LuaObjectReference
{
	// Token: 0x060011AC RID: 4524 RVA: 0x000793C2 File Offset: 0x000775C2
	[MoonSharpHidden]
	public LuaGameObjectReference(LuaGameObjectScript luaGameObjectScript, GameObject gameObject)
	{
		this.luaGameObjectScript = luaGameObjectScript;
		this.gameObject = gameObject;
		base.name = gameObject.name;
	}

	// Token: 0x060011AD RID: 4525 RVA: 0x000793E4 File Offset: 0x000775E4
	public LuaGameObjectReference getChild(string name)
	{
		return this.luaGameObjectScript.getChild(this.gameObject, name);
	}

	// Token: 0x060011AE RID: 4526 RVA: 0x000793F8 File Offset: 0x000775F8
	public List<LuaGameObjectReference> getChildren()
	{
		return this.luaGameObjectScript.getChildren(this.gameObject);
	}

	// Token: 0x060011AF RID: 4527 RVA: 0x0007940B File Offset: 0x0007760B
	public LuaComponentReference getComponent(string name)
	{
		return this.luaGameObjectScript.getComponent(this.gameObject, name);
	}

	// Token: 0x060011B0 RID: 4528 RVA: 0x0007941F File Offset: 0x0007761F
	public LuaComponentReference getComponentInChildren(string name)
	{
		return this.luaGameObjectScript.getComponentInChildren(this.gameObject, name);
	}

	// Token: 0x060011B1 RID: 4529 RVA: 0x00079433 File Offset: 0x00077633
	public List<LuaComponentReference> getComponents(string componentName = null)
	{
		return this.luaGameObjectScript.getComponents(this.gameObject, componentName);
	}

	// Token: 0x060011B2 RID: 4530 RVA: 0x00079447 File Offset: 0x00077647
	public List<LuaComponentReference> getComponentsInChildren(string componentName = null)
	{
		return this.luaGameObjectScript.getComponentsInChildren(this.gameObject, componentName);
	}

	// Token: 0x060011B3 RID: 4531 RVA: 0x0007945B File Offset: 0x0007765B
	public List<LuaMaterialReference> getMaterials()
	{
		return this.luaGameObjectScript.getMaterials(this.gameObject);
	}

	// Token: 0x060011B4 RID: 4532 RVA: 0x0007946E File Offset: 0x0007766E
	public List<LuaMaterialReference> getMaterialsInChildren()
	{
		return this.luaGameObjectScript.getMaterialsInChildren(this.gameObject);
	}

	// Token: 0x04000B67 RID: 2919
	[MoonSharpHidden]
	private GameObject gameObject;
}
