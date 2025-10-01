using System;

// Token: 0x02000205 RID: 517
public class CustomAssetState : IEquatable<CustomAssetState>
{
	// Token: 0x06001A81 RID: 6785 RVA: 0x000B9E14 File Offset: 0x000B8014
	public bool Equals(CustomAssetState other)
	{
		return other != null && (this == other || (this.Type == other.Type && this.Name == other.Name && this.URL == other.URL));
	}

	// Token: 0x06001A82 RID: 6786 RVA: 0x000B9E60 File Offset: 0x000B8060
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((CustomAssetState)obj)));
	}

	// Token: 0x06001A83 RID: 6787 RVA: 0x000B9E90 File Offset: 0x000B8090
	public override int GetHashCode()
	{
		return (int)((this.Type * (CustomAssetType)397 ^ (CustomAssetType)((this.Name != null) ? this.Name.GetHashCode() : 0)) * (CustomAssetType)397 ^ (CustomAssetType)((this.URL != null) ? this.URL.GetHashCode() : 0));
	}

	// Token: 0x06001A84 RID: 6788 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(CustomAssetState left, CustomAssetState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001A85 RID: 6789 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(CustomAssetState left, CustomAssetState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x040010CB RID: 4299
	public CustomAssetType Type;

	// Token: 0x040010CC RID: 4300
	public string Name;

	// Token: 0x040010CD RID: 4301
	[Tag(TagType.URL)]
	public string URL;
}
