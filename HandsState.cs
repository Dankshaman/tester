using System;
using System.Collections.Generic;
using Newtonsoft.Json;

// Token: 0x020001F9 RID: 505
public class HandsState : IEquatable<HandsState>
{
	// Token: 0x06001A3D RID: 6717 RVA: 0x000B9235 File Offset: 0x000B7435
	[JsonConstructor]
	public HandsState()
	{
	}

	// Token: 0x06001A3E RID: 6718 RVA: 0x000B9244 File Offset: 0x000B7444
	public HandsState(HandsState copyFrom, bool? Enable = null, bool? DisableUnused = null, HidingType? Hiding = null, List<HandTransformState> HandTransforms = null)
	{
		this.Enable = (Enable ?? copyFrom.Enable);
		this.DisableUnused = (DisableUnused ?? copyFrom.DisableUnused);
		this.Hiding = (Hiding ?? copyFrom.Hiding);
		this.HandTransforms = (HandTransforms ?? copyFrom.HandTransforms);
	}

	// Token: 0x06001A3F RID: 6719 RVA: 0x000B92D4 File Offset: 0x000B74D4
	public bool Equals(HandsState other)
	{
		return other != null && (this == other || (this.Enable == other.Enable && this.DisableUnused == other.DisableUnused && this.Hiding == other.Hiding && this.HandTransforms.SequenceEqualNullable(other.HandTransforms)));
	}

	// Token: 0x06001A40 RID: 6720 RVA: 0x000B9329 File Offset: 0x000B7529
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((HandsState)obj)));
	}

	// Token: 0x06001A41 RID: 6721 RVA: 0x000B9358 File Offset: 0x000B7558
	public override int GetHashCode()
	{
		return ((this.Enable.GetHashCode() * 397 ^ this.DisableUnused.GetHashCode()) * 397 ^ (int)this.Hiding) * 397 ^ ((this.HandTransforms != null) ? this.HandTransforms.GetHashCode() : 0);
	}

	// Token: 0x06001A42 RID: 6722 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(HandsState left, HandsState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001A43 RID: 6723 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(HandsState left, HandsState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x040010A3 RID: 4259
	public bool Enable = true;

	// Token: 0x040010A4 RID: 4260
	public bool DisableUnused;

	// Token: 0x040010A5 RID: 4261
	public HidingType Hiding;

	// Token: 0x040010A6 RID: 4262
	public List<HandTransformState> HandTransforms;
}
