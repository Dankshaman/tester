using System;

// Token: 0x02000221 RID: 545
public class JointState : IEquatable<JointState>
{
	// Token: 0x06001B37 RID: 6967 RVA: 0x000BBDB8 File Offset: 0x000B9FB8
	public void Assign(JointState jointState)
	{
		this.ConnectedBodyGUID = jointState.ConnectedBodyGUID;
		this.EnableCollision = jointState.EnableCollision;
		this.Anchor = jointState.Anchor;
		this.Axis = jointState.Axis;
		this.ConnectedAnchor = jointState.ConnectedAnchor;
		this.BreakForce = jointState.BreakForce;
		this.BreakTorgue = jointState.BreakTorgue;
	}

	// Token: 0x06001B38 RID: 6968 RVA: 0x000BBE1C File Offset: 0x000BA01C
	public bool Equals(JointState other)
	{
		return other != null && (this == other || (this.ConnectedBodyGUID == other.ConnectedBodyGUID && this.EnableCollision == other.EnableCollision && this.Axis.Equals(other.Axis) && this.Anchor.Equals(other.Anchor) && this.ConnectedAnchor.Equals(other.ConnectedAnchor) && this.BreakForce.Equals(other.BreakForce) && this.BreakTorgue.Equals(other.BreakTorgue)));
	}

	// Token: 0x06001B39 RID: 6969 RVA: 0x000BBEB4 File Offset: 0x000BA0B4
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((JointState)obj)));
	}

	// Token: 0x06001B3A RID: 6970 RVA: 0x000BBEE4 File Offset: 0x000BA0E4
	public override int GetHashCode()
	{
		return (((((((this.ConnectedBodyGUID != null) ? this.ConnectedBodyGUID.GetHashCode() : 0) * 397 ^ this.EnableCollision.GetHashCode()) * 397 ^ this.Axis.GetHashCode()) * 397 ^ this.Anchor.GetHashCode()) * 397 ^ this.ConnectedAnchor.GetHashCode()) * 397 ^ this.BreakForce.GetHashCode()) * 397 ^ this.BreakTorgue.GetHashCode();
	}

	// Token: 0x06001B3B RID: 6971 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(JointState left, JointState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001B3C RID: 6972 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(JointState left, JointState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x04001142 RID: 4418
	public string ConnectedBodyGUID = "";

	// Token: 0x04001143 RID: 4419
	public bool EnableCollision;

	// Token: 0x04001144 RID: 4420
	public VectorState Axis;

	// Token: 0x04001145 RID: 4421
	public VectorState Anchor;

	// Token: 0x04001146 RID: 4422
	public VectorState ConnectedAnchor;

	// Token: 0x04001147 RID: 4423
	public float BreakForce;

	// Token: 0x04001148 RID: 4424
	public float BreakTorgue;
}
