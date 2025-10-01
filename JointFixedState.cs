using System;

// Token: 0x02000222 RID: 546
public class JointFixedState : JointState, IEquatable<JointFixedState>
{
	// Token: 0x06001B3E RID: 6974 RVA: 0x000BBF98 File Offset: 0x000BA198
	public bool Equals(JointFixedState other)
	{
		return base.Equals(other);
	}

	// Token: 0x06001B3F RID: 6975 RVA: 0x000BBFA1 File Offset: 0x000BA1A1
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	// Token: 0x06001B40 RID: 6976 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(JointFixedState left, JointFixedState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001B41 RID: 6977 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(JointFixedState left, JointFixedState right)
	{
		return !object.Equals(left, right);
	}
}
