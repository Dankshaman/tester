using System;
using System.Collections.Generic;

// Token: 0x02000201 RID: 513
public class SnapPointState : IEquatable<SnapPointState>
{
	// Token: 0x06001A69 RID: 6761 RVA: 0x000B9B30 File Offset: 0x000B7D30
	public bool Equals(SnapPointState other)
	{
		return other != null && (this == other || (this.Position.Equals(other.Position) && Nullable.Equals<VectorState>(this.Rotation, other.Rotation) && this.Tags.SequenceEqualNullable(other.Tags)));
	}

	// Token: 0x06001A6A RID: 6762 RVA: 0x000B9B81 File Offset: 0x000B7D81
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((SnapPointState)obj)));
	}

	// Token: 0x06001A6B RID: 6763 RVA: 0x000B9BB0 File Offset: 0x000B7DB0
	public override int GetHashCode()
	{
		return (this.Position.GetHashCode() * 397 ^ this.Rotation.GetHashCode()) * 397 ^ ((this.Tags != null) ? this.Tags.GetHashCode() : 0);
	}

	// Token: 0x06001A6C RID: 6764 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(SnapPointState left, SnapPointState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001A6D RID: 6765 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(SnapPointState left, SnapPointState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x040010C1 RID: 4289
	public VectorState Position;

	// Token: 0x040010C2 RID: 4290
	public VectorState? Rotation;

	// Token: 0x040010C3 RID: 4291
	public List<string> Tags;
}
