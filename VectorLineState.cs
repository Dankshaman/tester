using System;
using System.Collections.Generic;

// Token: 0x0200021F RID: 543
public class VectorLineState : IEquatable<VectorLineState>
{
	// Token: 0x06001B2B RID: 6955 RVA: 0x000BBB00 File Offset: 0x000B9D00
	public bool Equals(VectorLineState other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (this.points3.SequenceEqualNullable(other.points3) && this.color.Equals(other.color) && this.thickness.Equals(other.thickness) && Nullable.Equals<VectorState>(this.rotation, other.rotation))
		{
			bool? flag = this.loop;
			bool? flag2 = other.loop;
			if (flag.GetValueOrDefault() == flag2.GetValueOrDefault() & flag != null == (flag2 != null))
			{
				flag2 = this.square;
				flag = other.square;
				return flag2.GetValueOrDefault() == flag.GetValueOrDefault() & flag2 != null == (flag != null);
			}
		}
		return false;
	}

	// Token: 0x06001B2C RID: 6956 RVA: 0x000BBBCC File Offset: 0x000B9DCC
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((VectorLineState)obj)));
	}

	// Token: 0x06001B2D RID: 6957 RVA: 0x000BBBFC File Offset: 0x000B9DFC
	public override int GetHashCode()
	{
		return ((((((this.points3 != null) ? this.points3.GetHashCode() : 0) * 397 ^ this.color.GetHashCode()) * 397 ^ this.thickness.GetHashCode()) * 397 ^ this.rotation.GetHashCode()) * 397 ^ this.loop.GetHashCode()) * 397 ^ this.square.GetHashCode();
	}

	// Token: 0x06001B2E RID: 6958 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(VectorLineState left, VectorLineState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001B2F RID: 6959 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(VectorLineState left, VectorLineState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x04001137 RID: 4407
	public List<VectorState> points3;

	// Token: 0x04001138 RID: 4408
	public ColourState color;

	// Token: 0x04001139 RID: 4409
	public float thickness = 0.1f;

	// Token: 0x0400113A RID: 4410
	public VectorState? rotation;

	// Token: 0x0400113B RID: 4411
	public bool? loop;

	// Token: 0x0400113C RID: 4412
	public bool? square;
}
