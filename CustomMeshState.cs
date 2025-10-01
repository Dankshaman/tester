using System;

// Token: 0x02000214 RID: 532
public class CustomMeshState : IEquatable<CustomMeshState>
{
	// Token: 0x06001AE6 RID: 6886 RVA: 0x000BAF6C File Offset: 0x000B916C
	public bool Equals(CustomMeshState other)
	{
		return other != null && (this == other || (this.MeshURL == other.MeshURL && this.DiffuseURL == other.DiffuseURL && this.NormalURL == other.NormalURL && this.ColliderURL == other.ColliderURL && this.Convex == other.Convex && this.MaterialIndex == other.MaterialIndex && this.TypeIndex == other.TypeIndex && object.Equals(this.CustomShader, other.CustomShader) && this.CastShadows == other.CastShadows));
	}

	// Token: 0x06001AE7 RID: 6887 RVA: 0x000BB020 File Offset: 0x000B9220
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((CustomMeshState)obj)));
	}

	// Token: 0x06001AE8 RID: 6888 RVA: 0x000BB050 File Offset: 0x000B9250
	public override int GetHashCode()
	{
		return (((((((((this.MeshURL != null) ? this.MeshURL.GetHashCode() : 0) * 397 ^ ((this.DiffuseURL != null) ? this.DiffuseURL.GetHashCode() : 0)) * 397 ^ ((this.NormalURL != null) ? this.NormalURL.GetHashCode() : 0)) * 397 ^ ((this.ColliderURL != null) ? this.ColliderURL.GetHashCode() : 0)) * 397 ^ this.Convex.GetHashCode()) * 397 ^ this.MaterialIndex) * 397 ^ this.TypeIndex) * 397 ^ ((this.CustomShader != null) ? this.CustomShader.GetHashCode() : 0)) * 397 ^ this.CastShadows.GetHashCode();
	}

	// Token: 0x06001AE9 RID: 6889 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(CustomMeshState left, CustomMeshState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001AEA RID: 6890 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(CustomMeshState left, CustomMeshState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x0400110D RID: 4365
	[Tag(TagType.URL)]
	public string MeshURL = "";

	// Token: 0x0400110E RID: 4366
	[Tag(TagType.URL)]
	public string DiffuseURL = "";

	// Token: 0x0400110F RID: 4367
	[Tag(TagType.URL)]
	public string NormalURL = "";

	// Token: 0x04001110 RID: 4368
	[Tag(TagType.URL)]
	public string ColliderURL = "";

	// Token: 0x04001111 RID: 4369
	public bool Convex = true;

	// Token: 0x04001112 RID: 4370
	public int MaterialIndex;

	// Token: 0x04001113 RID: 4371
	public int TypeIndex;

	// Token: 0x04001114 RID: 4372
	public CustomShaderState CustomShader;

	// Token: 0x04001115 RID: 4373
	public bool CastShadows = true;
}
