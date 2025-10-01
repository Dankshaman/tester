using System;
using System.Collections.Generic;

// Token: 0x02000216 RID: 534
public class LayoutZoneState : IEquatable<LayoutZoneState>
{
	// Token: 0x06001AF5 RID: 6901 RVA: 0x000BB428 File Offset: 0x000B9628
	public bool Equals(LayoutZoneState other)
	{
		return other != null && (this == other || (this.Options.Equals(other.Options) && this.GroupsInZone.SequenceEqualNullable(other.GroupsInZone)));
	}

	// Token: 0x06001AF6 RID: 6902 RVA: 0x000BB45B File Offset: 0x000B965B
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((LayoutZoneState)obj)));
	}

	// Token: 0x06001AF7 RID: 6903 RVA: 0x000BB489 File Offset: 0x000B9689
	public override int GetHashCode()
	{
		return this.Options.GetHashCode() * 397 ^ ((this.GroupsInZone != null) ? this.GroupsInZone.GetHashCode() : 0);
	}

	// Token: 0x06001AF8 RID: 6904 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(LayoutZoneState left, LayoutZoneState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001AF9 RID: 6905 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(LayoutZoneState left, LayoutZoneState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x0400111A RID: 4378
	public LayoutZoneOptions Options;

	// Token: 0x0400111B RID: 4379
	public List<List<string>> GroupsInZone;
}
