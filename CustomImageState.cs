using System;

// Token: 0x0200020D RID: 525
public class CustomImageState : IEquatable<CustomImageState>
{
	// Token: 0x06001ABC RID: 6844 RVA: 0x000BA8C8 File Offset: 0x000B8AC8
	public bool Equals(CustomImageState other)
	{
		return other != null && (this == other || (this.ImageURL == other.ImageURL && this.ImageSecondaryURL == other.ImageSecondaryURL && this.ImageScalar.Equals(other.ImageScalar) && this.WidthScale.Equals(other.WidthScale) && object.Equals(this.CustomDice, other.CustomDice) && object.Equals(this.CustomToken, other.CustomToken) && object.Equals(this.CustomJigsawPuzzle, other.CustomJigsawPuzzle) && object.Equals(this.CustomTile, other.CustomTile)));
	}

	// Token: 0x06001ABD RID: 6845 RVA: 0x000BA97B File Offset: 0x000B8B7B
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((CustomImageState)obj)));
	}

	// Token: 0x06001ABE RID: 6846 RVA: 0x000BA9AC File Offset: 0x000B8BAC
	public override int GetHashCode()
	{
		return ((((((((this.ImageURL != null) ? this.ImageURL.GetHashCode() : 0) * 397 ^ ((this.ImageSecondaryURL != null) ? this.ImageSecondaryURL.GetHashCode() : 0)) * 397 ^ this.ImageScalar.GetHashCode()) * 397 ^ this.WidthScale.GetHashCode()) * 397 ^ ((this.CustomDice != null) ? this.CustomDice.GetHashCode() : 0)) * 397 ^ ((this.CustomToken != null) ? this.CustomToken.GetHashCode() : 0)) * 397 ^ ((this.CustomJigsawPuzzle != null) ? this.CustomJigsawPuzzle.GetHashCode() : 0)) * 397 ^ ((this.CustomTile != null) ? this.CustomTile.GetHashCode() : 0);
	}

	// Token: 0x06001ABF RID: 6847 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(CustomImageState left, CustomImageState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001AC0 RID: 6848 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(CustomImageState left, CustomImageState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x040010F1 RID: 4337
	[Tag(TagType.URL)]
	public string ImageURL = "";

	// Token: 0x040010F2 RID: 4338
	[Tag(TagType.URL)]
	public string ImageSecondaryURL = "";

	// Token: 0x040010F3 RID: 4339
	public float ImageScalar = 1f;

	// Token: 0x040010F4 RID: 4340
	public float WidthScale;

	// Token: 0x040010F5 RID: 4341
	public CustomDiceState CustomDice;

	// Token: 0x040010F6 RID: 4342
	public CustomTokenState CustomToken;

	// Token: 0x040010F7 RID: 4343
	public CustomJigsawPuzzleState CustomJigsawPuzzle;

	// Token: 0x040010F8 RID: 4344
	public CustomTileState CustomTile;
}
