using System;

// Token: 0x02000210 RID: 528
public class CustomDiceState : IEquatable<CustomDiceState>
{
	// Token: 0x06001ACE RID: 6862 RVA: 0x000BACEC File Offset: 0x000B8EEC
	public bool Equals(CustomDiceState other)
	{
		return other != null && (this == other || this.Type == other.Type);
	}

	// Token: 0x06001ACF RID: 6863 RVA: 0x000BAD07 File Offset: 0x000B8F07
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((CustomDiceState)obj)));
	}

	// Token: 0x06001AD0 RID: 6864 RVA: 0x000BAD35 File Offset: 0x000B8F35
	public override int GetHashCode()
	{
		return (int)this.Type;
	}

	// Token: 0x06001AD1 RID: 6865 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(CustomDiceState left, CustomDiceState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001AD2 RID: 6866 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(CustomDiceState left, CustomDiceState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x04001102 RID: 4354
	public DiceType Type;
}
