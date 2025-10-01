using System;

// Token: 0x02000203 RID: 515
public class CustomDecalState : IEquatable<CustomDecalState>
{
	// Token: 0x06001A75 RID: 6773 RVA: 0x000B9CB0 File Offset: 0x000B7EB0
	public bool Equals(CustomDecalState other)
	{
		return other != null && (this == other || (this.Name == other.Name && this.ImageURL == other.ImageURL && this.Size.Equals(other.Size)));
	}

	// Token: 0x06001A76 RID: 6774 RVA: 0x000B9D01 File Offset: 0x000B7F01
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((CustomDecalState)obj)));
	}

	// Token: 0x06001A77 RID: 6775 RVA: 0x000B9D30 File Offset: 0x000B7F30
	public override int GetHashCode()
	{
		return (((this.Name != null) ? this.Name.GetHashCode() : 0) * 397 ^ ((this.ImageURL != null) ? this.ImageURL.GetHashCode() : 0)) * 397 ^ this.Size.GetHashCode();
	}

	// Token: 0x06001A78 RID: 6776 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(CustomDecalState left, CustomDecalState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001A79 RID: 6777 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(CustomDecalState left, CustomDecalState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x040010C6 RID: 4294
	public string Name;

	// Token: 0x040010C7 RID: 4295
	[Tag(TagType.URL)]
	public string ImageURL;

	// Token: 0x040010C8 RID: 4296
	public float Size;
}
