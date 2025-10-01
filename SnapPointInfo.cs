using System;
using System.Collections.Generic;

// Token: 0x02000232 RID: 562
public class SnapPointInfo : IEquatable<SnapPointInfo>
{
	// Token: 0x06001BEF RID: 7151 RVA: 0x000BFF54 File Offset: 0x000BE154
	public SnapPointInfo(SnapPointState state)
	{
		this.Position = state.Position;
		this.Rotation = state.Rotation;
		if (state.Tags != null && state.Tags.Count > 0)
		{
			this.Tags = NetworkSingleton<ComponentTags>.Instance.FlagsFromDisplayedTagLabels(state.Tags);
		}
	}

	// Token: 0x06001BF0 RID: 7152 RVA: 0x000BFFB6 File Offset: 0x000BE1B6
	public SnapPointInfo()
	{
	}

	// Token: 0x06001BF1 RID: 7153 RVA: 0x000BFFCC File Offset: 0x000BE1CC
	public bool Equals(SnapPointInfo other)
	{
		return other != null && (this == other || (this.Position.Equals(other.Position) && Nullable.Equals<VectorState>(this.Rotation, other.Rotation) && this.Tags.SequenceEqualNullable(other.Tags)));
	}

	// Token: 0x06001BF2 RID: 7154 RVA: 0x000C001D File Offset: 0x000BE21D
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((SnapPointInfo)obj)));
	}

	// Token: 0x06001BF3 RID: 7155 RVA: 0x000C004C File Offset: 0x000BE24C
	public override int GetHashCode()
	{
		return (this.Position.GetHashCode() * 397 ^ this.Rotation.GetHashCode()) * 397 ^ ((this.Tags != null) ? this.Tags.GetHashCode() : 0);
	}

	// Token: 0x06001BF4 RID: 7156 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(SnapPointInfo left, SnapPointInfo right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001BF5 RID: 7157 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(SnapPointInfo left, SnapPointInfo right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x04001184 RID: 4484
	public VectorState Position;

	// Token: 0x04001185 RID: 4485
	public VectorState? Rotation;

	// Token: 0x04001186 RID: 4486
	public List<ulong> Tags = new List<ulong>();
}
