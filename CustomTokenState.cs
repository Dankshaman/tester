using System;

// Token: 0x02000211 RID: 529
public class CustomTokenState : IEquatable<CustomTokenState>
{
	// Token: 0x06001AD4 RID: 6868 RVA: 0x000BAD40 File Offset: 0x000B8F40
	public bool Equals(CustomTokenState other)
	{
		return other != null && (this == other || (this.Thickness.Equals(other.Thickness) && this.MergeDistancePixels.Equals(other.MergeDistancePixels) && this.StandUp == other.StandUp && this.Stackable == other.Stackable));
	}

	// Token: 0x06001AD5 RID: 6869 RVA: 0x000BAD9C File Offset: 0x000B8F9C
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((CustomTokenState)obj)));
	}

	// Token: 0x06001AD6 RID: 6870 RVA: 0x000BADCC File Offset: 0x000B8FCC
	public override int GetHashCode()
	{
		return ((this.Thickness.GetHashCode() * 397 ^ this.MergeDistancePixels.GetHashCode()) * 397 ^ this.StandUp.GetHashCode()) * 397 ^ this.Stackable.GetHashCode();
	}

	// Token: 0x06001AD7 RID: 6871 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(CustomTokenState left, CustomTokenState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001AD8 RID: 6872 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(CustomTokenState left, CustomTokenState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x04001103 RID: 4355
	public float Thickness;

	// Token: 0x04001104 RID: 4356
	public float MergeDistancePixels;

	// Token: 0x04001105 RID: 4357
	public bool StandUp;

	// Token: 0x04001106 RID: 4358
	public bool Stackable;
}
