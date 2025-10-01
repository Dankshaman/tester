using System;
using Newtonsoft.Json;

// Token: 0x02000200 RID: 512
public class TabState : IEquatable<TabState>
{
	// Token: 0x06001A62 RID: 6754 RVA: 0x000B998D File Offset: 0x000B7B8D
	[JsonConstructor]
	public TabState()
	{
	}

	// Token: 0x06001A63 RID: 6755 RVA: 0x000B999C File Offset: 0x000B7B9C
	public TabState(UITab to)
	{
		this.title = to.title;
		this.body = to.body;
		this.visibleColor = new ColourState(to.VisibleColor);
		this.color = Colour.LabelFromColour(to.VisibleColor);
		this.id = to.id;
	}

	// Token: 0x06001A64 RID: 6756 RVA: 0x000B9A08 File Offset: 0x000B7C08
	public bool Equals(TabState other)
	{
		return other != null && (this == other || (this.title == other.title && this.body == other.body && this.color == other.color && this.visibleColor.Equals(other.visibleColor) && this.id == other.id));
	}

	// Token: 0x06001A65 RID: 6757 RVA: 0x000B9A7C File Offset: 0x000B7C7C
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((TabState)obj)));
	}

	// Token: 0x06001A66 RID: 6758 RVA: 0x000B9AAC File Offset: 0x000B7CAC
	public override int GetHashCode()
	{
		return (((((this.title != null) ? this.title.GetHashCode() : 0) * 397 ^ ((this.body != null) ? this.body.GetHashCode() : 0)) * 397 ^ ((this.color != null) ? this.color.GetHashCode() : 0)) * 397 ^ this.visibleColor.GetHashCode()) * 397 ^ this.id;
	}

	// Token: 0x06001A67 RID: 6759 RVA: 0x000B7302 File Offset: 0x000B5502
	public static bool operator ==(TabState left, TabState right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06001A68 RID: 6760 RVA: 0x000B730B File Offset: 0x000B550B
	public static bool operator !=(TabState left, TabState right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x040010BC RID: 4284
	public string title;

	// Token: 0x040010BD RID: 4285
	public string body;

	// Token: 0x040010BE RID: 4286
	public string color;

	// Token: 0x040010BF RID: 4287
	public ColourState visibleColor;

	// Token: 0x040010C0 RID: 4288
	public int id = -1;
}
