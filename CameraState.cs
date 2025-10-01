using System;

// Token: 0x02000220 RID: 544
public class CameraState : IEquatable<CameraState>
{
	// Token: 0x06001B31 RID: 6961 RVA: 0x000BBCA4 File Offset: 0x000B9EA4
	public bool Equals(CameraState other)
	{
		return other != null && (this == other || (this.Position.Equals(other.Position) && this.Rotation.Equals(other.Rotation) && this.Distance.Equals(other.Distance) && this.Zoomed == other.Zoomed && Nullable.Equals<VectorState>(this.AbsolutePosition, other.AbsolutePosition)));
	}

	// Token: 0x06001B32 RID: 6962 RVA: 0x000BBD16 File Offset: 0x000B9F16
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((CameraState)obj)));
	}

	// Token: 0x06001B33 RID: 6963 RVA: 0x000BBD44 File Offset: 0x000B9F44
	public override int GetHashCode()
	{
		return (((this.Position.GetHashCode() * 397 ^ this.Rotation.GetHashCode()) * 397 ^ this.Distance.GetHashCode()) * 397 ^ this.Zoomed.GetHashCode()) * 397 ^ this.AbsolutePosition.GetHashCode();
	}

	// Token: 0x06001B34 RID: 6964 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(CameraState left, CameraState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001B35 RID: 6965 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(CameraState left, CameraState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x0400113D RID: 4413
	public VectorState Position;

	// Token: 0x0400113E RID: 4414
	public VectorState Rotation;

	// Token: 0x0400113F RID: 4415
	public float Distance;

	// Token: 0x04001140 RID: 4416
	public bool Zoomed;

	// Token: 0x04001141 RID: 4417
	public VectorState? AbsolutePosition;
}
