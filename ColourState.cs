using System;
using UnityEngine;

// Token: 0x02000208 RID: 520
public struct ColourState : IEquatable<ColourState>
{
	// Token: 0x06001A92 RID: 6802 RVA: 0x000BA210 File Offset: 0x000B8410
	public ColourState(float r, float g, float b, float a = 1f)
	{
		this.r = r;
		this.g = g;
		this.b = b;
		if (a < 1f)
		{
			this.a = new float?(a);
			return;
		}
		this.a = null;
	}

	// Token: 0x06001A93 RID: 6803 RVA: 0x000BA24C File Offset: 0x000B844C
	public ColourState(Colour c)
	{
		this.r = c.r;
		this.g = c.g;
		this.b = c.b;
		if (c.a < 1f)
		{
			this.a = new float?(c.a);
			return;
		}
		this.a = null;
	}

	// Token: 0x06001A94 RID: 6804 RVA: 0x000BA2A8 File Offset: 0x000B84A8
	public Colour ToColour()
	{
		return new Colour(this.r, this.g, this.b, this.a ?? 1f);
	}

	// Token: 0x06001A95 RID: 6805 RVA: 0x000BA2EA File Offset: 0x000B84EA
	public static implicit operator Colour(ColourState colorState)
	{
		return colorState.ToColour();
	}

	// Token: 0x06001A96 RID: 6806 RVA: 0x000BA2F3 File Offset: 0x000B84F3
	public static implicit operator Color(ColourState colorState)
	{
		return colorState.ToColour();
	}

	// Token: 0x06001A97 RID: 6807 RVA: 0x000BA301 File Offset: 0x000B8501
	public static implicit operator Color32(ColourState colorState)
	{
		return colorState.ToColour();
	}

	// Token: 0x06001A98 RID: 6808 RVA: 0x000BA30F File Offset: 0x000B850F
	public static implicit operator ColourState(Colour colour)
	{
		return new ColourState(colour);
	}

	// Token: 0x06001A99 RID: 6809 RVA: 0x000BA317 File Offset: 0x000B8517
	public static implicit operator ColourState(Color colour)
	{
		return new ColourState(colour);
	}

	// Token: 0x06001A9A RID: 6810 RVA: 0x000BA324 File Offset: 0x000B8524
	public static implicit operator ColourState(Color32 colour)
	{
		return new ColourState(colour);
	}

	// Token: 0x06001A9B RID: 6811 RVA: 0x000BA334 File Offset: 0x000B8534
	public bool Equals(ColourState other)
	{
		return this.r.Equals(other.r) && this.g.Equals(other.g) && this.b.Equals(other.b) && Nullable.Equals<float>(this.a, other.a);
	}

	// Token: 0x06001A9C RID: 6812 RVA: 0x000BA390 File Offset: 0x000B8590
	public override bool Equals(object obj)
	{
		if (obj is ColourState)
		{
			ColourState other = (ColourState)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x06001A9D RID: 6813 RVA: 0x000BA3B8 File Offset: 0x000B85B8
	public override int GetHashCode()
	{
		return ((this.r.GetHashCode() * 397 ^ this.g.GetHashCode()) * 397 ^ this.b.GetHashCode()) * 397 ^ this.a.GetHashCode();
	}

	// Token: 0x06001A9E RID: 6814 RVA: 0x000BA40C File Offset: 0x000B860C
	public static bool operator ==(ColourState left, ColourState right)
	{
		return left.Equals(right);
	}

	// Token: 0x06001A9F RID: 6815 RVA: 0x000BA416 File Offset: 0x000B8616
	public static bool operator !=(ColourState left, ColourState right)
	{
		return !left.Equals(right);
	}

	// Token: 0x040010DA RID: 4314
	public float r;

	// Token: 0x040010DB RID: 4315
	public float g;

	// Token: 0x040010DC RID: 4316
	public float b;

	// Token: 0x040010DD RID: 4317
	public float? a;
}
