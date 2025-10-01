using System;

// Token: 0x0200021C RID: 540
public class BagState : IEquatable<BagState>
{
	// Token: 0x06001B19 RID: 6937 RVA: 0x000BB87B File Offset: 0x000B9A7B
	public bool Equals(BagState other)
	{
		return other != null && (this == other || this.Order == other.Order);
	}

	// Token: 0x06001B1A RID: 6938 RVA: 0x000BB896 File Offset: 0x000B9A96
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((BagState)obj)));
	}

	// Token: 0x06001B1B RID: 6939 RVA: 0x000BB8C4 File Offset: 0x000B9AC4
	public override int GetHashCode()
	{
		return (int)this.Order;
	}

	// Token: 0x06001B1C RID: 6940 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(BagState left, BagState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001B1D RID: 6941 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(BagState left, BagState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x0400112D RID: 4397
	public OrderType Order;
}
