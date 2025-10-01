using System;

// Token: 0x02000202 RID: 514
public class DecalState : IEquatable<DecalState>
{
	// Token: 0x06001A6F RID: 6767 RVA: 0x000B9C03 File Offset: 0x000B7E03
	public bool Equals(DecalState other)
	{
		return other != null && (this == other || (object.Equals(this.Transform, other.Transform) && object.Equals(this.CustomDecal, other.CustomDecal)));
	}

	// Token: 0x06001A70 RID: 6768 RVA: 0x000B9C36 File Offset: 0x000B7E36
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((DecalState)obj)));
	}

	// Token: 0x06001A71 RID: 6769 RVA: 0x000B9C64 File Offset: 0x000B7E64
	public override int GetHashCode()
	{
		return ((this.Transform != null) ? this.Transform.GetHashCode() : 0) * 397 ^ ((this.CustomDecal != null) ? this.CustomDecal.GetHashCode() : 0);
	}

	// Token: 0x06001A72 RID: 6770 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(DecalState left, DecalState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001A73 RID: 6771 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(DecalState left, DecalState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x040010C4 RID: 4292
	public TransformState Transform;

	// Token: 0x040010C5 RID: 4293
	public CustomDecalState CustomDecal;
}
