using System;

// Token: 0x0200021A RID: 538
public class ClockState : IEquatable<ClockState>
{
	// Token: 0x06001B0D RID: 6925 RVA: 0x000BB79C File Offset: 0x000B999C
	public bool Equals(ClockState other)
	{
		return other != null && (this == other || (this.Mode == other.Mode && this.SecondsPassed == other.SecondsPassed && this.Paused == other.Paused));
	}

	// Token: 0x06001B0E RID: 6926 RVA: 0x000BB7D5 File Offset: 0x000B99D5
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((ClockState)obj)));
	}

	// Token: 0x06001B0F RID: 6927 RVA: 0x000BB803 File Offset: 0x000B9A03
	public override int GetHashCode()
	{
		return (int)((this.Mode * (ClockScript.ClockMode)397 ^ (ClockScript.ClockMode)this.SecondsPassed) * (ClockScript.ClockMode)397 ^ (ClockScript.ClockMode)this.Paused.GetHashCode());
	}

	// Token: 0x06001B10 RID: 6928 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(ClockState left, ClockState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001B11 RID: 6929 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(ClockState left, ClockState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x04001129 RID: 4393
	public ClockScript.ClockMode Mode;

	// Token: 0x0400112A RID: 4394
	public int SecondsPassed;

	// Token: 0x0400112B RID: 4395
	public bool Paused;
}
