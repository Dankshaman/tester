using System;

// Token: 0x02000218 RID: 536
public class FogOfWarRevealerState : IEquatable<FogOfWarRevealerState>
{
	// Token: 0x06001B01 RID: 6913 RVA: 0x000BB5C0 File Offset: 0x000B97C0
	public bool Equals(FogOfWarRevealerState other)
	{
		return other != null && (this == other || (this.Active == other.Active && this.ShowOutLine == other.ShowOutLine && this.Range.Equals(other.Range) && this.Height.Equals(other.Height) && this.Color == other.Color && this.FoV.Equals(other.FoV) && this.FoVOffset.Equals(other.FoVOffset)));
	}

	// Token: 0x06001B02 RID: 6914 RVA: 0x000BB653 File Offset: 0x000B9853
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((FogOfWarRevealerState)obj)));
	}

	// Token: 0x06001B03 RID: 6915 RVA: 0x000BB684 File Offset: 0x000B9884
	public override int GetHashCode()
	{
		return (((((this.Active.GetHashCode() * 397 ^ this.ShowOutLine.GetHashCode()) * 397 ^ this.Range.GetHashCode()) * 397 ^ this.Height.GetHashCode()) * 397 ^ ((this.Color != null) ? this.Color.GetHashCode() : 0)) * 397 ^ this.FoV.GetHashCode()) * 397 ^ this.FoVOffset.GetHashCode();
	}

	// Token: 0x06001B04 RID: 6916 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(FogOfWarRevealerState left, FogOfWarRevealerState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001B05 RID: 6917 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(FogOfWarRevealerState left, FogOfWarRevealerState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x04001121 RID: 4385
	public bool Active;

	// Token: 0x04001122 RID: 4386
	public bool ShowOutLine;

	// Token: 0x04001123 RID: 4387
	public float Range;

	// Token: 0x04001124 RID: 4388
	public float Height = 1.2f;

	// Token: 0x04001125 RID: 4389
	public string Color;

	// Token: 0x04001126 RID: 4390
	public float FoV;

	// Token: 0x04001127 RID: 4391
	public float FoVOffset;
}
