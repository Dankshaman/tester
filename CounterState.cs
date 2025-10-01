using System;

// Token: 0x0200021B RID: 539
public class CounterState : IEquatable<CounterState>
{
	// Token: 0x06001B13 RID: 6931 RVA: 0x000BB82A File Offset: 0x000B9A2A
	public bool Equals(CounterState other)
	{
		return other != null && (this == other || this.value == other.value);
	}

	// Token: 0x06001B14 RID: 6932 RVA: 0x000BB845 File Offset: 0x000B9A45
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((CounterState)obj)));
	}

	// Token: 0x06001B15 RID: 6933 RVA: 0x000BB873 File Offset: 0x000B9A73
	public override int GetHashCode()
	{
		return this.value;
	}

	// Token: 0x06001B16 RID: 6934 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(CounterState left, CounterState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001B17 RID: 6935 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(CounterState left, CounterState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x0400112C RID: 4396
	public int value;
}
