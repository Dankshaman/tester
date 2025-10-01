using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;

// Token: 0x0200017E RID: 382
public class LuaContainer : LuaComponent
{
	// Token: 0x060011C1 RID: 4545 RVA: 0x00078CA7 File Offset: 0x00076EA7
	[MoonSharpHidden]
	public LuaContainer(LuaGameObjectScript LGOS) : base(LGOS)
	{
	}

	// Token: 0x060011C2 RID: 4546 RVA: 0x00079594 File Offset: 0x00077794
	public LuaGameObjectScript PutObject(LuaGameObjectScript luaGameObjectScript, int index = 1)
	{
		return null;
	}

	// Token: 0x060011C3 RID: 4547 RVA: 0x00079594 File Offset: 0x00077794
	public LuaGameObjectScript TakeObject(LuaGameObjectScript.LuaTakeObjectParameters luaTakeObjectParameters)
	{
		return null;
	}

	// Token: 0x060011C4 RID: 4548 RVA: 0x00079594 File Offset: 0x00077794
	public List<ObjectState> GetData()
	{
		return null;
	}

	// Token: 0x060011C5 RID: 4549 RVA: 0x00014D66 File Offset: 0x00012F66
	public bool SetData(List<ObjectState> objectStates)
	{
		return true;
	}

	// Token: 0x060011C6 RID: 4550 RVA: 0x00014D66 File Offset: 0x00012F66
	public bool AddData(ObjectState objectState, int index = 1)
	{
		return true;
	}

	// Token: 0x060011C7 RID: 4551 RVA: 0x00079594 File Offset: 0x00077794
	public ObjectState RemoveData(int index)
	{
		return null;
	}

	// Token: 0x060011C8 RID: 4552 RVA: 0x00079594 File Offset: 0x00077794
	public Table GetInfo()
	{
		return null;
	}

	// Token: 0x060011C9 RID: 4553 RVA: 0x00014D66 File Offset: 0x00012F66
	public bool Reset()
	{
		return true;
	}

	// Token: 0x060011CA RID: 4554 RVA: 0x00014D66 File Offset: 0x00012F66
	public bool Randomize()
	{
		return true;
	}

	// Token: 0x060011CB RID: 4555 RVA: 0x00079598 File Offset: 0x00077798
	public bool Search(string player, int maxCards = -1)
	{
		int num = NetworkSingleton<PlayerManager>.Instance.IDFromColour(player);
		if (num < 0)
		{
			return false;
		}
		NetworkSingleton<ManagerPhysicsObject>.Instance.SearchInventory(this.LGOS.NPO.ID, num, maxCards);
		return true;
	}

	// Token: 0x060011CC RID: 4556 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
	public int GetCount()
	{
		return 0;
	}
}
