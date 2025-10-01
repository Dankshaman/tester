using System;

// Token: 0x0200020E RID: 526
public class CustomAssetbundleState : IEquatable<CustomAssetbundleState>
{
	// Token: 0x06001AC2 RID: 6850 RVA: 0x000BAAC8 File Offset: 0x000B8CC8
	public bool Equals(CustomAssetbundleState other)
	{
		return other != null && (this == other || (this.AssetbundleURL == other.AssetbundleURL && this.AssetbundleSecondaryURL == other.AssetbundleSecondaryURL && this.MaterialIndex == other.MaterialIndex && this.TypeIndex == other.TypeIndex && this.LoopingEffectIndex == other.LoopingEffectIndex));
	}

	// Token: 0x06001AC3 RID: 6851 RVA: 0x000BAB32 File Offset: 0x000B8D32
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((CustomAssetbundleState)obj)));
	}

	// Token: 0x06001AC4 RID: 6852 RVA: 0x000BAB60 File Offset: 0x000B8D60
	public override int GetHashCode()
	{
		return (((((this.AssetbundleURL != null) ? this.AssetbundleURL.GetHashCode() : 0) * 397 ^ ((this.AssetbundleSecondaryURL != null) ? this.AssetbundleSecondaryURL.GetHashCode() : 0)) * 397 ^ this.MaterialIndex) * 397 ^ this.TypeIndex) * 397 ^ this.LoopingEffectIndex;
	}

	// Token: 0x06001AC5 RID: 6853 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(CustomAssetbundleState left, CustomAssetbundleState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001AC6 RID: 6854 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(CustomAssetbundleState left, CustomAssetbundleState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x040010F9 RID: 4345
	[Tag(TagType.URL)]
	public string AssetbundleURL = "";

	// Token: 0x040010FA RID: 4346
	[Tag(TagType.URL)]
	public string AssetbundleSecondaryURL = "";

	// Token: 0x040010FB RID: 4347
	public int MaterialIndex;

	// Token: 0x040010FC RID: 4348
	public int TypeIndex;

	// Token: 0x040010FD RID: 4349
	public int LoopingEffectIndex;
}
