using System;
using Newtonsoft.Json;

// Token: 0x020001F7 RID: 503
public class LightingState : IEquatable<LightingState>
{
	// Token: 0x06001A36 RID: 6710 RVA: 0x000B8D64 File Offset: 0x000B6F64
	[JsonConstructor]
	public LightingState()
	{
	}

	// Token: 0x06001A37 RID: 6711 RVA: 0x000B8E20 File Offset: 0x000B7020
	public LightingState(LightingState copyFrom, float? LightIntensity = null, ColourState? LightColor = null, float? AmbientIntensity = null, AmbientType? AmbientType = null, ColourState? AmbientSkyColor = null, ColourState? AmbientEquatorColor = null, ColourState? AmbientGroundColor = null, float? ReflectionIntensity = null, int? LutIndex = null, float? LutContribution = null, string LutURL = null)
	{
		this.LightIntensity = (LightIntensity ?? copyFrom.LightIntensity);
		this.LightColor = (LightColor ?? copyFrom.LightColor);
		this.AmbientIntensity = (AmbientIntensity ?? copyFrom.AmbientIntensity);
		this.AmbientType = (AmbientType ?? copyFrom.AmbientType);
		this.AmbientSkyColor = (AmbientSkyColor ?? copyFrom.AmbientSkyColor);
		this.AmbientEquatorColor = (AmbientEquatorColor ?? copyFrom.AmbientEquatorColor);
		this.AmbientGroundColor = (AmbientGroundColor ?? copyFrom.AmbientGroundColor);
		this.ReflectionIntensity = (ReflectionIntensity ?? copyFrom.ReflectionIntensity);
		this.LutIndex = (LutIndex ?? copyFrom.LutIndex);
		this.LutContribution = (LutContribution ?? copyFrom.LutContribution);
		this.LutURL = (LutURL ?? copyFrom.LutURL);
	}

	// Token: 0x06001A38 RID: 6712 RVA: 0x000B9038 File Offset: 0x000B7238
	public bool Equals(LightingState other)
	{
		return other != null && (this == other || (this.LightIntensity.Equals(other.LightIntensity) && this.LightColor.Equals(other.LightColor) && this.AmbientIntensity.Equals(other.AmbientIntensity) && this.AmbientType == other.AmbientType && this.AmbientSkyColor.Equals(other.AmbientSkyColor) && this.AmbientEquatorColor.Equals(other.AmbientEquatorColor) && this.AmbientGroundColor.Equals(other.AmbientGroundColor) && this.ReflectionIntensity.Equals(other.ReflectionIntensity) && this.LutIndex == other.LutIndex && this.LutContribution.Equals(other.LutContribution) && this.LutURL == other.LutURL));
	}

	// Token: 0x06001A39 RID: 6713 RVA: 0x000B9120 File Offset: 0x000B7320
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((LightingState)obj)));
	}

	// Token: 0x06001A3A RID: 6714 RVA: 0x000B9150 File Offset: 0x000B7350
	public override int GetHashCode()
	{
		return (((((((((this.LightIntensity.GetHashCode() * 397 ^ this.LightColor.GetHashCode()) * 397 ^ this.AmbientIntensity.GetHashCode()) * 397 ^ (int)this.AmbientType) * 397 ^ this.AmbientSkyColor.GetHashCode()) * 397 ^ this.AmbientEquatorColor.GetHashCode()) * 397 ^ this.AmbientGroundColor.GetHashCode()) * 397 ^ this.ReflectionIntensity.GetHashCode()) * 397 ^ this.LutIndex) * 397 ^ this.LutContribution.GetHashCode()) * 397 ^ ((this.LutURL != null) ? this.LutURL.GetHashCode() : 0);
	}

	// Token: 0x06001A3B RID: 6715 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(LightingState left, LightingState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001A3C RID: 6716 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(LightingState left, LightingState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x04001095 RID: 4245
	public float LightIntensity = 0.54f;

	// Token: 0x04001096 RID: 4246
	public ColourState LightColor = new ColourState(1f, 0.9804f, 0.8902f, 1f);

	// Token: 0x04001097 RID: 4247
	public float AmbientIntensity = 1.3f;

	// Token: 0x04001098 RID: 4248
	public AmbientType AmbientType;

	// Token: 0x04001099 RID: 4249
	public ColourState AmbientSkyColor = new ColourState(0.5f, 0.5f, 0.5f, 1f);

	// Token: 0x0400109A RID: 4250
	public ColourState AmbientEquatorColor = new ColourState(0.5f, 0.5f, 0.5f, 1f);

	// Token: 0x0400109B RID: 4251
	public ColourState AmbientGroundColor = new ColourState(0.5f, 0.5f, 0.5f, 1f);

	// Token: 0x0400109C RID: 4252
	public float ReflectionIntensity = 1f;

	// Token: 0x0400109D RID: 4253
	public int LutIndex;

	// Token: 0x0400109E RID: 4254
	public float LutContribution = 1f;

	// Token: 0x0400109F RID: 4255
	[Tag(TagType.URL)]
	public string LutURL;
}
