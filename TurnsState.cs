using System;
using System.Collections.Generic;
using Newtonsoft.Json;

// Token: 0x020001FD RID: 509
public class TurnsState : IEquatable<TurnsState>
{
	// Token: 0x06001A55 RID: 6741 RVA: 0x000B9632 File Offset: 0x000B7832
	[JsonConstructor]
	public TurnsState()
	{
	}

	// Token: 0x06001A56 RID: 6742 RVA: 0x000B964C File Offset: 0x000B784C
	public TurnsState(TurnsState copyFrom, bool? Enable = null, TurnType? Type = null, List<string> TurnOrder = null, bool? Reverse = null, bool? SkipEmpty = null, bool? DisableInteractions = null, bool? PassTurns = null, string TurnColor = null)
	{
		this.Enable = (Enable ?? copyFrom.Enable);
		this.Type = (Type ?? copyFrom.Type);
		this.TurnOrder = (TurnOrder ?? copyFrom.TurnOrder);
		this.Reverse = (Reverse ?? copyFrom.Reverse);
		this.SkipEmpty = (SkipEmpty ?? copyFrom.SkipEmpty);
		this.DisableInteractions = (DisableInteractions ?? copyFrom.DisableInteractions);
		this.PassTurns = (PassTurns ?? copyFrom.PassTurns);
		this.TurnColor = (TurnColor ?? copyFrom.TurnColor);
	}

	// Token: 0x06001A57 RID: 6743 RVA: 0x000B975C File Offset: 0x000B795C
	public bool Equals(TurnsState other)
	{
		return other != null && (this == other || (this.Enable == other.Enable && this.Type == other.Type && this.TurnOrder.SequenceEqualNullable(other.TurnOrder) && this.Reverse == other.Reverse && this.SkipEmpty == other.SkipEmpty && this.DisableInteractions == other.DisableInteractions && this.PassTurns == other.PassTurns && this.TurnColor == other.TurnColor));
	}

	// Token: 0x06001A58 RID: 6744 RVA: 0x000B97EE File Offset: 0x000B79EE
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((TurnsState)obj)));
	}

	// Token: 0x06001A59 RID: 6745 RVA: 0x000B981C File Offset: 0x000B7A1C
	public override int GetHashCode()
	{
		return ((((((this.Enable.GetHashCode() * 397 ^ (int)this.Type) * 397 ^ ((this.TurnOrder != null) ? this.TurnOrder.GetHashCode() : 0)) * 397 ^ this.Reverse.GetHashCode()) * 397 ^ this.SkipEmpty.GetHashCode()) * 397 ^ this.DisableInteractions.GetHashCode()) * 397 ^ this.PassTurns.GetHashCode()) * 397 ^ ((this.TurnColor != null) ? this.TurnColor.GetHashCode() : 0);
	}

	// Token: 0x06001A5A RID: 6746 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(TurnsState left, TurnsState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001A5B RID: 6747 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(TurnsState left, TurnsState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x040010AE RID: 4270
	public bool Enable;

	// Token: 0x040010AF RID: 4271
	public TurnType Type;

	// Token: 0x040010B0 RID: 4272
	public List<string> TurnOrder = new List<string>();

	// Token: 0x040010B1 RID: 4273
	public bool Reverse;

	// Token: 0x040010B2 RID: 4274
	public bool SkipEmpty;

	// Token: 0x040010B3 RID: 4275
	public bool DisableInteractions;

	// Token: 0x040010B4 RID: 4276
	public bool PassTurns = true;

	// Token: 0x040010B5 RID: 4277
	public string TurnColor;
}
