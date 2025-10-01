using System;

// Token: 0x020001FF RID: 511
public class TextState : IEquatable<TextState>
{
	// Token: 0x06001A5C RID: 6748 RVA: 0x000B98C4 File Offset: 0x000B7AC4
	public bool Equals(TextState other)
	{
		return other != null && (this == other || (this.Text == other.Text && Nullable.Equals<ColourState>(this.colorstate, other.colorstate) && this.fontSize == other.fontSize));
	}

	// Token: 0x06001A5D RID: 6749 RVA: 0x000B9912 File Offset: 0x000B7B12
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((TextState)obj)));
	}

	// Token: 0x06001A5E RID: 6750 RVA: 0x000B9940 File Offset: 0x000B7B40
	public override int GetHashCode()
	{
		return (((this.Text != null) ? this.Text.GetHashCode() : 0) * 397 ^ this.colorstate.GetHashCode()) * 397 ^ this.fontSize;
	}

	// Token: 0x06001A5F RID: 6751 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(TextState left, TextState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001A60 RID: 6752 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(TextState left, TextState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x040010B9 RID: 4281
	public string Text;

	// Token: 0x040010BA RID: 4282
	public ColourState? colorstate;

	// Token: 0x040010BB RID: 4283
	public int fontSize = 64;
}
