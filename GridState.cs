using System;
using Newtonsoft.Json;

// Token: 0x020001F5 RID: 501
public class GridState : IEquatable<GridState>
{
	// Token: 0x06001A2F RID: 6703 RVA: 0x000B8944 File Offset: 0x000B6B44
	[JsonConstructor]
	public GridState()
	{
	}

	// Token: 0x06001A30 RID: 6704 RVA: 0x000B89B4 File Offset: 0x000B6BB4
	public GridState(GridState copyFrom, GridType? Type = null, bool? Lines = null, ColourState? Color = null, float? Opacity = null, bool? ThickLines = null, bool? Snapping = null, bool? Offset = null, bool? BothSnapping = null, float? xSize = null, float? ySize = null, VectorState? PosOffset = null)
	{
		this.Type = (Type ?? copyFrom.Type);
		this.Lines = (Lines ?? copyFrom.Lines);
		this.Color = (Color ?? copyFrom.Color);
		this.Opacity = (Opacity ?? copyFrom.Opacity);
		this.ThickLines = (ThickLines ?? copyFrom.ThickLines);
		this.Snapping = (Snapping ?? copyFrom.Snapping);
		this.Offset = (Offset ?? copyFrom.Offset);
		this.BothSnapping = (BothSnapping ?? copyFrom.BothSnapping);
		this.xSize = (xSize ?? copyFrom.xSize);
		this.ySize = (ySize ?? copyFrom.ySize);
		this.PosOffset = (PosOffset ?? copyFrom.PosOffset);
	}

	// Token: 0x06001A31 RID: 6705 RVA: 0x000B8B8C File Offset: 0x000B6D8C
	public bool Equals(GridState other)
	{
		return other != null && (this == other || (this.Type == other.Type && this.Lines == other.Lines && this.Color.Equals(other.Color) && this.Opacity.Equals(other.Opacity) && this.ThickLines == other.ThickLines && this.Snapping == other.Snapping && this.Offset == other.Offset && this.BothSnapping == other.BothSnapping && this.xSize.Equals(other.xSize) && this.ySize.Equals(other.ySize) && this.PosOffset.Equals(other.PosOffset)));
	}

	// Token: 0x06001A32 RID: 6706 RVA: 0x000B8C60 File Offset: 0x000B6E60
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((GridState)obj)));
	}

	// Token: 0x06001A33 RID: 6707 RVA: 0x000B8C90 File Offset: 0x000B6E90
	public override int GetHashCode()
	{
		return (int)((((((((((this.Type * (GridType)397 ^ (GridType)this.Lines.GetHashCode()) * (GridType)397 ^ (GridType)this.Color.GetHashCode()) * (GridType)397 ^ (GridType)this.Opacity.GetHashCode()) * (GridType)397 ^ (GridType)this.ThickLines.GetHashCode()) * (GridType)397 ^ (GridType)this.Snapping.GetHashCode()) * (GridType)397 ^ (GridType)this.Offset.GetHashCode()) * (GridType)397 ^ (GridType)this.BothSnapping.GetHashCode()) * (GridType)397 ^ (GridType)this.xSize.GetHashCode()) * (GridType)397 ^ (GridType)this.ySize.GetHashCode()) * (GridType)397 ^ (GridType)this.PosOffset.GetHashCode());
	}

	// Token: 0x06001A34 RID: 6708 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(GridState left, GridState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001A35 RID: 6709 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(GridState left, GridState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x04001086 RID: 4230
	public GridType Type;

	// Token: 0x04001087 RID: 4231
	public bool Lines;

	// Token: 0x04001088 RID: 4232
	public ColourState Color = new ColourState(0f, 0f, 0f, 1f);

	// Token: 0x04001089 RID: 4233
	public float Opacity = 0.75f;

	// Token: 0x0400108A RID: 4234
	public bool ThickLines;

	// Token: 0x0400108B RID: 4235
	public bool Snapping;

	// Token: 0x0400108C RID: 4236
	public bool Offset;

	// Token: 0x0400108D RID: 4237
	public bool BothSnapping;

	// Token: 0x0400108E RID: 4238
	public float xSize = 2f;

	// Token: 0x0400108F RID: 4239
	public float ySize = 2f;

	// Token: 0x04001090 RID: 4240
	public VectorState PosOffset = new VectorState(0f, 1f, 0f);
}
