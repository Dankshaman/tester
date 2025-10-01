using System;
using MoonSharp.Interpreter;

// Token: 0x02000186 RID: 390
public class LuaLayoutZone : LuaZone
{
	// Token: 0x060011D5 RID: 4565 RVA: 0x000795DD File Offset: 0x000777DD
	[MoonSharpHidden]
	public LuaLayoutZone(LuaGameObjectScript LGOS) : base(LGOS)
	{
	}

	// Token: 0x060011D6 RID: 4566 RVA: 0x000795E6 File Offset: 0x000777E6
	public bool SetOptions(LuaLayoutZoneOptions luaOptions)
	{
		this.LGOS.NPO.layoutZone.SetOptions(luaOptions, false);
		return true;
	}

	// Token: 0x060011D7 RID: 4567 RVA: 0x00079600 File Offset: 0x00077800
	public LuaLayoutZoneOptions GetOptions()
	{
		return new LuaLayoutZoneOptions(this.LGOS.NPO.layoutZone.Options);
	}

	// Token: 0x060011D8 RID: 4568 RVA: 0x0007961C File Offset: 0x0007781C
	public bool Layout()
	{
		this.LGOS.GetComponent<LayoutZone>().ManualLayoutZone();
		return true;
	}
}
