using System;
using Newtonsoft.Json;
using UnityEngine;

// Token: 0x02000215 RID: 533
public class CustomShaderState : IEquatable<CustomShaderState>
{
	// Token: 0x06001AEC RID: 6892 RVA: 0x000BB17C File Offset: 0x000B937C
	[JsonConstructor]
	public CustomShaderState()
	{
	}

	// Token: 0x06001AED RID: 6893 RVA: 0x000BB1D0 File Offset: 0x000B93D0
	public CustomShaderState(ColourState specularColor, float specularIntensity, float specularSharpness, float fresnelStrength)
	{
		this.SpecularColor = specularColor;
		this.SpecularIntensity = specularIntensity;
		this.SpecularSharpness = specularSharpness;
		this.FresnelStrength = fresnelStrength;
	}

	// Token: 0x06001AEE RID: 6894 RVA: 0x000BB240 File Offset: 0x000B9440
	public CustomShaderState(Material material)
	{
		this.SpecularColor = new ColourState(material.GetColor("_SpecColor"));
		this.SpecularIntensity = material.GetFloat("_SpecInt");
		this.SpecularSharpness = material.GetFloat("_Shininess");
		this.FresnelStrength = material.GetFloat("_Fresnel");
	}

	// Token: 0x06001AEF RID: 6895 RVA: 0x000BB2E4 File Offset: 0x000B94E4
	public void AssignToMarmosetMaterial(Material material)
	{
		material.SetColor("_SpecColor", this.SpecularColor.ToColour());
		material.SetFloat("_SpecInt", this.SpecularIntensity);
		material.SetFloat("_Shininess", this.SpecularSharpness);
		material.SetFloat("_Fresnel", this.FresnelStrength);
	}

	// Token: 0x06001AF0 RID: 6896 RVA: 0x000BB340 File Offset: 0x000B9540
	public bool Equals(CustomShaderState other)
	{
		return other != null && (this == other || (this.SpecularColor.Equals(other.SpecularColor) && this.SpecularIntensity.Equals(other.SpecularIntensity) && this.SpecularSharpness.Equals(other.SpecularSharpness) && this.FresnelStrength.Equals(other.FresnelStrength)));
	}

	// Token: 0x06001AF1 RID: 6897 RVA: 0x000BB3A4 File Offset: 0x000B95A4
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((CustomShaderState)obj)));
	}

	// Token: 0x06001AF2 RID: 6898 RVA: 0x000BB3D4 File Offset: 0x000B95D4
	public override int GetHashCode()
	{
		return ((this.SpecularColor.GetHashCode() * 397 ^ this.SpecularIntensity.GetHashCode()) * 397 ^ this.SpecularSharpness.GetHashCode()) * 397 ^ this.FresnelStrength.GetHashCode();
	}

	// Token: 0x06001AF3 RID: 6899 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(CustomShaderState left, CustomShaderState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001AF4 RID: 6900 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(CustomShaderState left, CustomShaderState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x04001116 RID: 4374
	public ColourState SpecularColor = new ColourState(0.9f, 0.9f, 0.9f, 1f);

	// Token: 0x04001117 RID: 4375
	public float SpecularIntensity = 0.1f;

	// Token: 0x04001118 RID: 4376
	public float SpecularSharpness = 3f;

	// Token: 0x04001119 RID: 4377
	public float FresnelStrength = 0.1f;
}
