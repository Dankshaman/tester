using System;

// Token: 0x02000224 RID: 548
public class JointSpringState : JointState, IEquatable<JointSpringState>
{
	// Token: 0x06001B49 RID: 6985 RVA: 0x000BC120 File Offset: 0x000BA320
	public bool Equals(JointSpringState other)
	{
		return other != null && (this == other || (base.Equals(other) && this.Damper.Equals(other.Damper) && this.MaxDistance.Equals(other.MaxDistance) && this.MinDistance.Equals(other.MinDistance) && this.Spring.Equals(other.Spring)));
	}

	// Token: 0x06001B4A RID: 6986 RVA: 0x000BC18D File Offset: 0x000BA38D
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((JointSpringState)obj)));
	}

	// Token: 0x06001B4B RID: 6987 RVA: 0x000BC1BC File Offset: 0x000BA3BC
	public override int GetHashCode()
	{
		return (((base.GetHashCode() * 397 ^ this.Damper.GetHashCode()) * 397 ^ this.MaxDistance.GetHashCode()) * 397 ^ this.MinDistance.GetHashCode()) * 397 ^ this.Spring.GetHashCode();
	}

	// Token: 0x06001B4C RID: 6988 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(JointSpringState left, JointSpringState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001B4D RID: 6989 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(JointSpringState left, JointSpringState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x0400114F RID: 4431
	public float Damper;

	// Token: 0x04001150 RID: 4432
	public float MaxDistance;

	// Token: 0x04001151 RID: 4433
	public float MinDistance;

	// Token: 0x04001152 RID: 4434
	public float Spring;
}
