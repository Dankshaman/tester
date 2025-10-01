using System;

// Token: 0x020001FC RID: 508
public class HandTransformState : IEquatable<HandTransformState>
{
	// Token: 0x06001A4F RID: 6735 RVA: 0x000B9596 File Offset: 0x000B7796
	public bool Equals(HandTransformState other)
	{
		return other != null && (this == other || (this.Color == other.Color && object.Equals(this.Transform, other.Transform)));
	}

	// Token: 0x06001A50 RID: 6736 RVA: 0x000B95C9 File Offset: 0x000B77C9
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((HandTransformState)obj)));
	}

	// Token: 0x06001A51 RID: 6737 RVA: 0x000B95F7 File Offset: 0x000B77F7
	public override int GetHashCode()
	{
		return ((this.Color != null) ? this.Color.GetHashCode() : 0) * 397 ^ ((this.Transform != null) ? this.Transform.GetHashCode() : 0);
	}

	// Token: 0x06001A52 RID: 6738 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(HandTransformState left, HandTransformState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001A53 RID: 6739 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(HandTransformState left, HandTransformState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x040010AC RID: 4268
	public string Color;

	// Token: 0x040010AD RID: 4269
	public TransformState Transform;
}
