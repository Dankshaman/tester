using System;

// Token: 0x0200020C RID: 524
public class CustomDeckState : IEquatable<CustomDeckState>
{
	// Token: 0x06001AB6 RID: 6838 RVA: 0x000BA75C File Offset: 0x000B895C
	public bool Equals(CustomDeckState other)
	{
		return other != null && (this == other || (this.FaceURL == other.FaceURL && this.BackURL == other.BackURL && this.NumWidth == other.NumWidth && this.NumHeight == other.NumHeight && this.BackIsHidden == other.BackIsHidden && this.UniqueBack == other.UniqueBack && this.Type == other.Type));
	}

	// Token: 0x06001AB7 RID: 6839 RVA: 0x000BA7E2 File Offset: 0x000B89E2
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((CustomDeckState)obj)));
	}

	// Token: 0x06001AB8 RID: 6840 RVA: 0x000BA810 File Offset: 0x000B8A10
	public override int GetHashCode()
	{
		return (((((((this.FaceURL != null) ? this.FaceURL.GetHashCode() : 0) * 397 ^ ((this.BackURL != null) ? this.BackURL.GetHashCode() : 0)) * 397 ^ this.NumWidth) * 397 ^ this.NumHeight) * 397 ^ this.BackIsHidden.GetHashCode()) * 397 ^ this.UniqueBack.GetHashCode()) * 397 ^ (int)this.Type;
	}

	// Token: 0x06001AB9 RID: 6841 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(CustomDeckState left, CustomDeckState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001ABA RID: 6842 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(CustomDeckState left, CustomDeckState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x040010EA RID: 4330
	[Tag(TagType.URL)]
	public string FaceURL = "";

	// Token: 0x040010EB RID: 4331
	[Tag(TagType.URL)]
	public string BackURL = "";

	// Token: 0x040010EC RID: 4332
	public int NumWidth = 10;

	// Token: 0x040010ED RID: 4333
	public int NumHeight = 7;

	// Token: 0x040010EE RID: 4334
	public bool BackIsHidden;

	// Token: 0x040010EF RID: 4335
	public bool UniqueBack;

	// Token: 0x040010F0 RID: 4336
	public CardType Type;
}
