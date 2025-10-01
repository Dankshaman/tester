using System;
using System.Collections.Generic;

// Token: 0x02000217 RID: 535
public class FogOfWarState : IEquatable<FogOfWarState>
{
	// Token: 0x06001AFB RID: 6907 RVA: 0x000BB4BC File Offset: 0x000B96BC
	public bool Equals(FogOfWarState other)
	{
		return other != null && (this == other || (this.HideGmPointer == other.HideGmPointer && this.HideObjects == other.HideObjects && this.ReHideObjects == other.ReHideObjects && this.Height.Equals(other.Height) && this.RevealedLocations.SequenceEqualNullable(other.RevealedLocations)));
	}

	// Token: 0x06001AFC RID: 6908 RVA: 0x000BB524 File Offset: 0x000B9724
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((FogOfWarState)obj)));
	}

	// Token: 0x06001AFD RID: 6909 RVA: 0x000BB554 File Offset: 0x000B9754
	public override int GetHashCode()
	{
		return (((this.HideGmPointer.GetHashCode() * 397 ^ this.HideObjects.GetHashCode()) * 397 ^ this.ReHideObjects.GetHashCode()) * 397 ^ this.Height.GetHashCode()) * 397 ^ ((this.RevealedLocations != null) ? this.RevealedLocations.GetHashCode() : 0);
	}

	// Token: 0x06001AFE RID: 6910 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(FogOfWarState left, FogOfWarState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001AFF RID: 6911 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(FogOfWarState left, FogOfWarState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x0400111C RID: 4380
	public bool HideGmPointer;

	// Token: 0x0400111D RID: 4381
	public bool HideObjects;

	// Token: 0x0400111E RID: 4382
	public bool ReHideObjects;

	// Token: 0x0400111F RID: 4383
	public float Height;

	// Token: 0x04001120 RID: 4384
	public Dictionary<string, HashSet<int>> RevealedLocations;
}
