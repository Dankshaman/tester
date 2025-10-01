using System;
using MoonSharp.Interpreter;

// Token: 0x0200016E RID: 366
public class LuaRPGFigurine : LuaComponent
{
	// Token: 0x0600118C RID: 4492 RVA: 0x00078CA7 File Offset: 0x00076EA7
	[MoonSharpHidden]
	public LuaRPGFigurine(LuaGameObjectScript LGOS) : base(LGOS)
	{
	}

	// Token: 0x0600118D RID: 4493 RVA: 0x00079056 File Offset: 0x00077256
	public bool attack()
	{
		this.LGOS.NPO.rpgFigurines.Attack();
		return true;
	}

	// Token: 0x0600118E RID: 4494 RVA: 0x0007906E File Offset: 0x0007726E
	public bool changeMode()
	{
		this.LGOS.NPO.rpgFigurines.ChangeMode();
		return true;
	}

	// Token: 0x0600118F RID: 4495 RVA: 0x00079086 File Offset: 0x00077286
	public bool die()
	{
		this.LGOS.NPO.rpgFigurines.Die();
		return true;
	}

	// Token: 0x04000B48 RID: 2888
	public DynValue onAttack;

	// Token: 0x04000B49 RID: 2889
	public DynValue onHit;
}
