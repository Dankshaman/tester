using System;
using UnityEngine;

// Token: 0x02000223 RID: 547
public class JointHingeState : JointState, IEquatable<JointHingeState>
{
	// Token: 0x06001B43 RID: 6979 RVA: 0x000BBFB4 File Offset: 0x000BA1B4
	public bool Equals(JointHingeState other)
	{
		return other != null && (this == other || (base.Equals(other) && this.UseLimits == other.UseLimits && this.Limits.Equals(other.Limits) && this.UseMotor == other.UseMotor && this.Motor.Equals(other.Motor) && this.UseSpring == other.UseSpring && this.Spring.Equals(other.Spring)));
	}

	// Token: 0x06001B44 RID: 6980 RVA: 0x000BC05C File Offset: 0x000BA25C
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((JointHingeState)obj)));
	}

	// Token: 0x06001B45 RID: 6981 RVA: 0x000BC08C File Offset: 0x000BA28C
	public override int GetHashCode()
	{
		return (((((base.GetHashCode() * 397 ^ this.UseLimits.GetHashCode()) * 397 ^ this.Limits.GetHashCode()) * 397 ^ this.UseMotor.GetHashCode()) * 397 ^ this.Motor.GetHashCode()) * 397 ^ this.UseSpring.GetHashCode()) * 397 ^ this.Spring.GetHashCode();
	}

	// Token: 0x06001B46 RID: 6982 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(JointHingeState left, JointHingeState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001B47 RID: 6983 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(JointHingeState left, JointHingeState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x04001149 RID: 4425
	public bool UseLimits;

	// Token: 0x0400114A RID: 4426
	public JointLimits Limits;

	// Token: 0x0400114B RID: 4427
	public bool UseMotor;

	// Token: 0x0400114C RID: 4428
	public JointMotor Motor;

	// Token: 0x0400114D RID: 4429
	public bool UseSpring;

	// Token: 0x0400114E RID: 4430
	public JointSpring Spring;
}
