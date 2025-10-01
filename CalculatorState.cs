using System;

// Token: 0x0200021E RID: 542
public class CalculatorState : IEquatable<CalculatorState>
{
	// Token: 0x06001B25 RID: 6949 RVA: 0x000BBA60 File Offset: 0x000B9C60
	public bool Equals(CalculatorState other)
	{
		return other != null && (this == other || (this.value == other.value && this.memory.Equals(other.memory)));
	}

	// Token: 0x06001B26 RID: 6950 RVA: 0x000BBA93 File Offset: 0x000B9C93
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((CalculatorState)obj)));
	}

	// Token: 0x06001B27 RID: 6951 RVA: 0x000BBAC1 File Offset: 0x000B9CC1
	public override int GetHashCode()
	{
		return ((this.value != null) ? this.value.GetHashCode() : 0) * 397 ^ this.memory.GetHashCode();
	}

	// Token: 0x06001B28 RID: 6952 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(CalculatorState left, CalculatorState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001B29 RID: 6953 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(CalculatorState left, CalculatorState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x04001135 RID: 4405
	public string value = "";

	// Token: 0x04001136 RID: 4406
	public float memory;
}
